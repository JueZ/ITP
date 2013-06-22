using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lidgren.Network;
using System.Reflection;
using System.Collections;

namespace Akanonda
{
    public partial class StartMenu : Form
    {
        public StartMenu()
        {
            InitializeComponent();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (colorBox.SelectedIndex > -1)
            {
                label1.Visible = false;
            }

            if (textBoxName.TextLength > 0 && colorBox.SelectedIndex > -1)
            {
                buttonLogIn.Enabled = true;
            }
            else
            {
                buttonLogIn.Enabled = false;
            }
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            //NetOutgoingMessage sendMsg;

            //sendMsg = Program.netclient.CreateMessage();

            //sendMsg.Write(Program.guid.ToString() + ";" + textBoxName.Text);
            //Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);

            Program.ConnectPlayerToLobby(textBoxName.Text, colorBox.Text);
            

            //sollte zuerst lobby und dann mainform sein-> spieler wird beim anmelden aber sofort auf spielfeld gesetzt...
            LobbyForm Lobby = new LobbyForm(textBoxName.Text,colorBox.Items[colorBox.SelectedIndex].ToString());
            this.Hide();
            Lobby.Show();
            //this.Show();
            //this.Dispose();
        }

        private void colorBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.Bounds;
            if (e.Index >= 0)
            {
                string n = ((ComboBox)sender).Items[e.Index].ToString();
                Font f = new Font("Arial", 9, FontStyle.Regular);
                Color c = Color.FromName(n);
                Brush b = new SolidBrush(c);
                g.DrawString(n, f, Brushes.Black, rect.X, rect.Top);
                g.FillRectangle(b, rect.X + 100, rect.Y + 5,
                                rect.Width - 10, rect.Height - 10);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            colorBox.DroppedDown = true;
        }
    }
}
