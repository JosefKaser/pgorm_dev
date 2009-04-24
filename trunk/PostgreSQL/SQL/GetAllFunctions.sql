select
		p.oid as proc_oid,
		p.proname,
		regexp_split_to_array(oidvectortypes(p.proargtypes),E',\\s+') as arg_types,
		format_type(p.prorettype,null) as return_type,
		y.typtype as return_type_type,
		p.proargnames,
		p.pronargs::integer as num_args,
		p.proretset as returns_set,
		case when r.relkind = 'r' and y.typtype='c' then true else false end as is_table,
		case when r.relkind = 'v' and y.typtype='c' then true else false end as is_view,
		case when r.relkind = 'c' and y.typtype='c' then true else false end as is_composite,
		case when format_type(p.prorettype,null) = 'void' then true else false end as is_void,
		case when y.typtype = 'e' then true else false end as is_enum
				
from
		pg_proc p
		inner join pg_namespace n on p.pronamespace = n.oid
		left join pg_trigger t on t.tgfoid = p.oid
		inner join pg_type y on p.prorettype = y.oid
		left join pg_class r on p.prorettype=r.reltype
		inner join pg_language l on l.oid=p.prolang
where
		n.nspname not in ('information_schema','pg_catalog')
		and t.tgname is null
		and format_type(p.prorettype,null) <> 'trigger'
		and l.lanname not in ('c','internal')
		
order
		by p.proname asc
