using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lin.Chess
{
    public class SelectedEventArgs : EventArgs
    {
        public SelectedEventArgs(int position, Mouse mouse, int count, double x,double y,ChessPiece chess)
        {
            this.Position = position;
            this.Mouse = mouse;
            this.Count = count;
            this.Chess = chess;
            this.X = x;
            this.Y = y;
        }
        public int Position { get; private set; }
        public int Count { get; private set; }

        public ChessPiece Chess { get; private set; }

        public Mouse Mouse { get; private set; }

        public double X { get; private set; }
        public double Y { get; private set; }
    }
}
