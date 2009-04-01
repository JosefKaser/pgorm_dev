/*-------------------------------------------------------------------------
 * ObjectBuilder.cs
 *
 * This file is part of the PGORM project.
 * http://pgorm.googlecode.com/
 *
 * Copyright (c) 2002-2009, TrueSoftware B.V.
 *
 * IDENTIFICATION
 * 
 *  $Id$
 * 	$HeadURL$
 * 	
 *-------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Antlr.StringTemplate;

namespace PGORM
{
    public class ObjectBuilder : TemplateBase
    {
        protected DatabaseSchema schema;
        protected StringTemplateGroup recordsetGroup,factoryGroup,fullLoader;
        StringTemplate dataobject,recordset,factory,
            getby_method, insert_method, update_method,delete_method,get_all_method;

        public ObjectBuilder(ProjectFile p_project, VS2008Project p_vsproject, DatabaseSchema p_schema, Builder p_builder)
            : base(CodeTemplates.DataObject_stg, p_vsproject, p_project,p_builder)
        {
            schema = p_schema;
            recordsetGroup = new StringTemplateGroup(new StringReader(CodeTemplates.DataObjectRecordSet_stg));
            factoryGroup = new StringTemplateGroup(new StringReader(CodeTemplates.DataObjectFactory_stg));
        }

        public override void Build()
        {
            string out_dir="";
            dataobject = GetTemplate("dataobject");
            recordset = recordsetGroup.GetInstanceOf("recordset");
            factory = factoryGroup.GetInstanceOf("factory");
            getby_method = factoryGroup.GetInstanceOf("create_getby_method");
            insert_method = factoryGroup.GetInstanceOf("create_insert_method");
            update_method = factoryGroup.GetInstanceOf("create_update_method");
            delete_method = factoryGroup.GetInstanceOf("create_delete_method");
            get_all_method = factoryGroup.GetInstanceOf("get_all_method");


            SendMessage("Generating database objects...", BuilderMessageType.Major);


            #region Create Directories
            out_dir = project.ProjectOutputFolder + "\\DataObject";
            if (!Directory.Exists(out_dir))
                Directory.CreateDirectory(out_dir);

            out_dir = project.ProjectOutputFolder + "\\DataObjectFactory";
            if (!Directory.Exists(out_dir))
                Directory.CreateDirectory(out_dir);

            out_dir = project.ProjectOutputFolder + "\\DataObjectRecordSet";
            if (!Directory.Exists(out_dir))
                Directory.CreateDirectory(out_dir); 
            #endregion

            List<Table> sel_tables = new List<Table>();

            foreach (Table ct in project.CutsomQueries)
                project.Tables.Add(ct.TableName);

            if (project.Tables.Count != 0)
            {
                var selectedTables = from t in schema.Tables
                                     join s in project.Tables on t.TableName equals s
                                     select t;

                foreach (Table t in selectedTables)
                    sel_tables.Add(t);
            }

            if (project.Views.Count != 0)
            {

                var selectedViews = from t in schema.Tables
                                    join s in project.Views on t.TableName equals s
                                    select t;

                foreach (Table t in selectedViews)
                    sel_tables.Add(t);
            }

            if(sel_tables.Count != 0)
                schema.Tables = sel_tables;
           
            if (schema.CompositeTypes != null)
                foreach (Table t in schema.CompositeTypes)
                    schema.Tables.Add(t);

            foreach (Table table in schema.Tables)
            {
                SendMessage("Generate objects for {0}", BuilderMessageType.Minor,table.TableName);

                dataobject.Reset();
                dataobject.SetAttribute("libs", project.UsingLibs);
                dataobject.SetAttribute("namespace", project.RootNamespace);

                recordset.Reset();
                recordset.SetAttribute("libs", project.UsingLibs);
                recordset.SetAttribute("namespace", project.RootNamespace);

                factory.Reset();
                factory.SetAttribute("libs", project.UsingLibs);
                factory.SetAttribute("namespace", project.RootNamespace);


                #region Prepare Table
                table.TableName = Helper.RemovePrefix(table.TableName, project.RemoveTablePrefix);
                table.TemplateTableName = table.TableName;

                //TODO: consider SimplifyNames
                //if (project.SimplifyNames)
                //    table.TemplateTableName = table.TemplateTableName.ToUpper();

                table.FactoryName = table.TemplateTableName + "Factory";
                table.RecordSetName = table.TemplateTableName + "RecordSet";

                table.TemplateTableName += project.ObjectClassPostfix;

                foreach (Column c in table.Columns)
                    c.TemplateTableName = table.TemplateTableName;

                PrepareColumns(table.Columns); 
                #endregion

                dataobject.SetAttribute("table", table);
                recordset.SetAttribute("table", table);
                factory.SetAttribute("table", table);

                List<Index> indexGetMethods = new List<Index>();
                if (table.PrimaryKey.Columns.Count != 0)
                    indexGetMethods.Add(table.PrimaryKey);

                foreach (Index index in table.Indexes)
                    if (indexGetMethods.FindAll(im => im.ToString() == index.ToString()).Count() == 0)
                        indexGetMethods.Add(index);


                foreach (Index index in table.ForeignKeys)
                    if (indexGetMethods.FindAll(im => im.ToStringNoTable() == index.ToStringNoTable()).Count() == 0)
                        indexGetMethods.Add(index);

                foreach (Index index in indexGetMethods)
                {
                    PrepareColumns(index.Columns);
                    getby_method.Reset();
                    getby_method.SetAttribute("table", table);
                    getby_method.SetAttribute("icolumns", index.Columns);
                    factory.SetAttribute("getby_methods", getby_method.ToString());

                    if (table.IsInsertable && index.IndexType != IndexType.ForeignKey)
                    {
                        update_method.Reset();
                        update_method.SetAttribute("table", table);
                        update_method.SetAttribute("icolumns", index.Columns);
                        factory.SetAttribute("dml_functions", update_method.ToString());

                        delete_method.Reset();
                        delete_method.SetAttribute("table", table);
                        delete_method.SetAttribute("icolumns", index.Columns);
                        factory.SetAttribute("dml_functions", delete_method.ToString());
                    }
                }

                if (table.IsInsertable)
                {
                    insert_method.Reset();
                    insert_method.SetAttribute("table", table);
                    factory.SetAttribute("dml_functions", insert_method.ToString());
                }

                if (!table.IsCompositeType)
                {
                    get_all_method.Reset();
                    get_all_method.SetAttribute("table", table);
                    factory.SetAttribute("getby_methods", get_all_method.ToString());
                }

                string data_object_file = string.Format("{0}.cs", table.TemplateTableName);
                string factory_file = string.Format("{0}.cs", table.FactoryName);
                string recordset_file = string.Format("{0}.cs", table.RecordSetName);

                vsproject.AddCompileItem(string.Format(@"DataObject\{0}", data_object_file),dataobject.ToString());
                vsproject.AddCompileItem(string.Format(@"DataObjectFactory\{0}", factory_file), factory.ToString());
                vsproject.AddCompileItem(string.Format(@"DataObjectRecordSet\{0}", recordset_file), recordset.ToString());
            }
        }

        void PrepareColumns(List<Column> Columns)
        {
            foreach (Column col in Columns)
            {
                //TODO: consider SimplifyNames
                //if (project.SimplifyNames)
                //    col.TemplateColumnName = col.ColumnName.ToUpper();
                //else
                col.TemplateColumnName = col.ColumnName;
            }
        }
    }
}