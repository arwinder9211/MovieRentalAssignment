using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieRentalAssignment
{
    public partial class Reporting : Form
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter adp;
        private string connstr;

        public Reporting()
        {
            InitializeComponent();
            connstr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            con = new SqlConnection(connstr);
        }

        private void Reporting_Load(object sender, EventArgs e)
        {
            string Query = "SELECT *, ISNULL((SELECT COUNT(id) FROM RentedMovies WHERE custidfk = Customer.id), 0) AS RentedMovies FROM Customer ORDER BY RentedMovies DESC";
            cmd = new SqlCommand(Query, con);
            DataTable table = new DataTable();
            adp = new SqlDataAdapter(cmd);
            adp.Fill(table);
            dataGridView1.DataSource = table;

            Query = "SELECT *, ISNULL((SELECT COUNT (id) FROM RentedMovies WHERE movieidfk = Movies.id), 0) AS TimesRented FROM Movies ORDER BY TimesRented DESC";
            cmd = new SqlCommand(Query, con);
            table = new DataTable();
            adp = new SqlDataAdapter(cmd);
            adp.Fill(table);
            dataGridView2.DataSource = table;
        }
    }
}
