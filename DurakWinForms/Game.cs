using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using static DurakWinForms.GameWindow;

namespace DurakWinForms
{
    public class Game : MarshalByRefObject
    {
        public Dictionary<string, bool> Cards { get; set; }//Початкова колода карт

        public GameState CurrGameState { get; set; }//Стан гри

        public GameConfig CurrGameConfig { get; set; }//Параметри гри, налаштовуєтся при створені гри

        public event RenewGameHandler RenewGame;//Подія оновления стану гри

        public static int NumberOfInstance;

        public Game()
        {
            NumberOfInstance++;

            Cards = new Dictionary<string, bool>();
            CurrGameState = new GameState();
            CurrGameConfig = new GameConfig();

            //Конфігурування гри
            CurrGameConfig.SetCardsNumberIn1BoutRestriction(6);

            //Формування колоди
            Array suitValues = Enum.GetValues(typeof(Suits));
            Array rankValues = Enum.GetValues(typeof(Ranks));

            for (int s = 0; s < suitValues.Length; s++)
            {
                for (int r = 0; r < rankValues.Length; r++)
                {
                    Suits currSuit = (Suits)suitValues.GetValue(s);
                    Ranks currRank = (Ranks)rankValues.GetValue(r);

                    string cardName = Enum.GetName(typeof(Suits), currSuit) + "_" + Enum.GetName(typeof(Ranks), currRank);

                    Cards.Add(cardName, false);
                }
            }
        }

        public override object InitializeLifetimeService()
        {
            ILease il = (ILease)base.InitializeLifetimeService();
            il.InitialLeaseTime = TimeSpan.FromDays(1);
            il.RenewOnCallTime = TimeSpan.FromSeconds(10);
            return il;
        }

        public void Connect(Gamer p)
        {
            CurrGameState.Gamers.Add(p);
            ShowAllGamers(CurrGameState);
        }

        public void Disconnect(Gamer p)
        {
            CurrGameState.Gamers.Remove(p);
            ShowAllGamers(CurrGameState);
        }

        public void ShowAllGamers(GameState gameState)
        {
            if (RenewGame != null)
                RenewGame(gameState);
        }

        public void Deal()
        {
            if (CurrGameState.Deck.Count > 0)
            {
                CurrGameState.Deck.Clear();
            }

            string[] keyStrings = Cards.Keys.ToArray();
            foreach (string key in keyStrings)
            {
                Cards[key] = false;
            }

            Array suitValues = Enum.GetValues(typeof(Suits));
            Array rankValues = Enum.GetValues(typeof(Ranks));

            string cardName;
            Suits currSuit;
            Ranks currRank;

            for (int i = 0; i < Cards.Count; i++)
            {
                do
                {
                    currSuit =
                      (Suits)suitValues.GetValue(new Random((int)DateTime.Now.Ticks).Next(0, suitValues.Length));
                    currRank =
                      (Ranks)rankValues.GetValue(new Random((int)DateTime.Now.Ticks).Next(0, rankValues.Length));
                    cardName = Enum.GetName(typeof(Suits), currSuit) + "_" + Enum.GetName(typeof(Ranks), currRank);
                } while (Cards[cardName]);

                CurrGameState.Deck.Add(new Card(
                currSuit,
                currRank
                ));

                Cards[cardName] = true;
            }
        }

        public void Distribute(int gameState)
        {
            if (PlayerCount < 2) return;

            if (gameState == 0) //Начало гри
            {
                foreach (Gamer gamer in CurrGameState.Gamers)
                {
                    gamer.AlignmentClear();
                }

                //Козырь
                int trumpCardIndex = PlayerCount * 6 - 1;
                CurrGameState.TrumpSuit = CurrGameState.Deck[trumpCardIndex].Suit;
                Card trumpCard = CurrGameState.Deck[trumpCardIndex];
                CurrGameState.Deck.RemoveAt(trumpCardIndex);
                CurrGameState.Deck.Add(trumpCard); //Остання карта колоди - козирь, покласти її під колоду обличчям догори

                for (int i = 0; i < 6; i++)
                {
                    for (int k = 0; k < PlayerCount; k++)
                    {
                        CurrGameState.Gamers[k].AddCard(CurrGameState.Deck[0]);
                        CurrGameState.Deck.RemoveAt(0);
                    }
                }

                //Обираємо заходчика на початку
                if (CurrGameState.Attacker == null)
                {
                    Ranks min = Ranks.Ace;
                    foreach (Gamer gamer in CurrGameState.Gamers)
                    {
                        foreach (var card in gamer.Alignment)
                        {
                            if (card.Value.Suit == CurrGameState.TrumpSuit
                                && card.Value.Rank < min)
                            {
                                CurrGameState.Attacker = gamer;
                                int attackerIndex = CurrGameState.Gamers.IndexOf(gamer);
                                //Назначаємо відбиваючого
                                int defenderIndex = (attackerIndex + 1) % PlayerCount;

                                CurrGameState.Defender = CurrGameState.Gamers[defenderIndex];
                                min = card.Value.Rank;
                            }
                        }
                        if (min == Ranks.Six) break; //Прискорюємо
                    }
                    //Якщо немає козирів
                    if (CurrGameState.Attacker == null)
                    {
                        CurrGameState.Attacker = CurrGameState.Gamers[0];
                        CurrGameState.Defender = CurrGameState.Gamers[1];
                    }
                }
            }
            else
            {
                for (int k = 0; k < PlayerCount; k++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (CurrGameState.Deck.Count == 0)
                            break;
                        if (CurrGameState.Gamers[k].Alignment.Count >= 6)
                        {
                            continue;
                        }
                        CurrGameState.Gamers[k].AddCard(CurrGameState.Deck[0]);
                        CurrGameState.Deck.RemoveAt(0);
                    }
                    if (CurrGameState.Deck.Count == 0)
                        break;
                }
            }
        }

        public void Check()
        {
            int nGamersWithCards = 0;
            string loserName = String.Empty;
            foreach (Gamer gamer1 in CurrGameState.Gamers)
            {
                if (gamer1.Alignment.Count > 0)
                {
                    loserName = gamer1.Name;
                    nGamersWithCards++;
                }
            }
            if (nGamersWithCards <= 1)
            {
                if (nGamersWithCards == 1)
                {
                    CurrGameState.GameStateMessage =
                      loserName + @" - дурень. Нова гра почнется через 30 секунд. Якщо сервер не почне її раніше.";
                }
                else if (nGamersWithCards == 0)
                {
                    CurrGameState.GameStateMessage =
                      @"Нічія! Нова гра почнется через 30 секунд. Якщо сервер не почне її раніше.";
                }
                CurrGameState.GameRun = 2;
            }
        }

        public int PlayerCount
        {
            get
            {
                return CurrGameState.Gamers.Count;
            }
        }
    }
}
