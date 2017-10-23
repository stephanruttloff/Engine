using System.Globalization;

namespace Engine
{
    internal static class ExtensionMethods
    {
        #region double

        public static string ToStringIG(this double value)
        {
            return value.ToString("G", CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
