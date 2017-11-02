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

        public ATC atc;

        public WelcomeScreen(ATC atc)
        {
            InitializeComponent();
            nameLbl.Text = ATCNameService.GetName(atc.id).ToUpper();
            this.atc = atc;
        }

        private void WelcomeScreen_FormClosing(Object sender, FormClosingEventArgs e)
        {
            ATCService.shared.onlineATCs.Remove(atc);
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            Login loginScreen = new Login(atc);
            loginScreen.Show();
        }
    }
}
