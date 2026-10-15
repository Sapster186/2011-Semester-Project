-- Interest_Fix.sql
-- Run ONCE on a database that doesn't have these fixes yet.
-- Order: 1) Schema fixes  2) InterestQuestionnaire_Seed.sql  3) Data below

-- 1. Schema fixes
ALTER TABLE InterestQuery ADD isActive BIT NOT NULL DEFAULT 1;

EXEC sp_rename 'dbo.InterestQuery.queryID', 'interestQueryID', 'COLUMN';

-- 2. Now run InterestQuestionnaire_Seed.sql (5 groups, 10 questions)

-- 3. Degree to interest group mappings
-- Groups: 1 Computing, 2 Commerce, 3 Health, 4 Engineering, 5 Humanities
INSERT INTO DegreeInterestMapping (degreeID, interestGroupID, weight) VALUES
(1, 1, 1.0),   -- BCom Information Systems -> Computing
(1, 2, 0.8),   -- BCom Information Systems -> Commerce
(2, 2, 1.0),   -- BCom Accounting -> Commerce
(3, 2, 1.0),   -- BCom General -> Commerce
(4, 1, 1.0),   -- BSc Computer Science -> Computing
(5, 3, 1.0),   -- BSc Biological Sciences -> Health
(6, 4, 1.0),   -- BSc Engineering (Mechanical) -> Engineering
(7, 5, 1.0),   -- BA Humanities -> Humanities
(8, 5, 1.0),   -- BSocSci -> Humanities
(9, 5, 1.0),   -- LLB -> Humanities
(10, 3, 1.0);  -- Bachelor of Health Sciences -> Health

-- Interest answers for the 5 named learners
INSERT INTO LearnerInterestAnswer (learnerID, queryID, answer) VALUES
-- Gadija
(1, 1, '5'), (1, 2, '4'), (1, 3, '5'), (1, 4, '4'), (1, 5, '2'),
(1, 6, '2'), (1, 7, '1'), (1, 8, '2'), (1, 9, '3'), (1, 10, '2'),
-- Aadiel
(2, 1, '5'), (2, 2, '5'), (2, 3, '3'), (2, 4, '2'), (2, 5, '1'),
(2, 6, '1'), (2, 7, '3'), (2, 8, '3'), (2, 9, '2'), (2, 10, '2'),
-- Labeeqah
(3, 1, '2'), (3, 2, '2'), (3, 3, '1'), (3, 4, '2'), (3, 5, '5'),
(3, 6, '5'), (3, 7, '3'), (3, 8, '3'), (3, 9, '3'), (3, 10, '3'),
-- Siyabonga
(4, 1, '4'), (4, 2, '3'), (4, 3, '2'), (4, 4, '2'), (4, 5, '1'),
(4, 6, '2'), (4, 7, '5'), (4, 8, '5'), (4, 9, '2'), (4, 10, '1'),
-- Derek
(5, 1, '1'), (5, 2, '2'), (5, 3, '2'), (5, 4, '2'), (5, 5, '5'),
(5, 6, '4'), (5, 7, '1'), (5, 8, '2'), (5, 9, '4'), (5, 10, '4');

-- Check: expect 5, 10, 11, 50
SELECT COUNT(*) AS groups FROM InterestGroup;
SELECT COUNT(*) AS queries FROM InterestQuery;
SELECT COUNT(*) AS mappings FROM DegreeInterestMapping;
SELECT COUNT(*) AS answers FROM LearnerInterestAnswer;
