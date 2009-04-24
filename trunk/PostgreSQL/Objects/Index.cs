using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostgreSQL.Objects
{
    public enum IndexType
    {
        PrimaryKey,
        ForeignKey,
        UniqueIndex,
        CustomIndex
    }

    public class Index<C> where C : Column, new()
    {
        public string IndexName { get; set; }
        public List<C> Columns;
        public IndexType IndexType { get; set; }
        public Index()
        {
            Columns = new List<C>();
            IndexType = IndexType.CustomIndex;
            IndexName = "";
        }

        public bool Equals(Index<C> i)
        {
            string sa = "", sb = "";
            i.Columns.ForEach(c => sa += c.ColumnName + c.PG_Type);
            this.Columns.ForEach(c => sb += c.ColumnName + c.PG_Type);
            return sb == sa;
        }

        public string Signiture
        {
            get
            {
                String sig = "";
                this.Columns.ForEach(c => sig += string.Format("{0}{1}:",c.ColumnName,c.PG_Type));
                return sig;
            }
        }
    }
}