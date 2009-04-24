using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CodeBuilder
{
    public class Project
    {
        public AssemblyInfoData AssemblyInfo { get; set; }
        public DBConnectionInformation DatabaseConnectionInfo { get; set; }

        public string RootNamespace { get; set; }
        public string AssemblyName { get; set; }
        public string OutputFolder { get; set; }
        public List<string> RemoveTablePrefix;

        public List<string> Tables;
        public List<string> Views;
        [XmlIgnore]
        public List<string> InternalReferences;

        public bool BuildInDebugMode { get; set; }

        public Project()
        {
            AssemblyInfo = new AssemblyInfoData();
            DatabaseConnectionInfo = new DBConnectionInformation();
            InternalReferences = new List<string>();
            SetDefaultsByDatabaseName();
            Tables = new List<string>();
            Views = new List<string>();
            BuildInDebugMode = false;
            RemoveTablePrefix = new List<string>();
        }

        public void SetDefaultsByDatabaseName()
        {
            RootNamespace = DatabaseConnectionInfo.Database;
            AssemblyName = string.Format("{0}.dll", DatabaseConnectionInfo.Database); 
        }

        public static void SaveProject(Project p_Project, string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Project));
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
            ser.Serialize(fs, p_Project);
            fs.Close();
        }
    }
}
