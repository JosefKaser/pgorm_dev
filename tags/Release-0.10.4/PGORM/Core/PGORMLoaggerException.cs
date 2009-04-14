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
        public string ProjectPath;
        public string FileContent;

        public PGORMLoaggerException(BuildErrorEventArgs p_BuildEventArgs, string p_ProjectPath)
        {
            buildEventArgs = p_BuildEventArgs;
            ProjectPath = p_ProjectPath;
            string failFile = string.Format(@"{0}\{1}", Path.GetDirectoryName(ProjectPath), buildEventArgs.File);
            if(File.Exists(failFile))
                FileContent = File.ReadAllText(failFile);
        }

        public override string ToString()
        {
            return string.Format(CodeTemplates.Exception,
                buildEventArgs.Code,
                buildEventArgs.File,
                buildEventArgs.LineNumber,
                buildEventArgs.Message,
                FileContent);
        }
    }
}