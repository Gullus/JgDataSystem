using JgLibDataModel;
using JgTestConsole.Temp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            var ein = new StandortMaschineEintragen();

            Console.WriteLine("Fertig");
            Console.ReadKey();
        }
    }

    class Test1
    {
        public int MyProperty1 { get; set; }
        public int MyProperty2 { get; set; }
    }
}
