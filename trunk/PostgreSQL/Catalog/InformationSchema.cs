using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace PostgreSQL.Catalog
{
    internal class InformationSchema
    {
        public static List<pg_relation> Relations;
        public static List<pg_column> Columns;
        public static List<pg_type> Types;
        public static List<pg_dindex> DistinctIndexes;
        public static List<pg_view_depend> ViewTableDepends;
        public static List<pg_proc> Functions;
        public static List<pg_entitycolumn> EntityColumns;
        public static List<pg_serial> Serials;
        public static List<pg_index> Indexes;
        public static List<pg_column_comment> ColumnComments;
        private static NpgsqlTransaction transaction;
        public static string PGORM_SP_SIG = "_PGORM1___";


        public static void Read()
        {
            transaction = DataAccess.BeginTransaction();

            Functions = DataAccess.ExecuteObjectQuery<pg_proc>(SQLScripts.GetAllFunctions, transaction, pg_proc.FromReader);
            ProcessFunctions();
            PrepareStoredFunctionArguments();

            Relations = DataAccess.ExecuteObjectQuery<pg_relation>(SQLScripts.GetAllTablesViews,transaction, pg_relation.FromReader);
            PrepareRelations();

            Columns = DataAccess.ExecuteObjectQuery<pg_column>(SQLScripts.GetAllColumns,transaction, pg_column.FromReader);
            Types = DataAccess.ExecuteObjectQuery<pg_type>(SQLScripts.GetAllTypes,transaction, pg_type.FromReader);
            DistinctIndexes = DataAccess.ExecuteObjectQuery<pg_dindex>(SQLScripts.GetDistinctIndex,transaction, pg_dindex.FromReader);
            ViewTableDepends = DataAccess.ExecuteObjectQuery<pg_view_depend>(SQLScripts.GetViewTableDepends,transaction, pg_view_depend.FromReader);
            EntityColumns = DataAccess.ExecuteObjectQuery<pg_entitycolumn>(SQLScripts.GetAllEntityColumns, transaction, pg_entitycolumn.FromReader);
            Serials = DataAccess.ExecuteObjectQuery<pg_serial>(SQLScripts.GetAllSerials, transaction, pg_serial.FromReader);
            Indexes = DataAccess.ExecuteObjectQuery<pg_index>(SQLScripts.GetAllIndexes, transaction, pg_index.FromReader);
            ColumnComments = DataAccess.ExecuteObjectQuery<pg_column_comment>(SQLScripts.GetAllColumnComments, transaction, pg_column_comment.FromReader);
            transaction.Commit();
        }

        private static void PrepareStoredFunctionArguments()
        {
            foreach (pg_proc proc in Functions)
            {
                string sql = "";
                //string tmp_name = string.Format("\"{0}{1}_{2}\"", pg_sig, proc.proname, Guid.NewGuid().ToString("N"));
                //string tmp_name = string.Format("\"{0}{{{1},{2}}}\"", PGORM_SP_SIG,proc.proc_oid,proc.proname);
                string tmp_name = string.Format("\"{0}{{{1}}}\"", PGORM_SP_SIG, proc.proc_oid);

                // create a temporary table for each proc which imitates the argument names and types
                if (proc.num_args > 0)
                {
                    List<string> distinct_arg = new List<string>();
                    string arg_name="";
                    for (int a = 0; a != proc.num_args; a++)
                    {
                        arg_name = proc.proargnames[a];
                        if (distinct_arg.Exists(i => i == arg_name))
                            arg_name = string.Format("{0}_arg{1}", arg_name, a);
                        else
                            distinct_arg.Add(arg_name);

                        // correct arg types
                        if(proc.arg_types[a] == "anyarray")
                            proc.arg_types[a] = "varchar[]";

                        if (proc.arg_types[a] == "anyelement")
                            proc.arg_types[a] = "varchar";

                        sql += string.Format("\"{0}\" {1}\r\n,", arg_name, proc.arg_types[a]);
                    }
                    sql = string.Format("create temporary table {0} ({1}); ", tmp_name, sql.Substring(0, sql.Length - 1));
                    DataAccess.ExecuteNoneQuery(sql, transaction);
                }
            }
        }

        private static void PrepareRelations()
        {
            foreach (pg_relation rel in Relations)
            {
                if(rel.table_type == "LOCAL TEMPORARY" && rel.table_name.Contains(PGORM_SP_SIG))
                {
                    rel.table_type = "SP ARGUMENT";
                }
            }
        }

        private static void ProcessFunctions()
        {
            foreach (pg_proc proc in Functions)
            {
                if ((int)proc.num_args != 0 && proc.proargnames == null)
                {
                    List<string> new_args = new List<string>();
                    for(int a = 0; a != proc.num_args; a++)
                        new_args.Add(string.Format("p_{0}_{1}",
                            proc.arg_types[a].Replace(" ","_"),a));
                    proc.proargnames = new_args.ToArray();
                }
            }
        }

        public static pg_column_comment GetColumnComment(pg_column col)
        {
            return ColumnComments.Find(c => c.column_name == col.column_name &&
                c.table_name == col.table_name &&
                c.table_schema == col.table_schema);
        }

        public static List<pg_column> GetColumnsByRelation(pg_relation rel)
        {
            return Columns.FindAll(c => c.table_name == rel.table_name && c.table_schema == rel.table_schema);
        }

        public static pg_type GetPgTypeByName(string name)
        {
            return Types.Find(t => t.type_short_name == name || t.type_long_name == name);
        }

        public static string[] GetFunstionSigniture(string name)
        {
            string[] result = null;
            if (name.Contains(PGORM_SP_SIG))
            {
                string sql = string.Format("select '{0}'::text[] as sig", name.Replace(PGORM_SP_SIG, ""));
                NpgsqlCommand command = new NpgsqlCommand(sql, DataAccess.Connection, transaction);
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    result = (string[])reader.GetValue(0);
                reader.Close();
            }
            return result;
        }
    }
}
