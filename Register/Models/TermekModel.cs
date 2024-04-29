using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class TermekModel
    {
        public int Id { get; set; }
        public string Nev { get; set; }
        public int Ar { get; set; }
        public int Stock { get; set; }
        public string Kategoria { get; set; }
    }
}
