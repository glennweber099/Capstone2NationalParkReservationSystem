using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationSqlDAO
    {
        private string connectionString;
        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public int MakeReservation(Reservation reservation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("Insert Into reservation(site_id, name, from_date, to_date, create_date) Values (@siteId, @name, @fromDate, @toDate, @createDate); Select @@Identity", connection);
                    cmd.Parameters.AddWithValue("@siteId", reservation.SiteId);
                    cmd.Parameters.AddWithValue("@name", reservation.Name);
                    cmd.Parameters.AddWithValue("@fromDate", reservation.FromDate);
                    cmd.Parameters.AddWithValue("@toDate", reservation.ToDate);
                    cmd.Parameters.AddWithValue("@createDate", DateTime.Now);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
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
