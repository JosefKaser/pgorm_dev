select
	a.*
from
	(
	
    SELECT 
	   CAST(current_database() AS varchar) AS table_catalog,
           CAST(nc.nspname AS varchar) AS table_schema,
           CAST(c.relname AS varchar) AS table_name,
           CAST(a.attname AS varchar) AS column_name,
           CAST(a.attnum AS integer) AS ordinal_position,
           CAST(pg_get_expr(ad.adbin, ad.adrelid) AS varchar) AS column_default,
           CAST(CASE WHEN a.attnotnull OR (t.typtype = 'd' AND t.typnotnull) THEN 'NO' ELSE 'YES' END
             AS varchar)
             AS is_nullable,
           CAST(
             CASE WHEN t.typtype = 'd' THEN
               CASE WHEN bt.typelem <> 0 AND bt.typlen = -1 THEN 'ARRAY'
                    WHEN nbt.nspname = 'pg_catalog' THEN format_type(t.typbasetype, null)
                    ELSE 'USER-DEFINED' END
             ELSE
               CASE WHEN t.typelem <> 0 AND t.typlen = -1 THEN 'ARRAY'
                    WHEN nt.nspname = 'pg_catalog' THEN format_type(a.atttypid, null)
                    ELSE 'USER-DEFINED' END
             END
             AS varchar)
             AS data_type,

           CAST(
             information_schema._pg_char_max_length(information_schema._pg_truetypid(a, t), information_schema._pg_truetypmod(a, t))
             AS integer)
             AS character_maximum_length,

           CAST(
             information_schema._pg_numeric_precision(information_schema._pg_truetypid(a, t), information_schema._pg_truetypmod(a, t))
             AS integer)
             AS numeric_precision,

           CAST(
             information_schema._pg_numeric_precision_radix(information_schema._pg_truetypid(a, t), information_schema._pg_truetypmod(a, t))
             AS integer)
             AS numeric_precision_radix,

           CAST(
             information_schema._pg_numeric_scale(information_schema._pg_truetypid(a, t), information_schema._pg_truetypmod(a, t))
             AS integer)
             AS numeric_scale,

           CAST(
             information_schema._pg_datetime_precision(information_schema._pg_truetypid(a, t), information_schema._pg_truetypmod(a, t))
             AS integer)
             AS datetime_precision,

           CAST(CASE WHEN t.typtype = 'd' THEN current_database() ELSE null END
             AS varchar) AS domain_catalog,
           CAST(CASE WHEN t.typtype = 'd' THEN nt.nspname ELSE null END
             AS varchar) AS domain_schema,
           CAST(CASE WHEN t.typtype = 'd' THEN t.typname ELSE null END
             AS varchar) AS domain_name,

           CAST(current_database() AS varchar) AS udt_catalog,
           CAST(coalesce(nbt.nspname, nt.nspname) AS varchar) AS udt_schema,
           CAST(coalesce(bt.typname, t.typname) AS varchar) AS udt_name,
           CAST(coalesce(bt.oid, t.oid) AS oid) AS udt_name_oid

           --CAST(null AS varchar) AS scope_catalog,
           --CAST(null AS varchar) AS scope_schema,
           --CAST(null AS varchar) AS scope_name,

           --CAST(null AS integer) AS maximum_cardinality,
           --CAST(a.attnum AS varchar) AS dtd_identifier,
           --CAST('NO' AS varchar) AS is_self_referencing,

           --CAST('NO' AS varchar) AS is_identity,
           --CAST(null AS varchar) AS identity_generation,
           --CAST(null AS varchar) AS identity_start,
           --CAST(null AS varchar) AS identity_increment,
           --CAST(null AS varchar) AS identity_maximum,
           --CAST(null AS varchar) AS identity_minimum,
           --CAST(null AS varchar) AS identity_cycle,

           --CAST('NEVER' AS varchar) AS is_generated,
           --CAST(null AS varchar) AS generation_expression,

           --CAST(CASE WHEN c.relkind = 'r'
           --     THEN 'YES' ELSE 'NO' END AS varchar) AS is_updatable
                
    FROM (pg_attribute a LEFT JOIN pg_attrdef ad ON attrelid = adrelid AND attnum = adnum),
         pg_class c, pg_namespace nc,
         (pg_type t JOIN pg_namespace nt ON (t.typnamespace = nt.oid))
           LEFT JOIN (pg_type bt JOIN pg_namespace nbt ON (bt.typnamespace = nbt.oid))
           ON (t.typtype = 'd' AND t.typbasetype = bt.oid)

    WHERE a.attrelid = c.oid
          AND a.atttypid = t.oid
          AND nc.oid = c.relnamespace
          AND (NOT pg_is_other_temp_schema(nc.oid))

          AND a.attnum > 0 AND NOT a.attisdropped AND c.relkind in ('r', 'v','c')

          AND (pg_has_role(c.relowner, 'USAGE')
               OR has_table_privilege(c.oid, 'SELECT')
               OR has_table_privilege(c.oid, 'INSERT')
               OR has_table_privilege(c.oid, 'UPDATE')
               OR has_table_privilege(c.oid, 'REFERENCES') )
	) a
where
	table_schema not in ('information_schema','pg_catalog')
	
union 

select
	  CAST(current_database() AS varchar) AS table_catalog,
          CAST(ns.nspname AS varchar) AS table_schema,
          t.typname::varchar as table_name,
          e.enumlabel::varchar as column_name,
          0::integer as ordinal_position,
          null::varchar as column_default,
          'NO' AS is_nullable,
          format_type(1043, null) as data_type,
          CAST(null AS integer) AS character_maximum_length,
          CAST(null AS integer) AS numeric_precision,
          CAST(null AS integer) AS numeric_precision_radix,
          CAST(null AS integer) AS numeric_scale,
          CAST(null  AS integer) AS datetime_precision,
          CAST(null  AS varchar) AS domain_catalog,
          CAST(null  AS varchar) AS domain_schema,
          CAST(null  AS varchar) AS domain_name,
          CAST(null AS varchar) AS udt_catalog,
          CAST('public' AS varchar) AS udt_schema,
          CAST('varchar' AS varchar) AS udt_name,
          CAST(t.oid AS oid) AS udt_name_oid
          --CAST(null AS varchar) AS scope_catalog,
          --CAST(null AS varchar) AS scope_schema,
         -- CAST(null AS varchar) AS scope_name,
          --CAST(null AS integer) AS maximum_cardinality,
          --CAST(null AS varchar) AS dtd_identifier,
          --CAST('NO' AS varchar) AS is_self_referencing,
          --CAST('NO' AS varchar) AS is_identity,
          --CAST(null AS varchar) AS identity_generation,
          --CAST(null AS varchar) AS identity_start,
          --CAST(null AS varchar) AS identity_increment,
          --CAST(null AS varchar) AS identity_maximum,
          --CAST(null AS varchar) AS identity_minimum,
          --CAST(null AS varchar) AS identity_cycle,
          ----CAST('NEVER' AS varchar) AS is_generated,
          --CAST(null AS varchar) AS generation_expression,
          --CAST('NO' AS varchar) AS is_updatable
from
	pg_type t
	inner join pg_namespace ns on t.typnamespace=ns.oid
	inner join pg_enum e on e.enumtypid=t.oid
where
	t.typtype='e'	