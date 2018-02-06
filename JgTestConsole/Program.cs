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
            var s = "MyProperty1 = 59; MyProperty2 = 138";
            var t = new Test1();

            JgLibHelper.Helper.PropStringInOnjekt<Test1>(t, s);

            Console.WriteLine(t.MyProperty1 + "   " + t.MyProperty2);

            Console.ReadKey();

        }
    }

    class Test1
    {
        public int MyProperty1 { get; set; }
        public int MyProperty2 { get; set; }
    }
}
