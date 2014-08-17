using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public abstract class Knights : ChessPiece
    {
//        static const char ccKingDelta[4] = {-16, -1, 1, 16};
//// 仕(士)的步长
//static const char ccAdvisorDelta[4] = {-17, -15, 15, 17};
//// 马的步长，以帅(将)的步长作为马腿
 //       private static readonly int[,] setps = { { -33, -31 }, { -18, 14 }, { -14, 18 }, { 31, 33 } };
        private static readonly int[,] setps = new int[,] { { -33, -16 }, { -31, -16 }, { -18, -1 }, { 14, -1 }, { -14, 1 }, { 18, 1 }, { 31, 16 }, { 33, 16 } };
        internal Knights(int code)
            : base(code)
        {
            this.Chess = "马";
        }
        public override int[] Moves(Situation situation)
        {
            return null;
        }

        public override bool CanMove(Situation situation, int dest)
        {
            if (!situation.InChessboard(dest))//如果如果目标位置不在棋盘中
            {
                return false;
            }
            int setp = dest - situation.Positions[this];
            for (int n = 0; n < setps.GetLength(0); n++)
            {
                if (setps[n, 0] == setp && situation.Pieces[situation.Positions[this] + setps[n, 1]] == null)
                {
                    return true;
                }
            }
            return false;
        }
        //public override int[,] Setps
        //{
        //    get { return setps; }
        //}
    }
}
