using JgTestConsole.Temp;
using System;

namespace JgTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            var zk = new ZweiteKlasse();

            Console.WriteLine(zk.WertString);

            zk.WertString = "Arsch";

            Console.WriteLine(zk.WertString);

            Console.WriteLine("Fertig");
            Console.ReadKey();
        }
    }


    public interface I1
    {
        int feld1 { get; set; } 
    }

    public interface I2 : I1
    {
        int feld2 { get; set; } 
    }

    class Test1 : I1
    {
        public int feld1 { get; set; }
    }

    class Test2 : Test1, I2
    {
        public int feld2 { get; set; }

        public int MyProperty1 { get; set; }
        public int MyProperty2 { get; set; }
    }

    public class ErsteKlasse
    {
        public int Wert1 = 500;
    }

    public class ZweiteKlasse : ErsteKlasse
    {
        public string WertString = "Hallo";

        public ZweiteKlasse()
        {
            WertString = Wert1.ToString();
        }
    }

}
