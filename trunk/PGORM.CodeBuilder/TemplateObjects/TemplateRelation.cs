using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGORM.PostgreSQL.Objects;
using PGORM.CodeBuilder;


namespace PGORM.CodeBuilder.TemplateObjects
{
    public class TemplateRelation : PostgreSQL.Objects.Relation<TemplateColumn>
    {
        private string p_TemplateRelationName;
        public string TemplateRelationName
        {
            get
            {
                return p_TemplateRelationName;
            }
        }

        public string TemplateNamespace
        {
            get
            {
                return SchemaName.ToUpper();
            }
        }

        private string p_RecordsetName;
        public string RecordsetName
        {
            get
            {
                return p_RecordsetName;
            }
        }

        public string PostFixName
        {
            get
            {
                if (RelationType == RelationType.View)
                    return "View";
                else if (RelationType == RelationType.CompositeType)
                    return "UDT";
                else if (RelationType == RelationType.Enum)
                    return "";
                else
                    return "Object";

            }
        }

        private string p_FactoryName;
        public string FactoryName
        {
            get
            {
                return p_FactoryName;
            }
        }

        public List<Column> DMLColumns { get; set; }

        public void Prepare(ProjectBuilder p_Builder)
        {
            p_TemplateRelationName = Helper.MakeCLRSafe(Helper.RemovePrefix(RelationName, p_Builder.p_Project.RemoveTablePrefix));
            p_RecordsetName = string.Format("{0}_{1}RecordSet", TemplateRelationName,PostFixName);
            p_FactoryName = string.Format("{0}_{1}Factory", TemplateRelationName, PostFixName);
            p_TemplateRelationName = string.Format("{0}{1}", p_TemplateRelationName,PostFixName);

            foreach (TemplateColumn col in this.Columns)
            {
                col.Relation = this;
                col.Prepare(p_Builder);
            }

            foreach (Index<TemplateColumn> index in Indexes)
            {
                foreach (TemplateColumn col in index.Columns)
                {
                    col.Relation = this;
                    //col.TemplateRelationName = p_TemplateRelationName;
                    //col.TemplateSchemaName = this.TemplateNamespace; ;
                    col.Prepare(p_Builder);
                }
            }

            DMLColumns = new List<Column>();
            #region Get all columns that can be used to insert and update
            foreach (Column col in this.Columns)
                if (!col.IsEntity && !col.IsSerial)
                    DMLColumns.Add(col);
                else if (col.IsEntity && !col.IsSerial)
                    DMLColumns.Add(col);
            #endregion
        }
    }
}
