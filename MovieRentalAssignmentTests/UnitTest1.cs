using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MovieRentalAssignmentTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void VerifyConnection()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\MovieRentalAssignment\db.mdf;Integrated Security=True");
            
            if(con.State == ConnectionState.Closed)
            {
                con.Open();
                Assert.IsTrue(true);
            }
            else if(con.State == ConnectionState.Open)
            {
                Assert.IsTrue(true);
            }
            con.Close();
        }

        [TestMethod]
        public void FetchData()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\MovieRentalAssignment\db.mdf;Integrated Security=True");
            SqlCommand cmd;
            SqlDataAdapter adp;
            
            string Query = "SELECT *, ISNULL((SELECT COUNT(id) FROM RentedMovies WHERE custidfk = Customer.id), 0) AS RentedMovies FROM Customer ORDER BY RentedMovies DESC";
            cmd = new SqlCommand(Query, con);
            DataTable table = new DataTable();
            adp = new SqlDataAdapter(cmd);
            adp.Fill(table);

            Assert.IsTrue(table.Rows.Count > -1);
        }
    }
}
