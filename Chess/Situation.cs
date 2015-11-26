using Lin.Core;
using Lin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lin.Chess
{
    /// <summary>
    /// 存储当前的局面信息
    /// 并记录走棋的历史信息
    /// 可以由局面生成棋谱文件
    /// 可以由棋说文件构建局面
    /// </summary>
    public class Situation : Model
    {

        private ChessPiece[] pieces = new ChessPiece[256];//记录棋盘中的棋子
        //private ChessPiece[] removes = new ChessPiece[32];//被移出的棋子
        private ChessPiece[] codes = new ChessPiece[32];//
        private int[] positions = new int[32];//记录棋子位置
        private List<int> moves = null;//记录走棋信息
        public ChessSide Side { get; private set; }

        public ReadOnlyIndexProperty<int, ChessPiece> Pieces { get; private set; }
        public ReadOnlyIndexProperty<int, ChessPiece> Codes { get; private set; }

        public Lin.Util.ReadOnlyIndexProperty<ChessPiece, int> Positions { get; private set; }

        public Situation(ChessSide side = ChessSide.Red)
        {
            Side = side;
            ChessPiece piece = null;
            for (int n = 0; n < 32; n++)
            {
                piece = BuildChessPices(n);
                pieces[POSITIONS[n]] = piece;
                codes[n] = piece;
                positions[n] = POSITIONS[n];
            }
            init();
        }
        private void init()
        {
            this.Pieces = new ReadOnlyIndexProperty<int, ChessPiece>(pos => { return pieces[pos]; }, () => { return pieces.Length; });
            this.Codes = new ReadOnlyIndexProperty<int, ChessPiece>(pos => { return codes[pos]; }, () => { return pieces.Length; });
            this.Positions = new ReadOnlyIndexProperty<ChessPiece, int>(piece => { return positions[piece.Code]; }, () => { return positions.Length; });
        }

        private static void CopyProperty(Situation source, Situation dest)
        {
            for (int n = 0; n < 32; n++)
            {
                dest.positions[n] = source.positions[n];
                dest.codes[n] = source.codes[n];
                //this.removes[n] = situation.removes[n];
            }
            for (int n = 0; n < 256; n++)
            {
                dest.pieces[n] = source.pieces[n];
            }
        }
        private Situation(Situation situation)
        {
            //for (int n = 0; n < 32; n++)
            //{
            //    this.positions[n] = situation.positions[n];
            //    this.codes[n] = situation.codes[n];
            //    //this.removes[n] = situation.removes[n];
            //}
            //for (int n = 0; n < 256; n++)
            //{
            //    this.pieces[n] = situation.pieces[n];
            //}
            CopyProperty(situation, this);
            init();
        }

        /// <summary>
        /// 判断棋子是否在棋盘中
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public bool InChessboard(ChessPiece piece)
        {
            return inBoard[positions[piece.Code]];
        }

        /// <summary>
        /// 判断指定位置是否在棋盘中
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool InChessboard(int position)
        {
            return inBoard[position];
        }

        public void SwitchPlayer()
        {
            if (Side == ChessSide.Red)
            {
                Side = ChessSide.Black;
            }
            else
            {
                Side = ChessSide.Red;
            }
        }
        /// <summary>
        /// 表示当前棋子是否未过河
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>未过河返回true，已经过河返回fase</returns>
        public bool InHomeHalf(ChessPiece piece)
        {
            return (piece.Code & 16) != ((positions[piece.Code] & 128) >> 3);
        }

        /// <summary>
        /// 表示当前棋子是否已经过河
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>已经过河返回true，未过河返回fase</returns>
        public bool InAwayHalf(ChessPiece piece)
        {
            return (piece.Code & 16) != ((positions[piece.Code] & 128) >> 3);
        }

        /// <summary>
        /// 表示指定位置相对piece方来说是否已经过河
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>已经过河返回true，未过河返回fase</returns>
        public bool InHomeHalf(ChessPiece piece, int dest)
        {
            return (piece.Code & 16) != ((dest & 128) >> 3);
        }

        /// <summary>
        /// 表示指定位置相对piece方来说是否未过河
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>未过河返回true，已经过河返回fase</returns>
        public bool InAwayHalf(ChessPiece piece, int dest)
        {
            return (piece.Code & 16) != ((dest & 128) >> 3);
        }

        //AWAY_HALF

        /// <summary>
        /// 判断指定位置是否在九宫格中
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public bool InFort(ChessPiece piece)
        {
            return inFort[positions[piece.Code]];
        }
        /// <summary>
        /// 判断棋子是否在九宫格中
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool InFort(int position)
        {
            return inFort[position];
        }


        public bool CanBack { get { return true; } }

        //悔棋
        public void Back()
        {

        }

        public bool CanForward { get { return true; } }
        public void Forward()
        {

        }
        public void Move(ChessPiece piece, int position)
        {
            if (this.pieces[position] != null)
            {
                //this.positions[this.pieces[position].Code] = 256;
                this.pieces[position] = null;
            }
            int code = piece.Code;

            this.pieces[position] = this.pieces[this.positions[code]];
            //this.removes[code] = this.pieces[this.positions[code]];
            this.pieces[this.positions[code]] = null;
            this.positions[code] = position;

        }

        /// <summary>
        /// 判断当前是否被将军
        /// </summary>
        /// <returns></returns>
        public bool Checked
        {
            get
            {
                return false;
            }
        }
        public Situation Clone(Situation other = null)
        {
            if (other == null)
            {
                return new Situation(this);
            }
            else
            {
                CopyProperty(this, other);
                return other;
            }
        }

        private static ChessPiece BuildChessPices(int code)
        {
            ChessPiece piece = null;
            switch (code)
            {
                case 0:
                case 1:
                    piece = new RedRooks(code);
                    break;
                case 2:
                case 3:
                    piece = new RedKnights(code);
                    break;
                case 4:
                case 5:
                    piece = new RedElephants(code);
                    break;
                case 6:
                case 7:
                    piece = new RedMandarins(code);
                    break;
                case 8:
                    piece = new RedKing(code);
                    break;
                case 9:
                case 10:
                    piece = new RedCannons(code);
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    piece = new RedPawns(code);
                    break;

                case 0 + 16:
                case 1 + 16:
                    piece = new BlackRooks(code);
                    break;
                case 2 + 16:
                case 3 + 16:
                    piece = new BlackKnights(code);
                    break;
                case 4 + 16:
                case 5 + 16:
                    piece = new BlackElephants(code);
                    break;
                case 6 + 16:
                case 7 + 16:
                    piece = new BlackMandarins(code);
                    break;
                case 8 + 16:
                    piece = new BlackKing(code);
                    break;
                case 9 + 16:
                case 10 + 16:
                    piece = new BlackCannons(code);
                    break;
                case 11 + 16:
                case 12 + 16:
                case 13 + 16:
                case 14 + 16:
                case 15 + 16:
                    piece = new BlackPawns(code);
                    break;
            }
            return piece;
        }
        //public byte[] Positions { get; }
        private static readonly byte[] POSITIONS = new byte[] { 0xcb, 0xc3, 0xca, 0xc4, 0xc9, 0xc5, 0xc8, 0xc6, 0xc7, 0xaa, 0xa4, 0x9b, 0x99, 0x97, 0x95, 0x93, 0x33, 0x3b, 0x34, 0x3a, 0x35, 0x39, 0x36, 0x38, 0x37, 0x54, 0x5a, 0x63, 0x65, 0x67, 0x69, 0x6b };

        //还要判断将军和将对面的情况
        public bool CanMove(ChessPiece piece, int position)
        {
            return piece.CanMove(this, position);
        }

        /// <summary>
        /// 判断两个位置是否在同一行
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns>true：表示在同一行，false:表示不在同一行</returns>
        public bool IsSameRank(int p1, int p2)
        {
            return ((p1 ^ p2) & 0xf0) == 0;
        }

        /// <summary>
        /// 判断两个位置是否在同一列
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns>true：表示在同一列，false:表示不在同一列</returns>
        public bool IsSameFile(int p1, int p2)
        {
            return ((p1 ^ p2) & 0x0f) == 0;
        }
        /// <summary>
        /// 判断两个位置是否在同一行或同一列
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns>true：表示在同一行或同一列，false:表示不在同一行或同一列</returns>
        public bool IsSameRankOrFile(int p1, int p2)
        {
            return ((p1 ^ p2) & 0xf0) == 0 || ((p1 ^ p2) & 0x0f) == 0;
        }

//        // 判断是否被将军
//        BOOL PositionStruct::Checked() const {
//  int i, j, sqSrc, sqDst;
//        int pcSelfSide, pcOppSide, pcDst, nDelta;
//        pcSelfSide = SIDE_TAG(sdPlayer);
//        pcOppSide = OPP_SIDE_TAG(sdPlayer);
//  // 找到棋盘上的帅(将)，再做以下判断：

//  for (sqSrc = 0; sqSrc< 256; sqSrc ++) {
//    if (ucpcSquares[sqSrc] != pcSelfSide + PIECE_KING) {
//      continue;
//    }

//    // 1. 判断是否被对方的兵(卒)将军
//    if (ucpcSquares[SQUARE_FORWARD(sqSrc, sdPlayer)] == pcOppSide + PIECE_PAWN) {
//      return TRUE;
//    }
//    for (nDelta = -1; nDelta <= 1; nDelta += 2) {
//      if (ucpcSquares[sqSrc + nDelta] == pcOppSide + PIECE_PAWN) {
//        return TRUE;
//      }
//    }

//    // 2. 判断是否被对方的马将军(以仕(士)的步长当作马腿)
//    for (i = 0; i< 4; i ++) {
//      if (ucpcSquares[sqSrc + ccAdvisorDelta[i]] != 0) {
//        continue;
//      }
//      for (j = 0; j< 2; j ++) {
//        pcDst = ucpcSquares[sqSrc + ccKnightCheckDelta[i][j]];
//        if (pcDst == pcOppSide + PIECE_KNIGHT) {
//          return TRUE;
//        }
//      }
//    }

//    // 3. 判断是否被对方的车或炮将军(包括将帅对脸)
//    for (i = 0; i< 4; i ++) {
//      nDelta = ccKingDelta[i];
//      sqDst = sqSrc + nDelta;
//      while (IN_BOARD(sqDst)) {
//        pcDst = ucpcSquares[sqDst];
//        if (pcDst != 0) {
//          if (pcDst == pcOppSide + PIECE_ROOK || pcDst == pcOppSide + PIECE_KING) {
//            return TRUE;
//          }
//          break;
//        }
//        sqDst += nDelta;
//      }
//      sqDst += nDelta;
//      while (IN_BOARD(sqDst)) {
//        int pcDst = ucpcSquares[sqDst];
//        if (pcDst != 0) {
//          if (pcDst == pcOppSide + PIECE_CANNON) {
//            return TRUE;
//          }
//          break;
//        }
//        sqDst += nDelta;
//      }
//    }
//    return FALSE;
//  }
//  return FALSE;
//}

//// 判断是否被杀
//BOOL PositionStruct::IsMate(void)
//{
//    int i, nGenMoveNum, pcCaptured;
//    int mvs[MAX_GEN_MOVES];

//    nGenMoveNum = GenerateMoves(mvs);
//    for (i = 0; i < nGenMoveNum; i++)
//    {
//        pcCaptured = MovePiece(mvs[i]);
//        if (!Checked())
//        {
//            UndoMovePiece(mvs[i], pcCaptured);
//            return FALSE;
//        }
//        else
//        {
//            UndoMovePiece(mvs[i], pcCaptured);
//        }
//    }
//    return TRUE;
//}
#region
//棋盘
private static readonly bool[] inBoard = {
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false
        };

        //九宫格
        private static readonly bool[] inFort = {
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, true,  true,  true,  false, false, false, false, false, false, false,
          false, false, false, false, false, false, true,  true,  true,  false, false, false, false, false, false, false,
          false, false, false, false, false, false, true,  true,  true,  false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, true,  true,  true,  false, false, false, false, false, false, false,
          false, false, false, false, false, false, true,  true,  true,  false, false, false, false, false, false, false,
          false, false, false, false, false, false, true,  true,  true,  false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false
        };
        #endregion
    }
}
