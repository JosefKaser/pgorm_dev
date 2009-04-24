using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostgreSQL
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
}
