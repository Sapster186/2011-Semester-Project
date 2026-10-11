using System;
using System.Collections.Generic;
using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.Controller
{
    // this is the recommendation engine, compares a learner's subjects and marks against
    // a degree's requirements, scores them, and decides the outcome.
    public class AdviceController
    {
        private AdviceSessionDA adviceSessionDA = new AdviceSessionDA();
        private RecommendationResultDA recommendationResultDA = new RecommendationResultDA();

        // The four outcome labels the whole system uses
        public const string STRONG_MATCH = "Strong Match";
        public const string POSSIBLE_MATCH = "Possible Match";
        public const string NOT_ELIGIBLE = "Not Currently Eligible";
        public const string MORE_INFO_NEEDED = "More Information Needed";

        // ---------- ELIGIBILITY CHECK ----------
        // runs one full check for a learner against a single degree.
        public AdviceSession CheckEligibility(Learner learner,
                                              List<LearnerSubject> learnerSubjects,
                                              DegreeProgramme degree,
                                              List<DegreeRequirements> requirements)
        {
            AdviceSession session = new AdviceSession();
           session.learnerID = learner.learnerID;
          session.degreeID = degree.degreeID;
            session.dateOfSession = DateTime.Now;

//common null check
            if (learnerSubjects == null || learnerSubjects.Count == 0 ||
                requirements == null || requirements.Count == 0)
            {
                session.eligibleResult = MORE_INFO_NEEDED;
                session.overallScore = 0;
                return session;
            }

           double subjectScore = CalculateSubjectScore(learnerSubjects, requirements);
            double markScore = CalculateMarkMatchScore(learnerSubjects, requirements);
            double interestScore = CalculateInterestScore(learner, degree);
            double facultyScore = CalculateFacultyPreferenceScore(learner, degree);

            session.overallScore = CalculateScore(subjectScore, markScore, interestScore, facultyScore);

            string gap;
            session.eligibleResult = DetermineOutcome(subjectScore, markScore, session.overallScore, out gap);

            return session;
        }

        // repeat as above but returns the gap text too (used when saving recommendations)
        public AdviceSession CheckEligibilityWithGap(Learner learner,
                                                     List<LearnerSubject> learnerSubjects,
                                                     DegreeProgramme degree,
                                                     List<DegreeRequirements> requirements,
                                                     out string gapText,
                                                     out double subjectScore,
                                                     out double markScore,
                                                     out double interestScore,
                                                     out double facultyScore)
        {
             subjectScore = 0;
            markScore = 0;
            interestScore = 0;
            facultyScore = 0;
            gapText = "";

            AdviceSession session = new AdviceSession();
            session.learnerID = learner.learnerID;
            session.degreeID = degree.degreeID;
            session.dateOfSession = DateTime.Now;

            if (learnerSubjects == null || learnerSubjects.Count == 0 ||
                requirements == null || requirements.Count == 0)
            {
                session.eligibleResult = MORE_INFO_NEEDED;
                session.overallScore = 0;
                return session;
            }

            subjectScore = CalculateSubjectScore(learnerSubjects, requirements);
            markScore = CalculateMarkMatchScore(learnerSubjects, requirements);
            interestScore = CalculateInterestScore(learner, degree);
            facultyScore = CalculateFacultyPreferenceScore(learner, degree);

            session.overallScore = CalculateScore(subjectScore, markScore, interestScore, facultyScore);
            session.eligibleResult = DetermineOutcome(subjectScore, markScore, session.overallScore, out gapText);

            return session;
        }

        // ---------- SCORING ----------
        // percentage of required subjects the learner actually has
        public double CalculateSubjectScore(List<LearnerSubject> learnerSubjects,
                                            List<DegreeRequirements> requirements)
        {
            int requiredCount = 0;
            int matched = 0;

            foreach (DegreeRequirements req in requirements)
            {
                if (req.isNeeded)
                {
                    requiredCount++;
                    foreach (LearnerSubject ls in learnerSubjects)
                    {
                        if (ls.subjectID == req.subjectID)
                        {
                            matched++;
                            break;
                        }
                    }
                }
            }

            if (requiredCount == 0) return 0;
            return ((double)matched / requiredCount) * 100;
        }

        // how well the learner's marks meet the minimums
        public double CalculateMarkMatchScore(List<LearnerSubject> learnerSubjects,
                                              List<DegreeRequirements> requirements)
        {
            double totalScore = 0;
            int count = 0;

            foreach (DegreeRequirements req in requirements)
            {
                if (req.isNeeded)
                {
                    count++;
                    foreach (LearnerSubject ls in learnerSubjects)
                    {
                        if (ls.subjectID == req.subjectID)
                        {
                            if (ls.mark >= req.minMark)
                                totalScore += 100;
                            else if (req.minMark > 0)
                                totalScore += (ls.mark / req.minMark) * 100;
                            break;
                        }
                    }
                }
            }

            if (count == 0) return 0;
            return totalScore / count;
        }

        // Interest alignment. Placeholder for now: neutral 50 until Member A/C's interest DAs are in.
        public double CalculateInterestScore(Learner learner, DegreeProgramme degree)
        {
            // Will be replaced once LearnerInterestAnswerDA and DegreeInterestMappingDA are ready
            return 50;
        }

        // Bonus if the learner's preferred faculty matches this degree's faculty
        public double CalculateFacultyPreferenceScore(Learner learner, DegreeProgramme degree)
        {
            // No preference field on Learner yet, so no bonus for now
            return 0;
        }

        // Weighted combine: subjects 40%, marks 40%, interest 15%, faculty 5%
        public double CalculateScore(double subjectScore, double markScore,
                                     double interestScore, double facultyScore)
        {
            return (subjectScore * 0.4) + (markScore * 0.4) +
                   (interestScore * 0.15) + (facultyScore * 0.05);
        }

        // converts a mark to an APS point on the standard 1-7 scale
        public int MarkToAPS(double mark)
        {
            if (mark >= 80) return 7;
            if (mark >= 70) return 6;
            if (mark >= 60) return 5;
            if (mark >= 50) return 4;
            if (mark >= 40) return 3;
            if (mark >= 30) return 2;
            return 1;
        }

        // ---------- OUTCOME ----------
        // decides the outcome and writes gap text if there is one
        public string DetermineOutcome(double subjectScore, double markScore,
                                       double overallScore, out string gapObservation)
        {
            gapObservation = "";

            if (subjectScore < 100)
            {
                gapObservation = "You are missing one or more required subjects for this degree.";
                return NOT_ELIGIBLE;
            }

            if (markScore < 100)
            {
                gapObservation = "One or more of your marks is below the required minimum for this degree.";
                return POSSIBLE_MATCH;
            }

            if (overallScore >= 75)
                return STRONG_MATCH;

            return POSSIBLE_MATCH;
        }

        // repeated as above but produces a specific gap
        public string BuildDetailedGapText(List<LearnerSubject> learnerSubjects,
                                           List<DegreeRequirements> requirements)
        {
            foreach (DegreeRequirements req in requirements)
            {
                if (!req.isNeeded) continue;

                bool found = false;
                foreach (LearnerSubject ls in learnerSubjects)
                {
                    if (ls.subjectID == req.subjectID)
                    {
                        found = true;
                        if (ls.mark < req.minMark)
                        {
                            return "Subject " + req.subjectID + ": your mark of " + ls.mark +
                                   "% is below the minimum of " + req.minMark + "%.";
                        }
                        break;
                    }
                }
                if (!found)
                    return "You are missing a required subject (subjectID " + req.subjectID + ").";
            }
            return "";
        }

        // ---------- GENERATE RECOMMENDATIONS ----------
        // Scores every active degree, keeps the matches, sorts best-first
        public List<RecommendationResult> GenerateRecommendations(Learner learner,
                                                                  List<LearnerSubject> learnerSubjects,
                                                                  List<DegreeProgramme> allDegrees,
                                                                  List<DegreeRequirements> allRequirements)
        {
            List<RecommendationResult> results = new List<RecommendationResult>();

            foreach (DegreeProgramme degree in allDegrees)
            {
                if (!degree.isActive) continue;

                // Requirements for this degree only
                List<DegreeRequirements> degreeReqs = new List<DegreeRequirements>();
                foreach (DegreeRequirements req in allRequirements)
                    if (req.degreeID == degree.degreeID)
                        degreeReqs.Add(req);

                if (degreeReqs.Count == 0) continue;

                double subjectScore = CalculateSubjectScore(learnerSubjects, degreeReqs);
                double markScore = CalculateMarkMatchScore(learnerSubjects, degreeReqs);
                double interestScore = CalculateInterestScore(learner, degree);
                double facultyScore = CalculateFacultyPreferenceScore(learner, degree);
                double overall = CalculateScore(subjectScore, markScore, interestScore, facultyScore);

                string gap;
                string outcome = DetermineOutcome(subjectScore, markScore, overall, out gap);

                if (outcome == STRONG_MATCH || outcome == POSSIBLE_MATCH)
                {
                    RecommendationResult r = new RecommendationResult();
                    r.sessionID = 0;
                    r.degreeID = degree.degreeID;
                    r.subjectMatchScore = subjectScore;
                    r.markMatchScore = markScore;
                    r.interestMatchScore = interestScore;
                    r.desiredFacultyScore = facultyScore;
                    r.overallScore = overall;
                    r.isAlternative = true;
                    r.gapObservation = gap;
                    results.Add(r);
                }
            }

            // Highest score first
            results.Sort((a, b) => b.overallScore.CompareTo(a.overallScore));
            return results;
        }

        // ---------- SAVE ----------
        // saves the session and returns its new sessionID
        public int SaveSession(AdviceSession session)
        {
            return adviceSessionDA.AddSession(session);
        }

        public void SaveRecommendations(List<RecommendationResult> results)
        {
            foreach (RecommendationResult r in results)
                recommendationResultDA.AddRecommendation(r);
        }
    }
}