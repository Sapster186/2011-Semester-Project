SELECT
 (SELECT COUNT(*) FROM [dbo].[GradeLevel]) AS GradeLevel,
 (SELECT COUNT(*) FROM [dbo].[Subject]) AS Subject,
 (SELECT COUNT(*) FROM [dbo].[Learner]) AS Learner,
 (SELECT COUNT(*) FROM [dbo].[LearnerSubject]) AS LearnerSubject,
 (SELECT COUNT(*) FROM [dbo].[InterestGroup]) AS InterestGroup,
 (SELECT COUNT(*) FROM [dbo].[InterestQuery]) AS InterestQuery,
 (SELECT COUNT(*) FROM [dbo].[LearnerInterestAnswer]) AS LearnerInterestAnswer;

