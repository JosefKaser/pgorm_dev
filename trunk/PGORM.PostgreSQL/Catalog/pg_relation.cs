using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PGORM.PostgreSQL.Catalog
{
    internal class pg_relation
    {
		public String table_catalog {get; set;}
		public String table_schema {get; set;}
		public String table_name {get; set;}
		public String table_type {get; set;}
		public String is_insertable_into {get; set;}

        public static pg_relation FromReader(IDataReader reader)
        {
            pg_relation result = new pg_relation();
			result.table_catalog = DataAccess.Convert<String>(reader["table_catalog"],null);
			result.table_schema = DataAccess.Convert<String>(reader["table_schema"],null);
			result.table_name = DataAccess.Convert<String>(reader["table_name"],null);
			result.table_type = DataAccess.Convert<String>(reader["table_type"],null);
			result.is_insertable_into = DataAccess.Convert<String>(reader["is_insertable_into"],null);

            return result;
        }
    }
}
