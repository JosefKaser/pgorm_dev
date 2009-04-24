using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace MY_NAMESPACE.Core
{
    #region DataObjectValueTypeConverter
    public class DataObjectValueTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            Type t = GetGenericType(value.GetType().FullName);
            object result = Activator.CreateInstance(t);
            t.InvokeMember("DbValue", BindingFlags.SetProperty, null, result, new object[] { value });
            return result;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public static Type GetGenericType(string innerType)
        {
            // construct the mangled name
            string mangledName = string.Format("MY_NAMESPACE.DataObjectValue`1[[{0}]]", innerType);

            // get the open generic type
            Type genericType = Type.GetType(mangledName);
            return genericType;
        }
    }
    #endregion
}