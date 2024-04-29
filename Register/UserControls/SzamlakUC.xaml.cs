using Register.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SzamlakUC.xaml
    /// </summary>
    public partial class SzamlakUC : UserControl
    {
        EladUC euc;
        RegisterContext rc;
        List<BlokkModel> Szamlak;
        MainWindow mw;
        public SzamlakUC(RegisterContext rc, EladUC euc, MainWindow mw)
        {
            this.rc = rc;
            this.euc = euc;
            InitializeComponent();
            Szamlak = new List<BlokkModel>(from a in rc.BlokkLista
                                           where a.Fizetve == false
                                           select a).ToList();
            lbLista.ItemsSource = Szamlak;
            this.mw = mw;
        }

        private void btnUjSzamla(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
        }

        private void btnUjSzamlaVegleg(object sender, RoutedEventArgs e)
        {
            int vegosszeg = 0;
            List<TermekModel> items = new List<TermekModel>();
            if (euc.lbItemsIn.Items.Count > 0)
            {
                vegosszeg = int.Parse(euc.lbTotal.Content.ToString());
            }
            foreach (string i in euc.lbItemsIn.Items)
            {
                string[] temp = i.Split('\n');

                    for (int k = 0; k < int.Parse(temp[1]); k++)
                    {
                        items.Add((from TermekModel c in rc.TermekLista
                                   where c.Nev == temp[0]
                                   select c).FirstOrDefault());
                    }

                
                foreach (TermekModel item in rc.TermekLista)
                {
                    if (item.Nev == temp[0])
                    {
                        item.Stock -= int.Parse(temp[1]);
                    }
                }
                

            }
            popup.IsOpen = false;
            popup.StaysOpen = false;

            BlokkModel ujblokk = new BlokkModel { Vegosszeg = vegosszeg, Termekek = items, Datum = DateTime.Now, Fizetve = false, Nev = tbNev.Text, Elado = mw.elado };
            rc.BlokkLista.Add(ujblokk);
            rc.SaveChanges();
            Szamlak = new List<BlokkModel>(from a in rc.BlokkLista
                                           where a.Fizetve == false
                                           select a).ToList();
            lbLista.ItemsSource = Szamlak;
            euc.lbItemsIn.Items.Clear();
            euc.currentTotal = 0;
            euc.updateTotal();
        }

        private void btnFizetes(object sender, RoutedEventArgs e)
        {
            if (lbLista.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs számla kiválasztva!");
                return;
            }

            BlokkModel torlendo = Szamlak[lbLista.SelectedIndex];
            MessageBox.Show(torlendo.Nev + " számlája sikeresen fizetve.");
            foreach (BlokkModel bm in rc.BlokkLista)
            {
                if (bm.Id == torlendo.Id)
                {
                    bm.Fizetve = true;
                    mw.nap.Forgalom += bm.Vegosszeg;
                }
            }
            rc.SaveChanges();
            Szamlak = new List<BlokkModel>(from a in rc.BlokkLista
                                           where a.Fizetve == false
                                           select a).ToList();
            lbLista.ItemsSource = Szamlak;
        }

        private void btnBovites(object sender, RoutedEventArgs e)
        {
            BlokkModel bovitendo;
            int vegosszeg = 0;
            if (lbLista.SelectedIndex != -1)
            {
                bovitendo = Szamlak[lbLista.SelectedIndex];

            }
            else
            {
                MessageBox.Show("Nincs számla kiválasztva!");
                return;
            }
            List<TermekModel> items = new List<TermekModel>();
            if (euc.lbItemsIn.Items.Count > 0)
            {
                vegosszeg = int.Parse(euc.lbTotal.Content.ToString());
            }
            else
            {
                MessageBox.Show("Nincs felütött termék!");
                return;
            }
            foreach (string i in euc.lbItemsIn.Items)
            {
                string[] temp = i.Split('\n');

                    for (int k = 0; k < int.Parse(temp[1]); k++)
                    {
                        items.Add((from TermekModel c in rc.TermekLista
                                   where c.Nev == temp[0]
                                   select c).FirstOrDefault());
                    }

                
                foreach (TermekModel item in rc.TermekLista)
                {
                    if (item.Nev == temp[0])
                    {
                        item.Stock -= int.Parse(temp[1]);
                    }
                }

            }
            foreach (TermekModel b in items)
            {
                bovitendo.Termekek.Add(b);
            }
            Szamlak[lbLista.SelectedIndex] = bovitendo;
            BlokkModel torles = null;
            bovitendo.Vegosszeg += int.Parse(euc.lbTotal.Content.ToString());
            foreach (BlokkModel bm in rc.BlokkLista)
            {
                if (bm.Id == bovitendo.Id)
                {
                    bm.Termekek = bovitendo.Termekek;
                    bm.Vegosszeg = bovitendo.Vegosszeg;
                }
            }
            
            rc.SaveChanges();
            Szamlak = new List<BlokkModel>(from a in rc.BlokkLista
                                           where a.Fizetve == false
                                           select a).ToList();
            lbLista.ItemsSource = Szamlak;
            euc.lbItemsIn.Items.Clear();
            euc.currentTotal = 0;
            euc.updateTotal();
        }

        private void btnVissza(object sender, RoutedEventArgs e)
        {
            euc.ccKategoriak.Content = new KategoriakUC(euc, rc);
        }
    }
}
