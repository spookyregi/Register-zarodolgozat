using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class BlokkModel
    {
        public int Id { get; set; }
        public List<TermekModel> Termekek { get; set; }
        public string Nev {  get; set; }
        public bool Fizetve { get; set; }
        public int Vegosszeg { get; set; }
        public DateTime Datum { get; set; }
        public ProfilModel Elado { get; set; }
    }
}
