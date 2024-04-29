using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class NapModel
    {
        public int Id { get; set; }
        public DateOnly Datum {  get; set; }
        public int Forgalom {  get; set; }
        public bool Lezarva { get; set; }
        public ProfilModel Nyitott { get; set; }
        public ProfilModel Zart {  get; set; }
        public int NyitoOsszeg { get; set; }
        public int ZaroOsszeg { get; set; }
    }
}
