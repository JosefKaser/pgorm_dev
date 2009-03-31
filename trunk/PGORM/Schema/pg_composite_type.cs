/*-------------------------------------------------------------------------
 * pg_composite_type.cs
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
    public class pg_composite_type
    {
        public string type_name { get; set; }
        public string column_name { get; set; }
        public string db_type { get; set; }
        public int column_index { get; set; }
    }
}