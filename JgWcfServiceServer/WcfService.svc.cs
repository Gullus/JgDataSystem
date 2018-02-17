using JgLibDataModel;
using JgLibHelper;
using JgWcfServiceLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace JgWcfServiceServer
{
    public class WcfService : IWcfService
    {
        public JgCopyProperty<IJgBauteil> _KopieBauteil = new JgCopyProperty<IJgBauteil>();

        public WcfService()
        { }

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

        public Task<bool> SendeBauteil(JgWcfBauteil Bauteil, JgWcfMaschineStatus StatusMaschine)
        {
            throw new NotImplementedException();
        }

        private ScannerMeldung[] _MeldungEnde = new  ScannerMeldung[] { ScannerMeldung.ABMELDUNG, ScannerMeldung.COIL_ENDE, ScannerMeldung.REPA_ENDE, ScannerMeldung.WART_ENDE };

        public async Task<bool> SendeMeldung(JgWcfMeldung Meldung, JgWcfMaschineStatus StatusMaschine)
        {
            try
            {
                using (var db = new JgMaschineDb())
                {
                    if (Meldung.Meldung == ScannerMeldung.BAUT_ENDE)
                    {
                        // Die Endzeit eines Bauteils wird ittels einer Meldung
                        // Angezeigt und eingetragen

                        var baut = await db.TabBauteilSet.FindAsync(Meldung.Id);
                        if (baut != null)
                        {
                            baut.EndeFertigung = Meldung.Aenderung;
                            baut.Aenderung = Meldung.Aenderung;

                        }
                    }
                    else if (_MeldungEnde.Contains(Meldung.Meldung))
                    {
                        // Wird eine Abmeldung oder Beendigung gemeldet, wird die der 
                        // dazugehörige Start gesucht und die Abmeldezeit wird eingetragen

                        var meldung = await db.TabMeldungSet.FindAsync(Meldung.Id);
                        if (meldung != null)
                            meldung.ZeitAbmeldung = Meldung.Aenderung; 
                    }
                    else
                    {
                        await db.TabMeldungSet.AddAsync(new TabMeldung()
                        {
                            Id = Meldung.Id,
                            IdBediener = Meldung.IdBediener,
                            IdMaschine = Meldung.IdMaschine,
                            ZeitMeldung = Meldung.Aenderung,
                            Anzahl = Meldung.Anzahl,
                            Meldung = Meldung.Meldung,
                            Aenderung = Meldung.Aenderung
                        });
                    }

                    if (StatusMaschine != null)
                    {
                        var ma = await db.TabMaschineSet.FindAsync(Meldung.IdMaschine);
                        if (ma != null)
                        {
                            using (var memStream = new MemoryStream())
                            {
                                var formatter = new BinaryFormatter();
                                formatter.Serialize(memStream, StatusMaschine);
                                ma.StatusMaschine = memStream.ToArray();
                            };
                        }
                    }

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
