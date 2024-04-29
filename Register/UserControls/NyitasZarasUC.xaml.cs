using Register.Models;
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
    /// Interaction logic for NyitasZarasUC.xaml
    /// </summary>
    public partial class NyitasZarasUC : UserControl
    {
        MainWindow mw;
        RegisterContext rc;
        public NyitasZarasUC(MainWindow mw, RegisterContext rc)
        {
            InitializeComponent();
            this.mw = mw;
            this.rc = rc;
            if (mw.nap.Datum == DateOnly.FromDateTime(DateTime.Now))
            {
                tbAktivNap.Text = mw.nap.Datum.ToString();
            }

            lbProfilok.ItemsSource = (from a in rc.ProfilLista
                                      select a.Nev).ToList();
            lbProfilokZaras.ItemsSource = (from a in rc.ProfilLista
                                      select a.Nev).ToList();
        }
        private void btnExit(object sender, RoutedEventArgs e)
        {
            mw.KezdoCC.Content = new MainUC(mw);
        }

        private void btnNyitas(object sender, RoutedEventArgs e)
        {
            if (mw.nap.Datum != DateOnly.FromDateTime(DateTime.Now))
            {
                if (!int.TryParse(tbNyitoosszeg.Text, out int nyitoOsszeg))
                {
                    MessageBox.Show("Hibás nyitóösszeg.", "Hiba");
                    return;
                }
                if (lbProfilok.SelectedIndex == -1)
                {
                    MessageBox.Show("Nincs profil kiválasztva.", "Hiba");
                    return;
                }
                mw.nap.Nyitott = (from a in rc.ProfilLista
                                  where a.Nev == lbProfilok.SelectedItem
                                  select a).FirstOrDefault();
                mw.nap.NyitoOsszeg = int.Parse(tbNyitoosszeg.Text);
                mw.nap.Datum = DateOnly.FromDateTime(DateTime.Now);
                MessageBox.Show("Nap megnyitva.", "Sikeres nyitás.");
                lbProfilok.SelectedIndex = -1;
                tbNyitoosszeg.Clear();
                tbAktivNap.Text = mw.nap.Datum.ToString();
            }
            else
            {
                MessageBox.Show("A nap már meg van nyitva.", "Hiba");
            }
            
        }

        private void btnZaras(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbZaroOsszeg.Text, out int zaroOsszeg))
            {
                MessageBox.Show("Hibás záróösszeg.", "Hiba");
                return;
            }

            if (lbProfilokZaras.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs profil kiválasztva.", "Hiba");
                return;
            }
            if (mw.nap.Datum == DateOnly.FromDateTime(DateTime.Now))
            {
                mw.nap.Zart = (from a in rc.ProfilLista
                               where a.Nev == lbProfilokZaras.SelectedItem
                               select a).FirstOrDefault();
                mw.nap.ZaroOsszeg = zaroOsszeg;
                mw.nap.Lezarva = true;
                rc.NapLista.Add(mw.nap);
                rc.SaveChanges();
                MessageBox.Show($"A {mw.nap.Datum} nap forgalma {mw.nap.Forgalom} Ft.", "Sikeres lezárás.");
                lbProfilokZaras.SelectedIndex = -1;
                tbZaroOsszeg.Clear();
                mw.nap = new NapModel();
                tbAktivNap.Text = "";
            }
            else
            {
                MessageBox.Show("A nap nincs megnyitva.", "Hiba");
            }

        }

        private void lbProfilok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tbNyitoosszeg_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
