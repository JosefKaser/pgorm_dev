/*-------------------------------------------------------------------------
 * Program.cs
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
using System.Diagnostics;
using PGORM;

namespace Dellstore2_Builder
{
    class Program
    {
        // Don't forget to make the builder application static thread.
        // This is needed in order to use the MSBuild
        [STAThread]
        static void Main(string[] args)
        {
            //1. Create a new project
            ProjectFile projectFile = new ProjectFile();

            //2. Setup database connection information
            projectFile.DatabaseServer = "localhost";

            // We haved used the dellstore2 database here.
            // You can download the data schema from: 
            // http://pgfoundry.org/projects/dbsamples/
            projectFile.DatabaseName = "dellstore2";

            projectFile.DatabaseUsername = "postgres";
            projectFile.DatabasePassword = "postgres";

            //3. Put the generated assemblies in this folder
            projectFile.CompilerOutputFolder = @"..\..\..\..\REFERENCES";

            //4. Set the root namespace for the generated assembly.
            projectFile.RootNamespace = "Dellstore2";
            
            //5. Create a Builder object and assign log event handler
            Builder builder = new Builder();
            builder.OnBuildStep += new BuilderEventHandler(builder_OnBuildStep);

            //6. Create a database schema reader
            DatabaseSchema databaseSchema = new DatabaseSchema(projectFile, builder);

            //7. Build the project using the database schema above
            builder.Build(projectFile, databaseSchema);

            //8. Done.
            Console.WriteLine("Done. Press <ENTER> to exit.");
            Console.ReadLine();
        }

        static void builder_OnBuildStep(object sender, BuilderEventArgs e)
        {
#if DEBUG
            Debug.WriteLine(e.Message);
#endif
            Console.WriteLine(e.Message);
        }
    }
}