using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Akanonda
{
    public partial class resetForm : Form
    {
        public resetForm(int seconds, int minutes)
        {
            InitializeComponent();
            if (minutes == 0)
            {
                SurvivalTimeBox.Text = seconds > 1 || seconds == 0 ? "          GAME OVER!\nYou survived " + seconds + " seconds!" : "         GAME OVER!\nYou survived " + seconds + " second!";
            }
            else
            {
                SurvivalTimeBox.Text = minutes > 1 ? "      GAME OVER!\nYou survived " + seconds + " seconds and " + minutes + " minutes!" : "GAME OVER!\nYou survived " + seconds + " seconds and " + minutes + " minute!";
            }
        }


    }
}
