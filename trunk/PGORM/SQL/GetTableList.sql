select 
		*
from
		information_schema.tables
where
		table_schema='public' 
order by
		table_type asc,table_name asc;		