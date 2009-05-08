using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGORM.CodeBuilder.TemplateObjects;
using Antlr.StringTemplate;
using System.IO;
using PGORM.PostgreSQL.Objects;

namespace PGORM.CodeBuilder
{
    public class FactoryBuilder : TemplateBase
    {
        StringTemplate st;
        StringTemplate insert_method;
        StringTemplate getby_single_return_method;
        StringTemplate create_method_sub_name;
        StringTemplate get_method;
        StringTemplate delete_method;
        StringTemplate code_summary;
        StringTemplate record_count;
        StringTemplate update_method_single;
        StringTemplate update_many_method;
        StringTemplate delete_all_method;
        StringTemplate copy_in_method;

        string p_ObjectNamespace;
        string p_RecordSetNamespace;
        public List<TemplateMethod> Methods;

        #region FactoryTemplate
        private static string FactoryTemplate()
        {
            return string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}\r\n{6}\r\n{7}\r\n{8}\r\n",
                Templates.Factory_Main_stg,
                Templates.Factory_InsertInto_stg,
                Templates.Factory_GetSingle_stg,
                Templates.Factory_GetManyBy_stg,
                Templates.Factory_Factory_stg,
                Templates.Factory_Delete_stg,
                Templates.Factory_RecordCount_stg,
                Templates.Factoty_Update_stg,
                Templates.Factory_Copy_stg
                );

        } 
        #endregion

        #region FactoryBuilder
        public FactoryBuilder(ProjectBuilder p_builder, string object_namespace, string recordet_namespace)
            : base(FactoryTemplate(), p_builder)
        {
            st = p_stgGroup.GetInstanceOf("factory");
            p_ObjectNamespace = object_namespace;
            p_RecordSetNamespace = recordet_namespace;

            insert_method = p_stgGroup.GetInstanceOf("insert_method");
            getby_single_return_method = p_stgGroup.GetInstanceOf("getby_single_return_method");
            get_method = p_stgGroup.GetInstanceOf("get_method");
            code_summary = p_stgGroup.GetInstanceOf("code_summary");
            create_method_sub_name = p_stgGroup.GetInstanceOf("create_method_sub_name");
            delete_method = p_stgGroup.GetInstanceOf("delete_method");
            delete_all_method = p_stgGroup.GetInstanceOf("delete_all_method");
            record_count = p_stgGroup.GetInstanceOf("record_count");
            update_method_single = p_stgGroup.GetInstanceOf("update_method_single");
            update_many_method = p_stgGroup.GetInstanceOf("update_many_method");
            copy_in_method = p_stgGroup.GetInstanceOf("copy_in_method");


            Methods = new List<TemplateMethod>();
        } 
        #endregion

        #region Reset
        public void Reset()
        {
            st.Reset();
            Methods.Clear();
        } 
        #endregion

        #region AddMethod
        public void AddMethod(string sig, string content)
        {
            Methods.Add(new TemplateMethod()
            {
                Signiture = sig,
                Content = content
            }
            );
        } 
        #endregion

        #region AddMethod
        public void AddMethod(TemplateMethod method)
        {
            Methods.Add(method);
        } 
        #endregion

        #region CodeSummary
        public string CodeSummary(string text, params object[] args)
        {
            code_summary.Reset();
            code_summary.SetAttribute("text", string.Format(text, args));
            return code_summary.ToString();
        } 
        #endregion

        #region CreateDeleteAllMethod
        public TemplateMethod CreateDeleteAllMethod(TemplateRelation rel)
        {
            TemplateMethod method = new TemplateMethod();
            delete_all_method.Reset();
            delete_all_method.SetAttribute("table", rel);
            method.Content = delete_all_method.ToString();
            method.Signiture = "delete_all" + rel.TemplateRelationName;
            return method;
        }
        #endregion


        #region CreateInsertMethod
        public string CreateInsertMethod(TemplateRelation relation)
        {
            insert_method.Reset();
            insert_method.SetAttribute("table", relation);
            insert_method.SetAttribute("method_name", "Insert");
            return insert_method.ToString();
        } 
        #endregion

        #region CreateCopyInMethod
        public TemplateMethod CreateCopyInMethod(TemplateRelation relation)
        {
            TemplateMethod method = new TemplateMethod();
            copy_in_method.Reset();
            copy_in_method.SetAttribute("table", relation);
            method.Content = copy_in_method.ToString();
            method.Signiture = "copyin_" + relation.RelationName;
            return method;
        }
        #endregion


        #region CreateGetAllMethod
        public string CreateGetAllMethod(TemplateRelation rel)
        {
            get_method.Reset();
            get_method.SetAttribute("table", rel);
            get_method.SetAttribute("method_name", "GetList");
            get_method.SetAttribute("isgetall", "true");
            get_method.SetAttribute("summary", CodeSummary("Retrives a generic List&lt;{0}&gt;", rel.TemplateRelationName));
            return get_method.ToString();
        } 
        #endregion

        #region CreateGetMultiReturnMethod
        public TemplateMethod CreateGetMultiReturnMethod(TemplateRelation rel, string method_name, Index<TemplateColumn> index, string summary)
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
        #endregion

        #region CreateCountRecords
        public TemplateMethod CreateCountRecords(TemplateRelation rel)
        {
            TemplateMethod method = new TemplateMethod();
            record_count.Reset();
            record_count.SetAttribute("table", rel);
            method.Content = record_count.ToString();
            method.Signiture = "recond_count";
            return method;
        }
        
        #endregion

        #region CreateDeleteMethod
        public TemplateMethod CreateDeleteMethod(TemplateRelation rel, Index<TemplateColumn> index)
        {
            TemplateMethod method = new TemplateMethod();
            delete_method.Reset();
            delete_method.SetAttribute("table", rel);
            delete_method.SetAttribute("icolumns", index.Columns);
            method.Content = delete_method.ToString();
            method.Signiture = "deleteby_" + index.Signiture;
            return method;
        } 
        #endregion

        #region CreateGetSingleReturnMethod
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
        #endregion

        #region CreateMethodSubName
        public string CreateMethodSubName(List<TemplateColumn> columns)
        {
            create_method_sub_name.Reset();
            create_method_sub_name.SetAttribute("columns", columns);
            return create_method_sub_name.ToString();
        } 
        #endregion

        #region CreateUpdateManyMethod
        public TemplateMethod CreateUpdateManyMethod(TemplateRelation rel, string method_name, Index<TemplateColumn> index, string summary)
        {
            TemplateMethod method = new TemplateMethod();
            update_many_method.Reset();
            update_many_method.SetAttribute("table", rel);
            update_many_method.SetAttribute("icolumns", index.Columns);
            update_many_method.SetAttribute("method_name", method_name);
            update_many_method.SetAttribute("summary", summary);
            method.Content = update_many_method.ToString();
            method.Signiture = "update_many_" + index.Signiture;
            return method;
        } 
        #endregion

        #region CreateUpdateMethodSingle
        public TemplateMethod CreateUpdateSingleMethod(TemplateRelation rel, Index<TemplateColumn> index)
        {
            TemplateMethod method = new TemplateMethod();
            update_method_single.Reset();
            update_method_single.SetAttribute("table", rel);
            update_method_single.SetAttribute("icolumns", index.Columns);
            method.Content = update_method_single.ToString();
            method.Signiture = "update_" + index.Signiture;
            return method;
        } 
        #endregion

        #region Create
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
                foreach (TemplateMethod item in Methods)
                {
                    if (!distinct_method.Exists(m => m.Signiture == item.Signiture))
                        distinct_method.Add(item);
                }
                distinct_method.ForEach(m => st.SetAttribute("methods", m.Content));
            }

            File.WriteAllText(fname, st.ToString());
        } 
        #endregion
    }
}
