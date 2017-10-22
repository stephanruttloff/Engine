using System;

namespace Engine
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Constant : Attribute
    { }
}
