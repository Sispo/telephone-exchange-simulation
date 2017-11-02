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

        private ATCSettingsScreen settingsScreen;

        public WelcomeScreen(ATC atc)
        {
            InitializeComponent();
            nameLbl.Text = ATCNameService.GetName(atc.id).ToUpper();
            this.Text = atc.name;
            this.atc = atc;
            settingsScreen = new ATCSettingsScreen(atc);
            ATCService.shared.connect(atc);
        }

        private void WelcomeScreen_FormClosing(Object sender, FormClosingEventArgs e)
        {
            settingsScreen.Close();
            settingsScreen.Dispose();
            settingsScreen = null;
            ATCService.shared.disconnect(atc);
            atc.disconnect();
            atc = null;
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            Login loginScreen = new Login(atc);
            loginScreen.Show();
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            settingsScreen.Show();
        }
    }
}
