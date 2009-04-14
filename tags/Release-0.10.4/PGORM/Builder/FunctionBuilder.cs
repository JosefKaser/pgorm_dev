/*-------------------------------------------------------------------------
 * FunctionBuilder.cs
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
    public class FunctionBuilder : TemplateBase
    {
        protected DatabaseSchema schema;

        public FunctionBuilder(ProjectFile p_project, VS2008Project p_vsproject, DatabaseSchema p_schema, Builder p_builder)
            : base(CodeTemplates.Function_stg, p_vsproject, p_project, p_builder)
        {
            schema = p_schema;
        }

        string HasArgs(Function func)
        {
            return func.Parameters.Count != 0 ? "," : "";
        }

        string CheckSetNullable(string data)
        {
            if (data == "void" || data.Contains("[]") || data == typeof(string).ToString())
                return data;
            else
                return data + "?";
        }

        public override void Build()
        {
            List<Function> sel_functions = new List<Function>();
            StringTemplate stored_functions = GetTemplate("create_stored_function");
            stored_functions.SetAttribute("libs", project.UsingLibs);
            stored_functions.SetAttribute("namespace", project.RootNamespace);

            StringTemplate composite_set_returning = GetTemplate("create_composite_set_returning");
            StringTemplate composite_scalar = GetTemplate("create_composite_scalar");
            StringTemplate built_in_set_returning = GetTemplate("create_built_in_set_returning");
            StringTemplate built_in_scalar = GetTemplate("create_built_in_scalar");
            StringTemplate void_function = GetTemplate("create_void_function");

            if (schema.StoredFunctions != null && schema.StoredFunctions.Count != 0)
            {
                #region Filter unselected functions if possible
                if (project.Functions.Count != 0)
                {
                    var selectedFunctions = from t in schema.StoredFunctions
                                            join s in project.Functions on t.FunctionName equals s
                                            select t;

                    foreach (Function t in selectedFunctions)
                        sel_functions.Add(t);

                    schema.StoredFunctions = sel_functions;
                } 
                #endregion

                foreach (Function func in schema.StoredFunctions)
                {
                    if (func.ReturnTypeType == "c")
                    {
                        func.ReturnType = Helper.RemovePrefix(func.ReturnType, project.RemoveTablePrefix);
                        func.ReturnTypeRecordSet = Helper.RemovePrefix(func.ReturnTypeRecordSet, project.RemoveTablePrefix);
                        func.FactoryName = Helper.RemovePrefix(func.FactoryName, project.RemoveTablePrefix);
                    }

                    if (func.ReturnType == "void")
                    {
                        void_function.Reset();
                        void_function.SetAttribute("args", HasArgs(func));
                        void_function.SetAttribute("function", func);
                        void_function.SetAttribute("returntype", func.ReturnType);
                        stored_functions.SetAttribute("functions", void_function.ToString());
                    }
                    else if (func.ReturnsSet)
                    {
                        if (func.ReturnTypeType == "c") // composite
                        {
                            composite_set_returning.Reset();
                            composite_set_returning.SetAttribute("args", HasArgs(func));
                            composite_set_returning.SetAttribute("function", func);
                            composite_set_returning.SetAttribute("returntype", func.ReturnTypeRecordSet);
                            stored_functions.SetAttribute("functions", composite_set_returning.ToString());
                        }
                        else
                        {
                            func.ReturnType = CheckSetNullable(func.ReturnType);
                            built_in_set_returning.Reset();
                            built_in_set_returning.SetAttribute("args", HasArgs(func));
                            built_in_set_returning.SetAttribute("function", func);
                            built_in_set_returning.SetAttribute("returntype", func.ReturnType);
                            stored_functions.SetAttribute("functions", built_in_set_returning.ToString());
                        }
                    }
                    else // scalar
                    {
                        if (func.ReturnTypeType == "c") // composite
                        {
                            composite_scalar.Reset();
                            composite_scalar.SetAttribute("args", HasArgs(func));
                            composite_scalar.SetAttribute("function", func);
                            composite_scalar.SetAttribute("returntype", func.ReturnType);
                            stored_functions.SetAttribute("functions", composite_scalar.ToString());
                        }
                        else
                        {
                            func.ReturnType = CheckSetNullable(func.ReturnType);
                            built_in_scalar.Reset();
                            built_in_scalar.SetAttribute("args", HasArgs(func));
                            built_in_scalar.SetAttribute("function", func);
                            built_in_scalar.SetAttribute("returntype", func.ReturnType);
                            stored_functions.SetAttribute("functions", built_in_scalar.ToString());
                        }
                    }
                }
            }
            vsproject.AddCompileItem(@"StoredFunctions.cs", stored_functions.ToString());
        }
    }
}