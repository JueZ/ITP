using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{
    class PowerUps
    {

        public static void openTheWalls(int[] headCoordinates, int _x, int _y)
        {


            if (headCoordinates[0] < 0)
            {
                headCoordinates[0] = _x - 1;
            }
            if (headCoordinates[0] >= _x)
            {
                headCoordinates[0] = 0;
            }
            if (headCoordinates[1] < 0)
            {
                headCoordinates[1] = _y - 1;
            }
            if (headCoordinates[1] >= _y)
            {
                headCoordinates[1] = 0;
            }

        }
    }
}
