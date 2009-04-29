using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MY_NAMESPACE.Core
{
    //TODO: test and experimental code to resolve unknown types
    public class PostgreSQLTypeConverter
    {
        public virtual object FromString(string data)
        {
            throw new NotImplementedException();
        }

        public virtual Type CLR_Type()
        {
            throw new NotImplementedException();
        }

        public virtual string ToString(object obj)
        {
            throw new NotImplementedException();
        }
    }

    public class PostgreSQLEnumConverter<E> : PostgreSQLTypeConverter
    {
        public override Type CLR_Type()
        {
            return typeof(E);
        }
    }

    //TODO: test and experimental code to resolve unknown types
    public delegate void SchemaReaderResolvePGTypeEventHandler(SchemaReaderResolvePGTypeEventArgs e);
    //TODO: test and experimental code to resolve unknown types
    public class SchemaReaderResolvePGTypeEventArgs : EventArgs
    {
        public string PG_TypeName { get; set; }
        public PostgreSQLTypeConverter Converter { get; set; }

        public SchemaReaderResolvePGTypeEventArgs(string p_PG_TypeName)
        {
            PG_TypeName = p_PG_TypeName;
            Converter = null;
        }
    }

}
