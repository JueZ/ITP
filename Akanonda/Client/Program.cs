using System;
using System.Windows.Forms;
using System.Drawing;
using Akanonda.GameLibrary;
using Lidgren.Network;
using System.Threading;

namespace Akanonda
{
    internal sealed class Program
    {
        public static Game game;
        public static Guid guid = Guid.NewGuid();
        
        private static NetClient netclient;
        
        [STAThread]
        private static void Main(string[] args)
        {
            NetPeerConfiguration netconfig = new NetPeerConfiguration("game");
		
			netclient = new NetClient(netconfig);
			
			if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
			
			netclient.RegisterReceivedCallback(new SendOrPostCallback(ReceivedData));
			
			netclient.Start();
			NetOutgoingMessage message = netclient.CreateMessage(guid.ToString());
			netclient.Connect("127.0.0.1", 1337, message);
            
            game = Game.Instance;
            game.setFieldSize(500, 500);
            game.addlocalPlayer("Martin", Color.Green);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            
            netclient.Disconnect(guid.ToString());
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
//						string chat = im.ReadString();
//						Output(chat);
						break;
					default:
//						Output("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
						break;
				}
			}
		}

    }
}
