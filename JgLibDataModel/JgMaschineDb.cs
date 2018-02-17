using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JgLibDataModel
{
    public class JgMaschineDb : DbContext
    {
        public string SqlVerbindung { get; set; } = @"Data Source=.\SqlExpress;Initial Catalog = JgDataSystem; Integrated Security = True";

        public DbSet<TabStandort> TabStandortSet { get; set; }
        public DbSet<TabBediener> TabBedienerSet { get; set; }
        public DbSet<TabMaschine> TabMaschineSet { get; set; }
        public DbSet<TabBauteil> TabBauteilSet { get; set; }
        public DbSet<TabMeldung> TabMeldungSet { get; set; }
        public DbSet<TabReport> TabReportSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(SqlVerbindung);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            //ChangeTracker.DetectChanges();

            //var listeSpeichern = ChangeTracker
            //  .Entries()
            //  .Where(e => e.State == EntityState.Modified);

            //foreach (var item in listeSpeichern)
            //{
            //    if (item.Entity is JgLibHelper.IJgBase tabBase)
            //        tabBase.Aenderung = DateTime.Now;
            //}

            // http://www.bricelam.net/2016/12/13/validation-in-efcore.html

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
