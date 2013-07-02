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
        public bool goThroughWalls = false;
        private int goThroughWallCounter = 0;

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
            _playerList.Add(new Player(name, color, guid));
        }

        public void addDeadRemoveLivingPlayer(Guid guid)
        {

            for (int i = 0; i < _playerList.Count; i++)
            {
                if (_playerList[i].guid.Equals(guid))
                {
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
            if (goThroughWalls == true)
            {
                goThroughWallCounter++;
                if (goThroughWallCounter > 150)
                {
                    goThroughWalls = false;
                    goThroughWallCounter = 0;
                }
            }

            bool grow = false;
            if (_tickCounter == 0)
                grow = true;

            for (int i = 0; i < _playerList.Count; i++)
            {
                _playerList[i].playerMove(grow);
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

            // draw border
            Rectangle[] border = 
            { 
                new Rectangle(0, 0, width, offset_north), // north
                new Rectangle((offset_west + (_field.x * scale)), 0, offset_east, height), // east
                new Rectangle(0, (offset_north + (_field.y * scale)), width, offset_south), // south
                new Rectangle(0, 0, offset_west, height) // west
            };

            SolidBrush brush;
            if (goThroughWalls == true)
            {
                //if(_tickCounter % 10 == 0)
                    brush = new SolidBrush(Color.Gray);
                //else
                //    brush = new SolidBrush(Color.Gray);
                
            }
            else
            {
                brush = new SolidBrush(Color.Black);
            }

            g.FillRectangles(brush, border);
            //g.DrawRectangles(new Pen(brush), border);

            foreach (PowerUp power in _powerupList)
            {
                int idx = 0;
                foreach (int[] powerUpLocation in power.PowerUpLocation)
                {
                    
                    //Random randonGen = new Random();
                    //Color randomColor = Color.FromArgb(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
                    if (idx > 15)
                    {
                        g.FillRectangle(new SolidBrush(Color.White), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                    }
                    else
                    {
                        if(idx % 2 != 0)
                            g.FillRectangle(new SolidBrush(Color.Gray), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);
                        else
                            g.FillRectangle(new SolidBrush(Color.Black), (offset_west + powerUpLocation[0] * scale), (offset_north + powerUpLocation[1] * scale), scale, scale);

                    }

                    idx++;
                    //g.DrawRectangle(new Pen(player.color, (float)1), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                }
            }

            //draw dead players
            foreach (Player player in _deadList)
            {
                foreach (int[] playerbody in player.playerbody)
                {
                    g.FillRectangle(new SolidBrush(player.color), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                    //g.DrawRectangle(new Pen(player.color, (float)1), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                }
            }

            // draw players
            foreach (Player player in _playerList)
            {
                foreach (int[] playerbody in player.playerbody)
                {
                    g.FillRectangle(new SolidBrush(player.color), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                    //g.DrawRectangle(new Pen(player.color, (float)1), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
                }
            }

        }

        public void AddPowerUp()
        {
            _powerupList.Add(new PowerUp());
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
    }
}
