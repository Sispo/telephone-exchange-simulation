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
        public Login()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            LoginResult result = LoginService.login(numberTextBox.Text, passwordTextBox.Text, ATCService.local);

            if (result.isSuccessfull)
            {
                UserScreen screen = new UserScreen();
                screen.user = result.user;
                screen.user.log += new LogDelegate(screen.log);
                screen.Show();
            } else
            {
                MessageBox.Show(result.error);
            }
        }
    }
}
