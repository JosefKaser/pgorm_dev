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
        private void CreateEnums(string daBuildFolder)
        {
            p_Schema.Enums.ForEach(i => i.Prepare(this));
            CLREnumBuilder enumBuilder = new CLREnumBuilder(this, "Enums");
            #region composite enums
            foreach (TemplateRelation rel in p_Schema.Enums)
            {
                enumBuilder.Reset();
                SendMessage(this, ProjectBuilderMessageType.Major, "Generating code for {0}", rel.RelationName);
                enumBuilder.Create(rel, daBuildFolder);
            }
            #endregion
        }

        private void CreateDataAccessProject()
        {
            SendMessage(this, ProjectBuilderMessageType.Major, "Creating Core components.");
            string daBuildFolder = string.Format(@"{0}\DataAccess", p_BuildFolder);
            Directory.CreateDirectory(daBuildFolder);
            File.WriteAllText(string.Format(@"{0}\Helper.cs", daBuildFolder), DataAccessProjectFiles.Helper.Replace("MY_NAMESPACE", p_Project.RootNamespace));
            File.WriteAllText(string.Format(@"{0}\DataAccess.cs", daBuildFolder), DataAccessProjectFiles.DataAccess.Replace("MY_NAMESPACE", p_Project.RootNamespace));
            File.WriteAllText(string.Format(@"{0}\DatabaseOperation.cs", daBuildFolder), DataAccessProjectFiles.DatabaseOperation.Replace("MY_NAMESPACE", p_Project.RootNamespace));
            File.WriteAllText(string.Format(@"{0}\DataObjectBase.cs", daBuildFolder), DataAccessProjectFiles.DataObjectBase.Replace("MY_NAMESPACE", p_Project.RootNamespace));
            File.WriteAllText(string.Format(@"{0}\DataObjectValue.cs", daBuildFolder), DataAccessProjectFiles.DataObjectValue.Replace("MY_NAMESPACE", p_Project.RootNamespace));
            File.WriteAllText(string.Format(@"{0}\DataObjectValueTypeConverter.cs", daBuildFolder), DataAccessProjectFiles.DataObjectValueTypeConverter.Replace("MY_NAMESPACE", p_Project.RootNamespace));
            File.WriteAllText(string.Format(@"{0}\DataObjectRecordSetBase.cs", daBuildFolder), DataAccessProjectFiles.DataObjectRecordSetBase.Replace("MY_NAMESPACE", p_Project.RootNamespace));
            File.WriteAllText(string.Format(@"{0}\PostgreSQLTypeConverter.cs", daBuildFolder), DataAccessProjectFiles.PostgreSQLTypeConverter.Replace("MY_NAMESPACE", p_Project.RootNamespace));
            CreateEnums(daBuildFolder);

            AssemblyInfoData asmInfo = new AssemblyInfoData();
            AssemblyInfoBuilder asmInfoBuilder = new AssemblyInfoBuilder(asmInfo, this);
            File.WriteAllText(string.Format(@"{0}\AssemblyInfo.cs", daBuildFolder), asmInfoBuilder.BuildToString());

            string p_ProjAsmName = Path.GetFileNameWithoutExtension(p_Project.AssemblyName);
            p_DataAccessAssemblyFile = string.Format(@"{0}\{1}.core.dll", p_Project.OutputFolder, p_ProjAsmName);

            SendMessage(this, ProjectBuilderMessageType.Major, "Building {0} assembly.", p_DataAccessAssemblyFile);

            CSharpCodeProvider cscProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            CompilerParameters compParams = new CompilerParameters();

            compParams.GenerateExecutable = false;
            compParams.CompilerOptions = "/optimize";
            compParams.IncludeDebugInformation = p_Project.BuildInDebugMode;
            compParams.OutputAssembly = p_DataAccessAssemblyFile;
            compParams.ReferencedAssemblies.Add("System.dll");
            compParams.ReferencedAssemblies.Add("System.Xml.dll");
            compParams.ReferencedAssemblies.Add("System.Data.dll");

            compParams.ReferencedAssemblies.Add(Helper.Asm35("System.Core"));
            compParams.ReferencedAssemblies.Add(Helper.Asm35("System.Data.DataSetExtensions"));
            compParams.ReferencedAssemblies.Add(Helper.Asm35("System.Xml.Linq"));

            Helper.AddNpgsqlReferences(compParams);

            string[] files = Directory.GetFiles(daBuildFolder, "*.cs", SearchOption.AllDirectories);

            CompilerResults results = cscProvider.CompileAssemblyFromFile(compParams, files);
            Helper.CopyNpgsqlAssemblies(p_Project.OutputFolder);

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results.Errors)
                {
                    SendMessage(this, ProjectBuilderMessageType.Error, "{0}",
                        "Line number " + CompErr.Line +
                        ", Error Number: " + CompErr.ErrorNumber +
                        ", '" + CompErr.ErrorText + ";");
                }
            }
        }
	}
}
