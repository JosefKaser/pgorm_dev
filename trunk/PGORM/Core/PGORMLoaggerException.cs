/*-------------------------------------------------------------------------
 * PGORMLoaggerException.cs
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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;

namespace PGORM
{
    public class PGORMLoaggerException : Exception
    {
        public BuildErrorEventArgs buildEventArgs { get; set; }

        public PGORMLoaggerException(BuildErrorEventArgs p_BuildEventArgs)
        {
            buildEventArgs = p_BuildEventArgs;
        }

        public override string ToString()
        {
            return string.Format(CodeTemplates.Exception,
                buildEventArgs.Code,
                buildEventArgs.ColumnNumber,
                buildEventArgs.EndColumnNumber,
                buildEventArgs.EndLineNumber,
                buildEventArgs.File,
                buildEventArgs.LineNumber,
                buildEventArgs.Message);
        }
    }
}