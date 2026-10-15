# Member C test cases

## Current verification status

The solution builds successfully. The automated checks cover the areas listed below; the latest execution result is recorded at the end of this document. These checks cover controller validation, authorization, session state and form control/menu configuration. They do not establish that database operations or full interactive workflows pass. Staff sessions are simulated in automated tests; real account authentication remains pending.

Run `Tests\Run-MemberCTests.ps1` in PowerShell to build and run the automated checks. Generated executables are written to the ignored bin folder.

## Assigned test cases

| Test case | Function | Pre-condition / data | Expected outcome | Verification |
| --- | --- | --- | --- | --- |
| 1 | Career Advisor login | Local team database seeded with advisor1 / Advisor123! (for a newly inserted account). Select Career Advisor and enter those credentials. | MainWindow opens; Advisor menu is shown; Admin menu is hidden. Wrong password, wrong role and inactive accounts are refused. | Input validation and advisor menu tested automatically. Real credential verification is blocked by the missing database. |
| 10 | Access denied | Anonymous Learner or Career Advisor session. Attempt admin operations, including direct controller calls. | Admin navigation hidden; every admin read throws UnauthorizedAccessException; every save/deactivate returns access denied before database access. | Automated for logged-out, learner and simulated advisor sessions. Real advisor login remains pending. |
| 14 | Learner access | No username/password; no database needed for entry. Select Learner. | Continue enabled; credential boxes disabled; anonymous learner session and Learner menu; no admin access. | Automated for controller entry, input controls and menu visibility. Profile/marks persistence belongs to the database-dependent learner flow. |
| C-15 | Validation | Simulated Administrator session. Blank/oversized question, missing group, zero/negative/NaN/infinite weight; blank funding/type, invalid IDs. | Validation error before database access; no write occurs. | Automated. |
| C-16 | Session reset | Existing staff role or learner ID, then logout or failed login. | Old role/username cleared. Learner ID reset. Another learner cannot inherit the previous learner's ID. | Automated for controller state. Manually verify the full close/logout UI after the database is available. |

## Pending integration checks

Once a local test database with the agreed team schema and sample data is available:

1. Run case 1 with real active advisor and administrator credentials. Also check wrong password, wrong role and inactive account.
2. Login as Administrator. Open Admin Dashboard then Reference Data Maintenance. Try question add/edit/deactivate. Confirm updates after Refresh and in Member A's learner questionnaire. Cancel an edit and confirm nothing changes.
3. Funding: add an option with an existing type and future closing date, edit its fields and deactivate it. Cancel a deactivation first and verify it stays. Member D's reader only lists active options whose closing date has not passed, so a deactivated/expired option disappears from this maintenance list too.
4. Learner: Continue with blank credentials; finish profile, marks and questionnaire, then use Check Eligibility. It must use LearnerController.CurrentLearnerID, never a fixed demo learner. Log out and repeat with another learner.
5. Close the main window and use Logout; both should return to a cleared login screen. Exit should end the application. Verify any cancellation prompt in another member's child form can still cancel closure.
6. Beta-test Member D's funding filters, advisor search, reports and exports with real data. These were not changed or claimed as passed here.


## Member C seed prerequisites

On a fresh local database, use the current team table script first. Run Member C's `Database/InterestQuestionnaire_Seed.sql` before `Database/FillTables.sql`, because Member A's learner answers expect the ten questionnaire rows in their original order. Then run `Database/MemberC_Accounts_And_Mappings_Seed.sql` after the degrees are loaded. Member D's funding script is unchanged and is also needed for funding integration tests. These instructions do not create a database or change a connection string.

New demo accounts:

| Username | Password | Role |
| --- | --- | --- |
| admin1 | Admin123! | Administrator |
| advisor1 | Advisor123! | Career Advisor |

The script never resets an existing password. If an account already exists, use its existing password. If either username has a conflicting role or is inactive, the seed stops and rolls back rather than altering that account.

The current schema uses `InterestQuery.queryID`, `InterestQuery.questionText` and `DegreeInterestMapping.degreeID`. `InterestQuery.QueryText` and `DegreeInterestMapping.DegreeProgrammeID` remain the existing C# property names. No database columns were renamed in this work.

To verify the seeds with SQL Server once the local database is available:

1. Run both Member C seeds in the documented order, then inspect their output. The accounts must be active with the expected roles; every active degree must have a valid interest group and positive weight.
2. Run them again. Counts for groups, questions, accounts and mapping pairs must stay the same, and existing passwords and weights must remain unchanged. On the current fresh ten-degree dataset, expect five groups, ten questions, two demo accounts and ten mapping pairs.
3. Test a partially populated database: missing Member C rows should be added without duplicating existing rows. Existing identities are looked up rather than reset. If question IDs differ from Member A's fixed 1-10 answer references, Member A must check those answer links; this script never rewrites their learner answers.
4. Test TC1 with advisor1 and its password; verify CurrentUsername and CurrentUserRole. Try a wrong password and an Administrator role selection for advisor1. Both must fail and leave no staff session.
5. Test TC10 after a real advisor login, then TC14 with empty credentials. Verify logout clears both role and username and the saved learner ID.

These SQL execution and real-login checks remain pending while the configured local MDF is absent. Successful compilation, automated form/controller checks and SQL parsing do not substitute for them.

## Latest verification result

- Solution build: passed; no build errors reported.
- Member C automated checks: 93 passed, including TC1 input controls, TC10 exact authorization failures, TC14 anonymous entry, all three role menus, MDI child placement/reuse and logout state.
- SQL Server syntax parser: both Member C seed scripts and all four DegreeInterestMappingDA statements passed.
- Static schema/data checks: seed INSERT columns match CreateTables.sql; the ten mapping names match all ten current active degree definitions in FillTables.sql; all mapped group names are seeded; existing public C# names are preserved.
- Scope check: exactly the six planned Member C files differ from the snapshot taken before this pass. Other members' source files and schema scripts are unchanged.
- Not run: seed execution, actual database re-run/idempotency checks, real staff account authentication and database maintenance. Database/FuturePath.mdf is still absent. These remain integration tests rather than claimed passes.
