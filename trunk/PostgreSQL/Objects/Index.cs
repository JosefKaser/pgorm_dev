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

    public class Index<C> : ICloneable where C : Column, new() 
    {
        public string IndexName { get; set; }
        public List<C> Columns;
        public IndexType IndexType { get; set; }

        #region Index
        public Index()
        {
            Columns = new List<C>();
            IndexType = IndexType.CustomIndex;
            IndexName = "";
        }
        
        #endregion

        #region Index
        public bool Equals(Index<C> i)
        {
            string sa = "", sb = "";
            i.Columns.ForEach(c => sa += c.ColumnName + c.PG_Type);
            this.Columns.ForEach(c => sb += c.ColumnName + c.PG_Type);
            return sb == sa;
        } 
        #endregion

        #region Signiture
        public string Signiture
        {
            get
            {
                String sig = "";
                this.Columns.ForEach(c => sig += string.Format("{0}{1}:", c.ColumnName, c.PG_Type));
                return sig;
            }
        } 
        #endregion

        #region ICloneable Members

        public object Clone()
        {
            Index<C> i = new Index<C>();
            i.IndexName = this.IndexName;
            this.Columns.ForEach(c => i.Columns.Add(c.Clone<C>()));
            i.IndexType = this.IndexType;
            return i;
        }

        #endregion
    }
}