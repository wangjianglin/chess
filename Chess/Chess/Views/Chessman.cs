using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lin.Chess.Views
{
    public class Chessman : Control
    {
        static Chessman()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Chessman), new FrameworkPropertyMetadata(typeof(Chessman)));
        }

        public Chessman()
        {
            Console.WriteLine("ok.");
        }
    }
}
