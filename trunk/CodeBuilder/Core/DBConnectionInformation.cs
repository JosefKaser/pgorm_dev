using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CodeBuilder
{
    public class DBConnectionInformation
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        [XmlIgnore]
        public string Password { get; set; }
        public string Port { get; set; }
        public string Options { get; set; }

        public DBConnectionInformation()
        {
            Database = "";
            Options = "";
            Server = "localhost";
            Password = "postgres";
            Port = "5432";
        }

        public string GetConnectionString()
        {
            string result = string.Format("server={0};username={1};password={2};port={3}", Server,Username,Password,Port);

            if (!string.IsNullOrEmpty(Database))
                result = string.Format("{0};database={1}", result, Database);

            if (!string.IsNullOrEmpty(Options))
                result = string.Format("{0};database={1}", result, Options);

            return result;
        }
    }

}
