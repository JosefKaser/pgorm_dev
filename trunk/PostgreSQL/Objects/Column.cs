using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public string DefaultValue { get; set; }
        public string DB_Comment { get; set; }
        public PostgreSQLTypeConverter Converter { get; set; }

        public Column()
        {
            Converter = null;
        }
    }
}