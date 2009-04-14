/*-------------------------------------------------------------------------
 * pg_proc.cs
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
    public class pg_proc
    {
        public string proname { get; set; }
        public string[] arg_types { get; set; }
        public string return_type { get; set; }
        public string return_type_type { get; set; }
        public string[] proargnames { get; set; }
        public int num_args { get; set; }
        public bool returns_set { get; set; }
    }
}