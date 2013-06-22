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
        private List<Player> _playerlist;
        private List<Player> _lobbyList;
        private Guid _localplayer;
        private Color _localColor;
        private Collision _collision;

        public static Game Instance
        {
            get
            {
                return instance;
            }
        }

        private Game()
        {
            _playerlist = new List<Player>();

            _lobbyList = new List<Player>();

            _field.setSize(200, 200);

            _collision = new Collision(_field.x, _field.y);
        }

        public void setFieldSize(int x, int y)
        {
            _field.setSize(x, y);
        }

        public void addlocalPlayer(string name, Color color)
        {
            Player player = new Player(name, color);
            _localplayer = player.guid;
            _localColor = color;
            _playerlist.Add(player);
        }

        public void setlocalsteering(PlayerSteering playersteering)
        {
            for (int i = 0; i < _playerlist.Count; i++)
            {
                if (_playerlist[i].guid.Equals(_localplayer))
                {
                    _playerlist[i].playersteering = playersteering;
                }
            }
        }

        public void setsteering(Guid playerguid, PlayerSteering playersteering)
        {
            for (int i = 0; i < _playerlist.Count; i++)
            {
                if (_playerlist[i].guid.Equals(playerguid))
                {
                    _playerlist[i].playersteering = playersteering;
                }
            }
        }

        public void addPlayer(string name, Color color, Guid guid)
        {
            _playerlist.Add(new Player(name, color, guid));
        }

        public string getPlayerName(Guid guid){

            string name = "";
            foreach (Player pl in _playerlist)
            {
                if (pl.guid == guid)
                {
                    name = pl.name;
                    break;
                }
            }

            return name;
        }

        public void removePlayer(Guid guid)
        {
            for (int i = 0; i < _playerlist.Count; i++)
            {
                if (_playerlist[i].guid.Equals(guid))
                {
                    _playerlist.RemoveAt(i);
                }
            }
        }

        public void gametick()
        {
            for (int i = 0; i < _playerlist.Count; i++)
            {
                _playerlist[i].playerMove();
            }

            this.DetectCollision();
        }

        public void gamepaint(Graphics g)
        {
            //Pen drawingPen = new Pen(Color.Black, 1);

            //g.DrawRectangle(drawingPen, new Rectangle(new Point(0, 0), this.ClientSize));

            //g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), this.ClientSize));

            foreach (Player player in _playerlist)
            {
                foreach (int[] playerbody in player.playerbody)
                {
                    g.DrawRectangle(new Pen(player.color, (float)1), playerbody[0], playerbody[1], 1, 1);
                }
            }
        }

        public void DetectCollision()
        {
            _collision.DetectCollision(_playerlist);
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
    }
}
