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

        #endregion

        #region Operators

        public static string operator +(Operand left, Operand right)
        {
            return $"{left} + {right}";
        }

        public static string operator +(string left, Operand right)
        {
            return $"{left} + {right}";
        }

        public static string operator +(Operand left, string right)
        {
            return $"{left} + {right}";
        }

        public static Operand operator +(Operand left, double right)
        {
            var name = $"({left.Name} + {right.ToStringIG()})";
            var value = left.Value + right;
            return new Operand(name, value);
        }

        public static Operand operator +(double left, Operand right)
        {
            var name = $"({left.ToStringIG()} + {right.Name})";
            var value = left + right.Value;
            return new Operand(name, value);
        }

        public static string operator -(Operand left, Operand right)
        {
            return $"{left} - {right}";
        }

        public static string operator -(string left, Operand right)
        {
            return $"{left} - {right}";
        }

        public static string operator -(Operand left, string right)
        {
            return $"{left} - {right}";
        }

        public static Operand operator -(Operand left, double right)
        {
            var name = $"({left.Name} - {right.ToStringIG()})";
            var value = left.Value - right;
            return new Operand(name, value);
        }

        public static Operand operator -(double left, Operand right)
        {
            var name = $"({left.ToStringIG()} - {right.Name})";
            var value = left - right.Value;
            return new Operand(name, value);
        }

        public static string operator *(Operand left, Operand right)
        {
            return $"{left} * {right}";
        }

        public static string operator *(string left, Operand right)
        {
            return $"{left} * {right}";
        }

        public static string operator *(Operand left, string right)
        {
            return $"{left} * {right}";
        }

        public static Operand operator *(Operand left, double right)
        {
            var name = $"({left.Name} * {right.ToStringIG()})";
            var value = left.Value * right;
            return new Operand(name, value);
        }

        public static Operand operator *(double left, Operand right)
        {
            var name = $"({left.ToStringIG()} * {right.Name})";
            var value = left * right.Value;
            return new Operand(name, value);
        }

        public static string operator /(Operand left, Operand right)
        {
            return $"{left} / {right}";
        }

        public static string operator /(string left, Operand right)
        {
            return $"{left} / {right}";
        }

        public static string operator /(Operand left, string right)
        {
            return $"{left} / {right}";
        }

        public static Operand operator /(Operand left, double right)
        {
            var name = $"({left.Name} / {right.ToStringIG()})";
            var value = left.Value / right;
            return new Operand(name, value);
        }

        public static Operand operator /(double left, Operand right)
        {
            var name = $"({left.ToStringIG()} / {right.Name})";
            var value = left / right.Value;
            return new Operand(name, value);
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
