using Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for DolgozoFelveszUC.xaml
    /// </summary>
    public partial class DolgozoFelveszUC : UserControl
    {
        MainWindow mw;
        public DolgozoFelveszUC(MainWindow mw)
        {
            InitializeComponent();
            popup.IsOpen = true;
            popup.StaysOpen = true;
            spTeljes.Visibility = Visibility.Collapsed;
            this.mw = mw;
            lbProfilok.ItemsSource = (from a in mw.rc.ProfilLista
                                      select a.Nev).ToList();
        }
        private void btnExit(object sender, RoutedEventArgs e)
        {
            mw.KezdoCC.Content = new MainUC(mw);
        }

        private void btnDolgozoFelvetel(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbUjNev.Text) || string.IsNullOrEmpty(tbUjJelszo.Text))
            {
                MessageBox.Show("Hiányzó adatok!", "Hiba");
                return;
            }
            else
            {         
            mw.rc.ProfilLista.Add(new ProfilModel { Admin = cbJogosultsag.IsChecked.Value, Jelszo = ComputeSha256Hash(tbUjJelszo.Text), Nev = tbUjNev.Text });
            mw.rc.SaveChanges();
            MessageBox.Show("Dolgozó felvéve!", "Sikeres mentés");
            mw.KezdoCC.Content = new MainUC(mw);
            }
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
                spTeljes.Visibility = Visibility.Visible;
                popup.IsOpen = false;
            }
            else
            {
                popup.StaysOpen = false;
                popup.IsOpen = false;
                MessageBox.Show("Sikertelen bejelentkezés!", "Téves jelszó");
                mw.KezdoCC.Content = new MainUC(mw);
            }
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
