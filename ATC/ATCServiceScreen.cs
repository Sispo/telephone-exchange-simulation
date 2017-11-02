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
    public partial class ATCServiceScreen : Form
    {
        public ATCServiceScreen()
        {
            InitializeComponent();
            onlineGridView.ColumnCount = 1;
            onlineGridView.RowHeadersVisible = false;
            onlineGridView.ColumnHeadersVisible = false;
            onlineGridView.ScrollBars = ScrollBars.Vertical;
            onlineGridView.Columns[0].Width = 160;
            ATCService.shared.stateUpdated += updateOnlineList;
        }

        private void updateOnlineList()
        {
            onlineGridView.Rows.Clear();

            foreach(ATC atc in ATCService.shared.onlineATCs)
            {
                onlineGridView.Rows.Add(atc.nameExtended);
            }
        }

        private void handle(ATCLoginResult result)
        {
            if (result.isSuccessfull)
            {
                WelcomeScreen ws = new WelcomeScreen(result.atc);
                ws.Show();
            }
            else
            {
                MessageBox.Show(result.error);
            }
        }

        private void firstATCBtn_Click(object sender, EventArgs e)
        {
            handle(ATCService.shared.Enable("Mini", "00"));
        }

        private void secondATCBtn_Click(object sender, EventArgs e)
        {
            handle(ATCService.shared.Enable("City", "09"));
        }

        private void anotherBtn_Click(object sender, EventArgs e)
        {
            ATCLoginScreen ls = new ATCLoginScreen();
            ls.Show();
        }

        private void dataGridView_SelectionChanged(Object sender, EventArgs e)
        {
            (sender as DataGridView).ClearSelection();
        }

    }
}
