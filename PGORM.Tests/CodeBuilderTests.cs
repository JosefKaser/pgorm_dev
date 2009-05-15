using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PGORM.PostgreSQL;
using PGORM.PostgreSQL.Objects;
using Npgsql;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PGORM.Tests
{
    [TestFixture]
    public class CodeBuilderTests
    {
        #region Props
        string server = "localhost";
        string username = "postgres";
        string password = "postgres";
        string options = "";
        string test_db_name = "PGORM_TEST";
        string test_output_folder = AppDomain.CurrentDomain.BaseDirectory + @"\Output";
        string out_file_dll;
        NpgsqlConnection Connection;
        #endregion

        #region GenerateORM
#if (!ORM_GENERATED)
        [Test]
        public void GenerateORM()
        {
            CleanupFiles();
            PGORM.CodeBuilder.Project project = new PGORM.CodeBuilder.Project();
            project.DatabaseConnectionInfo.Server = server;
            project.DatabaseConnectionInfo.Database = test_db_name;
            project.DatabaseConnectionInfo.Username = "postgres";
            project.DatabaseConnectionInfo.Password = "postgres";
            project.DatabaseConnectionInfo.Port = "5432";
            project.OutputFolder = test_output_folder;
            project.BuildInDebugMode = true;

            project.Tables.Add("public.test1");
            project.Tables.Add("public.test2");
            project.Tables.Add("public.test3");
            project.Tables.Add("public.enum_table");
            project.Tables.Add("public.building");
            project.Tables.Add("public.building_complex");

            project.SetDefaultsByDatabaseName();
            project.AssemblyName = test_db_name;
            PGORM.CodeBuilder.ProjectBuilder projectBuilder = new PGORM.CodeBuilder.ProjectBuilder(project);
            projectBuilder.OnBuildStep += new PGORM.CodeBuilder.ProjectBuilderEventHandler(projectBuilder_OnBuildStep);
            projectBuilder.Build();
            Assert.AreEqual(File.Exists(out_file_dll), true);
        }

        void projectBuilder_OnBuildStep(object sender, PGORM.CodeBuilder.ProjectBuilderEventArgs e)
        {
            if (e.MessageType == PGORM.CodeBuilder.ProjectBuilderMessageType.Error)
            {
                throw new Exception(e.Message);
            }
        } 
#endif
        #endregion

        #region Controler
        [Test]
        public void Controler()
        {
            Assert.AreEqual(1, 1);
        }
        
        #endregion

        #region SetupTest
        [TestFixtureSetUp]
        public void SetupTest()
        {
            Connection = NewConnection();
            Connection.Open();

            if (CheckDatabaseExists(test_db_name))
            {
                DropTestDatabase();
                CreateTestDatabase();
                RunCreateSchemaScript();
            }
            else
            {
                CreateTestDatabase();
                RunCreateSchemaScript();
            }

            out_file_dll = string.Format(@"{0}\{1}.dll", test_output_folder, test_db_name);
        } 
        #endregion

        #region CleanUpTest
        [TestFixtureTearDown]
        public void CleanUpTest()
        {
            Connection.Close();
        } 
        #endregion

        #region Helpers

        private void CleanupFiles()
        {
            try
            {
                Directory.Delete(test_output_folder, true);
            }
            catch
            {
            }
        }

        private void DropTestDatabase()
        {
            Connection.ChangeDatabase("postgres");
            NpgsqlCommand command = new NpgsqlCommand(string.Format("DROP DATABASE \"{0}\"", test_db_name), Connection);
            command.ExecuteNonQuery();
        }


        private void CreateTestDatabase()
        {
            Connection.ChangeDatabase("postgres");
            NpgsqlCommand command = new NpgsqlCommand(string.Format("CREATE DATABASE \"{0}\"", test_db_name), Connection);
            command.ExecuteNonQuery();
        }

        private NpgsqlConnection NewConnection()
        {
            return new NpgsqlConnection(string.Format("server={0};username={1};password={2};{3}", server, username, password,options));
        }

        private void RunCreateSchemaScript()
        {
            Connection.ChangeDatabase(test_db_name);
            NpgsqlCommand command = new NpgsqlCommand(SQLStatements.TestSchema, Connection);
            command.ExecuteNonQuery();
        }

        private bool CheckDatabaseExists(string name)
        {
            string sql = "select count(*) from pg_database where datname=@name";
            Connection.ChangeDatabase("postgres");
            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.Parameters.Add(new NpgsqlParameter("@name", test_db_name));
            long result = (long)command.ExecuteScalar();
            return result == 1 ? true : false;
        } 
        #endregion
    }
}
