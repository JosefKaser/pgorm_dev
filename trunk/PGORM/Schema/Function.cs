using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM
{
    public class Function
    {
        public string FunctionName { get; set; }
        public string ReturnType { get; set; }
        public string ReturnTypeType { get; set; }
        public string ReturnTypeRecordSet {get;set;}
        public string FactoryName { get; set; }
        public List<Column> Parameters = new List<Column>();
        public bool ReturnsSet { get; set; }

        public override string ToString()
        {
            string n = FunctionName;
            foreach (Column col in Parameters)
                n += col.ColumnName + " " + col.CLR_Type.ToString();
            return n;
        }
    }
}
