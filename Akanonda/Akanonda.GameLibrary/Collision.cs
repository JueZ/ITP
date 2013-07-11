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

        private int _x;
        private int _y;

        public Collision(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void setCollision(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Dictionary<Guid, CollisionType> DetectCollision(List<Player> playerList)
        {

            Dictionary<Guid, CollisionType> collisions = new Dictionary<Guid, CollisionType>();
            
            foreach (Player player in playerList) // current player to check head
            {
                int[] headCoordinates = player.playerbody[player.playerbody.Count-1];
                //Console.WriteLine("x: " + coordinates[0].ToString() + "  y: " + coordinates[1].ToString());


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
                                    Game.Instance.goThroughWallCounter += 100;
                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.othersGoFast:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (Game.Instance.othersGoFastList.ContainsKey(Game.Instance.PLayerList[i].guid))
                                                    Game.Instance.othersGoFastList[Game.Instance.PLayerList[i].guid] = 100;
                                                else
                                                    Game.Instance.othersGoFastList.Add(Game.Instance.PLayerList[i].guid, 100);
                                            }
                                        }
                                    }
                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.iGoFast:
                                     if(Game.Instance.iGoFastList.ContainsKey(player.guid))
                                        Game.Instance.iGoFastList[player.guid] = 100;
                                    else
                                        Game.Instance.iGoFastList.Add(player.guid, 100);

                                     //deletePowerUpList.Add(power.guid);
                                     Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.iGoSlow:
                                    if(Game.Instance.iGoSlowList.ContainsKey(player.guid))
                                        Game.Instance.iGoSlowList[player.guid] = 100;
                                    else
                                        Game.Instance.iGoSlowList.Add(player.guid, 100);

                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.goldenApple:
                                    if(Game.Instance.goldenAppleDict.ContainsKey(player.guid))
                                        Game.Instance.goldenAppleDict[player.guid] += 20;
                                    else
                                        Game.Instance.goldenAppleDict.Add(player.guid, 20); //sets how much a player will grow when he eats the golden apple

                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
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

                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
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
                                    if (Game.Instance.iGoFastList.ContainsKey(player.guid))
                                        Game.Instance.iGoFastList[player.guid] += 50;
                                    else
                                        Game.Instance.iGoFastList.Add(player.guid, 50);
                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.movePowerUps:
                                    Game.Instance.movePowerUpsCounter += 100;
                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.othersGoSlow:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (Game.Instance.othersGoSlowList.ContainsKey(Game.Instance.PLayerList[i].guid))
                                                    Game.Instance.othersGoSlowList.Add(Game.Instance.PLayerList[i].guid, 100);
                                                else
                                                    Game.Instance.othersGoSlowList[Game.Instance.PLayerList[i].guid] = 100;
                                            }
                                        }
                                    }
                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.closingWalls:
                                    Game.Instance.closingWallsCounter += 40;
                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.biggerWalls:
                                    Game.Instance.biggerWallsCounter += 40;
                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.morePowerUps:
                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.othersGoFast);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.iGoFast);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.openWalls);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.movePowerUps);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.goldenApple);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.redApple);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.rabies);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.othersGoSlow);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.iGoSlow);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.closingWalls);
                                    if (Game.getRandomNumber(0, 10) % 3 == 0)
                                        Game.Instance.AddPowerUp(PowerUp.PowerUpKind.biggerWalls);
                                    
                                    break;
                                case PowerUp.PowerUpKind.iGoThroughWalls:
                                    if (Game.Instance.iGoThroughWalls.ContainsKey(player.guid))
                                        Game.Instance.iGoThroughWalls[player.guid] = 100;
                                    else
                                        Game.Instance.iGoThroughWalls.Add(player.guid, 100);

                                    //deletePowerUpList.Add(power.guid);
                                    Game.Instance.RemovePowerUp(power.guid);
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
                if (headCoordinates[0] < 0 || headCoordinates[0] > _x || headCoordinates[1] < 0 || headCoordinates[1] > _y)
                {
                    // Collision!!
                    

                    if (Game.Instance.goThroughWallCounter > 0 || Game.Instance.iGoThroughWalls.ContainsKey(player.guid))
                    {
                        PowerUp.openTheWalls(headCoordinates);
                    }
                    else
                    {
                        Console.WriteLine("Mit Kopf gegen Wand!");
                        if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
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
                            //player.playerbody.Remove(player.playerbody[0]);
                            // Collision!
                            
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

                                if (Game.Instance.iGoFastList[player.guid] - 1 > 0)
                                    Game.Instance.iGoFastList[player.guid]--;
                                else
                                    Game.Instance.iGoFastList.Remove(player.guid);

                                //if (Game.Instance.rabiesDict[player.guid] - 1 > 0)
                                //    Game.Instance.rabiesDict[player.guid]--;
                                //else
                                //    Game.Instance.rabiesDict.Remove(player.guid);

                                
                               
                            }
                        }
                    }
                }



                foreach (Player p in playerList) // player with which current head is checked against (can be same as above)
                {
                    if (player.guid != p.guid) // check against other players
                    {
                        int x = 0;
                        foreach (int[] array in p.playerbody)
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
                                    if (p.playerbody.Count > x)
                                    {
                                        p.playerbody.RemoveRange(0, x);
                                        p.score -= x;
                                    }
                                    Game.Instance.rabiesDict.Remove(player.guid);

                                    if (Game.Instance.iGoFastList[player.guid] - 1 > 0)
                                        Game.Instance.iGoFastList[player.guid]--;
                                    else
                                        Game.Instance.iGoFastList.Remove(player.guid);
                                    
                                }
                                // other player would be p.guid
                            }
                            
                        }
                    }
                    else // check against player itself
                    {
                        int size = p.playerbody.Count;
                        int[] array;

                        for (int i = 0; i < size-1; i++) // skip head, otherwise loop would always find a collision
                        {
                            array = p.playerbody[i];
                            if (headCoordinates[0] == array[0] && headCoordinates[1] == array[1]) // current head collides with own tail
                            {
                                //player.playerbody.Remove(player.playerbody[0]);
                                // Collision!

                                if (!Game.Instance.rabiesDict.ContainsKey(player.guid))
                                {
                                    Console.WriteLine("Player " + player.guid.ToString() + " collides with himself!");
                                    if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                        collisions.Add(player.guid, CollisionType.ToSelf);
                                }
                                else
                                {
                                    if (p.playerbody.Count > i)
                                        p.playerbody.RemoveRange(0, i);
                                    p.score -= i;
                                    Game.Instance.rabiesDict.Remove(player.guid);

                                    if (Game.Instance.iGoFastList[player.guid] - 1 > 0)
                                        Game.Instance.iGoFastList[player.guid]--;
                                    else
                                        Game.Instance.iGoFastList.Remove(player.guid);

                                    
                                    
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
