using System;
using System.Collections.Generic;
using System.Text;

namespace JamestsaiTW.Utilities.Converters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ValueConversionAttribute : Attribute
    {
        public ValueConversionAttribute(Type input, Type output)
        { }

        public Type ParameterType { get; set; }
    }
}
