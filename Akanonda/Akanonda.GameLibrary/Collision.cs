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

                //check for PowerUps
                foreach (PowerUp power in Game.Instance.PowerUpList)
                {
                    foreach (int[] array in power.PowerUpLocation)
                    {
                        if (headCoordinates[0] == array[0] && headCoordinates[1] == array[1]) // current head collides with PowerUp
                        {
                            switch(power.kind){
                                case PowerUp.PowerUpKind.openWalls:
                                    Game.Instance.goThroughWallCounter += 100;
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.othersGoFast:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (!Game.Instance.othersGoFastList.Contains(Game.Instance.PLayerList[i].guid))
                                                    Game.Instance.othersGoFastList.Add(Game.Instance.PLayerList[i].guid);
                                            }
                                        }
                                        Game.Instance.othersGoFastCounter += 100;
                                    }
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.iGoFast:
                                    Game.Instance.iGoFastList.Add(player.guid);
                                    Game.Instance.iGoFastCounter += 100;
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.iGoSlow:
                                    Game.Instance.iGoSlowList.Add(player.guid);
                                    Game.Instance.iGoSlowCounter += 100;
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.goldenApple:
                                    Game.Instance.goldenAppleDict.Add(player.guid, 20); //sets how much a player will grow when he eats the golden apple
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.redApple:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (!Game.Instance.redAppleDict.ContainsKey(Game.Instance.PLayerList[i].guid))
                                                    Game.Instance.redAppleDict.Add(Game.Instance.PLayerList[i].guid, 20); //sets how much a player will loose when he eats the red apple
                                            }
                                        }
                                    }
                                    
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.rabies:
                                    Game.Instance.rabiesDict.Add(player.guid, 5); //sets how many times a player can bite
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.movePowerUps:
                                    Game.Instance.movePowerUpsCounter += 150;
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.othersGoSlow:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (!Game.Instance.othersGoSlowList.Contains(Game.Instance.PLayerList[i].guid))
                                                    Game.Instance.othersGoSlowList.Add(Game.Instance.PLayerList[i].guid);
                                            }
                                        }
                                        Game.Instance.othersGoSlowCounter += 100;
                                       
                                    }
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.closingWalls:
                                    Game.Instance.closingWallsCounter += 40;
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.biggerWalls:
                                    Game.Instance.biggerWallsCounter += 40;
                                    Game.Instance.RemovePowerUp(power.guid);
                                    break;
                            }
                        }
                    }
                }




                // collision with head to wall
                if (headCoordinates[0] < 0 || headCoordinates[0] > _x || headCoordinates[1] < 0 || headCoordinates[1] > _y)
                {
                    // Collision!!
                    Console.WriteLine("Mit Kopf gegen Wand!");

                    if (Game.Instance.goThroughWallCounter > 0)
                    {
                        PowerUp.openTheWalls(headCoordinates);
                    }
                    else
                    {
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
                            Console.WriteLine("Player " + player.guid.ToString() + " collides with himself!");
                            if (!PowerUp.playerHasRabies(player.guid))
                            {

                                if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                    collisions.Add(player.guid, CollisionType.ToDead);
                            }
                            else
                            {
                                for (int x = 0; x < i; x++)
                                {
                                    deadPlayer.playerbody.RemoveAt(0);
                                }
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
                                Console.WriteLine("Player " + player.guid.ToString() + " collides with another player!");

                                if (!PowerUp.playerHasRabies(player.guid))
                                {
                                    if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                        collisions.Add(player.guid, CollisionType.ToPlayer);
                                }
                                else
                                {
                                    for (int i = 0; i < x; i++)
                                    {
                                            p.playerbody.RemoveAt(0);
                                    }



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
                                Console.WriteLine("Player " + player.guid.ToString() + " collides with himself!");
                                if (!PowerUp.playerHasRabies(player.guid))
                                {

                                    if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                        collisions.Add(player.guid, CollisionType.ToSelf);
                                }
                                else
                                {
                                    for (int x = 0; x < i; x++)
                                    {
                                        p.playerbody.RemoveAt(0);
                                    }
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
