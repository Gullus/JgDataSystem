using JgLibHelper;
using System;
using System.IO;
using System.Threading.Tasks;
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
                    foreach(var bedWcf in lWcfBediener)
                    {
                        if (_JgOpt.ListeBediener.ContainsKey(bedWcf.Id))
                        {
                            var bedMaschine = _JgOpt.ListeBediener[bedWcf.Id];
                            if (bedMaschine.Aenderung != bedWcf.Aenderung)
                            {
                                speichern = true;
                                Helper.CopyObject<IJgMaschineBauteil, JgBediener>(bedMaschine, bedWcf);
                            }
                        }
                        else
                        {
                            speichern = true;
                            var bedMaschine = Helper.CopyObject<IJgMaschineBauteil, JgBediener>(new JgBediener(), bedWcf);
                            _JgOpt.ListeBediener.Add(bedMaschine.Id, bedMaschine);
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
                    var lWcfMaschinen = dienst.GetMaschinen(_JgOpt.IdStandort);
                    foreach (var maWcf in lWcfMaschinen)
                    {
                        if (_JgOpt.ListeMaschinen.ContainsKey(maWcf.Id))
                        {
                            var maMaschine = _JgOpt.ListeMaschinen[maWcf.Id];
                            if (maMaschine.Aenderung != maWcf.Aenderung)
                            {
                                speichern = true;
                                Helper.CopyObject<IJgMaschine, JgMaschineStamm>(maMaschine, maWcf);
                            }
                        }
                        else
                        {
                            speichern = true;
 
                            JgMaschineStamm maMaschine = null;
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

                            Helper.CopyObject<IJgMaschine, JgMaschineStamm>(maMaschine, maWcf);
                            _JgOpt.ListeMaschinen.Add(maMaschine.Id, maMaschine);
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
                    var maschinenTypes = new Type[] { typeof(JgMaschineHand), typeof(JgMaschineEvg), typeof(JgMaschineSchnell) };
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
                Type[] personTypes = { typeof(JgMaschineHand), typeof(JgMaschineEvg), typeof(JgMaschineSchnell) };
                var serializer = new XmlSerializer(typeof(JgMaschineStamm[]), personTypes);
                serializer.Serialize(writer, arSpeichern);
            }
        }
    }
}
