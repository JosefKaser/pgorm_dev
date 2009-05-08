using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM.PostgreSQL
{
    public enum SchemaReaderMessageEventType
    {
        Information,
        Warning,
        Error,
    }

    public delegate void SchemaReaderMessageEventHandler(SchemaReaderEventArgs e);

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
