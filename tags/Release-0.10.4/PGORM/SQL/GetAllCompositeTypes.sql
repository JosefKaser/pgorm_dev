/*-------------------------------------------------------------------------
 * GetAllCompositeTypes.sql
 *
 * This file is part of the PGORM project.
 * http://pgorm.googlecode.com/
 *
 * Copyright (c) 2002-2009, TrueSoftware B.V.
 *
 * IDENTIFICATION
 * 
 *  $Id$
 * 	$HeadURL$
 * 	
 *-------------------------------------------------------------------------
 */
select
		t.typname::varchar as type_name,
		a.attname::varchar as column_name,
		format_type(a.atttypid,null) as db_type, 
		a.attnum::integer as column_index
		
from
		pg_type t
		inner join pg_attribute a on t.typrelid = a.attrelid
		inner join pg_class c on c.oid = t.typrelid
where
		t.typtype = 'c' and
		c.relkind = 'c'
order by 
		t.typname,a.attnum