using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostgreSQL;
using PostgreSQL.Catalog;

namespace PostgreSQL.Objects
{
    public class Column
    {
        public string ColumnName { get; set; }
        public Type CLR_Type { get; set; }
        public string PG_Type { get; set; }
        public int ColumnIndex { get; set; }
        public bool IsNullable { get; set; }
        public bool IsSerial { get; set; }
        public bool IsEntity { get; set; }
        public bool IsPgArray { get; set; }
        public string DefaultValue { get; set; }
        public string DB_Comment { get; set; }
        public PgTypeType PGTypeType { get; set; }
        public TypeInformation TypeInfo { get; set; }

        public Column()
        {
        }

        #region ICloneable Members

        public C Clone<C>() where C : Column ,new()
        {
            C c = new C();
            c.ColumnName = this.ColumnName;
            c.CLR_Type = this.CLR_Type;
            c.PG_Type = this.PG_Type;
            c.ColumnIndex = this.ColumnIndex;
            c.IsNullable = this.IsNullable;
            c.IsSerial = this.IsSerial;
            c.IsEntity = this.IsEntity;
            c.IsPgArray = this.IsPgArray;
            c.DefaultValue = this.DefaultValue;
            c.DB_Comment = this.DB_Comment;
            c.PGTypeType = this.PGTypeType;
            c.TypeInfo = (TypeInformation)this.TypeInfo.Clone();
            return c;
        }

        #endregion
    }
}