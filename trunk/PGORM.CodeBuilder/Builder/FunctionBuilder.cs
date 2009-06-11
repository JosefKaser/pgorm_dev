using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGORM.CodeBuilder.TemplateObjects;
using Antlr.StringTemplate;
using System.IO;
using PGORM.PostgreSQL.Objects;

namespace PGORM.CodeBuilder
{
    class FunctionBuilder : TemplateBase
    {
        StringTemplate st;
        string[] p_libs;

        public FunctionBuilder(ProjectBuilder p_builder, string[] p_Libs)
            : base(GetTemplate(), p_builder)
        {
            st = p_stgGroup.GetInstanceOf("func");
            p_libs = p_Libs;            
        }

        public static string GetTemplate()
        {
            return string.Format("{0}\n{1}\n",
                Templates.Function_Function_stg,
                Templates.Function_Command_stg);
        }

        public void Create(TemplateFunction function, string destFolder,bool transaction_support)
        {
            st.Reset();
            st.SetAttribute("function", function);
            st.SetAttribute("namespace", Helper.GetExplicitNamespace(p_Project, function));
            st.SetAttribute("libs", p_Project.InternalReferences);
            st.SetAttribute("libs", p_libs);
            if (function.Arguments != null)
                st.SetAttribute("args", function.TemplateArguments);

            st.SetAttribute("trans_support", transaction_support);

            string fname = string.Format(@"{0}\{1}_function.cs", destFolder,function.ToString());
            File.WriteAllText(fname, st.ToString());
        }
    }
}
