using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM
{
    public class pg_view_column_usage
    {
        public string view_name { get; set; }
        public string table_name { get; set; }
        public string column_name { get; set; }
    }
}
