using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeBuilder.TemplateObjects;
using Antlr.StringTemplate;
using System.IO;
using PostgreSQL.Objects;

namespace CodeBuilder
{
    public class CLREnumBuilder : TemplateBase
    {
        StringTemplate st;


        public CLREnumBuilder(ProjectBuilder p_builder, string enum_namespace)
            : base(
                Templates.CLREnum_stg, p_builder)
        {
            st = p_stgGroup.GetInstanceOf("create_enum");
        }

        #region Reset
        public void Reset()
        {
            st.Reset();
        }
        #endregion

        #region Create
        public void Create(TemplateRelation relation, string destFolder)
        {
            string nspace = Helper.GetExplicitNamespace(p_Project, relation);
            string fname = string.Format(@"{0}\{1}_{2}_enum.cs", destFolder, relation.SchemaName, relation.TemplateRelationName);
            st.SetAttribute("table", relation);
            st.SetAttribute("namespace", Helper.GetExplicitNamespace(p_Project, relation));
            st.SetAttribute("libs", p_Project.InternalReferences);
            st.SetAttribute("libs", string.Format("{0}.Factory", nspace));

            File.WriteAllText(fname, st.ToString());
        }
        #endregion
    }
}
