using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgLibDataModel.Tabellen
{
    public class JgMaschineDb : DbContext
    {
        public string SqlVerbindung { get; set; }

        public DbSet<tabBediener> TabBedienerSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(SqlVerbindung);
        }
    }
}
