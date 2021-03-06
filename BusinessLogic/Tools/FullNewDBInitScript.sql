USE [master]
GO
/****** Object:  Database [newdb]    Script Date: 7/30/2017 8:43:51 PM ******/
CREATE DATABASE [newdb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'newdb', FILENAME = N'C:\Users\korol\newdb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'newdb_log', FILENAME = N'C:\Users\korol\newdb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [newdb] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [newdb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [newdb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [newdb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [newdb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [newdb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [newdb] SET ARITHABORT OFF 
GO
ALTER DATABASE [newdb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [newdb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [newdb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [newdb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [newdb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [newdb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [newdb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [newdb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [newdb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [newdb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [newdb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [newdb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [newdb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [newdb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [newdb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [newdb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [newdb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [newdb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [newdb] SET  MULTI_USER 
GO
ALTER DATABASE [newdb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [newdb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [newdb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [newdb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [newdb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [newdb] SET QUERY_STORE = OFF
GO
USE [newdb]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [newdb]
GO
/****** Object:  Table [dbo].[CandidatePrevJobsContacts]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CandidatePrevJobsContacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Candidate] [int] NULL,
	[CompanyName] [nvarchar](max) NULL,
	[Position] [nvarchar](max) NULL,
	[Contacts] [int] NULL,
 CONSTRAINT [PK_CandidatesPrevJobsContacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CandidatePrimarySkill]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CandidatePrimarySkill](
	[Candidate] [int] NOT NULL,
	[TechSkill] [int] NOT NULL,
	[Level] [int] NULL,
 CONSTRAINT [PK_CandidatePrimarySkills] PRIMARY KEY CLUSTERED 
(
	[Candidate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Candidates]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Candidates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstNameRus] [nvarchar](max) NULL,
	[FirstNameEng] [nvarchar](max) NULL,
	[LastNameRus] [nvarchar](max) NULL,
	[LastNameEng] [nvarchar](max) NULL,
	[Picture] [nvarchar](max) NULL,
	[Contacts] [int] NULL,
	[City] [int] NULL,
	[DesiredSalary] [int] NULL,
	[Resume] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[EngLevel] [int] NULL,
	[PSExperience] [date] NULL,
	[HRM] [int] NOT NULL,
	[LastContactDate] [date] NULL,
	[LastModifier] [int] NULL,
	[Reminder] [date] NULL,
	[TechInterviewStatus] [bit] NULL,
	[GeneralInterviewStatus] [bit] NULL,
	[CustomerInterviewStatus] [bit] NULL,
	[CustomerInterviewDate] [date] NULL,
	[CustomerInterviewEndDate] [date] NULL,
 CONSTRAINT [PK_Candidates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CandidateSecondarySkills]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CandidateSecondarySkills](
	[Candidate] [int] NOT NULL,
	[TechSkill] [int] NOT NULL,
	[Level] [int] NULL,
 CONSTRAINT [PK_CandidateSecondarySkills] PRIMARY KEY CLUSTERED 
(
	[Candidate] ASC,
	[TechSkill] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CandidateStatuses]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CandidateStatuses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CandidateStatuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CandidatesVacancies]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CandidatesVacancies](
	[Candidate] [int] NOT NULL,
	[Vacancy] [int] NOT NULL,
 CONSTRAINT [PK_CandidatesVacancies] PRIMARY KEY CLUSTERED 
(
	[Candidate] ASC,
	[Vacancy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Skype] [nvarchar](max) NULL,
	[LinkedIn] [nvarchar](max) NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EngLevels]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EngLevels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_EngLevels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventIds]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventIds](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_EventIds] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[Id] [int] NOT NULL,
	[Event] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[User] [int] NOT NULL,
	[EventType] [int] NOT NULL,
	[Property] [nvarchar](max) NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
	[Candidate] [int] NULL,
	[Vacancy] [int] NULL,
	[GeneralInterview] [int] NULL,
	[TechInterview] [int] NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventTypes]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_EventTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneralInterviews]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralInterviews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Candidate] [int] NOT NULL,
	[City] [int] NULL,
	[Date] [date] NULL,
	[HRM] [int] NOT NULL,
	[Interviewer] [int] NULL,
	[Status] [int] NOT NULL,
	[EngLevel] [int] NULL,
	[Commentary] [nvarchar](max) NULL,
	[EndDate] [date] NULL,
 CONSTRAINT [PK_GeneralInterviews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneralSkills]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralSkills](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_GeneralSkills] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User] [int] NOT NULL,
	[Event] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TechInterviews]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TechInterviews](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Candidate] [int] NOT NULL,
	[City] [int] NULL,
	[Date] [date] NULL,
	[HRM] [int] NOT NULL,
	[Interviewer] [int] NULL,
	[Status] [int] NOT NULL,
	[TechSkill] [int] NOT NULL,
	[Mark] [int] NULL,
	[Commentary] [nvarchar](max) NULL,
	[EndDate] [date] NULL,
 CONSTRAINT [PK_TechInterviews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TechSkills]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TechSkills](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Picuture] [nvarchar](max) NULL,
 CONSTRAINT [PK_TechSkills] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Role] [int] NOT NULL,
	[Login] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Picture] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NOT NULL,
	[CalendarId] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vacancies]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vacancies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[RequestDate] [date] NULL,
	[StartDate] [date] NULL,
	[CloseDate] [date] NULL,
	[City] [int] NULL,
	[Status] [int] NOT NULL,
	[Link] [nvarchar](max) NULL,
	[EngLevel] [int] NULL,
	[Experience] [int] NULL,
	[VacancyName] [nvarchar](max) NULL,
	[HRM] [int] NOT NULL,
	[LastModifier] [int] NULL,
 CONSTRAINT [PK_Vacancies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VacancyPrimarySkill]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VacancyPrimarySkill](
	[Vacancy] [int] NOT NULL,
	[TechSkill] [int] NOT NULL,
	[Level] [int] NULL,
 CONSTRAINT [PK_VacancyPrimarySkill] PRIMARY KEY CLUSTERED 
(
	[Vacancy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VacancySecondarySkills]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VacancySecondarySkills](
	[Vacancy] [int] NOT NULL,
	[TechSkill] [int] NOT NULL,
	[Level] [int] NULL,
 CONSTRAINT [PK_VacancySecondarySkills] PRIMARY KEY CLUSTERED 
(
	[Vacancy] ASC,
	[TechSkill] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VacancyStatuses]    Script Date: 7/30/2017 8:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VacancyStatuses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_VacancyStatuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CandidatePrevJobsContacts]  WITH CHECK ADD  CONSTRAINT [FK_CandidatePrevJobsContacts_Candidates] FOREIGN KEY([Candidate])
REFERENCES [dbo].[Candidates] ([Id])
GO
ALTER TABLE [dbo].[CandidatePrevJobsContacts] CHECK CONSTRAINT [FK_CandidatePrevJobsContacts_Candidates]
GO
ALTER TABLE [dbo].[CandidatePrevJobsContacts]  WITH CHECK ADD  CONSTRAINT [FK_CandidatePrevJobsContacts_Contacts] FOREIGN KEY([Contacts])
REFERENCES [dbo].[Contacts] ([Id])
GO
ALTER TABLE [dbo].[CandidatePrevJobsContacts] CHECK CONSTRAINT [FK_CandidatePrevJobsContacts_Contacts]
GO
ALTER TABLE [dbo].[CandidatePrimarySkill]  WITH CHECK ADD  CONSTRAINT [FK_CandidatePrimarySkill_Candidates] FOREIGN KEY([Candidate])
REFERENCES [dbo].[Candidates] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CandidatePrimarySkill] CHECK CONSTRAINT [FK_CandidatePrimarySkill_Candidates]
GO
ALTER TABLE [dbo].[CandidatePrimarySkill]  WITH CHECK ADD  CONSTRAINT [FK_CandidatePrimarySkill_TechSkills] FOREIGN KEY([TechSkill])
REFERENCES [dbo].[TechSkills] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CandidatePrimarySkill] CHECK CONSTRAINT [FK_CandidatePrimarySkill_TechSkills]
GO
ALTER TABLE [dbo].[Candidates]  WITH CHECK ADD  CONSTRAINT [FK_Candidates_CandidateStatuses] FOREIGN KEY([Status])
REFERENCES [dbo].[CandidateStatuses] ([Id])
GO
ALTER TABLE [dbo].[Candidates] CHECK CONSTRAINT [FK_Candidates_CandidateStatuses]
GO
ALTER TABLE [dbo].[Candidates]  WITH CHECK ADD  CONSTRAINT [FK_Candidates_Cities] FOREIGN KEY([City])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Candidates] CHECK CONSTRAINT [FK_Candidates_Cities]
GO
ALTER TABLE [dbo].[Candidates]  WITH CHECK ADD  CONSTRAINT [FK_Candidates_Contacts] FOREIGN KEY([Contacts])
REFERENCES [dbo].[Contacts] ([Id])
GO
ALTER TABLE [dbo].[Candidates] CHECK CONSTRAINT [FK_Candidates_Contacts]
GO
ALTER TABLE [dbo].[Candidates]  WITH CHECK ADD  CONSTRAINT [FK_Candidates_EngLevels] FOREIGN KEY([EngLevel])
REFERENCES [dbo].[EngLevels] ([Id])
GO
ALTER TABLE [dbo].[Candidates] CHECK CONSTRAINT [FK_Candidates_EngLevels]
GO
ALTER TABLE [dbo].[Candidates]  WITH CHECK ADD  CONSTRAINT [FK_Candidates_Users] FOREIGN KEY([HRM])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Candidates] CHECK CONSTRAINT [FK_Candidates_Users]
GO
ALTER TABLE [dbo].[Candidates]  WITH CHECK ADD  CONSTRAINT [FK_Candidates_Users1] FOREIGN KEY([LastModifier])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Candidates] CHECK CONSTRAINT [FK_Candidates_Users1]
GO
ALTER TABLE [dbo].[CandidateSecondarySkills]  WITH CHECK ADD  CONSTRAINT [FK_CandidateSecondarySkills_Candidates] FOREIGN KEY([Candidate])
REFERENCES [dbo].[Candidates] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CandidateSecondarySkills] CHECK CONSTRAINT [FK_CandidateSecondarySkills_Candidates]
GO
ALTER TABLE [dbo].[CandidateSecondarySkills]  WITH CHECK ADD  CONSTRAINT [FK_CandidateSecondarySkills_TechSkills] FOREIGN KEY([TechSkill])
REFERENCES [dbo].[TechSkills] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CandidateSecondarySkills] CHECK CONSTRAINT [FK_CandidateSecondarySkills_TechSkills]
GO
ALTER TABLE [dbo].[CandidatesVacancies]  WITH CHECK ADD  CONSTRAINT [FK_CandidatesVacancies_Candidates] FOREIGN KEY([Candidate])
REFERENCES [dbo].[Candidates] ([Id])
GO
ALTER TABLE [dbo].[CandidatesVacancies] CHECK CONSTRAINT [FK_CandidatesVacancies_Candidates]
GO
ALTER TABLE [dbo].[CandidatesVacancies]  WITH CHECK ADD  CONSTRAINT [FK_CandidatesVacancies_Vacancies] FOREIGN KEY([Vacancy])
REFERENCES [dbo].[Vacancies] ([Id])
GO
ALTER TABLE [dbo].[CandidatesVacancies] CHECK CONSTRAINT [FK_CandidatesVacancies_Vacancies]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Candidates] FOREIGN KEY([Candidate])
REFERENCES [dbo].[Candidates] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Candidates]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_EventIds] FOREIGN KEY([Event])
REFERENCES [dbo].[EventIds] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_EventIds]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_EventTypes] FOREIGN KEY([EventType])
REFERENCES [dbo].[EventTypes] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_EventTypes]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_GeneralInterviews] FOREIGN KEY([GeneralInterview])
REFERENCES [dbo].[GeneralInterviews] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_GeneralInterviews]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_TechInterviews] FOREIGN KEY([TechInterview])
REFERENCES [dbo].[TechInterviews] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_TechInterviews]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Users] FOREIGN KEY([User])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Users]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Vacancies] FOREIGN KEY([Vacancy])
REFERENCES [dbo].[Vacancies] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Vacancies]
GO
ALTER TABLE [dbo].[GeneralInterviews]  WITH CHECK ADD  CONSTRAINT [FK_GeneralInterviews_Candidates] FOREIGN KEY([Candidate])
REFERENCES [dbo].[Candidates] ([Id])
GO
ALTER TABLE [dbo].[GeneralInterviews] CHECK CONSTRAINT [FK_GeneralInterviews_Candidates]
GO
ALTER TABLE [dbo].[GeneralInterviews]  WITH CHECK ADD  CONSTRAINT [FK_GeneralInterviews_Cities] FOREIGN KEY([City])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[GeneralInterviews] CHECK CONSTRAINT [FK_GeneralInterviews_Cities]
GO
ALTER TABLE [dbo].[GeneralInterviews]  WITH CHECK ADD  CONSTRAINT [FK_GeneralInterviews_Users] FOREIGN KEY([HRM])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralInterviews] CHECK CONSTRAINT [FK_GeneralInterviews_Users]
GO
ALTER TABLE [dbo].[GeneralInterviews]  WITH CHECK ADD  CONSTRAINT [FK_GeneralInterviews_Users1] FOREIGN KEY([Interviewer])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralInterviews] CHECK CONSTRAINT [FK_GeneralInterviews_Users1]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Events] FOREIGN KEY([Event])
REFERENCES [dbo].[Events] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Events]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Users] FOREIGN KEY([User])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Users]
GO
ALTER TABLE [dbo].[TechInterviews]  WITH CHECK ADD  CONSTRAINT [FK_TechInterviews_Candidates] FOREIGN KEY([Candidate])
REFERENCES [dbo].[Candidates] ([Id])
GO
ALTER TABLE [dbo].[TechInterviews] CHECK CONSTRAINT [FK_TechInterviews_Candidates]
GO
ALTER TABLE [dbo].[TechInterviews]  WITH CHECK ADD  CONSTRAINT [FK_TechInterviews_Cities] FOREIGN KEY([City])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[TechInterviews] CHECK CONSTRAINT [FK_TechInterviews_Cities]
GO
ALTER TABLE [dbo].[TechInterviews]  WITH CHECK ADD  CONSTRAINT [FK_TechInterviews_TechSkills] FOREIGN KEY([TechSkill])
REFERENCES [dbo].[TechSkills] ([Id])
GO
ALTER TABLE [dbo].[TechInterviews] CHECK CONSTRAINT [FK_TechInterviews_TechSkills]
GO
ALTER TABLE [dbo].[TechInterviews]  WITH CHECK ADD  CONSTRAINT [FK_TechInterviews_Users] FOREIGN KEY([HRM])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[TechInterviews] CHECK CONSTRAINT [FK_TechInterviews_Users]
GO
ALTER TABLE [dbo].[TechInterviews]  WITH CHECK ADD  CONSTRAINT [FK_TechInterviews_Users1] FOREIGN KEY([Interviewer])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[TechInterviews] CHECK CONSTRAINT [FK_TechInterviews_Users1]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoles] FOREIGN KEY([Role])
REFERENCES [dbo].[UserRoles] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRoles]
GO
ALTER TABLE [dbo].[Vacancies]  WITH CHECK ADD  CONSTRAINT [FK_Vacancies_Cities] FOREIGN KEY([City])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Vacancies] CHECK CONSTRAINT [FK_Vacancies_Cities]
GO
ALTER TABLE [dbo].[Vacancies]  WITH CHECK ADD  CONSTRAINT [FK_Vacancies_EngLevels] FOREIGN KEY([EngLevel])
REFERENCES [dbo].[EngLevels] ([Id])
GO
ALTER TABLE [dbo].[Vacancies] CHECK CONSTRAINT [FK_Vacancies_EngLevels]
GO
ALTER TABLE [dbo].[Vacancies]  WITH CHECK ADD  CONSTRAINT [FK_Vacancies_Users] FOREIGN KEY([HRM])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Vacancies] CHECK CONSTRAINT [FK_Vacancies_Users]
GO
ALTER TABLE [dbo].[Vacancies]  WITH CHECK ADD  CONSTRAINT [FK_Vacancies_Users1] FOREIGN KEY([LastModifier])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Vacancies] CHECK CONSTRAINT [FK_Vacancies_Users1]
GO
ALTER TABLE [dbo].[Vacancies]  WITH CHECK ADD  CONSTRAINT [FK_Vacancies_VacancyStatuses] FOREIGN KEY([Status])
REFERENCES [dbo].[VacancyStatuses] ([Id])
GO
ALTER TABLE [dbo].[Vacancies] CHECK CONSTRAINT [FK_Vacancies_VacancyStatuses]
GO
ALTER TABLE [dbo].[VacancyPrimarySkill]  WITH CHECK ADD  CONSTRAINT [FK_VacancyPrimarySkill_TechSkills] FOREIGN KEY([TechSkill])
REFERENCES [dbo].[TechSkills] ([Id])
GO
ALTER TABLE [dbo].[VacancyPrimarySkill] CHECK CONSTRAINT [FK_VacancyPrimarySkill_TechSkills]
GO
ALTER TABLE [dbo].[VacancyPrimarySkill]  WITH CHECK ADD  CONSTRAINT [FK_VacancyPrimarySkill_Vacancies] FOREIGN KEY([Vacancy])
REFERENCES [dbo].[Vacancies] ([Id])
GO
ALTER TABLE [dbo].[VacancyPrimarySkill] CHECK CONSTRAINT [FK_VacancyPrimarySkill_Vacancies]
GO
ALTER TABLE [dbo].[VacancySecondarySkills]  WITH CHECK ADD  CONSTRAINT [FK_VacancySecondarySkills_TechSkills] FOREIGN KEY([TechSkill])
REFERENCES [dbo].[TechSkills] ([Id])
GO
ALTER TABLE [dbo].[VacancySecondarySkills] CHECK CONSTRAINT [FK_VacancySecondarySkills_TechSkills]
GO
ALTER TABLE [dbo].[VacancySecondarySkills]  WITH CHECK ADD  CONSTRAINT [FK_VacancySecondarySkills_Vacancies] FOREIGN KEY([Vacancy])
REFERENCES [dbo].[Vacancies] ([Id])
GO
ALTER TABLE [dbo].[VacancySecondarySkills] CHECK CONSTRAINT [FK_VacancySecondarySkills_Vacancies]
GO
USE [master]
GO
ALTER DATABASE [newdb] SET  READ_WRITE 
GO
