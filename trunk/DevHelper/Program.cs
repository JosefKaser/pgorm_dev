using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGORM.PostgreSQL;
using PGORM.PostgreSQL.Objects;
using Npgsql;
using System.Diagnostics;

namespace DevHelper
{
    class Program
    {
        static bool has_error = false;
        static void Main(string[] args)
        {
            //string constr = "server=localhost;database=PGORM_TEST;username=postgres;password=postgres";
            //DataAccess.InitializeDatabase(constr); PGORM.PostgreSQL.CodeHelper.create_helper();

            PGORM.CodeBuilder.Project project = new PGORM.CodeBuilder.Project();
            project.DatabaseConnectionInfo.Server = "localhost";
            project.DatabaseConnectionInfo.Database = "PGORM_TEST";
            project.DatabaseConnectionInfo.Username = "postgres";
            project.DatabaseConnectionInfo.Password = "postgres";
            project.OutputFolder = AppDomain.CurrentDomain.BaseDirectory + @"\Output";
            project.BuildInDebugMode = true;

            //project.Tables.Add("public.test1");
            //project.Tables.Add("public.test2");
            //project.Tables.Add("public.test3");
            //project.Tables.Add("public.enum_table");
            //project.Tables.Add("public.building");
            //project.Tables.Add("public.building_complex");
            //project.Tables.Add("public.udt_single_test");
            //project.Tables.Add("public.udt_array_test");
            project.Tables.Add("public.udt_multi_array_test");

            project.SetDefaultsByDatabaseName();
            project.AssemblyName = @"test1.dll";
            PGORM.CodeBuilder.ProjectBuilder projectBuilder = new PGORM.CodeBuilder.ProjectBuilder(project);
            projectBuilder.OnBuildStep += new PGORM.CodeBuilder.ProjectBuilderEventHandler(projectBuilder_OnBuildStep);
            projectBuilder.Build();
            Console.WriteLine("Done.");
            //if (has_error)
                Console.ReadLine();
        }

        static void projectBuilder_OnBuildStep(object sender, PGORM.CodeBuilder.ProjectBuilderEventArgs e)
        {
            if (!e.Message.Contains("XML"))
            {
                if (e.MessageType == PGORM.CodeBuilder.ProjectBuilderMessageType.Error)
                    has_error = true;
                Console.WriteLine(string.Format("{0}\t{1}", e.MessageType, e.Message));
            }
        }
    }
}
