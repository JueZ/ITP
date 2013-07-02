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
        private Guid _guid;
        private PowerUpKind _kind;
        int startX, startY;
        
        public enum PowerUpKind
        {
            openWalls,
            goFast
        }

        public PowerUp(PowerUpKind kind, Guid guid = new Guid())
        {

            this._PowerUpLocation = new List<int[]>();
            _kind = kind;

            var guidIsEmpty = guid == Guid.Empty;
            if (guidIsEmpty)
                this._guid = Guid.NewGuid();
            else
                this._guid = guid;
           
            startX = Game.getRandomNumber(5, Game.Instance.getFieldx() - 5);
            startY = Game.getRandomNumber(5, Game.Instance.getFieldy() - 5);
            // frame of PowerUps
            this._PowerUpLocation.Add(new int[2] { startX, startY });
            this._PowerUpLocation.Add(new int[2] { startX, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX, startY + 4 });
            
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY + 4 });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY + 4 });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY + 4 });

            this._PowerUpLocation.Add(new int[2] { startX - 4, startY });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX - 4, startY + 4 });
            
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY });

            // inside of PowerUps
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY + 1 });
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY + 2 });
            this._PowerUpLocation.Add(new int[2] { startX - 1, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX - 2, startY + 3 });
            this._PowerUpLocation.Add(new int[2] { startX - 3, startY + 3 });

        }

        public List<int[]> PowerUpLocation
        {
            get { return _PowerUpLocation; }
        }

        public Guid guid
        {
            get { return _guid; }
        }

        public PowerUpKind kind
        {
            get { return _kind; }
        }
        
        
        
        
        
        
        
        
        public static void openTheWalls(int[] headCoordinates)
        {
            if (headCoordinates[0] < 0)
            {
                headCoordinates[0] = Game.Instance.getFieldx() - 1;
            }
            if (headCoordinates[0] >= Game.Instance.getFieldx())
            {
                headCoordinates[0] = 0;
            }
            if (headCoordinates[1] < 0)
            {
                headCoordinates[1] = Game.Instance.getFieldy() - 1;
            }
            if (headCoordinates[1] >= Game.Instance.getFieldy())
            {
                headCoordinates[1] = 0;
            }

        }
    }
}
