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

                    SqlCommand cmd = new SqlCommand("SELECT TOP 5 site.site_id FROM site " +
                        "JOIN reservation ON site.site_id = reservation.site_id " +
                        "WHERE campground_id = @campground_id AND (@departure < reservation.from_date OR @arrival > reservation.to_date)" +
                        "GROUP BY site.site_id");
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                    cmd.Parameters.AddWithValue("@arrival", arrival);
                    cmd.Parameters.AddWithValue("@departure", departure);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Site> topFiveSites = new List<Site>();
                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.CampgroundId = campgroundId;
                        site.DailyFee = Convert.ToDecimal(reader["campground.daily_fee"]);
                        site.HasUtilities = Convert.ToBoolean(reader["site.utilities"]);
                        site.Id = Convert.ToInt32(reader["site.site_id"]);
                        site.IsAccessible = Convert.ToBoolean(reader["site.accessible"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["site.max_occupancy"]);
                        site.MaxRVLength = Convert.ToInt32(reader["site.max_rv_length"]);
                        site.SiteNumber = Convert.ToInt32(reader["site.site_number"]);
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
