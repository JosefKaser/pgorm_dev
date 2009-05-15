using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGORM.CodeBuilder.TemplateObjects;
using Antlr.StringTemplate;
using System.IO;

namespace PGORM.CodeBuilder
{
    public class DataObjectBuilder : TemplateBase
    {
        StringTemplate st;
        string[] p_libs;
        public DataObjectBuilder(ProjectBuilder p_builder,string[] p_Libs)
            : base(Templates.DataObject_stg, p_builder)
        {
            st = p_stgGroup.GetInstanceOf("dataobject");
            p_libs = p_Libs;
        }

        public void Create(TemplateRelation relation,string object_namespace, string destFolder)
        {
            if (!string.IsNullOrEmpty(object_namespace))
                object_namespace = "." + object_namespace;

            string nspace = string.Format("{0}{1}", Helper.GetExplicitNamespace(p_Project, relation), object_namespace);
            string fname = string.Format(@"{0}\{1}_{2}_{3}.cs", destFolder, relation.SchemaName, relation.TemplateRelationName,relation.PostFixName).ToLower();
            st.Reset();
            st.SetAttribute("table", relation);
            st.SetAttribute("namespace", nspace);
            st.SetAttribute("libs", p_Project.InternalReferences);

            foreach (string lib in p_libs)
                st.SetAttribute("libs", string.Format("{0}.{1}", Helper.GetExplicitNamespace(p_Project, relation), lib));

            File.WriteAllText(fname, st.ToString());
        }
    }
}
