using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Windows.Forms;

namespace DurakWinForms
{
  public partial class Form1 : Form
  {
    protected Game game = null;//Сама гра 
    protected Gamer gamer = null;//Ігрок клієнта

    private const int _cardWidth = 80;//Ширина карти
    private const int _cardHeight = 100;//Висота карти

    //Изображения мастей для отображения козыря
    private Bitmap[] _suitsImages = 
    {
      Pictures.Diamonds, 
      Pictures.Clubs, 
      Pictures.Hearts, 
      Pictures.Spides
    };

    private List<Bitmap> _backsImages = new List<Bitmap>();//Рубашки

    private Dictionary<string, Bitmap> _cardsImages = new Dictionary<string, Bitmap>();//Лиця карт

    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      //Загрузка рубашки
      _backsImages.Add(Pictures.back);
      DeckBack.Image = _backsImages[0];

      TrumpCard.BringToFront();
      DeckBack.BringToFront();

      //Загрузка карт
      Array suitValues = Enum.GetValues(typeof(Suits));
      Array rankValues = Enum.GetValues(typeof(Ranks));

      for (int s = 0; s < suitValues.Length; s++)
      {
        for (int r = 0; r < rankValues.Length; r++)
        {
          Suits currSuit = (Suits)suitValues.GetValue(s);
          Ranks currRank = (Ranks)rankValues.GetValue(r);

          string cardName = Enum.GetName(typeof(Suits), currSuit) + "_" + Enum.GetName(typeof(Ranks), currRank);

          _cardsImages.Add(cardName, (Bitmap)Pictures.ResourceManager.GetObject(cardName));
        }
      }

      SetStyle(ControlStyles.ResizeRedraw, true);
    }

    [Serializable]
    public delegate void RenewGameHandler(GameState gameState);

    TcpChannel CreateChannel(int port, string name)
    {
      BinaryServerFormatterSinkProvider sp = new
          BinaryServerFormatterSinkProvider();
      sp.TypeFilterLevel = TypeFilterLevel.Full; // Дозвіл на передачу делегатів

      BinaryClientFormatterSinkProvider cp = new
          BinaryClientFormatterSinkProvider();

      IDictionary props = new Hashtable();
      props["port"] = port;
      props["name"] = name;

      return new TcpChannel(props, cp, sp);
    }

    public enum Suits
    {
      Diamonds,
      Clubs,
      Hearts,
      Spades,
    }

    public enum Ranks
    {
      Six = 6,
      Seven,
      Eight,
      Nine,
      Ten,
      Jack,
      Queen,
      King,
      Ace
    }

    [Serializable]
    public struct Card
    {
      private Suits _suit;
      private Ranks _rank;
      private bool _visible;

      //public override int GetHashCode()
      //{
      //  return (int)_suit * 100 + (int)_rank;
      //}

      public override string ToString()
      {
        return Enum.GetName(typeof(Suits), _suit) + "_" + Enum.GetName(typeof(Ranks), _rank);
      }

      public Suits Suit
      {
        get { return _suit; }
        set { _suit = value; }
      } //масть

      public Ranks Rank
      {
        get { return _rank; }
        set { _rank = value; }
      } //Величина

      public bool Visible
      {
        get { return _visible; }
        set { _visible = value; }
      } //Вибір сторони карти

      public Card(Suits suit, Ranks rank, bool visible = false)
      {
        _suit = suit;
        _rank = rank;
        _visible = visible;
      }
    }

    [Serializable]
    public class GameState : MarshalByRefObject
    {
      public List<Gamer> Gamers = new List<Gamer>();//Гравці

      public Gamer Attacker { get; set; }//Чій ход

      public Gamer Defender { get; set; }//Хто відбиваєтся

      public List<Card> BoutCardsAttack = new List<Card>();//Карти заходу
      public List<Card> BoutCardsDefend = new List<Card>();//Карти захисту
      public List<Card> BoutCardsAttackDefended = new List<Card>();//Карти вибиті

      public Suits TrumpSuit { get; set; }//Козирь

      public List<Card> Deck = new List<Card>();//Колода гри

      public int GameRun { get; set; }//Нова гра = 0, роздача = 1, хід гри = 2 

      public string GameStateMessage { get; set; }

      public override object InitializeLifetimeService()
      {
        ILease il = (ILease)base.InitializeLifetimeService();
        il.InitialLeaseTime = TimeSpan.FromDays(1);
        il.RenewOnCallTime = TimeSpan.FromSeconds(10);
        return il;
      }
      
      public Gamer GetDefender()
      {
        return Defender;
      }

      public void AddAttackCardToGameField(Card card)
      {
        BoutCardsAttack.Add(card);
      }

      public void RemoveAttackCardToGameField(Card card)
      {
        BoutCardsAttack.Remove(card);
      }

      public void InsertAttackCardToGameField(int i, Card card)
      {
        BoutCardsAttack.Insert(i, card);
      }

      public void AddDefendCardToGameField(Card card)
      {
        BoutCardsDefend.Add(card);
      }

      public void AddAttackDefendedCardToGameField(Card card)
      {
          BoutCardsAttackDefended.Add(card);
      }

      public int GetCountCardsOnGameField()
      {
        return BoutCardsAttack.Count + BoutCardsDefend.Count + BoutCardsAttackDefended.Count;
      }

      public int GetCountAttackCardsOnGameField()
      {
        return BoutCardsAttack.Count;
      }

      public int GetCountAttackDefendedCardsOnGameField()
      {
        return BoutCardsAttackDefended.Count;
      }

      public int GetCountDefendCardsOnGameField()
      {
        return BoutCardsDefend.Count;
      }

      public List<Card> GetAllCardsOnGameField()
      {
        List<Card> allCards = new List<Card>();
        allCards.AddRange(BoutCardsAttack);
        allCards.AddRange(BoutCardsDefend);
        allCards.AddRange(BoutCardsAttackDefended);
        return allCards;
      }

      public void BoutCardsClear()
      {
        BoutCardsAttack.Clear();
        BoutCardsDefend.Clear();
        BoutCardsAttackDefended.Clear();
      }
    }

    public class Gamer : MarshalByRefObject
    {
      protected Game Game { get; set; }
      public string Name { get; set; }
      public SortedDictionary<string, Card> Alignment { get; set; }
      
      public Gamer(Game game, string name = "Gamer")
      {
        Game = game;
        Name = name;
        Alignment = new SortedDictionary<string, Card>();
      }

      public override object InitializeLifetimeService()
      {
        ILease il = (ILease)base.InitializeLifetimeService();
        il.InitialLeaseTime = TimeSpan.FromDays(1);
        il.RenewOnCallTime = TimeSpan.FromSeconds(10);
        return il;
      }
      
      public void AddCard(Card card)
      {
        Alignment.Add(card.ToString(), card);
      }

      public void RemoveCard(Card card)
      {
        Alignment.Remove(card.ToString());
      }

      public void AlignmentClear()
      {
        Alignment.Clear();
      }
    }

    [Serializable]
    public struct GameConfig
    {
      public int CardsNumberIn1BoutRestriction { get; set; }//Обмеження на кількість карт у крузі


      internal void SetCardsNumberIn1BoutRestriction(int p)
      {
        CardsNumberIn1BoutRestriction = p;
      }
    }

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

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (game != null)
      {
        // Підключення до гри
        if (!RemotingServices.IsTransparentProxy(game)) // На сервері
          if (game.PlayerCount > 1)
          {
            // Захист від дурня
            MessageBox.Show(@"Ви не можете відключитися, коли підключені користувачі!");
            e.Cancel = true;
            return;
          }
        game.RenewGame -= OnRenewGame; 
        // Виходимо з гри
        game.Disconnect(gamer);
      }
    }

    public void OnRenewGame(GameState gameState)
    {
      for (int i = 1; i < gameState.Gamers.Count; i++)
      {
        BeginInvoke(new Action<int>(num =>
        {
          (Controls["Gamer" + num + "Zone"] as GroupBox).Controls.Clear();
        }), i);
      }
      BeginInvoke(new Action(() =>
      {
        IamZone.Controls.Clear();
        GameField.Controls.Clear();
      }));

      if (gameState.Gamers.Count > 1)
      {
        int i = 1;
        foreach (Gamer currGamer in gameState.Gamers)
        {
          if (currGamer.Name == gamer.Name)//Оформляємо зону гравця
          {
            //Надписи імені та статусу гравця
            if (gameState.Attacker != null && currGamer.Name == gameState.Attacker.Name)
            {
              BeginInvoke(new Action<string>(n =>
              {
                IamZone.Text = n + @" - Ходіть";
                gameStateTb.Text = @"Ходіть";
              }), gamer.Name);

            }
            else if (gameState.Defender != null && currGamer.Name == gameState.Defender.Name)
            {
              BeginInvoke(new Action<string>(n =>
              {
                IamZone.Text = n + @" - Відбивайтесь";
                gameStateTb.Text = @"Відбивайтесь";
              }), gamer.Name);
            }
            else
            {
              BeginInvoke(new Action<string>(n =>
              {
                IamZone.Text = n;
                gameStateTb.Text = @"Можете підкидувати";
              }), gamer.Name);
            }

            //Розкладаємо карти у зоні гравця
            if (currGamer.Alignment.Count > 0)
            {
              IAsyncResult iar = BeginInvoke(new Func<int>(() => IamZone.Width));

              int iamZoneWidth;

              if (iar.IsCompleted)
              {
                iamZoneWidth = (int)EndInvoke(iar);
              }
              else
              {
                iamZoneWidth = IamZone.Width;
              }

              int cardLeft = 10;
              int step = (iamZoneWidth - _cardWidth) / currGamer.Alignment.Count;
              foreach (var card in currGamer.Alignment) //Розкладаємо карти у зоні гравця на формі
                            {
                PictureBox imageCard = new PictureBox();
                imageCard.Image = _cardsImages[card.Key];
                imageCard.Height = _cardHeight;
                imageCard.Width = _cardWidth;
                imageCard.SizeMode = PictureBoxSizeMode.StretchImage;
                imageCard.Location = new Point(cardLeft, 20);
                imageCard.Tag = card.Value.Suit + "_" + card.Value.Rank;
                imageCard.MouseDown += imageCard_MouseDown;
                cardLeft += step;
                BeginInvoke(new Action<PictureBox>(img => IamZone.Controls.Add(img)), imageCard);
              }
            }
          }
          else
          {
            if (i != 3)//Оформляємо зони гравців, крім 3 (вони збоку)
            {
              if (gameState.Attacker != null && currGamer.Name == gameState.Attacker.Name)
              {
                BeginInvoke(new Action<int, string>((num, name) =>
                {
                  (Controls["Gamer" + num + "Zone"] as GroupBox).Text = name + @" - Ходить";
                }), i, currGamer.Name);
              }
              else if (gameState.Defender != null && currGamer.Name == gameState.Defender.Name)
              {
                BeginInvoke(new Action<int, string>((num, name) =>
                {
                  (Controls["Gamer" + num + "Zone"] as GroupBox).Text = name + @" - Відбиваєтся";
                }), i, currGamer.Name); 
              }
              else
              {
                BeginInvoke(new Action<int, string>((num, name) =>
                {
                  (Controls["Gamer" + num + "Zone"] as GroupBox).Text = name;
                }), i, currGamer.Name);
              }

              if (currGamer.Alignment.Count > 0)
              {
                IAsyncResult iar = BeginInvoke(new Func<int, int>(
                    num => (Controls["Gamer" + num + "Zone"] as GroupBox).Height), 
                  i);

                int gbHeight;
                if (iar.IsCompleted)
                {
                  gbHeight = (int)EndInvoke(iar);
                }
                else
                {
                  gbHeight = (Controls["Gamer" + i + "Zone"] as GroupBox).Height;
                }

                int cardTop = 20;
                int step = (gbHeight - _cardHeight) / currGamer.Alignment.Count;
                foreach (var card in currGamer.Alignment) 
                {
                  PictureBox imageCard = new PictureBox();
                  imageCard.Image = _backsImages[0];
                  imageCard.Height = _cardHeight;
                  imageCard.Width = _cardWidth;
                  imageCard.Location = new Point(20, cardTop);
                  imageCard.SizeMode = PictureBoxSizeMode.StretchImage;
                  imageCard.Tag = card.ToString();
                  cardTop += step;
                  BeginInvoke(
                    new Action<int, PictureBox>((num, picb) =>
                    {
                      (Controls["Gamer" + num + "Zone"] as GroupBox).Controls.Add(picb);
                    }), i, imageCard);
              }
              }
            }
            else
            {//Оформляємо зону 3 гравця (вона зверху)
              string gbName = "Gamer" + i + "Zone";

              if (gameState.Attacker != null && currGamer.Name == gameState.Attacker.Name)
              {
                BeginInvoke(new Action<string, string>((name, gbname) =>
                {
                  (Controls[gbname] as GroupBox).Text = name + @" - Ходит";
                }), currGamer.Name, gbName); 
              }
              else if (gameState.Defender != null && currGamer.Name == gameState.Defender.Name)
              {
                BeginInvoke(new Action<string, string>((name, gbname) =>
                {
                  (Controls[gbname] as GroupBox).Text = name + @" - Отбивается";
                }), currGamer.Name, gbName); 
              }
              else
              {
                BeginInvoke(new Action<string, string>((name, gbname) =>
                {
                  (Controls[gbname] as GroupBox).Text = name;
                }), currGamer.Name, gbName);
              }

              if (currGamer.Alignment.Count > 0)
              {
                IAsyncResult iarWidth =
                  BeginInvoke(new Func<string, int>(gbname => (Controls[gbname] as GroupBox).Width), gbName);

                int gbWidth;
                if (iarWidth.IsCompleted)
                {
                  gbWidth = (int) EndInvoke(iarWidth);
                }
                else
                {
                  gbWidth = (Controls[gbName] as GroupBox).Width;
                }

                int cardLeft = 10;
                int step = (gbWidth - _cardWidth) / currGamer.Alignment.Count;
                foreach (var card in currGamer.Alignment) //Розкладаємо карти у зоні гравця на формі
                                {
                  PictureBox imageCard = new PictureBox();
                  imageCard.Image = _backsImages[0];
                  imageCard.Height = _cardHeight;
                  imageCard.Width = _cardWidth;
                  imageCard.Location = new Point(cardLeft, 20);
                  imageCard.SizeMode = PictureBoxSizeMode.StretchImage;
                  imageCard.Tag = card.Value.Suit + "_" + card.Value.Rank;
                  cardLeft += step;
                  BeginInvoke(
                    new Action<int, PictureBox>((num, picb) =>
                    {
                      (Controls["Gamer" + num + "Zone"] as GroupBox).Controls.Add(picb);
                    }), i, imageCard);
              }
              }
            }
            i++;
          }
        }

        //Показуємо колоду та козирь
        if (gameState.GameRun == 0)
        {
          if (gameState.Deck.Count != 0)
          {
            BeginInvoke(new Action<Bitmap, Bitmap>((pic, trump) =>
            {
              TrumpCard.Visible = true;
              DeckBack.Visible = true;
              TrumpCard.Image = pic;
              TrumpImage.Image = trump;
              TrumpImage.SendToBack();
            }),
              _cardsImages[gameState.Deck[gameState.Deck.Count - 1].ToString()],
              _suitsImages[(int) gameState.TrumpSuit]);
          }
          else
          {
            BeginInvoke(new Action<Bitmap>(trump =>
            {
              TrumpCard.Visible = false;
              DeckBack.Visible = false;
              TrumpImage.Image = trump;
              TrumpImage.BringToFront();
            }),
            _suitsImages[(int)gameState.TrumpSuit]);
          }
        }

        //Ховаємо колоду, коли залишаєтся лише козирь
        if (gameState.Deck.Count == 1)
        {
          if (DeckZone.Controls.Count > 1)
          {
            BeginInvoke(new Action(() => DeckBack.Visible = false));
          }
        }

        //Скрываем колоду и козырь, когда в колоде не осталось карт
        if (gameState.GameRun == 1 && gameState.Deck.Count == 0)
        {
          if (DeckZone.Controls.Count > 1)
          {
            BeginInvoke(new Action(() => TrumpCard.Visible = false));
            BeginInvoke(new Action(() => DeckBack.Visible = false));
          }
        }

        //Розкладаємо карти для атаки
        //Биті карти
        int offset = 0;
        foreach (Card card in gameState.BoutCardsAttackDefended)
        {
          PictureBox imageStepCard = new PictureBox();
          imageStepCard.Image = _cardsImages[card.ToString()];
          imageStepCard.Height = _cardHeight;
          imageStepCard.Width = _cardWidth;

          if (offset <= 4)
          {
            imageStepCard.Location = new Point((offset * (_cardWidth + 10) + 10), 20);
          }
          else
          {
            imageStepCard.Location = new Point(((offset - 5) * (_cardWidth + 10) + 10), _cardHeight + 40);
          }

          imageStepCard.SizeMode = PictureBoxSizeMode.StretchImage;
          BeginInvoke(new Action<PictureBox>(pic => GameField.Controls.Add(pic)), imageStepCard);
          offset++;
        }

        //Не биті карти
        foreach (Card card in gameState.BoutCardsAttack)
        {
          PictureBox imageStepCard = new PictureBox();
          imageStepCard.Image = _cardsImages[card.ToString()];
          imageStepCard.Height = _cardHeight;
          imageStepCard.Width = _cardWidth;

          if (offset <= 4)
          {
            imageStepCard.Location = new Point((offset * (_cardWidth + 10) + 10), 20);
          }
          else
          {
            imageStepCard.Location = new Point(((offset - 5) * (_cardWidth + 10) + 10), _cardHeight + 40);
          }
          
          imageStepCard.SizeMode = PictureBoxSizeMode.StretchImage;
          BeginInvoke(new Action<PictureBox>(pic => GameField.Controls.Add(pic)), imageStepCard);
          offset++;
        }

        //Разкладаємо карти захисту
        offset = 0;
        foreach (Card card in gameState.BoutCardsDefend)
        {
          PictureBox imageStepCard = new PictureBox();
          imageStepCard.Image = _cardsImages[card.ToString()];
          imageStepCard.Height = _cardHeight;
          imageStepCard.Width = _cardWidth;

          if (offset <= 4)
          {
            imageStepCard.Location = new Point(offset * (_cardWidth + 10) + 20, 40);
          }
          else
          {
            imageStepCard.Location =new Point((offset - 5) * (_cardWidth + 10) + 20, _cardHeight + 60);
          }


          imageStepCard.SizeMode = PictureBoxSizeMode.StretchImage;
          BeginInvoke(new Action<PictureBox>(pic =>
          {
            GameField.Controls.Add(pic);
            GameField.Controls[GameField.Controls.Count - 1].BringToFront();
          }), imageStepCard);
          offset++;
        }
      }

      //Регулюємо кнопки "Беру" та "Відбій"
      if (gameState.Attacker != null)
      {
        if (gameState.GetCountCardsOnGameField() != 0
          && gameState.GetCountAttackCardsOnGameField() == 0)
        {
          if (gamer.Name == gameState.Attacker.Name)
          {
            BeginInvoke(new Action(() =>
            {
              takeCardsBtn.Enabled = false;
              endRoundBtn.Enabled = true;
            }));
          }
          if (gamer.Name == gameState.Defender.Name)
          {
            BeginInvoke(new Action(() =>
            {
              takeCardsBtn.Enabled = true;
              endRoundBtn.Enabled = false;
            }));
          }
        }
        else if (gameState.GetCountCardsOnGameField() != 0
          && gameState.GetCountAttackCardsOnGameField() != 0)
        {
          if (gamer.Name == gameState.Attacker.Name)
          {
            BeginInvoke(new Action(() =>
            {
              takeCardsBtn.Enabled = false;
              endRoundBtn.Enabled = false;
            }));
          }
          if (gamer.Name == gameState.Defender.Name)
          {
            BeginInvoke(new Action(() =>
            {
              takeCardsBtn.Enabled = true;
              endRoundBtn.Enabled = false;
            }));
          }
        }
        else
        {
          BeginInvoke(new Action(() =>
          {
            takeCardsBtn.Enabled = false;
            endRoundBtn.Enabled = false;
          }));
        }
      }
      
      if (game.CurrGameState.GameRun == 2)
      {
        BeginInvoke(new Action<string>(n => gameStateTb.Text = n), game.CurrGameState.GameStateMessage);

        if (!RemotingServices.IsTransparentProxy(game)) // Ми на сервері
        {
          new Thread(() =>
          {
            Thread.Sleep(30000);
            if (game.CurrGameState.GameRun == 2)
            {
              NewGame();
            }
          }).Start();
        }
      }
    }

    void imageCard_MouseDown(object sender, MouseEventArgs e)
    {
      PictureBox imageCard = (sender as PictureBox);

      string suitNRank = imageCard.Tag.ToString();

      Suits cardSuit;
      Enum.TryParse(suitNRank.Substring(0, suitNRank.IndexOf('_')), false, out cardSuit);

      Ranks rankCard;
      Enum.TryParse(suitNRank.Substring(suitNRank.IndexOf('_') + 1), false, out rankCard);

      if (gamer.Name == game.CurrGameState.Defender.Name)
      {
        for (int i = 0; i < game.CurrGameState.BoutCardsAttack.Count; i++)
        {
          Suits curSuits = game.CurrGameState.BoutCardsAttack[i].Suit;
          Ranks curRanks = game.CurrGameState.BoutCardsAttack[i].Rank;

          if (((cardSuit == curSuits && rankCard > curRanks)
            || cardSuit == game.CurrGameState.TrumpSuit && curSuits!=game.CurrGameState.TrumpSuit))
          {
            Card stepCard = gamer.Alignment[suitNRank];
            gamer.RemoveCard(stepCard);
            game.CurrGameState.AddDefendCardToGameField(stepCard);

            Card cardAttack = game.CurrGameState.BoutCardsAttack[i];
            game.CurrGameState.RemoveAttackCardToGameField(cardAttack);
            game.CurrGameState.AddAttackDefendedCardToGameField(cardAttack);
            break;
          }
        }

        if (game.CurrGameState.Defender.Alignment.Count == 0)
        {
          game.Check();//Отримуємо результати гри
          NewRound();
          return;
        }
      }
      else if (gamer.Name != game.CurrGameState.Defender.Name)
      {
        if (game.CurrGameState.Defender.Alignment.Count == game.CurrGameState.GetCountAttackCardsOnGameField())
          return;
        bool rightAddCard = false;

        if (game.CurrGameState.GetCountCardsOnGameField() == 0)
        {
          rightAddCard = true;
        }
        else
        {
          foreach (Card card in game.CurrGameState.GetAllCardsOnGameField())
          {
            if (card.Rank == rankCard)
            {
              rightAddCard = true;
              break;
            }
          }
        }

        if (rightAddCard)
        {
          Card stepCard = gamer.Alignment[suitNRank];
          gamer.RemoveCard(stepCard);
          game.CurrGameState.AddAttackCardToGameField(stepCard);
        }
      }

      game.Check();//Отримуємо результати гри

      game.ShowAllGamers(game.CurrGameState);
    }

    private void startServerToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
      CreateServer dlg = new CreateServer();
      dlg.serverNameTb.Text = Dns.GetHostName();
      DialogResult dr = dlg.ShowDialog();
      if (dr != DialogResult.OK)
        return; // Відміна підключення
      string nick = dlg.NickTb.Text;

      //Шукаємо вільний канал
      int channelPort = 8001;
        bool IsChannelRegistered = true;
        while (IsChannelRegistered)
        {
          try
          {
            // Створюємо канал, який буде слухати порт
            ChannelServices.RegisterChannel(CreateChannel(channelPort, "tcpDurak" + channelPort), false);
            IsChannelRegistered = false;
          }
          catch
          {
            channelPort++;
          }
        }

      // Створюємо обєкт-гру
      game = new Game();      

      // Надаємо обєкт-гру для визову з інших компютурів
      RemotingServices.Marshal(game, "GameObject");

      // Заходимо в гру
      game.RenewGame += OnRenewGame;
      gamer = new Gamer(game, nick);

      game.Connect(gamer);

      startServerToolStripMenuItem.Enabled = false;
      connectToServerToolStripMenuItem.Enabled = false;

      newGameToolStripMenuItem.Enabled = true;//Створює гру лише сервер
    }

    private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ConnectToServer();
    }

    void ConnectToServer()
    {
      InputBox dlg = new InputBox();
      dlg.HostNameTb.Text = Dns.GetHostName();
      if (dlg.ShowDialog() != DialogResult.OK)
        return; // Відміна підключения
      string nick = dlg.NickTb.Text;
      string serverName = dlg.HostNameTb.Text;

      // Створюємо канал, який буде підключений до сервера
      TcpChannel tcpChannel = CreateChannel(0, "tcpDurak");
      ChannelServices.RegisterChannel(tcpChannel, false);

      // Отримуємо силку на обєкт-гру. розміщену на
      // другому компютері
      game = (Game)Activator.GetObject(typeof(Game),
            String.Format("tcp://{0}:8001/GameObject", serverName));

      if (game.PlayerCount == 6)
      {
        MessageBox.Show(@"Вибачте, немає вільних місць!");
        return;
      }
      // Заходимо в гру
      game.RenewGame += OnRenewGame;
      gamer = new Gamer(game, nick);
      game.Connect(gamer);

      startServerToolStripMenuItem.Enabled = false;
      connectToServerToolStripMenuItem.Enabled = false;
    }

    private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
    {
      NewGame();
    }

    void NewGame()
    {
      game.CurrGameState.Attacker = null;
      game.CurrGameState.Defender = null;
      game.CurrGameState.BoutCardsClear();
      game.Deal();
      game.Distribute(0);

      game.CurrGameState.GameRun = 0;
      game.ShowAllGamers(game.CurrGameState);
      game.CurrGameState.GameRun = 1;
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void endRoundBtn_Click(object sender, EventArgs e)
    {
      NewRound();
    }

    void NewRound()
    {
      game.CurrGameState.BoutCardsClear();
      game.Distribute(1);

      //Назначаємо атакуючого
      int index = (game.CurrGameState.Gamers.IndexOf(game.CurrGameState.Defender) - 1) % game.PlayerCount;
      do
      {
        index = (index + 1) % game.PlayerCount;
      } while (game.CurrGameState.Gamers[index].Alignment.Count == 0);
      game.CurrGameState.Attacker = game.CurrGameState.Gamers[index];

      //Назначаємо відбиваючого
      index = game.CurrGameState.Gamers.IndexOf(game.CurrGameState.Attacker);
      do
      {
        index = (index + 1) % game.PlayerCount;
      } while (game.CurrGameState.Gamers[index].Alignment.Count == 0);
      game.CurrGameState.Defender = game.CurrGameState.Gamers[index];

      game.Check();//Отримуємо результати гри

      game.ShowAllGamers(game.CurrGameState);
    }

    private void takeCardsBtn_Click(object sender, EventArgs e)
    {
      foreach (Card card in game.CurrGameState.GetAllCardsOnGameField())
      {
        gamer.AddCard(card);
      }

      game.CurrGameState.BoutCardsClear();

      game.Distribute(1);

      //Назначаємо атакуючого
      int index = game.CurrGameState.Gamers.IndexOf(game.CurrGameState.Defender);
      do
      {
        index = (index + 1) % game.PlayerCount;
      } while (game.CurrGameState.Gamers[index].Alignment.Count == 0);
      game.CurrGameState.Attacker = game.CurrGameState.Gamers[index];

      //Назначаємо відбиваючого
      do
      {
        index = (index + 1) % game.PlayerCount;
      } while (game.CurrGameState.Gamers[index].Alignment.Count == 0);
      game.CurrGameState.Defender = game.CurrGameState.Gamers[index];

      game.Check();//Отримуємо результати гри

      game.ShowAllGamers(game.CurrGameState);
    }
  }
}
