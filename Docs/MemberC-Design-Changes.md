# Member C design changes and integration decisions

## Changes compared with the Part 1 design

| Change | Implementation and reason |
| --- | --- |
| Additional LoginController | The Part 1 class diagram assigned AdminController but did not include this separate login controller. LoginController now owns account verification and the shared username/role session, keeping authentication out of the form. |
| Anonymous learner entry | Selecting Learner creates a Learner role session with a null CurrentUsername and no credential/database lookup. Staff roles still require an active matching UserAccount. Logout clears role, username and learner session state. This implements TC14. |

## Confirmed naming and compatibility decisions

- The current `CreateTables.sql` defines `DegreeInterestMapping.degreeID`. Member C's DA previously used `degreeProgrammeID` in its SQL. Reads, the degree filter, inserts and updates now use `degreeID`; public methods and the `DegreeProgrammeID` entity property are unchanged. This corrects a mismatch rather than changing the table design.
- The current `CreateTables.sql`, `AlterLearnerTables.sql`, Member A's `LookupDA` and the generated dataset use `InterestQuery.questionText`. The migration explicitly converts the older `queryText` column to `questionText`. Member C's seed was the remaining SQL mismatch and was aligned with those checked-in files. The C# property remains `QueryText`. No table or migration owned by another member was edited, and no column was renamed by this work. A separately maintained database that still uses `queryText` needs schema agreement before these seeds run.
- Staff passwords retain the project's existing direct comparison with `passHash`; no hashing scheme or password migration was introduced. The name of the column is not evidence of hashing. The new seed credentials are demo data.

## Integration limits

- DegreeProgrammeDA and DegreeRequirementsDA currently provide reads only. Member C's maintenance form therefore shows those records but disables writes until Member B supplies the required methods. DegreeRequirements also has no active flag; deactivation needs an agreed design.
- Funding maintenance calls the existing Member D methods. GetActiveFundOptions does not list expired/inactive records, so those cannot be reopened for editing here.
- Member A consumes Member C's questions; Member B consumes Member C's questions, groups and degree-interest mappings for interest scoring. The corresponding seeds and DA correction are supplied. Their execution and end-to-end verification require the local team database, which is not included in this clone.
