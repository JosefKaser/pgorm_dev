using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Npgsql;

namespace PostgreSQL
{
    public class CodeHelper
    {
        public static void create_helper()
        {
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_relation.cs", CodeHelper.table_to_cs(SQLScripts.GetAllTablesViews, "pg_relation"));
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_column.cs", CodeHelper.table_to_cs(SQLScripts.GetAllColumns, "pg_column"));
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_type.cs", CodeHelper.table_to_cs(SQLScripts.GetAllTypes, "pg_type"));
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_dindex.cs", CodeHelper.table_to_cs(SQLScripts.GetDistinctIndex, "pg_dindex"));
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_view_depend.cs", CodeHelper.table_to_cs(SQLScripts.GetViewTableDepends, "pg_view_depend"));
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_proc.cs", CodeHelper.table_to_cs(SQLScripts.GetAllFunctions, "pg_proc"));
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_entitycolumn.cs", CodeHelper.table_to_cs(SQLScripts.GetAllEntityColumns, "pg_entitycolumn"));
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_serial.cs", CodeHelper.table_to_cs(SQLScripts.GetAllSerials, "pg_serial"));
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_index.cs", CodeHelper.table_to_cs(SQLScripts.GetAllIndexes, "pg_index"));
            File.WriteAllText(@"C:\Users\Gevik\WorkDir\PGORM\trunk\PostgreSQL\Catalog\pg_column_comment.cs", CodeHelper.table_to_cs(SQLScripts.GetAllColumnComments, "pg_column_comment"));
        }

        public static string table_to_cs(string p_sql, string csname)
        {
            string sql = p_sql;
            NpgsqlCommand command = new NpgsqlCommand(sql, DataAccess.Connection);
            ; NpgsqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
            DataTable schemaTable = reader.GetSchemaTable();
            string prop = "", ritem = "";
            for (int a = 0; a != reader.FieldCount; a++)
            {
                string type_name = reader.GetFieldType(a).Name;
                string field_name = reader.GetName(a);
                string nullable = type_name == "String" || reader.GetFieldType(a).IsArray ? "" : "?";
                prop += string.Format("\t\tpublic {0}{1} {2} {{get; set;}}\r\n",type_name,nullable,field_name);
                ritem += string.Format("\t\t\tresult.{0} = DataAccess.Convert<{1}{2}>(reader[\"{0}\"],null);\r\n",field_name,type_name,nullable);
            }
            reader.Close();
            return string.Format(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace PostgreSQL.Catalog
{{
    internal class {0}
    {{
{1}
        public static {0} FromReader(IDataReader reader)
        {{
            {0} result = new {0}();
{2}
            return result;
        }}
    }}
}}
", csname, prop, ritem);
        }

    }
}
