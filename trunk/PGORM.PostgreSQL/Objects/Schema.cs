using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM.PostgreSQL.Objects
{
    public class Schema<R,S,C> where R : Relation<C> where S : StoredFunction where C : Column, new()
    {
        public List<R> Tables;
        public List<R> Views;
        public List<R> CompositeTypes;
        public List<R> Enums;
        public List<S> StoredFunctions;
        
        public Schema()
        {
            Tables = new List<R>();
            Views = new List<R>();
            CompositeTypes = new List<R>();
            Enums = new List<R>();
            StoredFunctions = new List<S>();
        }
    }
}
