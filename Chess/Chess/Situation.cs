﻿using Lin.Core;
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
        private ChessPiece[] removes = new ChessPiece[32];//被移出的棋子
        private int[] positions = new int[32];//记录棋子位置

        public Situation()
        {
            Player = ChessPlayer.Red;
            ChessPiece piece = null;
            for (int n = 0; n < 32; n++)
            {
                piece = BuildChessPices(n);
                pieces[POSITIONS[n]] = piece;
                positions[n] = POSITIONS[n];
            }
            init();
        }

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
        private void init()
        {
            this.Pieces = new ReadOnlyIndexProperty<int, ChessPiece>(pos => { return pieces[pos]; }, () => { return pieces.Length; });
            this.Positions = new ReadOnlyIndexProperty<ChessPiece, int>(piece => { return positions[piece.Code]; }, () => { return positions.Length; }
                , () =>
                {
                    ChessPiece[] tmpPiece = new ChessPiece[32];
                    for (int n = 0; n < 32; n++)
                    {
                        tmpPiece[n] = pieces[positions[n]];
                    }
                    return tmpPiece;
                });
        }

        private Situation(Situation situation)
        {
            for (int n = 0; n < 32; n++)
            {
                this.positions[n] = situation.positions[n];
            }
            for (int n = 0; n < 256; n++)
            {
                this.pieces[n] = situation.pieces[n];
            }
            init();
        }
        public ReadOnlyIndexProperty<int, ChessPiece> Pieces { get; private set; }


        public Lin.Util.ReadOnlyIndexProperty<ChessPiece, int> Positions { get; private set; }

        public void Move(ChessPiece piece, int position)
        {
            if (this.pieces[position] != null)
            {
                this.positions[this.pieces[position].Code] = 256;
            }
            int code = piece.Code;
            this.pieces[position] = this.pieces[this.positions[code]];
            this.removes[code] = this.pieces[this.positions[code]];
            this.pieces[this.positions[code]] = null;
            this.positions[code] = position;
        }

        public ChessPlayer Player { get; set; }

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
    }
}