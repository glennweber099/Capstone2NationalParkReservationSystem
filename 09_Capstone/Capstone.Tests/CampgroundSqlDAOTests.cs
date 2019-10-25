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
    public class CampgroundSqlDAOTests
    {
        private TransactionScope transaction;
        const string connectionString = "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";
        private int newCampId = 0;
        private int parkId = 0;

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
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void TestGetCampgrounds()
        {
            // arrange
            CampgroundSqlDAO dao = new CampgroundSqlDAO(connectionString);

            // act
            IList<Campground> actualResult = dao.GetCampgrounds(parkId);

            // assert
            Assert.AreEqual(1, actualResult.Count);
            Assert.AreEqual("Camp Brandon", actualResult[0].Name);
            Assert.AreEqual(3, actualResult[0].OpenMonth);
            Assert.AreEqual(9, actualResult[0].CloseMonth);
            Assert.AreEqual(1000000, actualResult[0].DailyFee);
        }

        [TestMethod]
        public void TestCheckAvailability()
        {
            // arrange
            CampgroundSqlDAO dao = new CampgroundSqlDAO(connectionString);

            // act
            bool actualResult = dao.CheckAvailability(newCampId, Convert.ToDateTime("6/1/2019"), Convert.ToDateTime("7/1/2019"));
            bool actualResult2 = dao.CheckAvailability(newCampId, Convert.ToDateTime("1/1/2019"), Convert.ToDateTime("12/1/2019"));

            // assert
            Assert.AreEqual(true, actualResult);
            Assert.AreEqual(false, actualResult2);
        }
    }
}
