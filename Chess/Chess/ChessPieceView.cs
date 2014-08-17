using Lin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lin.Chess
{
    public class ChessPieceView:Control
    {
       
        private class ChessPieceViewModel : ViewModel
        {
            public ChessPieceViewModel(int code)
            {
                if (code == -1)
                {
                    self.IsContent = Visibility.Collapsed;
                }
                else
                {
                    self.IsContent = Visibility.Visible;
                    if (code < 16)
                    {
                        self.FontColor = new SolidColorBrush(Color.FromArgb(255, 235, 50, 10));
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

                //self.Size = 50;
                ////SizeChange();
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

        public bool IsMark { get { return vm.IsMark; } set { vm.IsMark = value; } }
      

        public double Size { get { return vm.Size; } set { vm.Size = value; } }
      
        public ChessPiece Piece { get; private set; }
        public ChessPieceView(ChessPiece piece = null)
        {
            if (piece != null)
            {
                vm = new ChessPieceViewModel(piece.Code);
                vm.Chess = piece.Chess;
            }
            else
            {
                vm = new ChessPieceViewModel(-1);
            }
            this.IsMark = false;
            this.Piece = piece;
            //this.IsMark = false;
            //this.DataContext = vm;
            this.SetDateContext((ChessPieceViewModel)vm);
        }

        #region 处理鼠标事件

        public event SelectedEventHandler Selected;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            FrameworkElement fe = (FrameworkElement)this.GetTemplateChild("PATH_ChessContent");
            if (fe != null)
            {
                //fe.MouseLeftButtonUp += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                //{
                //    //Console.WriteLine("MouseLeftButtonUp ok.");
                //};
                fe.MouseDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                {
                    this.FireSelected(e);
                };
                fe.MouseUp += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                {
                    this.FireSelected(e);
                };
                //fe.MouseRightButtonDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                //{
                //    this.FireSelected(e);
                //};
                //fe.MouseRightButtonDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                //{
                //    this.FireSelected(e);
                //};
                //fe.MouseRightButtonUp
            }
        }

        private void FireSelected(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.Selected != null)
            {
                Mouse mouse = Mouse.LeftUp;
                if (e.ChangedButton == System.Windows.Input.MouseButton.Right && e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                {
                    mouse = Mouse.RightDown;
                }else if (e.ChangedButton == System.Windows.Input.MouseButton.Right && e.ButtonState == System.Windows.Input.MouseButtonState.Released)
                {
                    mouse = Mouse.RightUp;
                }else if (e.ChangedButton == System.Windows.Input.MouseButton.Left && e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                {
                    mouse = Mouse.LeftDown;
                }
                SelectedEventArgs args = new SelectedEventArgs(256, mouse, e.ClickCount, e.MouseDevice.GetPosition(this).X, e.MouseDevice.GetPosition(this).Y, this.Piece);
                this.Selected(this, args);
            }
        }

        protected override void OnPreviewMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        protected override void OnMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnPreviewMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
        }
        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnPreviewMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
        }
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.Piece == null)
            {
                //Console.WriteLine("on mouse down.");
                this.FireSelected(e);
            }
            e.Handled = true;
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.Piece == null)
            {
                //Console.WriteLine("on mouse down.");
                this.FireSelected(e);
            }
            e.Handled = true;
        }

        protected override void OnPreviewMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        protected override void OnMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        protected override void OnPreviewMouseRightButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        protected override void OnMouseRightButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        #endregion

        static ChessPieceView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChessPieceView), new FrameworkPropertyMetadata(typeof(ChessPieceView)));
        }
        
    }
}
