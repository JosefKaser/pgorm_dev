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
    public class AssemblyInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Configuration { get; set; }
        public string Company { get; set; }
        public string Product { get; set; }
        public string Copyright { get; set; }
        public string Trademark { get; set; }
        public string Culture { get; set; }
        public string Guid { get; set; }
        public string Version { get; set; }
        public string FileVersion { get; set; }

        public AssemblyInfo()
        {
            Company = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion", "RegisteredOrganization", "");
            Copyright = string.Format("Copyright � {0} {1}", Company, DateTime.Now.Year);
            Guid = System.Guid.NewGuid().ToString("N");
            Version = "1.0.0.0";
            FileVersion = "1.0.0.0";
        }
    }

    #region ProjectFile
    public class ProjectFile
    {
        protected string UUID;

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

        public string OutputFolder { get; set; }
        public string CompilerOutputFolder { get; set; }
        public string DatabaseServer { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUsername { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabaseConnectionPort { get; set; }
        public string DatabaseConnectionOptions { get; set; }
        public List<string> Tables;
        public List<string> Views;
        public List<string> Functions;
        public List<Table> CutsomQueries;
        public List<string> RemoveTablePrefix;
        public List<string> ProjectRefs;
        public List<string> UsingLibs { get; set; }
        public Version Version { get; set; }
        public AssemblyInfo AssemblyInfo;

        #region CPROJName
        public string CPROJName
        {
            get
            {
                return string.Format(@"{0}\{1}.csproj", ProjectOutputFolder, AssemblyName);
            }
        }
        #endregion

        public string ObjectClassPostfix { get; private set; }
        public string AssemblyName { get { return RootNamespace; } }

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
    } 
    #endregion
}