using System;
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
            openWalls,
            othersGoFast,
            goldenApple, //gives the player 20 extra snake pieces
            redApple, // removes all other players 20 snake pieces
            movePowerUps,
            othersGoSlow,
            iGoSlow,
            iGoFast,
            rabies,
            closingWalls,
            biggerWalls,
            morePowerUps,
            iGoThroughWalls
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

            if (kind == PowerUpKind.goldenApple || kind == PowerUpKind.redApple || kind == PowerUpKind.rabies)
            {
                this._PowerUpLocation.Add(new int[2] { startX, startY });
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
            if (headCoordinates[0] > Game.Instance.getFieldX())
            {
                headCoordinates[0] = 0;
            }
            if (headCoordinates[1] < 0)
            {
                headCoordinates[1] = Game.Instance.getFieldY() - 1;
            }
            if (headCoordinates[1] > Game.Instance.getFieldY())
            {
                headCoordinates[1] = 0;
            }
        }

        public static bool playerAteGoldenApple(Guid guid)
        {
            if (Game.Instance.goldenAppleDict.ContainsKey(guid))
            {
                if (Game.Instance.goldenAppleDict[guid] - 1 > 0)
                    Game.Instance.goldenAppleDict[guid]--;
                else
                    Game.Instance.goldenAppleDict.Remove(guid);
                return true;
            }

            return false;
        }

        public static bool playerAteRedApple(Guid guid)
        {
            if (Game.Instance.iGoFastList.ContainsKey(guid))
            {
                if (Game.Instance.iGoFastList[guid] - 1 > 0)
                    Game.Instance.iGoFastList[guid]--;
                else
                    Game.Instance.iGoFastList.Remove(guid);
                return true;
            }

            return false;
        }


        public static void moveAllPowerUps(bool reset = false)
        {
            if (reset)
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
            else
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
                    foreach (int[] location in power.PowerUpLocation)
                    {
                        if (xPlusCounter > 0)
                            power.movePowerUpX = false;

                        if (xMinusCounter > 0)
                            power.movePowerUpX = true;

                        if (yPlusCounter > 0)
                            power.movePowerUpY = false;

                        if (yMinusCounter > 0)
                            power.movePowerUpY = true;

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
        }


        public static void removeAllTouchingPowerUps()
        {

            foreach (PowerUp power in Game.Instance.PowerUpList)
            {

                foreach (int[] location in power.PowerUpLocation)
                {
                    if (location[0] - 3 > Game.Instance.getFieldX())
                    {
                        Game.Instance.RemovePowerUp(power.guid);
                    }
                    if (location[0] + 3 < 0)
                    {
                        Game.Instance.RemovePowerUp(power.guid);
                    }
                    if (location[1] - 3> Game.Instance.getFieldY())
                    {
                        Game.Instance.RemovePowerUp(power.guid);
                    }
                    if (location[1] + 3 < 0)
                    {
                        Game.Instance.RemovePowerUp(power.guid);
                    }
                }
            }
        }


    }
}
