using System;

namespace NewCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ResourceKeyAttribute : Attribute
    {
        public string Key { get; }

        public ResourceKeyAttribute(string key) => Key = key;
    }
}