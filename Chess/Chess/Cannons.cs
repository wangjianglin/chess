using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public abstract class Cannons : ChessPiece
    {
        internal Cannons(int code)
            : base(code)
        {
            this.Chess = "炮";
        }
        public override int[] Moves(Situation situation)
        {
            return null;
        }

        public override bool CanMove(Situation situation, int dest)
        {
            return true;
        }
        //public override int[,] Setps
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}
