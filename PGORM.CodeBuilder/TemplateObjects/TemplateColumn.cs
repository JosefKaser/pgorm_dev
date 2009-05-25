using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGORM.PostgreSQL.Objects;
using PGORM.PostgreSQL;
using TemplateNS.Core;

namespace PGORM.CodeBuilder.TemplateObjects
{
    public class TemplateColumn : PostgreSQL.Objects.Column
    {
        #region Props
        public string p_TemplateColumnName;
        public bool HasConverter { get; set; }
        public ConverterProxy ConverterProxy;
        private Schema<TemplateRelation, Function, TemplateColumn> Schema { get; set; }
        public TemplateRelation Relation; 
        #endregion

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

            //rewrite enum types if possible
            if (PGTypeType == PgTypeType.EnumType && Relation.RelationType != RelationType.Enum)
            {
                ConverterProxy cp = p_Builder.Converters.Find(c => c.PgType == this.TypeInfo.TypeShortName && c.PgTypeSchema == this.TypeInfo.TypeNamespace);
                if (cp != null)
                {
                    HasConverter = true;
                    CLR_Type = cp.CLRType;
                    CLR_TypeRaw = CLR_Type;
                    ConverterProxy = cp;
                }
            }
            else if (PGTypeType != PgTypeType.EnumType && PGTypeType != PgTypeType.BaseType && Relation.RelationType != RelationType.Enum)
            {
                ConverterProxy cp = p_Builder.Converters.Find(c => c.PgType == this.TypeInfo.TypeShortName && c.PgTypeSchema == this.TypeInfo.TypeNamespace);
                if (cp != null)
                {
                    HasConverter = true;
                    CLR_TypeRaw = cp.CLRType;
                    if (!this.IsPgArray)
                    {
                        CLR_Type = cp.CLRType;
                        
                    }
                    else
                    {
                        if(Dimention > 1)
                            CLR_Type = CLR_TypeRaw.MakeArrayType(Dimention); 
                        else
                            CLR_Type = CLR_TypeRaw.MakeArrayType(); 
                    }
                    ConverterProxy = cp;
                }
            }
        } 
        #endregion

        #region CounterIndex
        public int CounterIndex
        {
            get
            {
                return this.ColumnIndex - 1;
            }
        } 
        #endregion

        #region ToString
        public override string ToString()
        {
            return string.Format("{0}.{1}", Relation.FullNameInvariant, TemplateColumnName);
        } 
        #endregion

        public string TemplateCLR_Type
        {
            get
            {
                if (CLR_Type.Name == typeof(Nullable<>).Name)
                {
                    Type innertType = CLR_Type.GetGenericArguments()[0];
                    return string.Format("{0}?", innertType);
                }
                else if (CLR_Type.IsArray && PGTypeType == PgTypeType.CompositeType)
                {
                    string type = string.Format("{0}", CLR_Type);
                    type = type.Replace("*", new String(',', Dimention));
                    return type;
                }
                else
                {
                    return string.Format("{0}", CLR_Type);
                }
            }
        }

    }
}
