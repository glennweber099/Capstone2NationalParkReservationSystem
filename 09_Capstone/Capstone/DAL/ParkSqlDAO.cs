using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkSqlDAO : IParkSqlDAO
    {
        private string connectionString;
        public ParkSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Park GetPark(int parkId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM park WHERE park_id = @park_id", connection);
                    cmd.Parameters.AddWithValue("@park_id", parkId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Park park = new Park();
                    while (reader.Read())
                    {
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);
                    }
                    return park;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public IList<Park> GetParks()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM park", connection);

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Park> parks = new List<Park>();
                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishDate = Convert.ToDateTime(reader["establish_date"]).Date;
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);
                        parks.Add(park);
                    }
                    return parks;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
