/*-------------------------------------------------------------------------
 * ProjectFile.cs
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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace PGORM
{
    #region ProjectFile
    public class ProjectFile
    {
        #region RootNamespace
        private string p_RootNamespace;
        public string RootNamespace
        {
            get
            {
                return p_RootNamespace;
            }
            set
            {
                p_RootNamespace = value;
            }
        }
        #endregion

        #region Props
        protected string UUID;
        [XmlIgnore]
        public string OutputFolder { get; set; }
        public string CompilerOutputFolder { get; set; }
        public string DatabaseServer { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUsername { get; set; }
        [XmlIgnore]
        public string DatabasePassword { get; set; }
        public string DatabaseConnectionPort { get; set; }
        public string DatabaseConnectionOptions { get; set; }
        public List<string> Tables;
        public List<string> Views;
        public List<string> Functions;
        [XmlIgnore]
        public List<Table> CutsomQueries;
        public List<string> RemoveTablePrefix;
        [XmlIgnore]
        public List<string> ProjectRefs { get; set; }
        [XmlIgnore]
        public List<string> UsingLibs { get; set; }
        [XmlIgnore]
        public Version Version { get; set; }
        public AssemblyInfo AssemblyInfo;
        [XmlIgnore]
        public string ObjectClassPostfix { get; private set; }
        public string AssemblyName { get { return RootNamespace; } } 
        #endregion

        #region CPROJName
        public string CPROJName
        {
            get
            {
                return string.Format(@"{0}\{1}.csproj", ProjectOutputFolder, AssemblyName);
            }
        }
        #endregion

        #region ProjectOutputFolder
#if DEBUG
        public string ProjectOutputFolder { get { return string.Format("{0}\\{1}", OutputFolder, RootNamespace); } }
#else
        public string ProjectOutputFolder { get { return string.Format("{0}\\{1}_{2}", OutputFolder, RootNamespace, UUID); } }
#endif
        #endregion

        #region DatabaseConnectionString
        public string DatabaseConnectionString
        {
            get
            {
                return string.Format("host={0};database={1};username={2};password={3};port={4};{5}",
                    DatabaseServer,
                    DatabaseName,
                    DatabaseUsername,
                    DatabasePassword,
                    DatabaseConnectionPort,
                    DatabaseConnectionOptions
                    );
            }
        }
        #endregion

        #region ProjectFile
        public ProjectFile()
        {
            UsingLibs = new List<string>();
            UUID = Guid.NewGuid().ToString();
            CompilerOutputFolder = @"bin\Release";
            DatabaseServer = "localhost";
            DatabaseName = "testdb";
            DatabaseUsername = "postgres";
            DatabasePassword = "postgres";
            DatabaseConnectionPort = "5432";
            DatabaseConnectionOptions = "";

            RootNamespace = "MyProject";
            OutputFolder = AppDomain.CurrentDomain.BaseDirectory;
            Version = Assembly.GetExecutingAssembly().GetName().Version;
            CompilerOutputFolder = @"C:\" + RootNamespace;

            Tables = new List<string>();
            Views = new List<string>();
            Functions = new List<string>();

            CutsomQueries = new List<Table>();
            RemoveTablePrefix = new List<string>();
            ProjectRefs = new List<string>();
            ObjectClassPostfix = "Object";
            AssemblyInfo = new AssemblyInfo();
            AssemblyInfo.Product = RootNamespace;
            AssemblyInfo.Title = RootNamespace;
            AssemblyInfo.Guid = UUID;
#if DEBUG
            AssemblyInfo.Configuration = "Debug";
#elif RELEASE
            AssemblyInfo.Configuration = "Release";
#endif

        }
        #endregion

        public static void SaveProject(ProjectFile p_ProjectFile, string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ProjectFile));
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
            ser.Serialize(fs, p_ProjectFile);
            fs.Close();
        }
    } 
    #endregion
}