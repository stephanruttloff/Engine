using System.Linq;

namespace Engine
{
    internal enum Operations
    {
        [Description(@"+")]
        Add,
        [Description(@"-")]
        Subtract,
        [Description(@"*")]
        Multiply,
        [Description(@"/")]
        Divide,
        [Description(@"Max")]
        Max,
        [Description(@"Min")]
        Min,
        [Description(@"%")]
        Remainder,
        [Description(@"Round")]
        Round,
        [Description(@"Floor")]
        Floor,
        [Description(@"Ceiling")]
        Ceiling
    }

    internal static class OperationsExtensionMethods
    {
        public static string Description(this Operations operation)
        {
            var type = typeof(Operations);
            var memberInfo = type.GetMember(operation.ToString());
            var attribute = memberInfo.FirstOrDefault()?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            var description = attribute?.Value;

            return description;
        }
    }
}
