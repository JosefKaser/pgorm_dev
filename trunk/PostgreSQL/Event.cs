using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostgreSQL
{
    public enum SchemaReaderMessageEventType
    {
        Information,
        Warning,
        Error,
    }

    public delegate void SchemaReaderMessageEventHandler(SchemaReaderEventArgs e);

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

    public class SchemaReaderEventArgs : EventArgs
    {
        public SchemaReaderMessageEventType EventType { get; set; }
        public string Message { get; set; }

        public SchemaReaderEventArgs(SchemaReaderMessageEventType p_EventType,string p_Message)
        {
            EventType = p_EventType;
            Message = p_Message;
        }
    }
}
