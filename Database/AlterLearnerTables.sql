
-- 1. Old column name queryText -> questionText (only if the old name is still there)
IF COL_LENGTH('dbo.InterestQuery', 'queryText') IS NOT NULL
   AND COL_LENGTH('dbo.InterestQuery', 'questionText') IS NULL
BEGIN
    EXEC sp_rename 'dbo.InterestQuery.queryText', 'questionText', 'COLUMN';
END
GO

-- 2. Add isActive (existing rows become active)
IF COL_LENGTH('dbo.InterestQuery', 'isActive') IS NULL
BEGIN
    ALTER TABLE [dbo].[InterestQuery]
        ADD [isActive] BIT NOT NULL
        CONSTRAINT [DF_InterestQuery_isActive] DEFAULT 1;
END
GO

-- 3. Check: should list queryID, interestGroupID, questionText, weight, isActive
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'InterestQuery'
ORDER BY ORDINAL_POSITION;