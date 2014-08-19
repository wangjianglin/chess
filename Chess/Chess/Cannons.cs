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
            int pos = situation.Positions[this];
            if (pos == dest)
            {
                return false;
            }
            int delta = 0;
            if (situation.IsSameRank(pos, dest))
            {
                delta = pos < dest ? 1 : -1;
            }
            else if (situation.IsSameFile(pos, dest))
            {
                delta = pos < dest ? 16 : -16;
            }
            else
            {
                return false;
            }

            pos += delta;
            while (pos != dest && situation.Pieces[pos] == null) { pos += delta; }
            if (situation.Pieces[pos] == null)
            {
                return true;
            }
            if (situation.Pieces[dest] == null || situation.Pieces[dest].Side == this.Side)
            {
                return false;
            }
            pos += delta;
            while (pos != dest && situation.Pieces[pos] == null) { pos += delta; }
            return pos == dest;
        }
        //public override int[,] Setps
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}
