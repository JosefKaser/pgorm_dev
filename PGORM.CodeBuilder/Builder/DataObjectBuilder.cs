using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGORM.CodeBuilder.TemplateObjects;
using PGORM.PostgreSQL.Objects;
using Antlr.StringTemplate;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;
using Microsoft.CSharp;

namespace PGORM.CodeBuilder
{
    public class DataObjectBuilder : TemplateBase
    {
        StringTemplate st;
        string[] p_libs;
        public DataObjectBuilder(ProjectBuilder p_builder,string[] p_Libs)
            : base(Templates.DataObject_stg, p_builder)
        {
            st = p_stgGroup.GetInstanceOf("dataobject");
            p_libs = p_Libs;
        }

        public void Create(TemplateRelation relation,string object_namespace, string destFolder, bool create_depend_converters,bool is_udt)
        {
            if (!string.IsNullOrEmpty(object_namespace))
                object_namespace = "." + object_namespace;

            string nspace = string.Format("{0}{1}", Helper.GetExplicitNamespace(p_Project, relation), object_namespace);
            string fname = string.Format(@"{0}\{1}_{2}.cs", destFolder, relation.SchemaName, relation.TemplateRelationName);
            string asmname = string.Format(@"{0}{1}_{2}.dll","", relation.SchemaName, relation.TemplateRelationName);
            st.Reset();
            st.SetAttribute("table", relation);
            st.SetAttribute("namespace", nspace);
            st.SetAttribute("libs", p_Project.InternalReferences);

            if(is_udt)
                st.SetAttribute("is_udt", true);

            SetLibs(st, p_libs, relation);

            File.WriteAllText(fname, st.ToString());

            //compile and load this udt
            if (create_depend_converters)
            {
                CompilerResults results;
                using (CSharpCodeProvider cscProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } }))
                {
                    CompilerParameters compParams = new CompilerParameters();

                    compParams.GenerateExecutable = false;
                    compParams.IncludeDebugInformation = p_Project.BuildInDebugMode;
                    compParams.ReferencedAssemblies.Add("System.dll");
                    compParams.ReferencedAssemblies.Add("System.Xml.dll");
                    compParams.ReferencedAssemblies.Add("System.Data.dll");

                    compParams.ReferencedAssemblies.Add(Helper.Asm35("System.Core"));
                    compParams.ReferencedAssemblies.Add(Helper.Asm35("System.Data.DataSetExtensions"));
                    compParams.ReferencedAssemblies.Add(Helper.Asm35("System.Xml.Linq"));
                    compParams.OutputAssembly = asmname;
                    compParams.GenerateInMemory = true;

                    Helper.AddNpgsqlReferences(compParams);
                    compParams.ReferencedAssemblies.Add(Builder.p_DataAccessAssemblyFile);

                    string[] files = new string[] { fname };

                    results = cscProvider.CompileAssemblyFromFile(compParams, files);
                }

                if (results.Errors.Count > 0)
                {
                    foreach (CompilerError CompErr in results.Errors)
                    {
                        Builder.SendMessage(this, ProjectBuilderMessageType.Error, "{0}",
                            "\r\nFile: " + CompErr.FileName +
                            "\n\rLine number: " + CompErr.Line +
                            "\n\rError Number: " + CompErr.ErrorNumber +
                            "\n\r" + CompErr.ErrorText + ";\n\r");
                    }
                }
                else if (results.Errors.Count == 0)
                {
                    //Builder.LoadAssembly(null, results.CompiledAssembly);
                    System.Threading.Thread.SpinWait(1000);
                    System.Threading.Thread.Sleep(250);
                    Builder.LoadAssembly(null, results.CompiledAssembly);

                }
            }
        }
    }
}
