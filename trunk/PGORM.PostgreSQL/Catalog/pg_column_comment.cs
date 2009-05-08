using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PGORM.PostgreSQL.Catalog
{
    internal class pg_column_comment
    {
		public String table_schema {get; set;}
		public String table_name {get; set;}
		public String column_name {get; set;}
		public String description {get; set;}

        public static pg_column_comment FromReader(IDataReader reader)
        {
            pg_column_comment result = new pg_column_comment();
			result.table_schema = DataAccess.Convert<String>(reader["table_schema"],null);
			result.table_name = DataAccess.Convert<String>(reader["table_name"],null);
			result.column_name = DataAccess.Convert<String>(reader["column_name"],null);
			result.description = DataAccess.Convert<String>(reader["description"],null);

            return result;
        }
    }
}
