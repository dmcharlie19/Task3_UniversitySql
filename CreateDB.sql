--Create table "Group"
IF NOT EXISTS (SELECT TOP 1 1 FROM sys.tables WHERE [name] = 'Group')
BEGIN
	CREATE TABLE [Group](
		[Id] INT IDENTITY(1,1) CONSTRAINT PK_Group PRIMARY KEY,
		[Title] NVARCHAR(100)
	)
END

--Insert groups into "Group"
INSERT INTO [Group]
	(Title)
VALUES ('IVT-11')
	,('PS-11')
	,('BI-11')

--Select all groups
SELECT * FROM [Group]

--Create table "Group"
IF NOT EXISTS (SELECT TOP 1 1 FROM sys.tables WHERE [name] = 'Student')
BEGIN
	CREATE TABLE Student(
		Id INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_Student PRIMARY KEY,
		FirstName NVARCHAR(100) NOT NULL,
		LastName NVARCHAR(100) NOT NULL
	)
END

--Insert students into "Student"
INSERT INTO Student
	(FirstName, LastName)
VALUES
	(N'Alex', N'Alexov'),
	(N'Den', N'Denov'),
	(N'Oleg', N'Olegov'),
	(N'Sergey', N'Sergeev')

--Select all students
SELECT * FROM Student

--Create table "StudentGroup"
IF NOT EXISTS (SELECT TOP 1 1 FROM sys.tables WHERE [name] = 'StudentGroup')
BEGIN
	CREATE TABLE StudentGroup(
		StudentId INT NOT NULL,
		GroupId INT NOT NULL,
		CONSTRAINT PK_StudentGroup PRIMARY KEY(StudentId, GroupId),
		CONSTRAINT FK_StudentGroup_Student FOREIGN KEY(StudentId)
		REFERENCES Student(Id)
		ON DELETE CASCADE,
		CONSTRAINT FK_StudentGroup_Group FOREIGN KEY(GroupId)
		REFERENCES [Group](Id)
		ON DELETE CASCADE
	)
END