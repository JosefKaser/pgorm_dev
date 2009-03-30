using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Antlr.StringTemplate;

namespace PGORM
{
    public class AssemblyInfoBuilder : TemplateBase
    {
        public AssemblyInfoBuilder(ProjectFile p_project, VS2008Project p_vsproject, Builder p_builder)
            : base(CodeTemplates.AssemblyInfo_stg, p_vsproject, p_project,p_builder)
        {
        }

        public override void Build()
        {
            SendMessage("Creating AssemblyInfo");
            StringTemplate st = GetTemplate("assembly_info");
            st.SetAttribute("project", project);
            st.SetAttribute("project_guid", vsproject.ProjectGuild);

            string out_dir = project.ProjectOutputFolder + "\\Properties";
            if (!Directory.Exists(out_dir))
            {
                System.Threading.Thread.Sleep(250);
                Directory.CreateDirectory(out_dir);
                System.Threading.Thread.Sleep(250);
            }

            vsproject.AddCompileItem(@"Properties\AssemblyInfo.cs",st.ToString());

        }
    }
}
