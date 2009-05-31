using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PGORM.PostgreSQL.Objects
{
    public enum FunctionReturnTypeType
    {
        Table,
        View,
        CompositeType,
        BaseType,
        Void,
        Enum
    }

    public class Function<R, C> : Relation<C>
        where C : Column, new()
        where R : Relation<C>, new()
    {
        public bool IsSetReturning { get; set; }
        public R Arguments { get; set; }
        public FunctionReturnTypeType ReturnTypeType { get; set; }
        public string FullReturnTypeInvariant { get; set; }
        public string ReturnTypeName { get; set; }
        public string ReturnTypeSchemaName { get; set; }
        public bool ColumnDefinitionRequired { get; set; }

        public override string ToString()
        {
            return string.Format("{0}_{1}_{2}_{3}", this.SchemaName, this.RelationName, this.FullReturnTypeInvariant, Guid.NewGuid().ToString("N"));
        }
    }
}
