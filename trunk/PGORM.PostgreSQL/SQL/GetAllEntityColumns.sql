
select
		t.constraint_type,c.*
from
		information_schema.constraint_column_usage c
		inner join information_schema.table_constraints t on c.constraint_name = t.constraint_name