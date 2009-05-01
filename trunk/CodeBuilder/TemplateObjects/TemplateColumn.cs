using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostgreSQL.Objects;
using TemplateNS.Core;

namespace CodeBuilder.TemplateObjects
{
    public class TemplateColumn : PostgreSQL.Objects.Column
    {
        public string p_TemplateColumnName;
        public bool HasConverter { get; set; }
        public ConverterProxy ConverterProxy;
        private Schema<TemplateRelation, StoredFunction, TemplateColumn> Schema { get; set; }
        public TemplateRelation Relation;

        #region TemplateColumn
        public TemplateColumn()
            : base()
        {
            HasConverter = false;

        } 
        #endregion

        #region TemplateColumnName
        public string TemplateColumnName
        {
            get
            {
                return p_TemplateColumnName;
            }
        } 
        #endregion

        #region CLR_Nullable
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
        #endregion

        #region GetDBComment
        private string GetDBComment(string c)
        {
            if (!string.IsNullOrEmpty(c))
                return "<para>Database comment: {6}</para>";
            else
                return "<para>{6}</para>";
        } 
        #endregion

        #region CLR_Description
        public string CLR_Description
        {
            get
            {
                string s =
                    "<para>PG Datatype: {0}</para>"
                    + "<para>Is PG array: {8}</para>"
                    + "<para>Is CLR array: {7}</para>"
                    + "<para>PG Type Type: {9}</para>"
                    + "<para>Autonumber: {1}</para>"
                    + "<para>Entity: {2}</para>"
                    + "<para>Nullable: {3}</para>"
                    + "<para>Default value: {4}</para>"
                    + "<para>Length: {5}</para>"
                    + GetDBComment(DB_Comment);
                ;
                return string.Format(s, PG_Type, IsSerial, IsEntity, IsNullable, (DefaultValue != "" ? DefaultValue : "none"), "Length", DB_Comment, CLR_Type.IsArray, IsPgArray, PGTypeType);
            }
        } 
        #endregion

        #region TemplateRelationName
        public string TemplateRelationName { get { return Relation.TemplateRelationName; } }
        public string TemplateSchemaName { get { return Relation.TemplateNamespace; } }
        #endregion        

        #region Prepare
        public void Prepare(ProjectBuilder p_Builder)
        {
            p_TemplateColumnName = Helper.MakeCLRSafe(ColumnName);
            if (Helper.IsReservedWord(ColumnName))
                p_TemplateColumnName = string.Format("_{0}", p_TemplateColumnName);
            Schema = p_Builder.p_Schema;

            //rewrite types if possible
            if (PGTypeType == PostgreSQL.PgTypeType.EnumType && Relation.RelationType != RelationType.Enum)
            {
                ConverterProxy cp = p_Builder.Converters.Find(c => c.PgType == this.TypeInfo.TypeShortName && c.PgTypeSchema == this.TypeInfo.TypeNamespace);
                if (cp != null)
                {
                    HasConverter = true;
                    CLR_Type = cp.CLRType;
                    ConverterProxy = cp;
                }
            }
        } 
        #endregion
    }
}
