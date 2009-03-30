using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Diagnostics;
using Antlr.StringTemplate;
using System.Reflection;


namespace PGORM
{
    public class VS2008Project : TemplateBase
    {
        #region Props
        public string ProjectGuild = System.Guid.NewGuid().ToString();

        protected StringTemplate csprojTemplate;
        protected StringTemplate compileItem;
        protected StringTemplate projectRef,dllRef; 
        #endregion

        #region VS2008Project
        public VS2008Project(ProjectFile p_project, Builder p_builder)
            : base(CodeTemplates.VS2008_Project_stg, p_project,p_builder)
        {
            csprojTemplate = GetTemplate("cs_project");
            compileItem = GetTemplate("cs_add_compile_item");
            csprojTemplate.SetAttribute("projectGuid", ProjectGuild);
            projectRef = GetTemplate("cs_add_project_ref");
            dllRef = GetTemplate("cs_add_dll_ref");
            Setup();
            csprojTemplate.SetAttribute("compile_output_folder", project.CompilerOutputFolder);
        } 
        #endregion

        #region AddCompileItem
        public void AddCompileItem(string path,string contents)
        {
            compileItem.Reset();
            compileItem.SetAttribute("path", path);
            csprojTemplate.SetAttribute("compile_items", compileItem.ToString());

            File.WriteAllText(
                string.Format(@"{0}\{1}", project.ProjectOutputFolder,path),
                contents);
        }
        #endregion

        #region Setup
        void Setup()
        {

#if DEBUG
            if (Directory.Exists(project.ProjectOutputFolder))
            {
                System.Threading.Thread.Sleep(500);
                SendMessage("Deleting existing {0}",project.ProjectOutputFolder);
                Directory.Delete(project.ProjectOutputFolder, true);
            }
#endif
            if (!Directory.Exists(project.OutputFolder))
            {
                System.Threading.Thread.Sleep(250);
                Directory.CreateDirectory(project.OutputFolder);
                System.Threading.Thread.Sleep(250);
            }

            if (!Directory.Exists(project.ProjectOutputFolder))
            {
                System.Threading.Thread.Sleep(250);
                Directory.CreateDirectory(project.ProjectOutputFolder);
                System.Threading.Thread.Sleep(250);
                Directory.CreateDirectory(project.ProjectOutputFolder + "\\" + "Objects");
                System.Threading.Thread.Sleep(250);
            }
        }
        #endregion

        #region Build
        public override void Build()
        {
            csprojTemplate.SetAttribute("project", project);

            foreach (string item in project.ProjectRefs)
                AddProjectRef(item);

            File.WriteAllText(
                project.CPROJName,
                csprojTemplate.ToString());
        }
        #endregion

        #region AddProjectRef
        void AddProjectRef(string path)
        {

            projectRef.Reset();
            projectRef.SetAttribute("path", path);

            if (Path.HasExtension(path) && Path.GetExtension(path).ToLower() == ".dll")
            {
                Assembly asm = Assembly.LoadFile(path);
                dllRef.Reset();
                dllRef.SetAttribute("name", asm.FullName);
                dllRef.SetAttribute("path", path);
                csprojTemplate.SetAttribute("project_refs", dllRef.ToString());
            }
            else
            {
                string ns = "{http://schemas.microsoft.com/developer/msbuild/2003}";
                XDocument xdoc = XDocument.Load(new StreamReader(path));
                var items = from i in xdoc.Descendants(ns + "PropertyGroup").Descendants()
                            where i.Name.LocalName == "ProjectGuid" || i.Name.LocalName == "AssemblyName"
                            select i;


                foreach (XElement item in items)
                {
                    if (item.Name.LocalName == "ProjectGuid")
                        projectRef.SetAttribute("guid", item.Value);

                    if (item.Name.LocalName == "AssemblyName")
                        projectRef.SetAttribute("name", item.Value);

                }

                string s = projectRef.ToString();
                csprojTemplate.SetAttribute("project_refs", projectRef.ToString());
            }
        } 
        #endregion
    }
}
