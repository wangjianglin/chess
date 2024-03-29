﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lin.Chess
{
    /// <summary>
    /// 人机对站
    /// </summary>
    public class BattleChessView : ChessView
    {
        private BattleChessControl _control = null;//new TwoBattleChessControl(this.)
        public override ChessControl Control { get { return _control; } }
        //private dynamic vm = new TwoBattleChessViewVM();

        static BattleChessView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BattleChessView), new FrameworkPropertyMetadata(typeof(BattleChessView)));
        }
        public BattleChessView()
            : base()
        {
            this._control = new BattleChessControl(this);
            this.Unloaded += ViewUnloaded;
        }

        private void ViewUnloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= ViewUnloaded;
            _control.Stop();
        }

        public ChessSide Side { get { return ChessSide.Red; } }

        
    }
}
