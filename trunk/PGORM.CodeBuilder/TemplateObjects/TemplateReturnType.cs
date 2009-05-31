using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM.CodeBuilder.TemplateObjects
{
    public class TemplateReturnType
    {
        string type;
        public TemplateReturnType(string clr_name, bool is_list)
        {
            type = clr_name;
            if (is_list)
                type = string.Format("List<{0}>", clr_name);
        }

        public override string ToString()
        {
            return type;
        }
    }

    public class TemplateVoidReturnType : TemplateReturnType
    {
        public TemplateVoidReturnType()
            : base("void", false)
        {
        }
    }
}
