using Register.UserControls;
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

namespace Register.UserControls
{
    /// <summary>
    /// Interaction logic for MainUC.xaml
    /// </summary>
    public partial class MainUC : UserControl
    {
        MainWindow mw;
        public MainUC(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
            if (mw.nap.Datum != DateOnly.FromDateTime(DateTime.Now))
            {
                nyitasz.Content = "Nyitás";
            }
            else
            {
                nyitasz.Content = "Zárás";
            }
        }

        private void ucSell(object sender, RoutedEventArgs e)
        {
            mw.KezdoCC.Content = new EladUC(mw);
        }

        private void btnExit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ucAddItem(object sender, RoutedEventArgs e)
        {
            mw.KezdoCC.Content = new TermekFelveszUC(mw.rc, mw);
        }

        private void ucNyitasZaras(object sender, RoutedEventArgs e)
        {
            mw.KezdoCC.Content = new NyitasZarasUC( mw, mw.rc);
        }

        private void ucDolgozoFelvesz(object sender, RoutedEventArgs e)
        {
            mw.KezdoCC.Content = new DolgozoFelveszUC(mw);
        }
    }
}
