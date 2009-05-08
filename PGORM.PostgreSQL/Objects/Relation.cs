using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM.PostgreSQL.Objects
{
    public enum RelationType
    {
        Table,
        View,
        CompositeType,
        Enum
    }

    public class Relation<C> where C : Column, new()
    {
        public string SchemaName { get; set; }
        public string RelationName { get; set; }
        public RelationType RelationType;
        public List<C> Columns { get; set; }
        public List<Index<C>> Indexes { get; set; }

        #region FullName
        public string FullName
        {
            get
            {
                return string.Format("\"{0}\".\"{1}\"", SchemaName, RelationName);
            }
        }

        public string FullNameInvariant
        {
            get
            {
                return FullName.Replace("\"", "");
            }
        }

        #endregion
        public Relation()
        {
            Columns = new List<C>();
            Indexes = new List<Index<C>>();
        }
    }
}