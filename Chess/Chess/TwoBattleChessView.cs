using Lin.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lin.Chess
{
    public class TwoBattleChessView:ChessView
    {
        private class TwoBattleChessViewVM : ViewModel
        {

        }
        private dynamic vm = new TwoBattleChessViewVM();

        static TwoBattleChessView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TwoBattleChessView), new FrameworkPropertyMetadata(typeof(TwoBattleChessView)));
        }
        public TwoBattleChessView():base(new CheckerboardView(), new TwoBattleChessControl())
        {
            this.DataContext = vm;
        }
    }
}
