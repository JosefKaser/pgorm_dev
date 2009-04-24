using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PostgreSQL.Catalog
{
    internal class pg_view_depend
    {
		public String view_schema {get; set;}
		public String view_name {get; set;}
		public String table_schema {get; set;}
		public String table_name {get; set;}

        public static pg_view_depend FromReader(IDataReader reader)
        {
            pg_view_depend result = new pg_view_depend();
			result.view_schema = DataAccess.Convert<String>(reader["view_schema"],null);
			result.view_name = DataAccess.Convert<String>(reader["view_name"],null);
			result.table_schema = DataAccess.Convert<String>(reader["table_schema"],null);
			result.table_name = DataAccess.Convert<String>(reader["table_name"],null);

            return result;
        }
    }
}
