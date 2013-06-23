using System;
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

        private static NetClient s_client;
        private static LobbyForm s_form;
        private string name;
        private Color color;
        private static GameLibrary.Game game = GameLibrary.Game.Instance;

        public LobbyForm(string n, Color c):base()
        {
            InitializeComponent();
            name = n;
            color = c;
            s_form = this;
            //startChat();
            MessageBox.KeyDown += new KeyEventHandler(MessageBox_KeyDown);
            
        }

        [STAThread]
        //static void startChat()
        //{
        //    //Application.EnableVisualStyles();
        //    //Application.SetCompatibleTextRenderingDefault(false);
        //    //s_form = ;

        //    NetPeerConfiguration config = new NetPeerConfiguration("chat");
        //    config.AutoFlushSendQueue = false;
        //    s_client = new NetClient(config);

        //    s_client.RegisterReceivedCallback(new SendOrPostCallback(GotMessage));

        //    int port;
        //    Int32.TryParse("1338", out port);
        //    Connect("localhost", port);


        //    //Application.Run(s_form);

        //    //s_client.Shutdown("Bye");
        //}

        public static void Connect(string host, int port)
        {
            s_client.Start();
            NetOutgoingMessage hail = s_client.CreateMessage(Program.guid.ToString() + ";" + "connected");
            s_client.Connect(host, port, hail);
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

        public static void Send(string text)
        {
            NetOutgoingMessage om = s_client.CreateMessage(Program.guid.ToString() + ";" + text);
            s_client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            //Output("Sending '" + text + "'");
            s_client.FlushSendQueue();
        }

        private static void Output(string text)
        {
            
            if (text != null && text != "")
            {
                s_form.ChatBox.AppendText("\r\n" + text.ToString());
                //s_form.ChatBox.Text += "\r\n" + text.ToString();
                s_form.ChatBox.SelectionStart = s_form.ChatBox.Text.Length;
                s_form.ChatBox.ScrollToCaret();
            }
        }

        public static void GotMessage(object peer)
        {
            s_client = Program.s_client;
            NetIncomingMessage im;
            while ((im = s_client.ReadMessage()) != null)
            {
                // handle incoming message
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        string text = im.ReadString();
                        Output(text);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                        if (status == NetConnectionStatus.Connected)
                        {
                            s_form.EnableInput();
                            Output("Successfully connected to Chat");
                        }
                        else
                        {
                            s_form.DisableInput();
                            s_form.ReConnect();
                            string reason = im.ReadString();
                            Output(status.ToString() + ": " + reason);
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        string chat = im.ReadString();
                        if (chat.StartsWith("PlayersIn"))
                        {
                            string[] chat2 = chat.Split(':');
                            string[] PlayersInLobby = chat2[0].Split(';');
                            string[] PlayersInGame = chat2[1].Split(';');
                            s_form.FillList(PlayersInLobby, PlayersInGame);
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

        public void ReConnect()
        {
                int port;
                Int32.TryParse("1338", out port);
                Connect("localhost", port);   
        }

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

        private void StartGame_Click(object sender, EventArgs e)
        {
            Program.ConnectPlayerToGame(name, color);
            MainForm Main = new MainForm();
            LobbyForm.ActiveForm.Close();
            Main.Show();
            NetOutgoingMessage om = s_client.CreateMessage("UpdateLobbyLists");
            s_client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            //Output("Sending '" + text + "'");
            s_client.FlushSendQueue();
        }

        private void FillList(string[] PlayersInLobby, string[] PlayersInGame)
        {
            PlayersInLobbyList.Items.Clear();
            for (int i = 1; i < PlayersInLobby.Length; i++ )
            {
                PlayersInLobbyList.Items.Add(PlayersInLobby[i]);
            }
            PlayersInGameList.Items.Clear();
            for (int i = 1; i < PlayersInGame.Length; i++)
            {
                PlayersInGameList.Items.Add(PlayersInGame[i]);
            }
        }
    }
}