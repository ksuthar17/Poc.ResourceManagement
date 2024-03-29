USE [master]
GO
/****** Object:  Database [Poc_Client]    Script Date: 1/17/2024 8:49:46 PM ******/
CREATE DATABASE [Poc_Client]
 go
USE [Poc_Client]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Department] ON 

INSERT [dbo].[Department] ([Id], [Name]) VALUES (1, N'HR')
INSERT [dbo].[Department] ([Id], [Name]) VALUES (2, N'IT')
INSERT [dbo].[Department] ([Id], [Name]) VALUES (3, N'Sales')
INSERT [dbo].[Department] ([Id], [Name]) VALUES (4, N'Finance')
SET IDENTITY_INSERT [dbo].[Department] OFF
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([Id])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Department]
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteDepartment]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_DeleteDepartment]
 @Id int
AS
BEGIN
	
	SET NOCOUNT ON;

 DELETE Department
  WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteEmployee]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_DeleteEmployee]
 @Id int
AS
BEGIN
	
	SET NOCOUNT ON;

 DELETE Employee
  WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDepartmentById]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_GetDepartmentById]
 @Id int
AS
BEGIN
	
	SET NOCOUNT ON;

 SELECT * FROM Department
  WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDepartments]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetDepartments]

AS
BEGIN
	
	SET NOCOUNT ON;

   select Id ,Name from Department
   order by Name 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetEmployeeById]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetEmployeeById]
 @Id int
AS
BEGIN
	
	SET NOCOUNT ON;

  select e.Id ,e.[Name] , DateOfBirth ,d.Name as DepartmentName ,e.DepartmentId ,Gender from Employee e inner join Department d on e.DepartmentId = d.Id
  WHERE e.Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetEmployees]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetEmployees]

@SORT_COLUMN NVARCHAR(50) = 'NAME',
@SORT_COLUMN_DIRECTION NVARCHAR(50) = 'ASC',
@SEARCH_TEXT NVARCHAR(100) = NULL,
@PAGESIZE INT = 10,
@SKIP INT = 0


AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @SearchTextFilterQuery nvarchar(max) = '';

	DECLARE @Sql nvarchar(max) = '';
	DECLARE @SqlResult nvarchar(max) = '';
	DECLARE @SqlCount nvarchar(max) = '';
	
	
IF(LEN(ISNULL(@SEARCH_TEXT, '')) > 1)
BEGIN
	SET @SearchTextFilterQuery = ' AND (e.[Name] LIKE ''%' + @SEARCH_TEXT + '%''';
	SET @SearchTextFilterQuery = @SearchTextFilterQuery+' OR d.[Name] LIKE ''%' + @SEARCH_TEXT + '%''';
	SET @SearchTextFilterQuery = @SearchTextFilterQuery+' OR [DateOfBirth] LIKE ''%' + @SEARCH_TEXT + '%''';
	SET @SearchTextFilterQuery = @SearchTextFilterQuery+')' ;

END
	PRINT @SearchTextFilterQuery

	SET @Sql = 'select e.Id ,e.[Name] , DateOfBirth ,d.Name as DepartmentName ,e.DepartmentId ,Gender  from Employee e inner join Department d on e.DepartmentId = d.Id
				where e.Id is not null
				 ' 
				+ @SearchTextFilterQuery;

				PRINT @Sql
				
  SET @SqlResult = @Sql +' ORDER BY ['+@SORT_COLUMN+'] ' + @SORT_COLUMN_DIRECTION + ' 
				OFFSET ' +CAST(@SKIP AS VARCHAR(10))+ ' ROWS FETCH NEXT ' +CAST(@PAGESIZE AS varchar(10))+ ' ROWS ONLY';


  EXEC sp_executeSql @SqlResult

  SET @SqlCount = 'SELECT COUNT(*) AS TOTALRECORDS FROM ('+@Sql+') P';
  EXEC sp_executeSql @SqlCount

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertDepartment]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_InsertDepartment]
 @Name nvarchar(100)
AS
BEGIN
	
	SET NOCOUNT ON;

  INSERT INTO Department(Name)
  VALUES (@Name)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertEmployee]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_InsertEmployee]
 @Name nvarchar(100),
 @DateOfBirth date,
 @DepartmentId int,
 @Gender nvarchar(10)
AS
BEGIN
	
	SET NOCOUNT ON;

  INSERT INTO Employee(Name , DateOfBirth , DepartmentId ,Gender)
  VALUES (@Name ,@DateOfBirth , @DepartmentId ,@Gender)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateDepartment]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_UpdateDepartment]
 @Name nvarchar(100),
 @Id int
AS
BEGIN
	
	SET NOCOUNT ON;

  UPDATE Department
  SET [Name] = @Name
  WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateEmployee]    Script Date: 1/17/2024 8:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_UpdateEmployee]
 @Name nvarchar(100),
 @DateOfBirth date,
 @DepartmentId int,
 @Id int,
 @Gender nvarchar(10)
AS
BEGIN
	
	SET NOCOUNT ON;

  UPDATE Employee 
  SET NAME = @Name , DateOfBirth = @DateOfBirth , DepartmentId = @DepartmentId , Gender = @Gender
  WHERE Id = @Id
END
GO
USE [master]
GO
ALTER DATABASE [Poc_Client] SET  READ_WRITE 
GO
