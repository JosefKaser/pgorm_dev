using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PGORM.PostgreSQL.Catalog
{
    internal class pg_dindex
    {
		public String table_namespace {get; set;}
		public String table_name {get; set;}
		public Int32[] constraint_keys {get; set;}

        public static pg_dindex FromReader(IDataReader reader)
        {
            pg_dindex result = new pg_dindex();
			result.table_namespace = DataAccess.Convert<String>(reader["table_namespace"],null);
			result.table_name = DataAccess.Convert<String>(reader["table_name"],null);
			result.constraint_keys = DataAccess.Convert<Int32[]>(reader["constraint_keys"],null);

            return result;
        }
    }
}
