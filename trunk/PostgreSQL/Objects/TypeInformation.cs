using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostgreSQL.Objects
{
    public class TypeInformation : ICloneable
    {
        public String TypeNamespace { get; set; }
        public String TypeShortName { get; set; }
        public String TypeLongName { get; set; }
        public PgTypeType TypeType { get; set; }
        public String Delimiter { get; set; }
        public String BaseTypeName { get; set; }
        public String BaseTypeSchemaName { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            TypeInformation r = new TypeInformation();
            r.TypeNamespace = this.TypeNamespace;
            r.TypeShortName = this.TypeShortName;
            r.TypeLongName = this.TypeLongName;
            r.TypeType = this.TypeType;
            r.Delimiter = this.Delimiter;
            r.BaseTypeName = this.BaseTypeName;
            r.BaseTypeSchemaName = this.BaseTypeSchemaName;
            return r;
        }

        #endregion
    }
}
