using System;
using System.Data;
using System.Globalization;

namespace Engine
{
    public struct Operand
    {
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
        /// True, if this is NOT the result of calculation
        /// </summary>
        public bool IsConstant { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Create an operand without a name. Name will be equal to value.
        /// </summary>
        /// <param name="value">Value of this operand.</param>
        public Operand(double value) : this()
        {
            Id = GetId();
            Name = value.ToString("G", CultureInfo.InvariantCulture);
            Value = value;
            IsConstant = true;
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
            IsConstant = true;
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
            IsConstant = false;
        }

        #endregion

        #region Operators

        public static Operand operator +(Operand left, Operand right)
        {
            return Add(left, right);
        }

        public static Operand operator +(double leftValue, Operand right)
        {
            return Add(new Operand(leftValue), right);
        }

        public static Operand operator +(Operand left, double rightValue)
        {
            return Add(left, new Operand(rightValue));
        }

        public static Operand operator -(Operand left, Operand right)
        {
            return Subtract(left, right);
        }

        public static Operand operator -(double leftValue, Operand right)
        {
            return Subtract(new Operand(leftValue), right);
        }

        public static Operand operator -(Operand left, double rightValue)
        {
            return Subtract(left, new Operand(rightValue));
        }

        public static Operand operator *(Operand left, Operand right)
        {
            return Multiply(left, right);
        }

        public static Operand operator *(double leftValue, Operand right)
        {
            return Multiply(new Operand(leftValue), right);
        }

        public static Operand operator *(Operand left, double rightValue)
        {
            return Multiply(left, new Operand(rightValue));
        }

        public static Operand operator /(Operand left, Operand right)
        {
            return Divide(left, right);
        }

        public static Operand operator /(double leftValue, Operand right)
        {
            return Divide(new Operand(leftValue), right);
        }

        public static Operand operator /(Operand left, double rightValue)
        {
            return Divide(left, new Operand(rightValue));
        }

        public static Operand operator %(Operand left, Operand right)
        {
            return Remainder(left, right);
        }

        public static Operand operator %(Operand left, double rightValue)
        {
            return Remainder(left, new Operand(rightValue));
        }

        public static Operand operator %(double leftValue, Operand right)
        {
            return Remainder(new Operand(leftValue), right);
        }

        #endregion

        #region Methods

        public string GetFormula(bool injectValue = false)
        {
            if (IsConstant)
                return injectValue ? $"{Value.ToString(@"G", CultureInfo.InvariantCulture)}" : $"{Name ?? Value.ToString(@"G", CultureInfo.InvariantCulture)}";

            if (!Origin.Binary)
                return $"{Origin.Operation.Description()}({Origin.Left.GetFormula(injectValue)})";

            if (Origin.Operation == Operations.Multiply ||
                Origin.Operation == Operations.Divide ||
                Origin.Operation == Operations.Remainder)
            {
                var formula = string.Empty;

                if (Origin.Left.IsConstant)
                {
                    formula += $"{Origin.Left.GetFormula(injectValue)}";
                }
                else
                {
                    var leftOperation = Origin.Left.Origin.Operation;
                    if (leftOperation == Operations.Add ||
                        leftOperation == Operations.Subtract)
                        formula += $"({Origin.Left.GetFormula(injectValue)})";
                    else
                        formula += $"{Origin.Left.GetFormula(injectValue)}";
                }

                formula += $" {Origin.Operation.Description()} ";

                if (Origin.Right.IsConstant)
                {
                    formula += $"{Origin.Right.GetFormula(injectValue)}";
                }
                else
                {
                    var rightOperation = Origin.Right.Origin.Operation;
                    if (rightOperation == Operations.Add ||
                        rightOperation == Operations.Subtract)
                        formula += $"({Origin.Right.GetFormula(injectValue)})";
                    else
                        formula += $"{Origin.Right.GetFormula(injectValue)}";
                }

                return formula;
            }

            if (Origin.Operation == Operations.Min ||
                Origin.Operation == Operations.Max)
                return $"{Origin.Operation.Description()}({Origin.Left.GetFormula(injectValue)}, {Origin.Right.GetFormula(injectValue)})";

            return $"{Origin.Left.GetFormula(injectValue)} {Origin.Operation.Description()} {Origin.Right.GetFormula(injectValue)}";
        }

        public Operand AsConstant()
        {
            return new Operand(Name, Value);
        }

        public Operand AsConstant(string name)
        {
            return new Operand(name, Value);
        }

        public override string ToString()
        {
            return $"[[{Id}]]";
        }

        public static Operand Min(Operand left, Operand right)
        {
            var value = Math.Min(left.Value, right.Value);
            var origin = new Origin(left, right, Operations.Min);

            return new Operand(null, value, origin);
        }

        public static Operand Max(Operand left, Operand right)
        {
            var value = Math.Max(left.Value, right.Value);
            var origin = new Origin(left, right, Operations.Max);

            return new Operand(null, value, origin);
        }

        public static Operand Round(Operand operand)
        {
            var value = Math.Round(operand.Value);
            var origin = new Origin(operand, new Operand(), Operations.Round);

            return new Operand(null, value, origin);
        }

        public static Operand Floor(Operand operand)
        {
            var value = Math.Floor(operand.Value);
            var origin = new Origin(operand, new Operand(), Operations.Floor);

            return new Operand(null, value, origin);
        }

        public static Operand Ceiling(Operand operand)
        {
            var value = Math.Ceiling(operand.Value);
            var origin = new Origin(operand, new Operand(), Operations.Ceiling);

            return new Operand(null, value, origin);
        }

        private static string GetId()
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }

        private static Operand Add(Operand left, Operand right)
        {
            var value = left.Value + right.Value;
            var origin = new Origin(left, right, Operations.Add);

            return new Operand(null, value, origin);
        }

        private static Operand Subtract(Operand left, Operand right)
        {
            var value = left.Value - right.Value;
            var origin = new Origin(left, right, Operations.Subtract);

            return new Operand(null, value, origin);
        }

        private static Operand Multiply(Operand left, Operand right)
        {
            var value = left.Value * right.Value;
            var origin = new Origin(left, right, Operations.Multiply);

            return new Operand(null, value, origin);
        }

        private static Operand Divide(Operand left, Operand right)
        {
            var value = left.Value / right.Value;
            var origin = new Origin(left, right, Operations.Divide);

            return new Operand(null, value, origin);
        }

        private static Operand Remainder(Operand left, Operand right)
        {
            var value = left.Value % right.Value;
            var origin = new Origin(left, right, Operations.Remainder);

            return new Operand(null, value, origin);
        }

        #endregion
    }
}
