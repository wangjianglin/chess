using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lin.Chess
{
    public abstract class ChessView:Control
    {
        public CheckerboardView Checkerboard { get; private set; }

        public ChessControl Control { get; private set; }

        public ChessView(CheckerboardView checkerboard, ChessControl control)
        {
            this.Checkerboard = checkerboard;
            this.Control = control;
        }
    }
}
