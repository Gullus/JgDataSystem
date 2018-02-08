using JgLibHelper;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    public class JgScannerAuswertung
    {
        private enum VorgangScanner
        {
            CRADDLEANMELDUNG,
            FEHLER,
            PROG,
            MITA,       // Mitarbeiter Anmeldung an Maschine  
            BF2D,
            BF3D,
            TEST,
            SCHALTER,
            VERBUNDEN
        }



        private JgOptionenCraddle _JgOpt;
        private string _TextEmpfangen = "";
        private char _Esc = Convert.ToChar(27);

        private bool ScannerIstMitDisplay = false;

        private string _ScannerKennung => (_TextEmpfangen.Length < 13) ? null : _TextEmpfangen.Substring(0, 13);
        private string _ScannerVorgangScan => (_TextEmpfangen.Length < 17) ? null : _TextEmpfangen.Substring(13, 4);
        private string _ScannerVorgangProgramm => (_TextEmpfangen.Length < 26) ? null : _TextEmpfangen.Substring(17, 9);

        private string ScannerKoerper
        {
            get
            {
                switch (_VorgangScan)
                {
                    case VorgangScanner.PROG:
                        return _TextEmpfangen.Substring(26);
                    case VorgangScanner.SCHALTER:
                        return _TextEmpfangen[14].ToString();
                    case VorgangScanner.MITA:  // Zeichen 17 ist 0 - für Anmeldung, 1 - für Abmeldung
                        return _TextEmpfangen.Substring(18);
                    default:
                        return _TextEmpfangen.Substring(17);       // bei allen anderen Vorgängen
                }
            }
        }

        private VorgangScanner _VorgangScan = VorgangScanner.FEHLER;
        private VorgangProgram _VorgangProgramm = VorgangProgram.FEHLER;

        private string _ZusatzText = "";
        private string[] _AusgabeZeilen = { " ", " ", " ", " ", " " };
        private string _AlarmMeldungsTon = null;

        public bool CraddeFehler { get; internal set; }

        public JgScannerAuswertung(JgOptionenCraddle CraddelOptionen)
        {
            _JgOpt = CraddelOptionen;
        }

        public void TextEmpfangen(string textEmpfangen)
        {
            CraddeFehler = true;

            if (textEmpfangen == _JgOpt.TextBeiFehler)
                Logger.Write($"{_JgOpt.Info} -> Fehlertext angesprochen.", "Service", 1, 0, System.Diagnostics.TraceEventType.Warning);
            else if (textEmpfangen.Length == 1)
                Logger.Write($"{_JgOpt.Info} -> Ein Zeichen Empfangen: {Convert.ToByte(textEmpfangen[0])}", "Service", 1, 0, System.Diagnostics.TraceEventType.Warning);
            else if (textEmpfangen.Length < 1)
                Logger.Write($"{_JgOpt.Info} -> Leeres Zeichen Empfangen!", "Service", 1, 0, System.Diagnostics.TraceEventType.Warning);
            else
            {
                CraddeFehler = false;

                // Letztes Zeichen, Seuerzeichen entfernen

                _TextEmpfangen = textEmpfangen.Substring(0, textEmpfangen.Length - 1);
                _VorgangScan = VorgangScanner.FEHLER;
                _VorgangProgramm = VorgangProgram.FEHLER;
                _ZusatzText = "";
                _AusgabeZeilen = new string[] { " ", " ", " ", " ", " " };

                if ((this._TextEmpfangen.Length == 16) && (this._TextEmpfangen[13] == 'S'))
                {
                    _VorgangScan = VorgangScanner.SCHALTER;
                    Logger.Write($"Schalter getrückt: {ScannerKoerper}", "Service", 0, 0, System.Diagnostics.TraceEventType.Verbose);
                }
                else if (this._TextEmpfangen.Length < 17)
                    FehlerAusgabe("Text zu kurz");
                else
                {
                    if (!Enum.TryParse<VorgangScanner>(_ScannerVorgangScan, true, out _VorgangScan))
                        FehlerAusgabe("Scan Vorgang falsch.", "Wert: " + _ScannerVorgangScan);
                    else
                    {
                        switch (_VorgangScan)
                        {
                            case VorgangScanner.BF2D: _VorgangProgramm = VorgangProgram.BAUTEIL; break;
                            case VorgangScanner.FEHLER: _VorgangProgramm = VorgangProgram.FEHLER; break;
                            case VorgangScanner.SCHALTER: _VorgangProgramm = VorgangProgram.SCHALTER; break;
                            case VorgangScanner.TEST: _VorgangProgramm = VorgangProgram.TEST; break;
                            case VorgangScanner.MITA:
                                if (_TextEmpfangen[17] == '0')
                                    _VorgangProgramm = VorgangProgram.ANMELDUNG;
                                else
                                    _VorgangProgramm = VorgangProgram.ABMELDUNG;
                                break;
                            case VorgangScanner.PROG:
                                if (!Enum.TryParse<VorgangProgram>(_ScannerVorgangProgramm, true, out _VorgangProgramm))
                                    FehlerAusgabe("Prog. Vorgang falsch.", "Wert: " + _ScannerVorgangProgramm);
                                break;
                        }
                    }
                }
            }
        }

        public byte[] PufferSendeText()
        {
            var sb = new StringBuilder(_ScannerKennung);

            if (ScannerIstMitDisplay)
            {
                sb.Append(_Esc + "[2J");
                for (int i = 0; i < _AusgabeZeilen.Length; i++)
                {
                    sb.Append(_Esc + "[0K" + (_AusgabeZeilen[i].Length > 22 ? _AusgabeZeilen[i].Substring(0, 22) : _AusgabeZeilen[i]) + _Esc + "[G");
                    if (i < _AusgabeZeilen.Length - 1)
                        sb.Append(_Esc + "[G");
                }
            }

            sb.Append(_ZusatzText);
            sb.Append(Convert.ToChar(13));

            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        public string TextCenter(string Text)
        {
            if (Text.Length > 24)
                Text = Text.Substring(0, 24);

            if (Text.Length < 23)
            {
                var anz = Convert.ToInt32((22 - Text.Length) / 2);
                return "".PadLeft(anz) + Text;
            }
            return Text;
        }

        public void FehlerAusgabe(params string[] FehlerText)
        {
            _AusgabeZeilen[0] = TextCenter("- F E H L E R -");

            for (int i = 0; i < FehlerText.Length; i++)
                _AusgabeZeilen[i + 1] = TextCenter(FehlerText[i]);

            _ZusatzText = _AlarmMeldungsTon;
        }
    }
}
