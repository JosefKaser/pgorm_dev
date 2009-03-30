select
		*
from
		information_schema.constraint_column_usage c
		inner join information_schema.table_constraints t on c.constraint_name = t.constraint_name
where
		t.constraint_type='PRIMARY KEY';