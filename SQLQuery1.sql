SELECT sessionID, degreeID, isAlternative, 
       LEN(ISNULL(gapObservation, '')) AS gap_length
FROM RecommendationResult
WHERE sessionID IN (2, 5)
ORDER BY sessionID, isAlternative;