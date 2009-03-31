/*-------------------------------------------------------------------------
 * Helper.cs
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
    public class Helper
    {
        public static string EscapeString(string data)
        {
            return data.Replace("\\","\\\\").Replace("\"", "\\\"");
        }

        public static string RemovePrefix(string source, List<string> items)
        {
            foreach (string prefix in items)
                if (source.IndexOf(prefix) == 0)
                {
                    source= source.Replace(prefix, "");
                    return source;
                }
            return source;
        }
    }
}