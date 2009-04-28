﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using Npgsql;
using PostgreSQL.Catalog;
using PostgreSQL.Objects;

namespace PostgreSQL
{
    public partial class SchemaReader<R,S,C> where R : Relation<C> , new() where S : StoredFunction,new()  where C : Column , new()
    {
        #region Properties
        public event SchemaReaderMessageEventHandler Message;
        public event SchemaReaderResolvePGTypeEventHandler ResolvePgType;
        private Schema<R,S,C> p_Schema;
        private List<Relation<C>> sp_arguments;
        #endregion

        #region SchemaReader
        public SchemaReader(string connection_string)
        {
            sp_arguments = new List<Relation<C>>();
            DataAccess.InitializeDatabase(connection_string);
            InformationSchema.Read();
        } 
        #endregion

        #region OnMessage
        private void OnMessage(SchemaReaderMessageEventType type, string message, params object[] args)
        {
            if (Message != null)
                Message(new SchemaReaderEventArgs(type, string.Format(message, args)));
#if DEBUG
            Debug.WriteLine(string.Format(message, args));
#endif
        }
        #endregion

        #region ReadSchema
        public Schema<R,S,C> ReadSchema()
        {
            p_Schema = new Schema<R,S,C>();

            CreateTablesAndViews();
            CreateTableIndexes();
            CreateViewIndexes();
            CreateStoredFunctions();
            return p_Schema;
        }
        
        #endregion

        #region CreateStoredFunctions
        private void CreateStoredFunctions()
        {
            foreach (Relation<C> item in sp_arguments)
            {
                string[] sig = InformationSchema.GetFunstionSigniture(item.RelationName);
                item.RelationName = sig[0];
            }

            foreach (pg_proc proc in InformationSchema.Functions)
            {
                S func = new S();
                Relation<C> arg_info = sp_arguments.Find(s => s.RelationName == proc.proc_oid.ToString());
                if (arg_info != null)
                    foreach (Column col in arg_info.Columns)
                    {
                        StoredFunctionArgument arg = new StoredFunctionArgument();
                        arg.ArgName = col.ColumnName;
                        arg.ArgPgTypeName = col.PG_Type;
                        arg.ArgCLRTypeName = col.CLR_Type.Name;
                        func.Arguments.Add(arg);
                    }
                func.FunstionName = proc.proname;
                func.IsSetReturning = (bool)proc.returns_set;

                if (SetFunstionReturnType(proc, func))
                    p_Schema.StoredFunctions.Add(func);
            }

            // deconflict functions
            DeConflictStoredFunctions();
        } 
        #endregion

        #region DeConflictStoredFunctions
        private void DeConflictStoredFunctions()
        {
            foreach (StoredFunction func in p_Schema.StoredFunctions)
            {
                // and any function that has the same signiture. 
                // Meaning after creating from pg_proc the CLR represenation
                // is conflicting.
                StoredFunction cfunc = p_Schema.StoredFunctions.Find(f => f != func && f.CLR_Signiture() == func.CLR_Signiture());
                if (cfunc != null)
                    cfunc.DeconflictName();
            }
        } 
        #endregion

        #region SetFunstionReturnType
        private bool SetFunstionReturnType(pg_proc proc, StoredFunction func)
        {
            if ((bool)proc.is_table)
            {
                func.ReturnTypeType = StoredFunctionReturnType.Table;
                func.ReturnTypeCLRName = proc.return_type;
            }
            else if ((bool)proc.is_view)
            {
                func.ReturnTypeType = StoredFunctionReturnType.View;
                func.ReturnTypeCLRName = proc.return_type;
            }
            else if ((bool)proc.is_void)
            {
                func.ReturnTypeType = StoredFunctionReturnType.Void;
                func.ReturnTypeCLRName = proc.return_type;
            }
            else if ((bool)proc.is_composite)
            {
                func.ReturnTypeType = StoredFunctionReturnType.CLRType;
                func.ReturnTypeCLRName = proc.return_type;
            }
            else if ((bool)proc.is_enum)
            {
                func.ReturnTypeType = StoredFunctionReturnType.PgEnum;
                func.ReturnTypeCLRName = proc.return_type;
            }
            else if (proc.return_type_type == "b")
            {
                func.ReturnTypeType = StoredFunctionReturnType.CLRType;
                pg_column col = new pg_column();
                col.data_type = proc.return_type;
                col.column_name = string.Format("return_type_{0}", func.FunstionName);
                List<pg_column> cols = new List<pg_column>();
                cols.Add(col);
                string sql = CreateCompositeTypeTemplateSQL(cols);
                NpgsqlCommand command = new NpgsqlCommand(sql, DataAccess.Connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                func.ReturnTypeCLRName = reader.GetFieldType(0).Name;
                reader.Close();
            }
            else
            {
                OnMessage(SchemaReaderMessageEventType.Error, "Return type for function {0} is not implemented in this version.", proc.proname);
                return false;
            }
            return true;
        } 
        #endregion

        #region CreateViewIndexes
        private void CreateViewIndexes()
        {
            foreach (Relation<C> rel in p_Schema.Views)
            {
                List<Relation<C>> DepTables = GetViewDependedTables(rel);
                foreach (Relation<C> dtable in DepTables)
                {
                    foreach (Index<C> index in dtable.Indexes)
                        if (QualifiesAsIndexForRelation(index, rel) && !rel.Indexes.Exists(i => i.Equals(index)))
                            rel.Indexes.Add(index);
                }
            }
        } 
        #endregion

        #region CreateTableIndexes
        private void CreateTableIndexes()
        {
            // Get all DML indexes
            foreach (Relation<C> rel in p_Schema.Tables)
            {
                // get indexes b this table
                List<pg_index> dindexes = InformationSchema.Indexes.FindAll(
                    i => i.table_name == rel.RelationName && i.table_namespace == rel.SchemaName);

                // for every index create index object and get columns from this table
                foreach (pg_index idx in dindexes)
                {
                    // exclude indexes on hidden columns like oid
                    List<int> hidden_check = idx.constraint_keys.ToList<int>();
                    if (!hidden_check.Exists(i => i < 0))
                    {
                        Index<C> index = new Index<C>();
                        index.IndexType = GetIndexType(idx.constraint_type);
                        index.IndexName = idx.constraint_name;

                        foreach (int col_index in idx.constraint_keys)
                            index.Columns.Add(rel.Columns.Find(c => c.ColumnIndex == col_index));

                        if (index.Columns.Count != 0)
                            rel.Indexes.Add(index);
                    }
                }
            }
        }
        #endregion

        #region GetIndexType
        private IndexType GetIndexType(string t)
        {
            if (t == "p")
                return IndexType.PrimaryKey;
            if (t == "f")
                return IndexType.ForeignKey;
            if (t == "u")
                return IndexType.UniqueIndex;

            if (t == "i")
                return IndexType.CustomIndex;

            throw new SchemaNotImplementedException(string.Format("Index type ({0}) is not implemented in this version", t));
        } 
        #endregion

        #region GetViewDependedTables
        public List<Relation<C>> GetViewDependedTables(Relation<C> rel)
        {
            List<pg_view_depend> dep_tables = InformationSchema.ViewTableDepends.FindAll(d => d.view_name == rel.RelationName && d.view_schema == rel.SchemaName);
            List<Relation<C>> result = new List<Relation<C>>();
            foreach (pg_view_depend dep in dep_tables)
            {
                Relation<C> r = p_Schema.Tables.Find(t => t.SchemaName == dep.table_schema && t.RelationName == dep.table_name);
                if (r != null)
                    result.Add(r);
            }
            return result;
        } 
        #endregion

        #region QualifiesAsIndexForRelation
        private bool QualifiesAsIndexForRelation(Index<C> index, Relation<C> rel)
        {
            bool result = true;
            //for every column in this index check for same column existance in relation
            foreach (Column icol in index.Columns)
            {
                if (!rel.Columns.Exists(c => c.ColumnName == icol.ColumnName))
                {
                    result = false;
                    break;
                }
            }
            return result;
        } 
        #endregion

        #region CreateTablesAndViews
        private void CreateTablesAndViews()
        {
            foreach (pg_relation pg_rel in InformationSchema.Relations)
            {
                R relation = new R();
                relation.SchemaName = pg_rel.table_schema;
                relation.RelationName = pg_rel.table_name;

                if (pg_rel.table_type == "BASE TABLE")
                {
                    relation.RelationType = RelationType.Table;
                    p_Schema.Tables.Add(relation);
                }
                else if (pg_rel.table_type == "VIEW")
                {
                    relation.RelationType = RelationType.View;
                    p_Schema.Views.Add(relation);
                }
                else if (pg_rel.table_type == "USER-DEFINED")
                {
                    relation.RelationType = RelationType.CompositeType;
                    p_Schema.CompositeTypes.Add(relation);
                }
                else if (pg_rel.table_type == "ENUM")
                {
                    relation.RelationType = RelationType.Enum;
                    p_Schema.Enums.Add(relation);
                }
                else if (pg_rel.table_type == "SP ARGUMENT")
                {
                    string[] s = InformationSchema.GetFunstionSigniture(relation.RelationName);
                    sp_arguments.Add(relation);
                }

                CreateRelationColumns(relation, pg_rel);
            }
        } 
        #endregion

        #region CreateCompositeTypeTemplateSQL
        private string CreateCompositeTypeTemplateSQL(List<pg_column> columns)
        {
            string sql = "",data_type="";
            foreach (pg_column col in columns)
            {
                if (col.data_type == "USER-DEFINED")
                    data_type = col.udt_name;
                if (col.data_type == "ARRAY")
                {
                    SetCorrectPgTypeWhenArray(col);
                    data_type = col.data_type;
                }
                else
                    data_type = col.data_type;
                sql += string.Format("\r\nnull::{0} as \"{1}\",", data_type, col.column_name);
            }
            sql = string.Format("select {0} ", sql.Substring(0, sql.Length - 1));
            return sql;
        } 
        #endregion

        #region CreateRelationColumns
        private void CreateRelationColumns(Relation<C> rel, pg_relation pg_rel)
        {
            string sql = "";
            
            List<pg_column> rel_cols = InformationSchema.GetColumnsByRelation(pg_rel);

            if (rel.RelationType == RelationType.CompositeType || rel.RelationType == RelationType.Enum )
            {
                sql = CreateCompositeTypeTemplateSQL(rel_cols);
            }
            else
            {
                sql = string.Format("select * from {0} where 1=0", rel.FullName);
            }


            NpgsqlCommand command = new NpgsqlCommand(sql, DataAccess.Connection);
            NpgsqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
            for (int a = 0; a != reader.FieldCount; a++)
            {
                string column_name = reader.GetName(a);
                pg_column rcol = rel_cols.Find(c => c.column_name == column_name);
                pg_column_comment comment = InformationSchema.GetColumnComment(rcol);
                C col = new C();
                col.ColumnName = CorrectUnknownColumn(column_name, a);
                GetCorrectPGAndCLRType(rcol, col, reader.GetFieldType(a));
                col.ColumnIndex = (int)rcol.ordinal_position;
                col.IsSerial = IsSerial(rcol,rel);
                col.DefaultValue = col.IsSerial ? "" : rcol.column_default;
                col.IsNullable = rcol.is_nullable == "YES" ? true : false;
                col.IsEntity = IsPartOfEntity(col, rel);
                col.DB_Comment = (comment == null ? "" : comment.description);
                rel.Columns.Add(col);
            }
            reader.Close();
        } 
        #endregion

        #region IsPartOfEntity
        private bool IsPartOfEntity(Column col, Relation<C> rel)
        {
            return InformationSchema.EntityColumns.Exists
                (
                    c => c.column_name == col.ColumnName &&
                    c.table_name == rel.RelationName &&
                    c.table_schema == rel.SchemaName
                    );
        } 
        #endregion

        void SetCorrectPgTypeWhenArray(pg_column rcol)
        {
            pg_type ptype = InformationSchema.GetPgTypeByName(rcol.udt_name);
            rcol.data_type = ptype.type_long_name;
        }

        void SetCorrectPgTypeWhenArray(pg_column rcol, Column col)
        {
            pg_type ptype = InformationSchema.GetPgTypeByName(rcol.udt_name);
            col.PG_Type = ptype.type_long_name;
        }

        #region GetCorrectPGAndCLRType
        private void GetCorrectPGAndCLRType(pg_column rcol, Column col, Type provided_type)
        {
            if (rcol.data_type != "USER-DEFINED")
            {
                if (rcol.data_type == "ARRAY" || rcol.data_type.Contains("[]"))
                {
                    SetCorrectPgTypeWhenArray(rcol, col);
                    col.IsPgArray = true;
                }
                else
                {
                    col.PG_Type = rcol.data_type;
                }
                col.CLR_Type = provided_type;
            }
            else
            {
                col.PG_Type = rcol.udt_name;
                col.CLR_Type = provided_type; // do this anyway
                pg_type ptype = InformationSchema.GetPgTypeByName(rcol.udt_name);

                if (ResolvePgType != null)
                {
                    SchemaReaderResolvePGTypeEventArgs e = new SchemaReaderResolvePGTypeEventArgs(rcol.udt_name);
                    ResolvePgType(e);
                    if (e.Converter != null)
                        col.CLR_Type = e.Converter.CLR_Type();
                }
            }
        }
        
        #endregion

        #region CorrectUnknownColumn
        private string CorrectUnknownColumn(string org_name, int index)
        {
            string r = org_name;
            if (r == "?column?")
                r = "unnamed_column" + index;
            return r;
        }
        #endregion

        #region IsSerial
        bool IsSerial(pg_column col,Relation<C> rel)
        {
            return InformationSchema.Serials.Exists(c =>
                c.column_name == col.column_name &&
                c.table_name == rel.RelationName &&
                c.table_schema == rel.SchemaName);
        }
        #endregion
    }
}
