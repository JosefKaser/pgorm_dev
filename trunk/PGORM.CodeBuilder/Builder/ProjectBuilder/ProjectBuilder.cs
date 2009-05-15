using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.CSharp;
using PGORM.PostgreSQL;
using PGORM.PostgreSQL.Objects;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Reflection;
using PGORM.CodeBuilder.TemplateObjects;


namespace PGORM.CodeBuilder
{
    public partial class ProjectBuilder
    {
        #region Props
        public Project p_Project { get; set; }
        public Schema<TemplateRelation, StoredFunction, TemplateColumn> p_Schema { get; set; }
        public List<ConverterProxy> Converters { get; set; }

        private string p_BuildFolder;
        private string p_Output;
        private string p_DataAccessAssemblyFile;
        private string p_DataObjectAssemblyFile;
        public event ProjectBuilderEventHandler OnBuildStep;

        private List<string> UsedEnums = new List<string>();
        private List<string> UsedCompositeTypes = new List<string>();
        #endregion

        #region ProjectBuilder
        public ProjectBuilder(Project propject)
        {
            p_Project = propject;
            Converters = new List<ConverterProxy>();
        }
        
        #endregion

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

            SendMessage(this, ProjectBuilderMessageType.Major, "Resolving Dependencies.");
            ResolveDepencencies();

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

        #region PrepareTypeConverters
        private void PrepareTypeConverters()
        {
            LoadAssembly(p_DataAccessAssemblyFile);
        } 
        #endregion

        #region LoadAssembly
        private void LoadAssembly(string fname)
        {
            SendMessage(this, ProjectBuilderMessageType.Major, "Loading {0}", fname);
            Assembly assembly = Assembly.LoadFile(fname);
            List<Type> public_types = assembly.GetExportedTypes().ToList();
            List<object> converters = new List<object>();

            //look into all public types
            foreach (Type type in public_types)
            {
                if (type.GetInterface("IPostgreSQLTypeConverter", true) != null)
                    Converters.Add(new ConverterProxy(Activator.CreateInstance(type)));
            }
        } 
        #endregion

        #region PrepareAll
        private void PrepareAll()
        {
            p_Schema.Tables.ForEach(i => i.Prepare(this));
            p_Schema.Views.ForEach(i => i.Prepare(this));
            p_Schema.CompositeTypes.ForEach(i => i.Prepare(this));
        } 
        #endregion

        #region ResolveDepencencies
        private void ResolveDepencencies()
        {
            //tables types
            List<TemplateRelation> rels = new List<TemplateRelation>();

            rels.AddRange((from t in p_Schema.Tables
                           join i in p_Project.Tables on t.FullNameInvariant equals i
                           select t).ToList());

            rels.AddRange((from t in p_Schema.Views
                           join i in p_Project.Views on t.FullNameInvariant equals i
                           select t).ToList());

            //resolve enums
            foreach (TemplateRelation rel in rels)
                foreach (Column col in rel.Columns)
                    if (col.PGTypeType == PgTypeType.EnumType && !UsedEnums.Contains(col.PG_Type))
                        UsedEnums.Add(col.PG_Type);
            ResolveTypes(rels);

            //clear the list and parse for composite type using composite type
            rels.Clear();
            rels.AddRange((from t in p_Schema.CompositeTypes
                           join i in UsedCompositeTypes on t.FullNameInvariant equals i.Replace("\"","")
                           select t).ToList());
            ResolveTypes(rels);

        } 
        #endregion

        #region ResolveTypes
        private void ResolveTypes(List<TemplateRelation> rels)
        {
            foreach (TemplateRelation rel in rels)
                foreach (Column col in rel.Columns)
                {
                    if (col.PGTypeType == PgTypeType.CompositeType && !UsedCompositeTypes.Contains(col.PG_Type))
                    {
                        Console.WriteLine(col.PG_Type);
                        UsedCompositeTypes.Add(col.PG_Type);
                    }
                }
        } 
        #endregion

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
