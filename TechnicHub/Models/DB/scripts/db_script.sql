drop database if exists technic_hub;
create database technic_hub collate utf8mb4_general_ci;
use technic_hub;

create table users(
	user_id int unsigned not null auto_increment,
    username varchar(100) not null,
    password varchar(300) not null,
    birthdate date null,
    email varchar(150) not null,
    gender int null,
    
    constraint user_id_PK primary key(user_id)
);

insert into users values(null, "erik", sha2("1234", 512), "2014-12-26", "e.x@gmail.com", 0);