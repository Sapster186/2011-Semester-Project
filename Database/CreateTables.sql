/*
  Future Path Student Advising System - create all 18 tables
  ----------------------------------------------------------
  How to run: right-click the database in Server Explorer, choose New Query,
  paste this whole file and click Execute.

  Safe to run more than once: a table that already exists is skipped,
  so nothing is dropped and no data is lost.

  The tables are in dependency order. A table with a foreign key must come
  after the table it points to, so do not reorder them.
*/

-- ===========================================================
-- ROUND 1: tables that depend on nothing
-- ===========================================================

IF OBJECT_ID(N'[dbo].[GradeLevel]', N'U') IS NULL
CREATE TABLE [dbo].[GradeLevel]
(
	[gradeLevelID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[gradeName] VARCHAR(20) NOT NULL
);

IF OBJECT_ID(N'[dbo].[Subject]', N'U') IS NULL
CREATE TABLE [dbo].[Subject]
(
	[subjectID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[subjectName] VARCHAR(50) NOT NULL,
	[isMaths] BIT NOT NULL,
	[isMathsLit] BIT NOT NULL,
	[isScience] BIT NOT NULL
);

IF OBJECT_ID(N'[dbo].[Faculty]', N'U') IS NULL
CREATE TABLE [dbo].[Faculty]
(
	[facultyID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[facultyName] VARCHAR(50) NOT NULL,
	[facultyDesc] VARCHAR(200) NULL
);

IF OBJECT_ID(N'[dbo].[InterestGroup]', N'U') IS NULL
CREATE TABLE [dbo].[InterestGroup]
(
	[interestGroupID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[groupName] VARCHAR(50) NOT NULL,
	[description] VARCHAR(200) NULL
);

IF OBJECT_ID(N'[dbo].[FundOption]', N'U') IS NULL
CREATE TABLE [dbo].[FundOption]
(
	[fundID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[fundName] VARCHAR(100) NOT NULL,
	[fundType] VARCHAR(50) NOT NULL,
	[eligibleSummary] VARCHAR(200) NULL,
	[necessaryDocs] VARCHAR(200) NULL,
	[closeDate] DATETIME NULL,
	[contactInformation] VARCHAR(200) NULL,
	[isActive] BIT NOT NULL
);

IF OBJECT_ID(N'[dbo].[UserAccount]', N'U') IS NULL
CREATE TABLE [dbo].[UserAccount]
(
	[userID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[userName] VARCHAR(50) NOT NULL,
	[passHash] VARCHAR(100) NOT NULL,
	[role] VARCHAR(20) NOT NULL,
	[isActive] BIT NOT NULL,
	CONSTRAINT [UQ_UserAccount_userName] UNIQUE ([userName])
);

-- ===========================================================
-- ROUND 2: depend on a Round 1 table
-- ===========================================================

IF OBJECT_ID(N'[dbo].[Learner]', N'U') IS NULL
CREATE TABLE [dbo].[Learner]
(
	[learnerID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[gradeLevelID] INT NOT NULL,
	[name] VARCHAR(100) NOT NULL,
	[schoolName] VARCHAR(100) NOT NULL,
	[province] VARCHAR(50) NOT NULL,
	[contactDetails] VARCHAR(100) NULL,
	[dateCreated] DATETIME NOT NULL,
	CONSTRAINT [FK_Learner_GradeLevel] FOREIGN KEY ([gradeLevelID]) REFERENCES [dbo].[GradeLevel] ([gradeLevelID])
);

IF OBJECT_ID(N'[dbo].[DegreeProgramme]', N'U') IS NULL
CREATE TABLE [dbo].[DegreeProgramme]
(
	[degreeID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[facultyID] INT NOT NULL,
	[degreeName] VARCHAR(100) NOT NULL,
	[minOverallAverage] FLOAT NOT NULL,
	[isActive] BIT NOT NULL,
	CONSTRAINT [FK_DegreeProgramme_Faculty] FOREIGN KEY ([facultyID]) REFERENCES [dbo].[Faculty] ([facultyID])
);

IF OBJECT_ID(N'[dbo].[InterestQuery]', N'U') IS NULL
CREATE TABLE [dbo].[InterestQuery]
(
	[queryID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[interestGroupID] INT NOT NULL,
	[queryText] VARCHAR(200) NOT NULL,
	[weight] FLOAT NOT NULL,
	CONSTRAINT [FK_InterestQuery_InterestGroup] FOREIGN KEY ([interestGroupID]) REFERENCES [dbo].[InterestGroup] ([interestGroupID])
);

-- ===========================================================
-- ROUND 3: depend on Round 1 and Round 2 tables
-- ===========================================================

IF OBJECT_ID(N'[dbo].[LearnerSubject]', N'U') IS NULL
CREATE TABLE [dbo].[LearnerSubject]
(
	[learnerSubjectID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[learnerID] INT NOT NULL,
	[subjectID] INT NOT NULL,
	[mark] FLOAT NOT NULL,
	CONSTRAINT [FK_LearnerSubject_Learner] FOREIGN KEY ([learnerID]) REFERENCES [dbo].[Learner] ([learnerID]),
	CONSTRAINT [FK_LearnerSubject_Subject] FOREIGN KEY ([subjectID]) REFERENCES [dbo].[Subject] ([subjectID]),
	CONSTRAINT [CK_LearnerSubject_mark] CHECK ([mark] >= 0 AND [mark] <= 100),
	CONSTRAINT [UQ_LearnerSubject_learner_subject] UNIQUE ([learnerID], [subjectID])
);

IF OBJECT_ID(N'[dbo].[LearnerInterestAnswer]', N'U') IS NULL
CREATE TABLE [dbo].[LearnerInterestAnswer]
(
	[answerID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[learnerID] INT NOT NULL,
	[queryID] INT NOT NULL,
	[answer] VARCHAR(50) NOT NULL,
	CONSTRAINT [FK_LearnerInterestAnswer_Learner] FOREIGN KEY ([learnerID]) REFERENCES [dbo].[Learner] ([learnerID]),
	CONSTRAINT [FK_LearnerInterestAnswer_InterestQuery] FOREIGN KEY ([queryID]) REFERENCES [dbo].[InterestQuery] ([queryID])
);

IF OBJECT_ID(N'[dbo].[DegreeRequirements]', N'U') IS NULL
CREATE TABLE [dbo].[DegreeRequirements]
(
	[requirementsID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[degreeID] INT NOT NULL,
	[subjectID] INT NOT NULL,
	[minMark] FLOAT NOT NULL,
	[isNeeded] BIT NOT NULL,
	[isAlternative] BIT NOT NULL,
	[adviceNote] VARCHAR(200) NULL,
	CONSTRAINT [FK_DegreeRequirements_DegreeProgramme] FOREIGN KEY ([degreeID]) REFERENCES [dbo].[DegreeProgramme] ([degreeID]),
	CONSTRAINT [FK_DegreeRequirements_Subject] FOREIGN KEY ([subjectID]) REFERENCES [dbo].[Subject] ([subjectID]),
	CONSTRAINT [CK_DegreeRequirements_minMark] CHECK ([minMark] >= 0 AND [minMark] <= 100)
);

IF OBJECT_ID(N'[dbo].[DegreeInterestMapping]', N'U') IS NULL
CREATE TABLE [dbo].[DegreeInterestMapping]
(
	[mappingID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[degreeID] INT NOT NULL,
	[interestGroupID] INT NOT NULL,
	[weight] FLOAT NOT NULL,
	CONSTRAINT [FK_DegreeInterestMapping_DegreeProgramme] FOREIGN KEY ([degreeID]) REFERENCES [dbo].[DegreeProgramme] ([degreeID]),
	CONSTRAINT [FK_DegreeInterestMapping_InterestGroup] FOREIGN KEY ([interestGroupID]) REFERENCES [dbo].[InterestGroup] ([interestGroupID])
);

IF OBJECT_ID(N'[dbo].[AdviceSession]', N'U') IS NULL
CREATE TABLE [dbo].[AdviceSession]
(
	[sessionID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[learnerID] INT NOT NULL,
	[degreeID] INT NOT NULL,
	[dateOfSession] DATETIME NOT NULL,
	[eligibleResult] VARCHAR(50) NOT NULL,
	[overallScore] FLOAT NOT NULL,
	CONSTRAINT [FK_AdviceSession_Learner] FOREIGN KEY ([learnerID]) REFERENCES [dbo].[Learner] ([learnerID]),
	CONSTRAINT [FK_AdviceSession_DegreeProgramme] FOREIGN KEY ([degreeID]) REFERENCES [dbo].[DegreeProgramme] ([degreeID])
);

IF OBJECT_ID(N'[dbo].[FundFaculty]', N'U') IS NULL
CREATE TABLE [dbo].[FundFaculty]
(
	[fundFacultyID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[fundID] INT NOT NULL,
	[facultyID] INT NOT NULL,
	CONSTRAINT [FK_FundFaculty_FundOption] FOREIGN KEY ([fundID]) REFERENCES [dbo].[FundOption] ([fundID]),
	CONSTRAINT [FK_FundFaculty_Faculty] FOREIGN KEY ([facultyID]) REFERENCES [dbo].[Faculty] ([facultyID])
);

IF OBJECT_ID(N'[dbo].[FundProgramme]', N'U') IS NULL
CREATE TABLE [dbo].[FundProgramme]
(
	[fundProgrammeID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[fundID] INT NOT NULL,
	[degreeID] INT NOT NULL,
	CONSTRAINT [FK_FundProgramme_FundOption] FOREIGN KEY ([fundID]) REFERENCES [dbo].[FundOption] ([fundID]),
	CONSTRAINT [FK_FundProgramme_DegreeProgramme] FOREIGN KEY ([degreeID]) REFERENCES [dbo].[DegreeProgramme] ([degreeID])
);

-- ===========================================================
-- ROUND 4: depend on AdviceSession
-- ===========================================================

IF OBJECT_ID(N'[dbo].[RecommendationResult]', N'U') IS NULL
CREATE TABLE [dbo].[RecommendationResult]
(
	[recommendationID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[sessionID] INT NOT NULL,
	[degreeID] INT NOT NULL,
	[subjectMatchScore] FLOAT NOT NULL,
	[markMatchScore] FLOAT NOT NULL,
	[interestMatchScore] FLOAT NOT NULL,
	[desiredFacultyScore] FLOAT NOT NULL,
	[overallScore] FLOAT NOT NULL,
	[isAlternative] BIT NOT NULL,
	[gapObservation] VARCHAR(300) NULL,
	CONSTRAINT [FK_RecommendationResult_AdviceSession] FOREIGN KEY ([sessionID]) REFERENCES [dbo].[AdviceSession] ([sessionID]),
	CONSTRAINT [FK_RecommendationResult_DegreeProgramme] FOREIGN KEY ([degreeID]) REFERENCES [dbo].[DegreeProgramme] ([degreeID])
);

IF OBJECT_ID(N'[dbo].[ExportReport]', N'U') IS NULL
CREATE TABLE [dbo].[ExportReport]
(
	[reportID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[sessionID] INT NOT NULL,
	[dateOfExport] DATETIME NOT NULL,
	[fileFormat] VARCHAR(20) NOT NULL,
	[contentReport] VARCHAR(MAX) NOT NULL,
	CONSTRAINT [FK_ExportReport_AdviceSession] FOREIGN KEY ([sessionID]) REFERENCES [dbo].[AdviceSession] ([sessionID])
);
