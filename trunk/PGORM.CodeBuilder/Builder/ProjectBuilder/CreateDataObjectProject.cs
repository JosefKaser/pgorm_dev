using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.CSharp;
using PGORM.PostgreSQL.Objects;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.CodeDom;
using PGORM.CodeBuilder.TemplateObjects;


namespace PGORM.CodeBuilder
{
    public partial class ProjectBuilder
    {
        //needs to be visible from factory builder
        public static string p_ObjectNamespace = "Entities";
        public static string p_TypesNamespace = "Types";

        #region CreateCompositeTypes
        private void CreateCompositeTypes(DataObjectBuilder objectBuilder, FactoryBuilder factoryBuilder, RecordSetBuilder recordsetBuilder, string doBuildFolder,bool create_depend_converters,bool is_udt)
        {
            foreach (TemplateRelation rel in SchemaUsedCompositeTypes)
            {
                SendMessage(this, ProjectBuilderMessageType.Major, "Generating code for {0}", rel.RelationName);

                objectBuilder.Create(rel, p_TypesNamespace, doBuildFolder,create_depend_converters,is_udt);
                recordsetBuilder.Create(rel, doBuildFolder);

                factoryBuilder.Reset();
                factoryBuilder.Create(rel, doBuildFolder);
            }
        }
        #endregion

        #region CreateViews
        private void CreateViews(DataObjectBuilder objectBuilder, FactoryBuilder factoryBuilder, RecordSetBuilder recordsetBuilder, string doBuildFolder)
        {
            foreach (TemplateRelation rel in GetRequestedViews())
            {
                SendMessage(this, ProjectBuilderMessageType.Major, "Generating code for {0}", rel.RelationName);

                objectBuilder.Create(rel, p_ObjectNamespace, doBuildFolder,false,false);
                recordsetBuilder.Create(rel, doBuildFolder);

                factoryBuilder.Reset();

                foreach (Index<TemplateColumn> index in rel.Indexes)
                {
                    string method_sub_name = factoryBuilder.CreateMethodSubName(index.Columns);
                    string getby_summary = factoryBuilder.CodeSummary("Retrives a generic List&lt;{0}&gt; based on {1}", rel.TemplateRelationName, index.IndexType, method_sub_name);
                    factoryBuilder.AddMethod(factoryBuilder.CreateGetMultiReturnMethod(rel, string.Format("GetManyBy_{0}", method_sub_name), index, getby_summary));
                }

                factoryBuilder.Create(rel, doBuildFolder);
            }
        } 
        #endregion

        #region GetRequestedTables
        private List<TemplateRelation> GetRequestedTables()
        {
            var tables = from i in p_Schema.Tables
                         join j in p_Project.Tables on i.FullNameInvariant equals j
                         select i;
            return tables.ToList();
        } 
        #endregion

        #region GetRequestedViews
        private List<TemplateRelation> GetRequestedViews()
        {
            var views = from i in p_Schema.Views
                        join j in p_Project.Views on i.FullNameInvariant equals j
                        select i;
            return views.ToList();
        }
        #endregion

        #region GetRequestedFunctions
        private List<TemplateFunction> GetRequestedFunctions()
        {
            var funcions = from i in p_Schema.Functions
                        join j in p_Project.Functions on i.FullNameInvariant equals j
                        select i;
            return funcions.ToList();
        }
        #endregion

        #region CreateTables
        private void CreateTables(DataObjectBuilder objectBuilder, FactoryBuilder factoryBuilder, RecordSetBuilder recordsetBuilder, string doBuildFolder)
        {
            foreach (TemplateRelation rel in GetRequestedTables())
            {
                SendMessage(this, ProjectBuilderMessageType.Major, "Generating code for {0}", rel.RelationName);

                objectBuilder.Create(rel, "Entities", doBuildFolder,false,false);
                recordsetBuilder.Create(rel, doBuildFolder);

                factoryBuilder.Reset();

                // create insert method
                factoryBuilder.AddMethod("insert", factoryBuilder.CreateInsertMethod(rel));

                // create import method
                factoryBuilder.AddMethod(factoryBuilder.CreateCopyInMethod(rel));

                // create delete all method
                factoryBuilder.AddMethod(factoryBuilder.CreateDeleteAllMethod(rel));

                // create get all method
                factoryBuilder.AddMethod("getall", factoryBuilder.CreateGetAllMethod(rel));

                // create count records
                factoryBuilder.AddMethod(factoryBuilder.CreateCountRecords(rel));

                // loop every unique and primary key index on this relation.
                foreach (Index<TemplateColumn> index in rel.Indexes)
                {
                    if (index.IndexType == IndexType.PrimaryKey || index.IndexType == IndexType.UniqueIndex)
                    {
                        factoryBuilder.AddMethod(factoryBuilder.CreateGetSingleReturnMethod(rel, index));
                        factoryBuilder.AddMethod(factoryBuilder.CreateDeleteMethod(rel, index));
                        factoryBuilder.AddMethod(factoryBuilder.CreateUpdateSingleMethod(rel, index));
                    }
                }

                foreach (Index<TemplateColumn> index in rel.Indexes)
                {
                    string method_sub_name = factoryBuilder.CreateMethodSubName(index.Columns);
                    string getby_summary = factoryBuilder.CodeSummary("Retrives a generic List&lt;{0}&gt; based on {1}", rel.TemplateRelationName, index.IndexType, method_sub_name);
                    string update_many_summary = factoryBuilder.CodeSummary("Updates table [{0}] using p_{0} as UPDATE SET parameters and retrives a generic List&lt;{0}&gt; of all updated/affected records. Use this method when updating foreign records.", rel.TemplateRelationName, index.IndexType, method_sub_name);


                    if (index.IndexType == IndexType.ForeignKey)
                    {
                        factoryBuilder.AddMethod(factoryBuilder.CreateGetMultiReturnMethod(rel, string.Format("GetManyBy_{0}", method_sub_name), index, getby_summary));
                        factoryBuilder.AddMethod(factoryBuilder.CreateUpdateManyMethod(rel, string.Format("UpdateManyBy_{0}", method_sub_name), index, update_many_summary));
                    }

                    if (index.IndexType == IndexType.CustomIndex)
                        factoryBuilder.AddMethod(factoryBuilder.CreateGetMultiReturnMethod(rel, string.Format("GetManyBy_{0}", method_sub_name), index, getby_summary));
                }

                factoryBuilder.Create(rel, doBuildFolder);
            }
        }
        #endregion

        #region AttachFunctionReturnTypes
        private void AttachFunctionReturnTypes()
        {
            //attach the correct return types
            foreach (TemplateFunction function in GetRequestedFunctions())
            {
                // void type
                if (function.ReturnTypeType == FunctionReturnTypeType.Void)
                {
                    function.TemplateReturnType = new TemplateVoidReturnType();
                }

                // table or view
                else if (function.ReturnTypeType == FunctionReturnTypeType.View || function.ReturnTypeType == FunctionReturnTypeType.Table)
                {
                    TemplateRelation rt = GetRequestedViews().Find(i => i.FullNameInvariant == function.FullReturnTypeInvariant);
                    if (rt == null)
                        rt = GetRequestedTables().Find(i => i.FullNameInvariant == function.FullReturnTypeInvariant);
                    function.TemplateReturnType = new TemplateReturnType(
                        string.Format("{0}.{1}.{2}.{3}", p_Project.RootNamespace, rt.TemplateNamespace, ProjectBuilder.p_ObjectNamespace, rt.TemplateRelationName),
                        function.IsSetReturning);
                }
                // composite type
                else if (function.ReturnTypeType == FunctionReturnTypeType.Enum)
                {
                    function.TemplateReturnType = new TemplateReturnType(
                        string.Format("{0}.{1}.Enums.{2}", p_Project.RootNamespace,
                        function.ReturnTypeSchemaName.ToUpper(), function.ReturnTypeName),
                        function.IsSetReturning);
                }
                else if (function.ReturnTypeType == FunctionReturnTypeType.BaseType)
                {
                    TemplateRelation rt = p_Schema.CompositeTypes.Find(i => i.FullNameInvariant == function.FullNameInvariant);
                    function.TemplateReturnType = new TemplateReturnType(
                        rt.Columns[0].TemplateCLR_Type, function.IsSetReturning);
                }
                else if (function.ReturnTypeType == FunctionReturnTypeType.CompositeType)
                {
                    TemplateRelation udt = p_Schema.CompositeTypes.Find(i => i.FullNameInvariant == function.FullNameInvariant);
                    if (udt == null)
                        udt = p_Schema.CompositeTypes.Find(i => i.FullNameInvariant == function.FullReturnTypeInvariant);

                    function.TemplateReturnType = new TemplateReturnType(
                        string.Format("{0}.{1}.Types.{2}", p_Project.RootNamespace, udt.TemplateNamespace, udt.TemplateRelationName),
                        function.IsSetReturning);
                    /*
                    function.Converter = Converters.Find(c => c.PgType == function.ReturnTypeName && c.PgTypeSchema == function.ReturnTypeSchemaName);
                    if(function.Converter == null)
                        function.Converter = Converters.Find(c => c.PgType == function.RelationName && c.PgTypeSchema == function.SchemaName);
                    */
                }
            }
        } 
        #endregion

        #region CreateFunctions
        private void CreateFunctions(string doBuildFolder)
        {
            AttachFunctionReturnTypes();
            foreach(TemplateFunction function in GetRequestedFunctions())
            {
                List<string> libs = new List<string>();
                /*
                if(function.ReturnTypeType == FunctionReturnTypeType.Table ||
                    function.ReturnTypeType == FunctionReturnTypeType.View)
                {
                    libs.Add(string.Format("{0}.{1}.{2}",p_Project.RootNamespace,function.ReturnTypeSchemaName.ToUpper(),p_ObjectNamespace));
                }

                if (function.ReturnTypeType == FunctionReturnTypeType.Enum)
                {
                    libs.Add(string.Format("{0}.{1}.Enums", p_Project.RootNamespace, function.ReturnTypeSchemaName.ToUpper(), p_ObjectNamespace));
                }
                */

                // remove the parameters access type and the prepare the columns
                if (function.Arguments != null)
                {
                    function.TemplateArguments = new List<TemplateColumn>();
                    foreach (TemplateColumn col in function.Arguments.Columns)
                    {
                        string[] colname = col.ColumnName.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (colname[0] == "IN" || colname[0] == "INOUT")
                        {
                            col.ColumnName = colname[1];
                            function.TemplateArguments.Add(col);
                        }
                    }
                    function.Arguments.Prepare(this);
                }

                FunctionBuilder functionBuilder = new FunctionBuilder(this, libs.ToArray());
                functionBuilder.Create(function, doBuildFolder);
            }
            

        } 
        #endregion

        #region CreateDataObjectProject
        private void CreateDataObjectProject()
        {
            string doBuildFolder = string.Format(@"{0}\Objects", p_BuildFolder);
            Directory.CreateDirectory(doBuildFolder);

            if (SchemaUsedCompositeTypes.Count() == 0)
                p_TypesNamespace = null;

            DataObjectBuilder objectBuilder = new DataObjectBuilder(this, new string[] {  p_TypesNamespace });
            RecordSetBuilder recordsetBuilder = new RecordSetBuilder(this, new string[] { p_ObjectNamespace, p_TypesNamespace });
            FactoryBuilder factoryBuilder = new FactoryBuilder(this, new string[] { p_ObjectNamespace, p_TypesNamespace, "RecordSet" });

            CreateCompositeTypes(objectBuilder, factoryBuilder, recordsetBuilder, doBuildFolder,true,true);
            // after creating the composite types we have to re-prepare everything in order for
            // TemplateColumn to resolve to correct types
            PrepareAll();

            //We have to complete then functions return types here instead of in the schema
            //reader because functions could return all kinds of composite types which are
            //not present in schema reader fase. 

            CreateCompositeTypes(objectBuilder, factoryBuilder, recordsetBuilder, doBuildFolder,false,true);
            CreateTables(objectBuilder, factoryBuilder, recordsetBuilder, doBuildFolder);
            CreateViews(objectBuilder, factoryBuilder, recordsetBuilder, doBuildFolder);
            CreateFunctions(doBuildFolder);

            AssemblyInfoData asmInfo = new AssemblyInfoData();
            AssemblyInfoBuilder asmInfoBuilder = new AssemblyInfoBuilder(p_Project.AssemblyInfo, this);
            File.WriteAllText(string.Format(@"{0}\AssemblyInfo.cs", doBuildFolder), asmInfoBuilder.BuildToString());

            string p_ProjAsmName = Path.GetFileNameWithoutExtension(p_Project.AssemblyName);
            p_DataObjectAssemblyFile = string.Format(@"{0}\{1}.dll", p_Project.OutputFolder, p_ProjAsmName);

            SendMessage(this, ProjectBuilderMessageType.Major, "Building {0} assembly.", p_DataObjectAssemblyFile);

            CSharpCodeProvider cscProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            CompilerParameters compParams = new CompilerParameters();

            compParams.GenerateExecutable = false;
            compParams.CompilerOptions = string.Format("/optimize /doc:{0}", p_DataObjectAssemblyFile.Replace(".dll", ".xml"));
            compParams.IncludeDebugInformation = p_Project.BuildInDebugMode;
            compParams.OutputAssembly = p_DataObjectAssemblyFile;
            compParams.ReferencedAssemblies.Add("System.dll");
            compParams.ReferencedAssemblies.Add("System.Xml.dll");
            compParams.ReferencedAssemblies.Add("System.Data.dll");

            compParams.ReferencedAssemblies.Add(Helper.Asm35("System.Core"));
            compParams.ReferencedAssemblies.Add(Helper.Asm35("System.Data.DataSetExtensions"));
            compParams.ReferencedAssemblies.Add(Helper.Asm35("System.Xml.Linq"));

            Helper.AddNpgsqlReferences(compParams);
            compParams.ReferencedAssemblies.Add(p_DataAccessAssemblyFile);

            string[] files = Directory.GetFiles(doBuildFolder, "*.cs", SearchOption.AllDirectories);

            CompilerResults results = cscProvider.CompileAssemblyFromFile(compParams, files);
            Helper.CopyNpgsqlAssemblies(p_Project.OutputFolder);
            
            if (results.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results.Errors)
                {
                    SendMessage(this, ProjectBuilderMessageType.Error, "{0}",
                        "\r\nFile: " + CompErr.FileName +
                        "\n\rLine number: " + CompErr.Line +
                        "\n\rError Number: " + CompErr.ErrorNumber +
                        "\n\r" + CompErr.ErrorText + ";\n\r");
                }
            }
        } 
        #endregion
    }
}
