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
        
        public ChessSide Side { get; private set; }

        /// <summary>
        /// 局面评价，值为RedEvaluate-BlackEvaluate
        /// </summary>
        public int Evaluate { get; private set; }
        //public int RedEvaluate { get; private set; }
        //public int BlackEvaluate { get; private set; }

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
            InitEvaluate();
        }
        private void init()
        {
            this.Pieces = new ReadOnlyIndexProperty<int, ChessPiece>(pos => { return pieces[pos]; }, () => { return pieces.Length; });
            this.Codes = new ReadOnlyIndexProperty<int, ChessPiece>(pos => { return codes[pos]; }, () => { return pieces.Length; });
            this.Positions = new ReadOnlyIndexProperty<ChessPiece, int>(piece => { return positions[piece.Code]; }, () => { return positions.Length; });
        }

        private void InitEvaluate()
        {
            int redEvaluate = 0;
            int blackEvaluate = 0;
            ChessPiece piece = null;
            for (int n = 0; n < 256; n++)
            {
                piece = this.pieces[n];
                if (piece == null)
                {
                    continue;
                }
                if (piece.Side == ChessSide.Red)
                {
                    redEvaluate += cvlPieces[piece.Code][n];
                }
                else
                {
                    blackEvaluate += cvlPieces[piece.Code][n];
                }
            }
            if (Side == ChessSide.Red)
            {
                redEvaluate += 3;
            }
            else
            {
                blackEvaluate += 3;
            }
            this.Evaluate = redEvaluate + blackEvaluate;
            //int[] ns = new int[]{0,2,4,6,8,9,11};

            //for (int n = 0; n < ns.Length; n++)
            //{
            //    Console.WriteLine("----------------------------------------------------------------------------------");
            //    p2(cvlPieces[ns[n]]);
            //    Console.WriteLine();
            //    p2(cvlPieces[ns[n] + 16]);
            //    Console.WriteLine("----------------------------------------------------------------------------------");
            //    Console.WriteLine();
            //    Console.WriteLine();
            //    Console.WriteLine();
            //}


            //Console.WriteLine();

           

        }

        //public void p2(int[] p)
        //{

        //    for (int n = 0; n < 16; n++)
        //    {
        //        for (int m = 0; m < 16; m++)
        //        {
        //            Console.Write("\t" + p[n * 16 + m] + ",");
        //        }
        //        Console.WriteLine();
        //    }
        //}

        private Situation(Situation situation)
        {
            for (int n = 0; n < 32; n++)
            {
                this.positions[n] = situation.positions[n];
                this.codes[n] = situation.codes[n];
                //this.removes[n] = situation.removes[n];
            }
            for (int n = 0; n < 256; n++)
            {
                this.pieces[n] = situation.pieces[n];
            }
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

        public void Move(ChessPiece piece, int position)
        {
            if (this.pieces[position] != null)
            {
                //this.positions[this.pieces[position].Code] = 256;
                this.Evaluate -= cvlPieces[this.pieces[position].Code][position];
                this.pieces[position] = null;
            }
            int code = piece.Code;
            this.Evaluate -= cvlPieces[code][this.positions[code]];
            this.Evaluate += cvlPieces[code][position];

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
        public Situation clone()
        {
            return new Situation(this);
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

        static Situation()
        {
            InitCvlPieces();
        }

        #region
        private static readonly bool[] inBoard = {
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false
        };

        private static readonly bool[] inFort = {
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, true, true, true, false, false, false, false, false, false, false,
          false, false, false, false, false, false, true, true, true, false, false, false, false, false, false, false,
          false, false, false, false, false, false, true, true, true, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, true, true, true, false, false, false, false, false, false, false,
          false, false, false, false, false, false, true, true, true, false, false, false, false, false, false, false,
          false, false, false, false, false, false, true, true, true, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
          false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false
        };
        #endregion


        // 子力位置价值表
       
        #region cvl piece

        private static readonly int[] cvlBlackKing = new int[256];
        private static readonly int[] cvlRedKing = new int[]{ // 帅(将)
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  1,  1,  1,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  2,  2,  2,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0, 11, 15, 11,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0
          };
        private static readonly int[] cvlBlackMandarins = new int[256];
        private static readonly int[] cvlRedMandarins = new int[]{ // 仕(士)
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0, 20,  0, 20,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0, 23,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0, 20,  0, 20,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0
          };
         private static readonly int[] cvlBlackElephants = new int[256];
        private static readonly int[] cvlRedElephants = new int[]{ // 相(象)
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0, 20,  0,  0,  0, 20,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0, 18,  0,  0,  0, 23,  0,  0,  0, 18,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0, 20,  0,  0,  0, 20,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0
      };
        private static readonly int[] cvlBlackKnights = new int[256];
        private static readonly int[] cvlRedKnights = new int[]{ // 马
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0, 90, 90, 90, 96, 90, 96, 90, 90, 90,  0,  0,  0,  0,
        0,  0,  0, 90, 96,103, 97, 94, 97,103, 96, 90,  0,  0,  0,  0,
        0,  0,  0, 92, 98, 99,103, 99,103, 99, 98, 92,  0,  0,  0,  0,
        0,  0,  0, 93,108,100,107,100,107,100,108, 93,  0,  0,  0,  0,
        0,  0,  0, 90,100, 99,103,104,103, 99,100, 90,  0,  0,  0,  0,
        0,  0,  0, 90, 98,101,102,103,102,101, 98, 90,  0,  0,  0,  0,
        0,  0,  0, 92, 94, 98, 95, 98, 95, 98, 94, 92,  0,  0,  0,  0,
        0,  0,  0, 93, 92, 94, 95, 92, 95, 94, 92, 93,  0,  0,  0,  0,
        0,  0,  0, 85, 90, 92, 93, 78, 93, 92, 90, 85,  0,  0,  0,  0,
        0,  0,  0, 88, 85, 90, 88, 90, 88, 90, 85, 88,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0
      };
        private static readonly int[] cvlBlackRooks = new int[256];
        private static readonly int[] cvlRedRooks = new int[]{ // 车
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,206,208,207,213,214,213,207,208,206,  0,  0,  0,  0,
        0,  0,  0,206,212,209,216,233,216,209,212,206,  0,  0,  0,  0,
        0,  0,  0,206,208,207,214,216,214,207,208,206,  0,  0,  0,  0,
        0,  0,  0,206,213,213,216,216,216,213,213,206,  0,  0,  0,  0,
        0,  0,  0,208,211,211,214,215,214,211,211,208,  0,  0,  0,  0,
        0,  0,  0,208,212,212,214,215,214,212,212,208,  0,  0,  0,  0,
        0,  0,  0,204,209,204,212,214,212,204,209,204,  0,  0,  0,  0,
        0,  0,  0,198,208,204,212,212,212,204,208,198,  0,  0,  0,  0,
        0,  0,  0,200,208,206,212,200,212,206,208,200,  0,  0,  0,  0,
        0,  0,  0,194,206,204,212,200,212,204,206,194,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0
      };
        private static readonly int[] cvlBlackCannons = new int[256];
        private static readonly int[] cvlRedCannons = new int[]{ // 炮
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,100,100, 96, 91, 90, 91, 96,100,100,  0,  0,  0,  0,
        0,  0,  0, 98, 98, 96, 92, 89, 92, 96, 98, 98,  0,  0,  0,  0,
        0,  0,  0, 97, 97, 96, 91, 92, 91, 96, 97, 97,  0,  0,  0,  0,
        0,  0,  0, 96, 99, 99, 98,100, 98, 99, 99, 96,  0,  0,  0,  0,
        0,  0,  0, 96, 96, 96, 96,100, 96, 96, 96, 96,  0,  0,  0,  0,
        0,  0,  0, 95, 96, 99, 96,100, 96, 99, 96, 95,  0,  0,  0,  0,
        0,  0,  0, 96, 96, 96, 96, 96, 96, 96, 96, 96,  0,  0,  0,  0,
        0,  0,  0, 97, 96,100, 99,101, 99,100, 96, 97,  0,  0,  0,  0,
        0,  0,  0, 96, 97, 98, 98, 98, 98, 98, 97, 96,  0,  0,  0,  0,
        0,  0,  0, 96, 96, 97, 99, 99, 99, 97, 96, 96,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0
      };
        private static readonly int[] cvlBlackPawns = new int[256];
        private static readonly int[] cvlRedPawns = new int[]{ // 兵(卒)
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  9,  9,  9, 11, 13, 11,  9,  9,  9,  0,  0,  0,  0,
        0,  0,  0, 19, 24, 34, 42, 44, 42, 34, 24, 19,  0,  0,  0,  0,
        0,  0,  0, 19, 24, 32, 37, 37, 37, 32, 24, 19,  0,  0,  0,  0,
        0,  0,  0, 19, 23, 27, 29, 30, 29, 27, 23, 19,  0,  0,  0,  0,
        0,  0,  0, 14, 18, 20, 27, 29, 27, 20, 18, 14,  0,  0,  0,  0,
        0,  0,  0,  7,  0, 13,  0, 16,  0, 13,  0,  7,  0,  0,  0,  0,
        0,  0,  0,  7,  0,  7,  0, 15,  0,  7,  0,  7,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0
      };


        private static readonly int[][] cvlPieces = new int[][] { 
            cvlRedRooks, cvlRedRooks, cvlRedKnights, cvlRedKnights, cvlRedElephants, cvlRedElephants, cvlRedMandarins, cvlRedMandarins, cvlRedKing, cvlRedCannons, cvlRedCannons, cvlRedPawns, cvlRedPawns, cvlRedPawns, cvlRedPawns, cvlRedPawns,
            cvlBlackRooks, cvlBlackRooks, cvlBlackKnights, cvlBlackKnights, cvlBlackElephants, cvlBlackElephants, cvlBlackMandarins, cvlBlackMandarins, cvlBlackKing, cvlBlackCannons, cvlBlackCannons, cvlBlackPawns, cvlBlackPawns, cvlBlackPawns, cvlBlackPawns, cvlBlackPawns
        };

        private static void InitCvlPieces()
        {
            int br = 0;
            for (int r = 0; r < 16; r++)
            {
                br = r + 16;
                for (int n = 3; n < 13; n++)
                {
                    for (int m = 3; m < 12; m++)
                    {
                        cvlPieces[br][n * 16 + m] = -cvlPieces[r][254 - n * 16 - m];
                    }
                }
            }
            //cvlPieces = new int[][] { cvlRooks, cvlRooks, cvlKnights, cvlKnights, cvlElephants, cvlElephants, cvlMandarins, cvlMandarins, cvlKing, cvlCannons, cvlPawns, cvlPawns, cvlPawns, cvlPawns, cvlPawns };
        }
        //public void p()
        //{
        //    Console.WriteLine("private static readonly int[] cvlBlackRooks = new int[]{");
        //    p2(cvlRooks);
        //    Console.WriteLine("};");
        //    Console.WriteLine();

        //    Console.WriteLine("private static readonly int[] cvlBlackKnights = new int[]{");
        //    p2(cvlKnights);
        //    Console.WriteLine("};");
        //    Console.WriteLine();

        //    Console.WriteLine("private static readonly int[] cvlMandarins = new int[]{");
        //    p2(cvlMandarins);
        //    Console.WriteLine("};");
        //    Console.WriteLine();

        //    Console.WriteLine("private static readonly int[] cvlElephants = new int[]{");
        //    p2(cvlElephants);
        //    Console.WriteLine("};");
        //    Console.WriteLine();

        //    Console.WriteLine("private static readonly int[] cvlKing = new int[]{");
        //    p2(cvlKing);
        //    Console.WriteLine("};");
        //    Console.WriteLine();

        //    Console.WriteLine("private static readonly int[] cvlPawns = new int[]{");
        //    p2(cvlCannons);
        //    Console.WriteLine("};");
        //    Console.WriteLine();

        //    p2(cvlPawns);
        //    Console.WriteLine("};");
        //    Console.WriteLine();
        //}

        //public void p2(int[] p)
        //{

        //    for (int n = 0; n < 16; n++)
        //    {
        //        for (int m = 0; n < 16; m++)
        //        {
        //            Console.Write("\t" + p[n * 16 + m] + ",");
        //        }
        //        Console.WriteLine();
        //    }
        //}
        #endregion

      
    }
}
