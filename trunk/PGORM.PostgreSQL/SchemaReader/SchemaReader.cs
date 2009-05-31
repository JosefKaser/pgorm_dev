//TODO: check the funationality of column raw type
//TODO: check the functionality of the TypeInfo (use the delimitter is the type type converter and array handling)
//TODO: thing about removing PostfixName
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using Npgsql;
using PGORM.PostgreSQL.Catalog;
using PGORM.PostgreSQL.Objects;

namespace PGORM.PostgreSQL
{
    public partial class SchemaReader<R, S, C>
        where R : Relation<C>, new()
        where S : Function<R,C>, new()
        where C : Column, new()
    {
        #region Properties
        public event SchemaReaderMessageEventHandler Message;
        private Schema<R, S, C> p_Schema;
        private List<R> FunctionArguments;
        #endregion

        #region SchemaReader
        public SchemaReader(string connection_string)
        {
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
        public Schema<R, S, C> ReadSchema()
        {
            p_Schema = new Schema<R, S, C>();
            FunctionArguments = new List<R>();

            CreateRelations();
            CreateTableIndexes();
            CreateViewIndexes();
            CreateFunctions();
            return p_Schema;
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
                            rel.Indexes.Add((Index<C>)index.Clone());
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

        #region CreateRelation
        private R CreateRelation(pg_relation pg_rel, RelationType rtype)
        {
            return new R()
            {
                SchemaName = pg_rel.table_schema,
                RelationName = pg_rel.table_name,
                RelationType = rtype
            };
        } 
        #endregion

        #region CreateRelations

        private void CreateRelations()
        {
            foreach (pg_relation pg_rel in InformationSchema.Relations)
            {
                switch (pg_rel.table_type)
                {
                    case "BASE TABLE":
                        {
                            R table = CreateRelation(pg_rel, RelationType.Table);
                            R udt = CreateRelation(pg_rel, RelationType.CompositeType);

                            p_Schema.Tables.Add(table);
                            p_Schema.CompositeTypes.Add(udt);

                            CreateRelationColumns(table, pg_rel);
                            CreateRelationColumns(udt, pg_rel);
                        }
                        break;

                    case "VIEW":
                        {
                            R view = CreateRelation(pg_rel, RelationType.View);
                            R udt = CreateRelation(pg_rel, RelationType.CompositeType);

                            p_Schema.Views.Add(view);
                            p_Schema.CompositeTypes.Add(udt);

                            CreateRelationColumns(view, pg_rel);
                            CreateRelationColumns(udt, pg_rel);
                        }
                        break;

                    case "USER-DEFINED":
                        {
                            R udt = CreateRelation(pg_rel, RelationType.CompositeType);
                            p_Schema.CompositeTypes.Add(udt);
                            CreateRelationColumns(udt, pg_rel);
                        }
                        break;

                    case "ENUM":
                        {
                            R c_enum = CreateRelation(pg_rel, RelationType.Enum);
                            p_Schema.Enums.Add(c_enum);

                            CreateRelationColumns(c_enum, pg_rel);
                        }
                        break;

                    case "FUNCTION ARGUMENT":
                        {
                            R rel = CreateRelation(pg_rel, RelationType.Function); // this makes CreateRelationColumns to run the correct SQL
                            string[] spinfo = pg_rel.table_name.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            rel.SchemaName = spinfo[1];
                            rel.RelationName = spinfo[2];
                            CreateRelationColumns(rel, pg_rel);
                            FunctionArguments.Add(rel);
                        }
                        break;

                    case "FUNCTION RETURN TYPE":
                        {
                            R rel = CreateRelation(pg_rel, RelationType.CompositeType); // this makes CreateRelationColumns to run the correct SQL
                            CreateRelationColumns(rel, pg_rel);
                            string[] spinfo = pg_rel.table_name.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            rel.SchemaName = spinfo[1];
                            rel.RelationName = spinfo[2];
                            p_Schema.CompositeTypes.Add(rel);
                        }
                        break;

                    default:
                        throw new SchemaNotImplementedException("Relation type {0} {1}{2} not implemented!", pg_rel.table_type, pg_rel.table_schema, pg_rel.table_name);
                }
            }
        }
        #endregion

        #region GetEnumCreationSql
        private string GetEnumCreationSql(List<pg_column> cols)
        {
            string sql = "";
            foreach (pg_column col in cols)
                sql += string.Format("\n\r''::varchar as \"{0}\",", col.column_name);
            sql = string.Format("select {0}", sql.Substring(0, sql.Length - 1));
            return sql;
        }
        #endregion

        #region CreateRelationColumns
        private void CreateRelationColumns(Relation<C> rel, pg_relation pg_rel)
        {
            string sql = "!";

            List<pg_column> rel_cols = InformationSchema.GetColumnsByRelation(pg_rel);

            if (rel.RelationType == RelationType.CompositeType)
                sql = string.Format("select * from {0}.\"{1}:{2}\"()", InformationSchema.TEMP_SCHEMA, pg_rel.table_schema, pg_rel.table_name);
            else if (rel.RelationType == RelationType.Table || rel.RelationType == RelationType.View)
                sql = string.Format("select * from {0} where 1=0", rel.FullName);
            else if (rel.RelationType == RelationType.Enum)
                sql = GetEnumCreationSql(rel_cols);
            else if (rel.RelationType == RelationType.Function)
                sql = string.Format("select * from {0}.\"{1}\"", pg_rel.table_schema, pg_rel.table_name);


            NpgsqlCommand command = new NpgsqlCommand(sql, DataAccess.Connection);
            NpgsqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
            for (int a = 0; a != reader.FieldCount; a++)
            {
                string column_name = reader.GetName(a);
                pg_column rcol = rel_cols.Find(c => c.column_name == column_name);
                pg_column_comment comment = InformationSchema.GetColumnComment(rcol);
                C col = new C();
                col.ColumnName = CorrectUnknownColumn(column_name, a);
                GetCorrectPGAndCLRType(rel, rcol, col, reader.GetFieldType(a));
                col.ColumnIndex = (int)rcol.ordinal_position;
                col.IsSerial = IsSerial(rcol, rel);
                col.DefaultValue = col.IsSerial ? "" : rcol.column_default;
                col.IsNullable = rcol.is_nullable == "YES" ? true : false;
                col.IsEntity = IsPartOfEntity(col, rel);
                col.DB_Comment = (comment == null ? "" : comment.description);
                col.Dimention = (int)rcol.column_dimation;
                CorrectNullableType(col);
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

        #region CorrectNullableType
        /// <summary>
        /// Makes a CLR nullable type if the PG type can be traslated to a CLR base (nullable) type (int, boolean etc..)
        /// </summary>
        /// <param name="column"></param>
        private void CorrectNullableType(C column)
        {
            if (column.IsNullable)
            {
                if (column.PGTypeType != PgTypeType.EnumType &&
                    column.PGTypeType != PgTypeType.CompositeType &&
                    column.CLR_Type != typeof(string) &&
                        !column.IsPgArray)
                {
                    column.CLR_Type = typeof(Nullable<>).MakeGenericType(column.CLR_Type);
                }
            }
        }
        #endregion

        #region GetCorrectPGAndCLRType
        private void GetCorrectPGAndCLRType(Relation<C> rel, pg_column rcol, Column col, Type provided_type)
        {
            pg_type pgtype = InformationSchema.Types.Find(t => t.type_oid == rcol.udt_name_oid);

            col.CLR_Type = provided_type;
            col.CLR_TypeRaw = col.CLR_Type; // do this anyway

            if (rel.RelationType == RelationType.Enum)
                col.PGTypeType = PgTypeType.EnumType;
            else
                col.PGTypeType = PgTypeTypeConverter.FromString(pgtype.type_type);

            col.TypeInfo = CreateTypeInformationFromPgType(pgtype);// do this anyway

            if (rcol.data_type == "USER-DEFINED")
            {
                col.PG_Type = GetCorrectTypeName(rcol.udt_schema, rcol.udt_name, col.PGTypeType);
            }
            else if (rcol.data_type == "ARRAY")
            {
                col.IsPgArray = true;
                pg_type b_pgtype = InformationSchema.Types.Find(t => t.type_oid == pgtype.base_type_oid);
                col.PGTypeType = PgTypeTypeConverter.FromString(b_pgtype.type_type);
                col.PG_Type = GetCorrectTypeName(pgtype.base_type_schema, pgtype.base_type, col.PGTypeType);
                col.TypeInfo = CreateTypeInformationFromPgType(b_pgtype);
            }
            else
                col.PG_Type = GetCorrectTypeName(rcol.udt_schema, rcol.data_type, col.PGTypeType);
        }

        #endregion

        #region GetCorrectTypeName
        public string GetCorrectTypeName(string schema_name, string type_name, PgTypeType type_type)
        {
            if (type_type == PgTypeType.BaseType)
                return type_name;

            if (schema_name == "pg_catalog")
                return type_name;
            else if (schema_name == "public")
                return string.Format("\"public\".\"{0}\"", type_name.Replace(schema_name, ""));
            else
                return string.Format("\"{0}\".\"{1}\"", schema_name, type_name.Replace(schema_name, ""));
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
        bool IsSerial(pg_column col, Relation<C> rel)
        {
            return InformationSchema.Serials.Exists(c =>
                c.column_name == col.column_name &&
                c.table_name == rel.RelationName &&
                c.table_schema == rel.SchemaName);
        }
        #endregion

        #region CreateTypeInformationFromPgType
        private TypeInformation CreateTypeInformationFromPgType(pg_type type)
        {
            TypeInformation t = new TypeInformation();
            t.BaseTypeName = type.base_type;
            t.BaseTypeSchemaName = type.base_type_schema;
            t.Delimiter = type.delimiter;
            t.TypeLongName = type.type_long_name;
            t.TypeNamespace = type.type_namespace;
            t.TypeShortName = type.type_short_name;
            t.TypeType = PgTypeTypeConverter.FromString(type.type_type);
            return t;
        }
        #endregion
    }
}
