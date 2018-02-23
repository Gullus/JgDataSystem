using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Serialization;

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

        public static string GetExcept(Exception Ex)
        {
            var sb = new StringBuilder(Ex.Message);
            var ex = Ex.InnerException;
            while (ex != null)
            {
                sb.AppendLine("\nInnerException:");
                sb.AppendLine("  " + ex.Message);
                ex = ex.InnerException;
            }

            return sb.ToString();
        }

        public static T ByteDatenXmlInObjekt<T>(byte[] Daten)
        {
            try
            {
                using (var mem = new MemoryStream(Daten))
                {
                    using (var reader = new StreamReader(mem))
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        return (T)serializer.Deserialize(reader);
                    }
                }
            }
            catch
            { }

            return default(T);
        }

        public static byte[] ObjectInXmlDatenByte<T>(object Objekt, Type[] Typen = null)
        {
            using (var mem = new MemoryStream())
            {
                using (var writer = new StreamWriter(mem))
                {
                    var serializer = new XmlSerializer(typeof(T), Typen);
                    serializer.Serialize(writer, Objekt);
                    return mem.ToArray();
                }
            }
        }
 
        public static T XmlDateiInObjekt<T>(string DataName, Type[] Typen = null)
        {
            if (File.Exists(DataName))
            {
                using (var reader = new StreamReader(DataName))
                {
                    var serializer = new XmlSerializer(typeof(T), Typen);
                    return (T)serializer.Deserialize(reader);
                }
            }

            return default(T);
        }

        public static void ObjektInXmlDatei<T>(object Objekt, string DataName, Type[] Typen = null)
        {
            using (var writer = new StreamWriter(DataName))
            {
                var serializer = new XmlSerializer(typeof(Object), Typen);
                serializer.Serialize(writer, Objekt);
            }
        }
    }
}
