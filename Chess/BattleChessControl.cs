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
    public class BattleChessControl : AbstractChessControl
    {
        private BattleChessView view;
        ///// <summary>
        ///// 局面评价，值为RedEvaluate-BlackEvaluate
        ///// </summary>
        //public int Evaluate { get; private set; }
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
            //battleSituation = view.Chessboard.Situation.clone();
            battleSituation = new BattleSituation(view.Chessboard.Situation.clone(),view.Side);
            InitBattle();
            //InitEvaluate();
        }

        private BattleSituation battleSituation = null;
        private static readonly int LIMIT_DEPTH = 32;//最大的搜索深度
        private static readonly int MATE_VALUE = 10000;//最高分值，即将死的分值
        private static readonly int WIN_VALUE = MATE_VALUE - 100;//搜索出胜负的分值界限，超出此值就说明已经搜索出杀棋了
        private static readonly int MAX_GEN_MOVES = 128;//最大的生成走法数
        private static readonly int ADVANCED_VALUE = 3;  // 先行权分值
        //const int MAX_GEN_MOVES = 128; // 最大的生成走法数
        //const int LIMIT_DEPTH = 32;    // 
        //const int MATE_VALUE = 10000;  // 
        //const int WIN_VALUE = MATE_VALUE - 100; // 
        //const int ADVANCED_VALUE = 3;  // 先行权分值
        /// <summary>
        ///  Alpha-Beta搜索
        /// </summary>
        private void AlphaBeta()
        {
            //Situation situation = Situation.clone(battleSituation);
            //List<int> moves = GenerateMoves(situation);
            BattleSituation bs = battleSituation.Clone();

            int[] historyTable = new int[65536]; // 历史表
            // 初始化
            //memset(Search.nHistoryTable, 0, 65536 * sizeof(int)); // 清空历史表
            //t = clock();       // 初始化定时器
            //pos.nDistance = 0; // 初始步数


            // 迭代加深过程
            for (int i = 1; i <= LIMIT_DEPTH; i++)
            {
                int vl = SearchFull(bs ,- MATE_VALUE, MATE_VALUE, i, historyTable);
                // 搜索到杀棋，就终止搜索
                if (vl > WIN_VALUE || vl < -WIN_VALUE)
                {
                    break;
                }
                // 超过一秒，就终止搜索
                //if (clock() - t > CLOCKS_PER_SEC)
                //{
                //    break;
                //}
            }
        }

        // "qsort"按历史表排序的比较函数
//        static int CompareHistory(const void* lpmv1, const void* lpmv2) {
//  return Search.nHistoryTable[*(int*)lpmv2] - Search.nHistoryTable[*(int*)lpmv1];
//}
    // 超出边界(Fail-Soft)的Alpha-Beta搜索过程
    int SearchFull(BattleSituation bs,int vlAlpha, int vlBeta, int nDepth,int[] historyTable)
        {
            int i, nGenMoves, pcCaptured;
            int vl, vlBest, mvBest;
            //int[] mvs = new int[MAX_GEN_MOVES];
            // 一个Alpha-Beta完全搜索分为以下几个阶段

            // 1. 到达水平线，则返回局面评价值
            if (nDepth == 0)
            {
                //return pos.Evaluate();
                //return 0;
                return bs.evaluate;
            }

            // 2. 初始化最佳值和最佳走法
            vlBest = -MATE_VALUE; // 这样可以知道，是否一个走法都没走过(杀棋)
            mvBest = 0;           // 这样可以知道，是否搜索到了Beta走法或PV走法，以便保存到历史表

            // 3. 生成全部走法，并根据历史表排序
            //nGenMoves = pos.GenerateMoves(mvs);
            List<int> moves = GenerateMoves(bs.situation);
            //qsort(mvs, nGenMoves, sizeof(int), CompareHistory);
            moves.Sort((int x, int y) =>
            {
                return historyTable[y] - historyTable[x];
            });

            // 4. 逐一走这些走法，并进行递归
            for (i = 0; i < nGenMoves; i++)
            {
                if (pos.MakeMove(mvs[i], pcCaptured))
                {
                    vl = -SearchFull(-vlBeta, -vlAlpha, nDepth - 1);
                    pos.UndoMakeMove(mvs[i], pcCaptured);

                    // 5. 进行Alpha-Beta大小判断和截断
                    if (vl > vlBest)
                    {    // 找到最佳值(但不能确定是Alpha、PV还是Beta走法)
                        vlBest = vl;        // "vlBest"就是目前要返回的最佳值，可能超出Alpha-Beta边界
                        if (vl >= vlBeta)
                        { // 找到一个Beta走法
                            mvBest = mvs[i];  // Beta走法要保存到历史表
                            break;            // Beta截断
                        }
                        if (vl > vlAlpha)
                        { // 找到一个PV走法
                            mvBest = mvs[i];  // PV走法要保存到历史表
                            vlAlpha = vl;     // 缩小Alpha-Beta边界
                        }
                    }
                }
            }

            // 5. 所有走法都搜索完了，把最佳走法(不能是Alpha走法)保存到历史表，返回最佳值
            if (vlBest == -MATE_VALUE)
            {
                // 如果是杀棋，就根据杀棋步数给出评价
                return pos.nDistance - MATE_VALUE;
            }
            if (mvBest != 0)
            {
                // 如果不是Alpha走法，就将最佳走法保存到历史表
                Search.nHistoryTable[mvBest] += nDepth * nDepth;
                if (pos.nDistance == 0)
                {
                    // 搜索根节点时，总是有一个最佳走法(因为全窗口搜索不会超出边界)，将这个走法保存下来
                    Search.mvResult = mvBest;
                }
            }
            return vlBest;
        }

        /// <summary>
        /// 如果"capture"为"true"则只生成吃子走法
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

        private bool isStop = false;
        public void Stop()
        {
            isStop = true;
        }
        private void InitBattle()
        {
            Thread.BackThread(args =>
            {
                bool moveing = false;
                while (!isStop)
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
            //if (this.Situation.Pieces[dest] != null)
            //{
            //    //this.positions[this.pieces[position].Code] = 256;
            //    this.Evaluate -= cvlPieces[this.Situation.Pieces[dest].Code][dest];
            //}
            //int code = piece.Code;
            //this.Evaluate -= cvlPieces[code][this.Situation.Positions[piece]];
            //this.Evaluate += cvlPieces[code][dest];
            battleSituation.Move(piece, dest);
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

        
     

        private class BattleSituation
        {
            /// <summary>
            /// 局面评价，值为RedEvaluate-BlackEvaluate
            /// </summary>
            public int evaluate;
            //{
            //    get { return redEvaluate - blackEvaluate; }
            //}
            //private int redEvaluate;
            //private int blackEvaluate;
            public Situation situation;
            ChessSide side;

            public BattleSituation(Situation situation, ChessSide side = ChessSide.Red)
            {
                this.situation = situation;
                InitEvaluate();
            }

            private BattleSituation() { }

            private void InitEvaluate()
            {
                int redEvaluate = 0;
                int blackEvaluate = 0;
                ChessPiece piece = null;
                for (int n = 0; n < 256; n++)
                {
                    piece = this.situation.Pieces[n];
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
                if (side == ChessSide.Red)
                {
                    redEvaluate += 3;
                }
                else
                {
                    blackEvaluate += 3;
                }
                this.evaluate = redEvaluate + blackEvaluate;

            }

            public BattleSituation Clone()
            {
                Situation s = situation.Clone();
                BattleSituation bs = new BattleSituation();
                bs.situation = s;
                bs.evaluate = this.evaluate;
                return bs;
            }

            public void Move(ChessPiece piece, int dest)
            {
                if (this.situation.Pieces[dest] != null)
                {
                    //this.positions[this.pieces[position].Code] = 256;
                    this.evaluate -= cvlPieces[this.situation.Pieces[dest].Code][dest];
                }
                int code = piece.Code;
                this.evaluate -= cvlPieces[code][this.situation.Positions[piece]];
                this.evaluate += cvlPieces[code][dest];
            }
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
