using System;

namespace Engine
{
    public struct Operand
    {
        #region Fields

        public static readonly string Regex_Op_Id = @"([{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?)";
        public static readonly string Regex_Op_Id_Placeholder = @"\[{2}([0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12})\]{2}";

        #endregion

        #region Properties

        /// <summary>
        /// ID of this operand
        /// </summary>
        internal string Id { get; }
        /// <summary>
        /// Name of this operand
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Value of this operand
        /// </summary>
        public double Value { get; }
        /// <summary>
        /// Origin ot this operand (if is result of calculation)
        /// </summary>
        internal Origin Origin { get; }
        /// <summary>
        /// True, if this operand is the result of a calculation
        /// </summary>
        public bool HasOrigin { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Create an operand without a name. Name will be equal to value.
        /// </summary>
        /// <param name="value">Value of this operand.</param>
        public Operand(double value) : this()
        {
            Id = GetId();
            Name = value.ToStringIG();
            Value = value;
        }

        /// <summary>
        /// Create an operand with name and value.
        /// </summary>
        /// <param name="name">Name of this operand.</param>
        /// <param name="value">Value of this operand.</param>
        public Operand(string name, double value) : this()
        {
            Id = GetId();
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Create an operand with name, value and origin.
        /// </summary>
        /// <param name="name">Name of this operand.</param>
        /// <param name="value">Value of this operand.</param>
        /// <param name="origin">Origin of this operand.</param>
        private Operand(string name, double value, Origin origin) : this()
        {
            Id = GetId();
            Name = name;
            Value = value;
            Origin = origin;
            HasOrigin = true;
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
