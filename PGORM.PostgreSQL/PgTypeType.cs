using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM.PostgreSQL
{
    public enum PgTypeType
    {
        BaseType,
        EnumType,
        CompositeType,
        Relation,
        View,
        Unknown
    }

    public class PgTypeTypeConverter
    {
        public static PgTypeType FromString(string t)
        {
            if (t == "-")
                return PgTypeType.Unknown;
            else if (t == "b")
                return PgTypeType.BaseType;
            else if (t == "r")
                return PgTypeType.Relation;
            else if (t == "c")
                return PgTypeType.CompositeType;
            else if (t == "e")
                return PgTypeType.EnumType;
            else if (t == "v")
                return PgTypeType.View;
            else
                throw new NotImplementedException("TypeType " + t + " is not implemented.");
        }
    }
}
