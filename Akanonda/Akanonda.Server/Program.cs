using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akanonda.GameLibrary;
using System.Threading;
using Lidgren.Network;
using System.Timers;
using System.Drawing;
using Akanonda.Properties;

namespace Akanonda
{
    public class Program
    {
        private static NetServer netserver;
        private static Game game;
        private static NetServer chatServer;
        private static volatile bool _chatStopped = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Akanonda Server");
            Console.WriteLine("---------------");

            Settings settings = new Settings();

            // NetServer START
            NetPeerConfiguration netconfig = new NetPeerConfiguration("game");
            netconfig.MaximumConnections = settings.MaxConnections;
			netconfig.Port = settings.LocalPort;
			netserver = new NetServer(netconfig);

            
            

            NetPeerConfiguration config = new NetPeerConfiguration("chat");
            config.MaximumConnections = 100;
            config.Port = 1338;
            chatServer = new NetServer(config);
           
            


            if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            
			netserver.RegisterReceivedCallback(new SendOrPostCallback(ReceiveData)); 
		
			netserver.Start();
            //chatServer.Start();

            StartChat();

            // NetServer END
			
            // Game START
            game = Game.Instance;
            //game.setFieldSize(250, 250);
            //game.addPlayer("Martin", Color.Blue, Guid.NewGuid());
            
            
            
            // Game END            
            

            // GameTimer START
            System.Timers.Timer timer = new System.Timers.Timer(100);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            // GameTimer END
                        
            // ConsoleCommand START
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
                    case "stop Chat":
                        StopChat();
                        Console.WriteLine("Chat stopped");
                        break;
                    case "start Chat":
                        StartChat();
                        Console.WriteLine("Chat started");
                        break;
                    default:
                        Console.WriteLine("Command not found.");
                        break;
                }
                
                Console.Write("Command: ");
            }
			// ConsoleCommand END
        }



        public static void StartChat()
        {
            _chatStopped = false;
            chatServer.Start();
            Thread t = new Thread(new ThreadStart(getMessages));
            t.Start();
        }


        public static void StopChat()
        {
            _chatStopped = true;
            chatServer.Shutdown("Chat disabled by Server administrator");
        }


        //object sender, EventArgs e
        private static void getMessages()
        {
            
            while (!_chatStopped)
            {
                NetIncomingMessage im;
                while ((im = chatServer.ReadMessage()) != null)
                {
                   
                    // handle incoming message
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            string text = im.ReadString();
                            Console.WriteLine(text);
                            //Output(text);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                            string reason = im.ReadString();
                            //Output(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                            UpdateConnectionsList();

                            if (im.SenderConnection.RemoteHailMessage != null && status == NetConnectionStatus.Connected)
                            {
                                string remotehailmessage = im.SenderConnection.RemoteHailMessage.ReadString();
                                string[] remotehailmessagearray = remotehailmessage.Split(';');

                                game.AddLobbyPlayer(remotehailmessagearray[2], Color.FromArgb(Convert.ToInt32(remotehailmessagearray[3])), Guid.Parse(remotehailmessagearray[0]));

                                //Console.WriteLine("Player connected! \t GUID: " + Guid.Parse(remotehailmessagearray[0]) + " name: " + remotehailmessagearray[1].ToString() + " color: " + Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])));
                                List<NetConnection> allPlayers = chatServer.Connections;
                                if (allPlayers.Count > 0)
                                {
                                    string inLobby = "PlayersInLobby;";
                                    string inGame = "PlayersInGame;";
                                    foreach (GameLibrary.Player p in game.LobbyList)
                                    {
                                        inLobby += p.name + ";";
                                    
                                    }
                                    foreach (GameLibrary.Player p in game.PLayerList)
                                    {
                                        inGame += p.name + ";";
                                    }
                                    inGame = inGame.Remove(inGame.Length - 1);
                                    inLobby = inLobby.Remove(inLobby.Length - 1);
                                    NetOutgoingMessage om = chatServer.CreateMessage();
                                    om.Write(inLobby + ":" + inGame);
                                    chatServer.SendMessage(om, allPlayers, NetDeliveryMethod.ReliableOrdered, 0);
                                    }
                            }
                            break;

                        case NetIncomingMessageType.Data:
                            // incoming chat message from a client
                            

                            //Output("Broadcasting '" + chat + "'");

                            // broadcast this to all connections, except sender
                            List<NetConnection> all = chatServer.Connections; // get copy
                            //all.Remove(im.SenderConnection);

                            if (all.Count > 0)
                            {
                                //string remotehailmessage = im.SenderConnection.RemoteHailMessage.ReadString();
                                //string[] remotehailmessagearray = remotehailmessage.Split(';');
                                //string chat = remotehailmessagearray[1];
                                string chat = im.ReadString();

                                if (chat.StartsWith("UpdateLobbyLists"))
                                {
                                    string inLobby = "PlayersInLobby;";
                                    string inGame = "PlayersInGame;";
                                    foreach (GameLibrary.Player p in game.LobbyList)
                                    {
                                        inLobby += p.name + ";";

                                    }
                                    foreach (GameLibrary.Player p in game.PLayerList)
                                    {
                                        inGame += p.name + ";";
                                    }
                                    inGame = inGame.Remove(inGame.Length - 1);
                                    inLobby = inLobby.Remove(inLobby.Length - 1);
                                    NetOutgoingMessage om = chatServer.CreateMessage();
                                    om.Write(inLobby + ":" + inGame);
                                    chatServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                                }
                                else
                                {


                                    string[] chatMessage = chat.Split(';');
                                    NetOutgoingMessage om = chatServer.CreateMessage();
                                    //om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
                                    om.Write(game.getLobbyPlayerName(Guid.Parse(chatMessage[0])) + ": " + chatMessage[1]);
                                    chatServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("(CHAT):Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                            break;
                    }
                }
                Thread.Sleep(1);
            }
        }

        private static void UpdateConnectionsList()
        {
            

            foreach (NetConnection conn in chatServer.Connections)
            {
                string str = NetUtility.ToHexString(conn.RemoteUniqueIdentifier) + " from " + conn.RemoteEndPoint.ToString() + " [" + conn.Status + "]";
                Console.WriteLine(str);
            }
        }



        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            game.gametick();
            game.DetectCollision();

            /*  
             *  DetectCollison speichert alle fälle die gefunden werden in das dictionary.
             *  Anhand der Guid kann der player identifiziert werden, CollisonType gibt an was passiert ist:
             *  Collision mit Wand, sich selber oder anderem Spieler
            */
            
            byte[] gamebyte = SerializeHelper.ObjectToByteArray(game);
            
            NetOutgoingMessage sendMsg = netserver.CreateMessage();
            sendMsg.Write(Convert.ToInt32(gamebyte.Length));
            sendMsg.Write(gamebyte);

            netserver.SendToAll(sendMsg, NetDeliveryMethod.ReliableSequenced);
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

                        string reason = im.ReadString();
                                                                     						
                        if(im.SenderConnection.RemoteHailMessage != null && status == NetConnectionStatus.Connected)
                        {
                            string remotehailmessage = im.SenderConnection.RemoteHailMessage.ReadString();
                            string[] remotehailmessagearray = remotehailmessage.Split(';');

                            game.addPlayer(remotehailmessagearray[1], Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])), Guid.Parse(remotehailmessagearray[0]));
                            game.RemoveLobbyPlayer(Guid.Parse(remotehailmessagearray[0]));
                            //game.AddLobbyPlayer(remotehailmessagearray[1], Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])), Guid.Parse(remotehailmessagearray[0]));

                            Console.WriteLine("Player connected! \t GUID: " + Guid.Parse(remotehailmessagearray[0]) + " name: " + remotehailmessagearray[1].ToString() + " color: " + Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])));
                        }

                        if (status == NetConnectionStatus.Disconnected)
                        {
                            game.removePlayer(Guid.Parse(reason));
                            game.RemoveLobbyPlayer(Guid.Parse(reason));
                            Console.WriteLine("Player disconnected! \t GUID: " + Guid.Parse(reason));
                        }

						
						
                        //Console.WriteLine(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);



                        
                        //Update user status
						//UpdateConnectionsList();
						break;
					case NetIncomingMessageType.Data:
						// incoming chat message from a client

                        //Console.WriteLine(im.ReadString());
						//Console.WriteLine(im.ReadInt32());

                        Guid remoteplayer = new Guid(im.ReadString());
                        PlayerSteering remoteplayersteering = (PlayerSteering)im.ReadInt32();

                        game.setsteering(remoteplayer, remoteplayersteering);

						
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
				netserver.Recycle(im);
			}        
        }
    }
}
