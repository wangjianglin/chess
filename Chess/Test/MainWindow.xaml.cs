using Lin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //this.DataContext = new VM();
            InitializeComponent();
        }
    }

    public class VM : Lin.Core.ViewModel.ViewModelBase
    {
        public VM()
        {
            self.Name = "ok.";
        }
        //public String Name { get; set; }
        [Command("command")]
        private void changedText(Object param)
        {
            //this.Text = "tmp";
            //dynamic tmp = this;
            //tmp.Text = "tmp";
            self.Text = "tmp";
            self.Name = "-----";
            Console.WriteLine("ok.");
        }

        [PropertyChanaged("Text")]
        private void TextChanged()
        {
            Console.WriteLine("ok.");
        }
        private dynamic This()
        {
            return this;
        }

        [CommandCanExecute("command")]
        private bool commandCanExceute(VM obj)
        {
            return true;
        }
    }
}
