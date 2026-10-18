# Future Path Student Advising System

A Windows Forms desktop application that helps Grade 11 and 12 learners check
their eligibility for university degrees, see funding options, and receive an
advisory summary.

Team 23 — INF2011S 2026

## What the app does

1. A learner creates a profile, captures subjects and marks, and answers a
   short interest questionnaire.
2. The system compares their marks against degree requirements and produces an
   outcome (Strong Match, Possible Match, Not Currently Eligible, or More
   Information Needed).
3. The learner sees recommended alternatives, relevant funding options, and a
   printable advisory summary.
4. Career Advisors can view learner sessions and generate two reports:
   - Report 2 (RPT-SUM-002) — Degree Interest and Eligibility
   - Report 3 (RPT-EXC-001) — Not Currently Eligible Exception

## Setup on a new laptop

### 1. Clone the repository and open the solution

    git clone <repo-url>
    Open 2011 Semester Project.sln in Visual Studio 2022.

### 2. Confirm LocalDB is running

In a command prompt:

    sqllocaldb info MSSQLLocalDB
    sqllocaldb start MSSQLLocalDB

### 3. Place the database file

The application uses a file-attach connection:

    AttachDbFilename=|DataDirectory|\Database\FuturePath.mdf

Put your copy of **FuturePath.mdf** (and, optionally, FuturePath_log.ldf) into
the project's **Database\** folder. Do NOT put it in the project root.

Do NOT commit the .mdf file. It is excluded by .gitignore and must be shared
between team members by hand or rebuilt from the SQL scripts below.

### 4. Build

    Ctrl+Shift+B in Visual Studio.

The .csproj copies Database\FuturePath.mdf into bin\Debug\Database\ on build.
After a successful build you should see both files in bin\Debug\Database\.

### 5. If you are rebuilding the database from scratch

In SQL Server Object Explorer, create a new database. Then run these scripts
once, in this order, against it:

1. Database\CreateTables.sql
2. Database\InterestQuestionnaire_Seed.sql
3. Database\FillTables.sql
4. Database\MemberC_Accounts_And_Mappings_Seed.sql
5. Database\seed_funding.sql
6. Database\Interest_Fix_(2).sql

Do NOT run FillTables.sql twice — it has no duplicate check.

### 6. Verify the schema matches

Run this against your DB:

    SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'InterestQuery';
    
    SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'DegreeInterestMapping';

Expected columns:
- InterestQuery: queryID, interestGroupID, questionText, weight, isActive
- DegreeInterestMapping: mappingID, degreeID, interestGroupID, weight

If InterestQuery shows **queryText** instead of **questionText**, run:

    EXEC sp_rename 'InterestQuery.queryText', 'questionText', 'COLUMN';

## Logins

| Role | Username | Password |
|------|----------|----------|
| Career Advisor | advisor1 | Advisor123! |
| Administrator  | admin1   | Admin123!   |
| Learner        | (leave blank) | (leave blank) |

For Learner access, leave username and password empty and pick "Learner" in
the role dropdown.

## Demo order

1. Login as Career Advisor (advisor1 / Advisor123!, role "Career Advisor").
2. Advisor menu → **Career Advisor Dashboard**. Shows the 5 learner sessions.
3. Click **Generate Reports**.
4. Report 2 (RPT-SUM-002): set a date range, click Generate Report, then
   Save as Text / PDF / Print.
5. Change the dropdown to Report 3 (RPT-EXC-001), Generate Report, Save.
6. Logout. Login as Learner (blank username, blank password, role "Learner").
7. Learner Profile → Next → Subject and Mark Entry → Next → Interest
   Questionnaire → Submit.
8. From the Advisory Outcome, open the Advisory Summary and save as Text.
   Show the ExportReport row in the database.

## Project structure

    Boundary/       Forms (login, learner flow, dashboard, reports, funding)
    Controller/     Business logic and validation
    DataAccess/     All SQL queries (the only layer that touches the DB)
    Entity/         Data classes (Learner, DegreeProgramme, FundOption, etc.)
    Database/       SQL scripts, plus FuturePath.mdf (not committed)

## Member ownership

- Member A: Database tables, learner data, learner forms (Profile, Subject and
  Mark Entry, Interest Questionnaire)
- Member B: Degrees, requirements, recommendation engine
- Member C: Login, main window, admin maintenance, interest mappings
- Member D: Funding, Advisory Summary, Career Advisor Dashboard, Reports 2 & 3

## Known issues

- Report 2 does not currently render the "Top Funding Category" and "Most
  Common Subject Gap" lines from the Part 2 design. Core session totals and
  the faculty/degree table work correctly.
- See Design_Changes_MemberD for the full list of deviations from Part 1.

## More information

- Design changes log: Design_Changes_MemberD.md
- The database must not be committed. Share FuturePath.mdf manually.