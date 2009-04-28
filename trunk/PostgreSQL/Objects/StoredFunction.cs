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
        private int deconflict_index;

        public StoredFunction()
        {
            deconflict_index = 0;
            Arguments = new List<StoredFunctionArgument>();
        }

        public void DeconflictName()
        {
            deconflict_index++;
            FunstionName = string.Format("{0}{1}",FunstionName,deconflict_index);
        }

        public string CLR_Signiture()
        {
            string sarg = "";
            Arguments.ForEach(a => sarg += a.ArgCLRTypeName);
            string s = string.Format("{0}{1}{2}",
                FunstionName,
                sarg,
                Arguments.Count);
            return s;
        }
    }
}
