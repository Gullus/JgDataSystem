using AutoMapper;
using JgTestConsole.Temp;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JgTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var t2 = new C2();

            C1 t1 = (C1)t2;

            Console.WriteLine(t1.feld1);
            Console.WriteLine(t1.GetType());

            var t = new TAttr();
            
            

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

    public class C1 : I1
    {
        public int feld1 { get; set; } = 300;

    }

    public class C2 : C1, I2
    {
        public int feld2 { get; set; } = 400;

        public C2()
        {
            feld1 = 1000;
        }
    }


    class Test1 : I1, ICloneable
    {
        public int feld1 { get; set; }

        public object Clone()
        {
            return (I1)this.MemberwiseClone();
        }
    }

    class Test2 : Test1, I2
    {
        public int feld2 { get; set; }

        public int MyProperty1 { get; set; }
        public int MyProperty2 { get; set; }
    }

    public abstract class TestAttr : Attribute
    {
        public string Err1 { get; set; }
        public string Err2 { get; set; }
    }


    public class TAttr : TestAttr
    {
        public void TestInt()
        { }

    }

}
