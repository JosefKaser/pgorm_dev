/*-------------------------------------------------------------------------
 * Column.cs
 *
 * This file is part of the PGORM project.
 * http://pgorm.googlecode.com/
 *
 * Copyright (c) 2002-2009, TrueSoftware B.V.
 *
 * IDENTIFICATION
 * 
 *  $Id$
 * 	$HeadURL$
 * 	
 *-------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.StringTemplate;

namespace PGORM
{
    public class Column
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public Type CLR_Type { get; set; }
        public string DB_Type { get; set; }
        public int ColumnIndex { get; set; }
        public bool IsSerial { get; set; }
        public bool IsEntity { get; set; }
        public bool IsNullable { get; set; }
        public bool HasDefaultValue { get; set; }
        public string DefaultValue { get; set; }
        public int Length { get; set; }
        public string DbComment { get; set; }
        public bool IsArrayType { get; set; }
        public string CLR_Description
        {
            get
            {
                string s = 
                    "<para>Comment: {6}</para>"
                    + "<para>Datatype: {0}</para>"
                    + "<para>Is array: {7}</para>"
                    + "<para>Autonumber: {1}</para>"
                    + "<para>Entity: {2}</para>"
                    + "<para>Nullable: {3}</para>"
                    + "<para>Default: {4}</para>"
                    + "<para>Length: {5}</para>"
                    ;
                return string.Format(s, DB_Type, IsSerial, IsEntity, IsNullable, (HasDefaultValue ? DefaultValue : "none"), Length, DbComment,IsArrayType);
            }
        }

        public string CLR_Nullable
        {
            get
            {
                if (
                    IsNullable && 
                    CLR_Type != typeof(string) && 
                    CLR_Type != typeof(object) &&
                    CLR_Type != typeof(byte[]) && 
                    !IsArrayType)
                    return "?";
                else
                    return "";
            }
        }
        public string TemplateColumnName { get; set; }
        public string TemplateTableName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:Database type:{1}, Is Serial:{2}", ColumnName, DB_Type, IsSerial);
        }
    }
}