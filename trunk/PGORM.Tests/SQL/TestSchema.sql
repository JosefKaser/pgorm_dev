create table test1
(
	id serial not null primary key,
	field1 varchar
);

create table test2
(
	id serial not null primary key,
	field1 varchar	
);

insert into test2(field1) values ('TEST OK');

create table test3
(
	id serial not null primary key,
	field1 varchar,
	field2 int default 1,
	field_z	int
);
create index test3_field2_index on test3 (field2);

insert into test3(field1,field2) values ('TEST1',1);
insert into test3(field1,field2) values ('TEST2',2);
insert into test3(field1,field2) values ('TEST3',2);

create type "RDBMS" as enum
(
	'POSTGRES',
	'ORACLE',
	'MSSQL'
);

create table enum_table
(
	id serial not null primary key,
	dbtype "RDBMS"
);

create type t_zipcode as
(
	range varchar(4),
	locator varchar(2)
);

create type t_address as
(
	street varchar,
	zipcode t_zipcode,
	city varchar,
	country varchar
);

create type t_building_type as enum
(
	'block',
	'aparment'
);

create table building
(
	id serial not null primary key,
	address t_address,
	type t_building_type,
	description varchar
);

create view view_building as select * from building;

create table building_complex
(
	id serial not null primary key,
	complex_building_view view_building,
	complex_building_table building
);

create table udt_single_test
(
	id serial not null primary key,
	address t_address
);

create table udt_array_test
(
	id serial not null primary key,
	address t_address[]
);

create table udt_multi_array_test
(
	id serial not null primary key,
	multi_zip_2 t_zipcode[][],
	multi_zip_3 t_zipcode[][][]
);

create table complex2
(
	id serial not null primary key,
	zipcode_single t_zipcode,
	zipcode_array t_zipcode[],
	zipcode_attay_multi t_zipcode[][],	
	
	address_single t_address,
	address_array t_address[],
	address_array_multy t_address[][]
);