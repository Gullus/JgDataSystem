using JgLibHelper;
using JgWcfServiceLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JgScannerMaschineLib
{
    public class JgScannerMaschinen
    {
        private bool _StatusServerOk = false;
        private JgMaschineStamm[] arSpeichern;

        private const string _PathQueue = ".\\Private$\\JgMaschineVonScanner";

        private Dictionary<Guid, JgMaschineStamm> _ListeMaschinen = new Dictionary<Guid, JgMaschineStamm>();
        private Dictionary<Guid, JgDbBediener> _ListeDbBediener = new Dictionary<Guid, JgDbBediener>();

        public CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();
        private Task TaskScannerMaschine;
        private Task TaskClientServer;

        public string FileSetupMaschinen = null;
        public Guid? IdStandort = null;

        public JgScannerMaschinen()
        { }

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
            var optScannerMaschine = new StructOptionenScannerMaschine()
            {
                PathQueue = _PathQueue,
                ListeBediener = _ListeDbBediener,
                ListeMaschinen = _ListeMaschinen
            };
            var ctScannerMaschine = Cts.Token;

            TaskScannerMaschine = new Task((optScanner) =>
            {
                var opt = (StructOptionenScannerMaschine)optScanner;
                while (true)
                {
                    opt.ListeMaschinen.First().Value.Id = Guid.NewGuid();
                    //Console.WriteLine("ClientMaschine");
                    //Thread.Sleep(5);
                }


                //var sm = new ScannerMaschine(opt);

            }, optScannerMaschine, ctScannerMaschine, TaskCreationOptions.LongRunning);

            TaskScannerMaschine.Start();
        }

        public void TaskClientServerStarten()
        {
            var optClientServer = new StructOptionenClientServer()
            {
                PathQueue = _PathQueue,
                ListeMaschinen = _ListeMaschinen
            };
            var ctClientServer = Cts.Token;

            TaskClientServer = new Task((optClient) =>
            {
                var opt = (StructOptionenClientServer)optClient;
                while (true)
                {
                    //opt.ListeMaschinen.First().Value.Id = Guid.NewGuid();
                    Console.WriteLine("ClientServer " + opt.ListeMaschinen.First().Value.Id.ToString());
                    //Thread.Sleep(5);
                }

            }, optClientServer, ctClientServer, TaskCreationOptions.LongRunning);


            TaskClientServer.Start();
        }

        private void DatenMitServerAbgleichen()
        {
            #region Erstellung Maschine aus Datenbank laden simulieren

            var listeMaschinenVonServer = new List<JgWcfMaschine>()
            {
                new JgWcfMaschine()
                {
                    Id = Guid.NewGuid(),
                    MaschineName = "Maschine Elg",
                    MaschineArt = JgWcfMaschine.EnumArtMaschine.Elg,
                    MaschinePort = 100,
                    MaschineIp = "192.168.15.1",
                },
                new JgWcfMaschine()
                {
                    Id = Guid.NewGuid(),
                    MaschineName = "Maschine Arsch",
                    MaschineArt = JgWcfMaschine.EnumArtMaschine.Arsch,
                    MaschinePort = 200
                },
                new JgWcfMaschine()
                {
                    Id = Guid.NewGuid(),
                    MaschineName = "Hand",
                    MaschineArt = JgWcfMaschine.EnumArtMaschine.Hand,
                    MaschinePort = 300
                }
            };

            #endregion

            var speichern = false;

            foreach (var maschineDb in listeMaschinenVonServer)
            {
                if (_ListeMaschinen.ContainsKey(maschineDb.Id))
                {
                    var maVorhanden = _ListeMaschinen[maschineDb.Id];
                    if (maschineDb.Datum != maVorhanden.Datum)
                    {
                        speichern = true;
                        Helper.CopyObject<JgDbMaschine>(maVorhanden, maschineDb);
                    }
                }
                else
                {
                    speichern = true;

                    JgMaschineStamm maschineNeu = null;
                    switch (maschineDb.MaschineArt)
                    {
                        case JgDbMaschine.EnumArtMaschine.Hand:
                            maschineNeu = new JgMaschineHand();

                            #region Bediener und Helfer hinzufügen

                            maschineNeu.Bediener = new JgDbBediener()
                            {
                                BedienerName = "Hallo",
                                Id = Guid.NewGuid()
                            };
                            maschineNeu.Helfer = new List<JgDbBediener>()
                            {
                                new JgDbBediener() { Id = Guid.NewGuid(), BedienerName = "Juhu" },
                                new JgDbBediener() { Id = Guid.NewGuid(), BedienerName = "Schule" }
                            };

                            #endregion

                            break;
                        case JgDbMaschine.EnumArtMaschine.Arsch:
                            maschineNeu = new JgMaschineArsch();
                            break;
                        case JgDbMaschine.EnumArtMaschine.Elg:
                            maschineNeu = new JgMaschineEvg();
                            break;
                    }

                    Helper.CopyObject<JgDbMaschine>(maschineNeu, maschineDb);
                    _ListeMaschinen.Add(maschineDb.Id, maschineNeu);
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

                _ListeMaschinen.Clear();
                foreach (var ma in arMaschineStamm)
                    _ListeMaschinen.Add(ma.Id, ma);
            }
            catch (Exception ex)
            {

            }
        }

        private void MaschinenLocalSpeichern()
        {
            arSpeichern = new JgMaschineStamm[_ListeMaschinen.Count];
            _ListeMaschinen.Values.CopyTo(arSpeichern, 0);

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
