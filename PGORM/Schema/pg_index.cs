using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM
{
    public class pg_index
    {
        public pg_index(string tname, string keys,bool is_unique)
        {
            TableName = tname;
            IsUnique = is_unique;

            string[] skeys = keys.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int a = 0; a != skeys.Length; a++)
            {
                ColumnIndexes.Add(int.Parse(skeys[a]));
            }

        }
        public bool IsUnique { get; set; }
        public string TableName { get; set; }
        public List<int> ColumnIndexes = new List<int>(); 
    }
}
