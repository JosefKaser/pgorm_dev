using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeBuilder.TemplateObjects;
using Antlr.StringTemplate;
using System.IO;

namespace CodeBuilder
{
    public class DataObjectBuilder : TemplateBase
    {
        StringTemplate st;
        string p_ObjectNamespace;
        public DataObjectBuilder(ProjectBuilder p_builder,string object_namespace)
            : base(Templates.DataObject_stg, p_builder)
        {
            st = p_stgGroup.GetInstanceOf("dataobject");
            p_ObjectNamespace = object_namespace;

            if (!string.IsNullOrEmpty(p_ObjectNamespace))
                p_ObjectNamespace = "." + p_ObjectNamespace;

        }

        public void Create(TemplateRelation relation, string destFolder)
        {
            string nspace = string.Format("{0}{1}", Helper.GetExplicitNamespace(p_Project, relation), p_ObjectNamespace);
            string fname = string.Format(@"{0}\{1}_{2}_{3}.cs", destFolder, relation.SchemaName, relation.TemplateRelationName,relation.PostFixName).ToLower();
            st.Reset();
            st.SetAttribute("table", relation);
            st.SetAttribute("namespace", nspace);
            st.SetAttribute("libs", p_Project.InternalReferences);
            File.WriteAllText(fname, st.ToString());
        }
    }
}
