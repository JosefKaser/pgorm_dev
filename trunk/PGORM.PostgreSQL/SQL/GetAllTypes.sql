select
	ns.nspname as type_namespace,
	t.oid as type_oid, 
	t.typname as type_short_name,
	format_type(t.oid,null) as type_long_name,
	(case when c.relkind is null then t.typtype else c.relkind end)::varchar as type_type,
	t.typdelim::varchar as delimiter,
	format_type(t.typelem,null) as base_type,
	t.typelem as base_type_oid,
	case when btns.nspname is null then ''::name else btns.nspname end as base_type_schema
from
	pg_type t
	left join pg_class c on t.oid=c.reltype
	inner join pg_namespace ns on t.typnamespace=ns.oid
	left join pg_type bt on t.typelem = bt.oid
	left join pg_namespace btns on bt.typnamespace=btns.oid
where
	ns.nspname not in ('information_schema','pg_toast');--,'pg_catalog');