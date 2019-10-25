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
    public class ParkSqlDAOTests
    {
        private TransactionScope transaction;
        const string connectionString = "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";
        private int newParkId = 0;

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
                    newParkId = Convert.ToInt32(rdr["NewPark"]);
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void TestGetPark()
        {
            // arrange
            ParkSqlDAO dao = new ParkSqlDAO(connectionString);

            // act
            Park actualResult = dao.GetPark(newParkId);

            // assert
            Assert.AreEqual("Tech Elevator National Park", actualResult.Name);
            Assert.AreEqual("Cleveland, OH", actualResult.Location);
            Assert.AreEqual(Convert.ToDateTime("06/01/2016"), actualResult.EstablishDate);
            Assert.AreEqual(10, actualResult.Area);
            Assert.AreEqual(120, actualResult.Visitors);
            Assert.AreEqual("The best school in the world", actualResult.Description);
        }

        [TestMethod]
        public void TestGetParks()
        {
            // arrange
            ParkSqlDAO dao = new ParkSqlDAO(connectionString);

            // act
            IList<Park> actualResult = dao.GetParks();

            // assert
            Assert.AreEqual(1, actualResult.Count);
            Assert.AreEqual("Tech Elevator National Park", actualResult[0].Name);
            Assert.AreEqual("Cleveland, OH", actualResult[0].Location);
            Assert.AreEqual(Convert.ToDateTime("06/01/2016"), actualResult[0].EstablishDate);
            Assert.AreEqual(10, actualResult[0].Area);
            Assert.AreEqual(120, actualResult[0].Visitors);
            Assert.AreEqual("The best school in the world", actualResult[0].Description);
        }
    }
}
