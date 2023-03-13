USE [master]
GO
/****** Object:  Database [LifeThreadening]    Script Date: 8-3-2023 19:03:26 ******/
CREATE DATABASE [LifeThreadening]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LifeThreadening', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\LifeThreadening.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LifeThreadening_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\LifeThreadening_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LifeThreadening] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LifeThreadening].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LifeThreadening] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LifeThreadening] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LifeThreadening] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LifeThreadening] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LifeThreadening] SET ARITHABORT OFF 
GO
ALTER DATABASE [LifeThreadening] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LifeThreadening] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LifeThreadening] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LifeThreadening] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LifeThreadening] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LifeThreadening] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LifeThreadening] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LifeThreadening] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LifeThreadening] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LifeThreadening] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LifeThreadening] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LifeThreadening] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LifeThreadening] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LifeThreadening] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LifeThreadening] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LifeThreadening] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LifeThreadening] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LifeThreadening] SET RECOVERY FULL 
GO
ALTER DATABASE [LifeThreadening] SET  MULTI_USER 
GO
ALTER DATABASE [LifeThreadening] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LifeThreadening] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LifeThreadening] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LifeThreadening] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LifeThreadening] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LifeThreadening] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [LifeThreadening] SET QUERY_STORE = OFF
GO
USE [LifeThreadening]
GO
/****** Object:  Table [dbo].[Ecosystem]    Script Date: 8-3-2023 19:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ecosystem](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[image] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EcosystemSpecies]    Script Date: 8-3-2023 19:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EcosystemSpecies](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ecosystemId] [int] NOT NULL,
	[speciesId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mutation]    Script Date: 8-3-2023 19:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mutation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](20) NOT NULL,
	[allel] [varchar](25) NOT NULL,
	[proteinBefore] [varchar](25) NOT NULL,
	[proteinAfter] [varchar](25) NOT NULL,
	[simulationPopulationId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MutationAffectedStatistic]    Script Date: 8-3-2023 19:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MutationAffectedStatistic](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[statistic] [varchar](50) NOT NULL,
	[affection] [int] NOT NULL,
	[mutationId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Obstruction]    Script Date: 8-3-2023 19:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Obstruction](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[image] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Simulation]    Script Date: 8-3-2023 19:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Simulation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[score] [int] NOT NULL,
	[dateStarted] [date] NOT NULL,
	[dateEnded] [date] NULL,
	[ecosystemId] [int] NOT NULL,
	[amountOfDisasters] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SimulationPopulation]    Script Date: 8-3-2023 19:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SimulationPopulation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[speciesId] [int] NOT NULL,
	[simulationId] [int] NOT NULL,
	[amountofAnimals] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Species]    Script Date: 8-3-2023 19:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Species](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](40) NOT NULL,
	[image] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vegetation]    Script Date: 8-3-2023 19:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vegetation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[image] [varchar](255) NOT NULL,
	[growth] [int] NOT NULL,
	[maxNutrition] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EcosystemSpecies]  WITH CHECK ADD  CONSTRAINT [FK_EcosystemSpecies_Ecosystem] FOREIGN KEY([ecosystemId])
REFERENCES [dbo].[Ecosystem] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EcosystemSpecies] CHECK CONSTRAINT [FK_EcosystemSpecies_Ecosystem]
GO
ALTER TABLE [dbo].[EcosystemSpecies]  WITH CHECK ADD  CONSTRAINT [FK_EcosystemSpecies_Species] FOREIGN KEY([speciesId])
REFERENCES [dbo].[Species] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EcosystemSpecies] CHECK CONSTRAINT [FK_EcosystemSpecies_Species]
GO
ALTER TABLE [dbo].[Mutation]  WITH CHECK ADD  CONSTRAINT [FK_Mutation_SimulationPopulationId] FOREIGN KEY([simulationPopulationId])
REFERENCES [dbo].[SimulationPopulation] ([id])
GO
ALTER TABLE [dbo].[Mutation] CHECK CONSTRAINT [FK_Mutation_SimulationPopulationId]
GO
ALTER TABLE [dbo].[MutationAffectedStatistic]  WITH CHECK ADD  CONSTRAINT [FK_MutationAffectedStatistic_Mutation] FOREIGN KEY([mutationId])
REFERENCES [dbo].[Mutation] ([id])
GO
ALTER TABLE [dbo].[MutationAffectedStatistic] CHECK CONSTRAINT [FK_MutationAffectedStatistic_Mutation]
GO
ALTER TABLE [dbo].[Simulation]  WITH CHECK ADD  CONSTRAINT [FK_Simulation_Ecosystem] FOREIGN KEY([ecosystemId])
REFERENCES [dbo].[Ecosystem] ([id])
GO
ALTER TABLE [dbo].[Simulation] CHECK CONSTRAINT [FK_Simulation_Ecosystem]
GO
ALTER TABLE [dbo].[SimulationPopulation]  WITH CHECK ADD  CONSTRAINT [FK_SimulationPopulation_Simulation] FOREIGN KEY([simulationId])
REFERENCES [dbo].[Simulation] ([id])
GO
ALTER TABLE [dbo].[SimulationPopulation] CHECK CONSTRAINT [FK_SimulationPopulation_Simulation]
GO
ALTER TABLE [dbo].[SimulationPopulation]  WITH CHECK ADD  CONSTRAINT [FK_SimulationPopulation_Species] FOREIGN KEY([speciesId])
REFERENCES [dbo].[Species] ([id])
GO
ALTER TABLE [dbo].[SimulationPopulation] CHECK CONSTRAINT [FK_SimulationPopulation_Species]
GO
USE [master]
GO
ALTER DATABASE [LifeThreadening] SET  READ_WRITE 
GO
