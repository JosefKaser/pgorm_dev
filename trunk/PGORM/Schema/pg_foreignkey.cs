/*-------------------------------------------------------------------------
 * pg_foreignkey.cs
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
    class pg_foreignkey
    {
        public string constraint_name { get; set; }
        public string table_name { get; set; }
    }
}