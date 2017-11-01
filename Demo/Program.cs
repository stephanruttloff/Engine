﻿using System;
using System.Linq;
using Engine;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var var1 = new Operand(@"var1", 1);
            var var2 = new Operand(@"var2", 2.22);
            var var3 = new Operand(@"Pi", Math.PI);

            var result = (var1 + var2) * var3;

            Console.WriteLine(@"Formula:  " + result.GetFormula());
            Console.WriteLine(@"Injected: " + result.GetFormula(true));
            Console.WriteLine(@"Result:   " + result.Value);

            Console.WriteLine();

            result = Operand.Max(var1, Operand.Min(var2, var3));

            Console.WriteLine(@"Formula:  " + result.GetFormula());
            Console.WriteLine(@"Injected: " + result.GetFormula(true));
            Console.WriteLine(@"Result:   " + result.Value);

            Console.WriteLine();

            result = Operand.Round(Operand.Floor(var1 * var2 / var3) / Operand.Ceiling(var1 * var2 / var3));

            Console.WriteLine(@"Formula:  " + result.GetFormula());
            Console.WriteLine(@"Injected: " + result.GetFormula(true));
            Console.WriteLine(@"Result:   " + result.Value);

            Console.WriteLine();

            result = Operand.Round(Operand.Floor(var1 * var2 / var3).AsConstant() / Operand.Ceiling(var1 * var2 / var3).AsConstant());

            Console.WriteLine(@"Formula:  " + result.GetFormula());
            Console.WriteLine(@"Injected: " + result.GetFormula(true));
            Console.WriteLine(@"Result:   " + result.Value);

            Console.WriteLine();

            result = var3 % (2 + 1);

            Console.WriteLine(@"Formula:  " + result.GetFormula());
            Console.WriteLine(@"Injected: " + result.GetFormula(true));
            Console.WriteLine(@"Result:   " + result.Value);

            Console.WriteLine();

            Console.ReadKey(false);
        }
    }
}
