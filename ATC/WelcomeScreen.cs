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
            nameLbl.Text = atc.name.ToUpper();
            this.Text = atc.name;
            this.atc = atc;
            settingsScreen = new ATCSettingsScreen(atc);
            ATCService.shared.connect(atc);
            this.FormClosing += WelcomeScreen_FormClosing;
        }

        private void WelcomeScreen_FormClosing(Object sender, FormClosingEventArgs e)
        {
            settingsScreen.disconnect();
            settingsScreen.Close();
            settingsScreen.Dispose();
            ATCService.shared.disconnect(atc);
            atc.disconnect();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            LoginScreen loginScreen = new LoginScreen(atc);
            loginScreen.Show();
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            if (settingsScreen.IsDisposed)
            {
                settingsScreen = new ATCSettingsScreen(atc);
            }
            settingsScreen.Show();
        }
    }
}
