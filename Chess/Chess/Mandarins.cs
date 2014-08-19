using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    /// <summary>
    /// 士
    /// </summary>
    public abstract class Mandarins : ChessPiece
    {
        internal Mandarins(int code) : base(code) { }

        public override int[] Moves(Situation situation)
        {
            return null;
        }
        private static readonly int[] setps = {-17, -15, 15, 17};
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
            if (situation.Pieces[dest] != null && situation.Pieces[dest].Side == this.Side)
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
