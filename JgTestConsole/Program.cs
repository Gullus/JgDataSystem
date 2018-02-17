﻿using AutoMapper;
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

            var c = new char[2] {  'g', 'a' };
            Console.WriteLine(c);

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

    public class C1 : I1, ICloneable
    {
        public int feld1 { get; set; } = 300;

        public object Clone()
        {
            return (I1)this.MemberwiseClone();
        }
    }

    public class C2 : I1
    {
        public int feld1 { get; set; } = 500;
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
}
