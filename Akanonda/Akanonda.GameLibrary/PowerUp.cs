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
        private bool _movePowerUpX, _movePowerUpY;
       
        public enum PowerUpKind
        {
            openWalls = 1,
            othersGoFast = 2,
            movePowerUps = 3,
            othersGoSlow = 4,
            iGoSlow = 5,
            iGoFast = 6,
            closingWalls = 7,
            biggerWalls = 8,
            changeColor = 9,
            iGoThroughWalls = 10,
            getMoreSnakes = 11,
            iGoDiagonal = 12,
            othersGoDiagonal = 13,
            morePowerUps = 14,
            cheesySnakes = 15,
            makePlayersBig = 16,
            redApple = 17, // shrink icon!! removes all other players 20 snake pieces
            goldenApple = 21, //gives the player 20 extra snake pieces
            rabies = 22,
            deleteAllSnakes = 60//-> redAppleIcon
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

            

            if(Game.getRandomNumber(1,3) % 2 == 0)
                _movePowerUpX = true;
            else
                _movePowerUpX = false;

            if (Game.getRandomNumber(1, 3) % 2 == 0)
                _movePowerUpY = true;
            else
                _movePowerUpY = false;

            startX = Game.getRandomNumber(5, Game.Instance.getFieldX() - 5);
            startY = Game.getRandomNumber(5, Game.Instance.getFieldY() - 5);

            if (kind == PowerUpKind.goldenApple || kind == PowerUpKind.deleteAllSnakes || kind == PowerUpKind.rabies)
            {
                this._PowerUpLocation.Add(new int[2] { startX, startY });
                this._PowerUpLocation.Add(new int[2] { startX, startY + 1 });
                this._PowerUpLocation.Add(new int[2] { startX + 1, startY +1 });
                this._PowerUpLocation.Add(new int[2] { startX + 1, startY });
            }
            else
            {

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
        public bool movePowerUpX
        {
            get { return _movePowerUpX; }
            set { _movePowerUpX = value; }
        }
        public bool movePowerUpY
        {
            get { return _movePowerUpY; }
            set { _movePowerUpY = value; }
        }


        
        
        
        public static void openTheWalls(int[] headCoordinates)
        {
            if (headCoordinates[0] < 0)
            {
                headCoordinates[0] = Game.Instance.getFieldX() - 1;
            }
            if (headCoordinates[0] > Game.Instance.getFieldX() - 1)
            {
                headCoordinates[0] = 0;
            }
            if (headCoordinates[1] < 0)
            {
                headCoordinates[1] = Game.Instance.getFieldY() - 1;
            }
            if (headCoordinates[1] > Game.Instance.getFieldY() - 1)
            {
                headCoordinates[1] = 0;
            }
        }

        public static void resetPowerUpMovingDirection()
        {
             foreach (PowerUp power in Game.Instance.PowerUpList)
             {
                 if (Game.getRandomNumber(0, 10) % 2 == 0)
                     power._movePowerUpX = false;
                 else
                     power._movePowerUpX = true;

                 if (Game.getRandomNumber(0, 10) % 2 == 0)
                     power._movePowerUpY = false;
                 else
                     power._movePowerUpY = true;

             }
        }

        public static void movePowerUpsButNotThroughWall()
        {
            
            foreach (PowerUp power in Game.Instance.PowerUpList)
            {

                int xPlusCounter = 0;
                int xMinusCounter = 0;
                int yPlusCounter = 0;
                int yMinusCounter = 0;

                foreach (int[] location in power.PowerUpLocation)
                {
                    if (location[0] + 1 > Game.Instance.getFieldX() - 1)
                    {
                        xPlusCounter++;
                    }
                    if (location[0] - 1 < 0)
                    {
                        xMinusCounter++;
                    }
                    if (location[1] + 1 > Game.Instance.getFieldY() - 1)
                    {
                        yPlusCounter++;
                    }
                    if (location[1] - 1 < 0)
                    {
                        yMinusCounter++;
                    }
                }

                if (xPlusCounter > 0)
                    power.movePowerUpX = false;
                if (xMinusCounter > 0)
                    power.movePowerUpX = true;

                if (yPlusCounter > 0)
                    power.movePowerUpY = false;
                if (yMinusCounter > 0)
                    power.movePowerUpY = true;

                foreach (int[] location in power.PowerUpLocation)
                {
                    if (power.movePowerUpX)
                        location[0]++;
                    else
                        location[0]--;

                    if (power.movePowerUpY)
                        location[1]++;
                    else
                        location[1]--;
                }
            }
        }

        public static void removeAllPowerUpsOutsideField()
        {
            List<Guid> deletePowerUpList = new List<Guid>();
            foreach (PowerUp power in Game.Instance.PowerUpList)
            {

                foreach (int[] location in power.PowerUpLocation)
                {
                    if (location[0] - 3 > Game.Instance.getFieldX())
                    {
                        deletePowerUpList.Add(power.guid);
                    }
                    if (location[0] + 3 < 0)
                    {
                        deletePowerUpList.Add(power.guid);
                    }
                    if (location[1] - 3> Game.Instance.getFieldY())
                    {
                        deletePowerUpList.Add(power.guid);
                    }
                    if (location[1] + 3 < 0)
                    {
                        deletePowerUpList.Add(power.guid);
                    }
                }
            }

            foreach (Guid guid in deletePowerUpList)
            {
                Game.Instance.RemovePowerUp(guid);
            }
        }

        public static int checkIfPlayerHasModification(PowerUpModifierKind a, Guid playerGuid)
        {
            List<PowerUpModifier> list = new List<PowerUpModifier>();
            if (!Game.Instance.powerUpModificationList.TryGetValue(playerGuid, out list))
                return -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetType().Name.Equals(a.ToString()))
                    return i;
            }
            return -1;
        }

        public static int countBigModifiers(Guid playerGuid)
        {
            List<PowerUpModifier> list = new List<PowerUpModifier>();
            int x = 1;
            if (!Game.Instance.powerUpModificationList.TryGetValue(playerGuid, out list))
                return -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetType().Name.Equals(PowerUpModifierKind.makePlayersBigModifier.ToString()))
                    x++;
            }
            return x;
        }

        public static bool checkThisShit(PowerUpModifierKind a, Guid guid)
        {

            if (Game.Instance.powerUpModificationList.ContainsKey(guid))
            {
                List<PowerUpModifier> list = Game.Instance.powerUpModificationList[guid];
                foreach (PowerUpModifier mod in list)
                {
                    if (mod.GetType().Name == PowerUpModifierKind.makePlayersBigModifier.ToString())
                        return true;
                }

                //    playerIsBig = true;
            }
            return false;


        }



    }
}
