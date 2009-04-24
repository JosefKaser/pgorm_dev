using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MY_NAMESPACE.Core
{
    #region DataObjectValue
    [DefaultProperty("Value")]
    [TypeConverter(typeof(DataObjectValueTypeConverter))]
    public class DataObjectValue<T>
    {
        #region props
        private object _value;
        #endregion

        #region DbValue
        public object DbValue
        {
            get
            {
				return _value;
            }
            set
            {
                _value = value;
            }
        }
        #endregion

        #region Value
        public T Value
        {
            set
            {
                _value = value;
            }
            get
            {
                object nullObj = null;
                if (_value == DBNull.Value)
                    return (T)nullObj;
                else
                    return (T)_value;
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            if (_value != null && _value != DBNull.Value)
                return _value.ToString();
            else
                return "";
        }
        #endregion

        #region implicit/explicit getter setter 
        public static implicit operator DataObjectValue<T>(T t)
        {
            return new DataObjectValue<T>() { DbValue = t };
        }

        public static explicit operator T(DataObjectValue<T> t)
        {
            return t.Value;
        }

        public static implicit operator string(DataObjectValue<T> t)
        {
            return t.Value.ToString();
        } 
        
        #endregion
    }
    #endregion
}
