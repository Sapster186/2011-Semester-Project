-- INF2011S: Interest Questionnaire Seed Script
-- Member C
-- Inserts interest groups and questions. Safe to re-run (IF NOT EXISTS guards).

/* ---------- 1. Interest Groups ---------- */
IF NOT EXISTS (SELECT 1 FROM InterestGroup WHERE groupName = 'Computing & IT')
BEGIN
    INSERT INTO InterestGroup (groupName, description) VALUES
    ('Computing & IT', 'Programming, data, systems and technology'),
    ('Commerce & Business', 'Finance, accounting, management and entrepreneurship'),
    ('Health & Life Sciences', 'Biology, health, medicine and living systems'),
    ('Engineering & Physical Sciences', 'Physics, chemistry, structures and machines'),
    ('Humanities & Social Sciences', 'Language, history, psychology and society');
END
GO

/* ---------- 2. Interest Queries ---------- */
IF OBJECT_ID('dbo.InterestQuery', 'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I enjoy solving problems using logic and technology.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (1, 'I enjoy solving problems using logic and technology.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I am interested in how software and computer systems work.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (1, 'I am interested in how software and computer systems work.', 1.50, 1);

    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I enjoy working with money, business and financial decisions.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (2, 'I enjoy working with money, business and financial decisions.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I like leading projects and organising people or resources.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (2, 'I like leading projects and organising people or resources.', 1.50, 1);

    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I am interested in the human body, health and medicine.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (3, 'I am interested in the human body, health and medicine.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I enjoy studying living things and how they function.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (3, 'I enjoy studying living things and how they function.', 1.50, 1);

    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I enjoy building, designing or understanding physical structures.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (4, 'I enjoy building, designing or understanding physical structures.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I am curious about how the physical world works.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (4, 'I am curious about how the physical world works.', 1.50, 1);

    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I enjoy reading, writing and discussing ideas.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (5, 'I enjoy reading, writing and discussing ideas.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM InterestQuery WHERE queryText = 'I am interested in people, society and how they behave.')
        INSERT INTO InterestQuery (interestGroupID, queryText, weight, isActive)
        VALUES (5, 'I am interested in people, society and how they behave.', 1.50, 1);

    PRINT 'InterestQuery questions inserted.';
END
ELSE
BEGIN
    PRINT 'ERROR: dbo.InterestQuery does not exist. Ask Member A to create it first.';
END
GO
