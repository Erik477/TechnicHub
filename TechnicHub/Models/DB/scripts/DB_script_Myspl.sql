/*
drop database if exists technichub;
create database technichub collate utf8mb4_general_ci;*/
use technichub;

/*
create table users(
	user_id int unsigned not null auto_increment,
	username varchar(100) not null,
	password varchar(300) not null,
	birthdate date null,
	email varchar(150) not null,
	gender int null,

	constraint user_id_PK primary key(user_id)
);

select * from users where username = 'erik' and user_id = 1;

insert into users values(null, "erik", sha2("Hallo123!", 512), "2004-12-26", "e.x@gmail.com", 0);

insert into users values(null, "clemens", sha2("Hallo123!", 512), "2004-03-01", "c.x@gmail.com", 0);
insert into users values(null, "julia", sha2("djkagvsdöogh!", 512), "2004-01-10", "j.x@gmail.com", 1);

insert into users values(null, "Werner", sha2("123456789", 512), "2004-01-10", "w.x@gmail.com", 1); 

select * from users;*/

create table pLanguage(
	JAVA varchar(100) not null,
    Python 
	
	constraint user_id_PK primary key(user_id)
);