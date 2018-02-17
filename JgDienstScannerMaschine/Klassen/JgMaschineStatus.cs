using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JgDienstScannerMaschine
{
    public class JgMaschinenStatus : IJgMaschineStatus
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

            _DateiAusgabe = GetDatai(PfadAusgabe, _Maschine.Id);
        }

        private static string GetDatai(string Pfad, Guid IdMaschine)
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

        public void Save()
        {
            if (_Maschine != null)
            {
                Task.Factory.StartNew((Opt) =>
                {
                    var optUeberg = (OptUebergabe)Opt;

                    try
                    {
                        using (var writer = new StreamWriter(optUeberg.DateiAusgabe))
                        {
                            var serializer = new XmlSerializer(typeof(JgMaschinenStatus));
                            serializer.Serialize(writer, optUeberg.StatusMaschine);
                        }
                    }
                    catch (Exception ex)
                    {
                        JgLog.Set($"Fehler Reader Load Maschinenstatus ausgelöst.\nGrund: {ex.Message}", JgLog.LogArt.Info);
                    }

                }, new OptUebergabe() { StatusMaschine = this, DateiAusgabe = _DateiAusgabe });
            }
        }

        public static void Load(JgMaschineStamm Maschine, string PfadAusgabe)
        {
            if (Maschine != null)
            {
                var datAusgabe = GetDatai(PfadAusgabe, Maschine.Id);

                if (File.Exists(datAusgabe))
                {
                    try
                    {
                        using (var reader = new StreamReader(datAusgabe))
                        {
                            var serializer = new XmlSerializer(typeof(JgMaschinenStatus));
                            var erg = (JgMaschinenStatus)serializer.Deserialize(reader);
                            if (erg != null)
                            {
                                Maschine.AktivBauteil = erg.AktivBauteil;
                                Maschine.ListeBauteile = erg.ListeBauteile;
                                Maschine.MeldBediener = erg.MeldBediener;
                                Maschine.MeldListeHelfer = erg.MeldListeHelfer;
                                Maschine.MeldMeldung = erg.MeldMeldung;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        JgLog.Set($"Fehler Reader Program 'Load' Maschinenstatus für Datei {datAusgabe} ausgelöst\nGrund: {ex.Message}", JgLog.LogArt.Info);
                    }
                }
            }
        }

        public ServiceRef.JgWcfMaschineStatus GetAsWcfMaschinenStatus()
        {
            if (_Maschine == null)
                return null;

            var lhelfer = MeldListeHelfer.Select(s => s.Id).ToList();

            var erg = new ServiceRef.JgWcfMaschineStatus()
            {
                Id = _Maschine.Id,
                Aenderung = DateTime.Now,
                IdMeldungBediener = MeldBediener?.Id,
                IdMeldungMeldung = MeldBediener?.Id,
                IdBauteilAktiv = AktivBauteil?.Id,
                ListeIdMeldungHelfer = new List<Guid>(lhelfer)
            };

            return erg;
        }
    }
}

