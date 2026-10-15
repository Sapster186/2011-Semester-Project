/*
=============================================================================

   Interest questions must be loaded first before running this script!

=============================================================================
*/

-- GradeLevel
IF NOT EXISTS (SELECT 1 FROM [dbo].[GradeLevel] WHERE [gradeName] IN ('Grade 11', 'Grade 12'))
BEGIN
    INSERT INTO [dbo].[GradeLevel] ([gradeName]) VALUES
    ('Grade 11'),
    ('Grade 12');
END

-- Subject (isMaths, isMathsLit, isScience: 1 = yes, 0 = no)
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

-- Learner (gradeLevelID: 1 = Grade 11, 2 = Grade 12)
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

-- Gadija Jacobs (learnerID 1)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 1)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (1, 1, 72),   -- English
    (1, 3, 70),   -- Mathematics
    (1, 7, 74),   -- Accounting
    (1, 8, 79),   -- Business Studies
    (1, 9, 70),   -- Economics
    (1, 13, 80);   -- Life Orientation
END

-- Aadiel-Raboo Magwa (learnerID 2)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 2)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (2, 1, 65),   -- English
    (2, 4, 78),   -- Mathematical Literacy
    (2, 8, 71),   -- Business Studies
    (2, 11, 69),   -- History
    (2, 14, 75),   -- Tourism
    (2, 13, 82);   -- Life Orientation
END

-- Labeeqah Rademeyer (learnerID 3)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 3)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (3, 1, 80),   -- English
    (3, 3, 58),   -- Mathematics
    (3, 5, 62),   -- Physical Sciences
    (3, 6, 70),   -- Life Sciences
    (3, 10, 66),   -- Geography
    (3, 13, 85);   -- Life Orientation
END

-- Siyabonga Dickson (learnerID 4)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 4)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (4, 1, 70),   -- English
    (4, 3, 75),   -- Mathematics
    (4, 5, 78),   -- Physical Sciences
    (4, 15, 73),   -- Engineering Graphics and Design
    (4, 13, 80);   -- Life Orientation
END

-- Derek Vin Diesel (learnerID 5)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 5)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (5, 1, 82),   -- English
    (5, 4, 76),   -- Mathematical Literacy
    (5, 11, 79),   -- History
    (5, 6, 68),   -- Life Sciences
    (5, 16, 85),   -- Visual Arts
    (5, 13, 90);   -- Life Orientation
END

-- Nomvula Dlamini (learnerID 6)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 6)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (6, 1, 74),   -- English
    (6, 4, 71),   -- Mathematical Literacy
    (6, 11, 80),   -- History
    (6, 10, 68),   -- Geography
    (6, 14, 70),   -- Tourism
    (6, 13, 84);   -- Life Orientation
END

-- Pieter van Wyk (learnerID 7)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 7)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (7, 1, 66),   -- English
    (7, 2, 72),   -- Afrikaans
    (7, 3, 48),   -- Mathematics
    (7, 7, 61),   -- Accounting
    (7, 8, 64),   -- Business Studies
    (7, 9, 58),   -- Economics
    (7, 13, 75);   -- Life Orientation
END

-- Ayesha Khan (learnerID 8)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 8)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (8, 1, 78),   -- English
    (8, 3, 85),   -- Mathematics
    (8, 5, 81),   -- Physical Sciences
    (8, 6, 76),   -- Life Sciences
    (8, 12, 88),   -- Computer Applications Technology
    (8, 13, 82);   -- Life Orientation
END

-- Sipho Ndlovu (learnerID 9)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 9)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (9, 1, 60),   -- English
    (9, 3, 66),   -- Mathematics
    (9, 5, 59),   -- Physical Sciences
    (9, 10, 63),   -- Geography
    (9, 15, 68),   -- Engineering Graphics and Design
    (9, 13, 70);   -- Life Orientation
END

-- Lerato Khumalo (learnerID 10)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 10)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (10, 1, 85),   -- English
    (10, 2, 79),   -- Afrikaans
    (10, 4, 65),   -- Mathematical Literacy
    (10, 6, 71),   -- Life Sciences
    (10, 11, 82),   -- History
    (10, 16, 77),   -- Visual Arts
    (10, 13, 88);   -- Life Orientation
END

-- Ethan Pillay (learnerID 11)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LearnerSubject] WHERE [learnerID] = 11)
BEGIN
    INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
    (11, 1, 70),   -- English
    (11, 13, 72);   -- Life Orientation
END

-- ===========================================================================================
-- MEMBER B: Faculty, DegreeProgramme, DegreeRequirements
-- These power the recommendation engine (AdviceController).
-- Requirements are tuned so the 5 named learners get their Part B outcomes:
--   Gadija   -> Strong Match     (BCom Information Systems, degreeID 1)
--   Aadiel   -> Not Eligible     (BSc Computer Science, degreeID 4, missing Maths)
--   Labeeqah -> Possible Match   (BSc Biological Sciences, degreeID 5, Maths 58% < 60%)
--   Siyabonga-> Strong Match     (BSc Engineering Mech, degreeID 6)
--   Derek    -> Not Eligible     (Bachelor of Health Sciences, degreeID 10, missing Maths + Phys Sci)
-- ===========================================================================================

-- Faculty (1=Commerce, 2=Humanities, 3=Science, 4=Engineering, 5=Health Sciences)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Faculty])
BEGIN
INSERT INTO [dbo].[Faculty] ([facultyName], [facultyDesc]) VALUES
('Commerce', 'Faculty of Commerce'),
('Humanities', 'Faculty of Humanities'),
('Science', 'Faculty of Science'),
('Engineering', 'Faculty of Engineering and the Built Environment'),
('Health Sciences', 'Faculty of Health Sciences');
END

-- DegreeProgramme (1=BCom IS, 2=BCom Acc, 3=BCom Gen, 4=BSc CS, 5=BSc Bio,
--                  6=BSc Eng Mech, 7=BA Hum, 8=BSocSci, 9=LLB, 10=BHS)
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

-- DegreeRequirements
-- BCom Information Systems (degreeID 1) - Gadija: Strong Match
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 1)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(1, 1, 60, 1, 0, 'English 60% required'),
(1, 3, 60, 1, 0, 'Mathematics 60% required'),
(1, 7, 60, 1, 0, 'Accounting 60% required');
END

-- BCom Accounting (degreeID 2)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 2)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(2, 1, 60, 1, 0, 'English 60% required'),
(2, 3, 65, 1, 0, 'Mathematics 65% required'),
(2, 7, 65, 1, 0, 'Accounting 65% required');
END

-- BCom General (degreeID 3) - Maths OR Maths Lit
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 3)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(3, 1, 55, 1, 0, 'English 55% required'),
(3, 3, 55, 0, 1, 'Mathematics accepted'),
(3, 4, 55, 0, 1, 'Maths Literacy accepted');
END

-- BSc Computer Science (degreeID 4) - Aadiel: Not Eligible (has Maths Lit, not Maths)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 4)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(4, 1, 60, 1, 0, 'English 60% required'),
(4, 3, 70, 1, 0, 'Mathematics 70% required (Maths Lit not accepted)');
END

-- BSc Biological Sciences (degreeID 5) - Labeeqah: Possible Match (Maths 58% < 60%)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 5)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(5, 1, 60, 1, 0, 'English 60% required'),
(5, 3, 60, 1, 0, 'Mathematics 60% required'),
(5, 5, 60, 1, 0, 'Physical Sciences 60% required');
END

-- BSc Engineering Mechanical (degreeID 6) - Siyabonga: Strong Match
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 6)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(6, 1, 60, 1, 0, 'English 60% required'),
(6, 3, 70, 1, 0, 'Mathematics 70% required'),
(6, 5, 70, 1, 0, 'Physical Sciences 70% required');
END

-- BA Humanities (degreeID 7)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 7)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(7, 1, 55, 1, 0, 'English 55% required');
END

-- BSocSci (degreeID 8) - Maths OR Maths Lit
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 8)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(8, 1, 60, 1, 0, 'English 60% required'),
(8, 3, 55, 0, 1, 'Mathematics accepted'),
(8, 4, 55, 0, 1, 'Maths Literacy accepted');
END

-- LLB Law (degreeID 9)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 9)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(9, 1, 70, 1, 0, 'English 70% required'),
(9, 11, 60, 0, 1, 'History is helpful');
END

-- Bachelor of Health Sciences (degreeID 10) - Derek: Not Eligible (missing Maths + Phys Sci)
IF NOT EXISTS (SELECT 1 FROM [dbo].[DegreeRequirements] WHERE [degreeID] = 10)
BEGIN
INSERT INTO [dbo].[DegreeRequirements] ([degreeID], [subjectID], [minMark], [isNeeded], [isAlternative], [adviceNote]) VALUES
(10, 1, 60, 1, 0, 'English 60% required'),
(10, 3, 60, 1, 0, 'Mathematics 60% required'),
(10, 5, 60, 1, 0, 'Physical Sciences 60% required');
END

-- ===========================================================================================
-- LearnerInterestAnswer (Member A). Member C's 10 questions must already be loaded,
-- so queryID 1 to 10 exist in InterestQuery (check: SELECT * FROM [dbo].[InterestQuery] ORDER BY queryID).
-- Answers are on a 1-5 scale (1 = strongly disagree, 5 = strongly agree).
-- Ethan Pillay has no answers on purpose (a learner who has not done the questionnaire yet).
-- ===========================================================================================

-- Gadija Jacobs (learnerID 1)
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
    (1, 10, '2');   -- question 10 (Humanities)
END

-- Aadiel-Raboo Magwa (learnerID 2)
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
    (2, 10, '2');   -- question 10 (Humanities)
END

-- Labeeqah Rademeyer (learnerID 3)
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
    (3, 10, '3');   -- question 10 (Humanities)
END

-- Siyabonga Dickson (learnerID 4)
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
    (4, 10, '1');   -- question 10 (Humanities)
END

-- Derek Vin Diesel (learnerID 5)
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
    (5, 10, '4');   -- question 10 (Humanities)
END

-- Nomvula Dlamini (learnerID 6)
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
    (6, 10, '4');   -- question 10 (Humanities)
END

-- Pieter van Wyk (learnerID 7)
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
    (7, 10, '2');   -- question 10 (Humanities)
END

-- Ayesha Khan (learnerID 8)
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
    (8, 10, '2');   -- question 10 (Humanities)
END

-- Sipho Ndlovu (learnerID 9)
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
    (9, 10, '2');   -- question 10 (Humanities)
END

-- Lerato Khumalo (learnerID 10)
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
    (10, 10, '5');   -- question 10 (Humanities)
END