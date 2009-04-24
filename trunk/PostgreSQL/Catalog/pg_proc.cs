using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PostgreSQL.Catalog
{
    internal class pg_proc
    {
		public Int64? proc_oid {get; set;}
		public String proname {get; set;}
		public String[] arg_types {get; set;}
		public String return_type {get; set;}
		public String return_type_type {get; set;}
		public String[] proargnames {get; set;}
		public Int32? num_args {get; set;}
		public Boolean? returns_set {get; set;}
		public Boolean? is_table {get; set;}
		public Boolean? is_view {get; set;}
		public Boolean? is_composite {get; set;}
		public Boolean? is_void {get; set;}
		public Boolean? is_enum {get; set;}

        public static pg_proc FromReader(IDataReader reader)
        {
            pg_proc result = new pg_proc();
			result.proc_oid = DataAccess.Convert<Int64?>(reader["proc_oid"],null);
			result.proname = DataAccess.Convert<String>(reader["proname"],null);
			result.arg_types = DataAccess.Convert<String[]>(reader["arg_types"],null);
			result.return_type = DataAccess.Convert<String>(reader["return_type"],null);
			result.return_type_type = DataAccess.Convert<String>(reader["return_type_type"],null);
			result.proargnames = DataAccess.Convert<String[]>(reader["proargnames"],null);
			result.num_args = DataAccess.Convert<Int32?>(reader["num_args"],null);
			result.returns_set = DataAccess.Convert<Boolean?>(reader["returns_set"],null);
			result.is_table = DataAccess.Convert<Boolean?>(reader["is_table"],null);
			result.is_view = DataAccess.Convert<Boolean?>(reader["is_view"],null);
			result.is_composite = DataAccess.Convert<Boolean?>(reader["is_composite"],null);
			result.is_void = DataAccess.Convert<Boolean?>(reader["is_void"],null);
			result.is_enum = DataAccess.Convert<Boolean?>(reader["is_enum"],null);

            return result;
        }
    }
}
