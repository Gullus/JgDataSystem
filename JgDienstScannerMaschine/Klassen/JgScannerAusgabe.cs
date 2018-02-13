using JgLibHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace JgDienstScannerMaschine
{
    public class JgScannerAusgabe
    {
        private char _Esc = Convert.ToChar(27);
        private string[] _Ausgabe = null;

        public string TextEmpfangen { get; set; }
        public string ScannerKennung { get => (TextEmpfangen.Length < 13) ? null : TextEmpfangen.Substring(0, 13); }

        public bool IstFehler = true;
        public bool MeldungAnMaschine = false;
        public string BvbsString = "";

        public bool ScannerMitDisplay { get; set; } = true;

        public ScannerProgram Program = ScannerProgram.SCVORGANG;

        public ScannerVorgang VorgangScan
        {
            get
            {
                var erg = ScannerVorgang.FEHLER;
                var scanVorgangText = TextEmpfangen.Substring(13, 4);

                if (!Enum.TryParse<ScannerVorgang>(scanVorgangText, true, out erg))
                {
                    JgLog.Set($"Scanner {ScannerKennung}. Sanvorgang konnte nicht ermittelt werden ({scanVorgangText}).", JgLog.LogArt.Fehler);
                    Set(false, true, "Scanvorgang falsch", scanVorgangText);
                }

                return erg;
            }
        }

        public string ScannKoerper
        {
            get
            {
                switch (VorgangScan)
                {
                    case ScannerVorgang.PROG:
                        return TextEmpfangen.Substring(26);
                    case ScannerVorgang.SCHALTER:
                        return TextEmpfangen[14].ToString();
                    case ScannerVorgang.MITA:
                        return TextEmpfangen.Substring(18); // Zeichen 17 ist 0 - für Anmeldung, 1 - für Abmeldung
                }
                return TextEmpfangen.Substring(17);
            }
        }

        // Programme ********************************************

        public JgScannerAusgabe(string TextEmpfangen)
        {
            // Steuerzeichen entfernen
            this.TextEmpfangen = TextEmpfangen.Substring(0, TextEmpfangen.Length - 1);
        }

        public byte[] AusgabeAufCraddle
        {
            get
            {
                var sb = new StringBuilder(ScannerKennung);

                if (ScannerMitDisplay)
                {
                    var lAusgabe = new List<string>(_Ausgabe);
                    if (IstFehler)
                        lAusgabe.Insert(0, ScannerTextCenter("- F E H L E R -"));

                    sb.Append(_Esc + "[2J");
                    foreach (var eintr in lAusgabe)
                        sb.Append(_Esc + "[0K" + ScannerTextCenter(eintr) + _Esc + "[G");
                }

                if (IstFehler)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        sb.Append(_Esc + "[4q");                       // Klingel
                        sb.Append(_Esc + "[8q");                       // Rote LED an
                        sb.Append(_Esc + "[5q" + _Esc + "[5q");        // 2 x 100 ms warten
                        sb.Append(_Esc + "[9q");                       // Rote LED aus
                        if (i < 3)
                            sb.Append(_Esc + "[5q" + _Esc + "[5q");      // 2 x 100 ms warten
                    }
                }

                sb.Append(Convert.ToChar(13));

                return Encoding.ASCII.GetBytes(sb.ToString());
            }
        }

        public void Set(bool MeldungAnMaschine, bool IstFehler, params string[] CraddleText)
        {
            this.MeldungAnMaschine = MeldungAnMaschine;
            this.IstFehler = IstFehler;
            _Ausgabe = CraddleText;
        }

        private string ScannerTextCenter(string Text)
        {
            if (Text.Length > 21)
                Text = Text.Substring(0, 21);

            if (Text.Length < 21)
            {
                var anz = Convert.ToInt32((21 - Text.Length) / 2);
                return "".PadLeft(anz) + Text;
            }
            return Text;
        }
    }
}
