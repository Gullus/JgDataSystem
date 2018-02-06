using JgLibHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JgDienstScannerMaschine
{
    public class JgScannerMaschinen
    {
        private bool _StatusServerOk = false;
        private JgOptionen _JgOpt;

        public CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();
        private Task TaskScannerMaschine;
        private Task TaskClientServer;

        public string FileSetupMaschinen = null;
        public Guid? IdStandort = null;

        public JgScannerMaschinen(JgOptionen JgOptionen)
        {
            _JgOpt = JgOptionen;
        }

        public void Init()
        {
            if (File.Exists(FileSetupMaschinen))
                MaschinenLocalLaden();

            _StatusServerOk = VerbindungServerOk();

            if (_StatusServerOk)
                DatenMitServerAbgleichen();

            TaskScannerMaschineStarten();
            TaskClientServerStarten();
        }

        public void TaskScannerMaschineStarten()
        {
            var ctScannerMaschine = Cts.Token;

            TaskScannerMaschine = new Task((optScanner) =>
            {
                var opt = (JgOptionen)optScanner;
                while (true)
                {
                    opt.ListeMaschinen.First().Value.Id = Guid.NewGuid();
                    //Console.WriteLine("ClientMaschine");
                    //Thread.Sleep(5);
                }


                //var sm = new ScannerMaschine(opt);

            }, _JgOpt, ctScannerMaschine, TaskCreationOptions.LongRunning);

            TaskScannerMaschine.Start();
        }

        public void TaskClientServerStarten()
        {
            var ctClientServer = Cts.Token;

            TaskClientServer = new Task((optClient) =>
            {
                var opt = (JgOptionen)optClient;
                while (true)
                {
                    //opt.ListeMaschinen.First().Value.Id = Guid.NewGuid();
                    Console.WriteLine("ClientServer " + opt.ListeMaschinen.First().Value.Id.ToString());
                    //Thread.Sleep(5);
                }

            }, _JgOpt, ctClientServer, TaskCreationOptions.LongRunning);


            TaskClientServer.Start();
        }

        private void DatenMitServerAbgleichen()
        {
            #region Erstellung Maschine aus Datenbank laden simulieren

            var listeMaschinenVonServer = new List<JgMaschineStamm>();
         

            #endregion

            var speichern = false;

            foreach (var maschineDb in listeMaschinenVonServer)
            {
                if (_JgOpt.ListeMaschinen.ContainsKey(maschineDb.Id))
                {
                    var maVorhanden = _JgOpt.ListeMaschinen[maschineDb.Id];
                    if (maschineDb.Aenderung != maVorhanden.Aenderung)
                    {
                        speichern = true;
                        // Helper.CopyObject<JgMaschineStamm>(maVorhanden, maschineDb);
                    }
                }
                else
                {
                    speichern = true;

                    JgMaschineStamm maschineNeu = null;
                    switch (maschineDb.MaschineArt)
                    {
                        case MaschinenArten.Hand:
                            maschineNeu = new JgMaschineHand();

                            #region Bediener und Helfer hinzufügen

                            maschineNeu.Bediener = new JgBediener()
                            {
                                BedienerName = "Hallo",
                                Id = Guid.NewGuid()
                            };
                            maschineNeu.Helfer = new List<JgBediener>()
                            {
                                new JgBediener() { Id = Guid.NewGuid(), BedienerName = "Juhu" },
                                new JgBediener() { Id = Guid.NewGuid(), BedienerName = "Schule" }
                            };

                            #endregion

                            break;
                        case MaschinenArten.Arsch:
                            maschineNeu = new JgMaschineArsch();
                            break;
                        case MaschinenArten.Evg:
                            maschineNeu = new JgMaschineEvg();
                            break;
                    }

                    Helper.CopyObject<JgMaschineStamm>(maschineNeu, maschineDb);
                    _JgOpt.ListeMaschinen.Add(maschineDb.Id, maschineNeu);
                }

            }

            if (speichern)
                MaschinenLocalSpeichern();
        }

        private void MaschinenLocalLaden()
        {
            try
            {
                JgMaschineStamm[] arMaschineStamm = null;
                using (var reader = new StreamReader(FileSetupMaschinen))
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

        private void MaschinenLocalSpeichern()
        {
            var arSpeichern = new JgMaschineStamm[_JgOpt.ListeMaschinen.Count];
            _JgOpt.ListeMaschinen.Values.CopyTo(arSpeichern, 0);

            using (var writer = new StreamWriter(FileSetupMaschinen))
            {
                Type[] personTypes = { typeof(JgMaschineHand), typeof(JgMaschineEvg), typeof(JgMaschineArsch) };
                var serializer = new XmlSerializer(typeof(JgMaschineStamm[]), personTypes);
                serializer.Serialize(writer, arSpeichern);
            }
        }

        public bool VerbindungServerOk()
        {
            return true;
        }
    }
}
