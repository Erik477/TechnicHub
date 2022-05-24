/*
drop database if exists technichub;
create database technichub collate utf8mb4_general_ci;
use technichub;


create table users(
	user_id int not null auto_increment,
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

select * from users;

create table pLanguage(
	Plang_id int not null auto_increment,
    Plang_name varchar(100),	
	constraint Plang_id_PK primary key(Plang_id)
);

insert into pLanguage values(null, "JAVA");
insert into pLanguage values(null, "Python");
insert into pLanguage values(null, "MySQL");
insert into pLanguage values(null, "JavaScript");
insert into pLanguage values(null, "CPlusPlus");
insert into pLanguage values(null, "Csharp");
insert into pLanguage values(null, "Rust");
insert into pLanguage values(null, "Kotlin");

select * from pLanguage;



create table zwTable(
user_id int not null,
Plang_id int not null,
CONSTRAINT zwTable_user foreign key (user_id) references users(user_id),
CONSTRAINT zwTable_PLang foreign key (Plang_id) references pLanguage(Plang_id),
CONSTRAINT zwTable_unique UNIQUE (Plang_id,user_id)
);


select * from zwTable;



create table posts(
	PostId int not null auto_increment,
	Message varchar(3000) not null,
	Post_date  date null,
	UserId varchar(256) not null,
	chatroomId int not null,
	constraint PostId_pk primary key(PostId)
);

select * from posts;



create table chatroom(
	Id int not null auto_increment,
	Name varchar(100) not null,
	Description varchar(255),
    constraint Id_pk primary key(Id)	
);


insert into chatroom values(null, "C# Projekt","Austauschort für C# Projekte");
select * from chatroom;

insert into posts values(null, "Cooles Projekt","2004-01-10",1,1);
select * from posts;


insert into chatroom values(2, "LOL Test","Testforum");
insert into chatroom values(2, "LOL Test","Testforum");

   
   drop table if exists posts;
   
create table posts(
	PostId int not null auto_increment,
	Message varchar(3000) not null,
	Post_date  date null,
	UserId int not null,
	chatroomID int not null,
	constraint PostId_pk primary key(PostId)
);

insert into posts values(null, "Cooles Projekt","2004-01-10",1,1);
*/
 use technichub;  

insert into zwtable values(1,2);
insert into zwtable values(2,3);
insert into zwtable values(3,4);
insert into zwtable values(4,5);
insert into zwtable values(5,3);

select * from zwtable;

/*
SELECT planguage.Plang_id, zwtable.user_id, planguage.Plang_name  FROM planguage INNER JOIN zwtable ON zwtable.user_id = 5;

SELECT plang_name from planguage, users, zwtable where users.user_id = zwtable.user_id and zwtable.plang_id = planguage.plang_id and users.user_id = 5;*/
