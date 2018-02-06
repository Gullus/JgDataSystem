using JgLibDataModel;
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

        public async Task<List<JgDbMaschine>> GetMaschinen(Guid IdStandort)
        {
            using (var db = new JgMaschineDb() { SqlVerbindung = _SqlVerbindung })
            {
                var maschinenDb = await db.TabMaschineSet.Where(w => w.FStandort == IdStandort).ToListAsync();
                return (from z in maschinenDb
                        select new JgDbMaschine()
                        {
                            Id = z.Id,
                            Aenderung = z.Aenderung,
                            MaschineArt = z.MaschinenArt,
                            MaschineName = z.MaschineName,
                            MaschineIp = z.IpAdresse,
                            MaschinePort = z.Port,
                        }).ToList();
            }
        }

        public async Task<List<JgDbBediener>> GetBediener()
        {
            using (var db = new JgMaschineDb() { SqlVerbindung = _SqlVerbindung })
            {
                var bedienerDb = await db.TabBedienerSet.ToListAsync();
                return (from z in bedienerDb
                        select new JgDbBediener()
                        {
                            Id = z.Id,
                            Aenderung = z.Aenderung,
                            BedienerName = $"{z.Nachname}, {z.Vorname}"
                        }).ToList();
            }
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

        public Task<bool> SendeMaschinenanmeldung(JgWcfMaschinenanmeldung Anmeldung)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendeMaschinenStatus(JgWcfMaschine Maschine)
        {
            throw new NotImplementedException();
        }
    }
}
