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
        private static string PGORM_SP_SIG = "!_PGORMSP_";
        public static string TEMP_SCHEMA = "";
        public static Version ServerVersion;
        #endregion

        #region InformationSchema
        static InformationSchema()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
        } 
        #endregion

        #region CurrentDomain_DomainUnload
        // acting as static destructor
        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            transaction.Rollback();
        } 
        #endregion


        #region Read
        public static void Read()
        {
            ServerVersion = new Version(DataAccess.Connection.ServerVersion);

            transaction = DataAccess.BeginTransaction();

            if (ServerVersion >= new Version(8, 4))
                Functions = DataAccess.ExecuteObjectQuery<pg_proc>(SQLScripts.GetAllFunctions84, transaction, pg_proc.FromReader);
            else
                Functions = DataAccess.ExecuteObjectQuery<pg_proc>(SQLScripts.GetAllFunctions83, transaction, pg_proc.FromReader);

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

        private static void PrepareUserDefinedTypes()
        {
            TEMP_SCHEMA = GetTemporarySchemaName();
            List<pg_relation> rels = Relations.FindAll(r => r.table_type == "USER-DEFINED" || r.table_type == "BASE TABLE" || r.table_type == "VIEW");
            foreach (pg_relation rel in rels)
            {
                string function_name = string.Format("{0}.\"{1}_{2}\"", TEMP_SCHEMA, rel.table_schema, rel.table_name);
                string return_type = string.Format("\"{0}\".\"{1}\"",rel.table_schema,rel.table_name);
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


        #region PrepareRelations
        private static void PrepareRelations()
        {
            foreach (pg_relation rel in Relations)
            {
                if (rel.table_type == "LOCAL TEMPORARY")
                {
                    if(rel.table_name.Contains(PGORM_SP_SIG))
                        rel.table_type = "SP ARGUMENT";
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

        #region GetFunstionSigniture
        public static string GetFunstionSigniture(string name)
        {
            if (name.Contains(PGORM_SP_SIG))
                return name.Replace(PGORM_SP_SIG + ";","").Trim();
            else
                return name;
        } 
        #endregion
    }
}
