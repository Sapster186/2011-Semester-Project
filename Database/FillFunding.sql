/*
  Future Path - funding links (FundFaculty and FundProgramme)
  --------------------------------------------------------------
  seed_funding.sql only fills FundOption. The Funding Information screen
  filters by faculty and degree through these two link tables, so without
  rows here the screen opens empty (spec test case 8).

  Run AFTER seed_funding.sql and FillTables.sql. Safe to run more than once:
  a link that already exists is skipped. Rows are matched by name, so it
  does not depend on the ID numbers.
*/

-- Faculty-level links: the fund is open to every degree in that faculty.
-- The expired and the inactive Commerce funds are linked on purpose, so you
-- can see them being filtered out (they must NOT show on screen).
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

-- Degree-level links: the fund is open to these degrees only.
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

-- Check 1: expect 7 faculty links and 3 degree links
SELECT (SELECT COUNT(*) FROM dbo.FundFaculty)   AS FundFacultyLinks,
       (SELECT COUNT(*) FROM dbo.FundProgramme) AS FundProgrammeLinks;

-- Check 2: what the screen shows for faculty Commerce, degree BCom Information Systems.
-- Expect exactly two rows: Commerce Merit Scholarship and General Commerce Bursary.
-- (Old Commerce Scholarship is past its closing date; Discontinued Bursary is inactive.)
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