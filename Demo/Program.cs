using System;
using Engine;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Constants();
            var var1 = new Operand(@"var1", 42);
            var var2 = new Operand(@"var2", 23);

            var simple1 = c.A + 2 / c.B - var1 * var2;
            var simple2 = (c.A + 2) / c.B - var1 * var2;

            var fComplex = $"{c.A} + ({c.B} / {c.C}) + (0,3 / {var1}) * {var2}";

            Console.WriteLine($"{nameof(fComplex)}:");
            Console.WriteLine(Evaluator.GetFormula(fComplex, c, var1, var2));
            Console.WriteLine(Evaluator.InjectValues(fComplex, c, var1, var2));
            Console.WriteLine(Evaluator.Evaluate(fComplex, c, var1, var2));

            Console.ReadKey(false);
        }
    }

    class Constants
    {
        [Constant]
        public Operand A = new Operand(@"A", 100);
        [Constant]
        public Operand B = new Operand(@"B", 200);
        [Constant]
        public Operand C = new Operand(@"C", 300);
    }
}
