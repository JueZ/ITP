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


        
        //Powerup thingies---> Ne idee wie man das in eine Liste oder Dict zusammenfassen könnte?
        private Dictionary<Guid, int> _goldenAppleDict = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _redAppleDict = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _rabiesDict = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _othersGoSlowDict = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _othersGoFastDict = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _iGoFastDict = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _iGoSlowDict = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _iGoThroughWallsDict = new Dictionary<Guid, int>();

        private int[] _powerUpCounters = new int[4];
        public const int moveAllPowerUps = 0;
        public const int openWalls = 1;
        public const int closingWalls = 2;
        public const int biggerWalls = 3;

        public int powerUpPopUpRate = 79;

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
        public int[] powerUpCounters { get { return _powerUpCounters; } set { _powerUpCounters = value; } }

        public Dictionary<Guid, int> othersGoSlowDict { get { return _othersGoSlowDict; } }
        public Dictionary<Guid, int> goldenAppleDict { get { return _goldenAppleDict; } set { _goldenAppleDict = value; } }
        public Dictionary<Guid, int> redAppleDict { get { return _redAppleDict; } set { _redAppleDict = value; } }
        public Dictionary<Guid, int> rabiesDict { get { return _rabiesDict; } set { _rabiesDict = value; } }
        public Dictionary<Guid, int> othersGoFastDict { get { return _othersGoFastDict; } }
        public Dictionary<Guid, int> iGoFastDict { get { return _iGoFastDict; } }
        public Dictionary<Guid, int> iGoSlowDict { get { return _iGoSlowDict; } }
        public Dictionary<Guid, int> iGoThroughWallsDict { get { return _iGoThroughWallsDict; } }
        //PowerUp get set --------------------END





        public Guid LocalPlayerGuid
        {
            set { _localplayer = value; }
            get { return _localplayer; }
        }

        public PlayerSteering LocalSteering
        {
            set
            {
                for (int i = 0; i < _playerList.Count; i++)
                {
                    if (_playerList[i].guid.Equals(_localplayer))
                    {
                        _playerList[i].playersteering = value;
                        break;
                    }
                }
            }
            get
            {
                for (int i = 0; i < _playerList.Count; i++)
                {
                    if (_playerList[i].guid.Equals(_localplayer))
                    {
                        return _playerList[i].playersteering;
                    }
                }
                throw new Exception("No localplayer found");
            }
        }

        public void setsteering(Guid playerguid, PlayerSteering playersteering)
        {
            for (int i = 0; i < _playerList.Count; i++)
            {
                if (_playerList[i].guid.Equals(playerguid))
                {
                    _playerList[i].playersteering = playersteering;
                    break;
                }
            }
        }

        public void addPlayer(string name, Color color, Guid guid)
        {
            int setScore = 0;
            for (int i = 0; i < _lobbyList.Count; i++)
            {
                if (_lobbyList[i].guid.Equals(guid))
                {
                    setScore = _lobbyList[i].score;
                    break;
                }
            }

            _playerList.Add(new Player(name, color, guid, setScore));
        }

        public void addDeadRemoveLivingPlayer(Guid guid)
        {
            for (int i = 0; i < _playerList.Count; i++)
            {
                if (_playerList[i].guid.Equals(guid))
                {
                    _playerList[i].guid = Guid.NewGuid();
                    _deadList.Add(_playerList[i]);
                    _playerList.RemoveAt(i);
                    break;
                }
            }
        }

        public string getPlayerName(Guid guid)
        {

            string name = "";
            foreach (Player pl in _playerList)
            {
                if (pl.guid == guid)
                {
                    name = pl.name;
                    break;
                }
            }

            return name;
        }

        public Color getPlayerColor(Guid guid)
        {

            Color color = Color.FromName("Black");
            foreach (Player pl in _playerList)
            {
                if (pl.guid == guid)
                {
                    color = pl.color;
                    return color;
                }
            }

            return color;
        }


        public void setScoreToLobbyPlayer(Guid guid)
        {
            int setScore = 0;

            for (int i = 0; i < _playerList.Count; i++)
            {
                if (_playerList[i].guid.Equals(guid))
                {
                    setScore = _playerList[i].score;
                    break;
                }
            }

            for (int i = 0; i < _lobbyList.Count; i++)
            {
                if (_lobbyList[i].guid.Equals(guid))
                {
                    _lobbyList[i].score = setScore;
                    break;
                }
            }

        }

        public void updateScore(Guid guid)
        {
            for (int i = 0; i < _playerList.Count; i++)
            {
                if (_playerList[i].guid.Equals(guid))
                {
                    int addScore = _playerList[i].playerbody.Count / 2;
                    addScore += _playerList[i].SurvivalTime % 60;
                    _playerList[i].score = addScore;
                    break;
                }
            }
        }

        public void removePlayer(Guid guid)
        {
            for (int i = 0; i < _playerList.Count; i++)
            {
                if (_playerList[i].guid.Equals(guid))
                {
                    _playerList.RemoveAt(i);
                    break;
                }
            }
        }

        public void removeDeadPlayer(Guid guid)
        {
            for (int i = 0; i < _deadList.Count; i++)
            {
                if (_deadList[i].guid.Equals(guid))
                {
                    _deadList.RemoveAt(i);
                    break;
                }
            }
        }

        public void gametick()
        {
            _tickCounter = (_tickCounter + 1) % _ticksUntilAdd;
            handlePowerUpTicks();
            movePlayersandAddScore();
            checkIfPowerUpsShouldBeAddedRelativeToFieldsize();
        }

        private void handlePowerUpTicks()
        {
            checkOpenWallCounter();
            checkMovePowerUpsCounter();
            checkIfWallsShouldGetSmallerOrBigger();
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
                    PowerUp.moveAllPowerUps();

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
                        _collision.setCollisionFieldSize(Game.Instance.getFieldX() - 1, Game.Instance.getFieldY() - 1);
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
                if (othersGoSlowDict.ContainsKey(_playerList[i].guid) || iGoSlowDict.ContainsKey(_playerList[i].guid))
                {
                    substrateTickfromSlowDict(i);
                    if (tickCounter % 2 == 0)
                        _playerList[i].playerMove(grow);
                }
                else
                {
                    _playerList[i].playerMove(grow);
                }

                addScoreEvery30Seconds(i);

            }
        }
        private void substrateTickfromSlowDict(int i)
        {
            if (othersGoSlowDict.ContainsKey(_playerList[i].guid))
            {
                if (Game.Instance.othersGoSlowDict[_playerList[i].guid] - 1 > 0)
                    Game.Instance.othersGoSlowDict[_playerList[i].guid]--;
                else
                    Game.Instance.othersGoSlowDict.Remove(_playerList[i].guid);
            }

            if (iGoSlowDict.ContainsKey(_playerList[i].guid))
            {
                if (Game.Instance.iGoSlowDict[_playerList[i].guid] - 1 > 0)
                    Game.Instance.iGoSlowDict[_playerList[i].guid]--;
                else
                    Game.Instance.iGoSlowDict.Remove(_playerList[i].guid);

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
            if (getRandomNumber(0, 9999) % powerUpPopUpRate == 0)
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

        public void DetectCollision()
        {
            _collisionList = _collision.DetectCollision(_playerList);

        }

        public void gamepaint(Graphics g)
        {
            paintPowerUps(g);
            paintAlivePlayers(g);
            paintDeadPlayers(g);
            paintGameBorderBlackOrGrayDependingOnOpenWalls(g);
        }

        private void paintPowerUps(Graphics g)
        {
            foreach (PowerUp power in _powerupList)
            {
                choosePowerUpPaint(power, g);
            }
        }

        private void choosePowerUpPaint(PowerUp power, Graphics g)
        {
            switch (power.kind)
            {
                case PowerUp.PowerUpKind.openWalls:
                case PowerUp.PowerUpKind.iGoThroughWalls:
                    paintPowerUpOpenWallsAndiGoThroughWalls(power, g);
                    break;
                case PowerUp.PowerUpKind.iGoFast:
                case PowerUp.PowerUpKind.othersGoFast:
                    paintPowerUpiGoFastAndOthersGoFast(power, g);
                    break;
                case PowerUp.PowerUpKind.redApple:
                case PowerUp.PowerUpKind.goldenApple:
                case PowerUp.PowerUpKind.rabies:
                    paintPowerUpredAppleandgoldenAppleandRabies(power, g);
                    break;
                case PowerUp.PowerUpKind.movePowerUps:
                    paintPowerUpMovePowerUps(power, g);
                    break;
                case PowerUp.PowerUpKind.iGoSlow:
                case PowerUp.PowerUpKind.othersGoSlow:
                    paintiGoSlowAandothersGoSlow(power, g);
                    break;
                case PowerUp.PowerUpKind.biggerWalls:
                    paintPowerUpBiggerWalls(power, g);
                    break;
                case PowerUp.PowerUpKind.closingWalls:
                    paintPowerUpClosingWalls(power, g);
                    break;
                case PowerUp.PowerUpKind.morePowerUps:
                    paintPowerUpmorePowerUps(power, g);
                    break;
            }
        }


        private void paintPowerUpOpenWallsAndiGoThroughWalls(PowerUp power, Graphics g)
        {
            int idx = 0;
            foreach (int[] powerUpLocation in power.PowerUpLocation)
            {

                if (idx > 15)
                {
                    g.FillRectangle(new SolidBrush(power.kind == PowerUp.PowerUpKind.iGoThroughWalls ? Color.Green : Color.LightSkyBlue),
                        (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                }
                else
                {
                    if (_tickCounter % 10 > 5)
                    {
                        if (idx % 2 != 0)
                            g.FillRectangle(new SolidBrush(power.kind == PowerUp.PowerUpKind.iGoThroughWalls ? Color.Green : Color.LightSkyBlue),
                                (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                        else
                            g.FillRectangle(new SolidBrush(Color.Black), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                    }
                    else
                    {
                        if (idx % 2 == 0)
                            g.FillRectangle(new SolidBrush(power.kind == PowerUp.PowerUpKind.iGoThroughWalls ? Color.Green : Color.LightSkyBlue),
                                (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                        else
                            g.FillRectangle(new SolidBrush(Color.Black), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);

                    }
                }

                idx++;
            }
        }

        private void paintPowerUpiGoFastAndOthersGoFast(PowerUp power, Graphics g)
        {
            int lowestX = 9999;
            int lowestY = 9999;
            
            foreach (int[] powerUpLocation in power.PowerUpLocation)
            {
                
                    lowestX = lowestX < (_field.offsetWest + powerUpLocation[0] * _field.Scale) ? lowestX : (_field.offsetWest + powerUpLocation[0] * _field.Scale);
                    lowestY = lowestY < (_field.offsetNorth + powerUpLocation[1] * _field.Scale) ? lowestY : (_field.offsetNorth + powerUpLocation[1] * _field.Scale);
                
            }
            if (power.kind == PowerUp.PowerUpKind.iGoFast)
            {
                Icon fastIcon = new Icon("iGoFast.ico");
                Rectangle rect = new Rectangle(lowestX - 3, lowestY - 3, 31, 31);
                g.DrawIcon(fastIcon, rect);
            }
            else
            {
                Icon fastIcon = new Icon("othersGoFast.ico");
                Rectangle rect = new Rectangle(lowestX - 3, lowestY - 3, 31, 31);
                g.DrawIcon(fastIcon, rect);
            }
        }

        private void paintPowerUpredAppleandgoldenAppleandRabies(PowerUp power, Graphics g)
        {
            foreach (int[] powerUpLocation in power.PowerUpLocation)
            {
                g.FillRectangle(new SolidBrush(power.kind == PowerUp.PowerUpKind.goldenApple ? Color.Yellow : power.kind == PowerUp.PowerUpKind.rabies ? Color.Black : Color.Red),
                    (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
            }
        }

        private void paintPowerUpMovePowerUps(PowerUp power, Graphics g)
        {
            int idx = 0;
            foreach (int[] powerUpLocation in power.PowerUpLocation)
            {
                switch (idx)
                {
                    case 2:
                    case 6:
                    case 10:
                    case 14:
                    case 16:
                    case 18:
                    case 22:
                    case 24:
                        g.FillRectangle(new SolidBrush(Color.Black), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                        break;
                    default:
                        g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                        break;
                }
                idx++;
            }
        }

        private void paintiGoSlowAandothersGoSlow(PowerUp power, Graphics g)
        {
            int lowestX = 9999;
            int lowestY = 9999;

            foreach (int[] powerUpLocation in power.PowerUpLocation)
            {

                lowestX = lowestX < (_field.offsetWest + powerUpLocation[0] * _field.Scale) ? lowestX : (_field.offsetWest + powerUpLocation[0] * _field.Scale);
                lowestY = lowestY < (_field.offsetNorth + powerUpLocation[1] * _field.Scale) ? lowestY : (_field.offsetNorth + powerUpLocation[1] * _field.Scale);

            }
            if (power.kind == PowerUp.PowerUpKind.iGoSlow)
            {
                Icon slowIcon = new Icon("iGoSlow.ico");
                Rectangle rect = new Rectangle(lowestX - 3, lowestY - 3, 31, 31);
                g.DrawIcon(slowIcon, rect);
            }
            else
            {
                Icon slowIcon = new Icon("othersGoSlow.ico");
                Rectangle rect = new Rectangle(lowestX - 3, lowestY - 3, 31, 31);
                g.DrawIcon(slowIcon, rect);
            }
        }

        private void paintPowerUpBiggerWalls(PowerUp power, Graphics g)
        {
            int idx = 0;
            foreach (int[] powerUpLocation in power.PowerUpLocation)
            {

                if (idx > 15)
                {
                    g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.Black), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                }

                idx++;
            }
        }

        private void paintPowerUpClosingWalls(PowerUp power, Graphics g)
        {
            int idx = 0;
            foreach (int[] powerUpLocation in power.PowerUpLocation)
            {
                if (idx < 15)
                {
                    g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                }
                else
                {
                    if (idx == 20 || idx == 15)
                        g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                    else
                        g.FillRectangle(new SolidBrush(Color.Black), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                }

                idx++;
            }
        }

        private void paintPowerUpmorePowerUps(PowerUp power, Graphics g)
        {
            int idx = 0;
            Random randonGen = new Random();
            Color randomColor = Color.White;
            if (_tickCounter % 10 == 5 || _tickCounter % 5 == 2)
                randomColor = Color.FromArgb(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));

            foreach (int[] powerUpLocation in power.PowerUpLocation)
            {
                switch (idx)
                {
                    case 2:
                    case 6:
                    case 10:
                    case 14:
                    case 16:
                    case 18:
                    case 22:
                    case 24:
                        g.FillRectangle(new SolidBrush(Color.White), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                        break;
                    default:
                        g.FillRectangle(new SolidBrush(randomColor), (_field.offsetWest + powerUpLocation[0] * _field.Scale), (_field.offsetNorth + powerUpLocation[1] * _field.Scale), _field.Scale, _field.Scale);
                        break;
                }
                idx++;
            }
        }

        private void paintAlivePlayers(Graphics g)
        {
            foreach (Player player in _playerList)
            {
                foreach (int[] playerbody in player.playerbody)
                {
                    if (playerbody[0] > -1 && playerbody[0] < getFieldX() && playerbody[1] > -1 && playerbody[1] < getFieldY())
                        g.FillRectangle(new SolidBrush(player.color), (_field.offsetWest + playerbody[0] * _field.Scale), (_field.offsetNorth + playerbody[1] * _field.Scale), _field.Scale, _field.Scale);
                }
            }
        }


        private void paintDeadPlayers(Graphics g)
        {
            foreach (Player player in _deadList)
            {
                foreach (int[] playerbody in player.playerbody)
                {
                    if (playerbody[0] > -1 && playerbody[0] < getFieldX() && playerbody[1] > -1 && playerbody[1] < getFieldY())
                        g.FillRectangle(new SolidBrush(player.color), (_field.offsetWest + playerbody[0] * _field.Scale), (_field.offsetNorth + playerbody[1] * _field.Scale), _field.Scale, _field.Scale);
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
            for (int i = 0; i < _lobbyList.Count; i++)
            {
                if (_lobbyList[i].guid.Equals(guid))
                {
                    _lobbyList.RemoveAt(i);
                    break;
                }
            }
        }

        public int getPlayerLength(Guid guid)
        {
            foreach (Player p in _playerList)
            {
                if (p.guid.Equals(guid))
                {
                    return p.playerbody.Count;
                }
            }
            return 0;
        }

        public void RemovePowerUp(Guid guid)
        {
            for (int i = 0; i < _powerupList.Count; i++)
            {
                if (_powerupList[i].guid.Equals(guid))
                {
                    _powerupList.RemoveAt(i);
                    break;
                }
            }
        }

        public string getLobbyPlayerName(Guid guid)
        {

            string name = "";
            foreach (Player pl in _lobbyList)
            {
                if (pl.guid == guid)
                {
                    name = pl.name;
                    break;
                }
            }

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