using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using static DurakWinForms.GameWindow;

namespace DurakWinForms
{
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
}
