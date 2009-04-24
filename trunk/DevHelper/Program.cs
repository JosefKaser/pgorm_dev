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
    #region MyRegion
    class Country
    {
        public string TwoLetterCode { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", TwoLetterCode, Name, Currency);
        }
    }

    class CountryTypeConverter : PostgreSQLTypeConverter
    {
        public override Type CLR_Type()
        {
            return typeof(Country);
        }

        public override string ToString(object obj)
        {
            return (obj as Country).ToString();
        }

        public override object FromString(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                string[] parts = data.Replace("(", "").Replace(")", "").Split(new char[] { ',' });
                Country country = new Country();
                country.TwoLetterCode = parts[0];
                country.Name = parts[1];
                country.Currency = parts[2];
                return country;
            }
            else
                return null;
        }
    } 
    #endregion

    class Program
    {
        #region MyRegion
        //public static void TestAllDBs()
        //{
        //    List<string> databases = new List<string>();
        //    List<Schema> scs = new List<Schema>();
        //    string constr = "server=db1.truesoftware.net;username=postgres;password=db359!wn";
        //    DataAccess.InitializeDatabase(constr);
        //    NpgsqlCommand command = new NpgsqlCommand("select datname from pg_database where datdba <> 10", DataAccess.Connection);
        //    NpgsqlDataReader reader = command.ExecuteReader();

        //    while (reader.Read())
        //        databases.Add(reader.GetString(0));

        //    foreach (string db in databases)
        //    {
        //        constr = string.Format("server=db1.truesoftware.net;database={0};username=postgres;password=db359!wn", db);
        //        SchemaReader sr = new SchemaReader(constr);
        //        Debug.Write("Reading " + db);
        //        scs.Add(sr.ReadSchema());
        //        Debug.WriteLine(" , Done.");
        //    }
        //} 
        #endregion

        static void Main(string[] args)
        {
            //string constr = "server=localhost;database=testdb;username=postgres;password=postgres";
            //DataAccess.InitializeDatabase(constr); PostgreSQL.CodeHelper.create_helper();

            CodeBuilder.Project project = new CodeBuilder.Project();
            project.DatabaseConnectionInfo.Server = "localhost";
            project.DatabaseConnectionInfo.Database = "testdb2";
            project.DatabaseConnectionInfo.Username = "postgres";
            project.DatabaseConnectionInfo.Password = "postgres";
            project.OutputFolder = AppDomain.CurrentDomain.BaseDirectory + @"\Output";
            project.RemoveTablePrefix.Add("tbl");
            project.RemoveTablePrefix.Add("view_");
            project.BuildInDebugMode = true;
            project.SetDefaultsByDatabaseName();
            CodeBuilder.ProjectBuilder projectBuilder = new CodeBuilder.ProjectBuilder(project);
            projectBuilder.OnBuildStep += new CodeBuilder.ProjectBuilderEventHandler(projectBuilder_OnBuildStep);
            projectBuilder.Build();
            Console.WriteLine("Done.");
            Console.ReadLine();

        }

        static void projectBuilder_OnBuildStep(object sender, CodeBuilder.ProjectBuilderEventArgs e)
        {
            Console.WriteLine(string.Format("{0}\t{1}", e.MessageType, e.Message));
        }

    }
}
