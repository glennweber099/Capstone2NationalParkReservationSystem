using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationSqlDAOTests
    {
        private TransactionScope transaction;
        const string connectionString = "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";
        private int newCampId = 0;
        private int parkId = 0;
        private int newSiteId = 0;
        private int newReservationId = 0;

        [TestInitialize]
        public void Setup()
        {
            // Begin a transaction
            this.transaction = new TransactionScope();
            string script;
            // Load a script file to setup the db the way we want it
            using (StreamReader sr = new StreamReader("..//..//..//test_setup.sql"))
            {
                script = sr.ReadToEnd();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(script, conn);

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    newCampId = Convert.ToInt32(rdr["NewCamp"]);
                    parkId = Convert.ToInt32(rdr["NewPark"]);
                    newSiteId = Convert.ToInt32(rdr["NewSite"]);
                    newReservationId = Convert.ToInt32(rdr["NewReservation"]);
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void MakeReservation()
        {
            //Arrange
            ReservationSqlDAO dao = new ReservationSqlDAO(connectionString);
            Reservation reservation = new Reservation();
            reservation.FromDate = new DateTime(2019, 5, 11);
            reservation.ToDate = new DateTime(2019, 5, 13);
            reservation.Name = "Glenn";
            reservation.SiteId = newSiteId;
            
            //Act
            int actualResult = dao.MakeReservation(reservation);

            //Assert
            Assert.AreEqual(newReservationId + 1, actualResult);
        }
    }
}
