using JgLibHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    public class JgMaschinenStatus
    {
        #region Schnittstelle

        public JgBauteil AktivBauteil { get; set; }
        public JgMeldung MeldBediener { get; set; }
        public List<JgMeldung> MeldListeHelfer { get; set; }
        public JgMeldung MeldMeldung { get; set; }

        #endregion

        public List<JgBauteilFertig> ListeBauteile { get; set; }

        private struct OptUebergabe
        {
            public JgMaschinenStatus StatusMaschine { get; set; }
            public string DateiAusgabe { get; set; }
        }

        private string _DateiAusgabe;
        private JgMaschineStamm _Maschine = null;

        public JgMaschinenStatus()
        { }

        public JgMaschinenStatus(JgMaschineStamm Maschine, string PfadAusgabe)
        {
            _Maschine = Maschine;

            if (_Maschine != null)
                MaschineInThis(Maschine);

            _DateiAusgabe = GetDateiName(PfadAusgabe, _Maschine.Id);
        }

        private static string GetDateiName(string Pfad, Guid IdMaschine)
        {
            return Pfad + "StatusMaschine_" + IdMaschine.ToString() + ".xml";
        }

        private void MaschineInThis(JgMaschineStamm Maschine)
        {
            AktivBauteil = Maschine.AktivBauteil;
            MeldBediener = Maschine.MeldBediener;
            MeldListeHelfer = Maschine.MeldListeHelfer;
            MeldMeldung = Maschine.MeldMeldung;
            ListeBauteile = Maschine.ListeBauteile;
        }

        public void SaveStatusMaschineLocal()
        {
            if (_Maschine != null)
            {
                Task.Factory.StartNew((Opt) =>
                {
                    var optUeberg = (OptUebergabe)Opt;

                    try
                    {
                        Helper.ObjektInXmlDatei<JgMaschinenStatus>(optUeberg.StatusMaschine, optUeberg.DateiAusgabe);
                    }
                    catch (Exception ex)
                    {
                        JgLog.Set(null, $"Fehler Reader Load Maschinenstatus ausgelöst.\nGrund: {ex.Message}", JgLog.LogArt.Info);
                    }

                }, new OptUebergabe() { StatusMaschine = this, DateiAusgabe = _DateiAusgabe });
            }
        }

        public static void LoadStatusMaschineLocal(JgMaschineStamm Maschine, string PfadAusgabe)
        {
            if (Maschine != null)
            {
                var datAusgabe = GetDateiName(PfadAusgabe, Maschine.Id);

                try
                {
                    var erg = Helper.XmlDateiInObjekt<JgMaschinenStatus>(datAusgabe);

                    if (erg != null)
                    {
                        Maschine.AktivBauteil = erg.AktivBauteil;
                        Maschine.ListeBauteile = erg.ListeBauteile;
                        Maschine.MeldBediener = erg.MeldBediener;
                        Maschine.MeldListeHelfer = erg.MeldListeHelfer;
                        Maschine.MeldMeldung = erg.MeldMeldung;
                    }
                }
                catch (Exception ex)
                {
                    JgLog.Set(null, $"Fehler Reader Program 'Load' Maschinenstatus für Datei {datAusgabe} ausgelöst\nGrund: {ex.Message}", JgLog.LogArt.Info);
                }
            }
        }

        public byte[] GetStatusAsXmlByte()
        {
            if (_Maschine != null)
            {
                var lhelfer = MeldListeHelfer.Select(s => s.Id).ToList();

                var erg = new JgMaschinenStatusMeldungen()
                {
                    Aenderung = DateTime.Now,
                    IdBediener = MeldBediener?.Id,
                    IdMeldung = MeldMeldung?.Id,
                    IdAktivBauteil = AktivBauteil?.Id,
                    IdListeHelfer = new List<Guid>(lhelfer),
                    Information = _Maschine.Information
                };

                try
                {
                    return Helper.ObjectInXmlDatenByte<JgMaschinenStatusMeldungen>(erg);
                }
                catch (Exception ex)
                {
                    JgLog.Set(null, $"Fehler Reader Load Maschinenstatus ausgelöst.\nGrund: {ex.Message}", JgLog.LogArt.Info);
                }
            }

            return null;
        }
    }
}

