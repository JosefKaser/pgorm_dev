using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGORM.PostgreSQL.Objects;
using PGORM.CodeBuilder;


namespace PGORM.CodeBuilder.TemplateObjects
{
    public class TemplateFunction : Function<TemplateRelation, TemplateColumn>
    {
        public TemplateReturnType TemplateReturnType { get; set; }
        public ConverterProxy Converter { get; set; }
        public List<TemplateColumn> TemplateArguments { get; set; }

        public void Prepare(ProjectBuilder p_Project)
        {
            // remove the nullable type from arguments
            if (TemplateArguments != null)
            {
                foreach (TemplateColumn col in TemplateArguments)
                {
                    col.Prepare(p_Project);
                }
            }
        }

        public bool HasConverter
        {
            get
            {
                return Converter != null;
            }
        }
    }
}
