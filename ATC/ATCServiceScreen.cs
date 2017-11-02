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
        }

        string incrementStringID(string id)
        {
            int length = id.Length;
            string newId = (Convert.ToInt32(id) + 1).ToString();
            while (newId.Length != length)
            {
                newId = "0" + newId;
            }
            return newId;
        }

        private void firstATCBtn_Click(object sender, EventArgs e)
        {
            Console.WriteLine(incrementStringID("001"));
            Console.WriteLine(incrementStringID("015"));
            Console.WriteLine(incrementStringID("5"));
            Console.WriteLine(incrementStringID("05"));
            Console.WriteLine(incrementStringID("15"));
            Console.WriteLine(incrementStringID("0008"));
        }

        private void secondATCBtn_Click(object sender, EventArgs e)
        {

        }

        private void anotherBtn_Click(object sender, EventArgs e)
        {
            ATCLoginScreen ls = new ATCLoginScreen();
            ls.Show();
        }
    }
}
