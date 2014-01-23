using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Akanonda.GameLibrary
{
    [Serializable()]
    public sealed class Game
    {
        private static Game instance = new Game();
        private Field _field;
        private List<Player> _playerList;
        private List<PowerUp> _powerupList;
        private List<Player> _lobbyList;
        private List<Player> _deadList;
        private Guid _localplayer;
        private Collision _collision;
        private int _ticksUntilAdd;
        private int _tickCounter;
        private Dictionary<Guid, CollisionType> _collisionList;
        private Dictionary<Guid, List<PowerUpModifier>> _powerUpModificationList;
        private int[] _powerUpCounters = new int[5];
        public const int moveAllPowerUps = 0;
        public const int openWalls = 1;
        public const int closingWalls = 2;
        public const int biggerWalls = 3;
        public const int cheesySnakes = 4;
        private int _powerUpPopUpRate = 65;

        public static Game Instance
        {
            get
            {
                return instance;
            }
        }

        private Game()
        {
            _playerList = new List<Player>();
            _deadList = new List<Player>();
            _lobbyList = new List<Player>();
            _powerupList = new List<PowerUp>();
            _powerUpModificationList = new Dictionary<Guid, List<PowerUpModifier>>();

            _field.setSize(120, 120); // testhalber, derzeit wird neuer user auf 105x105 oder so gesetzt
            _field.Scale = 5;
            _field.Offset = new int[] {20, 20, 20, 20}; // testhalber

            _collision = new Collision(_field.x, _field.y);
            _ticksUntilAdd = 10; // set how fast player grows
            _tickCounter = 0;

            _powerUpCounters[moveAllPowerUps] = 0;
            _powerUpCounters[openWalls] = 0;
            _powerUpCounters[closingWalls] = 0;
            _powerUpCounters[biggerWalls] = 0;
            _powerUpCounters[cheesySnakes] = 0;

        }

        public List<Player> PLayerList { get { return _playerList; } }
        public List<Player> LobbyList { get { return _lobbyList; } }
        public List<Player> DeadList { get { return _deadList; } }
        public List<PowerUp> PowerUpList { get { return _powerupList; } }
        public int tickCounter { get { return _tickCounter; } }
        

        public void setFieldSize(int x, int y)
        {
            _field.setSize(x, y);
        }

        //PowerUp get set --------------------
        public Dictionary<Guid, List<PowerUpModifier>> powerUpModificationList { get { return _powerUpModificationList; } set { _powerUpModificationList = value; } }
        public int[] powerUpCounters { get { return _powerUpCounters; } set { _powerUpCounters = value; } }
        public int PowerUpPopUpRate
        {
            get { return _powerUpPopUpRate; }
            set {
                if (value >= 40 && value <= 100)
                    _powerUpPopUpRate = value;
            }
        }
        
        public Guid LocalPlayerGuid
        {
            set { _localplayer = value; }
            get { return _localplayer; }
        }

        public PlayerSteering LocalSteering
        {
            set
            {
                Player playerToFind =_playerList.Find(item => item.guid == _localplayer);
                if(playerToFind != null)
                    playerToFind.playersteering = value;
            }
            get
            {
                Player playerToFind = _playerList.Find(item => item.guid == _localplayer);
                if(playerToFind != null)
                    return playerToFind.playersteering;
                else
                throw new Exception("No localplayer found");
            }
        }

        public void setsteering(Guid playerguid, PlayerSteering playersteering, Boolean use4Buttons)
        {

            List<Player> playerToFind = Game.Instance.PLayerList.FindAll(item => item.guid == playerguid);
            foreach (Player player in playerToFind)
            {
                if (use4Buttons)
                {
                    switch (playersteering)
                    {
                        case PlayerSteering.Right:
                            if (player.playersteering != PlayerSteering.Left)
                            {
                                player.playersteering = PlayerSteering.Right;
                            }

                            break;
                        case PlayerSteering.Left:
                            if (player.playersteering != PlayerSteering.Right)
                            {
                                player.playersteering = PlayerSteering.Left;
                            }

                            break;
                        case PlayerSteering.Up:
                            if (player.playersteering != PlayerSteering.Down)
                            {
                                player.playersteering = PlayerSteering.Up;
                            }

                            break;
                        case PlayerSteering.Down:
                            if (player.playersteering != PlayerSteering.Up)
                            {
                                player.playersteering = PlayerSteering.Down;
                            }
                            break;
                    }
                }
                else
                {
                    int test = (int)player.playersteering;
                    if (playersteering == PlayerSteering.Right)
                    {
                        if (player.playersteering == PlayerSteering.Left)
                            test = 1;
                        else
                            test++;
                        player.playersteering = (PlayerSteering)test;
                    }
                    if (playersteering == PlayerSteering.Left)
                    {
                        if (player.playersteering == PlayerSteering.Up)
                            test = 4;
                        else
                            test--;
                        player.playersteering = (PlayerSteering)test;
                    }
                }
            }
        }


        public void addPlayer(string name, Color color, Guid guid)
        {
            int setScore = 0;
            Player playerToFind = _lobbyList.Find(item => item.guid == guid);
            if(playerToFind != null)
                setScore = playerToFind.score;
            _playerList.Add(new Player(name, color, guid, setScore));
        }

        public void addDuplicatePlayer(string name, Color color, Guid guid, int duplicateIndex)
        {
            _playerList.Add(new Player(name, color, guid, 0, duplicateIndex));
        }

        public void addDeadRemoveLivingPlayer(Guid guid)
        {
            Player playerToRemove = _playerList.Find(item => item.guid == guid);
            if (playerToRemove != null)
            {
                _playerList.Remove(playerToRemove);
                playerToRemove.guid = Guid.NewGuid();
                _deadList.Add(playerToRemove);
            }
        }

        public void clearModificationListOnDead(Guid guid){
                _powerUpModificationList.Remove(guid);
        }

        public string getPlayerName(Guid guid)
        {
            string name = "";
            Player playerToFind = _playerList.Find(item => item.guid == guid);
            if (playerToFind != null)
                name = playerToFind.name;
            return name;
        }

        public Color getPlayerColor(Guid guid)
        {
            Color color = Color.FromName("Black");
            Player playerToFind = _playerList.Find(item => item.guid == guid);
            if(playerToFind != null)
                color = playerToFind.color;
            return color;
        }

        public void setScoreToLobbyPlayer(Guid guid)
        {
            int setScore = 0;
            Player playerToFind = _playerList.Find(item => item.guid == guid);
            if(playerToFind != null)
                setScore = playerToFind.score;
            playerToFind = _lobbyList.Find(item => item.guid == guid);
            if(playerToFind != null)
                playerToFind.score = setScore;
        }

        public void updateScore(Guid guid)
        {
            Player playerToBeUpdated = _playerList.Find(item => item.guid == guid);
            int addScore = playerToBeUpdated.playerbody.Count / 2;
            addScore += playerToBeUpdated.SurvivalTime % 60;
            playerToBeUpdated.score = addScore;
        }

        public void removePlayer(Guid guid)
        {
            Player playerToFind = _playerList.Find(item => item.guid == guid);
            if(playerToFind != null)
                _playerList.Remove(playerToFind);
        }

        public void removeDeadPlayer(Guid guid)
        {
            Player deadToFind = _deadList.Find(item => item.guid == guid);
            if(deadToFind != null)
                _deadList.Remove(deadToFind);
        }

        public void gametick()
        {
            _tickCounter = (_tickCounter + 1) % _ticksUntilAdd;
            handlePowerUpTicks();
            movePlayersandAddScore();
            checkIfPowerUpsShouldBeAddedRelativeToFieldsize();
            detectCollision();
        }

        private void handlePowerUpTicks()
        {
            checkOpenWallCounter();
            checkMovePowerUpsCounter();
            checkIfWallsShouldGetSmallerOrBigger();
            checkCheesySnakeCounter();
            remove1ModificationCountEveryTick();
            setPopUpRateToNormal();

        }

        private void checkOpenWallCounter()
        {
            if (powerUpCounters[openWalls] > 0)
                powerUpCounters[openWalls]--;
        }

        private void checkMovePowerUpsCounter()
        {
            if (powerUpCounters[moveAllPowerUps] > 0)
            {
                if (_tickCounter % 2 == 0) //Powerup moving speed
                    PowerUp.movePowerUpsButNotThroughWall();

                powerUpCounters[moveAllPowerUps]--;
                if (powerUpCounters[moveAllPowerUps] <= 0)
                    PowerUp.resetPowerUpMovingDirection();
            }
        }

        private void checkIfWallsShouldGetSmallerOrBigger()
        {
            if (powerUpCounters[closingWalls] > 0 && powerUpCounters[biggerWalls] > 0)
            {
                powerUpCounters[closingWalls]--;
                powerUpCounters[biggerWalls]--;
            }
            else
            {
                checkClosingWallsCounter();
                checkBiggerWallsCounter();
            }
        }

        private void checkCheesySnakeCounter()
        {
            if (powerUpCounters[cheesySnakes] > 0)
                powerUpCounters[cheesySnakes]--;
        }

        private void setPopUpRateToNormal()
        {
            if (getRandomNumber(0,100) % 99 == 0)
            PowerUpPopUpRate++;
        }

        private void checkClosingWallsCounter()
        {
            if (powerUpCounters[closingWalls] > 0)
            {
                powerUpCounters[closingWalls]--;
                if (tickCounter % 3 == 0) //closingWall speed
                {
                    if (Game.Instance.getFieldX() > 60 && Game.Instance.getFieldY() > 60)
                    {
                        setFieldSize(Game.Instance.getFieldX() - 1, Game.Instance.getFieldY() - 1);
                        _collision.setCollisionFieldSize(Game.Instance.getFieldX(), Game.Instance.getFieldY());
                        PowerUp.removeAllPowerUpsOutsideField();
                    }
                }
            }
        }

        private void checkBiggerWallsCounter()
        {
            if (powerUpCounters[biggerWalls] > 0)
            {
                powerUpCounters[biggerWalls]--;
                if (tickCounter % 3 == 0) //biggerWall speed
                {
                    if (Game.Instance.getFieldX() > 60 && Game.Instance.getFieldY() > 60)
                    {
                        setFieldSize(Game.Instance.getFieldX() + 1, Game.Instance.getFieldY() + 1);
                        _collision.setCollisionFieldSize(Game.Instance.getFieldX() + 1, Game.Instance.getFieldY() + 1);
                    }
                }
            }
        }

        private void movePlayersandAddScore()
        {
            bool grow = false;
            if (_tickCounter == 0)
                grow = true;

            for (int i = 0; i < _playerList.Count; i++)
            {
                if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.iGoSlowModifier, _playerList[i].guid) > -1)
                {
                    if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.iGoFastModifier, _playerList[i].guid) > -1)
                    {
                        _playerList[i].playerMove(grow);
                    }
                    else
                    {
                        if (tickCounter % 2 == 0)
                            _playerList[i].playerMove(grow);
                    }
                }
                else
                {
                    _playerList[i].playerMove(grow);
                }

                addScoreEvery30Seconds(i);
            }
        }

        private void remove1ModificationCountEveryTick()
        {
            List<PowerUpModifier> deleteModification = new List<PowerUpModifier>();
            foreach (KeyValuePair<Guid, List<PowerUpModifier>> pair in powerUpModificationList)
            {
                foreach (PowerUpModifier pUM in pair.Value)
                {
                    pUM.reduceCounterBy1();
                    if (pUM.getCount() == 0)
                        deleteModification.Add(pUM);
                    }
                foreach (PowerUpModifier pM in deleteModification)
                {
                    pair.Value.Remove(pM);
                }
                deleteModification.Clear();
            }
        }

        private void addScoreEvery30Seconds(int i)
        {
            if (_playerList[i].SurvivalTime % 30 == 0)
            {
                _playerList[i].score += 10;
                _playerList[i].SurvivalTime = 1;
            }
        }

        private void checkIfPowerUpsShouldBeAddedRelativeToFieldsize()
        {
            if (PowerUpList.Count < getFieldX() / 8 && PLayerList.Count > 0)
            {
                makePowerUpsPopUp();
            }
        }

        private void makePowerUpsPopUp()
        {
            if (getRandomNumber(0, 900) % PowerUpPopUpRate == 0)
            {
                PowerUp.PowerUpKind RandomPowerUp = GetRandomPowerUp<PowerUp.PowerUpKind>();
                AddPowerUp(RandomPowerUp);
            }
        }

        static T GetRandomPowerUp<T>()
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            T V = (T)A.GetValue(getRandomNumber(0, A.Length));
            return V;
        }

        public void detectCollision()
        {
            _collisionList = _collision.DetectCollision();

        }

        public void gamepaint(Graphics g)
        {
            paintPowerUps(g);
            paintPlayers(g, PLayerList);
            paintPlayers(g, DeadList);
            paintGameBorderBlackOrGrayDependingOnOpenWalls(g);
        }

        private void paintPowerUps(Graphics g)
        {
            foreach (PowerUp power in _powerupList)
            {
                choosePowerUpIcon(power, g);
            }
        }

        private void choosePowerUpIcon(PowerUp power, Graphics g)
        {
            int lowestX = 9999;
            int lowestY = 9999;
            Icon powerupIcon = null;
            
            foreach (int[] powerUpLocation in power.PowerUpLocation)
            {

                lowestX = lowestX < (_field.offsetWest + powerUpLocation[0] * _field.Scale) ? lowestX : (_field.offsetWest + powerUpLocation[0] * _field.Scale);
                lowestY = lowestY < (_field.offsetNorth + powerUpLocation[1] * _field.Scale) ? lowestY : (_field.offsetNorth + powerUpLocation[1] * _field.Scale);
            }
            Rectangle rect;
            if (power.kind != PowerUp.PowerUpKind.rabies && power.kind != PowerUp.PowerUpKind.goldenApple && power.kind != PowerUp.PowerUpKind.deleteAllSnakes)
                rect = new Rectangle(lowestX, lowestY, 26, 26);
            else
            {
                rect = new Rectangle(lowestX - 2, lowestY - 2, 15, 15);
            }

            powerupIcon = new Icon("images/" + power.kind + ".ico");
            g.DrawIcon(powerupIcon, rect);
        }


        private void paintPlayers(Graphics g, List<Player> PlayersToPaint)
        {
            foreach (Player player in PlayersToPaint)
            {
                for(int i = 0; i < player.playerbody.Count; i++)
                {
                    int x = i+1;
                    if (x == player.playerbody.Count)
                        x--;
                    bool snakepartIsBig = false;
                    int howBig = 1;
                    foreach (KeyValuePair<int[], int[]> checkLocation in player.bigPlayerLocation)
                    {
                        if (player.playerbody[i][0] == checkLocation.Value[0] && player.playerbody[i][1] == checkLocation.Value[1])
                        {
                            snakepartIsBig = true;
                            howBig = checkLocation.Key[0];
                            break;
                        }
                    }
                    if (player.playerbody[i][0] > -1 && player.playerbody[i][0] < getFieldX() && player.playerbody[i][1] > -1 && player.playerbody[i][1] < getFieldY())
                        if (snakepartIsBig)
                        {
                        g.FillRectangle(new SolidBrush(player.color), (_field.offsetWest + player.playerbody[i][0] * _field.Scale),
                        (_field.offsetNorth + player.playerbody[i][1] * _field.Scale),
                        player.playerbody[x][0] == player.playerbody[i][0] ? _field.Scale * howBig : _field.Scale,
                        player.playerbody[x][1] == player.playerbody[i][1] ? _field.Scale * howBig : _field.Scale);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(player.color), (_field.offsetWest + player.playerbody[i][0] * _field.Scale),
                            (_field.offsetNorth + player.playerbody[i][1] * _field.Scale),
                            _field.Scale,_field.Scale);
                        }
                }
            }
        }

        private void paintGameBorderBlackOrGrayDependingOnOpenWalls(Graphics g)
        {
            Rectangle[] border = 
            { 
                new Rectangle(0, 0, _field.Width, _field.offsetNorth), // north
                new Rectangle((_field.offsetWest + (_field.x * _field.Scale)), 0, _field.offsetEast, _field.Height), // east
                new Rectangle(0, (_field.offsetNorth + (_field.y * _field.Scale)), _field.Width, _field.offsetSouth), // south
                new Rectangle(0, 0, _field.offsetWest, _field.Height) // west
            };

            SolidBrush brush;

            if (powerUpCounters[openWalls] > 0)
                brush = new SolidBrush(Color.Gray);
            else
                brush = new SolidBrush(Color.Black);

            g.FillRectangles(brush, border);
        }

        private int checkIfPlayerBigAndGetModifierIndex(Player player)
        {
            if (_powerUpModificationList.ContainsKey(player.guid))
            {
                List<PowerUpModifier> list = _powerUpModificationList[player.guid];
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].GetType().Name == PowerUpModifierKind.makePlayersBigModifier.ToString())
                        return i;
                }
            }
            return -1;
        }


        public void AddPowerUp(PowerUp.PowerUpKind kind)
        {
            _powerupList.Add(new PowerUp(kind));
        }


        public void AddLobbyPlayer(string name, Color color, Guid guid)
        {
            _lobbyList.Add(new Player(name, color, guid));
        }

        public void RemoveLobbyPlayer(Guid guid)
        {
            Player playerToFind = _lobbyList.Find(item => item.guid == guid);
            if(playerToFind != null)
                _lobbyList.Remove(playerToFind);
        }

        public int getPlayerLength(Guid guid)
        {
            Player playerToFind = _playerList.Find(item => item.guid == guid);
            if (playerToFind != null)
                return playerToFind.playerbody.Count;
            else
                return 0;
        }

        public void RemovePowerUp(Guid guid)
        {
            PowerUp powerUpToFind = _powerupList.Find(item => item.guid == guid);
            if(powerUpToFind != null)
                _powerupList.Remove(powerUpToFind);
        }

        public string getLobbyPlayerName(Guid guid)
        {

            string name = "";
            Player playerToFind = _lobbyList.Find(item => item.guid == guid);
            if(playerToFind != null)
                name = playerToFind.name;
            return name;
        }

        public int getFieldX()
        {
            return _field.x;
        }

        public int getFieldY()
        {
            return _field.y;
        }

        public int getFieldScale()
        {
            return _field.Scale;
        }

        public void setScale(int scale)
        {
            _field.Scale = scale;
        }

        // 4 values in order of north east south west
        public void setOffset(int[] offset)
        {
            _field.Offset = offset;
        }

        public void adjustGameFormSize(Form gameForm)
        {
            gameForm.Size = new System.Drawing.Size
                (
                    _field.Offset[3] + (_field.x * _field.Scale) + _field.Offset[1],
                    _field.Offset[0] + (_field.y * _field.Scale) + _field.Offset[2]
                );
        }

        public Dictionary<Guid, CollisionType> CollisionList
        {
            get { return _collisionList; }
        }

        public int TicksUntilAdd
        {
            set
            {
                if (value > 0)
                    _ticksUntilAdd = value;
                else
                    throw new ArgumentOutOfRangeException("TicksUntilAdd", "TicksUntilAdd must be a value greater than 0");
            }
            get { return _ticksUntilAdd; }
        }

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int getRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

    }
}