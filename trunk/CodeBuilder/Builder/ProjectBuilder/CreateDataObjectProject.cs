using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.CSharp;
using PostgreSQL.Objects;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.CodeDom;
using CodeBuilder.TemplateObjects;


namespace CodeBuilder
{
    public partial class ProjectBuilder
    {
        private void CreateDataObjectProject()
        {
            string doBuildFolder = string.Format(@"{0}\Objects", p_BuildFolder);
            Directory.CreateDirectory(doBuildFolder);

            string p_ObjectNamespace = "Entities";
            DataObjectBuilder objectBuilder = new DataObjectBuilder(this,p_ObjectNamespace);
            RecordSetBuilder recordsetBuilder = new RecordSetBuilder(this, p_ObjectNamespace);
            FactoryBuilder factoryBuilder = new FactoryBuilder(this, p_ObjectNamespace, "RecordSet");

            foreach (TemplateRelation rel in p_Schema.Tables.FindAll(t => t.RelationName == "no_pkey" || t.RelationName == "tbluser" ))
            {
                rel.Prepare(p_Project);

                objectBuilder.Create(rel, doBuildFolder);
                recordsetBuilder.Create(rel, doBuildFolder);

                factoryBuilder.Reset();

                // create insert method
                factoryBuilder.AddMethod("insert",factoryBuilder.CreateInsertMethod(rel));

                // loop every index on this relation
                foreach (Index<TemplateColumn> index in rel.Indexes)
                {
                    if(index.IndexType == IndexType.PrimaryKey)
                        factoryBuilder.AddMethod(factoryBuilder.CreateGetySingleReturnMethod(rel, index));

                    if (index.IndexType == IndexType.UniqueIndex)
                        factoryBuilder.AddMethod(factoryBuilder.CreateGetySingleReturnMethod(rel, index));
                }

                factoryBuilder.Create(rel, doBuildFolder);
            }

            /*
            foreach (TemplateRelation rel in p_Schema.Views)
            {
                rel.Prepare(p_Project);
                objectBuilder.Create(rel, doBuildFolder);
                recordsetBuilder.Create(rel, doBuildFolder);

                factoryBuilder.Reset();
                factoryBuilder.Create(rel, doBuildFolder);
            }
            */

            AssemblyInfoData asmInfo = new AssemblyInfoData();
            AssemblyInfoBuilder asmInfoBuilder = new AssemblyInfoBuilder(p_Project.AssemblyInfo, this);
            File.WriteAllText(string.Format(@"{0}\AssemblyInfo.cs", doBuildFolder), asmInfoBuilder.BuildToString());

            p_DataObjectAssemblyFile = string.Format(@"{0}\{1}.Objects.dll", p_Project.OutputFolder, p_Project.RootNamespace);

            CSharpCodeProvider cscProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            CompilerParameters compParams = new CompilerParameters();

            compParams.GenerateExecutable = false;
            compParams.CompilerOptions = string.Format("/optimize /doc:{0}", p_DataObjectAssemblyFile.Replace(".dll",".xml"));
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
    }
}
