using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lin.Chess
{
    public class AbstractChessControl:ChessControl
    {
        protected CheckerboardView Checkerboard { get; private set; }
        public AbstractChessControl(CheckerboardView checkerboard)
        {
            this.Checkerboard = checkerboard;
            if (checkerboard != null)
            {
                checkerboard.Selected += (object sender, SelectedEventArgs args) =>
                {
                    Console.WriteLine("ok.");
                };
            }
        }
    }
}
