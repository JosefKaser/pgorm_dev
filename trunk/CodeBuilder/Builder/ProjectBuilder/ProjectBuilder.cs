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
using System.Reflection;
using CodeBuilder.TemplateObjects;


namespace CodeBuilder
{
    public partial class ProjectBuilder
    {
        public Project p_Project {get;set;}
        public Schema<TemplateRelation,StoredFunction,TemplateColumn> p_Schema{ get; set; }
        public List<ConverterProxy> Converters { get; set; }

        private string p_BuildFolder;
        private string p_Output;
        private string p_DataAccessAssemblyFile;
        private string p_DataObjectAssemblyFile;

        public event ProjectBuilderEventHandler OnBuildStep;

        public ProjectBuilder(Project propject)
        {
            p_Project = propject;
            Converters = new List<ConverterProxy>();
        }

        #region PrepareBuildenvironment
        private void PrepareBuildenvironment()
        {
            p_BuildFolder = Helper.GetDirectoryNameIncremented(AppDomain.CurrentDomain.BaseDirectory, p_Project.RootNamespace);

            Directory.CreateDirectory(p_BuildFolder);

            p_Output = string.Format(@"{0}\Output", p_BuildFolder);
            Directory.CreateDirectory(p_Output);

            if (!Directory.Exists(p_Project.OutputFolder))
                Directory.CreateDirectory(p_Project.OutputFolder);
        } 
        #endregion

        #region ReadSchema
        private void ReadSchema()
        {
            SchemaReader<TemplateRelation, StoredFunction, TemplateColumn> schemaReader
                = new SchemaReader<TemplateRelation, StoredFunction, TemplateColumn>(p_Project.DatabaseConnectionInfo.GetConnectionString());
            p_Schema = schemaReader.ReadSchema();
        } 
        #endregion

        #region Build
        public void Build()
        {
            p_Project.InternalReferences.Add(p_Project.RootNamespace);
            p_Project.InternalReferences.Add(string.Format(@"{0}.Core", p_Project.RootNamespace));

            SendMessage(this, ProjectBuilderMessageType.Major, "Reading database schema.");
            ReadSchema();
            SendMessage(this, ProjectBuilderMessageType.Major, "Preparing build environment.");
            PrepareBuildenvironment();
            CreateDataAccessProject();
            PrepareTypeConverters();

            SendMessage(this, ProjectBuilderMessageType.Major, "Preparing entities.");
            PrepareAll();
            SendMessage(this, ProjectBuilderMessageType.Major, "Creating entities.");
            CreateDataObjectProject();
        } 
        #endregion

        private void PrepareTypeConverters()
        {
            LoadAssembly(p_DataAccessAssemblyFile);
        }

        private void LoadAssembly(string fname)
        {
            SendMessage(this, ProjectBuilderMessageType.Major, "Loading {0}",fname);
            Assembly assembly =  Assembly.LoadFile(fname);
            List<Type> public_types = assembly.GetExportedTypes().ToList();
            List<object> converters = new List<object>();

            //look into all public types
            foreach (Type type in public_types)
            {
                if (type.GetInterface("IPostgreSQLTypeConverter", true) != null)
                    Converters.Add(new ConverterProxy(Activator.CreateInstance(type)));
            }
        }


        private void PrepareAll()
        {
            p_Schema.Tables.ForEach(i => i.Prepare(this));
            p_Schema.Views.ForEach(i => i.Prepare(this));
            p_Schema.CompositeTypes.ForEach(i => i.Prepare(this));
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
