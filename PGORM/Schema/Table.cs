/*-------------------------------------------------------------------------
 * Table.cs
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
    public class Table
    {
        public string TableName;
        public List<Column> Columns = new List<Column>();
        public List<Column> DMLColumns = new List<Column>();
        public Index PrimaryKey = new Index();
        public List<Index> Indexes = new List<Index>();
        public List<Index> ForeignKeys = new List<Index>();
        public bool IsInsertable { get; set; }
        public bool IsView { get; set; }
        public bool IsCompositeType { get; set; }

        public string TemplateTableName { get; set; }
        public string FactoryName { get; set; }
        public string RecordSetName { get; set; }

        public string CustomQuerySource { get; set; }
    }
}