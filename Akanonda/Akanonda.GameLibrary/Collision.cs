﻿using System;
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
        ToDead,
        kicked
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
            int playerIndex = 0;
            List<int> removePlayerList = new List<int>();
            List<Player> addDuplicateList = new List<Player>();
            foreach (Player player in game.PLayerList) // current player to check head
            {
                List<int[]> coordinatesToCheckList = new List<int[]>();
                int[] headCoordinates = player.playerbody[player.playerbody.Count-1];
                coordinatesToCheckList.Add(headCoordinates);
                bool collisionHappened = false;
                List<Guid> deletePowerUpList = new List<Guid>();
                List<Player> duplicateCount = game.PLayerList.FindAll(x => x.guid == player.guid);
                int playerhasRabies = PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.rabiesModifier, player.guid);
                int playerHasChangeColor = PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.changeColorModifier, player.guid);



                foreach (KeyValuePair<int[], int[]> bigLocation in player.bigPlayerLocation)
                {
                    if (bigLocation.Value[0] == headCoordinates[0] && bigLocation.Value[1] == headCoordinates[1])
                    {

                        for (int i = 1; i < bigLocation.Key[0]; i++)
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
                }


                if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.iGoFastModifier, player.guid) > -1)
                {
                    if(player.playerbody[player.playerbody.Count - 2][0] != -40)
                    coordinatesToCheckList.Add(player.playerbody[player.playerbody.Count - 2]);
                }

                //check for PowerUp Collision
                foreach (PowerUp power in game.PowerUpList)
                {
                    foreach (int[] powerUpLocation in power.PowerUpLocation)
                    {
                        if (checkCollisionToPlayerOrPowerUp(coordinatesToCheckList, powerUpLocation)) // current head collides with PowerUp
                        {

                            if (!game.powerUpModificationList.ContainsKey(player.guid))
                                game.powerUpModificationList.Add(player.guid, new List<PowerUpModifier>());

                            PowerUpModifier newModification;
                            switch(power.kind){
                                case PowerUp.PowerUpKind.othersGoFast:
                                    foreach (Player otherPlayer in game.PLayerList)
                                    {
                                        if (checkIfOtherPlayer(otherPlayer.guid, player.guid))
                                        {
                                            newModification = new iGoFastModifier();
                                            game.powerUpModificationList[otherPlayer.guid].Add(newModification);
                                        }
                                    }
                                    break;
                                case PowerUp.PowerUpKind.iGoFast:
                                    newModification = new iGoFastModifier();
                                    game.powerUpModificationList[player.guid].Add(newModification);
                                    break;
                                case PowerUp.PowerUpKind.iGoSlow:
                                    newModification= new iGoSlowModifier();
                                    game.powerUpModificationList[player.guid].Add(newModification);
                                    break;
                                case PowerUp.PowerUpKind.goldenApple:
                                    newModification = new goldenAppleModifier();
                                    game.powerUpModificationList[player.guid].Add(newModification);
                                    break;
                                case PowerUp.PowerUpKind.redApple:
                                    foreach(Player otherPlayer in game.PLayerList)
                                    {
                                        if (checkIfOtherPlayer(otherPlayer.guid, player.guid))
                                        {
                                            newModification = new redAppleModifier();
                                            game.powerUpModificationList[otherPlayer.guid].Add(newModification);
                                        }
                                    }
                                    break;
                                case PowerUp.PowerUpKind.rabies:
                                    newModification = new rabiesModifier();                                    
                                    game.powerUpModificationList[player.guid].Add(newModification);
                                    newModification = new iGoFastModifier();
                                    newModification.setCount(50);
                                    game.powerUpModificationList[player.guid].Add(newModification);
                                    break;
                                case PowerUp.PowerUpKind.othersGoSlow:
                                    foreach(Player otherPlayer in game.PLayerList)
                                    {
                                        if (checkIfOtherPlayer(otherPlayer.guid, player.guid))
                                        {
                                            newModification = new iGoSlowModifier();
                                            game.powerUpModificationList[otherPlayer.guid].Add(newModification);
                                        }
                                    }
                                    break;
                                case PowerUp.PowerUpKind.makePlayersBig:
                                    foreach(Player otherPlayer in game.PLayerList)
                                    {
                                        if (checkIfOtherPlayer(otherPlayer.guid, player.guid))
                                        {
                                            newModification = new makePlayersBigModifier();
                                            game.powerUpModificationList[otherPlayer.guid].Add(newModification);
                                        }
                                    }
                                    break;
                                case PowerUp.PowerUpKind.iGoThroughWalls:
                                    newModification = new iGoThroughWallsModifier();
                                    game.powerUpModificationList[player.guid].Add(newModification);
                                    break;
                                case PowerUp.PowerUpKind.deleteAllSnakes:
                                    foreach (Player playerToDelete in game.PLayerList)
                                    {
                                        if (playerToDelete.guid != player.guid)
                                            playerToDelete.playerbody.RemoveRange(0, playerToDelete.playerbody.Count - 2);
                                    }
                                    break;
                                case PowerUp.PowerUpKind.openWalls:
                                    game.powerUpCounters[Game.openWalls] += 100;
                                    break;
                                case PowerUp.PowerUpKind.closingWalls:
                                    game.powerUpCounters[Game.closingWalls] += 40;
                                    break;
                                case PowerUp.PowerUpKind.biggerWalls:
                                    game.powerUpCounters[Game.biggerWalls] += 40;
                                    break;
                                case PowerUp.PowerUpKind.morePowerUps:
                                    deletePowerUpList.Add(power.guid);
                                    game.PowerUpPopUpRate -= 55;
                                    break;
                                case PowerUp.PowerUpKind.movePowerUps:
                                    game.powerUpCounters[Game.moveAllPowerUps] += 100;
                                    break;
                                case PowerUp.PowerUpKind.getMoreSnakes:
                                    addDuplicateList.Add(player);
                                    break;
                                case PowerUp.PowerUpKind.changeColor:
                                    newModification = new changeColorModifier();
                                    game.powerUpModificationList[player.guid].Add(newModification);
                                    newModification = new iGoFastModifier();
                                    newModification.setCount(50);
                                    game.powerUpModificationList[player.guid].Add(newModification);
                                    break;
                                case PowerUp.PowerUpKind.iGoDiagonal:
                                    newModification = new iGoDiagonalModifier();
                                    game.powerUpModificationList[player.guid].Add(newModification);
                                    break;
                                case PowerUp.PowerUpKind.othersGoDiagonal:
                                    foreach (Player otherPlayer in game.PLayerList)
                                    {
                                        if (checkIfOtherPlayer(otherPlayer.guid, player.guid))
                                        {
                                            newModification = new iGoDiagonalModifier();
                                            game.powerUpModificationList[otherPlayer.guid].Add(newModification);
                                        }
                                    }
                                    break;
                                case PowerUp.PowerUpKind.cheesySnakes:
                                    game.powerUpCounters[Game.cheesySnakes] += 100;
                                    break;
                            }
                            deletePowerUpList.Add(power.guid);
                        }
                    }
                }

                foreach (Guid guid in deletePowerUpList)
                {
                    Game.Instance.RemovePowerUp(guid);
                }


                foreach (int[] allHeadCoordinates in coordinatesToCheckList)
                {
                    if (allHeadCoordinates[0] < 0 || allHeadCoordinates[0] >= _x || allHeadCoordinates[1] < 0 || allHeadCoordinates[1] >= _y)
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
                        if (duplicateCount.Count == 1)
                        {
                            if (!collisions.ContainsKey(player.guid))
                                collisions.Add(player.guid, CollisionType.ToWall);
                        }
                        else
                        {
                            removePlayerList.Add(playerIndex);
                        }
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

                            if (playerhasRabies == -1 && duplicateCount.Count == 1 && playerHasChangeColor == -1)
                            {
                                //Console.WriteLine("Player " + player.guid.ToString() + " collides with himself!");
                                if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                    collisions.Add(player.guid, CollisionType.ToDead);
                            }
                            if (playerhasRabies > -1)
                            {
                                if (deadPlayer.playerbody.Count > i)
                                {
                                    deadPlayer.playerbody.RemoveRange(0, i);
                                    deadPlayer.score -= i; 
                                }
                                removeModification(player.guid, PowerUpModifierKind.rabiesModifier);
                                removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
                                break;

                            }
                            else if (duplicateCount.Count > 1)
                            {
                                removePlayerList.Add(playerIndex);
                            }
                            if (playerHasChangeColor > -1)
                            {
                                System.Drawing.Color ColorBuffer = deadPlayer.color;
                                deadPlayer.color = player.color;
                                player.color = ColorBuffer;
                                removeModification(player.guid, PowerUpModifierKind.changeColorModifier);
                                removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
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
                            List<int[]> checkOtherPlayerCoordinates = new List<int[]>();
                            
                            foreach (KeyValuePair<int[], int[]> bigLocation in otherPlayer.bigPlayerLocation)
                            {
                                if (bigLocation.Value[0] == headCoordinates[0] && bigLocation.Value[1] == headCoordinates[1])
                                {

                                    for (int i = 1; i < bigLocation.Key[0]; i++)
                                    {
                                        if (bigLocation.Key[1] == 4 || bigLocation.Key[1] == 2)
                                        {
                                            if (checkCollisionToPlayerOrPowerUp(coordinatesToCheckList, new int[] { headCoordinates[0], headCoordinates[1] + i}))
                                            {

                                                if (playerhasRabies == -1 && duplicateCount.Count == 1 && playerHasChangeColor == -1)
                                                {
                                                    //Console.WriteLine("Player " + player.guid.ToString() + " collides with another player!");
                                                    if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                                        collisions.Add(player.guid, CollisionType.ToPlayer);
                                                }
                                                if (playerhasRabies > -1)
                                                {
                                                    if (otherPlayer.playerbody.Count > x)
                                                    {
                                                        otherPlayer.playerbody.RemoveRange(0, x);
                                                        otherPlayer.score -= x;
                                                    }
                                                    removeModification(player.guid, PowerUpModifierKind.rabiesModifier);
                                                    removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
                                                    break;
                                                }
                                                else if (duplicateCount.Count > 1)
                                                {
                                                    removePlayerList.Add(playerIndex);
                                                }
                                                if (playerHasChangeColor > -1)
                                                {
                                                    System.Drawing.Color ColorBuffer = otherPlayer.color;
                                                    otherPlayer.color = player.color;
                                                    player.color = ColorBuffer;
                                                    removeModification(player.guid, PowerUpModifierKind.changeColorModifier);
                                                    removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (checkCollisionToPlayerOrPowerUp(coordinatesToCheckList, new int[] { headCoordinates[0] + i, headCoordinates[1] }))
                                            {

                                                if (playerhasRabies == -1 && duplicateCount.Count == 1 && playerHasChangeColor == -1)
                                                {
                                                    //Console.WriteLine("Player " + player.guid.ToString() + " collides with another player!");
                                                    if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                                        collisions.Add(player.guid, CollisionType.ToPlayer);
                                                }
                                                if (playerhasRabies > -1)
                                                {
                                                    if (otherPlayer.playerbody.Count > x)
                                                    {
                                                        otherPlayer.playerbody.RemoveRange(0, x);
                                                        otherPlayer.score -= x;
                                                    }
                                                    removeModification(player.guid, PowerUpModifierKind.rabiesModifier);
                                                    removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
                                                    break;
                                                }
                                                else if (duplicateCount.Count > 1)
                                                {
                                                    removePlayerList.Add(playerIndex);
                                                }
                                                if (playerHasChangeColor > -1)
                                                {
                                                    System.Drawing.Color ColorBuffer = otherPlayer.color;
                                                    otherPlayer.color = player.color;
                                                    player.color = ColorBuffer;
                                                    removeModification(player.guid, PowerUpModifierKind.changeColorModifier);
                                                    removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            x++;
                            if (checkCollisionToPlayerOrPowerUp(coordinatesToCheckList, otherPlayerCoordinates)) // current head collides with other player
                            {
                            
                                // Collision!
                                if (playerhasRabies == -1 && duplicateCount.Count == 1 && playerHasChangeColor == -1)
                                {
                                    //Console.WriteLine("Player " + player.guid.ToString() + " collides with another player!");
                                    if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                        collisions.Add(player.guid, CollisionType.ToPlayer);
                                }
                                if (playerhasRabies > -1)
                                {
                                    if (otherPlayer.playerbody.Count > x)
                                    {
                                        otherPlayer.playerbody.RemoveRange(0, x);
                                        otherPlayer.score -= x;
                                    }
                                    removeModification(player.guid, PowerUpModifierKind.rabiesModifier);
                                    removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
                                    break;
                                }
                                else if (duplicateCount.Count > 1)
                                {
                                    removePlayerList.Add(playerIndex);
                                }
                                if (playerHasChangeColor > -1)
                                {
                                    System.Drawing.Color ColorBuffer = otherPlayer.color;
                                    otherPlayer.color = player.color;
                                    player.color = ColorBuffer;
                                    removeModification(player.guid, PowerUpModifierKind.changeColorModifier);
                                    removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
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
                            List<int[]> onlyHead = new List<int[]>();
                            onlyHead.Add(headCoordinates);
                            if (checkCollisionToPlayerOrPowerUp(onlyHead, ownPlayerBody)) // current head collides with own tail
                            {                                   //coordinatesToCheckList makes player always crash when big
                             // Collision!
                             if (playerhasRabies == -1 && duplicateCount.Count == 1 && playerHasChangeColor == -1)
                             {
                                 //Console.WriteLine("Player " + player.guid.ToString() + " collides with himself!");
                                 if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                     collisions.Add(player.guid, CollisionType.ToSelf);
                             }
                             if (playerhasRabies > -1)
                             {
                                 if (player.playerbody.Count > i)
                                     player.playerbody.RemoveRange(0, i);
                                 player.score -= i;
                                 removeModification(player.guid, PowerUpModifierKind.rabiesModifier);
                                 removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
                                 break;
                             }
                             else if (duplicateCount.Count > 1)
                             {
                                 removePlayerList.Add(playerIndex);
                             }
                             if (playerHasChangeColor > -1)
                             {
                                 player.color = System.Drawing.Color.Lavender;
                                 removeModification(player.guid, PowerUpModifierKind.changeColorModifier);
                                 removeModification(player.guid, PowerUpModifierKind.iGoFastModifier);
                             }
                            }
                        }
                    }
                }
                playerIndex++;
            }

            foreach (int removePlayerIndex in removePlayerList)
            {
                game.PLayerList[removePlayerIndex].guid = Guid.NewGuid();
                game.DeadList.Add(game.PLayerList[removePlayerIndex]);
                game.PLayerList.RemoveAt(removePlayerIndex);
            }

            foreach (Player player in addDuplicateList)
            {
                int Index = game.PLayerList.FindIndex(x => x.guid == player.guid && x.name == player.name);

                game.addDuplicatePlayer(player.name, player.color, player.guid, Index);
                game.addDuplicatePlayer(player.name, player.color, player.guid, Index);
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

        private void removeModification(Guid playerGuid, PowerUpModifierKind ModificationKind)
        {
            List<PowerUpModifier> pUMs = new List<PowerUpModifier>();
            game.powerUpModificationList.TryGetValue(playerGuid, out pUMs);

            List<PowerUpModifier> deleteModification = new List<PowerUpModifier>();
            foreach (PowerUpModifier pUM in pUMs)
            {
                if (pUM.getType() == ModificationKind)
                    deleteModification.Add(pUM);
            }
            foreach (PowerUpModifier pM in deleteModification)
            {
                game.powerUpModificationList[playerGuid].Remove(pM);
            }
            deleteModification.Clear();
        }

        private bool checkIfOtherPlayer(Guid otherPlayerGuid, Guid playerGuid)
        {
            if (otherPlayerGuid != playerGuid)
            {
                if (!game.powerUpModificationList.ContainsKey(otherPlayerGuid))
                    game.powerUpModificationList.Add(otherPlayerGuid, new List<PowerUpModifier>());

                return true;
            }
            return false;
        }
    }
}
