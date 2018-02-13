using JgLibDataModel;
using JgLibHelper;
using JgWcfServiceLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace JgWcfServiceServer
{
    public class WcfService : IWcfService
    {
        private string _SqlVerbindung = ConfigurationManager.AppSettings["SqlVerbindung"];

        public async Task<List<JgWcfMaschine>> GetMaschinen(Guid IdStandort)
        {
            var lWcfMaschien = new List<JgWcfMaschine>();
            var copyMaschine = new JgCopyProperty<IJgMaschine>();

            using (var db = new JgMaschineDb() { SqlVerbindung = _SqlVerbindung })
            {
                var lMaschinenDb = await db.TabMaschineSet.Where(w => (w.IdStandort == IdStandort) && w.IstAktiv).ToListAsync();

                foreach (var maDb in lMaschinenDb)
                    lWcfMaschien.Add((JgWcfMaschine)copyMaschine.CopyProperties(maDb, new JgWcfMaschine()));
            }

            return lWcfMaschien;
        }

        public async Task<List<JgWcfBediener>> GetBediener()
        {
            var lWcfBenutzer = new List<JgWcfBediener>();
            var copyBediener = new JgCopyProperty<IJgBediener>();
            
            using (var db = new JgMaschineDb() { SqlVerbindung = _SqlVerbindung })
            {
                var tempDb = await db.TabBedienerSet.ToListAsync();

                foreach (var bediener in tempDb)
                    lWcfBenutzer.Add((JgWcfBediener)copyBediener.CopyProperties(bediener, new JgWcfBediener()));
            }

            return lWcfBenutzer;
        }

        public string WcfTest(string TestString)
        {
            return TestString;
        }

        public Task<bool> SendeBauteil(JgWcfBauteil Bauteil)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendeMeldung(JgWcfMeldung Meldung)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendeMaschinenStatus(JgWcfMaschine Maschine)
        {
            throw new NotImplementedException();
        }
    }
}
