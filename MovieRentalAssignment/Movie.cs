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
    public partial class Movie : Form
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter adp;
        private string connstr;

        string MovieID;

        public Movie()
        {
            InitializeComponent();
            connstr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            con = new SqlConnection(connstr);
            FillGridView();
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void FillGridView()
        {
            string Query = "SELECT * FROM [Movies]";
            cmd = new SqlCommand(Query, con);
            DataTable table = new DataTable();
            adp = new SqlDataAdapter(cmd);
            adp.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ValYear, ValCopies;

            if (rating.Text == "" || title.Text == "" || year.Text == "" || copies.Text == "" || plot.Text == "" || genre.Text == "")
            {
                MessageBox.Show("All fields are mandatory");
            }
            else if (!int.TryParse(year.Text, out ValYear) || !(int.TryParse(copies.Text, out ValCopies)))
            {
                MessageBox.Show("Invalid Year or Copies");
            }
            else
            {
                int Rental = 0;
                if ((DateTime.Now.Year - ValYear) > 5)
                {
                    Rental = 2;
                }
                else
                {
                    Rental = 5;
                }
                if (button1.Text == "Add Movie")
                {
                    string Query = "INSERT INTO [Movies] VALUES (@rating, @title, @year, @rentalcost, @copies, @plot, @genre)";
                    cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@rating", rating.Text);
                    cmd.Parameters.AddWithValue("@title", title.Text);
                    cmd.Parameters.AddWithValue("@year", year.Text);
                    cmd.Parameters.AddWithValue("@rentalcost", Rental);
                    cmd.Parameters.AddWithValue("@copies", copies.Text);
                    cmd.Parameters.AddWithValue("@plot", plot.Text);
                    cmd.Parameters.AddWithValue("@genre", genre.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    FillGridView();
                    MessageBox.Show("Movie Added");
                }
                else
                {
                    string Query = "UPDATE [Movies] SET [rating] = @rating, [title] = @title, [year] = @year, [rentalcost] = @rentalcost, [copies] = @copies, [plot] =  @plot, [genre] = @genre WHERE [id] = @id";
                    cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@rating", rating.Text);
                    cmd.Parameters.AddWithValue("@title", title.Text);
                    cmd.Parameters.AddWithValue("@year", year.Text);
                    cmd.Parameters.AddWithValue("@rentalcost", Rental);
                    cmd.Parameters.AddWithValue("@copies", copies.Text);
                    cmd.Parameters.AddWithValue("@plot", plot.Text);
                    cmd.Parameters.AddWithValue("@genre", genre.Text);
                    cmd.Parameters.AddWithValue("@id", MovieID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MovieID = "";

                    button2_Click(this, null);
                    FillGridView();
                    MessageBox.Show("Movie Updated");
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MovieID = "";
            rating.Text = "";
            title.Text = "";
            year.Text = "";
            copies.Text = "";
            plot.Text = "";
            genre.Text = "";

            button1.Text = "Add Movie";
            button2.Visible = false;
            button3.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string Query = "DELETE FROM [Movies] WHERE [id] = @id";
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@id", MovieID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                button2_Click(this, null);
                FillGridView();
                MessageBox.Show("Movie Deleted");
                
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MovieID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            string Query = "SELECT * FROM [Movies] WHERE [id] = @id";
            cmd = new SqlCommand(Query, con);
            cmd.Parameters.AddWithValue("@id", MovieID);
            DataTable table = new DataTable();
            adp = new SqlDataAdapter(cmd);
            adp.Fill(table);

            if (table.Rows.Count > 0)
            {
                rating.Text = table.Rows[0]["rating"].ToString();
                title.Text = table.Rows[0]["title"].ToString();
                year.Text = table.Rows[0]["year"].ToString();
                copies.Text = table.Rows[0]["copies"].ToString();
                plot.Text = table.Rows[0]["plot"].ToString();
                genre.Text = table.Rows[0]["genre"].ToString();

                button1.Text = "Update Movie";
                button2.Visible = true;
                button3.Visible = true;
            }
        }
    }
}
