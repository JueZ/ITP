using System;
using System.Windows.Forms;
using System.Drawing;
using Akanonda.GameLibrary;
using Lidgren.Network;
using System.Threading;
using Akanonda.Properties;
using System.Collections.Generic;



namespace Akanonda
{
    internal sealed class Program
    {
        public static Game game;
        public static NetClient s_client;
        public static Guid guid = Guid.NewGuid();
        public static NetClient netclient;
        public static System.Windows.Forms.Timer SurvivalTimer;
        public static int SurvivalSecond = 0;
        public static int SurvivalMinute = 0;
        private static Settings settings = new Settings();
        private static int length = 0;
        [STAThread]
        private static void Main(string[] args)
        {

            //Color.Blue.ToArgb().ToString();
            //Color.FromArgb

            game = Game.Instance;
            game.LocalPlayerGuid = Program.guid;
            SurvivalTimer = new System.Windows.Forms.Timer();
            SurvivalTimer.Interval = 1000;
            SurvivalTimer.Tick += SurvivalTimer_Tick;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartMenu());
            
            //Application.Run(new MainForm());
        }



        public static void ConnectPlayerToGame(string playername, Color playercolor, string playOrWatch)
        {
            NetPeerConfiguration netconfig = new NetPeerConfiguration("game");

            netclient = new NetClient(netconfig);

            if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            //string playername = "Martin";
            string hailmessage;

            
                hailmessage = guid.ToString() + ";" + playername + ";" + Convert.ToString(playercolor.ToArgb()) + ";"+playOrWatch;
            //else
            //    hailmessage = guid.ToString() + ";" + playername + ";" + Convert.ToString(playercolor.ToArgb()) + ";justWatching";


            netclient.RegisterReceivedCallback(new SendOrPostCallback(ReceivedData));

            netclient.Start();

            

            NetOutgoingMessage message = netclient.CreateMessage(hailmessage);
            netclient.Connect(settings.GameServer, settings.GamePort, message);

            

        }

        public static void ConnectPlayerToLobby(string playername, Color playercolor)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("chat");
            config.AutoFlushSendQueue = false;
            s_client = new NetClient(config);

            s_client.RegisterReceivedCallback(new SendOrPostCallback(LobbyForm.GotMessage));
            //-----------------------------------------------------------HIER--------------

            int port;
            //Color playercolor = Color.FromName(color);
            string hailmessage = guid.ToString() + ";" + playername + ";" + Convert.ToString(playercolor.ToArgb());

            Int32.TryParse("1338", out port);
            s_client.Start();
            //NetOutgoingMessage hail = s_client.CreateMessage(Program.guid.ToString() + ";" + "connected");
            NetOutgoingMessage hail = s_client.CreateMessage(hailmessage);
            s_client.Connect(settings.ChatServer, settings.ChatPort, hail);
            //s_client.Connect("server.xios.at", port, hail);
            
            
        }

        public static void SurvivalTimer_Tick(object sender, EventArgs e)
        {
            SurvivalSecond++;

            if (SurvivalSecond == 60)
            {
                SurvivalMinute++;
                SurvivalSecond = 0;
            }
        }

        public static void ReceivedData(object peer)
		{
			NetIncomingMessage im;
			while ((im = netclient.ReadMessage()) != null)
			{
				// handle incoming message
				switch (im.MessageType)
				{
					case NetIncomingMessageType.DebugMessage:
					case NetIncomingMessageType.ErrorMessage:
					case NetIncomingMessageType.WarningMessage:
					case NetIncomingMessageType.VerboseDebugMessage:
//						string text = im.ReadString();
//						Output(text);
						break;
					case NetIncomingMessageType.StatusChanged:
//						NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
//
//						if (status == NetConnectionStatus.Connected)
//							s_form.EnableInput();
//						else
//							s_form.DisableInput();
//
//						if (status == NetConnectionStatus.Disconnected)
//							s_form.button2.Text = "Connect";
//
//						string reason = im.ReadString();
//						Output(status.ToString() + ": " + reason);
//
						break;
					case NetIncomingMessageType.Data:
						int gamedatalength = im.ReadInt32();
						
						byte[] gamedata = im.ReadBytes(gamedatalength);
						
						game = (Game)SerializeHelper.ByteArrayToObject(gamedata);
                        game.LocalPlayerGuid = Program.guid;

                        if (game.getPlayerLength(game.LocalPlayerGuid) > 0)
                            length = game.getPlayerLength(game.LocalPlayerGuid);
                        //if (game.CollisionList.Count > 0)
                        //{
                        //    MessageBox.Show("collision found");
                        //}


                        foreach (KeyValuePair<Guid, CollisionType> key in game.CollisionList)
                        {

                            if (key.Key == game.LocalPlayerGuid)
                            {
                                
                                ConnectPlayerToGame(game.getPlayerName(guid), game.getPlayerColor(guid), "dead");
    
                                //MainForm.Dispose();
                                string text;

                                switch (key.Value)
                                {
                                    case CollisionType.ToPlayer:
                                        text = "You crashed into another player!";
                                        break;

                                    case CollisionType.ToSelf:
                                        text = "You crashed into yourself!";
                                        break;

                                    case CollisionType.ToWall:
                                        text = "Your face hit the wall!";
                                        break;

                                    default:
                                        text = "You crashed!";
                                        break;
                                }

                                //overlayForm Overlay = new overlayForm();
                                //Overlay.Location = MainForm.M_Form.Location;
                                //Overlay.Size = MainForm.M_Form.Size;
                                //Overlay.FormBorderStyle = FormBorderStyle.None;
                                //Overlay.Show();

                                MainForm.M_Form.showOverlay(SurvivalMinute, SurvivalSecond, text, length);

                                //MessageBox.Show(text);
                                LobbyForm.L_form.StartGame_Enable();
                                LobbyForm.L_form.Focus();
                                //Program.ConnectPlayerToLobby(game.getPlayerName(guid), game.getPlayerColor(guid));
                               
                            }
                        }


//						string chat = im.ReadString();
//						Output(chat);
						break;
					default:
//						Output("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
						break;
				}
				netclient.Recycle(im);
			}
		}

    }
}
