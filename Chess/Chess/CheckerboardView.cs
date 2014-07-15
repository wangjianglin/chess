using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace Lin.Chess
{
    public class CheckerboardView:Control
    {
        //在VM中确定各个棋子与屏幕上显示的位置，
        private class CheckerboardViewModel : Lin.Core.ViewModel.ViewModel
        {
            public CheckerboardViewModel()
            {

            }

            //[Command(nmae="")]
            private void Command(object param)
            {

            }

            //采用动态类对 属性 进行扩展
            //[PropertyChaned(nmae="")]
            private void PropertyChaned(object value,object old){

            }
        }
        static CheckerboardView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckerboardView), new FrameworkPropertyMetadata(typeof(CheckerboardView)));
        }

        private ChessPiece[] pieces = new ChessPiece[32];

        public CheckerboardView()
        {
            this.DataContext = new CheckerboardViewModel();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DependencyObject board = this.GetTemplateChild("PATH_Board");
            for (byte n = 0; n < 32; n++)
            {
                pieces[n] = BuildChessPices(n);
                //board.Children.Add(pieces[n]);
            }
        }


        private static ChessPiece BuildChessPices(byte code, byte? position = null)
        {
            ChessPiece piece = null;
            switch (code)
            {
                case 0:
                case 1:
                    piece = new RedRooks(code, position);
                    break;
                case 2:
                case 3:
                    piece = new RedKnights(code, position);
                    break;
                case 4:
                case 5:
                    piece = new RedElephants(code, position);
                    break;
                case 6:
                case 7:
                    piece = new RedMandarins(code, position);
                    break;
                case 8:
                    piece = new RedKing(code, position);
                    break;
                case 9:
                case 10:
                    piece = new RedCannons(code, position);
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    piece = new RedPawns(code, position);
                    break;

                case 0 + 16:
                case 1 + 16:
                    piece = new BlackRooks(code, position);
                    break;
                case 2 + 16:
                case 3 + 16:
                    piece = new BlackKnights(code, position);
                    break;
                case 4 + 16:
                case 5 + 16:
                    piece = new BlackElephants(code, position);
                    break;
                case 6 + 16:
                case 7 + 16:
                    piece = new BlackMandarins(code, position);
                    break;
                case 8 + 16:
                    piece = new BlackKing(code, position);
                    break;
                case 9 + 16:
                case 10 + 16:
                    piece = new BlackCannons(code, position);
                    break;
                case 11 + 16:
                case 12 + 16:
                case 13 + 16:
                case 14 + 16:
                case 15 + 16:
                    piece = new BlackPawns(code, position);
                    break;
            }
            return piece;
        }
    }
}
