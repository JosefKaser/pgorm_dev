using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM
{
    public class pg_composite_type
    {
        public string type_name { get; set; }
        public string column_name { get; set; }
        public string db_type { get; set; }
        public int column_index { get; set; }
    }
}
