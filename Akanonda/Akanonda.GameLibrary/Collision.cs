using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{
    [Serializable()]
    public enum CollisionType
    {
        ToPlayer,
        ToWall,
        ToSelf,
        ToDead
    }

    [Serializable()]
    public class Collision
    {
        private static Game game;
        private int _x;
        private int _y;

        public Collision(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void setCollisionFieldSize(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Dictionary<Guid, CollisionType> DetectCollision(List<Player> playerList)
        {
            game = Game.Instance;
            Dictionary<Guid, CollisionType> collisions = new Dictionary<Guid, CollisionType>();
            
            foreach (Player player in playerList) // current player to check head
            {
                int[] headCoordinates = player.playerbody[player.playerbody.Count-1];

                List<Guid> deletePowerUpList = new List<Guid>();
                //check for PowerUp Collision
                foreach (PowerUp power in Game.Instance.PowerUpList)
                {
                    foreach (int[] array in power.PowerUpLocation)
                    {
                        if (headCoordinates[0] == array[0] && headCoordinates[1] == array[1]) // current head collides with PowerUp
                        {
                            switch(power.kind){
                                case PowerUp.PowerUpKind.openWalls:
                                    game.powerUpCounters[Game.openWalls] += 1000;
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.othersGoFast:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (Game.Instance.othersGoFastDict.ContainsKey(Game.Instance.PLayerList[i].guid))
                                                    Game.Instance.othersGoFastDict[Game.Instance.PLayerList[i].guid] = 100;
                                                else
                                                    Game.Instance.othersGoFastDict.Add(Game.Instance.PLayerList[i].guid, 100);
                                            }
                                        }
                                    }
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.iGoFast:
                                     if(Game.Instance.iGoFastDict.ContainsKey(player.guid))
                                        Game.Instance.iGoFastDict[player.guid] = 100;
                                    else
                                        Game.Instance.iGoFastDict.Add(player.guid, 100);

                                     deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.iGoSlow:
                                    if(Game.Instance.iGoSlowDict.ContainsKey(player.guid))
                                        Game.Instance.iGoSlowDict[player.guid] = 100;
                                    else
                                        Game.Instance.iGoSlowDict.Add(player.guid, 100);

                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.goldenApple:
                                    if(Game.Instance.goldenAppleDict.ContainsKey(player.guid))
                                        Game.Instance.goldenAppleDict[player.guid] += 20;
                                    else
                                        Game.Instance.goldenAppleDict.Add(player.guid, 20); //sets how much a player will grow when he eats the golden apple

                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.redApple:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (Game.Instance.redAppleDict.ContainsKey(Game.Instance.PLayerList[i].guid))
                                                    Game.Instance.redAppleDict[Game.Instance.PLayerList[i].guid] = 20;
                                                else
                                                    Game.Instance.redAppleDict.Add(Game.Instance.PLayerList[i].guid, 20); //sets how much a player will loose when he eats the red apple
                                            }
                                        }
                                    }

                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.rabies:
                                    if (Game.Instance.rabiesDict.ContainsKey(player.guid))
                                    {
                                        Game.Instance.rabiesDict[player.guid]++;
                                    }
                                    else
                                    {
                                        Game.Instance.rabiesDict.Add(player.guid, 1); //sets how many times a player can bite
                                    }
                                    if (Game.Instance.iGoFastDict.ContainsKey(player.guid))
                                        Game.Instance.iGoFastDict[player.guid] += 50;
                                    else
                                        Game.Instance.iGoFastDict.Add(player.guid, 50);
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.movePowerUps:
                                    game.powerUpCounters[Game.moveAllPowerUps] += 1000;
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.othersGoSlow:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (Game.Instance.othersGoSlowDict.ContainsKey(Game.Instance.PLayerList[i].guid))
                                                    Game.Instance.othersGoSlowDict.Add(Game.Instance.PLayerList[i].guid, 100);
                                                else
                                                    Game.Instance.othersGoSlowDict[Game.Instance.PLayerList[i].guid] = 100;
                                            }
                                        }
                                    }
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.closingWalls:
                                    game.powerUpCounters[Game.closingWalls] += 40;
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.biggerWalls:
                                    game.powerUpCounters[Game.biggerWalls] += 40;
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.morePowerUps:
                                    deletePowerUpList.Add(power.guid);
                                    game.powerUpPopUpRate = 10;
                                    break;
                                case PowerUp.PowerUpKind.iGoThroughWalls:
                                    if (Game.Instance.iGoThroughWallsDict.ContainsKey(player.guid))
                                        Game.Instance.iGoThroughWallsDict[player.guid] = 100;
                                    else
                                        Game.Instance.iGoThroughWallsDict.Add(player.guid, 100);

                                    deletePowerUpList.Add(power.guid);
                                    break;
                            }
                        }
                    }
                }

                foreach (Guid guid in deletePowerUpList)
                {
                    Game.Instance.RemovePowerUp(guid);
                }


                // collision with head to wall
                if (headCoordinates[0] < 0 || headCoordinates[0] > _x - 1 || headCoordinates[1] < 0 || headCoordinates[1] > _y - 1)
                {
                    if (game.powerUpCounters[Game.openWalls] > 0 || Game.Instance.iGoThroughWallsDict.ContainsKey(player.guid))
                    {
                        PowerUp.openTheWalls(headCoordinates);
                    }
                    else
                    {
                        if (!collisions.ContainsKey(player.guid))
                            collisions.Add(player.guid, CollisionType.ToWall);
                    }
                }


                foreach (Player deadPlayer in Game.Instance.DeadList) // player with which current head is checked against (can be same as above)
                {
                    int size = deadPlayer.playerbody.Count;
                    int[] array;

                    for (int i = 0; i < size - 1; i++) // skip head, otherwise loop would always find a collision
                    {
                        array = deadPlayer.playerbody[i];
                        if (headCoordinates[0] == array[0] && headCoordinates[1] == array[1]) // current head collides with own tail
                        {
                            if (!Game.Instance.rabiesDict.ContainsKey(player.guid))
                            {
                                Console.WriteLine("Player " + player.guid.ToString() + " collides with himself!");
                                if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                    collisions.Add(player.guid, CollisionType.ToDead);
                            }
                            else
                            {
                                if (deadPlayer.playerbody.Count > i)
                                    deadPlayer.playerbody.RemoveRange(0, i);
                                Game.Instance.rabiesDict.Remove(player.guid);
                            }
                        }
                    }
                }



                foreach (Player otherPlayer in playerList) // player with which current head is checked against
                {
                    if (player.guid != otherPlayer.guid) // check against other players
                    {
                        int x = 0;
                        foreach (int[] array in otherPlayer.playerbody)
                        {
                            x++;
                            if (headCoordinates[0] == array[0] && headCoordinates[1] == array[1]) // current head collides with other player
                            {
                                // Collision!
                                if (!Game.Instance.rabiesDict.ContainsKey(player.guid))
                                {
                                    Console.WriteLine("Player " + player.guid.ToString() + " collides with another player!");
                                    if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                        collisions.Add(player.guid, CollisionType.ToPlayer);
                                }
                                else
                                {
                                    if (otherPlayer.playerbody.Count > x)
                                    {
                                        otherPlayer.playerbody.RemoveRange(0, x);
                                        otherPlayer.score -= x;
                                    }
                                    Game.Instance.rabiesDict.Remove(player.guid);
                                }
                            }
                            
                        }
                    }
                    else // check against player itself
                    {
                        int size = otherPlayer.playerbody.Count;
                        int[] array;

                        for (int i = 0; i < size-1; i++) // skip head, otherwise loop would always find a collision
                        {
                            array = otherPlayer.playerbody[i];
                            if (headCoordinates[0] == array[0] && headCoordinates[1] == array[1]) // current head collides with own tail
                            {
                                // Collision!
                                if (!Game.Instance.rabiesDict.ContainsKey(player.guid))
                                {
                                    Console.WriteLine("Player " + player.guid.ToString() + " collides with himself!");
                                    if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                        collisions.Add(player.guid, CollisionType.ToSelf);
                                }
                                else
                                {
                                    if (player.playerbody.Count > i)
                                        player.playerbody.RemoveRange(0, i);
                                    player.score -= i;
                                    Game.Instance.rabiesDict.Remove(player.guid);
                                }
                            }
                        }
                    }
                }
                
            }

            return collisions;
        }
    }
}
