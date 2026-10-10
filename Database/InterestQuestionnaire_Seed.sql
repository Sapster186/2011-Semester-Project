/* ============================================================
   Interest Questionnaire Seed Script
   Member C - INF2011S Semester Project
   Run AFTER Member A's table-creation script.
   ============================================================ */

-- Replace [YourDatabaseName] with the actual database name
USE [YourDatabaseName];
GO

/* ---------- 1. InterestGroup ---------- */
IF NOT EXISTS (SELECT 1 FROM InterestGroup)
BEGIN
    SET IDENTITY_INSERT InterestGroup ON;

    INSERT INTO InterestGroup (interestGroupID, groupName, description) VALUES
    (1, 'Computing & IT', 'Programming, data, systems and technology'),
    (2, 'Commerce & Business', 'Finance, accounting, management and entrepreneurship'),
    (3, 'Health & Life Sciences', 'Biology, health, medicine and living systems'),
    (4, 'Engineering & Physical Sciences', 'Physics, chemistry, structures and machines'),
    (5, 'Humanities & Social Sciences', 'Language, history, psychology and society');

    SET IDENTITY_INSERT InterestGroup OFF;
    PRINT 'InterestGroup seeded.';
END
ELSE
    PRINT 'InterestGroup already has data - skipped.';
GO

/* ---------- 2. InterestQuery ---------- */
IF NOT EXISTS (SELECT 1 FROM InterestQuery)
BEGIN
    SET IDENTITY_INSERT InterestQuery ON;

    INSERT INTO InterestQuery (interestQueryID, interestGroupID, questionText, weight, isActive) VALUES
    (1,  1, 'I enjoy solving problems using logic and technology.',                 2.00, 1),
    (2,  1, 'I am interested in how software and computer systems work.',           1.50, 1),
    (3,  2, 'I enjoy working with money, business and financial decisions.',        2.00, 1),
    (4,  2, 'I like leading projects and organising people or resources.',          1.50, 1),
    (5,  3, 'I am interested in the human body, health and medicine.',              2.00, 1),
    (6,  3, 'I enjoy studying living things and how they function.',                1.50, 1),
    (7,  4, 'I enjoy building, designing or understanding physical structures.',    2.00, 1),
    (8,  4, 'I am curious about how the physical world works (physics/chemistry).', 1.50, 1),
    (9,  5, 'I enjoy reading, writing and discussing ideas.',                       2.00, 1),
    (10, 5, 'I am interested in people, society and how they behave.',              1.50, 1);

    SET IDENTITY_INSERT InterestQuery OFF;
    PRINT 'InterestQuery seeded.';
END
ELSE
    PRINT 'InterestQuery already has data - skipped.';
GO

/* ---------- 3. Verify ---------- */
SELECT 'InterestGroup' AS TableName, COUNT(*) AS RowCount FROM InterestGroup
UNION ALL
SELECT 'InterestQuery', COUNT(*) FROM InterestQuery;
GO
