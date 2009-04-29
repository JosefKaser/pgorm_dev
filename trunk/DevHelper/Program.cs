using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostgreSQL;
using PostgreSQL.Objects;
using Npgsql;
using System.Diagnostics;

namespace DevHelper
{
    class Program
    {
        static bool has_error = false;
        static void Main(string[] args)
        {
            //string constr = "server=localhost;database=testdb;username=postgres;password=postgres";
            //DataAccess.InitializeDatabase(constr); PostgreSQL.CodeHelper.create_helper();

            CodeBuilder.Project project = new CodeBuilder.Project();
            project.DatabaseConnectionInfo.Server = "localhost";
            project.DatabaseConnectionInfo.Database = "testdb";
            project.DatabaseConnectionInfo.Username = "postgres";
            project.DatabaseConnectionInfo.Password = "postgres";
            project.OutputFolder = AppDomain.CurrentDomain.BaseDirectory + @"\Output";
            project.RemoveTablePrefix.Add("tbl");
            project.RemoveTablePrefix.Add("view_");
            project.BuildInDebugMode = true;
            project.SetDefaultsByDatabaseName();
            project.AssemblyName = @"test1.dll";
            CodeBuilder.ProjectBuilder projectBuilder = new CodeBuilder.ProjectBuilder(project);
            projectBuilder.OnBuildStep += new CodeBuilder.ProjectBuilderEventHandler(projectBuilder_OnBuildStep);
            projectBuilder.Build();
            Console.WriteLine("Done.");
            //if (has_error)
                Console.ReadLine();
        }

        static void projectBuilder_OnBuildStep(object sender, CodeBuilder.ProjectBuilderEventArgs e)
        {
            if (!e.Message.Contains("zXML"))
            {
                if (e.MessageType == CodeBuilder.ProjectBuilderMessageType.Error)
                    has_error = true;
                Console.WriteLine(string.Format("{0}\t{1}", e.MessageType, e.Message));
            }
        }
    }
}
