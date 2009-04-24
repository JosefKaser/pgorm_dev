using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.CSharp;
using PostgreSQL;
using PostgreSQL.Objects;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.CodeDom;
using CodeBuilder.TemplateObjects;


namespace CodeBuilder
{
    public partial class ProjectBuilder
    {
        public Project p_Project {get;set;}
        private Schema<TemplateRelation,StoredFunction,TemplateColumn> p_Schema{ get; set; }

        private string p_BuildFolder;
        private string p_Output;
        private string p_DataAccessAssemblyFile;
        private string p_DataObjectAssemblyFile;

        public event ProjectBuilderEventHandler OnBuildStep;

        public ProjectBuilder(Project propject)
        {
            p_Project = propject;
        }

        private void PrepareBuildenvironment()
        {
            p_BuildFolder = Helper.GetDirectoryNameIncremented(AppDomain.CurrentDomain.BaseDirectory, p_Project.RootNamespace);

            Directory.CreateDirectory(p_BuildFolder);

            p_Output = string.Format(@"{0}\Output",p_BuildFolder);
            Directory.CreateDirectory(p_Output);

            if (!Directory.Exists(p_Project.OutputFolder))
                Directory.CreateDirectory(p_Project.OutputFolder);
        }

        private void ReadSchema()
        {
            SchemaReader<TemplateRelation,StoredFunction,TemplateColumn> schemaReader
                = new SchemaReader<TemplateRelation, StoredFunction, TemplateColumn>(p_Project.DatabaseConnectionInfo.GetConnectionString());
            p_Schema = schemaReader.ReadSchema();
        }

        public void Build()
        {
            p_Project.InternalReferences.Add(p_Project.RootNamespace);
            p_Project.InternalReferences.Add(string.Format(@"{0}.Core", p_Project.RootNamespace));

            PrepareBuildenvironment();
            CreateDataAccessProject();
            ReadSchema();
            CreateDataObjectProject();
        }

        #region SendMessage
        public void SendMessage(object sender, ProjectBuilderMessageType messageType, string data, params object[] args)
        {
#if DEBUG
            Debug.WriteLine(string.Format(data, args));
#endif
            if (OnBuildStep != null)
            {
                OnBuildStep(sender, new ProjectBuilderEventArgs(string.Format(data, args), messageType));
            }
        }
        #endregion
    }
}
