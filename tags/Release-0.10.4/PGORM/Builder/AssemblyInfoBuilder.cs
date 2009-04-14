/*-------------------------------------------------------------------------
 * AssemblyInfoBuilder.cs
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
using System.IO;
using Antlr.StringTemplate;

namespace PGORM
{
    public class AssemblyInfoBuilder : TemplateBase
    {
        public AssemblyInfoBuilder(ProjectFile p_project, VS2008Project p_vsproject, Builder p_builder)
            : base(CodeTemplates.AssemblyInfo_stg, p_vsproject, p_project,p_builder)
        {
        }

        public override void Build()
        {
            SendMessage("Creating AssemblyInfo...",BuilderMessageType.Major);
            StringTemplate st = GetTemplate("assembly_info");
            st.SetAttribute("asm", project.AssemblyInfo);

            string out_dir = project.ProjectOutputFolder + "\\Properties";
            if (!Directory.Exists(out_dir))
            {
                System.Threading.Thread.Sleep(250);
                Directory.CreateDirectory(out_dir);
                System.Threading.Thread.Sleep(250);
            }

            vsproject.AddCompileItem(@"Properties\AssemblyInfo.cs",st.ToString());

        }
    }
}