using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace PGORM.PostgreSQL.Catalog
{
    internal class InformationSchema
    {
        #region Props
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
        public static string PGORM_SP_SIG = "!PGORMSP";
        public static string PGORM_SP_RET = "!PGORMRT";
        public static string TEMP_SCHEMA = "";
        public static Version ServerVersion;
        #endregion

        #region Read
        public static void Read()
        {
            ServerVersion = new Version(DataAccess.Connection.ServerVersion);
            TEMP_SCHEMA = GetTemporarySchemaName();
            transaction = DataAccess.BeginTransaction();

            PrepareFunctions();

            Relations = DataAccess.ExecuteObjectQuery<pg_relation>(SQLScripts.GetAllTablesViews, transaction, pg_relation.FromReader);
            PrepareRelations();
            Columns = DataAccess.ExecuteObjectQuery<pg_column>(SQLScripts.GetAllColumns, transaction, pg_column.FromReader);
            PrepareUserDefinedTypes();

            Types = DataAccess.ExecuteObjectQuery<pg_type>(SQLScripts.GetAllTypes, transaction, pg_type.FromReader);
            DistinctIndexes = DataAccess.ExecuteObjectQuery<pg_dindex>(SQLScripts.GetDistinctIndex, transaction, pg_dindex.FromReader);
            ViewTableDepends = DataAccess.ExecuteObjectQuery<pg_view_depend>(SQLScripts.GetViewTableDepends, transaction, pg_view_depend.FromReader);
            EntityColumns = DataAccess.ExecuteObjectQuery<pg_entitycolumn>(SQLScripts.GetAllEntityColumns, transaction, pg_entitycolumn.FromReader);
            Serials = DataAccess.ExecuteObjectQuery<pg_serial>(SQLScripts.GetAllSerials, transaction, pg_serial.FromReader);
            Indexes = DataAccess.ExecuteObjectQuery<pg_index>(SQLScripts.GetAllIndexes, transaction, pg_index.FromReader);
            ColumnComments = DataAccess.ExecuteObjectQuery<pg_column_comment>(SQLScripts.GetAllColumnComments, transaction, pg_column_comment.FromReader);
        }
        #endregion

        #region InformationSchema
        static InformationSchema()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
        } 
        #endregion

        #region PrepareUserDefinedTypes
        private static void PrepareUserDefinedTypes()
        {
            // create a temp function that reatuns a record of udt/table/view
            List<pg_relation> rels = Relations.FindAll(r => r.table_type == "USER-DEFINED" || r.table_type == "BASE TABLE" || r.table_type == "VIEW" || r.table_type == "FUNCTION RETURN TYPE");
            foreach (pg_relation rel in rels)
            {
                string function_name = string.Format("{0}.\"{1}:{2}\"", TEMP_SCHEMA, rel.table_schema, rel.table_name);
                string return_type = string.Format("\"{0}\".\"{1}\"", rel.table_schema, rel.table_name);
                int num_columns = Columns.Count(c => c.table_name == rel.table_name && c.table_schema == rel.table_schema);
                string cols = "";
                for (int a = 0; a != num_columns; a++)
                    cols += "null,";
                cols = cols.Substring(0, cols.Length - 1);
                string sql = string.Format("create or replace function {0}() returns {1} as $$ select ({2})::{1}; $$ language sql;",
                    function_name, return_type, cols);
                NpgsqlCommand command = new NpgsqlCommand(sql, DataAccess.Connection, transaction);
                command.ExecuteNonQuery();
            }
        } 
        #endregion

        #region GetTemporarySchemaName
        private static string GetTemporarySchemaName()
        {
            string result = "";
            string sql = "create temporary table dummp(id integer) on commit drop;" +
                          "select nspname from pg_namespace where oid=pg_my_temp_schema();";
            NpgsqlCommand command = new NpgsqlCommand(sql, DataAccess.Connection, transaction);
            NpgsqlDataReader reader = command.ExecuteReader();
            reader.Read();
            result = reader.GetString(0);
            reader.Close();
            return result;
        } 
        #endregion

        #region PrepareRelations
        private static void PrepareRelations()
        {
            foreach (pg_relation rel in Relations)
            {
                if (rel.table_type == "LOCAL TEMPORARY")
                {
                    if(rel.table_name.Contains(PGORM_SP_SIG))
                        rel.table_type = "FUNCTION ARGUMENT";
                    else if (rel.table_name.Contains(PGORM_SP_RET))
                        rel.table_type = "FUNCTION RETURN TYPE";

                }
            }
        } 
        #endregion

        #region GetColumnComment
        public static pg_column_comment GetColumnComment(pg_column col)
        {
            return ColumnComments.Find(c => c.column_name == col.column_name &&
                c.table_name == col.table_name &&
                c.table_schema == col.table_schema);
        } 
        #endregion

        #region GetColumnsByRelation
        public static List<pg_column> GetColumnsByRelation(pg_relation rel)
        {
            return Columns.FindAll(c => c.table_name == rel.table_name && c.table_schema == rel.table_schema);
        } 
        #endregion

        #region CurrentDomain_DomainUnload
        // acting as static destructor
        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            transaction.Rollback();
        }
        #endregion

        #region PrepareFunctions
        private static void PrepareFunctions()
        {
            if (ServerVersion >= new Version(8, 4))
                Functions = DataAccess.ExecuteObjectQuery<pg_proc>(SQLScripts.GetAllFunctions84, transaction, pg_proc.FromReader);
            else
                Functions = DataAccess.ExecuteObjectQuery<pg_proc>(SQLScripts.GetAllFunctions83, transaction, pg_proc.FromReader);

            // prepare arg types and arg modes
            foreach (pg_proc proc in Functions)
            {
                if (proc.all_arg_types != null)
                {
                    proc.arg_types = proc.all_arg_types;
                    proc.num_args = proc.all_arg_types.Length;
                }
                else
                {
                    proc.arg_modes = new string[(int)proc.num_args];
                    for (int a = 0; a != proc.arg_modes.Length; a++)
                        proc.arg_modes[a] = "i";
                }
            }

            PrepareFunctionArgNames();
            PrepareFunctionArgTypes();
            PrepareFunctionReturnsTypes();
        } 
        #endregion

        #region PrepareFunctionReturnsTypes
        private static void PrepareFunctionReturnsTypes()
        {
            foreach (pg_proc proc in Functions)
            {
                // create a composite type when the return type is record
                if (proc.return_type_type == "RECORD" || proc.return_type_type == "BASE")
                {
                    string sql = "";
                    string column = "";
                    //if the function has out arguments then we need to create a composite type
                    if (proc.arg_modes.Contains("o") || proc.arg_modes.Contains("b"))
                    {
                        //loop the arguments and modes and create a composite type
                        for (int a = 0; a != (int)proc.num_args; a++)
                        {
                            if (proc.arg_modes[a] == "o" || proc.arg_modes[a] == "b")
                            {
                                column += string.Format("{0} {1},\n",
                                    proc.arg_names[a],
                                    proc.arg_types[a]);
                            }
                        }

                        // when there is no out param then this function actually returns a record 
                        // where we return a string type.
                        if (!string.IsNullOrEmpty(column))
                            column = column.Substring(0, column.Length - 2);
                        else
                            column = "\"Value\" varchar";

                        sql = string.Format("CREATE TYPE {0}.{1} AS (\n{2}\n);",
                            TEMP_SCHEMA,
                            CreateFunctionSigniture(PGORM_SP_RET, proc),
                            column);
                    }
                    else // should be base type 
                    {
                        string return_type = proc.return_type;
                        if (proc.return_type == "record" || proc.return_type == "anyelement" ) // actuall RECORD type where translated to string
                            return_type = "varchar";
                        sql = string.Format("CREATE TYPE {0}.{1} AS (\"Value\" {2});",
                                TEMP_SCHEMA,
                                CreateFunctionSigniture(PGORM_SP_RET, proc),
                                return_type);
                        
                    }
                    DataAccess.ExecuteNoneQuery(sql, transaction);
                }
            }
        } 
        #endregion

        #region PrepareFunctionDefaultValues
        private static List<string> PrepareFunctionDefaultValues(pg_proc proc)
        {
            List<object> values = new List<object>();
            List<string> result = new List<string>();
            if (proc.num_args != 0)
            {
                if (!string.IsNullOrEmpty(proc.argument_defaults))
                {
                    // run pg_get_expr to the a record of the default values
                    // then prepare the default value for every argument. set null when not default value
                    string sql = string.Format("SELECT {0}", proc.argument_defaults);
                    NpgsqlCommand command = new NpgsqlCommand(sql, DataAccess.Connection);
                    NpgsqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    for (int a = 0; a != reader.FieldCount; a++)
                        values.Add(reader.GetValue(a));
                    reader.Close();

                    //setindex = num values minus num args 
                    int setindex = (int)proc.num_args - values.Count;
                    for (int a = 0; a != (int)proc.num_args; a++)
                    {
                        if (a >= setindex)
                            result.Add(values[a - setindex].ToString());
                        else
                            result.Add(null);
                    }
                }
                else
                {
                    proc.arg_names.ToList().ForEach(i => result.Add(null));
                }
            }
            return result;
        } 
        #endregion

        private static string FixPesudoType(string data)
        {
            if (data == "anyarray")
                return "varchar[]";
            else
                return data;
        }

        #region PrepareFunctionArgTypes
        private static void PrepareFunctionArgTypes()
        {
            //create a table from this functions argument types. this table will be used to 
            //create to construct the CLR functions arguments
            foreach (pg_proc proc in Functions)
            {
                if (proc.num_args != 0)
                {
                    List<string> defaultValues = PrepareFunctionDefaultValues(proc);

                    //loop the argument types and construct CREATE table
                    string columns = "\n";
                    for (int a = 0; a != proc.arg_types.Count(); a++)
                    {
                        string col_default = "";
                        if (defaultValues[a] != null)
                            col_default = string.Format("DEFAULT '{0}'::{1}", defaultValues[a], proc.arg_types[a]);

                        string arg_type =  FixPesudoType(proc.arg_types[a]);

                        columns += string.Format("\"{0}:{1}\" {2} {3} {4}\n",
                            GetArgMode(proc.arg_modes[a]),
                            proc.arg_names[a],
                            arg_type,
                            col_default,
                            (a != (int)proc.num_args - 1) ? "," : "");
                    }

                    string sql = string.Format("CREATE TEMP TABLE {0} ({1}) ON COMMIT DROP;",
                        CreateFunctionSigniture(PGORM_SP_SIG,proc),
                        columns);

                    DataAccess.ExecuteNoneQuery(sql, transaction);
                }
            } 
        } 
        #endregion

        #region GetArgMode
        private static string GetArgMode(string data)
        {
            switch (data)
            {
                case "i":
                    return "IN";
                case "o":
                    return "OUT";
                case "b":
                    return "INOUT";
                default:
                    throw new SchemaNotImplementedException("Argument mode ({0}) is not implemented!", data);
            }
        } 
        #endregion

        #region CreateFunctionSigniture
        private static string CreateFunctionSigniture(string sig, pg_proc proc)
        {
            return string.Format("\"{0}:{1}:{2}\"",
                sig,
                proc.schema_name,
                proc.function_name
                );
        } 
        #endregion

        #region PrepareFunctionArgNames
        private static void PrepareFunctionArgNames()
        {
            List<string> vargs = new List<string>();
            foreach (pg_proc proc in Functions)
            {
                // when we have any arguments
                if (proc.num_args != 0)
                {
                    // when no names are defined we create virtual ones
                    if (proc.arg_names == null)
                    {
                        vargs.Clear();
                        for (int a = 0; a != proc.num_args; a++)
                            vargs.Add(string.Format("arg_{0}", (a + 1)));
                        proc.arg_names = vargs.ToArray();
                    }
                    else
                    {
                        // get argument names if possible of create virtual ones when not set
                        vargs.Clear();
                        for (int a = 0; a != proc.num_args; a++)
                            if (string.IsNullOrEmpty(proc.arg_names[a]))
                                vargs.Add(string.Format("arg_{0}", (a + 1)));
                            else
                                vargs.Add(string.Format("{0}", proc.arg_names[a]));

                        proc.arg_names = vargs.ToArray();
                    }
                }
            }
        } 
        #endregion
    }
}
