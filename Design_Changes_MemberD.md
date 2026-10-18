'===============================================================================
MEMBER D — DESIGN CHANGES LOG
Future Path Student Advising System
Compiled: 16 October 2026
===============================================================================

This document records every deviation from Part 1 (the Systems Design
Specification) that Member D made while building the Funding, Advisory Summary,
Career Advisor Dashboard, and Reports 2 and 3 section.

===============================================================================
1. ARCHITECTURE CHANGES FROM PART 1
===============================================================================

1.1 Funding lookup placed in ReportController

Part 1: The class diagram defines four controllers and none for funding.
Built:  The funding lookup lives in ReportController.
Reason: No funding controller exists in Part 1's design. The lookup was placed
        alongside the reporting logic since both serve the advisor pipeline.

1.2 Report queries consolidated in a ReportDA class

Part 1: The package diagram includes a ReportDA class in the Data Access layer.
Built:  All summary, dashboard, funding-filter and report queries live in
        ReportDA. Basic CRUD and deactivate methods for funding remain in
        FundOptionDA, FundFacultyDA, FundProgrammeDA and ExportReportDA.
Reason: Keeps SQL out of the controller. Preserves the golden flow:
        Boundary → Controller → Data Access → Database.

1.3 Form naming

Part 1: LearnerAdvisorySummaryForm, ReadOnlyAdvisorySummaryForm,
        ReportSelectionGenerationForm.
Built:  AdvisorySummaryForm, ReadOnlyAdvisorySummaryForm, ReportSelectionForm.
Reason: Same screens, shorter names.

1.4 Report 2 (RPT-SUM-002) layout

Part 1: Executive summary with totals, percentages, top funding category,
        most common gap, and a table grouped by faculty.
Built:  Session totals with percentage breakdown per outcome, followed by a
        table grouped by Faculty (GROUP rows) with degrees sorted by Total
        Selections descending. An extra "More Info Needed" column was added so
        each row sums to the total number of sessions.
Reason: Makes each faculty row internally consistent.
Note:   Two Part-2 lines are not rendered (see section 5.4).

1.5 Report 3 (RPT-EXC-001) content

Part 1: Learner name, grade, degree, outcome, identified gaps and session date,
        grouped by grade with subtotals and a grand total, exceptions in red.
Built:  As specified. Identified gaps come from
        RecommendationResult.gapObservation. Only "Not Currently Eligible" and
        "More Information Needed" outcomes are listed.
Reason: Implements the report design exactly.

1.6 Role-based access control on Reports 2 and 3

Part 1: Reports 2 and 3 are restricted to logged-in Career Advisors and
        Administrators.
Built:  CareerAdvisorDashboardForm and ReportSelectionForm receive the logged-in
        role as a parameter. ReportController checks the role again before
        returning data.
Reason: Enforces access control in both the presentation layer and the
        business logic layer.

===============================================================================
2. RUNTIME BEHAVIOUR CHANGES
===============================================================================

2.1 Export options for the Advisory Summary

Part 1: Report 1 must be saved, printed or exported (PDF).
Built:  Save as Text, Save as PDF (via Windows "Microsoft Print to PDF"), and
        Print. Each action is recorded in ExportReport with the file format
        (TXT, PDF, PRINT), the date, and the full text of the summary. The
        session cannot be closed until one of them has been used.
Reason: Covers every route listed in the specification and produces an audit
        trail.

2.2 Requirement comparison in Report 1

Part 1: The report compares the learner's marks with the stored requirements
        for the selected degree.
Built:  Under the eligibility outcome, each requirement is listed with its
        minimum mark and the learner's mark, marked as met or below minimum.
        APS is shown per subject only, not as a total, matching the Part 2
        layout.

2.3 Interest alignment wording

Part 1: The report shows wording such as "Strong alignment with research and
        sciences".
Built:  Wording generated from the interest score: 70+ is "Strong", 40–69 is
        "Moderate", below 40 is "Low". The best-matching interest group for the
        degree is appended when a mapping exists.
Reason: The specification gives the wording but not the thresholds. 70/40
        chosen as balanced cut-offs.

2.4 Outcome always paired with its explanation

Part 1: An outcome must never be shown as a bare status label.
Built:  Every outcome is shown with its gap observation. For a Strong Match
        with no gap recorded, the text reads "You meet the requirements for
        this degree." The Career Advisor Dashboard also shows the gap for each
        session.

2.5 Funding filters and sample-data flag

Part 1: Funding is filtered by faculty, degree and type. Funding may apply to
        faculties, degrees, or both.
Built:  Funding linked to a whole faculty also appears for that faculty's
        degrees. The type list is loaded from the database. The Funding
        Information Form can open pre-filtered via
        FundingInformationForm(facultyID, degreeID). Every option is flagged
        "Sample information - verify with institution". Inactive options and
        options past their closing date are excluded.
Reason: Matches the funding link tables in the ERD and test case 8.

===============================================================================
3. IMPLEMENTATION AND ENVIRONMENT CHANGES
===============================================================================

3.1 Database standardised on FuturePath.mdf (attached file)

Part 1: Per-part setup allowed either a named DB on LocalDB or a file attach.
        The brief says not to commit the .mdf (share as a script).
Built:  Group decision (15 Oct) to standardise on a file-attach connection:
            AttachDbFilename=|DataDirectory|\Database\FuturePath.mdf
        Implemented via Member B's DatabaseHelper.cs. Every developer needs
        Database\FuturePath.mdf in the project folder.
Reason: One connection string across all laptops. Avoids the "which DB am I
        on?" confusion that caused multiple bugs during development.
Impact: Member D's local FuturePathDB was detached, copied into the project
        folder as Database\FuturePath.mdf and Database\FuturePath_log.ldf, and
        reattached by the app. No data loss.

3.2 .csproj — CopyToOutputDirectory set to PreserveNewest

Built:  Changed the CopyToOutputDirectory for FuturePath.mdf from Always to
        PreserveNewest.
Reason: Always causes MSBuild to overwrite the .mdf on every build, which
        fails while SQL Server holds the file open. PreserveNewest skips the
        copy when the destination is unchanged.

3.3 .csproj — FuturePath_log.ldf reference removed

Built:  The <Content Include="FuturePath_log.ldf"> block was removed.
Reason: A stale .ldf from a previous detach caused "auto-named database" errors
        on attach. SQL Server rebuilds the log file automatically.

3.4 InterestQuery column name aligned to questionText

Built:  On Member D's DB:
            EXEC sp_rename 'InterestQuery.queryText', 'questionText', 'COLUMN';
Reason: Member A's CreateTables.sql, Member C's InterestQueryDA.cs and the
        DataSet designer all reference questionText. The mismatch caused an
        unhandled exception ("Invalid column name 'questionText'") when the
        learner flow opened the Interest Questionnaire.
Impact: Not a Member D code change. Logged for cross-machine consistency.
        Every developer's DB must have questionText.

3.5 Database name displayed as full path in SQL Server Object Explorer

Observed: AttachDbFilename without an Initial Catalog causes SQL Server to
          register the DB under a name derived from the full file path.
Impact:   Cosmetic. The app connects by file path, not by database name, so
          functionality is unaffected. "USE FuturePath;" fails against the
          path-named DB.
Fix (deferred): Add "Initial Catalog=FuturePath;" to DatabaseHelper.cs after
          the AttachDbFilename clause. Requires detaching the path-named DB
          first. Deferred to avoid disrupting the working setup before the
          demo.

===============================================================================
4. TEST DATA
===============================================================================

4.1 Session data

Built:  All 5 named learners (Gadija Jacobs, Aadiel-Raboo Magwa, Labeeqah
        Rademeyer, Siyabonga Dickson, Derek Vin Diesel) were run through
        Member B's recommendation engine after B's interest-score fix.
        Sessions 2009–2013, one per learner, correct outcomes and non-zero
        interest scores.
Reason: The implementation plan shares session-data creation between members.
        Running the real flow is preferable to a scripted fallback because it
        exercises the actual pipeline.

4.2 Seed data

FundOption: 8 rows (1 inactive, 1 past closeDate).
FundFaculty: 9 rows linking funds to faculties.
FundProgramme: 2 rows linking funds to specific degrees.
DegreeInterestMapping: 11 rows (B's standard).
LearnerInterestAnswer: 50 rows (5 learners × 10 questions).
UserAccount: advisor1 (Career Advisor), admin1 (Administrator).

===============================================================================
5. KNOWN GAPS AND PENDING VERIFICATION
===============================================================================

5.1 Report 2 lines not rendering
    The Part 2 design specifies "Top Funding Category" and "Most common subject
    gap" on Report 2. Neither is currently rendered. Core aggregation, grouping
    and sorting work. Deferred as a future enhancement (see 5.4).

5.2 RBAC negative case
    The positive case (Career Advisor can see Reports 2 and 3) has been tested.
    The negative case (Learner tries to reach Report Selection and is refused)
    has not been exercised end-to-end.

5.3 FuturePath.mdf path consistency
    Every teammate's FuturePath.mdf must live at [project]\Database\ to match
    the .csproj and B's connection string.

5.4 Report 2 — Top Funding Category and Most Common Subject Gap
    The data behind these two lines exists in the DB (FundOption,
    DegreeInterestMapping, RecommendationResult), but the rendering is not
    implemented. The report still passes test case 12 (session totals,
    outcome percentages, faculty/degree table all correct).

    Reason for not fixing before the demo: risk of introducing new bugs at
    the end of the project, with Report 2 already passing its test case.
    Deferred as a future enhancement.

    Status: Known gap. Not a functional bug.

===============================================================================
END OF DOCUMENT
===============================================================================