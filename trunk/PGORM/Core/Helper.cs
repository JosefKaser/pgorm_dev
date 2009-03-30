using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM
{
    public class Helper
    {
        public static string EscapeString(string data)
        {
            return data.Replace("\\","\\\\").Replace("\"", "\\\"");
        }

        public static string RemovePrefix(string source, List<string> items)
        {
            foreach (string prefix in items)
                if (source.IndexOf(prefix) == 0)
                {
                    source= source.Replace(prefix, "");
                    return source;
                }
            return source;
        }
    }
}
