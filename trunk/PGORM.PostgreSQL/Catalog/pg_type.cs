using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PGORM.PostgreSQL.Catalog
{
    internal class pg_type
    {
		public String type_namespace {get; set;}
		public Int64? type_oid {get; set;}
		public String type_short_name {get; set;}
		public String type_long_name {get; set;}
		public String type_type {get; set;}
		public String delimiter {get; set;}
		public String base_type {get; set;}
		public Int64? base_type_oid {get; set;}
		public String base_type_schema {get; set;}

        public static pg_type FromReader(IDataReader reader)
        {
            pg_type result = new pg_type();
			result.type_namespace = DataAccess.Convert<String>(reader["type_namespace"],null);
			result.type_oid = DataAccess.Convert<Int64?>(reader["type_oid"],null);
			result.type_short_name = DataAccess.Convert<String>(reader["type_short_name"],null);
			result.type_long_name = DataAccess.Convert<String>(reader["type_long_name"],null);
			result.type_type = DataAccess.Convert<String>(reader["type_type"],null);
			result.delimiter = DataAccess.Convert<String>(reader["delimiter"],null);
			result.base_type = DataAccess.Convert<String>(reader["base_type"],null);
			result.base_type_oid = DataAccess.Convert<Int64?>(reader["base_type_oid"],null);
			result.base_type_schema = DataAccess.Convert<String>(reader["base_type_schema"],null);

            return result;
        }
    }
}
