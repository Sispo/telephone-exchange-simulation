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
    public partial class ATCSettingsScreen : Form
    {
        ATC atc;
        public ATCSettingsScreen(ATC atc)
        {
            InitializeComponent();
            ConfigureUI();
            this.atc = atc;
            atc.logEvent += AddLog;
            ATCService.shared.stateUpdated += updateOnlineATCList;
        }

        void ConfigureUI()
        {
            atcGridView.ColumnCount = 1;
            atcGridView.RowHeadersVisible = false;
            atcGridView.ColumnHeadersVisible = false;
            atcGridView.ScrollBars = ScrollBars.Vertical;
            atcGridView.Columns[0].Width = 150;

            usersGridView.ColumnCount = 1;
            usersGridView.RowHeadersVisible = false;
            usersGridView.ColumnHeadersVisible = false;
            usersGridView.ScrollBars = ScrollBars.Vertical;
            usersGridView.Columns[0].Width = 150;

            connectionsGridView.ColumnCount = 1;
            connectionsGridView.RowHeadersVisible = false;
            connectionsGridView.ColumnHeadersVisible = false;
            connectionsGridView.ScrollBars = ScrollBars.Vertical;
            connectionsGridView.Columns[0].Width = 270;

            logsGridView.ColumnCount = 1;
            logsGridView.RowHeadersVisible = false;
            logsGridView.ColumnHeadersVisible = false;
            logsGridView.ScrollBars = ScrollBars.Vertical;
            logsGridView.Columns[0].Width = 655;
        }

        void AddLog(string message)
        {
            logsGridView.Rows.Add(message);
            logsGridView.Rows[logsGridView.Rows.Count - 1].Height = message.Length > 100 ? 70 : 35;
            logsGridView.FirstDisplayedScrollingRowIndex = logsGridView.RowCount - 1;
        }

        private void updateOnlineATCList()
        {
            atcGridView.Rows.Clear();

            foreach (ATC onlineAtc in ATCService.shared.onlineATCs)
            {
                atcGridView.Rows.Add(onlineAtc.nameExtended);
                if (this.atc.connectedATCs.Exists(a => a.id == onlineAtc.id))
                {
                    atcGridView.Rows[atcGridView.Rows.Count - 1].Cells[0].Style.BackColor = Color.Green;
                }
            }
        }


    }
}
