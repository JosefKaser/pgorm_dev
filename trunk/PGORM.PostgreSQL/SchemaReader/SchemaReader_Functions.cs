using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using Npgsql;
using PGORM.PostgreSQL.Catalog;
using PGORM.PostgreSQL.Objects;

namespace PGORM.PostgreSQL
{
    public partial class SchemaReader<R, S, C>
        where R : Relation<C>, new()
        where S : Function, new()
        where C : Column, new()
    {
    }
}