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
        
        private ChessControl _control = null;//new TwoBattleChessControl(this.)
        public override ChessControl Control { get { return _control; } }
        private dynamic vm = new TwoBattleChessViewVM();

        static TwoBattleChessView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TwoBattleChessView), new FrameworkPropertyMetadata(typeof(TwoBattleChessView)));
        }
        public TwoBattleChessView()
            : base()
        {
            this.DataContext = vm;
            this._control = new TwoBattleChessControl(this.Checkerboard);
        }
    }
}
