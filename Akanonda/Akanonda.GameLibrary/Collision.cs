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

        public Dictionary<Guid, CollisionType> DetectCollision()
        {
            game = Game.Instance;
            Dictionary<Guid, CollisionType> collisions = new Dictionary<Guid, CollisionType>();
            
            foreach (Player player in game.PLayerList) // current player to check head
            {
                List<int[]> coordinatesToCheckList = new List<int[]>();
                int[] headCoordinates = player.playerbody[player.playerbody.Count-1];
                coordinatesToCheckList.Add(headCoordinates);
                bool collisionHappened = false;
                List<Guid> deletePowerUpList = new List<Guid>();
                //check for PowerUp Collision
                foreach (PowerUp power in Game.Instance.PowerUpList)
                {
                    foreach (int[] powerUpLocation in power.PowerUpLocation)
                    {
                        int checkForBigPlayerAndGetModifierIndex = PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.makePlayersBigModifier, player.guid);
                        
                        if ( checkForBigPlayerAndGetModifierIndex > -1)
                        {
                            makePlayersBigModifier howBig = (makePlayersBigModifier)game.powerUpModificationList[player.guid][checkForBigPlayerAndGetModifierIndex];
                            for (int i = 1; i < howBig.getSize(); i++)
                            {
                                if (player.playersteering == PlayerSteering.Left || player.playersteering == PlayerSteering.Right)
                                {
                                    coordinatesToCheckList.Add(new int[] { headCoordinates[0], headCoordinates[1] + i });
                                }
                                else
                                {
                                    coordinatesToCheckList.Add(new int[] { headCoordinates[0] + i, headCoordinates[1] });
                                }
                            }
                        }

                        if (checkCollisionToPlayerOrPowerUp(coordinatesToCheckList, powerUpLocation)) // current head collides with PowerUp
                        {
                            switch(power.kind){
                                
                                case PowerUp.PowerUpKind.othersGoFast:
                                    if (game.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < game.PLayerList.Count; i++)
                                        {
                                            if (!game.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (!game.powerUpModificationList.ContainsKey(game.PLayerList[i].guid))
                                                    game.powerUpModificationList.Add(game.PLayerList[i].guid, new List<PowerUpModifier>());

                                                othersGoFastModifier oGFM = new othersGoFastModifier();
                                                game.powerUpModificationList[game.PLayerList[i].guid].Add(oGFM);
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
                                    if (game.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < game.PLayerList.Count; i++)
                                        {
                                            if (!game.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (!game.powerUpModificationList.ContainsKey(game.PLayerList[i].guid))
                                                    game.powerUpModificationList.Add(game.PLayerList[i].guid, new List<PowerUpModifier>());

                                                redAppleModifier rAM = new redAppleModifier();
                                                game.powerUpModificationList[game.PLayerList[i].guid].Add(rAM);
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
                                    if (game.PLayerList.Count > 1)
                                    {
                                        for (int i = 0; i < game.PLayerList.Count; i++)
                                        {
                                            if (!game.PLayerList[i].guid.Equals(player.guid))
                                            {
                                                if (!game.powerUpModificationList.ContainsKey(game.PLayerList[i].guid))
                                                    game.powerUpModificationList.Add(game.PLayerList[i].guid, new List<PowerUpModifier>());

                                                othersGoSlowModifier oGSM = new othersGoSlowModifier();
                                                game.powerUpModificationList[game.PLayerList[i].guid].Add(oGSM);
                                            }
                                        }
                                    }
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.makePlayersBig:

                                    for (int i = 0; i < game.PLayerList.Count; i++)
                                        {
                                            if (!game.powerUpModificationList.ContainsKey(game.PLayerList[i].guid))
                                            {
                                                game.powerUpModificationList.Add(game.PLayerList[i].guid, new List<PowerUpModifier>());
                                                makePlayersBigModifier mPBM = new makePlayersBigModifier();
                                                game.powerUpModificationList[game.PLayerList[i].guid].Add(mPBM);
                                            }
                                            else
                                            {
                                                int bigModifierIndex = PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.makePlayersBigModifier, game.PLayerList[i].guid);
                                                if (bigModifierIndex > -1)
                                                {
                                                    makePlayersBigModifier howBig = (makePlayersBigModifier)game.powerUpModificationList[game.PLayerList[i].guid][bigModifierIndex];
                                                    howBig.makeBiggerByOne();
                                                    howBig.setCount(1000);
                                                }
                                                else
                                                {
                                                    makePlayersBigModifier mPBM = new makePlayersBigModifier();
                                                    game.powerUpModificationList[game.PLayerList[i].guid].Add(mPBM);
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
                                case PowerUp.PowerUpKind.deleteAllSnakes:
                                    foreach (Player playerToDelete in game.PLayerList)
                                    {
                                        if (playerToDelete.guid != player.guid)
                                            playerToDelete.playerbody.RemoveRange(0, playerToDelete.playerbody.Count - 2);
                                    }
                                    deletePowerUpList.Add(power.guid);
                                    break;
                                case PowerUp.PowerUpKind.openWalls:
                                    game.powerUpCounters[Game.openWalls] += 100;
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


                foreach (int[] allHeadCoordinates in coordinatesToCheckList)
                {
                    if (allHeadCoordinates[0] < 0 || allHeadCoordinates[0] > _x -1 || allHeadCoordinates[1] < 0 || allHeadCoordinates[1] > _y -1)
                    {
                        collisionHappened = true;
                        break;
                    }
                }

                // collision with head to wall
                if (collisionHappened)
                {
                    List<PowerUpModifier> list = new List<PowerUpModifier>();
                    if (game.powerUpCounters[Game.openWalls] > 0 || PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.iGoThroughWallsModifier, player.guid) > -1)
                    {
                        PowerUp.openTheWalls(headCoordinates);
                    }
                    else
                    {
                        if (!collisions.ContainsKey(player.guid))
                            collisions.Add(player.guid, CollisionType.ToWall);
                    }
                    collisionHappened = false;
                }


                foreach (Player deadPlayer in Game.Instance.DeadList) // player with which current head is checked against (can be same as above)
                {
                    int size = deadPlayer.playerbody.Count;
                    int[] deadPlayerBody;

                    for (int i = 0; i < size - 1; i++) // skip head, otherwise loop would always find a collision
                    {
                        deadPlayerBody = deadPlayer.playerbody[i];

                        if (checkCollisionToPlayerOrPowerUp(coordinatesToCheckList, deadPlayerBody)) // current head collides with own tail
                        {
                            if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.rabiesModifier, player.guid) == -1)
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



                foreach (Player otherPlayer in game.PLayerList) // player with which current head is checked against
                {
                    if (player.guid != otherPlayer.guid) // check against other players
                    {
                        int x = 0;
                        foreach (int[] otherPlayerCoordinates in otherPlayer.playerbody)
                        {
                            x++;
                            if (checkCollisionToPlayerOrPowerUp(coordinatesToCheckList, otherPlayerCoordinates)) // current head collides with other player
                            {
                                // Collision!
                                if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.rabiesModifier, player.guid) == -1)
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
                        int[] ownPlayerBody;

                        for (int i = 0; i < size-1; i++) // skip head, otherwise loop would always find a collision
                        {
                            ownPlayerBody = otherPlayer.playerbody[i];
                            if (checkCollisionToPlayerOrPowerUp(coordinatesToCheckList, ownPlayerBody)) // current head collides with own tail
                            {
                             // Collision!

                             if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.rabiesModifier, player.guid) == -1)
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


        private bool checkCollisionToPlayerOrPowerUp(List<int[]> allHeadCoordinates, int[] coordinatesToCheck)
        {

            foreach (int[] headCoordinates in allHeadCoordinates)
            {
                if (headCoordinates[0] == coordinatesToCheck[0] && headCoordinates[1] == coordinatesToCheck[1])
                {
                    return true;
                }
            }
            return false;
        }


    }
}
