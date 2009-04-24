using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PostgreSQL.Catalog
{
    internal class pg_serial
    {
		public String sequence_name {get; set;}
		public String table_name {get; set;}
		public String table_schema {get; set;}
		public String column_name {get; set;}

        public static pg_serial FromReader(IDataReader reader)
        {
            pg_serial result = new pg_serial();
			result.sequence_name = DataAccess.Convert<String>(reader["sequence_name"],null);
			result.table_name = DataAccess.Convert<String>(reader["table_name"],null);
			result.table_schema = DataAccess.Convert<String>(reader["table_schema"],null);
			result.column_name = DataAccess.Convert<String>(reader["column_name"],null);

            return result;
        }
    }
}
