﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

using Lidgren.Network;


namespace Akanonda
{
    public partial class LobbyForm : Form
    {

        //private static NetClient s_client = Program.s_client;
        public static LobbyForm L_form;
        public string name;
        public Color color;
        private static GameLibrary.Game game = GameLibrary.Game.Instance;
        public static FormConnector lobbyConnector;

        public LobbyForm(string n, Color c):base()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lobbyConnector = new FormConnector(this);
            MainForm.mainConnector.ConnectForm(this);
            name = n;
            color = c;
            L_form = this;
            MessageBox.KeyDown += new KeyEventHandler(MessageBox_KeyDown);
        }

      

        public static void Connect(string host, int port)
        {
            Program.s_client.Start();
            NetOutgoingMessage hail = Program.s_client.CreateMessage(Program.guid.ToString() + ";" + "connected");
            Program.s_client.Connect(host, port, hail);
        }

        void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                // return is equivalent to clicking "send"
                SendButton_Click(sender, e);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            // Send
            if (!string.IsNullOrEmpty(MessageBox.Text))
                Send(MessageBox.Text);
            MessageBox.Text = "";

        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            settingsForm settingForm = new settingsForm();
            settingForm.ShowDialog();
        }

        public static void Send(string text)
        {
            NetOutgoingMessage om = Program.s_client.CreateMessage(Program.guid.ToString() + ";" + text);
            Program.s_client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            //Output("Sending '" + text + "'");
            Program.s_client.FlushSendQueue();
        }

        private static void Output(string text)
        {
            
            if (text != null && text != "" && L_form != null)
            {
                try
                {
                    if (L_form.ChatBox.TextLength == 0)
                    {
                        L_form.ChatBox.AppendText(text.ToString());
                    }
                    else
                    {
                        L_form.ChatBox.AppendText("\n" + text.ToString());
                    }
                    //s_form.ChatBox.Text += "\r\n" + text.ToString();
                    L_form.ChatBox.SelectionStart = L_form.ChatBox.Text.Length;
                    L_form.ChatBox.ScrollToCaret();
                }
                catch
                {
                    throw new Exception();
                }

            }
        }

        public static void GotMessage(object peer)
        {
            NetIncomingMessage im;
            while ((im = Program.s_client.ReadMessage()) != null)
            {
                // handle incoming message
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        string text = im.ReadString();
                        //Output(text);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                        if (status == NetConnectionStatus.Connected)
                        {
                            L_form.EnableInput();
                            Output("Successfully connected to Chat");
                        }
                        else
                        {
                            //s_form.DisableInput();
                            //s_form.ReConnect();
                            //string reason = im.ReadString();
                            //Output(status.ToString() + ": " + reason);
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        string chat = im.ReadString();
                        if (chat.StartsWith("PlayersIn"))
                        {
                            string[] chat2 = chat.Split(':');
                            string[] PlayersInLobby = chat2[0].Split(';');
                            string[] PlayersInGame = chat2[1].Split(';');
                            L_form.FillList(PlayersInLobby, PlayersInGame);
                            //L_form.PlayersInLobbyList.View = View.Details;

                        }
                        else if(chat == "kick:"+Program.game.LocalPlayerGuid.ToString())
                        {
                            System.Windows.Forms.MessageBox.Show("You were kicked by the Admin!");
                            L_form.LobbyForm_FormClosing(null, null);
                        }
                        else if (chat == "ban:" + Program.game.LocalPlayerGuid.ToString())
                        {
                            System.Windows.Forms.MessageBox.Show("You were banned by the Admin!");
                            L_form.LobbyForm_FormClosing(null, null);
                        }
                        else
                        {
                            Output(chat);
                        }
                        break;
                    default:
                        Output("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
                        break;
                }
            }
        }

        //public void ReConnect()
        //{
        //        int port;
        //        Int32.TryParse("1338", out port);
        //        Connect("localhost", port);   
        //}

        public void EnableInput()
        {
            MessageBox.Enabled = true;
            SendButton.Enabled = true;
        }

        public void DisableInput()
        {
            MessageBox.Enabled = false;
            SendButton.Enabled = false;
        }

        public void StartGame_Enable()
        {
            StartGame.Enabled = true;
            StartGame.Text = "Start";

        }

        public void StartGame_Click(object sender, EventArgs e)
        {
            MainForm.M_Form.closeOverlay();
            Program.SurvivalSecond = 0;
            Program.SurvivalMinute = 0;
            Program.SurvivalTimer.Start();
            StartGame.Enabled = false;
            StartGame.Text = "In Game";
            //Program.s_client.Shutdown(game.LocalPlayerGuid.ToString());
            MainForm.M_Form.Focus();
            Program.ConnectPlayerToGame(name, color, "playing");
            //MainForm Main = new MainForm();
            //LobbyForm.ActiveForm.Close();
            
            //Main.Show();
            //Main.Location = new Point(this.Left + this.Width, this.Top);
            
            
            
        }

        private void FillList(string[] PlayersInLobby, string[] PlayersInGame)
        {
            PlayersInLobbyList.Items.Clear();
            for (int i = 1; i < PlayersInLobby.Length; i++ )
            {
                string[] Player = PlayersInLobby[i].Split('-');
                ListViewItem newPlayer = new ListViewItem(Player[0]);
                newPlayer.SubItems.Add(Player[1]);
                newPlayer.SubItems.Add(Player[2]);
                PlayersInLobbyList.Items.Add(newPlayer);
            }
            PlayersInGameList.Items.Clear();
            for (int i = 1; i < PlayersInGame.Length; i++)
            {
                string[] Player = PlayersInGame[i].Split('-');
                ListViewItem newPlayer = new ListViewItem(Player[0]);
                newPlayer.SubItems.Add(Player[1]);
                newPlayer.SubItems.Add(Player[2]);
                PlayersInGameList.Items.Add(newPlayer);
            }
        }

        private static String GetColorName(Color color)
        {
            string name = "Unknown";
            foreach (KnownColor kc in Enum.GetValues(typeof(KnownColor)))
            {
                Color known = Color.FromKnownColor(kc);
                if (color.ToArgb() == known.ToArgb())
                {
                    name = known.Name;
                }
            }
            return "";
        }

        private void LobbyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.netclient.Shutdown(Program.guid.ToString());
            Program.s_client.Shutdown(Program.guid.ToString());

            while (Program.netclient.Status != NetPeerStatus.NotRunning || Program.s_client.Status != NetPeerStatus.NotRunning)
                System.Threading.Thread.Sleep(10);
            MainForm.M_Form.Dispose();
            this.Dispose();
            Environment.Exit(0);
        }

        private void LobbyForm_Activated(object sender, EventArgs e)
        {
            //MainForm.M_Form.BringToFront();
            StartGame.Focus();
        }
    }
}