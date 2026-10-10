-- GradeLevel
INSERT INTO [dbo].[GradeLevel] ([gradeName]) VALUES
('Grade 11'),
('Grade 12');

-- Subject (isMaths, isMathsLit, isScience: 1 = yes, 0 = no)
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

-- Learner (gradeLevelID: 1 = Grade 11, 2 = Grade 12)
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

-- Gadija Jacobs (learnerID 1)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(1, 1, 72),   -- English
(1, 3, 70),   -- Mathematics
(1, 7, 74),   -- Accounting
(1, 8, 79),   -- Business Studies
(1, 9, 70),   -- Economics
(1, 13, 80);   -- Life Orientation

-- Aadiel-Raboo Magwa (learnerID 2)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(2, 1, 65),   -- English
(2, 4, 78),   -- Mathematical Literacy
(2, 8, 71),   -- Business Studies
(2, 11, 69),   -- History
(2, 14, 75),   -- Tourism
(2, 13, 82);   -- Life Orientation

-- Labeeqah Rademeyer (learnerID 3)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(3, 1, 80),   -- English
(3, 3, 58),   -- Mathematics
(3, 5, 62),   -- Physical Sciences
(3, 6, 70),   -- Life Sciences
(3, 10, 66),   -- Geography
(3, 13, 85);   -- Life Orientation

-- Siyabonga Dickson (learnerID 4)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(4, 1, 70),   -- English
(4, 3, 75),   -- Mathematics
(4, 5, 78),   -- Physical Sciences
(4, 15, 73),   -- Engineering Graphics and Design
(4, 13, 80);   -- Life Orientation

-- Derek Vin Diesel (learnerID 5)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(5, 1, 82),   -- English
(5, 4, 76),   -- Mathematical Literacy
(5, 11, 79),   -- History
(5, 6, 68),   -- Life Sciences
(5, 16, 85),   -- Visual Arts
(5, 13, 90);   -- Life Orientation

-- Nomvula Dlamini (learnerID 6)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(6, 1, 74),   -- English
(6, 4, 71),   -- Mathematical Literacy
(6, 11, 80),   -- History
(6, 10, 68),   -- Geography
(6, 14, 70),   -- Tourism
(6, 13, 84);   -- Life Orientation

-- Pieter van Wyk (learnerID 7)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(7, 1, 66),   -- English
(7, 2, 72),   -- Afrikaans
(7, 3, 48),   -- Mathematics
(7, 7, 61),   -- Accounting
(7, 8, 64),   -- Business Studies
(7, 9, 58),   -- Economics
(7, 13, 75);   -- Life Orientation

-- Ayesha Khan (learnerID 8)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(8, 1, 78),   -- English
(8, 3, 85),   -- Mathematics
(8, 5, 81),   -- Physical Sciences
(8, 6, 76),   -- Life Sciences
(8, 12, 88),   -- Computer Applications Technology
(8, 13, 82);   -- Life Orientation

-- Sipho Ndlovu (learnerID 9)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(9, 1, 60),   -- English
(9, 3, 66),   -- Mathematics
(9, 5, 59),   -- Physical Sciences
(9, 10, 63),   -- Geography
(9, 15, 68),   -- Engineering Graphics and Design
(9, 13, 70);   -- Life Orientation

-- Lerato Khumalo (learnerID 10)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(10, 1, 85),   -- English
(10, 2, 79),   -- Afrikaans
(10, 4, 65),   -- Mathematical Literacy
(10, 6, 71),   -- Life Sciences
(10, 11, 82),   -- History
(10, 16, 77),   -- Visual Arts
(10, 13, 88);   -- Life Orientation

-- Ethan Pillay (learnerID 11)
INSERT INTO [dbo].[LearnerSubject] ([learnerID], [subjectID], [mark]) VALUES
(11, 1, 70),   -- English
(11, 13, 72);   -- Life Orientation

-- ===========================================================================================
-- LearnerInterestAnswer - NEEDS MEMBER C'S STUFF, so it is commented out.
-- Every answer points at a question (queryID) in InterestQuery, which is Member C's table.
-- When their questions are merged: check that queryID 1 to 8 are the Part B questions in order
-- (SELECT * FROM [dbo].[InterestQuery] ORDER BY queryID). Then delete the line that says
-- COMMENT START and the line that says COMMENT END to switch the answers on.
-- Answers are on a 1-5 scale (1 = strongly disagree, 5 = strongly agree).
-- Ethan Pillay has no answers on purpose (a learner who has not done the questionnaire yet).
-- ===========================================================================================
/* COMMENT START
-- Gadija Jacobs (learnerID 1)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(1, 1, '5'),   -- question 1
(1, 2, '3'),   -- question 2
(1, 3, '4'),   -- question 3
(1, 4, '2'),   -- question 4
(1, 5, '3'),   -- question 5
(1, 6, '5'),   -- question 6
(1, 7, '3'),   -- question 7
(1, 8, '2');   -- question 8

-- Aadiel-Raboo Magwa (learnerID 2)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(2, 1, '3'),   -- question 1
(2, 2, '3'),   -- question 2
(2, 3, '5'),   -- question 3
(2, 4, '2'),   -- question 4
(2, 5, '3'),   -- question 5
(2, 6, '4'),   -- question 6
(2, 7, '4'),   -- question 7
(2, 8, '2');   -- question 8

-- Labeeqah Rademeyer (learnerID 3)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(3, 1, '3'),   -- question 1
(3, 2, '4'),   -- question 2
(3, 3, '2'),   -- question 3
(3, 4, '5'),   -- question 4
(3, 5, '3'),   -- question 5
(3, 6, '1'),   -- question 6
(3, 7, '3'),   -- question 7
(3, 8, '2');   -- question 8

-- Siyabonga Dickson (learnerID 4)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(4, 1, '4'),   -- question 1
(4, 2, '2'),   -- question 2
(4, 3, '4'),   -- question 3
(4, 4, '4'),   -- question 4
(4, 5, '2'),   -- question 5
(4, 6, '2'),   -- question 6
(4, 7, '5'),   -- question 7
(4, 8, '1');   -- question 8

-- Derek Vin Diesel (learnerID 5)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(5, 1, '1'),   -- question 1
(5, 2, '4'),   -- question 2
(5, 3, '2'),   -- question 3
(5, 4, '2'),   -- question 4
(5, 5, '5'),   -- question 5
(5, 6, '2'),   -- question 6
(5, 7, '3'),   -- question 7
(5, 8, '5');   -- question 8

-- Nomvula Dlamini (learnerID 6)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(6, 1, '2'),   -- question 1
(6, 2, '4'),   -- question 2
(6, 3, '2'),   -- question 3
(6, 4, '1'),   -- question 4
(6, 5, '5'),   -- question 5
(6, 6, '3'),   -- question 6
(6, 7, '2'),   -- question 7
(6, 8, '5');   -- question 8

-- Pieter van Wyk (learnerID 7)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(7, 1, '5'),   -- question 1
(7, 2, '2'),   -- question 2
(7, 3, '3'),   -- question 3
(7, 4, '1'),   -- question 4
(7, 5, '2'),   -- question 5
(7, 6, '5'),   -- question 6
(7, 7, '3'),   -- question 7
(7, 8, '2');   -- question 8

-- Ayesha Khan (learnerID 8)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(8, 1, '5'),   -- question 1
(8, 2, '2'),   -- question 2
(8, 3, '5'),   -- question 3
(8, 4, '4'),   -- question 4
(8, 5, '2'),   -- question 5
(8, 6, '3'),   -- question 6
(8, 7, '4'),   -- question 7
(8, 8, '1');   -- question 8

-- Sipho Ndlovu (learnerID 9)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(9, 1, '3'),   -- question 1
(9, 2, '3'),   -- question 2
(9, 3, '3'),   -- question 3
(9, 4, '4'),   -- question 4
(9, 5, '2'),   -- question 5
(9, 6, '2'),   -- question 6
(9, 7, '5'),   -- question 7
(9, 8, '2');   -- question 8

-- Lerato Khumalo (learnerID 10)
INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID], [queryID], [answer]) VALUES
(10, 1, '1'),   -- question 1
(10, 2, '5'),   -- question 2
(10, 3, '2'),   -- question 3
(10, 4, '2'),   -- question 4
(10, 5, '5'),   -- question 5
(10, 6, '2'),   -- question 6
(10, 7, '3'),   -- question 7
(10, 8, '4');   -- question 8
COMMENT END */