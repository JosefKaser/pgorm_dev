/*-------------------------------------------------------------------------
 * pg_view_column_usage.cs
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
    public class pg_view_column_usage
    {
        public string view_name { get; set; }
        public string table_name { get; set; }
        public string column_name { get; set; }
    }
}