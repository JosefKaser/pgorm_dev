/*-------------------------------------------------------------------------
 * GetAllIndexes.sql
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
	i.indkey as keys,
	t.relname as table_name,
	i2.relname as index_name,
	i.indisprimary as is_primary,
	i.indisunique as is_unique,
	t.oid as table_oid
from
	pg_index i
	inner join pg_class t  on i.indrelid = t.oid
	inner join pg_class i2 on i.indexrelid = i2.oid
where
	i.indisprimary = false;