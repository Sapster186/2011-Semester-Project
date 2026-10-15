-- INF2011S: Member C demo accounts and degree-interest mappings.
-- Run after InterestQuestionnaire_Seed.sql and FillTables.sql.
-- Passwords follow LoginController's existing plain-text comparison convention.
-- Demo accounts only: admin1 / Admin123! and advisor1 / Advisor123!
-- Existing accounts, passwords, mappings and weights are never overwritten.
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WHERE userName = 'admin1')
        INSERT INTO dbo.UserAccount (userName, passHash, role, isActive)
        VALUES ('admin1', 'Admin123!', 'Administrator', 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WHERE userName = 'advisor1')
        INSERT INTO dbo.UserAccount (userName, passHash, role, isActive)
        VALUES ('advisor1', 'Advisor123!', 'Career Advisor', 1);

    -- A conflicting existing username needs review; do not silently change it.
    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount
                   WHERE userName = 'admin1' AND role = 'Administrator' AND isActive = 1)
        THROW 50001, 'Existing admin1 is inactive or has another role. Review that account before seeding.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount
                   WHERE userName = 'advisor1' AND role = 'Career Advisor' AND isActive = 1)
        THROW 50002, 'Existing advisor1 is inactive or has another role. Review that account before seeding.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.DegreeProgramme WHERE isActive = 1)
        THROW 50003, 'No active degrees found. Run the team degree seed in FillTables.sql first.', 1;

    -- Names below are the ten degree definitions currently in FillTables.sql.
    -- Join by name to use their real degreeID values, including on local databases
    -- where identities differ. A positive weight of 1 gives one primary interest.
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

    -- Do not claim success if a missing group or newly added degree is uncovered.
    IF EXISTS (SELECT 1 FROM dbo.DegreeProgramme d
               WHERE d.isActive = 1
                 AND NOT EXISTS (SELECT 1 FROM dbo.DegreeInterestMapping m
                                 INNER JOIN dbo.InterestGroup g ON g.interestGroupID = m.interestGroupID
                                 WHERE m.degreeID = d.degreeID AND m.weight > 0))
        THROW 50004, 'An active degree has no positive interest mapping. Check the groups and degree names.', 1;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

-- Verification output (passwords are not displayed).
SELECT userName, role, isActive FROM dbo.UserAccount
WHERE userName IN ('admin1', 'advisor1');

SELECT d.degreeID, d.degreeName, g.groupName, m.weight
FROM dbo.DegreeProgramme d
INNER JOIN dbo.DegreeInterestMapping m ON m.degreeID = d.degreeID
INNER JOIN dbo.InterestGroup g ON g.interestGroupID = m.interestGroupID
WHERE d.isActive = 1
ORDER BY d.degreeID, g.groupName;
