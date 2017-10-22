using System;
using Engine;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Constants();
            var f = $"{c.A}+({c.B}/{c.C})+0,3";

            Console.WriteLine(Evaluator.GetFormula(f, c));
            Console.WriteLine(Evaluator.InjectValues(f, c));
            Console.WriteLine(Evaluator.Evaluate(f, c));

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
