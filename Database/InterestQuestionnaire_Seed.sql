-- INF2011S: Member C interest groups and questions.
-- Run after CreateTables.sql, before FillTables.sql on a fresh database.
-- Current checked-in schema: queryID, questionText (see AlterLearnerTables.sql).
-- No schema changes, deletes, identity resets or updates to existing questions.
-- Keep the question order: Member A's fresh-database answer seeds use IDs 1-10.
SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- Insert each missing group separately so a partially seeded database is supported.
IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Computing & IT')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Computing & IT', 'Programming, data, systems and technology');

IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Commerce & Business')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Commerce & Business', 'Finance, accounting, management and entrepreneurship');

IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Health & Life Sciences')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Health & Life Sciences', 'Biology, health, medicine and living systems');

IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Engineering & Physical Sciences')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Engineering & Physical Sciences', 'Physics, chemistry, structures and machines');

IF NOT EXISTS (SELECT 1 FROM dbo.InterestGroup WHERE groupName = 'Humanities & Social Sciences')
    INSERT INTO dbo.InterestGroup (groupName, description)
    VALUES ('Humanities & Social Sciences', 'Language, history, psychology and society');

-- Resolve real group IDs rather than assuming they are always 1-5.
IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy solving problems using logic and technology.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy solving problems using logic and technology.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Computing & IT';

IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am interested in how software and computer systems work.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I am interested in how software and computer systems work.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Computing & IT';

IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy working with money, business and financial decisions.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy working with money, business and financial decisions.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Commerce & Business';

IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I like leading projects and organising people or resources.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I like leading projects and organising people or resources.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Commerce & Business';

IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am interested in the human body, health and medicine.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I am interested in the human body, health and medicine.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Health & Life Sciences';

IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy studying living things and how they function.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy studying living things and how they function.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Health & Life Sciences';

IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy building, designing or understanding physical structures.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy building, designing or understanding physical structures.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Engineering & Physical Sciences';

IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am curious about how the physical world works.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I am curious about how the physical world works.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Engineering & Physical Sciences';

IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy reading, writing and discussing ideas.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I enjoy reading, writing and discussing ideas.', 2.00, 1
    FROM dbo.InterestGroup WHERE groupName = 'Humanities & Social Sciences';

IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am interested in people, society and how they behave.')
    INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
    SELECT interestGroupID, 'I am interested in people, society and how they behave.', 1.50, 1
    FROM dbo.InterestGroup WHERE groupName = 'Humanities & Social Sciences';

COMMIT TRANSACTION;
PRINT 'Member C interest groups and questions are ready.';
