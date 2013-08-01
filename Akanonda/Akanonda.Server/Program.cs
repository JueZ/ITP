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
        private static Settings settings = new Settings();
        public static System.Timers.Timer SurvivalTimer;

        static void Main(string[] args)
        {
            Console.WriteLine("Akanonda Server");
            Console.WriteLine("---------------");

            

            // NetServer START
            NetPeerConfiguration netconfig = new NetPeerConfiguration("game");
            netconfig.MaximumConnections = settings.MaxConnections;
			netconfig.Port = settings.LocalPort;
			netserver = new NetServer(netconfig);

            
            

            NetPeerConfiguration config = new NetPeerConfiguration("chat");
            config.MaximumConnections = 100;
            config.Port = 1338;
            chatServer = new NetServer(config);


            SurvivalTimer = new System.Timers.Timer();
            SurvivalTimer.Interval = 1000;
            SurvivalTimer.Elapsed += new ElapsedEventHandler(gameSecondTick);
            SurvivalTimer.Start();

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
            

            // GameTimer START                                  set gameSpeed here
            System.Timers.Timer timer = new System.Timers.Timer(settings.GameSpeed);
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


        public static void gameSecondTick(object source, ElapsedEventArgs e)
        {
            foreach (Player p in game.PLayerList)
            {
                p.SurvivalTime++;
            }

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

                            //UpdateConnectionsList()

                            if (im.SenderConnection.RemoteHailMessage != null && status == NetConnectionStatus.Connected)
                            {
                                string remotehailmessage = im.SenderConnection.RemoteHailMessage.ReadString();
                                string[] remotehailmessagearray = remotehailmessage.Split(';');

                                game.AddLobbyPlayer(remotehailmessagearray[1], Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])), Guid.Parse(remotehailmessagearray[0]));
                                
                                //Console.WriteLine("Player connected! \t GUID: " + Guid.Parse(remotehailmessagearray[0]) + " name: " + remotehailmessagearray[1].ToString() + " color: " + Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])));
                                UpdateLobbyLists();
                                Console.WriteLine("[Chat]Player connected! \t GUID: " + Guid.Parse(remotehailmessagearray[0]) + " name: " + remotehailmessagearray[1].ToString() + " color: " + Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])));
                            }

                            if (status == NetConnectionStatus.Disconnected)
                            {
                                game.removePlayer(Guid.Parse(reason));
                                game.RemoveLobbyPlayer(Guid.Parse(reason));
                                Console.WriteLine("[Chat]Player disconnected! \t GUID: " + Guid.Parse(reason));
                                UpdateLobbyLists();
                                
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
                                Console.WriteLine(chat);
                                


                                    string[] chatMessage = chat.Split(';');
                                    NetOutgoingMessage om = chatServer.CreateMessage();
                                    //om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
                                    om.Write(game.getLobbyPlayerName(Guid.Parse(chatMessage[0])) + ": " + chatMessage[1]);
                                    chatServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                                
                            }
                            break;
                        default:
                            Console.WriteLine("[Chat]:Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
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
            UpdateLobbyLists();
            foreach (KeyValuePair<Guid, CollisionType> key in game.CollisionList)
            {
                Game.Instance.setScoreToLobbyPlayer(key.Key);
                Game.Instance.addDeadRemoveLivingPlayer(key.Key);
            }

            List<Guid> removeDeadList = new List<Guid>();
            
                foreach (Player player in game.DeadList)
                {
                    if (player.playerbody.Count > 0)
                        player.playerbody.RemoveAt(0);
                    else
                    {
                        removeDeadList.Add(player.guid);
                        //game.removeDeadPlayer(player.guid);


                        if(game.goldenAppleDict.ContainsKey(player.guid))
                            game.goldenAppleDict.Remove(player.guid);

                        if (game.redAppleDict.ContainsKey(player.guid))
                            game.redAppleDict.Remove(player.guid);

                        if (game.iGoFastDict.ContainsKey(player.guid))
                            game.iGoFastDict.Remove(player.guid);

                        if (game.iGoSlowDict.ContainsKey(player.guid))
                            game.iGoSlowDict.Remove(player.guid);

                        if (game.othersGoFastDict.ContainsKey(player.guid))
                            game.othersGoFastDict.Remove(player.guid);

                        if (game.othersGoSlowDict.ContainsKey(player.guid))
                            game.othersGoSlowDict.Remove(player.guid);

                        if (game.rabiesDict.ContainsKey(player.guid))
                            game.rabiesDict.Remove(player.guid);

                        if (game.iGoThroughWallsDict.ContainsKey(player.guid))
                            game.iGoThroughWallsDict.Remove(player.guid);
                    }
                }

                foreach (Guid guid in removeDeadList)
                {
                    game.removeDeadPlayer(guid);
                }
            
            


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
                            if (remotehailmessagearray[3] == "playing")
                            {
                                game.addPlayer(remotehailmessagearray[1], Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])), Guid.Parse(remotehailmessagearray[0]));
                                //game.setScoreToGamePlayer(Guid.Parse(remotehailmessagearray[0]));
                                game.AddPowerUp(PowerUp.PowerUpKind.morePowerUps); // For testing
                                game.AddPowerUp(PowerUp.PowerUpKind.rabies); // For testing
                                //game.AddPowerUp(PowerUp.PowerUpKind.biggerWalls); // For testing
                                //game.RemoveLobbyPlayer(Guid.Parse(remotehailmessagearray[0]));
                                //game.AddLobbyPlayer(remotehailmessagearray[1], Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])), Guid.Parse(remotehailmessagearray[0]));
                                Console.WriteLine("[Game]Player <playing>! \t GUID: " + Guid.Parse(remotehailmessagearray[0]) + " name: " + remotehailmessagearray[1].ToString() + " color: " + Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])));

                            }
                            else if (remotehailmessagearray[3] == "justWatching")
                            {
                                game.removePlayer(Guid.Parse(remotehailmessagearray[0]));
                                //game.AddLobbyPlayer(remotehailmessagearray[1], Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])), Guid.Parse(remotehailmessagearray[0]));
                                Console.WriteLine("[Game]Player connected <watching>! \t GUID: " + Guid.Parse(remotehailmessagearray[0]) + " name: " + remotehailmessagearray[1].ToString() + " color: " + Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])));

                            }
                            else if (remotehailmessagearray[3] == "dead")
                            {
                               // game.removePlayer(Guid.Parse(remotehailmessagearray[0]));
                                game.addDeadRemoveLivingPlayer(Guid.Parse(remotehailmessagearray[0]));
                                //game.AddLobbyPlayer(remotehailmessagearray[1], Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])), Guid.Parse(remotehailmessagearray[0]));
                                Console.WriteLine("[Game]Player died <now watching>! \t GUID: " + Guid.Parse(remotehailmessagearray[0]) + " name: " + remotehailmessagearray[1].ToString() + " color: " + Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])));

                            }
                                //UpdateLobbyLists();

                        }

                        if (status == NetConnectionStatus.Disconnected)
                        {
                            if (reason != "Connection timed out" && Guid.Parse(reason) != null)
                            {
                                game.removePlayer(Guid.Parse(reason));
                                game.RemoveLobbyPlayer(Guid.Parse(reason));
                                UpdateLobbyLists();
                                Console.WriteLine("[Game]Player disconnected! \t GUID: " + Guid.Parse(reason));
                            }
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

                        //UpdateLobbyLists();
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

        private static string getColorFormARGB(Color colorToCheck)
        {
            string name = "Unknown";
            foreach (KnownColor kc in Enum.GetValues(typeof(KnownColor)))
            {
                Color known = Color.FromKnownColor(kc);
                if (colorToCheck.ToArgb() == known.ToArgb())
                {
                    name = known.Name;
                }
            }
            return name;
        }


        private static void UpdateLobbyLists()
        {
            List<NetConnection> all = chatServer.Connections; // get copy
            //all.Remove(im.SenderConnection);

            if (all.Count > 0)
            {
                //string remotehailmessage = im.SenderConnection.RemoteHailMessage.ReadString();
                //string[] remotehailmessagearray = remotehailmessage.Split(';');
                //string chat = remotehailmessagearray[1];
                
                
                    string inLobby = "PlayersInLobby;";
                    string inGame = "PlayersInGame;";
                    foreach (GameLibrary.Player p in game.LobbyList)
                    {
                        if(game.getPlayerName(p.guid) == "")
                            inLobby += p.name + "-" + getColorFormARGB(p.color) + "-" + p.score + ";";

                    }
                    foreach (GameLibrary.Player p in game.PLayerList)
                    {
                        inGame += p.name + "-" + getColorFormARGB(p.color) + "-" + p.score + ";";
                    }
                    inGame = inGame.Remove(inGame.Length - 1);
                    inLobby = inLobby.Remove(inLobby.Length - 1);
                    NetOutgoingMessage om = chatServer.CreateMessage();
                    om.Write(inLobby + ":" + inGame);
                    chatServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                
            }
        }

    }
}
