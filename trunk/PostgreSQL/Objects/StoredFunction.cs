using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostgreSQL.Objects
{
    public class StoredFunctionArgument
    {
        public string ArgName { get; set; }
        public string ArgPgTypeName { get; set; }
        public string ArgCLRTypeName { get; set; }
    }

    public enum StoredFunctionReturnType
    {
        Table,
        View,
        CompositeType,
        CLRType,
        Void,
        PgEnum
    }

    public class StoredFunction
    {
        public string FunstionName { get; set; }
        public string ReturnTypeCLRName { get; set; }
        public bool IsSetReturning { get; set; }
        public StoredFunctionReturnType ReturnTypeType { get; set; }
        public List<StoredFunctionArgument> Arguments { get; set; }

        public StoredFunction()
        {
            Arguments = new List<StoredFunctionArgument>();
        }
    }
}
