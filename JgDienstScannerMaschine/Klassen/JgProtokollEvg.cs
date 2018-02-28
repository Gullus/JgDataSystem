using System;

namespace JgDienstScannerMaschine
{
    public class JgProtokollEvg : JgProtokollStamm
    {
        internal string[] _ArDaten;
        private StructDatenZeit _MerkeDs = null;

        public JgProtokollEvg()
        { }

        public override void DatenEinlesen(string[] TextDaten, DateTime? DatumDaten = null)
        {

            foreach (var ds in TextDaten)
            {
                _ArDaten = ds.Split(new char[] { ';' });

                switch (_ArDaten[1][0])
                {
                    case 'A':
                        if (_MerkeDs != null)
                            ListeDatenZeit.Add(_MerkeDs);

                        var protBuegel = new JgProtokollEvgBuegel(_ArDaten);
                        _MerkeDs = new StructDatenZeit()
                        {
                            Kennzeichen = protBuegel.Buegelname,
                            StartZeit = DatumDaten.Value + protBuegel.Uhrzeit
                        };

                        break;
                    case 'D':
                        var protProd = new JgProtokollEvgProduktion(_ArDaten);
                        _MerkeDs.EndZeit = DatumDaten.Value + protProd.Uhrzeit;

                        break;
                        //case 'Z':
                        //    _Ds = new JgProtokollEvgAutomatikStartStop(_ArDaten);
                        //    break;
                        //case 'C':
                        //    _Ds = new JgProtokollEvgFehler(_ArDaten);
                        //    break;
                        //case 'U':
                        //    _Ds = new JgProtokollEvgBenutzer(_ArDaten);
                        //    break;
                }
            }
        }
    }

    public class JgProtokollEvgBase : JgProtokollBase // A
    {
        public TimeSpan Uhrzeit
        {
            get => new TimeSpan(Convert.ToInt32(_ArDaten[0].Substring(0, 2)), Convert.ToInt32(_ArDaten[0].Substring(2, 2)), Convert.ToInt32(_ArDaten[0].Substring(4, 2)));
        }

        public JgProtokollEvgBase(string[] MyDaten)
         : base(MyDaten)
        { }
    }

    // Geometrie wird nicht berücksichtigt

    public class JgProtokollEvgBuegel : JgProtokollEvgBase // A
    {
        public int IstStueckZahl { get => Convert.ToInt32(_ArDaten[2]); }
        public string Buegelname { get => _ArDaten[3]; }
        public int Drahtduchmesser { get => Convert.ToInt32(_ArDaten[4]); }
        public int GesamtLaenge { get => Convert.ToInt32(_ArDaten[5]); }
        public string Kommission { get => _ArDaten[6]; }
        public string Position { get => _ArDaten[7]; }
        public string ProdiktionslistenName { get => _ArDaten[8]; }
        public string PgmVersion { get => _ArDaten[9]; }
        public bool IstInch { get => _ArDaten[10][0] == '1'; }  // Ja = 1
        public int BiegeKopf { get => Convert.ToInt32(_ArDaten[11]); }
        public int Wiederholungen { get => Convert.ToInt32(_ArDaten[12]); }

        public JgProtokollEvgBuegel(string[] MyDaten)
            : base(MyDaten)
        { }
    }

    public class JgProtokollEvgProduktion : JgProtokollEvgBase // D
    {
        public int AnzahlDraehte { get => Convert.ToInt32(_ArDaten[2]); }

        public JgProtokollEvgProduktion(string[] MyDaten)
            : base(MyDaten)
        { }
    }

    public class JgProtokollEvgAutomatikStartStop : JgProtokollEvgBase // Z
    {
        public bool IstStart { get => _ArDaten[2][0] == '1'; }

        public JgProtokollEvgAutomatikStartStop(string[] MyDaten)
            : base(MyDaten)
        { }
    }

    public class JgProtokollEvgFehler : JgProtokollEvgBase // C
    {
        public int Fehler { get => Convert.ToInt32(_ArDaten[2]); }
        public int SubFehler { get => Convert.ToInt32(_ArDaten[3]); }

        public JgProtokollEvgFehler(string[] MyDaten)
            : base(MyDaten)
        { }
    }

    public class JgProtokollEvgBenutzer : JgProtokollEvgBase // U
    {
        public int NummerBenutzer { get => Convert.ToInt32(_ArDaten[2]); }

        public JgProtokollEvgBenutzer(string[] MyDaten)
            : base(MyDaten)
        { }
    }
}
