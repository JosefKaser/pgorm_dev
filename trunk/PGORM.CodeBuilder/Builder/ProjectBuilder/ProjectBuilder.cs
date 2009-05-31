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
        public Schema<TemplateRelation, TemplateFunction, TemplateColumn> p_Schema { get; set; }
        public List<ConverterProxy> Converters { get; set; }

        private string p_BuildFolder;
        private string p_Output;
        public string p_DataAccessAssemblyFile;
        private string p_DataObjectAssemblyFile;
        public event ProjectBuilderEventHandler OnBuildStep;

        private List<string> UsedEnums = new List<string>();
        private List<TemplateRelation> SchemaUsedCompositeTypes = new List<TemplateRelation>();

        #endregion

        #region ProjectBuilder
        public ProjectBuilder(Project propject)
        {
            p_Project = propject;
            Converters = new List<ConverterProxy>();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        #region CurrentDomain_AssemblyResolve
        /*
         * Try to resolve the core assmbly when loading the temporary composite types
         */
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string core_asm = Path.GetFileNameWithoutExtension(p_DataAccessAssemblyFile);
            if (args.Name.Contains(core_asm))
                return Assembly.LoadFile(p_DataAccessAssemblyFile);
            else
                throw new NotImplementedException(args.Name);
        } 
        #endregion
        
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
            SchemaReader<TemplateRelation, TemplateFunction, TemplateColumn> schemaReader
                = new SchemaReader<TemplateRelation, TemplateFunction, TemplateColumn>(p_Project.DatabaseConnectionInfo.GetConnectionString());
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
            LoadAssembly(p_DataAccessAssemblyFile,null);
        } 
        #endregion

        #region LoadAssembly
        public void LoadAssembly(string fname,Assembly preloaded_asm)
        {
            Assembly assembly;
            if (preloaded_asm == null)
                assembly = Assembly.LoadFile(fname);
            else
                assembly = preloaded_asm;

            SendMessage(this, ProjectBuilderMessageType.Major, "Loading {0}", assembly.FullName);
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
            p_Schema.CompositeTypes.ForEach(i => i.Prepare(this));
            p_Schema.Tables.ForEach(i => i.Prepare(this));
            p_Schema.Views.ForEach(i => i.Prepare(this));
        } 
        #endregion

        #region ResolveFunctionDepencencies
        private void ResolveFunctionDepencencies()
        {
            //loop each function and resolve the return types;
            foreach (TemplateFunction function in GetRequestedFunctions())
            {
                switch (function.ReturnTypeType)
                {
                    case FunctionReturnTypeType.Table:
                        if (!p_Project.Tables.Exists(i => i == function.FullReturnTypeInvariant))
                            p_Project.Tables.Add(function.FullReturnTypeInvariant); ;
                        break;

                    case FunctionReturnTypeType.View:
                        if (!p_Project.Views.Exists(i => i == function.FullReturnTypeInvariant))
                            p_Project.Views.Add(function.FullReturnTypeInvariant); ;
                        break;

                    case FunctionReturnTypeType.Enum:
                        if (!UsedEnums.Exists(i => i == function.FullReturnTypeInvariant))
                            UsedEnums.Add(function.FullReturnTypeInvariant); ;
                        break;

                    case FunctionReturnTypeType.CompositeType:
                        //seach composite types that have the same name as the function name
                        if (!SchemaUsedCompositeTypes.Exists(i => i.FullNameInvariant == function.FullNameInvariant) ||
                            !SchemaUsedCompositeTypes.Exists(i => i.FullNameInvariant == function.FullReturnTypeInvariant))
                        {
                            TemplateRelation udt = p_Schema.CompositeTypes.Find(i => i.FullNameInvariant == function.FullNameInvariant);
                            // this must be an actuall composite type (not a return type)
                            if (udt == null)
                            {
                                udt = p_Schema.CompositeTypes.Find(i => i.FullNameInvariant == function.FullReturnTypeInvariant);
                            }
                            else
                            {
                                udt.IsFunctionReturnType = true;
                            }
                            SchemaUsedCompositeTypes.Add(udt);
                        }
                        break;
                }
            }
        } 
        #endregion

        #region ResolveDepencencies
        private void ResolveDepencencies()
        {
            //tables types
            List<TemplateRelation> rels = new List<TemplateRelation>();

            ResolveFunctionDepencencies();

            rels.AddRange((from t in p_Schema.Tables
                           join i in p_Project.Tables on t.FullNameInvariant equals i
                           select t).ToList());

            rels.AddRange((from t in p_Schema.Views
                           join i in p_Project.Views on t.FullNameInvariant equals i
                           select t).ToList());

            /*
            // check to see which function is using witch return type (composite type)
            rels.AddRange((from t in p_Schema.Functions
                           join i in p_Project.Functions on t.FullNameInvariant equals i
                           join c in p_Schema.CompositeTypes on i equals c.FullNameInvariant
                           select c).ToList());
             */

            //resolve enums
            foreach (TemplateRelation rel in rels)
                foreach (Column col in rel.Columns)
                    if (col.PGTypeType == PgTypeType.EnumType && !UsedEnums.Contains(col.PG_Type))
                        UsedEnums.Add(col.PG_Type);

            //resolve used udts in entities
            ResolveTypes(rels);

            //clear the list and parse for composite type using composite type
            rels.Clear();
            rels.AddRange((from t in p_Schema.CompositeTypes
                           join i in SchemaUsedCompositeTypes on t.FullNameInvariant equals i.FullNameInvariant
                           select t).ToList());
            ResolveTypes(rels);

            /*
            //add each used return type to SchemaUsedCompositeTypes
            List<TemplateRelation> function_return_Types = new List<TemplateRelation>();
            function_return_Types.AddRange((from t in p_Schema.Functions
                           join i in p_Project.Functions on t.FullNameInvariant equals i
                           join c in p_Schema.CompositeTypes on i equals c.FullNameInvariant
                           select c).ToList());
            function_return_Types.ForEach(i => i.IsFunctionReturnType = true);
            SchemaUsedCompositeTypes.AddRange(function_return_Types);
            */
        } 
        #endregion

        #region ResolveTypes
        private void ResolveTypes(List<TemplateRelation> rels)
        {
            List<TemplateRelation> all_types = new List<TemplateRelation>();
            all_types.AddRange(p_Schema.CompositeTypes);

            foreach (TemplateRelation rel in rels)
                foreach (Column col in rel.Columns)
                {
                    if (col.PGTypeType == PgTypeType.CompositeType && !SchemaUsedCompositeTypes.Exists(i => i.FullName == col.PG_Type))
                    {
                        SchemaUsedCompositeTypes.Add(all_types.Find(i => i.FullName == col.PG_Type));
                    }
                    else if ((col.PGTypeType == PgTypeType.Relation || col.PGTypeType == PgTypeType.View) && !SchemaUsedCompositeTypes.Exists(i => i.FullName == col.PG_Type))
                    {
                        SchemaUsedCompositeTypes.Add(all_types.Find(i => i.FullName == col.PG_Type));
                    }
                    else if (col.PGTypeType != PgTypeType.BaseType && col.PGTypeType != PgTypeType.EnumType && col.PGTypeType != PgTypeType.CompositeType && col.PGTypeType != PgTypeType.View && !SchemaUsedCompositeTypes.Exists(i => i.FullName == col.PG_Type))
                    {
                        throw new NotImplementedException(col.PGTypeType.ToString());
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
