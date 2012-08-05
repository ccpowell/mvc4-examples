
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 08/04/2012 18:29:31
-- Generated from EDMX file: C:\GIT\OmbudsmanMap\OmbMvc4\OmbMf\Db\Ombudsman.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Ombudsman];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Facilities_Ombudsmen]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Facilities] DROP CONSTRAINT [FK_Facilities_Ombudsmen];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Facilities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Facilities];
GO
IF OBJECT_ID(N'[dbo].[Ombudsmen]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ombudsmen];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Facilities'
CREATE TABLE [dbo].[Facilities] (
    [FacilityId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [OmbudsmanId] int  NULL
);
GO

-- Creating table 'Ombudsmen'
CREATE TABLE [dbo].[Ombudsmen] (
    [OmbudsmanId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [UserName] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [FacilityId] in table 'Facilities'
ALTER TABLE [dbo].[Facilities]
ADD CONSTRAINT [PK_Facilities]
    PRIMARY KEY CLUSTERED ([FacilityId] ASC);
GO

-- Creating primary key on [OmbudsmanId] in table 'Ombudsmen'
ALTER TABLE [dbo].[Ombudsmen]
ADD CONSTRAINT [PK_Ombudsmen]
    PRIMARY KEY CLUSTERED ([OmbudsmanId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [OmbudsmanId] in table 'Facilities'
ALTER TABLE [dbo].[Facilities]
ADD CONSTRAINT [FK_dbo_Facilities_dbo_Ombudsmen_OmbudsmanId]
    FOREIGN KEY ([OmbudsmanId])
    REFERENCES [dbo].[Ombudsmen]
        ([OmbudsmanId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Facilities_dbo_Ombudsmen_OmbudsmanId'
CREATE INDEX [IX_FK_dbo_Facilities_dbo_Ombudsmen_OmbudsmanId]
ON [dbo].[Facilities]
    ([OmbudsmanId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------