using System;

namespace Engine
{
    /// <summary>
    /// Fields/Properties marked with this attribute will be recognized by Engine when parent object is passed in as context
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Constant : Attribute
    { }
}
