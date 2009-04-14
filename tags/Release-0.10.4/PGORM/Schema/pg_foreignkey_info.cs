/*-------------------------------------------------------------------------
 * pg_foreignkey_info.cs
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
    class pg_foreignkey_info
    {
        public string constraint_name { get; set; }
        public string table_name { get; set; }
        public string foreign_table_name { get; set; }
        public string column_name { get; set; }
        public short[] local_keys { get; set; }
        public short[] foreign_keys { get; set; }
    }
}