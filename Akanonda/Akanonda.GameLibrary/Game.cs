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
        //private Color _localColor; // compiler sagt wird nirgens verwendet?
        private Collision _collision;
        private int _ticksUntilAdd;
        private int _tickCounter;
        private Dictionary<Guid, CollisionType> _collisionList;

        //Powerup thingies
        //private bool _goThroughWalls = false;
        private int _goThroughWallCounter = 0;
        //private bool _othersGoFast = false;
        private int _othersGoFastCounter = 0;
        private Dictionary<Guid, int> _goldenAppleDict = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _redAppleDict = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _rabiesDict = new Dictionary<Guid, int>();
        private List<Guid> _othersGoSlowList = new List<Guid>();
        private List<Guid> _othersGoFastList = new List<Guid>();
        private int _othersGoSlowCounter = 0;
        //private bool _othersGoSlow = false;
        //private bool _movePowerUps = false;
        private int _movePowerUpsCounter = 0;
        //private bool _iGoFast = false;
        private int _iGoFastCounter = 0;
        private List<Guid> _iGoFastList = new List<Guid>();
        //private bool _iGoSlow = false;
        private int _iGoSlowCounter = 0;
        private List<Guid> _iGoSlowList = new List<Guid>();
        private int _closingWallsCounter = 0;
        private int _biggerWallsCounter = 0;

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
        }

        public List<Player> PLayerList
        {
            get
            {
                return _playerList;
            }
        }

        public List<Player> LobbyList
        {
            get
            {
                return _lobbyList;
            }
        }

        public List<Player> DeadList
        {
            get
            {
                return _deadList;
            }
        }

        public List<PowerUp> PowerUpList
        {
            get
            {
                return _powerupList;
            }
        }

        public void setFieldSize(int x, int y)
        {
            _field.setSize(x, y);
        }

        public int tickCounter
        {
            get { return _tickCounter; }
        }

        //PowerUp get set --------------------
        
        public int othersGoFastCounter
        {
            get { return _othersGoFastCounter; }
            set { _othersGoFastCounter = value; }
        }
        public int goThroughWallCounter
        {
            get { return _goThroughWallCounter; }
            set { _goThroughWallCounter = value; }
        }
        public int othersGoSlowCounter
        {
            get { return _othersGoSlowCounter; }
            set { _othersGoSlowCounter = value; }
        }
        public List<Guid> othersGoSlowList
        {
            get { return _othersGoSlowList; }
        }
     
        public Dictionary<Guid, int> goldenAppleDict
        {
            get { return _goldenAppleDict; }
            set { _goldenAppleDict = value; }
        }
        public Dictionary<Guid, int> redAppleDict
        {
            get { return _redAppleDict; }
            set { _redAppleDict = value; }
        }
        public Dictionary<Guid, int> rabiesDict
        {
            get { return _rabiesDict; }
            set { _rabiesDict = value; }
        }
       
        public int movePowerUpsCounter
        {
            get { return _movePowerUpsCounter; }
            set { _movePowerUpsCounter = value; }
        }
        public List<Guid> othersGoFastList
        {
            get { return _othersGoFastList; }
        }

        public int iGoFastCounter
        {
            get { return _iGoFastCounter; }
            set { _iGoFastCounter = value; }
        }
        public List<Guid> iGoFastList
        {
            get { return _iGoFastList; }
        }
        
        public int iGoSlowCounter
        {
            get { return _iGoSlowCounter; }
            set { _iGoSlowCounter = value; }
        }
        public List<Guid> iGoSlowList
        {
            get { return _iGoSlowList; }
        }
        public int closingWallsCounter
        {
            get { return _closingWallsCounter; }
            set { _closingWallsCounter = value; }
        }

        public int biggerWallsCounter
        {
            get { return _biggerWallsCounter; }
            set { _biggerWallsCounter = value; }
        }
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
                        //break;
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
        public string getPlayerName(Guid guid){

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
                    break;
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

        //public void setScoreToGamePlayer(Guid guid)
        //{
        //    int setScore = 0;
        //    for (int i = 0; i < _lobbyList.Count; i++)
        //    {
        //        if (_lobbyList[i].guid.Equals(guid))
        //        {
        //            setScore = _lobbyList[i].score;
        //            break;
        //        }
        //    }
        //    for (int i = 0; i < _playerList.Count; i++)
        //    {
        //        if (_playerList[i].guid.Equals(guid))
        //        {
        //            _playerList[i].score = setScore;
        //            break;
        //        }
        //    }

        //}


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
            if (goThroughWallCounter > 0)
            {
                goThroughWallCounter--;
            }

            if (othersGoFastCounter > 0)
            {
                othersGoFastCounter--;
            }

            if (iGoFastCounter > 0)
            {
                iGoFastCounter--;
            }

            if (iGoSlowCounter > 0)
            {
                iGoSlowCounter--;
            }

            if (movePowerUpsCounter > 0)
            {
                if (_tickCounter % 2 == 0) //Powerup moving speed
                    PowerUp.moveAllPowerUps();

                movePowerUpsCounter--;
                if (movePowerUpsCounter <= 0)
                    PowerUp.moveAllPowerUps(true);
            }

            if (othersGoSlowCounter > 0)
            {
                othersGoSlowCounter--;
            }

            if (closingWallsCounter > 0)
            {
                closingWallsCounter--;
                if (tickCounter % 3 == 0) //closingWall speed
                {
                    if (Game.Instance.getFieldx() > 60 && Game.Instance.getFieldy() > 60)
                    {
                        setFieldSize(Game.Instance.getFieldx() - 1, Game.Instance.getFieldy() - 1);
                        _collision.setCollision(Game.Instance.getFieldx() - 1, Game.Instance.getFieldy() - 1);
                        PowerUp.moveAllTouchingPowerUps();
                    }
                }
               // if (closingWallsCounter <= 0)
                    //movePowerUpsCounter += 15;
            }

            if (biggerWallsCounter > 0)
            {
                biggerWallsCounter--;
                if (tickCounter % 3 == 0) //biggerWall speed
                {
                    if (Game.Instance.getFieldx() > 60 && Game.Instance.getFieldy() > 60)
                    {
                        setFieldSize(Game.Instance.getFieldx() + 1, Game.Instance.getFieldy() + 1);
                        _collision.setCollision(Game.Instance.getFieldx() + 1, Game.Instance.getFieldy() + 1);
                        //PowerUp.moveAllTouchingPowerUps();
                    }
                }
                // if (closingWallsCounter <= 0)
                //movePowerUpsCounter += 15;
            }

            bool grow = false;
            if (_tickCounter == 0)
                grow = true;

            for (int i = 0; i < _playerList.Count; i++)
            {

                if (othersGoSlowCounter > 0 || iGoSlowCounter > 0)
                {
                    if(othersGoSlowList.Contains(_playerList[i].guid) || iGoSlowList.Contains(_playerList[i].guid))
                    {
                        if (tickCounter % 2 == 0)
                            _playerList[i].playerMove(grow);
                    }
                    else
                    {
                        _playerList[i].playerMove(grow);
                    }
                }
                else
                {
                    _playerList[i].playerMove(grow);
                }

                if (_playerList[i].SurvivalTime % 30 == 0)
                {
                    _playerList[i].score += 10; //for ervery half minute of survived time u get 10 points
                    _playerList[i].SurvivalTime = 1;
                }
                //_playerList[i].playerMove(grow);
                
                
                
            }
            
            if (PowerUpList.Count < getFieldx() / 8 && PLayerList.Count > 0) //powerups according to fieldsize
            {
                if (getRandomNumber(0, 9999) % 79 == 0)
                {
                    if (getRandomNumber(0, 9999) % 11 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.othersGoFast);

                    if (getRandomNumber(0, 9999) % 11 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.iGoFast);

                    if (getRandomNumber(0, 9999) % 11 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.openWalls);

                    if (getRandomNumber(0, 9999) % 11 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.movePowerUps);

                    if (getRandomNumber(0, 9999) % 15 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.goldenApple);

                    if (getRandomNumber(0, 9999) % 15 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.redApple);

                    if (getRandomNumber(0, 9999) % 13 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.rabies);

                    if (getRandomNumber(0, 9999) % 11 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.othersGoSlow);

                    if (getRandomNumber(0, 9999) % 11 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.iGoSlow);

                    if (getRandomNumber(0, 9999) % 11 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.closingWalls);

                    if (getRandomNumber(0, 9999) % 11 == 0)
                        AddPowerUp(PowerUp.PowerUpKind.biggerWalls);

                }
            }


        }

        public void DetectCollision()
        {
             _collisionList = _collision.DetectCollision(_playerList);
             
        }

        public void gamepaint(Graphics g)
        {
            int scale = _field.Scale;

            int offset_north = _field.Offset[0];
            int offset_east = _field.Offset[1];
            int offset_south = _field.Offset[2];
            int offset_west = _field.Offset[3];

            int width = offset_west + (_field.x * _field.Scale) + offset_east;
            int height = offset_north + (_field.y * _field.Scale) + offset_south;

            

            
            //g.DrawRectangles(new Pen(brush), border);

            foreach (PowerUp power in _powerupList)
            {
                int idx = 0;
                switch(power.kind)
                {
                    case PowerUp.PowerUpKind.openWalls:
                            foreach (int[] powerUpLocation in power.PowerUpLocation)
                            {
                    
                                //Random randonGen = new Random();
                                //Color randomColor = Color.FromArgb(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
                                if (idx > 15)
                                {
                                    g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                }
                                else
                                {
                                    if (_tickCounter % 10 > 5)
                                    {
                                        if (idx % 2 != 0)
                                            g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                        else
                                            g.FillRectangle(new SolidBrush(Color.Black), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                    }
                                    else
                                    {
                                        if (idx % 2 == 0)
                                            g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                        else
                                            g.FillRectangle(new SolidBrush(Color.Black), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);

                                    }
                                }

                                idx++;
                                //g.DrawRectangle(new Pen(player.color, (float)1), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                            }
                            break;
                    case PowerUp.PowerUpKind.iGoFast:
                    case PowerUp.PowerUpKind.othersGoFast:
                            idx = 0;
                            foreach (int[] powerUpLocation in power.PowerUpLocation)
                            {
                                switch (idx)
                                {
                                    case 2:
                                    case 6:
                                    case 10:
                                    case 14:
                                    case 16:
                                    case 19:
                                    case 20:
                                    case 21:
                                    case 22:
                                        g.FillRectangle(new SolidBrush(Color.Black), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                        break;
                                    default:
                                        g.FillRectangle(new SolidBrush(power.kind == PowerUp.PowerUpKind.iGoFast ? Color.Green : Color.Red), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                        break;
                                }
                                idx++;
                            }
                            
                        break;
                    case PowerUp.PowerUpKind.redApple:
                    case PowerUp.PowerUpKind.goldenApple:
                    case PowerUp.PowerUpKind.rabies:
                        foreach (int[] powerUpLocation in power.PowerUpLocation)
                        {
                            g.FillRectangle(new SolidBrush(power.kind == PowerUp.PowerUpKind.goldenApple ? Color.Yellow : power.kind == PowerUp.PowerUpKind.rabies ? Color.Black : Color.Red), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                        }
                        break;
                    case PowerUp.PowerUpKind.movePowerUps:
                        idx = 0;
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
                                        g.FillRectangle(new SolidBrush(Color.Black), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                        break;
                                    default:
                                        g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                        break;
                                }
                                idx++;
                            }
                        break;
                    case PowerUp.PowerUpKind.iGoSlow:
                    case PowerUp.PowerUpKind.othersGoSlow:
                        idx = 0;
                            foreach (int[] powerUpLocation in power.PowerUpLocation)
                            {
                                switch (idx)
                                {
                                    case 2:
                                    case 6:
                                    case 10:
                                    case 14:
                                    case 18:
                                    case 19:
                                    case 20:
                                    case 21:
                                    case 24:
                                        g.FillRectangle(new SolidBrush(Color.Black), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                        break;
                                    default:
                                        g.FillRectangle(new SolidBrush(power.kind == PowerUp.PowerUpKind.iGoSlow ? Color.Green : Color.Red), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                        break;
                                }
                                idx++;
                            }
                        break;
                    case PowerUp.PowerUpKind.biggerWalls:
                        foreach (int[] powerUpLocation in power.PowerUpLocation)
                        {

                            //Random randonGen = new Random();
                            //Color randomColor = Color.FromArgb(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
                            if (idx > 15)
                            {
                                g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                            }
                            else
                            {
                                g.FillRectangle(new SolidBrush(Color.Black), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                            }

                            idx++;
                            //g.DrawRectangle(new Pen(player.color, (float)1), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                        }
                        break;
                    case PowerUp.PowerUpKind.closingWalls:
                        foreach (int[] powerUpLocation in power.PowerUpLocation)
                        {

                            //Random randonGen = new Random();
                            //Color randomColor = Color.FromArgb(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
                            if (idx < 15)
                            {
                                g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                            }
                            else
                            {
                                if (idx == 20 || idx == 15)
                                    g.FillRectangle(new SolidBrush(Color.LightSkyBlue), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                                else
                                    g.FillRectangle(new SolidBrush(Color.Black), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                            }

                            idx++;
                            //g.DrawRectangle(new Pen(player.color, (float)1), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                        }
                        break;
                }
            }
            // draw border
            Rectangle[] border = 
            { 
                new Rectangle(0, 0, width, offset_north), // north
                new Rectangle((offset_west + (_field.x * scale)), 0, offset_east, height), // east
                new Rectangle(0, (offset_north + (_field.y * scale)), width, offset_south), // south
                new Rectangle(0, 0, offset_west, height) // west
            };

            SolidBrush brush;
            if (goThroughWallCounter > 0)
            {
                brush = new SolidBrush(Color.Gray);
            }
            else
            {
                brush = new SolidBrush(Color.Black);
            }

            g.FillRectangles(brush, border);



            //draw dead players
            foreach (Player player in _deadList)
            {
                foreach (int[] playerbody in player.playerbody)
                {
                    if(playerbody[0] > -1 && playerbody[0] < getFieldx() && playerbody[1] > -1 && playerbody[1] < getFieldy())
                    g.FillRectangle(new SolidBrush(player.color), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                    //g.DrawRectangle(new Pen(player.color, (float)1), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                }
            }

            // draw players
            foreach (Player player in _playerList)
            {
                foreach (int[] playerbody in player.playerbody)
                {
                    if (playerbody[0] > -1 && playerbody[0] < getFieldx() && playerbody[1] > -1 && playerbody[1] < getFieldy())
                    g.FillRectangle(new SolidBrush(player.color), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                    //g.DrawRectangle(new Pen(player.color, (float)1), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                }
            }

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
            foreach(Player p in _playerList)
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

        public int getFieldx()
        {
            return _field.x;
        }

        public int getFieldy()
        {
            return _field.y;
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
            gameForm.ClientSize = new Size
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
