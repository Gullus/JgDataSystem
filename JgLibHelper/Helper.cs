using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection;

namespace JgLibHelper
{
    public static class Helper
    {
        public static void PropStringInOnjekt<T>(object InObject, string AusString)
        {
            var type = typeof(T);
            var arWertePaar = AusString.Split(new char[] { ';' });
            var Werte = new Dictionary<string, object>();
            foreach (var paar in arWertePaar)
            {
                var arWerte = paar.Split(new char[] { '=' });
                if (arWerte.Length == 2)
                    Werte.Add(arWerte[0].Trim().ToUpper(), arWerte[1].Trim());
            }

            foreach (var info in type.GetProperties())
            {
                var key = info.Name.ToUpper();
                if (Werte.ContainsKey(key))
                {
                    var wert = Werte[key].ToString();
                    if (wert != "")
                    {
                        if (info.PropertyType == typeof(int))
                            info.SetValue(InObject, Convert.ToInt32(wert));
                        else
                            info.SetValue(InObject, wert);
                    }
                }
            }
        }

        public static bool IstPingOk(string IpAdresse, out string Fehlertext)
        {
            Fehlertext = "Ping Ok";

            if (string.IsNullOrWhiteSpace(IpAdresse))
            {
                Fehlertext = "Ip Adresse ist leer!";
            }
            else
            {
                var sender = new Ping();
                PingReply result = null;
                try
                {
                    result = sender.Send(IpAdresse);

                    if (result.Status == IPStatus.Success)
                        return true;
                    else
                        Fehlertext = $"Bing {IpAdresse} mit Status {result.Status}  fehlgeschlagen!";
                }
                catch (Exception f)
                {
                    Fehlertext = $"Fehler bei Pingabfrage zu Adresse {IpAdresse}!\nGrund: {f.Message}";
                }
            }
            return false;
        }
    }
}