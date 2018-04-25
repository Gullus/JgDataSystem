using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JgLibDataModel
{
    public class JgMaschineDb : DbContext
    {
        public string SqlVerbindung { get; set; } = @"Data Source=.\SqlExpress;Initial Catalog = JgMaschineSystem; Integrated Security = True";

        public DbSet<TabStandort> TabStandortSet { get; set; }
        public DbSet<TabBediener> TabBedienerSet { get; set; }
        public DbSet<TabMaschine> TabMaschineSet { get; set; }
        public DbSet<TabBauteil> TabBauteilSet { get; set; }
        public DbSet<TabMeldung> TabMeldungSet { get; set; }
        public DbSet<TabReport> TabReportSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TabBauteil>()
                .HasIndex(b => b.IdBauteilJgData);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(SqlVerbindung);
        }

        public async Task<int> DbSave(ClaimsPrincipal user, TabBase obj)
        {
            //if (string.IsNullOrWhiteSpace(obj.Ersteller))
            //{
            //    obj.ErstelltDatum = DateTime.Now;
            //    obj.Ersteller = user.Identity.Name.Length > 30 ? user.Identity.Name.Substring(0, 30) : user.Identity.Name;
            //}
            //else
            //{
            //    obj.GeaendertDatum = DateTime.Now;
            //    obj.GeandertName = user.Identity.Name.Length > 30 ? user.Identity.Name.Substring(0, 30) : user.Identity.Name;
            //}

            return await SaveChangesAsync();
        }
    }
}
