select
		p.oid,
		p.proname as function_name,
		n.nspname as schema_name,
		case when (en.typname is not null and en.nspname is not null) then regexp_replace(format_type(p.prorettype,null),en.nspname || '.','') else format_type(p.prorettype,null) end as return_type,
		case when (en.typname is not null and en.nspname is not null) then en.nspname else n.nspname end as return_type_schema,
		p.proargnames as arg_names,
		p.pronargs::integer as num_args,
		p.proretset as returns_set,
		pg_get_expr(p.proargdefaults,'pg_proc'::regclass) as argument_defaults,
		case when p.proargtypes <> '' then regexp_split_to_array(oidvectortypes(p.proargtypes),E',\\s+') else null end as arg_types,
		regexp_split_to_array(oidvectortypes( (regexp_replace(p.proallargtypes::text,E'\\D',' ','g')::oidvector) ),E',\\s+') as all_arg_types,
		p.proargmodes as arg_modes,
		case when y.typtype = 'e' then 'ENUM'
		else
		    case when r.relkind = 'r' and y.typtype='c' then 'TABLE'
		    else
		        case when r.relkind = 'v' and y.typtype='c' then 'VIEW'
		        else
		            case when r.relkind = 'c' and y.typtype='c' then 'UDT'
		            else 
		                case when y.typtype='b' then 'BASE'
		                else
		                    case when format_type(p.prorettype,null) = 'void' then 'VOID'
		                    else
		                        case when r.relkind = 'e' then 'ENUM'
		                        else
		                             case when format_type(p.prorettype,null)='record' or format_type(p.prorettype,null)='anyelement' then 'RECORD' end
		                        end
		                    end
		                end
		        end
		    end
		end
		end 
		as return_type_type
from
		pg_proc p
		inner join pg_namespace n on p.pronamespace = n.oid
		left join pg_trigger t on t.tgfoid = p.oid
		inner join pg_type y on p.prorettype = y.oid
		left join pg_class r on p.prorettype=r.reltype
		inner join pg_language l on l.oid=p.prolang
		left join pg_namespace rn on rn.oid = r.relnamespace
		left join (select 
distinct ns.nspname,t.oid,t.*
from
pg_enum e
inner join pg_type t on t.oid=e.enumtypid
inner join pg_namespace ns on t.typnamespace=ns.oid) en on en.oid=p.prorettype
where
		n.nspname not in ('information_schema','pg_catalog')
		and t.tgname is null
		and format_type(p.prorettype,null) <> 'trigger'
		and l.lanname not in ('c','internal')
		
order
		by p.proname asc
