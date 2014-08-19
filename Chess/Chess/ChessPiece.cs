using Lin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lin.Chess
{
    public abstract class ChessPiece// : Control
    {

        public string Chess { get; protected set; }
   
        public int Code { get; private set; }
        //private byte? pos;
        internal ChessPiece(int code)
        {
            this.Code = code;
            if (code < 16)
            {
                this.Side = ChessSide.Red;
            }
            else
            {
                this.Side = ChessSide.Black;
            }
        }

        public ChessSide Side { get; private set; }

        //public byte Position
        //{
        //    get { return _pos; }
        //    internal set { _pos = value; }
        //}

        /// <summary>
        /// n*2的数组，数据的第一列表示可以表的偏移量，第二列表示指定位置有棋子时，则此种走法不行的
        /// </summary>
        //public abstract int[,] Setps { get; }

        public abstract int[] Moves(Situation situation);

        public abstract bool CanMove(Situation situation, int dest);
        //{
        //    //int pos = situation.Positions[this.Code];
        //    //for (int n=0;n<Setps.GetLength(0);n++)
        //    //{
        //    //    if (pos + Setps[n,0] == dest && situation.Pieces[Setps[n,1]] == null)
        //    //    {
        //    //        return true;
        //    //    }
        //    //}
        //    return false;
        //}
        //public void Move(int positon)
        //{

        //}

        
        //public virtual bool CanMove(int position)
        //{
        //    foreach (int setp in Setps)
        //    {
        //        if (this.Position + setp == position)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

       
    }
}
