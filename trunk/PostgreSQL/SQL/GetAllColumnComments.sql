select 
	ns.nspname as table_schema,			
	t.relname as table_name,
	c.attname as column_name,
	d.description
from 
	pg_attribute c
	inner join pg_class t on c.attrelid=t.oid
	inner join pg_namespace ns on ns.oid=t.relnamespace
	left join pg_description d on d.objoid=t.oid and d.objsubid=c.attnum
where
	ns.nspname not in ('information_schema','pg_catalog','pg_toast') and
	d.description is not null
