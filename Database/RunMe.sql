-- Create GradeLevel table
CREATE TABLE [dbo].[GradeLevel]
(
	[gradeLevelID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[gradeName] VARCHAR(20) NOT NULL
);

-- Create Subject table
CREATE TABLE [dbo].[Subject]
(
	[subjectID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[subjectName] VARCHAR(50) NOT NULL,
	[isMaths] BIT NOT NULL,
	[isMathsLit] BIT NOT NULL,
	[isScience] BIT NOT NULL
);

-- Create Faculty table
CREATE TABLE [dbo].[Faculty]
(
	[facultyID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[facultyName] VARCHAR(50) NOT NULL,
	[facultyDesc] VARCHAR(200) NULL
);

-- Create InterestGroup table
CREATE TABLE [dbo].[InterestGroup]
(
	[interestGroupID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[groupName] VARCHAR(50) NOT NULL,
	[description] VARCHAR(200) NULL
);

-- Create FundOption table
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

-- Create UserAccount table
CREATE TABLE [dbo].[UserAccount]
(
	[userID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[userName] VARCHAR(50) NOT NULL,
	[passHash] VARCHAR(100) NOT NULL,
	[role] VARCHAR(20) NOT NULL,
	[isActive] BIT NOT NULL,
	CONSTRAINT [UQ_UserAccount_userName] UNIQUE ([userName])
);

-- Create Learner table
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

-- Create DegreeProgramme table
CREATE TABLE [dbo].[DegreeProgramme]
(
	[degreeID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[facultyID] INT NOT NULL,
	[degreeName] VARCHAR(100) NOT NULL,
	[minOverallAverage] FLOAT NOT NULL,
	[isActive] BIT NOT NULL,
	CONSTRAINT [FK_DegreeProgramme_Faculty] FOREIGN KEY ([facultyID]) REFERENCES [dbo].[Faculty] ([facultyID])
);

-- Create InterestQuery table
CREATE TABLE [dbo].[InterestQuery]
(
	[queryID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[interestGroupID] INT NOT NULL,
	[questionText] VARCHAR(200) NOT NULL,
	[weight] FLOAT NOT NULL,
	[isActive] BIT NOT NULL CONSTRAINT [DF_InterestQuery_isActive] DEFAULT 1,
	CONSTRAINT [FK_InterestQuery_InterestGroup] FOREIGN KEY ([interestGroupID]) REFERENCES [dbo].[InterestGroup] ([interestGroupID])
);

-- Create LearnerSubject table
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

-- Create LearnerInterestAnswer table
CREATE TABLE [dbo].[LearnerInterestAnswer]
(
	[answerID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[learnerID] INT NOT NULL,
	[queryID] INT NOT NULL,
	[answer] VARCHAR(50) NOT NULL,
	CONSTRAINT [FK_LearnerInterestAnswer_Learner] FOREIGN KEY ([learnerID]) REFERENCES [dbo].[Learner] ([learnerID]),
	CONSTRAINT [FK_LearnerInterestAnswer_InterestQuery] FOREIGN KEY ([queryID]) REFERENCES [dbo].[InterestQuery] ([queryID])
);

-- Create DegreeRequirements table
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

-- Create DegreeInterestMapping table
CREATE TABLE [dbo].[DegreeInterestMapping]
(
	[mappingID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[degreeID] INT NOT NULL,
	[interestGroupID] INT NOT NULL,
	[weight] FLOAT NOT NULL,
	CONSTRAINT [FK_DegreeInterestMapping_DegreeProgramme] FOREIGN KEY ([degreeID]) REFERENCES [dbo].[DegreeProgramme] ([degreeID]),
	CONSTRAINT [FK_DegreeInterestMapping_InterestGroup] FOREIGN KEY ([interestGroupID]) REFERENCES [dbo].[InterestGroup] ([interestGroupID])
);

-- Create AdviceSession table
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

-- Create FundFaculty table
CREATE TABLE [dbo].[FundFaculty]
(
	[fundFacultyID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[fundID] INT NOT NULL,
	[facultyID] INT NOT NULL,
	CONSTRAINT [FK_FundFaculty_FundOption] FOREIGN KEY ([fundID]) REFERENCES [dbo].[FundOption] ([fundID]),
	CONSTRAINT [FK_FundFaculty_Faculty] FOREIGN KEY ([facultyID]) REFERENCES [dbo].[Faculty] ([facultyID])
);

-- Create FundProgramme table
CREATE TABLE [dbo].[FundProgramme]
(
	[fundProgrammeID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[fundID] INT NOT NULL,
	[degreeID] INT NOT NULL,
	CONSTRAINT [FK_FundProgramme_FundOption] FOREIGN KEY ([fundID]) REFERENCES [dbo].[FundOption] ([fundID]),
	CONSTRAINT [FK_FundProgramme_DegreeProgramme] FOREIGN KEY ([degreeID]) REFERENCES [dbo].[DegreeProgramme] ([degreeID])
);

-- Create RecommendationResult table
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

-- Create ExportReport table
CREATE TABLE [dbo].[ExportReport]
(
	[reportID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[sessionID] INT NOT NULL,
	[dateOfExport] DATETIME NOT NULL,
	[fileFormat] VARCHAR(20) NOT NULL,
	[contentReport] VARCHAR(MAX) NOT NULL,
	CONSTRAINT [FK_ExportReport_AdviceSession] FOREIGN KEY ([sessionID]) REFERENCES [dbo].[AdviceSession] ([sessionID])
);

-- Separate schema creation from data insertion
GO

-- Rollback on run-time errors
SET XACT_ABORT ON;
-- Begin transaction for interest groups and queries
BEGIN TRANSACTION;

-- Insert Computing & IT interest group
IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Computing & IT')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Computing & IT', 'Programming, data, systems and technology');

-- Insert Commerce & Business interest group
IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Commerce & Business')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Commerce & Business', 'Finance, accounting, management and entrepreneurship');

-- Insert Health & Life Sciences interest group
IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Health & Life Sciences')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Health & Life Sciences', 'Biology, health, medicine and living systems');

-- Insert Engineering & Physical Sciences interest group
IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Engineering & Physical Sciences')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Engineering & Physical Sciences', 'Physics, chemistry, structures and machines');

-- Insert Humanities & Social Sciences interest group
IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Humanities & Social Sciences')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Humanities & Social Sciences', 'Language, history, psychology and society');

-- Insert logic query for Computing & IT
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy solving problems using logic and technology.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy solving problems using logic and technology.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Computing & IT';

-- Insert software query for Computing & IT
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am interested in how software and computer systems work.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I am interested in how software and computer systems work.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Computing & IT';

-- Insert finance query for Commerce & Business
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy working with money, business and financial decisions.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy working with money, business and financial decisions.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Commerce & Business';

-- Insert leadership query for Commerce & Business
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I like leading projects and organising people or resources.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I like leading projects and organising people or resources.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Commerce & Business';

-- Insert medicine query for Health & Life Sciences
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am interested in the human body, health and medicine.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I am interested in the human body, health and medicine.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Health & Life Sciences';

-- Insert biology query for Health & Life Sciences
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy studying living things and how they function.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy studying living things and how they function.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Health & Life Sciences';

-- Insert structural query for Engineering & Physical Sciences
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy building, designing or understanding physical structures.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy building, designing or understanding physical structures.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Engineering & Physical Sciences';

-- Insert physics query for Engineering & Physical Sciences
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am curious about how the physical world works.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I am curious about how the physical world works.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Engineering & Physical Sciences';

-- Insert reading/writing query for Humanities & Social Sciences
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy reading, writing and discussing ideas.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy reading, writing and discussing ideas.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Humanities & Social Sciences';

-- Insert society query for Humanities & Social Sciences
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am interested in people, society and how they behave.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I am interested in people, society and how they behave.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Humanities & Social Sciences';

-- Commit interest groups and queries
COMMIT TRANSACTION;

-- Insert funding options
IF NOT EXISTS (SELECT 1 FROM FundOption)
BEGIN
    INSERT INTO FundOption (fundName, fundType, eligibleSummary, necessaryDocs, closeDate, contactInformation, isActive)
    VALUES
    ('Commerce Merit Scholarship',      'Scholarship',      'Merit-based: strong results, applying to Commerce',          'ID, academic transcript',                    '2027-03-31', 'commerce.funding@example.org', 1),
    ('Science Faculty Bursary',         'Bursary',          'Need-based: financial need, applying to Science',            'ID, proof of income, academic transcript',   '2027-04-15', 'science.bursary@example.org', 1),
    ('Engineering Excellence Award',    'Scholarship',      'Merit-based: top Maths and Physical Sciences results',       'ID, academic transcript, motivation letter', '2027-02-28', 'eng.awards@example.org', 1),
    ('Health Sciences Financial Aid',   'Financial Aid',    'Need-based support for Health Sciences applicants',          'ID, proof of income',                        '2027-05-31', 'health.aid@example.org', 1),
    ('General Commerce Bursary',        'Bursary',          'Need-based: open to all Commerce applicants',                'ID, proof of income',                        '2027-06-30', 'commerce.bursary@example.org', 1),
    ('External STEM Foundation Grant',  'External Funding', 'Third-party funding for STEM degree applicants',             'ID, academic transcript',                    '2027-03-15', 'info@stemfoundation.example', 1),
    ('Old Commerce Scholarship',        'Scholarship',      'Closing date has passed (test data for test case 8)',        'ID, transcript',                             '2026-06-30', 'old@example.org', 1),
    ('Discontinued Bursary',            'Bursary',          'Deactivated option (test data for test case 8)',             'ID, transcript',                             '2027-08-31', 'discontinued@example.org', 0);
END

-- Insert Grade Levels
IF NOT EXISTS (SELECT 1 FROM [dbo].[GradeLevel] WHERE [gradeName] IN ('Grade 11', 'Grade 12'))
BEGIN
    INSERT INTO [dbo].[GradeLevel] ([gradeName]) VALUES
    ('Grade 11'),
    ('Grade 12');
END

-- Insert Subjects
IF NOT EXISTS (SELECT 1 FROM [dbo].[Subject] WHERE [subjectName] = 'Mathematics')
BEGIN
    INSERT INTO [dbo].[Subject] ([subjectName], [isMaths], [isMathsLit], [isScience]) VALUES
    ('English', 0, 0, 0),
    ('Afrikaans', 0, 0, 0),
    ('Mathematics', 1, 0, 0),
    ('Mathematical Literacy', 0, 1, 0),
    ('Physical Sciences', 0, 0, 1),
    ('Life Sciences', 0, 0, 0),
    ('Accounting', 0, 0, 0),
    ('Business Studies', 0, 0, 0),
    ('Economics', 0, 0, 0),
    ('Geography', 0, 0, 0),
    ('History', 0, 0, 0),
    ('Computer Applications Technology', 0, 0, 0),
    ('Life Orientation', 0, 0, 0),
    ('Tourism', 0, 0, 0),
    ('Engineering Graphics and Design', 0, 0, 0),
    ('Visual Arts', 0, 0, 0);
END

-- Insert Learners
IF NOT EXISTS (SELECT 1 FROM [dbo].[Learner] WHERE [name] = 'Gadija Jacobs')
BEGIN
    INSERT INTO [dbo].[Learner] ([gradeLevelID], [name], [schoolName], [province], [contactDetails], [dateCreated]) VALUES
    (2, 'Gadija Jacobs', 'Westlake High School', 'Western Cape', NULL, GETDATE()),   -- learnerID 1
    (2, 'Aadiel-Raboo Magwa', 'Riverside Secondary School', 'Gauteng', 'aadiel.magwa@example.com', GETDATE()),   -- learnerID 2
    (1, 'Labeeqah Rademeyer', 'Mountain View High School', 'Western Cape', '082 555 0103', GETDATE()),   -- learnerID 3
    (2, 'Siyabonga Dickson', 'Lakeside High School', 'KwaZulu-Natal', NULL, GETDATE()),   -- learnerID 4
    (2, 'Derek Vin Diesel', 'Greenfield Secondary School', 'Gauteng', 'derek.diesel@example.com', GETDATE()),   -- learnerID 5
    (2, 'Nomvula Dlamini', 'Ridgeview Secondary School', 'KwaZulu-Natal', NULL, GETDATE()),   -- learnerID 6
    (2, 'Pieter van Wyk', 'Hillcrest High School', 'Free State', '083 555 0107', GETDATE()),   -- learnerID 7
    (1, 'Ayesha Khan', 'Parkview Academy', 'Gauteng', 'ayesha.khan@example.com', GETDATE()),   -- learnerID 8
    (2, 'Sipho Ndlovu', 'Valley Road Secondary', 'Eastern Cape', NULL, GETDATE()),   -- learnerID 9
    (1, 'Lerato Khumalo', 'Sunrise High School', 'Limpopo', '071 555 0110', GETDATE()),   -- learnerID 10
    (1, 'Ethan Pillay', 'Bayside Secondary School', 'KwaZulu-Natal', NULL, GETDATE());   -- learnerID 11
END

-- Insert subject marks for learnerID 1
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 1)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (1, 1, 72),   -- English
    (1, 3, 70),   -- Mathematics
    (1, 7, 74),   -- Accounting
    (1, 8, 79),   -- Business Studies
    (1, 9, 70),   -- Economics
    (1, 13, 80);  -- Life Orientation
END

-- Insert subject marks for learnerID 2
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 2)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (2, 1, 65),   -- English
    (2, 4, 78),   -- Mathematical Literacy
    (2, 8, 71),   -- Business Studies
    (2, 11, 69),  -- History
    (2, 14, 75),  -- Tourism
    (2, 13, 82);  -- Life Orientation
END

-- Insert subject marks for learnerID 3
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 3)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (3, 1, 80),   -- English
    (3, 3, 58),   -- Mathematics
    (3, 5, 62),   -- Physical Sciences
    (3, 6, 70),   -- Life Sciences
    (3, 10, 66),  -- Geography
    (3, 13, 85);  -- Life Orientation
END

-- Insert subject marks for learnerID 4
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 4)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (4, 1, 70),   -- English
    (4, 3, 75),   -- Mathematics
    (4, 5, 78),   -- Physical Sciences
    (4, 15, 73),  -- Engineering Graphics and Design
    (4, 13, 80);  -- Life Orientation
END

-- Insert subject marks for learnerID 5
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 5)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (5, 1, 82),   -- English
    (5, 4, 76),   -- Mathematical Literacy
    (5, 11, 79),  -- History
    (5, 6, 68),   -- Life Sciences
    (5, 16, 85),  -- Visual Arts
    (5, 13, 90);  -- Life Orientation
END

-- Insert subject marks for learnerID 6
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 6)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (6, 1, 74),   -- English
    (6, 4, 71),   -- Mathematical Literacy
    (6, 11, 80),  -- History
    (6, 10, 68),  -- Geography
    (6, 14, 70),  -- Tourism
    (6, 13, 84);  -- Life Orientation
END

-- Insert subject marks for learnerID 7
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 7)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (7, 1, 66),   -- English
    (7, 2, 72),   -- Afrikaans
    (7, 3, 48),   -- Mathematics
    (7, 7, 61),   -- Accounting
    (7, 8, 64),   -- Business Studies
    (7, 9, 58),   -- Economics
    (7, 13, 75);  -- Life Orientation
END

-- Insert subject marks for learnerID 8
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 8)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (8, 1, 78),   -- English
    (8, 3, 85),   -- Mathematics
    (8, 5, 81),   -- Physical Sciences
    (8, 6, 76),   -- Life Sciences
    (8, 12, 88),  -- Computer Applications Technology
    (8, 13, 82);  -- Life Orientation
END

-- Insert subject marks for learnerID 9
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 9)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (9, 1, 60),   -- English
    (9, 3, 66),   -- Mathematics
    (9, 5, 59),   -- Physical Sciences
    (9, 10, 63),  -- Geography
    (9, 15, 68),  -- Engineering Graphics and Design
    (9, 13, 70);  -- Life Orientation
END

-- Insert subject marks for learnerID 10
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 10)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (10, 1, 85),   -- English
    (10, 2, 79),   -- Afrikaans
    (10, 4, 65),   -- Mathematical Literacy
    (10, 6, 71),   -- Life Sciences
    (10, 11, 82),  -- History
    (10, 16, 77),  -- Visual Arts
    (10, 13, 88);  -- Life Orientation
END

-- Insert subject marks for learnerID 11
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 11)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (11, 1, 70),   -- English
    (11, 13, 72);  -- Life Orientation
END

-- Insert Faculties
IF NOT EXISTS (SELECT 1 FROM [dbo].[Faculty])
BEGIN
INSERT INTO [dbo].[Faculty] ([facultyName], [facultyDesc]) VALUES
('Commerce', 'Faculty of Commerce'),
('Humanities', 'Faculty of Humanities'),
('Science', 'Faculty of Science'),
('Engineering', 'Faculty of Engineering and the Built Environment'),
('Health Sciences', 'Faculty of Health Sciences');
END

-- Insert Degree Programmes
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeProgramme])
BEGIN
INSERT INTO [dbo].[DegreeProgramme] ([facultyID], [degreeName], [minOverallAverage], [isActive]) VALUES
(1, 'BCom Information Systems', 60, 1),
(1, 'BCom Accounting', 65, 1),
(1, 'BCom General', 55, 1),
(3, 'BSc Computer Science', 70, 1),
(3, 'BSc Biological Sciences', 60, 1),
(4, 'BSc Engineering (Mechanical)', 70, 1),
(2, 'BA Humanities', 55, 1),
(2, 'BSocSci', 60, 1),
(2, 'LLB Law', 70, 1),
(5, 'Bachelor of Health Sciences', 70, 1);
END

-- Insert requirements for BCom Information Systems (degreeID 1)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 1)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(1, 1, 60, 1, 0, 'English 60% required'),
(1, 3, 60, 1, 0, 'Mathematics 60% required'),
(1, 7, 60, 1, 0, 'Accounting 60% required');
END

-- Insert requirements for BCom Accounting (degreeID 2)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 2)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(2, 1, 60, 1, 0, 'English 60% required'),
(2, 3, 65, 1, 0, 'Mathematics 65% required'),
(2, 7, 65, 1, 0, 'Accounting 65% required');
END

-- Insert requirements for BCom General (degreeID 3)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 3)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(3, 1, 55, 1, 0, 'English 55% required'),
(3, 3, 55, 0, 1, 'Mathematics accepted'),
(3, 4, 55, 0, 1, 'Maths Literacy accepted');
END

-- Insert requirements for BSc Computer Science (degreeID 4)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 4)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(4, 1, 60, 1, 0, 'English 60% required'),
(4, 3, 70, 1, 0, 'Mathematics 70% required (Maths Lit not accepted)');
END

-- Insert requirements for BSc Biological Sciences (degreeID 5)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 5)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(5, 1, 60, 1, 0, 'English 60% required'),
(5, 3, 60, 1, 0, 'Mathematics 60% required'),
(5, 5, 60, 1, 0, 'Physical Sciences 60% required');
END

-- Insert requirements for BSc Engineering Mechanical (degreeID 6)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 6)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(6, 1, 60, 1, 0, 'English 60% required'),
(6, 3, 70, 1, 0, 'Mathematics 70% required'),
(6, 5, 70, 1, 0, 'Physical Sciences 70% required');
END

-- Insert requirements for BA Humanities (degreeID 7)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 7)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(7, 1, 55, 1, 0, 'English 55% required');
END

-- Insert requirements for BSocSci (degreeID 8)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 8)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(8, 1, 60, 1, 0, 'English 60% required'),
(8, 3, 55, 0, 1, 'Mathematics accepted'),
(8, 4, 55, 0, 1, 'Maths Literacy accepted');
END

-- Insert requirements for LLB Law (degreeID 9)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 9)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(9, 1, 70, 1, 0, 'English 70% required'),
(9, 11, 60, 0, 1, 'History is helpful');
END

-- Insert requirements for Bachelor of Health Sciences (degreeID 10)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 10)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(10, 1, 60, 1, 0, 'English 60% required'),
(10, 3, 60, 1, 0, 'Mathematics 60% required'),
(10, 5, 60, 1, 0, 'Physical Sciences 60% required');
END

-- Insert questionnaire responses for learnerID 1
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 1)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (1, 1, '4'),   -- question 1 (Computing)
    (1, 2, '4'),   -- question 2 (Computing)
    (1, 3, '5'),   -- question 3 (Commerce)
    (1, 4, '4'),   -- question 4 (Commerce)
    (1, 5, '1'),   -- question 5 (Health)
    (1, 6, '2'),   -- question 6 (Health)
    (1, 7, '2'),   -- question 7 (Engineering)
    (1, 8, '2'),   -- question 8 (Engineering)
    (1, 9, '3'),   -- question 9 (Humanities)
    (1, 10, '2');  -- question 10 (Humanities)
END

-- Insert questionnaire responses for learnerID 2
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 2)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (2, 1, '5'),   -- question 1 (Computing)
    (2, 2, '5'),   -- question 2 (Computing)
    (2, 3, '3'),   -- question 3 (Commerce)
    (2, 4, '2'),   -- question 4 (Commerce)
    (2, 5, '1'),   -- question 5 (Health)
    (2, 6, '1'),   -- question 6 (Health)
    (2, 7, '3'),   -- question 7 (Engineering)
    (2, 8, '3'),   -- question 8 (Engineering)
    (2, 9, '2'),   -- question 9 (Humanities)
    (2, 10, '2');  -- question 10 (Humanities)
END

-- Insert questionnaire responses for learnerID 3
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 3)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (3, 1, '2'),   -- question 1 (Computing)
    (3, 2, '2'),   -- question 2 (Computing)
    (3, 3, '1'),   -- question 3 (Commerce)
    (3, 4, '2'),   -- question 4 (Commerce)
    (3, 5, '5'),   -- question 5 (Health)
    (3, 6, '5'),   -- question 6 (Health)
    (3, 7, '3'),   -- question 7 (Engineering)
    (3, 8, '3'),   -- question 8 (Engineering)
    (3, 9, '3'),   -- question 9 (Humanities)
    (3, 10, '3');  -- question 10 (Humanities)
END

-- Insert questionnaire responses for learnerID 4
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 4)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (4, 1, '4'),   -- question 1 (Computing)
    (4, 2, '3'),   -- question 2 (Computing)
    (4, 3, '2'),   -- question 3 (Commerce)
    (4, 4, '2'),   -- question 4 (Commerce)
    (4, 5, '1'),   -- question 5 (Health)
    (4, 6, '2'),   -- question 6 (Health)
    (4, 7, '5'),   -- question 7 (Engineering)
    (4, 8, '5'),   -- question 8 (Engineering)
    (4, 9, '2'),   -- question 9 (Humanities)
    (4, 10, '1');  -- question 10 (Humanities)
END

-- Insert questionnaire responses for learnerID 5
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 5)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (5, 1, '1'),   -- question 1 (Computing)
    (5, 2, '2'),   -- question 2 (Computing)
    (5, 3, '2'),   -- question 3 (Commerce)
    (5, 4, '2'),   -- question 4 (Commerce)
    (5, 5, '5'),   -- question 5 (Health)
    (5, 6, '4'),   -- question 6 (Health)
    (5, 7, '1'),   -- question 7 (Engineering)
    (5, 8, '2'),   -- question 8 (Engineering)
    (5, 9, '4'),   -- question 9 (Humanities)
    (5, 10, '4');  -- question 10 (Humanities)
END

-- Insert questionnaire responses for learnerID 6
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 6)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (6, 1, '1'),   -- question 1 (Computing)
    (6, 2, '2'),   -- question 2 (Computing)
    (6, 3, '3'),   -- question 3 (Commerce)
    (6, 4, '3'),   -- question 4 (Commerce)
    (6, 5, '2'),   -- question 5 (Health)
    (6, 6, '3'),   -- question 6 (Health)
    (6, 7, '1'),   -- question 7 (Engineering)
    (6, 8, '1'),   -- question 8 (Engineering)
    (6, 9, '5'),   -- question 9 (Humanities)
    (6, 10, '4');  -- question 10 (Humanities)
END

-- Insert questionnaire responses for learnerID 7
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 7)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (7, 1, '3'),   -- question 1 (Computing)
    (7, 2, '3'),   -- question 2 (Computing)
    (7, 3, '4'),   -- question 3 (Commerce)
    (7, 4, '5'),   -- question 4 (Commerce)
    (7, 5, '1'),   -- question 5 (Health)
    (7, 6, '2'),   -- question 6 (Health)
    (7, 7, '2'),   -- question 7 (Engineering)
    (7, 8, '1'),   -- question 8 (Engineering)
    (7, 9, '3'),   -- question 9 (Humanities)
    (7, 10, '2');  -- question 10 (Humanities)
END

-- Insert questionnaire responses for learnerID 8
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 8)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (8, 1, '4'),   -- question 1 (Computing)
    (8, 2, '3'),   -- question 2 (Computing)
    (8, 3, '2'),   -- question 3 (Commerce)
    (8, 4, '2'),   -- question 4 (Commerce)
    (8, 5, '5'),   -- question 5 (Health)
    (8, 6, '4'),   -- question 6 (Health)
    (8, 7, '3'),   -- question 7 (Engineering)
    (8, 8, '4'),   -- question 8 (Engineering)
    (8, 9, '2'),   -- question 9 (Humanities)
    (8, 10, '2');  -- question 10 (Humanities)
END

-- Insert questionnaire responses for learnerID 9
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 9)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (9, 1, '3'),   -- question 1 (Computing)
    (9, 2, '3'),   -- question 2 (Computing)
    (9, 3, '2'),   -- question 3 (Commerce)
    (9, 4, '3'),   -- question 4 (Commerce)
    (9, 5, '1'),   -- question 5 (Health)
    (9, 6, '1'),   -- question 6 (Health)
    (9, 7, '5'),   -- question 7 (Engineering)
    (9, 8, '4'),   -- question 8 (Engineering)
    (9, 9, '2'),   -- question 9 (Humanities)
    (9, 10, '2');  -- question 10 (Humanities)
END

-- Insert questionnaire responses for learnerID 10
IF (SELECT COUNT(*) FROM [dbo].[InterestQuery]) >= 10 AND NOT EXISTS (SELECT 1 FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID] = 10)
BEGIN
    INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
    (10, 1, '1'),   -- question 1 (Computing)
    (10, 2, '2'),   -- question 2 (Computing)
    (10, 3, '2'),   -- question 3 (Commerce)
    (10, 4, '2'),   -- question 4 (Commerce)
    (10, 5, '3'),   -- question 5 (Health)
    (10, 6, '3'),   -- question 6 (Health)
    (10, 7, '1'),   -- question 7 (Engineering)
    (10, 8, '2'),   -- question 8 (Engineering)
    (10, 9, '5'),   -- question 9 (Humanities)
    (10, 10, '5');  -- question 10 (Humanities)
END

-- Rollback on run-time errors
SET XACT_ABORT ON;

-- Begin transaction for demo accounts and degree mappings
BEGIN TRY
    BEGIN TRANSACTION;

    -- Insert default admin account
    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WHERE userName = 'admin1')
        INSERT INTO dbo.UserAccount (userName, passHash, role, isActive)
        VALUES ('admin1', 'Admin123!', 'Administrator', 1);

    -- Insert default advisor account
    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WHERE userName = 'advisor1')
        INSERT INTO dbo.UserAccount (userName, passHash, role, isActive)
        VALUES ('advisor1', 'Advisor123!', 'Career Advisor', 1);

    -- Verify active admin role status
    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WHERE userName = 'admin1' AND role = 'Administrator' AND isActive = 1)
        THROW 50001, 'Existing admin1 is inactive or has another role. Review that account before seeding.', 1;

    -- Verify active advisor role status
    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WHERE userName = 'advisor1' AND role = 'Career Advisor' AND isActive = 1)
        THROW 50002, 'Existing advisor1 is inactive or has another role. Review that account before seeding.', 1;

    -- Check for presence of active degree programs
    IF NOT EXISTS (SELECT 1 FROM dbo.DegreeProgramme WHERE isActive = 1)
        THROW 50003, 'No active degrees found. Run the team degree seed in FillTables.sql first.', 1;

    -- Map degrees to corresponding interest groups
    INSERT INTO dbo.DegreeInterestMapping (degreeID, interestGroupID, weight)
    SELECT d.degreeID, g.interestGroupID, 1.00
    FROM (VALUES
        ('BCom Information Systems', 'Computing & IT'),
        ('BCom Accounting', 'Commerce & Business'),
        ('BCom General', 'Commerce & Business'),
        ('BSc Computer Science', 'Computing & IT'),
        ('BSc Biological Sciences', 'Health & Life Sciences'),
        ('BSc Engineering (Mechanical)', 'Engineering & Physical Sciences'),
        ('BA Humanities', 'Humanities & Social Sciences'),
        ('BSocSci', 'Humanities & Social Sciences'),
        ('LLB Law', 'Humanities & Social Sciences'),
        ('Bachelor of Health Sciences', 'Health & Life Sciences')
    ) AS seed(degreeName, groupName)
    INNER JOIN dbo.DegreeProgramme d ON d.degreeName = seed.degreeName
    INNER JOIN dbo.InterestGroup g ON g.groupName = seed.groupName
    WHERE d.isActive = 1
      AND NOT EXISTS (SELECT 1 FROM dbo.DegreeInterestMapping m
                       WHERE m.degreeID = d.degreeID AND m.interestGroupID = g.interestGroupID);

    -- Ensure all active degrees have a positive interest mapping
    IF EXISTS (SELECT 1 FROM dbo.DegreeProgramme d
               WHERE d.isActive = 1
                 AND NOT EXISTS (SELECT 1 FROM dbo.DegreeInterestMapping m
                                 INNER JOIN dbo.InterestGroup g ON g.interestGroupID = m.interestGroupID
                                 WHERE m.degreeID = d.degreeID AND m.weight > 0))
        THROW 50004, 'An active degree has no positive interest mapping. Check the groups and degree names.', 1;

    -- Commit user accounts and mappings
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    -- Rollback on failure
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

-- Query default user accounts
SELECT userName, role, isActive FROM dbo.UserAccount
WHERE userName IN ('admin1', 'advisor1');

-- Query mapping links between degrees and interest groups
SELECT d.degreeID, d.degreeName, g.groupName, m.weight
FROM dbo.DegreeProgramme d
INNER JOIN dbo.DegreeInterestMapping m ON m.degreeID = d.degreeID
INNER JOIN dbo.InterestGroup g ON g.interestGroupID = m.interestGroupID
WHERE d.isActive = 1
ORDER BY d.degreeID, g.groupName;

-- Map funding options to eligible faculties
INSERT INTO dbo.FundFaculty (fundID, facultyID)
SELECT f.fundID, fa.facultyID
FROM (VALUES
    ('Commerce Merit Scholarship',     'Commerce'),
    ('General Commerce Bursary',       'Commerce'),
    ('Old Commerce Scholarship',       'Commerce'),
    ('Discontinued Bursary',           'Commerce'),
    ('Science Faculty Bursary',        'Science'),
    ('Engineering Excellence Award',   'Engineering'),
    ('Health Sciences Financial Aid',  'Health Sciences')
) AS v (fundName, facultyName)
JOIN dbo.FundOption f ON f.fundName = v.fundName
JOIN dbo.Faculty fa   ON fa.facultyName = v.facultyName
WHERE NOT EXISTS (SELECT 1 FROM dbo.FundFaculty x WHERE x.fundID = f.fundID AND x.facultyID = fa.facultyID);

-- Map funding options to eligible degree programmes
INSERT INTO dbo.FundProgramme (fundID, degreeID)
SELECT f.fundID, d.degreeID
FROM (VALUES
    ('External STEM Foundation Grant', 'BSc Computer Science'),
    ('External STEM Foundation Grant', 'BSc Biological Sciences'),
    ('External STEM Foundation Grant', 'BSc Engineering (Mechanical)')
) AS v (fundName, degreeName)
JOIN dbo.FundOption f      ON f.fundName = v.fundName
JOIN dbo.DegreeProgramme d ON d.degreeName = v.degreeName
WHERE NOT EXISTS (SELECT 1 FROM dbo.FundProgramme x WHERE x.fundID = f.fundID AND x.degreeID = d.degreeID);

-- Count generated links in funding pivot tables
SELECT (SELECT COUNT(*) FROM dbo.FundFaculty)   AS FundFacultyLinks,
       (SELECT COUNT(*) FROM dbo.FundProgramme) AS FundProgrammeLinks;

-- Query active funding options for Commerce faculty and BCom Information Systems degree
SELECT f.fundName, f.closeDate, f.isActive
FROM dbo.FundOption f
WHERE f.isActive = 1
  AND (f.closeDate IS NULL OR f.closeDate >= CAST(GETDATE() AS DATE))
  AND (EXISTS (SELECT 1 FROM dbo.FundFaculty ff
               JOIN dbo.Faculty fa ON fa.facultyID = ff.facultyID
               WHERE ff.fundID = f.fundID AND fa.facultyName = 'Commerce')
       OR EXISTS (SELECT 1 FROM dbo.FundProgramme fp
                  JOIN dbo.DegreeProgramme d ON d.degreeID = fp.degreeID
                  JOIN dbo.Faculty fa ON fa.facultyID = d.facultyID
                  WHERE fp.fundID = f.fundID AND fa.facultyName = 'Commerce'))
ORDER BY f.fundName;

-- Aggregate total row counts per table for data verification
SELECT
 (SELECT COUNT(*) FROM [dbo].[GradeLevel]) AS GradeLevel,
 (SELECT COUNT(*) FROM [dbo].[Subject]) AS Subject,
 (SELECT COUNT(*) FROM [dbo].[Learner]) AS Learner,
 (SELECT COUNT(*) FROM [dbo].[LearnerSubject]) AS LearnerSubject,
 (SELECT COUNT(*) FROM [dbo].[InterestGroup]) AS InterestGroup,
 (SELECT COUNT(*) FROM [dbo].[InterestQuery]) AS InterestQuery,
 (SELECT COUNT(*) FROM [dbo].[LearnerInterestAnswer]) AS LearnerInterestAnswer;