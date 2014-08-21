using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public abstract class Pawns : ChessPiece
    {
        internal Pawns(int code) : base(code) { }

        public override int[] Moves(Situation situation, bool capture = false)
        {
            int pos = situation.Positions[this];
            int dest = 0;
            ChessPiece destPiece = null;
            List<int> moves = new List<int>();
            if (situation.InHomeHalf(this))//如果未过河
            {
                dest = pos + (this.Code & 16) == 0 ? -16 : 16;
                destPiece = situation.Pieces[dest];
                if (destPiece == null || destPiece.Side != Side)
                {
                    moves.Add(pos | (dest << 8));
                }
            }
            for (int n = 0 + ((this.Code & 16) >> 4); n < 3 + ((this.Code & 16) >> 4); n++)
            {
                dest = pos + setps[n];
                destPiece = situation.Pieces[dest];
                if (!situation.InFort(dest) ||
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

        private static readonly int[] setps = { -16, -1, 1, 16 };
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

            int setp = dest - situation.Positions[this];
            if (situation.InHomeHalf(this, dest))//未过河
            {
                return (this.Code & 16) == 0 ? setp == -16 : setp == 16;
            }
            for (int n = 0 + ((this.Code & 16) >> 4); n < 3 + ((this.Code & 16) >> 4); n++)
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
