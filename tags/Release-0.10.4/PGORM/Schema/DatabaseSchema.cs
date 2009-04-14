/*-------------------------------------------------------------------------
 * DatabaseSchema.cs
 *
 * This file is part of the PGORM project.
 * http://pgorm.googlecode.com/
 *
 * Copyright (c) 2002-2009, TrueSoftware B.V.
 *
 * IDENTIFICATION
 * 
 *  $Id$
 * 	$HeadURL$
 * 	
 *-------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace PGORM
{
    public class DatabaseSchema : BuilderEventProvider
    {
        #region Props

        public List<Table> Tables;
        public List<Table> CompositeTypes;
        protected List<Column> AllColumns;
        protected List<Column> AllPrimaryKeys;
        protected List<Index> AllIndexes;
        protected List<Index> AllForeignKeys;
        protected List<Column> AllKeyColumnUsages;
        protected List<pg_view_column_usage> AllViewInfo;
        protected List<Table> CustomQueries;
        public List<Function> StoredFunctions;
        private List<string> Warnings;
        #endregion

        #region DatabaseSchema
        public DatabaseSchema(ProjectFile p_projectFile, Builder p_builder)
            : base(p_builder)
        {
            CustomQueries = p_projectFile.CutsomQueries;
            ReadSchema(p_projectFile.DatabaseConnectionString);
        }

        public DatabaseSchema(Builder p_builder)
            : base(p_builder)
        {
        }
        #endregion

        public void ReadSchema(string DatabaseConnectionString)
        {
            Warnings = new List<string>();
            SendMessage("Reading Database Schema", BuilderMessageType.Major);
            DataAccess.InitializeDatabase(DatabaseConnectionString);
            PrepareCustomQueries();
            LoadAllFunctions();
            LoadAllViewInfo();
            LoadAllColumns();
            LoadAllPrimaryKeys();
            LoadAllIndexes();
            LoadAllForeignKeys();
            LoadTables();
        }

        #region CreateNewCompositeType
        pg_composite_type CreateNewCompositeType(IDataReader reader)
        {
            return new pg_composite_type()
            {
                column_index = DataAccess.Convert<int>(reader["column_index"], -1),
                column_name = DataAccess.Convert<string>(reader["column_name"], ""),
                db_type = DataAccess.Convert<string>(reader["db_type"], ""),
                type_name = DataAccess.Convert<string>(reader["type_name"], "")
            };
        }
        #endregion

        #region CreateNewPgProc
        pg_proc CreateNewPgProc(IDataReader reader)
        {
            return new pg_proc()
            {
                arg_types = DataAccess.Convert<string>(reader["arg_types"], "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                num_args = DataAccess.Convert<int>(reader["num_args"], 0),
                proargnames = DataAccess.Convert<string[]>(reader["proargnames"], null),
                proname = DataAccess.Convert<string>(reader["proname"], ""),
                return_type = DataAccess.Convert<string>(reader["return_type"], ""),
                return_type_type = DataAccess.Convert<string>(reader["return_type_type"], ""),
                returns_set = DataAccess.Convert<bool>(reader["returns_set"], false)
            };
        }
        #endregion

        #region LoadAllFunctions
        void LoadAllFunctions()
        {
            SendMessage("Loading functions",BuilderMessageType.Major);
            List<pg_proc> pg_procs = DataAccess.ExecuteObjectQuery<pg_proc>(SQLScripts.GetAllFunctions,
                CreateNewPgProc);

            List<pg_composite_type> comp_types = DataAccess.ExecuteObjectQuery<pg_composite_type>(SQLScripts.GetAllCompositeTypes,
                CreateNewCompositeType);

            if (pg_procs.Count != 0)
            {
                CompositeTypes = new List<Table>();

                // creating table objects for every composite type
                var tables = (from c in comp_types select c.type_name).Distinct();
                foreach (string titem in tables)
                {
                    Table table = new Table();
                    table.TableName = titem;
                    table.IsCompositeType = true;

                    var columns = from c in comp_types
                                  where c.type_name == titem
                                  select c;

                    if (columns.Count() != 0)
                    {
                        foreach (pg_composite_type citem in columns)
                        {
                            Column col = new Column();
                            col.ColumnName = citem.column_name;
                            col.ColumnIndex = citem.column_index;
                            if (citem.db_type.Contains("[]"))
                                col.CLR_Type = GetCLRType("ARRAY", citem.db_type);
                            else
                                col.CLR_Type = GetCLRType(citem.db_type, "");
                            table.Columns.Add(col);
                        }
                    }

                    if (table.Columns.Count != 0)
                        CompositeTypes.Add(table);
                }

                StoredFunctions = new List<Function>();
                foreach (pg_proc proc in pg_procs)
                {
                    Function func = new Function();
                    func.ReturnsSet = proc.returns_set;

                    if (!Helper.IsReservedWord(proc.proname))
                    {
                        func.FunctionName = proc.proname;
                    }
                    else
                    {
                        func.FunctionName = string.Format("__" + proc.proname);
                        SendMessage("Renaming function {0} to {1} due C# reserved word usage.", BuilderMessageType.Major, proc.proname, func.FunctionName);
                    }

                    func.ReturnTypeType = proc.return_type_type;
                    func.DB_ReturnType = proc.return_type;

                    if (proc.return_type_type == "b") // built in type
                    {
                        if (proc.return_type.Contains("[]"))
                            func.ReturnType = GetCLRType("ARRAY", proc.return_type.Replace("[]", "")).ToString();
                        else
                            func.ReturnType = GetCLRType(proc.return_type, "").ToString();
                    }
                    else if (proc.return_type_type == "p")
                    {
                        if (proc.return_type == "void")
                            func.ReturnType = "void";
                        else
                            func.ReturnType = GetCLRType(proc.return_type, "").ToString();
                    }
                    else if (proc.return_type_type == "c")
                    {
                        func.ReturnType = string.Format("{0}Object", CleanUpDBName(proc.return_type));
                        func.ReturnTypeRecordSet = string.Format("{0}RecordSet", CleanUpDBName(proc.return_type));
                        func.FactoryName = string.Format("{0}Factory", CleanUpDBName(proc.return_type));
                    }
                    else
                    {
                        func.ReturnType = GetCLRType(proc.return_type, "").ToString();
                        func.ReturnTypeRecordSet = string.Format("{0}RecordSet", CleanUpDBName(proc.return_type));
                        func.FactoryName = string.Format("{0}Factory", CleanUpDBName(proc.return_type));
                        string warning = "StoredFunction ReturnType [" + proc.return_type + "] is not implemented yet.\nThis type will be mapped to typeof(object)";
                        SendMessage("{0}", BuilderMessageType.Minor, warning);
                        Warnings.Add(warning);
                    }

                    if (proc.num_args != 0)
                    {
                        for (int a = 0; a != proc.num_args; a++)
                        {
                            string arg_type = proc.arg_types[a].Trim();
                            Column col = new Column();
                            col.ColumnIndex = a;
                            col.DB_Type = arg_type;
                            if (arg_type.Contains("[]"))
                                col.CLR_Type = GetCLRType("ARRAY", arg_type.Replace("[]", ""));
                            else
                                col.CLR_Type = GetCLRType(arg_type, "");

                            // rename the arg if possible
                            if (proc.proargnames != null && proc.proargnames[a] != "")
                                col.ColumnName = proc.proargnames[a].Replace("p_", "");
                            else
                                col.ColumnName = "ARG" + a;
                            func.Parameters.Add(col);
                        }
                    }

                    int cnt = StoredFunctions.Count(f => f.ToString() == func.ToString());
                    if (cnt != 0)
                        func.FunctionName += cnt;
                    StoredFunctions.Add(func);
                }
            }
        }
        #endregion

        #region CleanUpDBName
        string CleanUpDBName(string data)
        {
            return data.Replace("\"", "");
        }
        #endregion

        #region CleanUp
        [Obsolete("TODO: REMOVE THIS FUNCTIONALITY")]
        public void CleanUp()
        {
            string sql = "";
            if (CustomQueries.Count == 0)
                return;
            Console.Write("Cleanup.");
            foreach (Table tbl in CustomQueries)
            {
                sql = string.Format("DROP VIEW \"{0}\" CASCADE",
                    tbl.TableName);
                DataAccess.ExecuteNoneQuery(sql);
            }
            Console.WriteLine(",Done");
        }
        #endregion

        #region PrepareCustomQueries
        public void PrepareCustomQueries()
        {
            string sql = "";

            if (CustomQueries == null)
                CustomQueries = new List<Table>();

            if (CustomQueries.Count == 0)
                return;

            SendMessage("Preparing custom queries", BuilderMessageType.Minor);
            foreach (Table tbl in CustomQueries)
            {
                sql = string.Format("CREATE OR REPLACE VIEW \"{0}\" AS {1}",
                    tbl.TableName,
                    tbl.CustomQuerySource);
                DataAccess.ExecuteNoneQuery(sql);
            }
        }
        #endregion

        #region CreateNewViewInfo
        pg_view_column_usage CreateNewViewInfo(IDataReader reader)
        {
            return new pg_view_column_usage()
            {
                table_name = DataAccess.Convert<string>(reader["table_name"], null),
                column_name = DataAccess.Convert<string>(reader["column_name"], null),
                view_name = DataAccess.Convert<string>(reader["view_name"], null)
            };
        }
        #endregion

        #region LoadAllViewInfo
        void LoadAllViewInfo()
        {
            AllViewInfo = DataAccess.ExecuteObjectQuery<pg_view_column_usage>(SQLScripts.GetAllViewDepends, CreateNewViewInfo);
        }
        #endregion

        #region CreateNewPGIndex
        private pg_index CreateNewPGIndex(IDataReader reader)
        {
            return new pg_index(
                DataAccess.Convert<string>(reader["table_name"], null),
                DataAccess.Convert<string>(reader["keys"], null),
                DataAccess.Convert<bool>(reader["is_unique"], null)
                );
        }
        #endregion

        #region CreateNewPGForeignKey
        private pg_foreignkey CreateNewPGForeignKey(IDataReader reader)
        {
            return new pg_foreignkey()
            {
                table_name = DataAccess.Convert<string>(reader["table_name"], null),
                constraint_name = DataAccess.Convert<string>(reader["constraint_name"], null)
            };
        }
        #endregion

        #region CreateNewKeyColumnUsage
        private pg_foreignkey_info CreateNewKeyColumnUsage(IDataReader reader)
        {
            return new pg_foreignkey_info()
            {
                table_name = DataAccess.Convert<string>(reader["local_table_name"], null),
                constraint_name = DataAccess.Convert<string>(reader["constraint_name"], null),
                foreign_table_name = DataAccess.Convert<string>(reader["foreign_table_name"], null),
                local_keys = DataAccess.Convert<short[]>(reader["local_keys"], null),
                foreign_keys = DataAccess.Convert<short[]>(reader["foreign_keys"], null),
            };
        }
        #endregion

        #region LoadAllForeignKeys
        private void LoadAllForeignKeys()
        {
            SendMessage("Loading foreign keys", BuilderMessageType.Major);
            AllForeignKeys = new List<Index>();

            List<pg_foreignkey> foreign_keys = DataAccess.ExecuteObjectQuery<pg_foreignkey>(
                SQLScripts.GetAllForeignKeys, CreateNewPGForeignKey);

            List<pg_foreignkey_info> foreign_keys_info = DataAccess.ExecuteObjectQuery<pg_foreignkey_info>(
                SQLScripts.GetAllForeignKeyInfo, CreateNewKeyColumnUsage);

            foreach (pg_foreignkey fkey in foreign_keys)
            {
                var fkeys_per_table = from k in foreign_keys_info
                                      where k.constraint_name == fkey.constraint_name && k.table_name == fkey.table_name
                                      select k;

                Index index = new Index();
                index.TablenName = fkey.table_name;
                index.IndexType = IndexType.ForeignKey;

                foreach (pg_foreignkey_info fkeyinfo in fkeys_per_table)
                {
                    var cols_per_table = from c in AllColumns
                                         where c.TableName == fkey.table_name
                                         select c;

                    List<short> fkey_col_indexes = fkeyinfo.local_keys.ToList<short>();

                    var fkey_cols = from c in cols_per_table
                                    join k in fkey_col_indexes on c.ColumnIndex equals k
                                    select c;

                    index.Columns.AddRange(fkey_cols);
                }
                AllForeignKeys.Add(index);
            }
        }
        #endregion

        #region LoadAllIndexes

        private void LoadAllIndexes()
        {
            SendMessage("Loading Indexces", BuilderMessageType.Major);
            AllIndexes = new List<Index>();
            List<pg_index> pg_indexes = DataAccess.ExecuteObjectQuery<pg_index>(
                SQLScripts.GetAllIndexes, CreateNewPGIndex);

            foreach (pg_index pg_index in pg_indexes)
            {
                Index index = new Index();
                index.TablenName = pg_index.TableName;
                index.IndexType = pg_index.IsUnique ? IndexType.Unique : IndexType.NotUnique;

                var columns = from c in AllColumns
                              where c.TableName == index.TablenName &&
                              pg_index.ColumnIndexes.FindAll(i => i == c.ColumnIndex).Count != 0
                              select c;
                // some indexes have compusite columns which cannot be operated on
                if (columns.Count() != 0)
                {
                    foreach (Column col in columns.Reverse())
                        index.Columns.Add(col);

                    AllIndexes.Add(index);
                }
            }
        }
        #endregion

        #region CreateNewPrimaryKey
        private Column CreateNewPrimaryKey(IDataReader reader)
        {
            return new Column()
            {
                TableName = Convert.ToString(reader["table_name"]),
                ColumnName = Convert.ToString(reader["column_name"])
            };
        }
        #endregion

        #region LoadAllPrimaryKeys
        private void LoadAllPrimaryKeys()
        {
            SendMessage("Loading primary keys", BuilderMessageType.Major);
            AllPrimaryKeys = DataAccess.ExecuteObjectQuery<Column>(SQLScripts.GetAllPrimaryKeys, CreateNewPrimaryKey);
        }
        #endregion

        #region LoadAllColumns
        private void LoadAllColumns()
        {
            SendMessage("Loading table columns", BuilderMessageType.Major);
            AllColumns = DataAccess.ExecuteObjectQuery<Column>(SQLScripts.GetAllColumns, CreateNewColumn);
        }
        #endregion

        #region CreateNewColumn
        private Column CreateNewColumn(IDataReader reader)
        {
            Column col = new Column();
            col.TableName = Convert.ToString(reader["table_name"]);
            col.ColumnName = Convert.ToString(reader["column_name"]);
            col.DB_Type = Convert.ToString(reader["data_type"]);
            col.Length = DataAccess.Convert<int>(reader["character_maximum_length"], -1);
            col.CLR_Type = GetCLRType(col.DB_Type, GetUDTName(reader));
            col.IsArrayType = (col.DB_Type == "ARRAY" ? true : false);
            col.IsSerial = IsSerial(reader);
            col.ColumnIndex = Convert.ToInt32(reader["ordinal_position"]);
            col.IsNullable = Convert.ToString(reader["is_nullable"]) == "YES" ? true : false;
            col.HasDefaultValue = DataAccess.Convert<string>(reader["column_default"], "") == "" ? false : true;
            col.DefaultValue = DataAccess.Convert<string>(reader["column_default"], "");
            col.DbComment = DataAccess.Convert<string>(reader["description"], "none");
            return col;
        }
        #endregion

        #region GetUDTName
        string GetUDTName(IDataReader reader)
        {
            return DataAccess.Convert<string>(reader["udt_name"], "").Replace("_", "").Replace("[]", "");
        }
        #endregion

        #region GetCLRType

        private Type GetCLRType(string db_type, string udt_name)
        {
            switch (db_type)
            {
                case "anyarray":
                    return typeof(object[]);

                case "text":
                case "tsvector":
                case "character":
                case "character varying":
                case "varchar":
                case "interval":
                case "tid":
                case "regprocedure":
                    return typeof(string);

                case "smallint":
                    return typeof(System.Int16);
                case "integer":
                case "int4":
                    return typeof(System.Int32);

                case "bigint":
                case "oid":
                    return typeof(long);

                case "date":
                case "time without time zone":
                case "timestamp with time zone":
                case "timestamp without time zone":
                case "timestamp":
                    return typeof(DateTime);

                case "numeric":
                    return typeof(decimal);

                case "real":
                case "double precision":
                case "float8":
                case "float4":
                    return typeof(double);

                case "boolean":
                    return typeof(bool);

                case "uuid":
                    return typeof(Guid);

                case "bytea":
                    return typeof(byte[]);

                case "ARRAY":
                    switch (udt_name)
                    {
                        case "uuid":
                            return typeof(Guid[]);
                        case "int2":
                        case "int4":
                            return typeof(int[]);
                        case "int8":
                            return typeof(long[]);
                        case "varchar":
                        case "integer":
                        case "text":
                        case "character varying":
                            return typeof(string[]);
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            string message = string.Format("{0}{1} Is not implemented yet. This type will be mapped to CLR typeof(object).",db_type,(udt_name != "" ? "[ " + udt_name + " ]" : ""));
            Warnings.Add(message);
            SendMessage("{0}",BuilderMessageType.Minor,db_type,message);
            if(db_type == "ARRAY")
                return typeof(object[]);
            else
                return typeof(object);
        }
        #endregion

        #region CreateNewTable
        Table CreateNewTable(IDataReader reader)
        {
            Table table = new Table();

            table.IsCompositeType = false;
            table.TableName = Convert.ToString(reader["table_name"]);
            table.IsInsertable = Convert.ToString(reader["is_insertable_into"]) == "YES" ? true : false;
            table.IsView = Convert.ToString(reader["table_type"]) == "VIEW" ? true : false;

            CreateColumnsPerTable(table);

            #region Get primary key columns
            var keys = from k in AllPrimaryKeys
                       where k.TableName == table.TableName
                       select k;

            table.PrimaryKey.TablenName = table.TableName;
            table.PrimaryKey.IndexType = IndexType.PrimaryKey;

            foreach (Column key in keys)
                foreach (Column col in table.Columns)
                    if (col.ColumnName == key.ColumnName)
                    {
                        col.IsEntity = true;
                        table.PrimaryKey.Columns.Add(col);
                        break;
                    }
            #endregion

            #region Get all index columns that are not part of any primary/foreign
            var indexes = from index in AllIndexes
                          where index.TablenName == table.TableName
                          select index;

            foreach (Index index in indexes)
                if (index != table.PrimaryKey)
                    table.Indexes.Add(index);
            #endregion

            #region Get all columns that can be used to insert and update
            foreach (Column col in table.Columns)
                if (!col.IsEntity && !col.IsSerial)
                    table.DMLColumns.Add(col);
                else if (col.IsEntity && !col.IsSerial)
                    table.DMLColumns.Add(col);
            #endregion

            #region Get all foreign key columns
            var fkeys = from f in AllForeignKeys
                        where f.TablenName == table.TableName
                        select f;
            table.ForeignKeys.AddRange(fkeys);
            #endregion
            return table;
        }
        #endregion

        #region CreateColumnsPerTable
        void CreateColumnsPerTable(Table table)
        {
            var columns = from c in AllColumns where c.TableName == table.TableName select c;
            foreach (Column col in columns)
                table.Columns.Add(col);
        }
        #endregion

        #region PrepareViewIndexes
        void PrepareViewIndexes()
        {
            SendMessage("Loading view indexces", BuilderMessageType.Major);
            foreach (Table table in Tables)
            {
                if (table.IsView)
                {
                    #region Prepare view indexes
                    List<Index> addIndex = new List<Index>();
                    var vicols_unclean = from i in AllViewInfo
                                 where i.view_name == table.TableName
                                 select i;

                    //filter duplicate view columns is view_column_usage
                    List<pg_view_column_usage> vicols = new List<pg_view_column_usage>();
                    foreach (pg_view_column_usage vic in vicols_unclean)
                        if (vicols.Find(c => c.column_name == vic.column_name) == null)
                            vicols.Add(vic);
                    
                    // find all indexes for every table that is used in this view
                    foreach (pg_view_column_usage vic in vicols)
                    {
                        Table t = Tables.Find(i => i.TableName == vic.table_name);
                        if (t != null)
                        {
                            if (t.PrimaryKey.Columns.Count != 0)
                                if(!addIndex.Contains(t.PrimaryKey))
                                    addIndex.Add(t.PrimaryKey);

                            foreach(Index idx in t.Indexes)
                                if (!addIndex.Contains(idx))
                                    addIndex.Add(idx);
                        }
                    }
                    #endregion

                    #region Add Table Indexes
                    bool check = false;
                    foreach (Index ai in addIndex)
                    {
                        foreach (Column c in ai.Columns)
                        {
                            // remove the table to make the duplicate check work here
                            ai.TablenName = "";
                            check = (table.Columns.Find(tc => tc.ColumnName == c.ColumnName)) != null;
                            if (!check)
                                break;
                        }

                        if (check)
                            if (table.Indexes.Find(i => i.ToString() == ai.ToString()) == null)
                                table.Indexes.Add(ai);
                    }
                    #endregion

                    Table ccTable = CustomQueries.Find(cc => cc.TableName == table.TableName);
                    if (ccTable != null)
                    {
                        foreach (Index idx in ccTable.Indexes)
                        {
                            Index nIndex = new Index();
                            foreach (Column cc in idx.Columns)
                            {
                                Column nColumn = table.Columns.Find(c => c.ColumnName == cc.ColumnName.Replace("\"", ""));
                                if (nColumn != null)
                                    nIndex.Columns.Add(nColumn);
                            }
                            if (nIndex.Columns.Count != 0)
                                table.Indexes.Add(nIndex);
                        }
                    }
                }
            }
        }
        #endregion

        #region LoadTables
        private void LoadTables()
        {
            Tables = DataAccess.ExecuteObjectQuery<Table>(SQLScripts.GetTableList, CreateNewTable);
            PrepareViewIndexes();
        }
        #endregion

        #region IsSerial
        bool IsSerial(IDataReader reader)
        {
            string t = string.Format("nextval('{0}_{1}_seq'::regclass)",
                Convert.ToString(reader["table_name"]),
                Convert.ToString(reader["column_name"])).Replace("\"", "").Replace("'", "");

            string def = Convert.ToString(reader["column_default"]).Replace("\"", "").Replace("'", "");

            return t.ToLower() == def.ToLower();
        }
        #endregion

        public string GetWarningMessages()
        {
            string result = "";
            foreach (string item in Warnings)
                result += item + "\n";
            return result;
        }
    }
}