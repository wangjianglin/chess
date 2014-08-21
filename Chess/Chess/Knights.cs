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
        public override int[] Moves(Situation situation, bool capture = false)
        {
            int pos = situation.Positions[this];
            int dest = 0;
            ChessPiece destPiece = null;
            List<int> moves = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                dest = pos + setps[i, 0];
                destPiece = situation.Pieces[dest];
                if (!situation.InChessboard(dest) ||
                    situation.Pieces[pos + setps[i, 1]] != null ||
                    (destPiece != null && destPiece.Side == this.Side))
                {
                    continue;
                }
                //pcDst = ucpcSquares[sqDst];
                if (capture && destPiece == null)
                {
                    continue;
                }
                moves.Add(pos | (dest << 8) | (this.Code << 16));
            }
            return moves.ToArray();
        }

        public override bool CanMove(Situation situation, int dest)
        {
            if (!situation.InChessboard(dest))//如果如果目标位置不在棋盘中
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
        //    get { return setps; }
        //}
    }
}
