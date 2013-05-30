using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akanonda.GameLibrary;
using System.Threading;
using Lidgren.Network;
using System.Timers;

namespace Akanonda
{
    public class Program
    {
        private static NetServer netserver;
                    
        static void Main(string[] args)
        {


            Console.WriteLine("Akanonda Server");
            Console.WriteLine("---------------");
                        		
            NetPeerConfiguration netconfig = new NetPeerConfiguration("game");
			
            netconfig.MaximumConnections = 10;
			netconfig.Port = 1337;
			
			netserver = new NetServer(netconfig);

            if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            
			netserver.RegisterReceivedCallback(new SendOrPostCallback(ReceiveData)); 
		
			netserver.Start();

            System.Timers.Timer timer = new System.Timers.Timer(100);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            
            Console.Write("Command: ");

			while (true)
            {
                string input = Console.ReadLine();
                
                switch (input)
                { 
                    case "start":
                        break;
                    case "status":
                        Console.WriteLine(netserver.ConnectionsCount);
                        break;
                    case "exit":
                        netserver.Shutdown("Exit");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Command not found.");
                        break;
                }
                
                Console.Write("Command: ");
            }
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        public static void ReceiveData(object peer)
        {
			NetIncomingMessage im;
			while ((im = netserver.ReadMessage()) != null)
			{
				switch (im.MessageType)
				{
					case NetIncomingMessageType.DebugMessage:
					case NetIncomingMessageType.ErrorMessage:
					case NetIncomingMessageType.WarningMessage:
					case NetIncomingMessageType.VerboseDebugMessage:
						string text = im.ReadString();
                        Console.WriteLine(text);
                        break;
					case NetIncomingMessageType.StatusChanged:
                        
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        						
                        if(im.SenderConnection.RemoteHailMessage != null && status == NetConnectionStatus.Connected)
                            Console.WriteLine(im.SenderConnection.RemoteHailMessage.ReadString());
						
						string reason = im.ReadString();
						Console.WriteLine(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);
						

						
                        // Update user status
						//UpdateConnectionsList();
						break;
					case NetIncomingMessageType.Data:
//						// incoming chat message from a client
						string chat = im.ReadString();
						Console.WriteLine(chat);

//						Output("Broadcasting '" + chat + "'");
//
//						// broadcast this to all connections, except sender
//						List<NetConnection> all = s_server.Connections; // get copy
//						all.Remove(im.SenderConnection);
//
//						if (all.Count > 0)
//						{
//							NetOutgoingMessage om = s_server.CreateMessage();
//							om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
//							s_server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
//						}
						break;
					default:
						Console.WriteLine("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
						break;
				}
			}        
        }
        
    }
}
