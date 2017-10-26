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
    public partial class UserScreen : Form
    {
        public User user;
        public UserScreen()
        {
            InitializeComponent();
        }

        public void log(string message)
        {

        }

        private void numberBtn_Click(object sender, EventArgs e)
        {
            string number = Convert.ToString((sender as Control).Tag);
            user.send(new Signal(SignalType.number, number));
        }

        private void callBtn_Click(object sender, EventArgs e)
        {
            user.send(new Signal(SignalType.phone));
        }

        private void endBtn_Click(object sender, EventArgs e)
        {
            user.send(new Signal(SignalType.cancel));
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                user.send(new Signal(SignalType.message, messageTextBox.Text));
            }
        }
    }
}
