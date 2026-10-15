using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.DataAccess
{
    // The ONLY class allowed to talk to the DegreeInterestMapping table directly.
    public class DegreeInterestMappingDA
    {
        // SQL uses degreeID; the existing entity property is DegreeProgrammeID.
        // Gets every mapping row.
        public List<DegreeInterestMapping> GetAllMappings()
        {
            List<DegreeInterestMapping> results = new List<DegreeInterestMapping>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM DegreeInterestMapping";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DegreeInterestMapping m = new DegreeInterestMapping();
                    m.MappingID = (int)reader["mappingID"];
                    m.DegreeProgrammeID = (int)reader["degreeID"];
                    m.InterestGroupID = (int)reader["interestGroupID"];
                    m.Weight = Convert.ToSingle(reader["weight"]);
                    results.Add(m);
                }

                reader.Close();
            }

            return results;
        }

        // Gets all mappings for a specific degree programme.
        public List<DegreeInterestMapping> GetMappingsByDegree(int degreeProgrammeID)
        {
            List<DegreeInterestMapping> results = new List<DegreeInterestMapping>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM DegreeInterestMapping WHERE degreeID = @degreeProgrammeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@degreeProgrammeID", degreeProgrammeID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DegreeInterestMapping m = new DegreeInterestMapping();
                    m.MappingID = (int)reader["mappingID"];
                    m.DegreeProgrammeID = (int)reader["degreeID"];
                    m.InterestGroupID = (int)reader["interestGroupID"];
                    m.Weight = Convert.ToSingle(reader["weight"]);
                    results.Add(m);
                }

                reader.Close();
            }

            return results;
        }

        // Inserts a new mapping between a degree and an interest group.
        public void AddMapping(DegreeInterestMapping m)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO DegreeInterestMapping (degreeID, interestGroupID, weight)
                                 VALUES (@degreeProgrammeID, @interestGroupID, @weight)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@degreeProgrammeID", m.DegreeProgrammeID);
                cmd.Parameters.AddWithValue("@interestGroupID", m.InterestGroupID);
                cmd.Parameters.AddWithValue("@weight", m.Weight);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Updates an existing mapping.
        public void UpdateMapping(DegreeInterestMapping m)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"UPDATE DegreeInterestMapping SET
                                 degreeID = @degreeProgrammeID,
                                 interestGroupID = @interestGroupID,
                                 weight = @weight
                                 WHERE mappingID = @mappingID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@mappingID", m.MappingID);
                cmd.Parameters.AddWithValue("@degreeProgrammeID", m.DegreeProgrammeID);
                cmd.Parameters.AddWithValue("@interestGroupID", m.InterestGroupID);
                cmd.Parameters.AddWithValue("@weight", m.Weight);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
