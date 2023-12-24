using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoPlay.BattleShip.Domain
{
    public class Ship
    {
        public List<Cell> Decks { get; set; }

        public int Length { get; set; }

        public Ship(int length)
        {
            Decks = new List<Cell>();
            Length = length;
        }

        public void PutDeck(int y, int x)
        {
            Decks.Add(new Cell(y, x));
        }
    }
}
