-- seed_funding.sql (Member D): sample funding data. This is sample information to be verified.
-- Only inserts when the FundOption table is empty, so running it twice does nothing.
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

