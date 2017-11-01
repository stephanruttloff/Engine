using System;

namespace Engine
{
    internal class DescriptionAttribute : Attribute
    {
        #region Properties

        public string Value { get; }

        #endregion

        #region Constructor

        public DescriptionAttribute(string value)
        {
            Value = value;
        }

        #endregion
    }
}
