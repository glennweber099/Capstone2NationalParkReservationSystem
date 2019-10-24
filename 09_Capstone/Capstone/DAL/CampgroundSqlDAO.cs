using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    class CampgroundSqlDAO : ICampgroundSqlDAO
    {
        private string connectionString;
        public CampgroundSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public bool CheckAvailability(int campgroundId, DateTime arrival, DateTime departure)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM park WHERE campground_id = @campground_id AND open_from_mm <= @arrival AND open_to_mm >= @departure");
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                    cmd.Parameters.AddWithValue("@arrival", arrival.Month);
                    cmd.Parameters.AddWithValue("@departure", arrival.Month);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Campground> availableCampgrounds = new List<Campground>();
                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.Name = Convert.ToString(reader["name"]);
                        availableCampgrounds.Add(campground);
                    }
                    if (availableCampgrounds.Count > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public IList<Campground> GetCampgrounds(int parkId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM campground JOIN park ON campground.park_id = park.park_id WHERE campground.park_id = @parkId", connection);
                    cmd.Parameters.AddWithValue("@parkId", parkId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Campground> campgrounds = new List<Campground>();
                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.OpenMonth = Convert.ToInt32(reader["open_from_mm"]);
                        campground.CloseMonth = Convert.ToInt32(reader["open_to_mm"]);
                        campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                        campgrounds.Add(campground);
                    }
                    return campgrounds;
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
