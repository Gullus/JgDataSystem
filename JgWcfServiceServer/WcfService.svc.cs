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
            using (var db = new JgMaschineDb() { SqlVerbindung = _SqlVerbindung })
            {
                var lMaschinenDb = await db.TabMaschineSet.Where(w => (w.IdStandort == IdStandort) && w.IstAktiv).ToListAsync();
                foreach (var ma in lMaschinenDb)
                    lWcfMaschien.Add(Helper.CopyObject<IJgMaschine, JgWcfMaschine>(new JgWcfMaschine(), ma));
            }
            return lWcfMaschien;
        }

        public async Task<List<JgWcfBediener>> GetBediener()
        {
            var lWcfBenutzer = new List<JgWcfBediener>();
            using (var db = new JgMaschineDb() { SqlVerbindung = _SqlVerbindung })
            {
                var lBedienerDb = await db.TabBedienerSet.ToListAsync();
                foreach (var bed in lBedienerDb)
                    lWcfBenutzer.Add(Helper.CopyObject<IJgMaschineBauteil, JgWcfBediener>(new JgWcfBediener(), bed));
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
