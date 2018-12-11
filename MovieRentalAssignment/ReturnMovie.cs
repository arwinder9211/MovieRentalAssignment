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
    public partial class ReturnMovie : Form
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter adp;
        private string connstr;

        public ReturnMovie()
        {
            InitializeComponent();
            connstr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            con = new SqlConnection(connstr);
            FillGridView();
        }

        private void FillGridView()
        {
            string Query = "SELECT rentedmovies.id, Customer.fname, Customer.lname, Customer.[Address], Movies.title, Movies.rentalcost, RentedMovies.rentedon, RentedMovies.returnedon FROM RentedMovies INNER JOIN Movies ON RentedMovies.movieidfk = Movies.id INNER JOIN Customer ON RentedMovies.custidfk = Customer.id WHERE RentedMovies.returnedon IS NULL";
            cmd = new SqlCommand(Query, con);
            DataTable table = new DataTable();
            adp = new SqlDataAdapter(cmd);
            adp.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void ReturnMovie_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (MessageBox.Show("Confirm", "Return?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string Date = DateTime.Now.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToShortTimeString();
                string Query = "UPDATE [RentedMovies] SET [returnedon] = @returnedon WHERE [id] = @id";
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@returnedon", Date);
                cmd.Parameters.AddWithValue("@id", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                FillGridView();
                MessageBox.Show("Movie Returned");
            }
        }
    }
}
