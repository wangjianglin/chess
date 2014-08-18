using Lin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lin.Chess
{
    /// <summary>
    /// 人机对站
    /// </summary>
    public class BattleChessControl:AbstractChessControl
    {
        private BattleChessView view;
        public BattleChessControl(BattleChessView view)
            : base(view.Chessboard)
        {
            this.view = view;
            InitBattle();
        }

        private void InitBattle() {
            Thread.BackThread(args =>
            {
                bool moveing = false;
                while (true)
                {
                    if (moveing == false && view.Player != Situation.Player)
                    {
                        System.Threading.Thread.Sleep(2000);
                        moveing = true;
                        Thread.UIThread(a =>
                        {
                            this.Move(Situation.Codes[this.piece.Code ^ 16], 0xfe - dest);
                            moveing = false;
                        });
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }, null);
        }

        private ChessPiece piece = null;
        private int dest;
        protected override void OnMove(ChessPiece piece, int dest)
        {
            this.piece = piece;
            this.dest = dest;
        }

        protected override void OnSelected(SelectedEventArgs args)
        {
            if (args.Mouse == Mouse.LeftDown || args.Mouse == Mouse.RightDown || args.Mouse == Mouse.RightUp)
            {
                return;
            }
            if (view.Player == Situation.Player)
            {
                base.OnSelected(args);
            }
        }
    }
}
