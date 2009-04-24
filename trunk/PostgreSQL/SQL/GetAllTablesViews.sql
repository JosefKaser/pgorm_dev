select
	*
from
(	
    SELECT CAST(current_database() AS varchar) AS table_catalog,
           CAST(nc.nspname AS varchar) AS table_schema,
           CAST(c.relname AS varchar) AS table_name,

           CAST(
             CASE WHEN nc.oid = pg_my_temp_schema() THEN 'LOCAL TEMPORARY'
                  WHEN c.relkind = 'r' THEN 'BASE TABLE'
                  WHEN c.relkind = 'v' THEN 'VIEW'
                  WHEN c.relkind = 'c' THEN 'USER-DEFINED'
                  ELSE null END
             AS varchar) AS table_type,

           CAST(CASE WHEN c.relkind = 'r'
                THEN 'YES' ELSE 'NO' END AS varchar) AS is_insertable_into

    FROM pg_namespace nc, pg_class c

    WHERE c.relnamespace = nc.oid
          AND c.relkind IN ('r', 'v','c','e')
          --AND (NOT pg_is_other_temp_schema(nc.oid))
          AND (pg_has_role(c.relowner, 'USAGE')
               OR has_table_privilege(c.oid, 'SELECT')
               OR has_table_privilege(c.oid, 'INSERT')
               OR has_table_privilege(c.oid, 'UPDATE')
               OR has_table_privilege(c.oid, 'DELETE')
               OR has_table_privilege(c.oid, 'REFERENCES')
               OR has_table_privilege(c.oid, 'TRIGGER') )
order by 
	c.oid asc
               
) a   
	
where table_schema not in('information_schema','pg_catalog')

union

    select 
	CAST(current_database() AS varchar) AS table_catalog,
	CAST(nc.nspname AS varchar) AS table_schema,
	t.typname AS table_name,
	'ENUM' as table_type,
	'NO' AS is_insertable_into
    from 
	pg_type t 
	inner join pg_namespace nc on t.typnamespace=nc.oid
    where
        t.typtype='e'
        

