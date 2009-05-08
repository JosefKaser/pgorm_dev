using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PGORM.PostgreSQL.Catalog
{
    internal class pg_entitycolumn
    {
		public String constraint_type {get; set;}
		public String table_catalog {get; set;}
		public String table_schema {get; set;}
		public String table_name {get; set;}
		public String column_name {get; set;}
		public String constraint_catalog {get; set;}
		public String constraint_schema {get; set;}
		public String constraint_name {get; set;}

        public static pg_entitycolumn FromReader(IDataReader reader)
        {
            pg_entitycolumn result = new pg_entitycolumn();
			result.constraint_type = DataAccess.Convert<String>(reader["constraint_type"],null);
			result.table_catalog = DataAccess.Convert<String>(reader["table_catalog"],null);
			result.table_schema = DataAccess.Convert<String>(reader["table_schema"],null);
			result.table_name = DataAccess.Convert<String>(reader["table_name"],null);
			result.column_name = DataAccess.Convert<String>(reader["column_name"],null);
			result.constraint_catalog = DataAccess.Convert<String>(reader["constraint_catalog"],null);
			result.constraint_schema = DataAccess.Convert<String>(reader["constraint_schema"],null);
			result.constraint_name = DataAccess.Convert<String>(reader["constraint_name"],null);

            return result;
        }
    }
}
