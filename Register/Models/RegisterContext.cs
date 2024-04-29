using Microsoft.EntityFrameworkCore;
using Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class RegisterContext : DbContext
    {
        public DbSet<TermekModel> TermekLista { get; set; }
        public DbSet<BlokkModel> BlokkLista { get; set; }
        public DbSet<ProfilModel> ProfilLista { get; set; }
        public DbSet<NapModel> NapLista { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string c = "server = localhost; userid = root; password =; database = register";

            optionsBuilder.UseMySql(c, ServerVersion.AutoDetect(c));

        }
    }
}
