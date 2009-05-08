using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PGORM.PostgreSQL.Catalog
{
    internal class pg_column
    {
		public String table_catalog {get; set;}
		public String table_schema {get; set;}
		public String table_name {get; set;}
		public String column_name {get; set;}
		public Int32? ordinal_position {get; set;}
		public String column_default {get; set;}
		public String is_nullable {get; set;}
		public String data_type {get; set;}
		public Int32? character_maximum_length {get; set;}
		public Int32? numeric_precision {get; set;}
		public Int32? numeric_precision_radix {get; set;}
		public Int32? numeric_scale {get; set;}
		public Int32? datetime_precision {get; set;}
		public String domain_catalog {get; set;}
		public String domain_schema {get; set;}
		public String domain_name {get; set;}
		public String udt_catalog {get; set;}
		public String udt_schema {get; set;}
		public String udt_name {get; set;}
		public Int64? udt_name_oid {get; set;}

        public static pg_column FromReader(IDataReader reader)
        {
            pg_column result = new pg_column();
			result.table_catalog = DataAccess.Convert<String>(reader["table_catalog"],null);
			result.table_schema = DataAccess.Convert<String>(reader["table_schema"],null);
			result.table_name = DataAccess.Convert<String>(reader["table_name"],null);
			result.column_name = DataAccess.Convert<String>(reader["column_name"],null);
			result.ordinal_position = DataAccess.Convert<Int32?>(reader["ordinal_position"],null);
			result.column_default = DataAccess.Convert<String>(reader["column_default"],null);
			result.is_nullable = DataAccess.Convert<String>(reader["is_nullable"],null);
			result.data_type = DataAccess.Convert<String>(reader["data_type"],null);
			result.character_maximum_length = DataAccess.Convert<Int32?>(reader["character_maximum_length"],null);
			result.numeric_precision = DataAccess.Convert<Int32?>(reader["numeric_precision"],null);
			result.numeric_precision_radix = DataAccess.Convert<Int32?>(reader["numeric_precision_radix"],null);
			result.numeric_scale = DataAccess.Convert<Int32?>(reader["numeric_scale"],null);
			result.datetime_precision = DataAccess.Convert<Int32?>(reader["datetime_precision"],null);
			result.domain_catalog = DataAccess.Convert<String>(reader["domain_catalog"],null);
			result.domain_schema = DataAccess.Convert<String>(reader["domain_schema"],null);
			result.domain_name = DataAccess.Convert<String>(reader["domain_name"],null);
			result.udt_catalog = DataAccess.Convert<String>(reader["udt_catalog"],null);
			result.udt_schema = DataAccess.Convert<String>(reader["udt_schema"],null);
			result.udt_name = DataAccess.Convert<String>(reader["udt_name"],null);
			result.udt_name_oid = DataAccess.Convert<Int64?>(reader["udt_name_oid"],null);

            return result;
        }
    }
}
