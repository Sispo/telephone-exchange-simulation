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
            this.Name = $"User Screen";
            logGridView.ColumnCount = 1;
            logGridView.ColumnHeadersVisible = false;
            logGridView.RowHeadersVisible = false;
            logGridView.Columns[0].Width = 355;
            logGridView.ScrollBars = ScrollBars.Vertical;
            logGridView.ClearSelection();
        }

        public void log(string message)
        {
            logGridView.Rows.Add(message);
            logGridView.ClearSelection();
            logGridView.FirstDisplayedScrollingRowIndex = logGridView.RowCount - 1;
        }

        private void numberBtn_Click(object sender, EventArgs e)
        {
            string number = Convert.ToString((sender as Control).Tag);
            user.send(SignalType.number, number);
        }

        private void callBtn_Click(object sender, EventArgs e)
        {
            user.send(SignalType.phone,null);
        }

        private void endBtn_Click(object sender, EventArgs e)
        {
            user.send(SignalType.cancel,null);
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                user.send(SignalType.message, messageTextBox.Text);
                messageTextBox.Clear();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void UserScreen_FormClosing(Object sender, FormClosingEventArgs e)
        {
            user.disconnect();
        }
    }
}
