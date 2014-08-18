﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lin.Chess
{
    /// <summary>
    /// 一个基础的ChessControl，实现两个在同一个棋盘上对下功能
    /// 可以覆盖相应的方法，实现人机站、网络对站以及棋谱研究等
    /// </summary>
    public class AbstractChessControl:ChessControl
    {
        protected ChessboardView Chessboard { get; private set; }
        protected Situation Situation { get; private set; }
        public AbstractChessControl(ChessboardView chessboard)
        {
            this.Chessboard = chessboard;
            if (chessboard != null)
            {
                chessboard.Selected += (sender, args) => { OnSelected(args); };
                this.Situation = chessboard.Situation;
            }
        }
        private void SwitchPlayer()
        {
            if (Situation.Player == ChessPlayer.Red)
            {
                Situation.Player = ChessPlayer.Black;
            }
            else
            {
                Situation.Player = ChessPlayer.Red;
            }
        }
        
       
        private ChessPiece currPiece = null;
        /// <summary>
        /// 当选中棋子或选中棋盘中某个位置时执行
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnSelected(SelectedEventArgs args)
        {
            //if (args.Mouse == Mouse.LeftDown || args.Mouse == Mouse.RightDown || args.Mouse == Mouse.RightUp)
            //{
            //    return false;
            //}
            //return true;

            //if (!this.OnSelected(args))
            //{
            //    return;
            //}
            
            if (args.Position == 256)
            {
                return;
            }
            if (currPiece == null && args.Chess == null)
            {
                return;
            }
            Console.WriteLine("selected pos:" + args.Position);
            if (args.Chess != null && args.Chess.Player == this.Situation.Player)
            {
                //args.Chess.IsMark = true;
                this.Chessboard.Mark(args.Chess, true);
                if (currPiece != null && currPiece != args.Chess)
                {
                    //prePicec.IsMark = false;
                    this.Chessboard.Mark(currPiece, false);
                }
                currPiece = args.Chess;
            }
            else
            {
                //if(args.Chess != null)吃子
                if (currPiece != null)
                {
                    //prePicec.IsMark = false;
                    //currPiece = null;
                    this.Move(currPiece, args.Position);
                    currPiece = null;
                }
            }
        }



        private ChessPiece prePiece = null;
        /// <summary>
        /// 走棋
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="dest"></param>
        protected void Move(ChessPiece piece, int dest)
        {
            if (!this.Chessboard.CanMove(piece, dest))
            {
                return;
            }
            this.OnPreMove(piece, dest);
            this.Chessboard.Mark(piece, true);
            this.Chessboard.Mark(Situation.Positions[piece], true);
            this.Chessboard.Move(piece, dest);
            if (prePiece != null)
            {
                this.Chessboard.Mark(prePiece, false);
            }
            prePiece = piece;
            currPiece = null;
            this.SwitchPlayer();
            this.OnMove(piece, dest);
        }

        /// <summary>
        /// 走棋前
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="dest"></param>
        protected virtual void OnPreMove(ChessPiece piece,int dest) { }
        /// <summary>
        /// 走棋后
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="dest"></param>
        protected virtual void OnMove(ChessPiece piece, int dest) { }

    }
}
