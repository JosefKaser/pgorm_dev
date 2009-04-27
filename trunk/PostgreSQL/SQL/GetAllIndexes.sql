select * from
(
select 
	ire.relname as constraint_name,
	ns.nspname as table_namespace,
	re.relname as table_name,
	string_to_array(i.indkey::varchar,' ')::integer[] as constraint_keys,
	case when i.indisunique then 'u'::varchar else 'i'::varchar end as constraint_type,
	null::varchar as foreign_table_namespace,
	null::varchar as foreign_table_name
from 
	pg_index i
	inner join pg_class re on re.oid=i.indrelid
	inner join pg_class ire on ire.oid=i.indexrelid
	inner join pg_namespace ns on re.relnamespace=ns.oid
where 
	i.indisprimary=false
	and ns.nspname not in ('pg_catalog','information_schema')

union

select
	cn.conname as constraint_name,
	ns.nspname as table_namespace,
	re.relname as table_name,	
	cn.conkey::integer[] as  constraint_keys,
	cn.contype::varchar as constraint_type,
	fns.nspname as foreign_table_namespace,
	fre.relname as foreign_table_name
from
	pg_constraint cn
	inner join pg_class re on cn.conrelid = re.oid
	inner join pg_namespace ns on re.relnamespace=ns.oid
	left join pg_class fre on cn.confrelid = fre.oid
	left join pg_namespace fns on fre.relnamespace=fns.oid
) 
a
where a.constraint_type <> 'c'
order by a.table_name