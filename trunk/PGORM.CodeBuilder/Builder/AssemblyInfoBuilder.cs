using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.StringTemplate;

namespace PGORM.CodeBuilder
{
    class AssemblyInfoBuilder : TemplateBase
    {
        private AssemblyInfoData p_Info;

        public AssemblyInfoBuilder(AssemblyInfoData info, ProjectBuilder p_builder)
            : base(Templates.AssemblyInfo_stg, p_builder)
        {
            p_Info = info;
        }

        public override string BuildToString()
        {
            StringTemplate st = p_stgGroup.GetInstanceOf("assembly_info");
            st.SetAttribute("asm", p_Info);
            return st.ToString();
        }

    }
}
