using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.CSharp;
using PGORM.PostgreSQL;
using PGORM.PostgreSQL.Objects;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Reflection;
using PGORM.CodeBuilder.TemplateObjects;


namespace PGORM.CodeBuilder
{
    public class ConverterProxy
    {
        public  object instance;

        public string PgType {get;private set;}
        public string PgTypeSchema { get; private set; }
        public Type CLRType { get; private set; }
        public string Converter { get; private set; }

        public ConverterProxy(object obj)
        {
            instance = obj;
            PgType = (string)GetPropertyValue("PgType");
            PgTypeSchema = (string)GetPropertyValue("PgTypeSchema");
            CLRType = (Type)GetPropertyValue("CLRType");
            Converter = obj.GetType().ToString(); // provides the full name
        }

        private object GetPropertyValue(string name)
        {
            return instance.GetType().InvokeMember(name, BindingFlags.InvokeMethod, null, instance, null);
        }
    }
}
