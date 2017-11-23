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
    public partial class Login : Form
    {
        public ATC atc;
        public Login(ATC atc)
        {
            InitializeComponent();
            this.atc = atc;
        }
        private void numberTextBox_Enter(object sender, EventArgs e)
        {
            if (numberTextBox.Text == "Enter a number")
            {
                numberTextBox.Text = "";
                numberTextBox.ForeColor = Color.Black;
            }
        }

        private void numberTextBox_Leave(object sender, EventArgs e)
        {
            if (numberTextBox.Text == "")
            {
                numberTextBox.Text = "Enter a number";
                numberTextBox.ForeColor = Color.DimGray;
            }
        }
        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            if (passwordTextBox.Text == "password")
            {
                passwordTextBox.Text = "";
                passwordTextBox.ForeColor = Color.Black;
            }
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            if (passwordTextBox.Text == "")
            {
                passwordTextBox.Text = "password";
                passwordTextBox.ForeColor = Color.DimGray;
            }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            LoginResult result = LoginService.login(numberTextBox.Text, passwordTextBox.Text, atc);

            if (result.isSuccessfull)
            {
                UserScreen screen = new UserScreen();
                screen.user = result.user;
                screen.user.logEvent += new LogDelegate(screen.log);
                screen.user.screen = screen;
                screen.Show();
                screen.Text = $"[{atc.name}] User screen {result.user.id}";
                this.Close();
            } else
            {
                MessageBox.Show(result.error);
            }
        }
    }
}
