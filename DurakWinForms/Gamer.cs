using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using static DurakWinForms.GameWindow;

namespace DurakWinForms
{
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
}
