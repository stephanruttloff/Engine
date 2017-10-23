using System;
using Engine;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Constants();
            var var1 = new Operand(0.3);
            var var2 = var1 + 2;
            var fSimple = c.A + c.B / c.C + var1 + var2;

            Console.WriteLine($"{nameof(fSimple)}:");
            Console.WriteLine(Evaluator.GetFormula(fSimple, c, var1, var2));
            Console.WriteLine(Evaluator.InjectValues(fSimple, c, var1, var2));
            Console.WriteLine(Evaluator.Evaluate(fSimple, c, var1, var2));

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
