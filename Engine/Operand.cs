using System;
using System.Globalization;

namespace Engine
{
    public struct Operand
    {
        #region Fields

        public static readonly string Regex_Op_Id = @"([{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?)";
        public static readonly string Regex_Op_Id_Placeholder = @"\[{2}([0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12})\]{2}";

        #endregion

        #region Properties

        public string Id { get; }
        public string Name { get; }
        public double Value { get; }
        public Origin Origin { get; }

        #endregion

        #region Constructor

        public Operand(double value) : this()
        {
            Id = GetId();
            Name = value.ToStringIG();
            Value = value;
        }

        public Operand(string name, double value) : this()
        {
            Id = GetId();
            Name = name;
            Value = value;
        }

        private Operand(string name, double value, Origin origin) : this()
        {
            Id = GetId();
            Name = name;
            Value = value;
            Origin = origin;
        }

        #endregion

        #region Operators

        public static Operand operator +(Operand left, Operand right)
        {
            var algorithm = $"({left} + {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value + right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator -(Operand left, Operand right)
        {
            var algorithm = $"({left} - {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value - right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator *(Operand left, Operand right)
        {
            var algorithm = $"({left} * {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value * right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator /(Operand left, Operand right)
        {
            var algorithm = $"({left} / {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value / right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator +(double leftValue, Operand right)
        {
            var left = new Operand(leftValue);
            var algorithm = $"({left} + {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value + right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator +(Operand left, double rightValue)
        {
            var right = new Operand(rightValue);
            var algorithm = $"({left} + {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value + right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator -(double leftValue, Operand right)
        {
            var left = new Operand(leftValue);
            var algorithm = $"({left} - {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value - right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator -(Operand left, double rightValue)
        {
            var right = new Operand(rightValue);
            var algorithm = $"({left} - {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value - right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator *(double leftValue, Operand right)
        {
            var left = new Operand(leftValue);
            var algorithm = $"({left} * {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value * right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator *(Operand left, double rightValue)
        {
            var right = new Operand(rightValue);
            var algorithm = $"({left} * {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value * right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator /(double leftValue, Operand right)
        {
            var left = new Operand(leftValue);
            var algorithm = $"({left} / {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value / right.Value;

            return new Operand(name, value, origin);
        }

        public static Operand operator /(Operand left, double rightValue)
        {
            var right = new Operand(rightValue);
            var algorithm = $"({left} / {right})";
            var name = Evaluator.GetFormula(algorithm, left, right);
            var origin = new Origin(algorithm, left, right);
            var value = left.Value / right.Value;

            return new Operand(name, value, origin);
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"[[{Id}]]";
        }

        private static string GetId()
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }

        #endregion
    }
}
