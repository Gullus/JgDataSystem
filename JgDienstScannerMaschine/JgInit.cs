using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JgDienstScannerMaschine
{
    public class JgInit
    {
        public JgOptionen _JgOpt;
        public string _PfadDateien; 

        private string _FileMaschinen { get => _PfadDateien + "JgMaschinen.xml"; }
        private string _FileBediener { get => _PfadDateien + "JgBediener.xml"; }

        public JgInit(JgOptionen MyOptionen)
        {
            _JgOpt = MyOptionen;
            _PfadDateien = _JgOpt.PfadExe + @"\Daten\";

            if (!Directory.Exists(_PfadDateien))
                Directory.CreateDirectory(_PfadDateien);
        }

        public bool BedienerVonServer()
        {
            var speichern = false;

            try
            {
                using (var dienst = new ServiceRef.WcfServiceClient())
                {
                    var lBediener = dienst.GetBediener();
                    foreach(var bediener in lBediener)
                    {
                        if (_JgOpt.ListeBediener.ContainsKey(bediener.Id))
                        {
                            var bed = _JgOpt.ListeBediener[bediener.Id];
                            if (bed.Aenderung != bediener.Aenderung)
                            {
                                speichern = true;
                                JgLibHelper.Helper.CopyObject<ServiceRef.JgDbBediener>(bed, bediener);
                            }
                        }
                        else
                        {
                            speichern = true;
                            var bed = new JgBediener();
                            JgLibHelper.Helper.CopyObject<ServiceRef.JgDbBediener>(bed, bediener);
                            _JgOpt.ListeBediener.Add(bediener.Id, bed);
                        }
                    }
                }
            }
            catch
            {
                return false;
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
            catch
            {

            }
        }

        public void BedienerLocalSpeichern()
        {
            var arSpeichern = new JgBediener[_JgOpt.ListeBediener.Count];
            _JgOpt.ListeBediener.Values.CopyTo(arSpeichern, 0);

            using (var writer = new StreamWriter(_FileBediener))
            {
                var serializer = new XmlSerializer(typeof(JgBediener[]));
                serializer.Serialize(writer, arSpeichern);
            }
        }

        public bool MaschinenVonServer()
        {
            var speichern = false;

            try
            {
                using (var dienst = new ServiceRef.WcfServiceClient())
                {
                    var lMaschinen = dienst.GetMaschinen(_JgOpt.IdStandort);
                    foreach (var maschine in lMaschinen)
                    {
                        if (_JgOpt.ListeMaschinen.ContainsKey(maschine.Id))
                        {
                            var ma = _JgOpt.ListeMaschinen[maschine.Id];
                            if (ma.Aenderung != maschine.Aenderung)
                            {
                                speichern = true;
                                JgLibHelper.Helper.CopyObject<ServiceRef.JgDbMaschine>(ma, maschine);
                            }
                        }
                        else
                        {
                            speichern = true;
 
                            JgMaschineStamm ma = null;
                            switch (maschine.MaschineArt)
                            {
                                case JgLibHelper.MaschinenArten.Hand:
                                    ma = new JgMaschineHand();
                                    break;
                                case JgLibHelper.MaschinenArten.Evg:
                                    ma = new JgMaschineEvg();
                                    break;
                                case JgLibHelper.MaschinenArten.Arsch:
                                    ma = new JgMaschineArsch();
                                    break;
                                default:
                                    break;
                            }

                            JgLibHelper.Helper.CopyObject<ServiceRef.JgDbMaschine>(ma, maschine);
                            _JgOpt.ListeMaschinen.Add(maschine.Id, ma);
                        }
                    }
                }
            }
            catch
            {
                return false;
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
                    var maschinenTypes = new Type[] { typeof(JgMaschineHand), typeof(JgMaschineEvg), typeof(JgMaschineArsch) };
                    var serializer = new XmlSerializer(typeof(JgMaschineStamm[]), maschinenTypes);
                    arMaschineStamm = (JgMaschineStamm[])serializer.Deserialize(reader);
                }

                _JgOpt.ListeMaschinen.Clear();
                foreach (var ma in arMaschineStamm)
                    _JgOpt.ListeMaschinen.Add(ma.Id, ma);
            }
            catch
            {

            }
        }

        public void MaschinenLocalSpeichern()
        {
            var arSpeichern = new JgMaschineStamm[_JgOpt.ListeMaschinen.Count];
            _JgOpt.ListeMaschinen.Values.CopyTo(arSpeichern, 0);

            using (var writer = new StreamWriter(_FileMaschinen))
            {
                Type[] personTypes = { typeof(JgMaschineHand), typeof(JgMaschineEvg), typeof(JgMaschineArsch) };
                var serializer = new XmlSerializer(typeof(JgMaschineStamm[]), personTypes);
                serializer.Serialize(writer, arSpeichern);
            }
        }
    }
}
