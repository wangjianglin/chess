using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lin.Chess
{
    public class AbstractChessControl:ChessControl
    {
        protected ChessboardView Checkerboard { get; private set; }
        private Situation situation;
        public AbstractChessControl(ChessboardView checkerboard)
        {
            this.Checkerboard = checkerboard;
            if (checkerboard != null)
            {
                checkerboard.Selected += CheckerboardSelected;
                this.situation = checkerboard.Situation;
            }
        }
        private void SwitchPlayer()
        {
            if (situation.Player == ChessPlayer.Red)
            {
                situation.Player = ChessPlayer.Black;
            }
            else
            {
                situation.Player = ChessPlayer.Red;
            }
        }
        private ChessPiece prePiece = null;
        private ChessPiece currPiece = null;
        private void CheckerboardSelected(object sender, SelectedEventArgs args)
        {
            if (args.Mouse == Mouse.LeftDown || args.Mouse == Mouse.RightDown)
            {
                return;
            }
            if (args.Position == 256)
            {
                return;
            }
            if (args.Chess != null && args.Chess.Player != this.situation.Player)
            {
                return;
            }
            Console.WriteLine("selected pos:" + args.Position);
            if (args.Chess != null)
            {
                //args.Chess.IsMark = true;
                this.Checkerboard.Mark(args.Chess, true);
                if (currPiece != null)
                {
                    //prePicec.IsMark = false;
                    this.Checkerboard.Mark(currPiece, false);
                }
                currPiece = args.Chess;
            }
            else
            {
                if (currPiece != null)
                {
                    //prePicec.IsMark = false;
                    //currPiece = null;
                    if (!this.Checkerboard.CanMove(currPiece, args.Position))
                    {
                        return;
                    }
                    this.Checkerboard.Mark(situation.Positions[currPiece], true);
                    this.Checkerboard.Move(currPiece, args.Position);
                    if (prePiece != null)
                    {
                        this.Checkerboard.Mark(prePiece, false);
                    }
                    prePiece = currPiece;
                    currPiece = null;
                    this.SwitchPlayer();
                }
            }
        }
    }
}
