select
	c.relname as sequence_name,
	r.relname as table_name,
	ns.nspname as table_schema,
	a.attname as column_name
from
	pg_depend dp
	inner join pg_class c on dp.objid=c.oid
	inner join pg_class r on dp.refobjid=r.oid
	inner join pg_attribute a on dp.refobjsubid=a.attnum and a.attrelid=r.oid
	inner join pg_namespace ns on r.relnamespace=ns.oid
where
	c.relkind='S'
