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

        private R Void_ReturnType()
        {
            R rel = new R();
            rel.RelationName = "void";
            rel.SchemaName = "";
            return rel;
        }

        private void CreateFunctions()
        {
            foreach (pg_proc proc in InformationSchema.Functions)
            {
                S sp = new S();
                sp.SchemaName = proc.schema_name;
                sp.RelationName = proc.function_name;
                sp.RelationType = RelationType.Function;
                sp.IsSetReturning = (bool)proc.returns_set;
                sp.Arguments = FunctionArguments.Find(i => i.RelationName == proc.function_name && i.SchemaName == proc.schema_name);
                sp.ReturnTypeType = GetFunctionReturnTypeType(proc);
                sp.FullReturnTypeInvariant = string.Format("{0}.{1}", proc.return_type_schema, proc.return_type);
                sp.ReturnTypeName = proc.return_type;
                sp.ReturnTypeSchemaName = proc.return_type_schema;
                sp.ArgNamesWithDefaults = proc.name_args_with_defaults.ToList();
                sp.ArgNumWithDefaults = (int)proc.num_args_with_defaults;

                //if a function returns setof record then it should be called with column definition;
                sp.ColumnDefinitionRequired = (proc.return_type_type == "RECORD" && proc.return_type == "record" && (bool)proc.returns_set == true);

                p_Schema.Functions.Add(sp);
            }
        }

        private FunctionReturnTypeType GetFunctionReturnTypeType(pg_proc proc)
        {
            switch (proc.return_type_type)
            {
                case "TABLE":
                    return FunctionReturnTypeType.Table;
                case "VIEW":
                    return FunctionReturnTypeType.View;
                case "BASE":
                    return FunctionReturnTypeType.BaseType;
                case "RECORD":
                case "UDT":
                    return FunctionReturnTypeType.CompositeType;
                case "VOID":
                    return FunctionReturnTypeType.Void;
                case "ENUM":
                    return FunctionReturnTypeType.Enum;
                default:
                    throw new SchemaNotImplementedException("Unknwon function return type type [{0}]", proc.return_type_type);
            }
        }
    }
}