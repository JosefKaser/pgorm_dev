using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PGORM.PostgreSQL.Catalog
{
    internal class pg_proc
    {
		public Int64? oid {get; set;}
		public String function_name {get; set;}
		public String schema_name {get; set;}
		public String return_type {get; set;}
		public String return_type_schema {get; set;}
		public String[] arg_names {get; set;}
		public Int32? num_args {get; set;}
		public Boolean? returns_set {get; set;}
		public String argument_info {get; set;}
		public String[] arg_types {get; set;}
		public String[] all_arg_types {get; set;}
		public String[] proargmodes {get; set;}
		public String return_type_type {get; set;}

        public static pg_proc FromReader(IDataReader reader)
        {
            pg_proc result = new pg_proc();
			result.oid = DataAccess.Convert<Int64?>(reader["oid"],null);
			result.function_name = DataAccess.Convert<String>(reader["function_name"],null);
			result.schema_name = DataAccess.Convert<String>(reader["schema_name"],null);
			result.return_type = DataAccess.Convert<String>(reader["return_type"],null);
			result.return_type_schema = DataAccess.Convert<String>(reader["return_type_schema"],null);
			result.arg_names = DataAccess.Convert<String[]>(reader["arg_names"],null);
			result.num_args = DataAccess.Convert<Int32?>(reader["num_args"],null);
			result.returns_set = DataAccess.Convert<Boolean?>(reader["returns_set"],null);
			result.argument_info = DataAccess.Convert<String>(reader["argument_info"],null);
			result.arg_types = DataAccess.Convert<String[]>(reader["arg_types"],null);
			result.all_arg_types = DataAccess.Convert<String[]>(reader["all_arg_types"],null);
			result.proargmodes = DataAccess.Convert<String[]>(reader["proargmodes"],null);
			result.return_type_type = DataAccess.Convert<String>(reader["return_type_type"],null);

            return result;
        }
    }
}
