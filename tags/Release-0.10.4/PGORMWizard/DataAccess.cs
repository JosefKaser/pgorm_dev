using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace PGORMWizard
{
    public class PGDatabase
    {
        public string Name { get; set; }
    }

    class DataAccess
    {
        public static List<PGDatabase> GetDbListing(string server, string username, string password, string options)
        {
            List<PGDatabase> result = new List<PGDatabase>();
            try
            {
                string connstr = string.Format("server={0};username={1};password={2};{3}", server, username, password, options);
                NpgsqlConnection conn = new NpgsqlConnection(connstr);
                NpgsqlCommand cmd = new NpgsqlCommand("select * from pg_database where datallowconn=true order by datname asc", conn);
                conn.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(
                        new PGDatabase() { Name = (string)reader["datname"] }
                        );
                }
                conn.Clone();
            }
            catch(Exception ex)
            {
               object o = ex;
            }
            return result;
        }
    }
}
