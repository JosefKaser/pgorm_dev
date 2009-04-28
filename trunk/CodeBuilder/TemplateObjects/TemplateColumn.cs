using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeBuilder.TemplateObjects
{
    public class TemplateColumn : PostgreSQL.Objects.Column
    {
        public string p_TemplateColumnName;

        public string TemplateColumnName
        {
            get
            {
                return p_TemplateColumnName;
            }
        }

        public string CLR_Nullable
        {
            get
            {

                if (!CLR_Type.IsArray && CLR_Type != typeof(string))
                {
                    return "?";
                }
                else
                {
                    return "";
                }
            }
        }

        private string GetDBComment(string c)
        {
            if (!string.IsNullOrEmpty(c))
                return "<para>Database comment: {6}</para>";
            else
                return "<para>{6}</para>";
        }

        public string CLR_Description
        {
            get
            {
                string s =
                    "<para>Datatype: {0}</para>"
                    + "<para>Is array: {7}</para>"
                    + "<para>Is PG array: {8}</para>"
                    + "<para>Autonumber: {1}</para>"
                    + "<para>Entity: {2}</para>"
                    + "<para>Nullable: {3}</para>"
                    + "<para>Default value: {4}</para>"
                    + "<para>Length: {5}</para>"
                    + GetDBComment(DB_Comment);
                    ;
                return string.Format(s, PG_Type, IsSerial, IsEntity, IsNullable, (DefaultValue != "" ? DefaultValue : "none"), "Length", DB_Comment, CLR_Type.IsArray,IsPgArray);
            }
        }

        public string TemplateRelationName { get; set; } 
        

        public void Prepare(Project p_Project)
        {
            p_TemplateColumnName = ColumnName;
            if (Helper.IsReservedWord(ColumnName))
                p_TemplateColumnName = string.Format("_{0}", p_TemplateColumnName);
        }
    }
}
