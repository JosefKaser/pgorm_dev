using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using PGORM.CodeBuilder.TemplateObjects;

namespace PGORM.CodeBuilder
{
    public class Helper
    {
        public static string GetDirectoryNameIncremented(string path, string name)
        {
            int a = 0;
            while (Directory.Exists(string.Format(@"{0}\{1}.{2}", path, name, a)))
                a++;
            return string.Format(@"{0}\{1}.{2}", path, name, a);
        }

        public static string MakeCLRSafe(string data)
        {
            return data.Replace(" ", "_");
        }

        public static string RemovePrefix(string source, List<string> items)
        {
            foreach (string prefix in items)
                if (source.IndexOf(prefix) == 0)
                {
                    source = source.Replace(prefix, "");
                    return source;
                }
            return source;
        }

        public static string GetExplicitNamespace(Project project, TemplateRelation rel)
        {
            return string.Format("{0}.{1}", project.RootNamespace, rel.TemplateNamespace);
        }

        public static string GetExplicitNamespace(Project project, TemplateFunction rel)
        {
            return string.Format("{0}.{1}", project.RootNamespace, rel.SchemaName.ToUpper());
        }


        public static string Asm35(string name)
        {
            return string.Format(@"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.5\{0}.dll", name);
        }

        private static List<string> LocalNpgsqlAssemblies()
        {
            string root = AppDomain.CurrentDomain.BaseDirectory;
            List<string> result = new List<string>();
            result.Add(string.Format(@"{0}\{1}", root, "Npgsql.dll"));
            result.Add(string.Format(@"{0}\{1}", root, "Mono.Security.dll"));
            return result;
        }

        public static void AddNpgsqlReferences(CompilerParameters compParams)
        {
            foreach(string item in LocalNpgsqlAssemblies())
                compParams.ReferencedAssemblies.Add(item);
       }

        public static void CopyNpgsqlAssemblies(string output_folder)
        {
            foreach (string item in LocalNpgsqlAssemblies())
            {
                string fname = Path.GetFileName(item);
                string dest = string.Format(@"{0}\{1}",output_folder,fname);
                File.Copy(item, dest,true);
            }
        }

        public static D DirtyCopy<S, D>(S source) where D : new()
        {
            D result = new D();

            Type sourceType = source.GetType();
            Type destinationType = result.GetType();

            foreach (PropertyInfo sourceProperty in sourceType.GetProperties())
            {
                PropertyInfo destinationProperty = destinationType.GetProperty(sourceProperty.Name);
                if (destinationProperty != null)
                {
                    if (destinationProperty.GetSetMethod() != null)
                        destinationProperty.SetValue(result, sourceProperty.GetValue(source, null), null);
                }
            }
            return result;
        }

        private static List<string> p_ReservedWords = new List<string>();
        public static bool IsReservedWord(string word)
        {
            // load if first time
            if (p_ReservedWords.Count == 0)
                p_ReservedWords.AddRange(HelperResources.ReservedWords.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            return p_ReservedWords.Contains(word);
        }

        public static string[] RemoveArrayItemIfExist(string[] array, string item)
        {
            List<string> list = array.ToList();
            list.Remove(item);
            return list.ToArray();
        }

    }
}
