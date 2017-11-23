using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATC
{
    public partial class ATCLoginScreen : Form
    {
        public ATCLoginScreen()
        {
            InitializeComponent();
        }

        private void nameTextBox_Enter(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "Enter ATC Name")
            {
                nameTextBox.Text = "";
                nameTextBox.ForeColor = Color.Black;
            }
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "")
            {
                nameTextBox.Text = "Enter ATC Name";
                nameTextBox.ForeColor = Color.DimGray;
            }
        }

        private void idTextBox_Enter(object sender, EventArgs e)
        {
            if (idTextBox.Text == "Automatic ID")
            {
                idTextBox.Text = "";
                idTextBox.ForeColor = Color.Black;
            }
        }

        private void idTextBox_Leave(object sender, EventArgs e)
        {
            if (idTextBox.Text == "")
            {
                idTextBox.Text = "Automatic ID";
                idTextBox.ForeColor = Color.DimGray;
            }
        }

        private void enableBtn_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "Enter ATC Name")
            {
                MessageBox.Show("Please enter ATC Name");
            } else
            {
                ATCLoginResult result = ATCService.shared.Enable(nameTextBox.Text, idTextBox.Text == "Automatic ID" ? null : idTextBox.Text);
                if (result.isSuccessfull)
                {
                    WelcomeScreen ws = new WelcomeScreen(result.atc);
                    ws.Show();
                    this.Close();
                } else
                {
                    MessageBox.Show(result.error);
                }
            }
        }
    }
}
