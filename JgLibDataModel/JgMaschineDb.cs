using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgLibDataModel
{
    public class JgMaschineDb : DbContext
    {
        public string SqlVerbindung { get; set; } = @"Data Source=.\SqlExpress;Initial Catalog = JgDataSystem; Integrated Security = True";

        public DbSet<TabStandort> TabStandortSet { get; set; }
        public DbSet<TabBediener> TabBedienerSet { get; set; }
        public DbSet<TabMaschine> TabMaschineSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(SqlVerbindung);
        }
    }
}
