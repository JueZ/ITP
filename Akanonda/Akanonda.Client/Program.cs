using System;
using System.Windows.Forms;
using System.Drawing;
using Akanonda.GameLibrary;
using Lidgren.Network;
using System.Threading;
using System.Collections.Generic;

namespace Akanonda
{
    internal sealed class Program
    {
        public static Game game;
        public static NetClient s_client;
        public static Guid guid = Guid.NewGuid();
        public static NetClient netclient;
        
        [STAThread]
        private static void Main(string[] args)
        {

            //Color.Blue.ToArgb().ToString();

            //Color.FromArgb

            game = Game.Instance;
            game.LocalPlayerGuid = Program.guid;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartMenu());
            //Application.Run(new MainForm());
        }

        public static void ConnectPlayerToGame(string playername, Color playercolor)
        {
            NetPeerConfiguration netconfig = new NetPeerConfiguration("game");

            netclient = new NetClient(netconfig);

            if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            
            //string playername = "Martin";

            string hailmessage = guid.ToString() + ";" + playername + ";" + Convert.ToString(playercolor.ToArgb());

            netclient.RegisterReceivedCallback(new SendOrPostCallback(ReceivedData));

            netclient.Start();
            NetOutgoingMessage message = netclient.CreateMessage(hailmessage);
            netclient.Connect("127.0.0.1", 1337, message);

            

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
            string hailmessage = guid.ToString() + ";connected;" + playername + ";" + Convert.ToString(playercolor.ToArgb());

            Int32.TryParse("1338", out port);
            s_client.Start();
            //NetOutgoingMessage hail = s_client.CreateMessage(Program.guid.ToString() + ";" + "connected");
            NetOutgoingMessage hail = s_client.CreateMessage(hailmessage);
            s_client.Connect("127.0.0.1", port, hail);
            
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

                        foreach (KeyValuePair<Guid, CollisionType> key in game.CollisionList)
                        {
                            if (key.Key == game.LocalPlayerGuid)
                            {

                                MainForm.ActiveForm.Close();
                                MessageBox.Show(key.Value.ToString());
                                LobbyForm Lobby = new LobbyForm(game.getPlayerName(guid), game.getPlayerColor(guid));
                                Program.ConnectPlayerToLobby(game.getPlayerName(guid), game.getPlayerColor(guid));
                                Lobby.ShowDialog();
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
