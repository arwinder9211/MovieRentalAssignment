using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieRentalAssignment
{
    public partial class MDI : Form
    {
        public MDI()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Customer().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Movie().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new IssueMovie().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new ReturnMovie().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Reporting().ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Confirm", "Exit?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }
}
