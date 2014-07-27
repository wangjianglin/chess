using Lin.Core.ViewModel;
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
    public abstract class ChessPiece : Control
    {
        private class ChessPieceVM : ViewModel
        {
            public ChessPieceVM(int code)
            {
                if (code < 16)
                {
                    self.FontColor = new SolidColorBrush(Color.FromArgb(255,235,50,10));
                    self.BlackVisibility = Visibility.Collapsed;
                    self.RedVisibility = Visibility.Visible;
                }
                else
                {
                    self.FontColor = new SolidColorBrush(Colors.Black);
                    self.RedVisibility = Visibility.Collapsed;
                    self.BlackVisibility = Visibility.Visible;
                }
            }

            [PropertyChanged("Size")]
            private void SizeChange()
            {
                double size = self.Size;
                self.Margin = -size / 2.0;
                self.Diameter = size * 0.9;
                self.FontSize = size * 0.6;
            }
        }
        protected dynamic vm = null;
        public string Chess { get { return vm.Chess; } protected set { vm.Chess = value; } }
        static ChessPiece()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChessPiece), new FrameworkPropertyMetadata(typeof(ChessPiece)));
        }
        private static readonly byte[] POSITIONS = new byte[] { 0xcb, 0xc3, 0xca, 0xc4, 0xc9, 0xc5, 0xc8, 0xc6, 0xc7, 0xaa, 0xa4, 0x9b, 0x99, 0x97, 0x95, 0x93, 0x33, 0x3b, 0x34, 0x3a, 0x35, 0x39, 0x36, 0x38, 0x37, 0x54, 0x5a, 0x63, 0x65, 0x67, 0x69, 0x6b };

        //private byte[] positions = null;//new byte[] {0xcb,0xc3,0xca,0xc4,0xc9,0xc5,0xc8,0xc6,0xc7,0xaa,0xa4,0x9b,0x99,0x97,0x95,0x93,0x33,0x3b,0x34,0x3a,0x35,0x39,0x36,0x38,0xc7,0x54,0x5a,0x63,0x65,0x67,0x69,0x6a};
        private byte _pos = 0;
        private int code;
        private byte? pos;
        internal ChessPiece(int code, byte? pos = null)
        {
            this.code = code;
            if (pos == null)
            {
                _pos = POSITIONS[code];
            }
            else
            {
                _pos = (byte)pos;
            }
            vm = new ChessPieceVM(this.code);
            this.DataContext = vm;
        }

        public byte Position
        {
            get { return _pos; }
            protected set { _pos = value; }
        }

        public bool IsMark { get; set; }

        public double Size { get { return vm.Size; } set { vm.Size = value; } }

        public abstract int[] Setps { get; }

        public void Move(int positon)
        {

        }

        protected override void OnPreviewMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            //base.OnPreviewMouseLeftButtonUp(e);
            //e.Handled = true;
        }
        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            //base.OnMouseLeftButtonUp(e);
            e.Handled = true;
        }
        public abstract bool CanMove(int position);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            FrameworkElement fe = (FrameworkElement)this.GetTemplateChild("PATH_ChessContent");
            if (fe != null)
            {
                fe.MouseLeftButtonUp += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                {
                    Console.WriteLine("ok.");
                };
            }
        }

    }
}
