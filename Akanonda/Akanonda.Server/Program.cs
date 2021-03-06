﻿using System;
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
        public static System.Timers.Timer gameSpeedTimer;
        private static List<NetConnection> adminList = new List<NetConnection>();
        private static Dictionary<Guid, NetConnection> playerClientConn = new Dictionary<Guid, NetConnection>();
        private static List<System.Net.IPAddress> banList = new List<System.Net.IPAddress>();
        static void Main(string[] args)
        {
            Console.WriteLine("Akanonda Server");
            Console.WriteLine("---------------");

            // NetServer START
            NetPeerConfiguration netconfig = new NetPeerConfiguration("game");
            netconfig.MaximumConnections = settings.MaxConnections;
			netconfig.Port = settings.GamePort;
			netserver = new NetServer(netconfig);

            NetPeerConfiguration config = new NetPeerConfiguration("chat");
            config.MaximumConnections = 100;
            config.Port = settings.ChatPort;
            chatServer = new NetServer(config);

            SurvivalTimer = new System.Timers.Timer();
            SurvivalTimer.Interval = 1000;
            SurvivalTimer.Elapsed += new ElapsedEventHandler(gameSecondTick);
            SurvivalTimer.Start();

            if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            
			netserver.RegisterReceivedCallback(new SendOrPostCallback(ReceiveData));

            netserver.Start();
            StartChat();

            game = Game.Instance;
  
            gameSpeedTimer = new System.Timers.Timer(settings.GameSpeed);
            gameSpeedTimer.Elapsed += new ElapsedEventHandler(gameSpeedTimer_Elapsed);
            gameSpeedTimer.Enabled = true;
            
                        
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
                    case "kick Player":
                        Console.WriteLine("Enter Player name:");
                        string playerToKick = Console.ReadLine();
                        Player playerToFind = game.PLayerList.Find(item => item.name == playerToKick);
                        if (playerToFind != null)
                        {
                            game.addDeadRemoveLivingPlayer(playerToFind.guid);
                        }
                        break;
                    default:
                        Console.WriteLine("Command not found.");
                        break;
                }
                Console.Write("Command: ");
            }
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

        private static void getMessages()
        {
            while (!_chatStopped)
            {
                NetIncomingMessage im;
                while ((im = chatServer.ReadMessage()) != null)
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

                            if (im.SenderConnection.RemoteHailMessage != null && status == NetConnectionStatus.Connected)
                            {
                                try
                                {
                                    string remotehailmessage = im.SenderConnection.RemoteHailMessage.ReadString();
                                    string[] remotehailmessagearray = remotehailmessage.Split(';');
                                    playerClientConn.Add(Guid.Parse(remotehailmessagearray[0]), im.SenderConnection);
                                    if (banList.Contains(im.SenderConnection.RemoteEndPoint.Address))
                                    {
                                        NetOutgoingMessage om2 = chatServer.CreateMessage();
                                        om2.Write("ban:" + Guid.Parse(remotehailmessagearray[0]));
                                        chatServer.SendMessage(om2, playerClientConn[Guid.Parse(remotehailmessagearray[0])], NetDeliveryMethod.ReliableOrdered, 0);
                                    }
                                    game.AddLobbyPlayer(remotehailmessagearray[1], Color.FromArgb(Convert.ToInt32(remotehailmessagearray[2])), Guid.Parse(remotehailmessagearray[0]));
                                }
                                catch
                                {
                                    Console.WriteLine("a wron Guid format was sent!!");
                                }
                            }

                            if (status == NetConnectionStatus.Disconnected)
                            {
                                try
                                {
                                    game.removePlayer(Guid.Parse(reason));
                                    game.RemoveLobbyPlayer(Guid.Parse(reason));
                                    playerClientConn.Remove(Guid.Parse(reason));
                                }
                                catch
                                {
                                    Console.WriteLine("a wron Guid format was sent!!");
                                }
                            }
                            break;

                        case NetIncomingMessageType.Data:
                            // broadcast this to all connections, except sender
                            List<NetConnection> all = chatServer.Connections; // get copy
                            if (all.Count > 0)
                            {
                                NetOutgoingMessage om = chatServer.CreateMessage();
                                string chat = im.ReadString();
                                string[] chatMessage = chat.Split(';');
                                if (adminList.Contains(im.SenderConnection))
                                {
                                    if (chatMessage[1].StartsWith("kick") || chatMessage[1].StartsWith("ban"))
                                    {
                                        string[] playerToKick = chatMessage[1].Split(':');
                                        Player playerToFind = game.PLayerList.Find(item => item.name == playerToKick[1]);
                                        if(playerToFind == null)
                                            playerToFind = game.LobbyList.Find(item => item.name == playerToKick[1]);
                                        if (playerToFind != null)
                                        {
                                            NetOutgoingMessage om2 = chatServer.CreateMessage();

                                            if (chatMessage[1].StartsWith("kick")){
                                                if (playerClientConn.ContainsKey(playerToFind.guid))
                                                {
                                                    om2.Write("kick:"+playerToFind.guid.ToString());
                                                    om.Write(playerToKick[1] + " was kicked successfully");
                                                }
                                            }
                                            if (chatMessage[1].StartsWith("ban"))
                                            {
                                                if (playerClientConn.ContainsKey(playerToFind.guid))
                                                {
                                                    om2.Write("ban:" + playerToFind.guid.ToString());
                                                    banList.Add(playerClientConn[playerToFind.guid].RemoteEndPoint.Address);
                                                    om.Write(playerToKick[1] + " was banned successfully");
                                                }   
                                            }
                                            chatServer.SendMessage(om2, playerClientConn[playerToFind.guid], NetDeliveryMethod.ReliableOrdered, 0);
                                        }
                                        else
                                        {
                                            if (playerToKick[1] != null && playerToKick[1] != "")
                                            {
                                                if (playerToFind == null)
                                                    om.Write("The Player wasn't found");
                                                else
                                                    om.Write("The Player " + playerToKick[1] + " could not be kicked/banned");
                                            }
                                            else
                                                om.Write("wrong use of kick/ban Command\nPlease use 'kick:playername' or 'ban:playername'");
                                        }
                                    }
                                    else
                                    {
                                        switch (chatMessage[1])
                                        {
                                            case "start":
                                                om.Write("Game allready started");
                                                break;
                                            case "status":
                                                string ServerStatus = "Connected Players: " + netserver.ConnectionsCount.ToString();
                                                ServerStatus += "\n Configuration: " + netserver.Configuration.ToString();
                                                om.Write(ServerStatus);
                                                break;
                                            case "exit":
                                                netserver.Shutdown("Exit");
                                                Environment.Exit(0);
                                                break;
                                            case "stop Chat":
                                                StopChat();
                                                om.Write("Chat stopped");
                                                break;
                                            case "start Chat":
                                                StartChat();
                                                om.Write("Chat started");
                                                break;
                                            case "help":
                                            case "Help":
                                                string helptext = "Possible Commands:\n'start' - Start Game\n'status' - show Connected Players and Configuration\n";
                                                helptext += "'exit' - Kill Server\n'stop Chat' & 'start Chat'\n 'kick:playername' - kick player\n";
                                                helptext += "'gamespeed up/down'\n 'add/remove all powerups'\nLogout";
                                                om.Write(helptext);
                                                break;
                                            case "add all powerups":
                                                foreach (PowerUp.PowerUpKind kind in (PowerUp.PowerUpKind[])Enum.GetValues(typeof(PowerUp.PowerUpKind)))
                                                {
                                                    game.AddPowerUp(kind);
                                                }
                                                om.Write("All Powerup kinds added");
                                                break;
                                            case "remove all powerups":
                                                game.PowerUpList.Clear();
                                                om.Write("All Powerups removed");
                                                break;
                                            case "gamespeed up":
                                                gameSpeedTimer.Interval -= 10;
                                                om.Write("Game speeded up");
                                                break;
                                            case "gamespeed down":
                                                gameSpeedTimer.Interval += 10;
                                                om.Write("Game slowed down");
                                                break;
                                            case "Logout":
                                            case "logout":
                                                adminList.Remove(im.SenderConnection);
                                                om.Write("You are now logged out");
                                                break;
                                            default:
                                                om.Write("Command not found!\nType help for more information");
                                                break;
                                        }
                                    }
                                    if(om.LengthBits != 0)
                                    chatServer.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                    else
                                    {
                                        try
                                        {
                                            om.Write(game.getLobbyPlayerName(Guid.Parse(chatMessage[0])) + ": " + chatMessage[1]);
                                            chatServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                                        }
                                        catch
                                        {
                                            Console.WriteLine("a wron Guid format was sent!!");
                                        }
                                    }
                                }
                                else
                                {
                                    if (chatMessage[1] == "LOGINAKANONDAADMIN")
                                    {
                                        adminList.Add(im.SenderConnection);
                                        om.Write("You are now logged in as Admin");
                                        chatServer.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            om.Write(game.getLobbyPlayerName(Guid.Parse(chatMessage[0])) + ": " + chatMessage[1]);
                                            chatServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                                        }
                                        catch
                                        {
                                            Console.WriteLine("a wron Guid format was sent!!");
                                        }
                                    }
                                }
                            }
                            break;
                        default:
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



        static void gameSpeedTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            game.gametick();
            
            foreach (KeyValuePair<Guid, CollisionType> key in game.CollisionList)
            {
                
                Game.Instance.clearModificationListOnDead(key.Key);
                Game.Instance.addDeadRemoveLivingPlayer(key.Key);
                Game.Instance.setScoreToLobbyPlayer(key.Key);
            }

            List<Guid> removeDeadList = new List<Guid>();
            
                foreach (Player player in game.DeadList)
                {
                    if (player.playerbody.Count > 0)
                        player.playerbody.RemoveAt(0);
                    else
                    {
                        removeDeadList.Add(player.guid);
                        if(game.powerUpModificationList.ContainsKey(player.guid))
                            game.powerUpModificationList.Remove(player.guid);
                    }
                }

            foreach (Guid guid in removeDeadList)
            {
                game.removeDeadPlayer(guid);
            }
            removeDeadList.Clear();
            UpdateLobbyLists();
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
                            
                        }

                        if (status == NetConnectionStatus.Disconnected)
                        {
                            try
                                {
                            if (reason != "Connection timed out" && Guid.Parse(reason) != null) //fix for server crashes
                            {
                                    im.SenderConnection.Disconnect("ByeBye");
                                    game.addDeadRemoveLivingPlayer(Guid.Parse(reason));
                                }
                            }
                            catch
                            {
                                Console.WriteLine("a wron Guid format was sent!!");
                            }
                        }
						break;
					case NetIncomingMessageType.Data:
                        string firstIM = im.ReadString();
                        if (firstIM == "ConnectToGame")
                        {
                            try
                            {
                                game.addPlayer(im.ReadString(), Color.FromArgb(Convert.ToInt32(im.ReadString())), Guid.Parse(im.ReadString()));
                            }
                            catch
                            {
                                Console.WriteLine("a wron Guid format was sent!!");
                            }
                        }
                        else
                        {
                            Guid remoteplayer = new Guid(firstIM);
                            PlayerSteering remoteplayersteering = (PlayerSteering)im.ReadInt32();
                            bool use4Buttons = (bool)im.ReadBoolean();
                            game.setsteering(remoteplayer, remoteplayersteering, use4Buttons);
                        }
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
            if (all.Count > 0)
            {
                string inLobby = "PlayersInLobby;";
                string inGame = "PlayersInGame;";
                foreach (GameLibrary.Player p in game.LobbyList)
                {
                    if(game.getPlayerName(p.guid) == "")
                        inLobby += p.name + "-" + getColorFormARGB(p.color) + "-" + p.score + ";";

                }
                List<Guid> noDuplicatesList = new List<Guid>();
                foreach (GameLibrary.Player p in game.PLayerList)
                {
                    if(!noDuplicatesList.Contains(p.guid))
                    inGame += p.name + "-" + getColorFormARGB(p.color) + "-" + p.score + ";";
                    noDuplicatesList.Add(p.guid);
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
