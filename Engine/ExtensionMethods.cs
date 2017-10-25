using System.Globalization;

namespace Engine
{
    internal static class ExtensionMethods
    {
        #region double

        /// <summary>
        /// Convert double to invariant culture, using 'G' format string
        /// </summary>
        /// <param name="value">Double value</param>
        /// <returns></returns>
        internal static string ToStringIG(this double value)
        {
            return value.ToString("G", CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
