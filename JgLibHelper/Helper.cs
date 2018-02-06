using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void CopyObject<T>(object InObject, T VonObject)
        {
            var typeVon = typeof(T);
            var typeIn = InObject.GetType();

            var propertiesIn = typeIn.GetProperties().Select(s => s.Name).ToList();

            foreach (var propVon in typeVon.GetProperties())
            {
                if (propertiesIn.Contains(propVon.Name))
                {
                    var propIn = typeIn.GetProperty(propVon.Name);
                    propIn.SetValue(InObject, propVon.GetValue(VonObject));
                }
            }
        }
    }
}
