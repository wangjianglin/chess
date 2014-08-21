/*
                            _ooOoo_    
                           o8888888o    
                           88" . "88    
                           (| -_- |)    
                            O\ = /O    
                        ____/`---'\____    
                      .   ' \\| |// `.    
                       / \\||| : |||// \    
                     / _||||| -:- |||||- \    
                       | | \\\ - /// | |    
                     | \_| ''\---/'' | |    
                      \ .-\__ `-` ___/-. /    
                   ___`. .' /--.--\ `. . __    
                ."" '< `.___\_<|>_/___.' >'"".    
               | | : `- \`.;`\ _ /`;.`/ - ` : | |    
                 \ \ `-. \_ __\ /__ _/ .-` / /    
         ======`-.____`-.___\_____/___.-`____.-'======    
                            `=---='    
    
         .............................................    
                佛祖镇楼                  BUG辟易    

*/
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
        /// <summary>
        /// 局面评价，值为RedEvaluate-BlackEvaluate
        /// </summary>
        public int Evaluate { get; private set; }
        //public int RedEvaluate { get; private set; }
        //public int BlackEvaluate { get; private set; }

        static BattleChessControl()
        {
            InitCvlPieces();
        }
        public BattleChessControl(BattleChessView view)
            : base(view.Chessboard)
        {
            this.view = view;
            battleSituation = view.Chessboard.Situation.clone();
            InitBattle();
            InitEvaluate();
        }

        private Situation battleSituation = null;
        /// <summary>
        ///  Alpha-Beta搜索
        /// </summary>
        private void AlphaBeta()
        {
            Situation situation = Situation.clone(battleSituation);
            List<int> moves = GenerateMoves(situation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="situation"></param>
        /// <param name="capture"></param>
        /// <returns></returns>
        private List<int> GenerateMoves(Situation situation, bool capture = false)
        {
            ChessPiece piece = null;
            //int[] moves = null;
            List<int> moves = new List<int>();
            for (int n = 0; n < 256; n++)
            {
                piece = situation.Pieces[n];
                if (piece == null || piece.Side != Situation.Side)
                {
                    continue;
                }
                moves.AddRange(piece.Moves(situation, capture));
            }
            return moves;
        }

        private void InitBattle() {
            Thread.BackThread(args =>
            {
                bool moveing = false;
                while (true)
                {
                    if (moveing == false && view.Side != Situation.Side)
                    {
                        //System.Threading.Thread.Sleep(2000);
                        moveing = true;
                        AlphaBeta();
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

        protected override void OnPreMove(ChessPiece piece, int dest)
        {
            if (this.Situation.Pieces[dest] != null)
            {
                //this.positions[this.pieces[position].Code] = 256;
                this.Evaluate -= cvlPieces[this.Situation.Pieces[dest].Code][dest];
            }
            int code = piece.Code;
            this.Evaluate -= cvlPieces[code][this.Situation.Positions[piece]];
            this.Evaluate += cvlPieces[code][dest];
        }

        protected override void OnSelected(SelectedEventArgs args)
        {
            if (args.Mouse == Mouse.LeftDown || args.Mouse == Mouse.RightDown || args.Mouse == Mouse.RightUp)
            {
                return;
            }
            if (view.Side == Situation.Side)
            {
                base.OnSelected(args);
            }
        }

        private void InitEvaluate()
        {
            int redEvaluate = 0;
            int blackEvaluate = 0;
            ChessPiece piece = null;
            for (int n = 0; n < 256; n++)
            {
                piece = this.Situation.Pieces[n];
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
            if (view.Side == ChessSide.Red)
            {
                redEvaluate += 3;
            }
            else
            {
                blackEvaluate += 3;
            }
            this.Evaluate = redEvaluate + blackEvaluate;

        }
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
        }
        #endregion

    }
}
