/*-------------------------------------------------------------------------
 * GetAllFunctions.sql
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
		p.proname,
		oidvectortypes(p.proargtypes) as arg_types,
		format_type(p.prorettype,null) as return_type,
		y.typtype as return_type_type,
		p.proargnames,
		p.pronargs::integer as num_args,
		p.proretset as returns_set
		
from
		pg_proc p
		inner join pg_namespace n on p.pronamespace = n.oid
		left join pg_trigger t on t.tgfoid = p.oid
		inner join pg_type y on p.prorettype = y.oid
where
		n.nspname='public'
		and t.tgname is null
		and format_type(p.prorettype,null) <> 'trigger'
		
order
		by p.proname asc