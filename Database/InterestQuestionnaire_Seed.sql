-- INF2011S: Interest Questionnaire Seed Script
-- Member C
-- Inserts interest questions linked to interest groups.


INSERT INTO [dbo].[InterestGroup] ([groupName]) VALUES
('Computing & IT'),
('Commerce & Business'),
('Health & Life Sciences'),
('Engineering & Physical Sciences'),
('Humanities & Social Sciences');




IF OBJECT_ID('dbo.InterestQuery', 'U') IS NOT NULL
BEGIN
    -- Computing & IT questions
    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy solving problems using logic and technology.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (1, 'I enjoy solving problems using logic and technology.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am interested in how software and computer systems work.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (1, 'I am interested in how software and computer systems work.', 1.50, 1);

    -- Commerce & Business questions
    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy working with money, business and financial decisions.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (2, 'I enjoy working with money, business and financial decisions.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I like leading projects and organising people or resources.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (2, 'I like leading projects and organising people or resources.', 1.50, 1);

    -- Health & Life Sciences questions
    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am interested in the human body, health and medicine.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (3, 'I am interested in the human body, health and medicine.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy studying living things and how they function.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (3, 'I enjoy studying living things and how they function.', 1.50, 1);

    -- Engineering & Physical Sciences questions
    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy building, designing or understanding physical structures.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (4, 'I enjoy building, designing or understanding physical structures.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am curious about how the physical world works.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (4, 'I am curious about how the physical world works.', 1.50, 1);

    -- Humanities & Social Sciences questions
    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I enjoy reading, writing and discussing ideas.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (5, 'I enjoy reading, writing and discussing ideas.', 2.00, 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.InterestQuery WHERE questionText = 'I am interested in people, society and how they behave.')
        INSERT INTO dbo.InterestQuery (interestGroupID, questionText, weight, isActive)
        VALUES (5, 'I am interested in people, society and how they behave.', 1.50, 1);

    PRINT 'InterestQuery questions inserted.';
END
ELSE
BEGIN
    PRINT 'ERROR: dbo.InterestQuery does not exist. Ask Member A to create it first.';
END
GO
