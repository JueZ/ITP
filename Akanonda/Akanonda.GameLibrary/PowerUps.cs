﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{
    [Serializable()]
    public class PowerUp
    {
        private List<int[]> _PowerUpLocation;

        public PowerUp(){

            this._PowerUpLocation = new List<int[]>();
            Random rndX = new Random();
            Random rndY = new Random();
            int startX, startY;
            startX = rndX.Next(20, 100);
            startY = rndY.Next(20, 100);

            this._PowerUpLocation.Add(new int[2] { startX, startY });
            this._PowerUpLocation.Add(new int[2] { startX, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX, startY + 4 });
            this._PowerUpLocation.Add(new int[2] { startX, startY + 5 });
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY });
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY + 4 });
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY + 5 });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY + 4 });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY + 5 });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY + 4 });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY + 5 });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY + 4 });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY + 5 });
            this._PowerUpLocation.Add(new int[2] { startX - 5, startY });
            this._PowerUpLocation.Add(new int[2] { startX - 5, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX - 5, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX - 5, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX - 5, startY + 4 });
            this._PowerUpLocation.Add(new int[2] { startX - 5, startY + 5 });
        }

        public List<int[]> PowerUpLocation
        {
            get { return _PowerUpLocation; }
        }
        
        
        
        
        
        
        
        
        
        
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
