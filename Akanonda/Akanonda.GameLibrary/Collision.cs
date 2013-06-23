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
        ToSelf
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

        public Dictionary<Guid, CollisionType> DetectCollision(List<Player> playerList)
        {

            Dictionary<Guid, CollisionType> collisions = new Dictionary<Guid, CollisionType>();
            
            foreach (Player player in playerList) // current player to check head
            {
                int[] headCoordinates = player.playerbody[player.playerbody.Count-1];
                //Console.WriteLine("x: " + coordinates[0].ToString() + "  y: " + coordinates[1].ToString());

                // collision with head to wall
                if (headCoordinates[0] < 0 || headCoordinates[0] >= _x || headCoordinates[1] < 0 || headCoordinates[1] >= _y)
                {
                    // Collision!!
                    Console.WriteLine("Mit Kopf gegen Wand!");

                    if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                        collisions.Add(player.guid, CollisionType.ToWall);
                }

                foreach (Player p in playerList) // player with which current head is checked against (can be same as above)
                {
                    if (player.guid != p.guid) // check against other players
                    {
                        foreach (int[] array in p.playerbody)
                        {
                            if (headCoordinates[0] == array[0] && headCoordinates[1] == array[1]) // current head collides with other player
                            {
                                // Collision!
                                Console.WriteLine("Player " + player.guid.ToString() + " collides with another player!");
                                
                                if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                    collisions.Add(player.guid, CollisionType.ToPlayer);
                                
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
                                // Collision!
                                Console.WriteLine("Player " + player.guid.ToString() + " collides with himself!");

                                if (!collisions.ContainsKey(player.guid)) // player can only have 1 collision
                                    collisions.Add(player.guid, CollisionType.ToSelf);
                            }
                        }
                    }
                }
            }

            return collisions;
        }
    }
}
