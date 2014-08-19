using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    /// <summary>
    /// 相
    /// </summary>
    public abstract class Elephants : ChessPiece
    {
        internal Elephants(int code) : base(code) { }

        public override int[] Moves(Situation situation)
        {
            return null;
        }
        private static readonly int[,] setps = new int[,] { { -34, -17 }, { 34, 17 }, { 30, 15 }, { -30, -15 } };
    
        public override bool CanMove(Situation situation, int dest)
        {
            if (!situation.InChessboard(dest))//如果如果目标位置不在棋盘中
            {
                return false;
            }
            if (!situation.InHomeHalf(this,dest))
            {
                return false;
            }
            if (situation.Pieces[dest] != null && situation.Pieces[dest].Side == this.Side)
            {
                return false;
            }
            int pos = situation.Positions[this];
            int setp = dest - pos;
            for (int n = 0; n < setps.GetLength(0); n++)
            {
                if (setps[n, 0] == setp && situation.Pieces[pos + setps[n, 1]] == null)
                {
                    return true;
                }
            }
            return false;
        }

        //public override int[,] Setps
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}
