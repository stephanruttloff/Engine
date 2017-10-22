using System;
using System.Globalization;

namespace Engine
{
    public struct Operand
    {
        public static readonly string Regex_Op_Id = @"([{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?)";
        public static readonly string Regex_Op_Id_Placeholder = @"\[{2}([0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12})\]{2}";

        public string Id { get; }
        public string Name { get; }
        public double Value { get; }

        public Operand(double value) : this()
        {
            Id = GetId();
            Name = Value.ToString(CultureInfo.InvariantCulture);
            Value = value;
        }

        public Operand(string name, double value) : this()
        {
            Id = GetId();
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"[[{Id}]]";
        }

        private static string GetId()
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }
}
