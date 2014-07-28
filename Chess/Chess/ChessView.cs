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
        public event SelectedEventHandler Selected;
        public CheckerboardView Checkerboard { get; private set; }

        public abstract ChessControl Control { get; }

        public ChessView()
        {
            this.Checkerboard = new CheckerboardView();
            //this.Control = control;
            
        }
    }
}
