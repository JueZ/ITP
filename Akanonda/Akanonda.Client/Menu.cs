﻿using System;
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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.TextLength > 0)
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
            NetOutgoingMessage sendMsg;

            sendMsg = Program.netclient.CreateMessage();

            sendMsg.Write(Program.guid.ToString() + ";" + textBoxName.Text);
            Program.netclient.SendMessage(sendMsg, NetDeliveryMethod.ReliableSequenced);

            Form mainForm = new LobbyForm();
            this.Hide();
            mainForm.ShowDialog();
            this.Show();
            
        }
    }
}