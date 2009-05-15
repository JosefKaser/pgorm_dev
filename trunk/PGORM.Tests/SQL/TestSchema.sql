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
	field2 int default 1	
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


create type type1 as
(
	field1 varchar,
	field2 varchar
);

create table use_type
(
	field1 type1
);

create type t_zipcode as
(
	part1 varchar,
	part2 varchar
);

create type t_address as
(
	street varchar,
	zipcode t_zipcode,
	city varchar
);


create table inner_type_test
(
	id serial not null primary key,
	address t_address
);