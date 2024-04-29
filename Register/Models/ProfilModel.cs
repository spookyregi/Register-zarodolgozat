using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class ProfilModel
    {
        public int Id { get; set; }
        public string Nev { get; set; }
        public bool Admin { get; set; }
        public string Jelszo { get; set; }
    }
}
