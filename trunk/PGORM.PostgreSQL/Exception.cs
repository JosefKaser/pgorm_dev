using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM.PostgreSQL
{
    public class SchemaNotImplementedException : NotImplementedException
    {
        public SchemaNotImplementedException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

    public class SchemaInvalidOperationException : InvalidOperationException
    {
        public SchemaInvalidOperationException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

}
