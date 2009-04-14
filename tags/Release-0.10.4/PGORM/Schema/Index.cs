/*-------------------------------------------------------------------------
 * Index.cs
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

namespace PGORM
{
    public enum IndexType
    {
        PrimaryKey,
        ForeignKey,
        Unique,
        NotUnique
    }

    public class Index
    {
        public string TablenName { get; set; }
        public string ForeignTableName { get; set; }
        public IndexType IndexType { get; set; }
        public List<Column> Columns = new List<Column>();

        // this is needed to filter foreign key indexes that are also primarykey
        public string ToStringNoTable()
        {
            string s = "";
            foreach (Column c in Columns)
                s += c.ColumnName;
            return s;
        }

        public override string ToString()
        {
            string s = TablenName;
            foreach (Column c in Columns)
                s += c.ColumnName;
            return s;
        }
    }
}