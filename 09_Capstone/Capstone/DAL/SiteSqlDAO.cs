using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteSqlDAO
    {
        private string connectionString;
        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> TopFiveSites(int campgroundId, DateTime arrival, DateTime departure)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("Select Top 5 s.*, campground.daily_fee from site s join campground on s.campground_id = campground.campground_id WHERE s.campground_id = @campground_id AND s.site_id NOT IN(SELECT s.site_id FROM site s left JOIN reservation r ON s.site_id = r.site_id WHERE s.campground_id = @campground_id AND(@arrival >= from_date AND @arrival < to_date) OR(@departure <= to_date AND @departure > from_date))", connection);
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                    cmd.Parameters.AddWithValue("@arrival", arrival);
                    cmd.Parameters.AddWithValue("@departure", departure);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Site> topFiveSites = new List<Site>();
                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.CampgroundId = campgroundId;
                        site.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                        site.HasUtilities = Convert.ToBoolean(reader["utilities"]);
                        site.Id = Convert.ToInt32(reader["site_id"]);
                        site.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        topFiveSites.Add(site);
                    }
                    return topFiveSites;
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
