﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Akanonda
{
    public partial class settingsForm : Form
    {
        public settingsForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.ServerAdress.Text = Program.settings.ServerAdresse;
            this.ChatPort.Text = Program.settings.ChatPort.ToString();
            this.GamePort.Text = Program.settings.GamePort.ToString();

            if (Program.settings.Use4Buttons)
                this.use4ButtonsRadio.Checked = true;
            else
                this.use2ButtonsRadio.Checked = true;

        }

        private void use4ButtonsRadio_CheckedChanged(object sender, EventArgs e)
        {
            Program.settings.Use4Buttons = true;
        }

        private void use2ButtonsRadio_CheckedChanged(object sender, EventArgs e)
        {
            Program.settings.Use4Buttons = false;
        }

        private void closeSettingsButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void saveSettingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                Program.netclient.Disconnect(Program.guid.ToString());
                NetOutgoingMessage message = Program.netclient.CreateMessage("Connect");
                Program.netclient.Connect(this.ServerAdress.Text, Convert.ToInt32(this.GamePort.Text), message);


                string hailmessage = Program.guid.ToString() + ";" + LobbyForm.L_form.name + ";" + Convert.ToString(LobbyForm.L_form.color.ToArgb());
                Program.s_client.Disconnect(Program.guid.ToString());
                message = Program.netclient.CreateMessage(hailmessage);
                Program.s_client.Connect(this.ServerAdress.Text, Convert.ToInt32(this.ChatPort.Text), message);
                Program.settings.ServerAdresse = this.ServerAdress.Text;
                Program.settings.ChatPort = Convert.ToInt32(this.ChatPort.Text);
                Program.settings.GamePort = Convert.ToInt32(this.GamePort.Text);
                Program.settings.Save();
                LobbyForm.L_form.StartGame_Enable();
            }
            catch
            {
                MessageBox.Show("Only Numbers!");
            }
        }
    }
}
