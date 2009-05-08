using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateNS.Core
{
    public interface IPostgreSQLTypeConverter
    {
        object FromString(string data);
        string ToString(object obj);
        string PgType();
        string PgTypeSchema();
        Type CLRType();
    }

    [AttributeUsageAttribute(AttributeTargets.Class)]
    public class PostgreSQLTypeConverterAttribute : Attribute
    {
    }

    //TODO: test and experimental code to resolve unknown types
    public delegate void SchemaReaderResolvePGTypeEventHandler(SchemaReaderResolvePGTypeEventArgs e);
    //TODO: test and experimental code to resolve unknown types
    public class SchemaReaderResolvePGTypeEventArgs : EventArgs
    {
        public string PG_TypeName { get; set; }
        public IPostgreSQLTypeConverter Converter { get; set; }

        public SchemaReaderResolvePGTypeEventArgs(string p_PG_TypeName)
        {
            PG_TypeName = p_PG_TypeName;
            Converter = null;
        }
    }

}
