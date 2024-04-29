using Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AddItemUC.xaml
    /// </summary>
    public partial class TermekFelveszUC : UserControl
    {
        MainWindow mw;
        RegisterContext rc;
        public TermekFelveszUC(RegisterContext rc, MainWindow mw)
        {
            this.mw = mw;
            InitializeComponent();
            this.rc = rc;
            refreshCB();
            spTeljes.Visibility = Visibility.Collapsed;
            popup.IsOpen = true;
            popup.StaysOpen = true;
            lbProfilok.ItemsSource = (from a in mw.rc.ProfilLista
                                      select a.Nev).ToList();
        }

        private void btnTermekFelvetel(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbUjAr.Text, out int ar) || !int.TryParse(tbUjKeszlet.Text, out int keszlet))
            {
                MessageBox.Show("Hibás beviteli érték!", "Hiba");
                return;
            }

            if (cbUjKategoria.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs kategória kiválasztva!", "Hiba");
                return;
            }
            if ((from a in rc.TermekLista
                where a.Kategoria == cbUjKategoria.SelectedItem.ToString()
                select a).Count() < 11)
            {
                rc.TermekLista.Add(new TermekModel { Nev = tbUjNev.Text, Ar = ar, Stock = keszlet, Kategoria = cbUjKategoria.SelectedItem.ToString() });

                rc.SaveChanges();
                MessageBox.Show("Termék hozzáadva.", "Kész");
                tbUjNev.Clear();

                cbUjKategoria.SelectedIndex = -1;
                tbUjAr.Clear();
                tbUjKeszlet.Clear();
                refreshCB();
            }
            else
            {
                MessageBox.Show("Ebben a kategóriában túl sok termék van.", "Hiba!");
            }
            
        }

        private void ClearCombo(object sender, RoutedEventArgs e)
        {
            cbUjKategoria.SelectedIndex = -1;
        }


        private void refreshCB()
        {
            cbUjKategoria.ItemsSource = (from i in rc.TermekLista
                                         select i.Kategoria).Distinct().ToList();
        }
        void btnBejelentkezes(object sender, RoutedEventArgs e)
        {
            if (lbProfilok.SelectedIndex == -1 || string.IsNullOrEmpty(tbJelszo.Password) || (lbProfilok.SelectedIndex == -1 && string.IsNullOrEmpty(tbJelszo.Password)))
            {
                popup.IsOpen = false;
                MessageBox.Show("Sikertelen bejelentkezés", "Hiba");
                mw.KezdoCC.Content = new MainUC(mw);
                return;
            }
            string elad = lbProfilok.SelectedItem.ToString();
            ProfilModel ellenorzes = (from a in mw.rc.ProfilLista
                                      where a.Nev == elad
                                      select a).FirstOrDefault();
            if (ellenorzes.Jelszo == ComputeSha256Hash(tbJelszo.Password) && ellenorzes.Admin == true)
            {
                mw.elado = ellenorzes;
                popup.IsOpen = false;
                spTeljes.Visibility = Visibility.Visible;
            }
            else
            {
                popup.IsOpen = false;
                MessageBox.Show("Sikertelen bejelentkezés!", "Téves jelszó");
                mw.KezdoCC.Content = new MainUC(mw);
            }
        }

     
        private void btnExit(object sender, RoutedEventArgs e)
        {
            mw.KezdoCC.Content = new MainUC(mw);
        }
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
