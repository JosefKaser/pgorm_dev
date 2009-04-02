/*-------------------------------------------------------------------------
 * Builder.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;
using System.Windows.Forms;

namespace PGORM
{
    #region BuilderEventArgs
    public class BuilderEventArgs : EventArgs
    {
        public string Message { get; set; }
        public BuilderMessageType MessageType;

        public BuilderEventArgs(string p_Message,BuilderMessageType p_MessageType)
        {
            Message = p_Message;
            MessageType = p_MessageType;
        }
    } 
    #endregion

    #region BuilderMessageType
    public enum BuilderMessageType
    {
        Major,
        Minor,
        Error
    } 
    #endregion


    public delegate void BuilderEventHandler(object sender,BuilderEventArgs e);

    public class Builder
    {
        #region Props
        protected ProjectFile objectProject;
        protected VS2008Project vsObjectProject,vsDataProject;
        protected ProjectFile dataAccessProject;
        protected AssemblyInfoBuilder assemblyInfoBuilder;
        protected HelperClassesBuilder helperClassesBuilder;
        protected ObjectBuilder objectBuilder;
        protected FunctionBuilder functionBuilder;
        protected string tempFname;
        protected DatabaseSchema dbSchema;
        public event BuilderEventHandler OnBuildStep;
        #endregion

        #region Ctor

        public Builder()
        {
        }

        #endregion

        #region Build

        private void CreateDataAccessProject(string projectRootNamespace)
        {
            dataAccessProject = new ProjectFile();
            // force this to be default 
            dataAccessProject.CompilerOutputFolder = @"bin\Release";
            dataAccessProject.RootNamespace = projectRootNamespace + ".Data";
            vsDataProject = new VS2008Project(dataAccessProject, this);
            vsDataProject.AddCompileItem(
                "DataAccess.cs", DataAccessFiles.DataAccess.Replace("MY_NAMESPACE", dataAccessProject.RootNamespace));

            vsDataProject.AddCompileItem(
                "DatabaseOperation.cs", DataAccessFiles.DatabaseOperation.Replace("MY_NAMESPACE", dataAccessProject.RootNamespace));

            string[] current_asm_info = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string current_asm_version = current_asm_info[1].Split(new char[] { '=' })[1];
            dataAccessProject.AssemblyInfo.Title = dataAccessProject.RootNamespace;
            dataAccessProject.AssemblyInfo.Product = dataAccessProject.RootNamespace;
            dataAccessProject.AssemblyInfo.Version = current_asm_version;
            dataAccessProject.AssemblyInfo.FileVersion = current_asm_version;
            dataAccessProject.AssemblyInfo.Description = objectProject.AssemblyInfo.Description;

            AssemblyInfoBuilder asmInfoBuilder = new AssemblyInfoBuilder(dataAccessProject, vsDataProject, this);
            asmInfoBuilder.Build();

            dataAccessProject.ProjectRefs.Add(AppDomain.CurrentDomain.BaseDirectory + "Npgsql.dll");

            string dataAccessAssemblyName = string.Format(@"{0}\{1}\{2}.dll",
                dataAccessProject.CPROJName.Replace(dataAccessProject.AssemblyName + ".csproj", ""),
                dataAccessProject.CompilerOutputFolder,
                dataAccessProject.AssemblyName);

            objectProject.ProjectRefs.Add(dataAccessAssemblyName);
            objectProject.UsingLibs.Add(dataAccessProject.RootNamespace);                

            vsDataProject.Build();
        }

        public void Build(ProjectFile pf, DatabaseSchema db_schmea)
        {
            objectProject = pf;

            // save for later use
            string projectRootNamespace = objectProject.RootNamespace;

            CreateDataAccessProject(projectRootNamespace);

            objectProject.RootNamespace += ".Objects";

            if (db_schmea == null)
                dbSchema = new DatabaseSchema(objectProject,this);
            else
                dbSchema = db_schmea;

            vsObjectProject = new VS2008Project(objectProject,this);

            assemblyInfoBuilder = new AssemblyInfoBuilder(objectProject, vsObjectProject,this);
            assemblyInfoBuilder.Build();

            objectBuilder = new ObjectBuilder(objectProject, vsObjectProject, dbSchema,this);
            objectBuilder.Build();

            functionBuilder = new FunctionBuilder(objectProject, vsObjectProject, dbSchema,this);
            functionBuilder.Build();

            //these two need to be the last

            helperClassesBuilder = new HelperClassesBuilder(objectProject, vsObjectProject, dbSchema,this);
            helperClassesBuilder.Build();

            BuildAssembly(dataAccessProject.CPROJName);
            vsObjectProject.Build();
            BuildAssembly(objectProject.CPROJName);

            //dbSchema.CleanUp();
        }

        #endregion

        #region ReadProject
        //TODO: remove read project file
        //void ReadProject(string p_project_file)
        //{
        //    XmlSerializer ser = new XmlSerializer(typeof(ProjectFile));
        //    FileStream fs = new FileStream(p_project_file, FileMode.Open);
        //    project = (ProjectFile)ser.Deserialize(fs);
        //    project.Prepare();
        //    fs.Close();
        //}
        #endregion

        void BuildAssembly(string projectPath)
        {
            SendMessage(this, BuilderMessageType.Major, "Building {0}", Path.GetFileName(projectPath).Replace(".csproj", ".dll"));
            Engine buildEngine = new Engine();
            BuildPropertyGroup pGroup = new BuildPropertyGroup();
            ConsoleLogger logger = new ConsoleLogger(LoggerVerbosity.Quiet);
            PGORMLogger pgormLogger = new PGORMLogger(this,projectPath);
            pGroup.SetProperty("Configuration", "Release");
            pGroup.SetProperty("DebugType", "none");
            pGroup.SetProperty("DebugSymbols", "false");
            buildEngine.RegisterLogger(logger);
            buildEngine.RegisterLogger(pgormLogger);
            buildEngine.BuildProjectFile(projectPath, new string[] { "Build" }, pGroup);
            if (pgormLogger.HasErrors)
                foreach (PGORMLoaggerException item in pgormLogger.Exceptions)
                    throw item;
        }

        #region SendMessage
        public void SendMessage(object sender,BuilderMessageType messageType, string data, params object[] args)
        {
            if (OnBuildStep != null)
            {
                OnBuildStep(sender, new BuilderEventArgs(string.Format(data, args),messageType));
            }
        }
        #endregion
    }
}