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
                                    game.powerUpCounters[Game.openWalls] += 100;
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.othersGoFast:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (!game.powerUpModificationList.ContainsKey(player.guid))
                                                    game.powerUpModificationList.Add(player.guid, new List<PowerUpModifier>());

                                                othersGoFastModifier oGFM = new othersGoFastModifier();
                                                game.powerUpModificationList[playerList[i].guid].Add(oGFM);
                                            }
                                        }
                                    }
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.iGoFast:
                                     if (!game.powerUpModificationList.ContainsKey(player.guid))
                                         game.powerUpModificationList.Add(player.guid, new List<PowerUpModifier>());

                                    iGoFastModifier iGFM = new iGoFastModifier();
                                    game.powerUpModificationList[player.guid].Add(iGFM);

                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.iGoSlow:
                                    if (!game.powerUpModificationList.ContainsKey(player.guid))
                                        game.powerUpModificationList.Add(player.guid, new List<PowerUpModifier>());

                                    iGoSlowModifier iGSM = new iGoSlowModifier();
                                    game.powerUpModificationList[player.guid].Add(iGSM);
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.goldenApple:
                                    if (!game.powerUpModificationList.ContainsKey(player.guid))
                                        game.powerUpModificationList.Add(player.guid, new List<PowerUpModifier>());

                                    goldenAppleModifier gAM = new goldenAppleModifier();
                                    game.powerUpModificationList[player.guid].Add(gAM);
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.redApple:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (!game.powerUpModificationList.ContainsKey(player.guid))
                                                    game.powerUpModificationList.Add(player.guid, new List<PowerUpModifier>());

                                                redAppleModifier rAM = new redAppleModifier();
                                                game.powerUpModificationList[playerList[i].guid].Add(rAM);
                                            }
                                        }
                                    }

                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.rabies:
                                    if (!game.powerUpModificationList.ContainsKey(player.guid))
                                        game.powerUpModificationList.Add(player.guid, new List<PowerUpModifier>());

                                    rabiesModifier rM = new rabiesModifier();
                                    iGoFastModifier iGFM2 = new iGoFastModifier();
                                    iGFM2.setCount(50);
                                    game.powerUpModificationList[player.guid].Add(rM);
                                    game.powerUpModificationList[player.guid].Add(iGFM2);

                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.othersGoSlow:
                                    if (Game.Instance.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < Game.Instance.PLayerList.Count; i++)
                                        {
                                            if (!Game.Instance.PLayerList[i].guid.Equals(game.PLayerList[i].guid))
                                            {
                                                if (!game.powerUpModificationList.ContainsKey(game.PLayerList[i].guid))
                                                    game.powerUpModificationList.Add(game.PLayerList[i].guid, new List<PowerUpModifier>());

                                                othersGoSlowModifier oGS = new othersGoSlowModifier();
                                                game.powerUpModificationList[game.PLayerList[i].guid].Add(oGS);
                                            }
                                        }
                                    }
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.iGoThroughWalls:
                                    if (!game.powerUpModificationList.ContainsKey(player.guid))
                                        game.powerUpModificationList.Add(player.guid, new List<PowerUpModifier>());

                                    iGoThroughWallsModifier iGTW = new iGoThroughWallsModifier();
                                    game.powerUpModificationList[player.guid].Add(iGTW);
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
                                    game.PowerUpPopUpRate -= 10;
                                    break;
                                case PowerUp.PowerUpKind.movePowerUps:
                                    game.powerUpCounters[Game.moveAllPowerUps] += 100;
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
                    List<PowerUpModifier> list = new List<PowerUpModifier>();
                    if (game.powerUpCounters[Game.openWalls] > 0 || PowerUp.checkIfPlayerHasModification(new iGoThroughWallsModifier().GetType(), player.guid) > -1)
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
                            if (PowerUp.checkIfPlayerHasModification(new rabiesModifier().GetType(), player.guid) == -1)
                            {
                                Console.WriteLine("Player " + player.guid.ToString() + " collides with himself!");
                                if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                    collisions.Add(player.guid, CollisionType.ToDead);
                            }
                            else
                            {
                                if (deadPlayer.playerbody.Count > i)
                                    deadPlayer.playerbody.RemoveRange(0, i);
                                break;
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
                                if (PowerUp.checkIfPlayerHasModification(new rabiesModifier().GetType(), player.guid) == -1)
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
                                    break;
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
                                if (PowerUp.checkIfPlayerHasModification(new rabiesModifier().GetType(), player.guid) == -1)
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
                                    break;
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
