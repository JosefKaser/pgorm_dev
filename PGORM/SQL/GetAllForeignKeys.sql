/*-------------------------------------------------------------------------
 * GetAllForeignKeys.sql
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
	information_schema.table_constraints tc
where	
	tc.constraint_type = 'FOREIGN KEY'