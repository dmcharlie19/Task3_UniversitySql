use University

create table StudyGroup(
	Id int identity(1,1) constraint PK_StudyGroup primary key,
	Title nvarchar(100)
)

insert into StudyGroup
	(Title)
values
	('IVT-11'),
	('PS-11'),
	('BI-11')

select * from StudyGroup

create table Student(
	Id int identity(1,1) constraint PK_Student primary key,
	FirstName nvarchar(100),
	LastName nvarchar(100)
)

insert into Student
values
	('Егор', 'Смирнов'),
	('Иван', 'Иванов'),
	('Сидр', 'Сидоров'),
	('Кузнец', 'Кузнецов')

select * from Student

create table StudentsAndGroups(
	Id int identity(1,1) constraint PK_StudentsAndGroups primary key,
	StudentId int constraint FK_Student references Student(Id),
	StudyGroupId int constraint FK_StudyGroup references StudyGroup(Id)
)

insert into StudentsAndGroups
values
	('1', '1'),
	('2', '2'),
	('3', '3'),
	('6', '1')

select * from StudentsAndGroups

--Получить всех студентов распеределнныых по группам
select [FirstName], [LastName], [Title] from StudentsAndGroups as studentsAndGroups
join Student as s on s.Id = studentsAndGroups.StudentId
join StudyGroup as g on g.Id = studentsAndGroups.StudyGroupId

--Получить всех студентов по группам id группы
select count(*) from StudentsAndGroups as studentsAndGroups
join Student as s on s.Id = studentsAndGroups.StudentId
where studentsAndGroups.StudyGroupId = 1
