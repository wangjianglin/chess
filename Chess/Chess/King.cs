using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public abstract class King : ChessPiece
    {
        internal King(int code) : base(code) { }
        public override int[] Moves(Situation situation)
        {
            return null;
        }
        private static readonly int[] setps = {-16, -1, 1, 16};
        public override bool CanMove(Situation situation, int dest)
        {
            if (!situation.InChessboard(dest))//如果如果目标位置不在棋盘中
            {
                return false;
            }
            if (!situation.InFort(dest))//如果如果目标位置不在九宫格中
            {
                return false;
            }
            int setp = situation.Positions[this] - dest;
            for (int n = 0; n < setps.Length; n++)
            {
                if (setp == setps[n])
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
