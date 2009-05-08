select
	distinct 
	view_schema,
	view_name,
	table_schema,
	table_name
from
	information_schema.view_column_usage
where
	view_schema not in ('information_schema','pg_catalog');