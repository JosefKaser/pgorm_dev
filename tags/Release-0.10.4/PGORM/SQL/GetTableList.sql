/*-------------------------------------------------------------------------
 * GetTableList.sql
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
		*
from
		information_schema.tables
where
		table_schema='public' 
order by
		table_type asc,table_name asc;