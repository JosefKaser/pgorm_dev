/*-------------------------------------------------------------------------
 * GetAllForeignKeyInfo.sql
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
	cn.conname as constraint_name,
	f.relname as local_table_name,
	p.relname as foreign_table_name,
	cn.conkey as local_keys,
	cn.confkey as foreign_keys,
	cn.contype
from
	pg_constraint cn
	inner join pg_class f on f.oid = cn.conrelid
	inner join pg_class p on p.oid = cn.confrelid