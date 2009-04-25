using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeBuilder.TemplateObjects;
using Antlr.StringTemplate;
using System.IO;
using PostgreSQL.Objects;

namespace CodeBuilder
{
    public class FactoryBuilder : TemplateBase
    {
        StringTemplate st;
        StringTemplate insert_method;
        StringTemplate getby_single_return_method;
        StringTemplate create_method_sub_name;
        StringTemplate get_method;
        StringTemplate code_summary;
        string p_ObjectNamespace;
        string p_RecordSetNamespace;
        public List<TemplateMethod> Methods;

        public FactoryBuilder(ProjectBuilder p_builder, string object_namespace,string recordet_namespace)
            : base(Templates.DataObjectFactory_stg, p_builder)
        {
            st = p_stgGroup.GetInstanceOf("factory");
            p_ObjectNamespace = object_namespace;
            p_RecordSetNamespace = recordet_namespace;

            insert_method = p_stgGroup.GetInstanceOf("insert_method");
            getby_single_return_method = p_stgGroup.GetInstanceOf("getby_single_return_method");
            get_method = p_stgGroup.GetInstanceOf("get_method");
            code_summary = p_stgGroup.GetInstanceOf("code_summary");
            create_method_sub_name = p_stgGroup.GetInstanceOf("create_method_sub_name");

            Methods = new List<TemplateMethod>();
        }

        public void Reset()
        {
            st.Reset();
            Methods.Clear();
        }

        public void AddMethod(string sig, string content)
        {
            Methods.Add(new TemplateMethod()
            {
                Signiture = sig,
                Content = content
            }
            );
        }

        public void AddMethod(TemplateMethod method)
        {
            Methods.Add(method);
        }

        public string CodeSummary(string text,params object[] args)
        {
            code_summary.Reset();
            code_summary.SetAttribute("text", string.Format(text, args));
            return code_summary.ToString();
        }

        public string CreateInsertMethod(TemplateRelation relation)
        {
            insert_method.Reset();
            insert_method.SetAttribute("table", relation);
            string s = insert_method.ToString();
            return insert_method.ToString();
        }

        public string CreateGetAllMethod(TemplateRelation rel)
        {
            get_method.Reset();
            get_method.SetAttribute("table", rel);
            get_method.SetAttribute("method_name", "GetList");
            get_method.SetAttribute("summary", CodeSummary("Retrives a generic List&lt;{0}&gt;", rel.TemplateRelationName));
            return get_method.ToString();
        }

        public TemplateMethod CreateGetMultiReturnMethod(TemplateRelation rel, string method_name, Index<TemplateColumn> index,string summary)
        {
            //get_method(table,method_name,icolumns,sep_comma,summary)
            TemplateMethod method = new TemplateMethod();
            get_method.Reset();
            get_method.SetAttribute("table", rel);
            get_method.SetAttribute("method_name", method_name);
            get_method.SetAttribute("summary", summary);
            get_method.SetAttribute("icolumns", index.Columns);
            get_method.SetAttribute("sep_comma", ",");
            method.Content = get_method.ToString();
            method.Signiture = index.Signiture;
            return method;
        }

        public TemplateMethod CreateGetSingleReturnMethod(TemplateRelation rel, Index<TemplateColumn> index)
        {
            TemplateMethod method = new TemplateMethod();
            getby_single_return_method.Reset();
            getby_single_return_method.SetAttribute("table", rel);
            getby_single_return_method.SetAttribute("icolumns", index.Columns);
            getby_single_return_method.SetAttribute("index", index);
            method.Content = getby_single_return_method.ToString();
            method.Signiture = index.Signiture;
            return method;
        }

        public string CreateMethodSubName(List<TemplateColumn> columns)
        {
            create_method_sub_name.Reset();
            create_method_sub_name.SetAttribute("columns", columns);
            return create_method_sub_name.ToString();
        }

        public void Create(TemplateRelation relation, string destFolder)
        {
            string nspace = Helper.GetExplicitNamespace(p_Project, relation);
            string fname = string.Format(@"{0}\{1}_{2}_factory.cs", destFolder, relation.SchemaName, relation.TemplateRelationName);
            st.SetAttribute("table", relation);
            st.SetAttribute("namespace", Helper.GetExplicitNamespace(p_Project, relation));
            st.SetAttribute("libs", p_Project.InternalReferences);
            st.SetAttribute("libs", string.Format("{0}.Factory", nspace));
            if (!string.IsNullOrEmpty(p_ObjectNamespace))
                st.SetAttribute("libs", string.Format("{0}.{1}", Helper.GetExplicitNamespace(p_Project, relation), p_ObjectNamespace));

            if (!string.IsNullOrEmpty(p_RecordSetNamespace))
                st.SetAttribute("libs", string.Format("{0}.{1}", Helper.GetExplicitNamespace(p_Project, relation), p_RecordSetNamespace));

            if (Methods.Count != 0)
            {
                List<TemplateMethod> distinct_method = new List<TemplateMethod>();
                foreach(TemplateMethod item in Methods)
                {
                    if(!distinct_method.Exists(m => m.Signiture == item.Signiture))
                        distinct_method.Add(item);
                }
                distinct_method.ForEach(m => st.SetAttribute("methods", m.Content));
            }

            File.WriteAllText(fname, st.ToString());
        }
    }
}
