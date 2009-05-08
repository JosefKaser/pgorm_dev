using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PGORM.PostgreSQL.Catalog
{
    internal class pg_index
    {
		public String constraint_name {get; set;}
		public String table_namespace {get; set;}
		public String table_name {get; set;}
		public Int32[] constraint_keys {get; set;}
		public String constraint_type {get; set;}
		public String foreign_table_namespace {get; set;}
		public String foreign_table_name {get; set;}

        public static pg_index FromReader(IDataReader reader)
        {
            pg_index result = new pg_index();
			result.constraint_name = DataAccess.Convert<String>(reader["constraint_name"],null);
			result.table_namespace = DataAccess.Convert<String>(reader["table_namespace"],null);
			result.table_name = DataAccess.Convert<String>(reader["table_name"],null);
			result.constraint_keys = DataAccess.Convert<Int32[]>(reader["constraint_keys"],null);
			result.constraint_type = DataAccess.Convert<String>(reader["constraint_type"],null);
			result.foreign_table_namespace = DataAccess.Convert<String>(reader["foreign_table_namespace"],null);
			result.foreign_table_name = DataAccess.Convert<String>(reader["foreign_table_name"],null);

            return result;
        }
    }
}
