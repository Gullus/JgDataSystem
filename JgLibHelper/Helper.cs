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
            
            foreach(var info in type.GetProperties())
            {
                var key = info.Name.ToUpper();
                if (Werte.ContainsKey(key))
                {
                    if (info.PropertyType == typeof(int))
                        info.SetValue(InObject, Convert.ToInt32(Werte[key]));
                    else
                        info.SetValue(InObject, Werte[key].ToString());                   
                }

            }
        }

        public static void CopyObject<T>(object InObject, T VonObject)
        {
            var typeVon = typeof(T);
            var typeIn = InObject.GetType();

            foreach (var propVon in typeVon.GetProperties())
            {
                var propIn = typeIn.GetProperty(propVon.Name);
                propIn.SetValue(InObject, propVon.GetValue(VonObject));
            }
        }
    }
}
