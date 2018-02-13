using System;
using System.IO;
using System.Xml.Serialization;

namespace JgDienstScannerMaschine
{
    public class JgInit
    {
        public JgOptionen _JgOpt;

        private string _FileMaschinen { get => _JgOpt.PfadDaten + "JgMaschinen.xml"; }
        private string _FileBediener { get => _JgOpt.PfadDaten + "JgBediener.xml"; }

        public JgInit(JgOptionen MyOptionen)
        {
            _JgOpt = MyOptionen;
        }

        public bool BedienerVonServer()
        {
            var speichern = false;

            try
            {
                using (var dienst = new ServiceRef.WcfServiceClient())
                {
                    var lWcfBediener = dienst.GetBediener();

                    foreach (var bedWcf in lWcfBediener)
                    {
                        JgBediener bedMaschine = null;

                        if (_JgOpt.ListeBediener.ContainsKey(bedWcf.Id))
                            bedMaschine = _JgOpt.ListeBediener[bedWcf.Id];
                        else
                        {
                            bedMaschine = new JgBediener() { Id = bedWcf.Id, Aenderung = DateTime.Now };
                            _JgOpt.ListeBediener.Add(bedWcf.Id, bedMaschine);
                        }

                        if (bedMaschine.Aenderung != bedWcf.Aenderung)
                        {
                            speichern = true;

                            bedMaschine.Vorname = bedWcf.Vorname;
                            bedMaschine.Nachname = bedWcf.Nachname;
                            bedMaschine.NummerAusweis = bedWcf.NummerAusweis;

                            bedMaschine.Aenderung = bedWcf.Aenderung;
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                JgLog.Set($"Fehler Wcf Bediener!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }

            return speichern;
        }

        public void BedienerLocalLaden()
        {
            if (!File.Exists(_FileBediener))
                return;
            try
            {
                JgBediener[] arBediener = null;
                using (var reader = new StreamReader(_FileBediener))
                {
                    var serializer = new XmlSerializer(typeof(JgBediener[]));
                    arBediener = (JgBediener[])serializer.Deserialize(reader);
                }

                _JgOpt.ListeBediener.Clear();
                foreach (var ma in arBediener)
                    _JgOpt.ListeBediener.Add(ma.Id, ma);
            }
            catch (Exception ex)
            {
                JgLog.Set($"Fehler beim localen laden Bediener!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }
        }

        public void BedienerLocalSpeichern()
        {
            var arSpeichern = new JgBediener[_JgOpt.ListeBediener.Count];
            _JgOpt.ListeBediener.Values.CopyTo(arSpeichern, 0);

            try
            {
                using (var writer = new StreamWriter(_FileBediener))
                {
                    var serializer = new XmlSerializer(typeof(JgBediener[]));
                    serializer.Serialize(writer, arSpeichern);
                }
            }
            catch (Exception ex)
            {
                JgLog.Set($"Fehler beim localen speichern der Maschinen!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }
        }

        public bool MaschinenVonServer()
        {
            var speichern = false;

            try
            {
                using (var dienst = new ServiceRef.WcfServiceClient())
                {
                    var lWcfMaschinen = dienst.GetMaschinen(_JgOpt.IdStandort);
                    foreach (var maWcf in lWcfMaschinen)
                    {
                        JgMaschineStamm maMaschine = null;

                        if (_JgOpt.ListeMaschinen.ContainsKey(maWcf.Id))
                            maMaschine = _JgOpt.ListeMaschinen[maWcf.Id];
                        else
                        {
                            switch (maWcf.MaschineArt)
                            {
                                case JgLibHelper.MaschinenArten.Hand:
                                    maMaschine = new JgMaschineHand();
                                    break;
                                case JgLibHelper.MaschinenArten.Evg:
                                    maMaschine = new JgMaschineEvg();
                                    break;
                                case JgLibHelper.MaschinenArten.Schnell:
                                    maMaschine = new JgMaschineSchnell();
                                    break;
                                case JgLibHelper.MaschinenArten.Progress:
                                    maMaschine = new JgMaschineProgress();
                                    break;
                            }

                            maMaschine.Id = maWcf.Id;
                            maMaschine.Aenderung = DateTime.Now;

                            _JgOpt.ListeMaschinen.Add(maMaschine.Id, maMaschine);
                        }

                        if (maMaschine.Aenderung != maWcf.Aenderung)
                        {
                            speichern = true;

                            maMaschine.MaschineName = maWcf.MaschineName;
                            maMaschine.MaschineArt = maWcf.MaschineArt;

                            maMaschine.MaschineIp = maWcf.MaschineIp;
                            maMaschine.MaschinePort = maWcf.MaschinePort;

                            maMaschine.NummerScanner = maWcf.NummerScanner;
                            maMaschine.ScannerMitDisplay = maWcf.ScannerMitDisplay;
                            maMaschine.SammelScannung = maWcf.SammelScannung;

                            maMaschine.Aenderung = maWcf.Aenderung;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                JgLog.Set($"Fehler beim Wcf Maschien laden!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }

            return speichern;
        }

        public void MaschinenLocalLaden()
        {
            if (!File.Exists(_FileMaschinen))
                return;

            try
            {
                JgMaschineStamm[] arMaschineStamm = null;
                using (var reader = new StreamReader(_FileMaschinen))
                {
                    var maschinenTypes = new Type[] { typeof(JgMaschineHand), typeof(JgMaschineEvg), typeof(JgMaschineSchnell), typeof(JgMaschineProgress) };
                    var serializer = new XmlSerializer(typeof(JgMaschineStamm[]), maschinenTypes);
                    arMaschineStamm = (JgMaschineStamm[])serializer.Deserialize(reader);
                }

                _JgOpt.ListeMaschinen.Clear();
                foreach (var ma in arMaschineStamm)
                    _JgOpt.ListeMaschinen.Add(ma.Id, ma);
            }
            catch (Exception ex)
            {
                JgLog.Set($"Fehler locales laden XML Daten Maschine!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }
        }

        public void MaschinenLocalSpeichern()
        {
            var arSpeichern = new JgMaschineStamm[_JgOpt.ListeMaschinen.Count];
            _JgOpt.ListeMaschinen.Values.CopyTo(arSpeichern, 0);

            try
            {
                using (var writer = new StreamWriter(_FileMaschinen))
                {
                    Type[] maschinenTypes = { typeof(JgMaschineHand), typeof(JgMaschineEvg), typeof(JgMaschineSchnell), typeof(JgMaschineProgress) };
                    var serializer = new XmlSerializer(typeof(JgMaschineStamm[]), maschinenTypes);
                    serializer.Serialize(writer, arSpeichern);
                }
            }
            catch (Exception ex)
            {
                JgLog.Set($"Fehler beim localen speichern der Maschinen!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }
        }
    }
}
