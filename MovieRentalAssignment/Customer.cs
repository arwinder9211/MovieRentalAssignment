using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MovieRentalAssignment
{
    public partial class Customer : Form
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter adp;
        private string connstr;

        string custid;

        public Customer()
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
            string Query = "SELECT * FROM [Customer]";
            cmd = new SqlCommand(Query, con);
            DataTable table = new DataTable();
            adp = new SqlDataAdapter(cmd);
            adp.Fill(table);

            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (fname.Text == "" || lname.Text == "" || address.Text == "" || phoneno.Text == "")
            {
                MessageBox.Show("All fields are required");
            }
            else
            {
                if (button1.Text == "Add Customer")
                {
                    string Query = "INSERT INTO [Customer] VALUES (@fname, @lname, @address, @phoneno)";
                    cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@fname", fname.Text);
                    cmd.Parameters.AddWithValue("@lname", lname.Text);
                    cmd.Parameters.AddWithValue("@address", address.Text);
                    cmd.Parameters.AddWithValue("@phoneno", phoneno.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    FillGridView();
                    MessageBox.Show("Customer Added");
                }
                else
                {
                    string Query = "UPDATE [Customer] SET [fname] = @fname, [lname] = @lname, [address] = @address, [phoneno] = @phoneno WHERE [id] = @id";
                    cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@fname", fname.Text);
                    cmd.Parameters.AddWithValue("@lname", lname.Text);
                    cmd.Parameters.AddWithValue("@address", address.Text);
                    cmd.Parameters.AddWithValue("@phoneno", phoneno.Text);
                    cmd.Parameters.AddWithValue("@id", custid);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    custid = "";

                    button2_Click(this, null);
                    FillGridView();
                    MessageBox.Show("Customer Updated");
                }
            }
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            custid = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string Query = "SELECT * FROM [Customer] WHERE [id] = @id";
            cmd = new SqlCommand(Query, con);
            cmd.Parameters.AddWithValue("@id", custid);
            DataTable table = new DataTable();
            adp = new SqlDataAdapter(cmd);
            adp.Fill(table);

            if (table.Rows.Count > 0)
            {
                fname.Text = table.Rows[0]["fname"].ToString();
                lname.Text = table.Rows[0]["lname"].ToString();
                address.Text = table.Rows[0]["address"].ToString();
                phoneno.Text = table.Rows[0]["phoneno"].ToString();

                button1.Text = "Update Customer";
                button2.Visible = true;
                button3.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            custid = "";

            fname.Text = "";
            lname.Text = "";
            address.Text = "";
            phoneno.Text = "";

            button1.Text = "Add Customer";
            button2.Visible = false;
            button3.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string Query = "DELETE FROM [Customer] WHERE [id] = @id";
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@id", custid);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                button2_Click(this, null);
                FillGridView();
                MessageBox.Show("Customer Deleted");
            }
        }
    }
}
