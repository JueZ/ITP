using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{
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

        public void DetectCollision(List<Player> playerList)
        {
            
            foreach (Player player in playerList) // current player to check head
            {
                int[] headCoordinates = player.playerbody[0];
                //Console.WriteLine("x: " + coordinates[0].ToString() + "  y: " + coordinates[1].ToString());

                // collision with head to wall
                if (headCoordinates[0] < 0 || headCoordinates[0] >= _x || headCoordinates[1] < 0 || headCoordinates[1] >= _y)
                {
                    // Collision!!
                    Console.WriteLine("Mit Kopf gegen Wand!");
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
                                Console.WriteLine("Player " + player.guid.ToString() + " collides!");
                            }
                        }
                    }
                    else // check against player itself
                    {
                        int size = p.playerbody.Count;
                        int[] array;

                        for (int i = 1; i < size; i++) // skip head
                        {
                            array = p.playerbody[i];
                            if (headCoordinates[0] == array[0] && headCoordinates[1] == array[1]) // current head collides with own tail
                            {
                                // Collision!
                                Console.WriteLine("Player " + player.guid.ToString() + " collides!");
                            }
                        }
                    }
                    
                }
            }

        }

    }
}
