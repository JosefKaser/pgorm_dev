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
    public class RecordSetBuilder : TemplateBase
    {
        StringTemplate st;
        string[] p_libs;

        public RecordSetBuilder(ProjectBuilder p_builder, string[] p_Libs)
            : base(Templates.DataObjectRecordSet_stg, p_builder)
        {
            st = p_stgGroup.GetInstanceOf("recordset");
            p_libs = p_Libs;
        }

        public void Create(TemplateRelation relation, string destFolder)
        {
            string nspace = Helper.GetExplicitNamespace(p_Project, relation);
            string fname = string.Format(@"{0}\{1}_{2}_recordset.cs", destFolder, relation.SchemaName, relation.TemplateRelationName);
            st.Reset();
            st.SetAttribute("table", relation);
            st.SetAttribute("namespace", Helper.GetExplicitNamespace(p_Project, relation));
            st.SetAttribute("libs", p_Project.InternalReferences);
            st.SetAttribute("libs", string.Format("{0}.RecordSet", nspace));

            // if this object is a UDT then we do not need the Entities namespace
            if (relation.RelationType == RelationType.CompositeType)
                SetLibs(st, Helper.RemoveArrayItemIfExist(p_libs, ProjectBuilder.p_ObjectNamespace), relation);
            else
                SetLibs(st, p_libs, relation);

            File.WriteAllText(fname, st.ToString());
        }
    }
}
