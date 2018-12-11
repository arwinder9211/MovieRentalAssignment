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
    public partial class IssueMovie : Form
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private string ConnectionString;

        public IssueMovie()
        {
            InitializeComponent();
            ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            connection = new SqlConnection(ConnectionString);
        }

        private void FillMovieGrid()
        {
            string Query = "SELECT * FROM [Customer]";
            command = new SqlCommand(Query, connection);
            DataTable table = new DataTable();
            adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            dataGridViewCustomer.DataSource = table;
        }

        private void FillCustomerGrid()
        {
            string Query = "SELECT * FROM [Movies]";
            command = new SqlCommand(Query, connection);
            DataTable table = new DataTable();
            adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            dataGridViewMovie.DataSource = table;
        }

        private void IssueMovie_Load(object sender, EventArgs e)
        {
            FillCustomerGrid();
            FillMovieGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridViewCustomer.SelectedRows.Count > -1 && dataGridViewCustomer.SelectedRows.Count > -1)
            {
                string CustId = dataGridViewCustomer.SelectedRows[0].Cells[0].Value.ToString();
                string MovieId = dataGridViewMovie.SelectedRows[0].Cells[0].Value.ToString();

                int AvailableCopies = 0;

                string Query = "SELECT (SELECT copies FROM movies WHERE id = @id) - (SELECT ISNULL(COUNT(movieidfk), 0) FROM RentedMovies WHERE movieidfk = @id AND returnedon IS NULL)";
                command = new SqlCommand(Query, connection);
                command.Parameters.AddWithValue("@id", MovieId);
                DataTable table = new DataTable();
                adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                AvailableCopies = Convert.ToInt32(table.Rows[0][0]);

                if (AvailableCopies > 0)
                {
                    string Date = DateTime.Now.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToShortTimeString();

                    Query = "INSERT INTO [RentedMovies] (movieidfk, custidfk, rentedon) VALUES (@movieidfk, @custidfk, @rentedon)";

                    command = new SqlCommand(Query, connection);
                    command.Parameters.AddWithValue("@movieidfk", MovieId);
                    command.Parameters.AddWithValue("@custidfk", CustId);
                    command.Parameters.AddWithValue("@rentedon", Date);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Movie Rented");
                }
                else
                {
                    MessageBox.Show("No copy available to rent");
                }
            }
        }
    }
}
