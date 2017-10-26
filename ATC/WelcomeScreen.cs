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
    public partial class WelcomeScreen : Form
    {
        public WelcomeScreen()
        {
            InitializeComponent();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            Login loginScreen = new Login();
            loginScreen.Show();
        }
    }
}
