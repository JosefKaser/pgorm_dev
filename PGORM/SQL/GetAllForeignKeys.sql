select
	*
from
	information_schema.table_constraints tc
where	
	tc.constraint_type = 'FOREIGN KEY'