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
        public JgCopyProperty<IJgBauteil> _KopieBauteil = new JgCopyProperty<IJgBauteil>();
        public JgCopyProperty<IJgMeldung> _KopieMeldung = new JgCopyProperty<IJgMeldung>();

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

        private JgCopyProperty<IJgBauteil> _CopyBauteil = new JgCopyProperty<IJgBauteil>(); 

        public async Task<string> SendeBauteil(JgWcfBauteil Bauteil, byte[] TStatusMaschine)
        {
            try
            {
                using (var db = new JgMaschineDb())
                {
                    var bauteil = await db.TabBauteilSet.FindAsync(Bauteil.Id);
                    if (bauteil != null)
                        return $"OK Fehler Bauteil mit Id {Bauteil.Id} bereits in Datenbank vorhanden ! Vorgang wird ignoriert.";

                    var bt = new TabBauteil();
                    _CopyBauteil.CopyProperties(Bauteil, bt);
                    bt.StartFertigung = Bauteil.Aenderung;
                    await db.TabBauteilSet.AddAsync(bt);

                    if (TStatusMaschine != null)
                    {
                        var ma = await db.TabMaschineSet.FindAsync(Bauteil.IdMaschine);
                        if (ma != null)
                            ma.StatusMaschine = TStatusMaschine;
                    }

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return Helper.GetExcept(ex);
            }

            return "OK";
        }

        private ScannerMeldung[] _MeldungEnde = new ScannerMeldung[] { ScannerMeldung.ABMELDUNG, ScannerMeldung.COIL_ENDE, ScannerMeldung.REPA_ENDE, ScannerMeldung.WART_ENDE };

        public async Task<string> SendeMeldung(JgWcfMeldung Meldung, byte[] StatusMaschine)
        {
            try
            {
                using (var db = new JgMaschineDb())
                {
                    if (Meldung.Meldung == ScannerMeldung.BAUT_ENDE)
                    {
                        // Die Endzeit eines Bauteils wird mittels einer Meldung
                        // Angezeigt und eingetragen

                        var baut = await db.TabBauteilSet.FindAsync(Meldung.Id);
                        if (baut != null)
                        {
                            baut.EndeFertigung = Meldung.Aenderung;
                            baut.Aenderung = Meldung.Aenderung;
                        }
                        else
                            return $"OK Bauteil Ende nicht eingetragen, Id {Meldung.Id} nicht gefunden! Vorgang wird ignoriert.";
                    }
                    else if (_MeldungEnde.Contains(Meldung.Meldung))
                    {
                        // Wird eine Abmeldung oder Beendigung gemeldet, wird die der 
                        // dazugehörige Start gesucht und die Abmeldezeit wird eingetragen

                        var meldung = await db.TabMeldungSet.FindAsync(Meldung.Id);
                        if (meldung != null)
                        {
                            meldung.ZeitAbmeldung = Meldung.Aenderung;
                            meldung.Aenderung = Meldung.Aenderung;
                        }
                        else
                            return $"OK Meldung {meldung.Meldung} nicht eingetragen, Id {Meldung.Id} nicht gefunden! Vorgang wird ignoriert.";
                    }
                    else
                    {
                        var meld = await db.TabMeldungSet.FindAsync(Meldung.Id);
                        if (meld != null)
                            return $"OK Fehler {Meldung.Meldung} Id {Meldung.Id} bereits in Datenbank vorhanden ! Vorgang wird ignoriert.";

                        meld = new TabMeldung()
                        {
                            ZeitMeldung = Meldung.Aenderung
                        };
                        _KopieMeldung.CopyProperties(Meldung, meld);
                        await db.TabMeldungSet.AddAsync(meld);
                    }

                    if (StatusMaschine != null)
                    {
                        var ma = await db.TabMaschineSet.FindAsync(Meldung.IdMaschine);
                        if (ma != null)
                        {
                            ma.StatusMaschine = StatusMaschine;
                            ma.Aenderung = new DateTime();
                        }
                    }

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return Helper.GetExcept(ex);
            }

            return "OK";
        }
    }
}
