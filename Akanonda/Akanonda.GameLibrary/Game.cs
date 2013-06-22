﻿using System;
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

            _field.setSize(120, 120);
            _field.Scale = 5;
            _field.Offset = new int[] {20, 20, 20, 20};

            _collision = new Collision(_field.x, _field.y);
        }

        public List<Player> PLayerList
        {
            get
            {
                return _playerlist;
            }
        }

        public List<Player> LobbyList
        {
            get
            {
                return _lobbyList;
            }
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

            SolidBrush brush = new SolidBrush(Color.Black);
            g.FillRectangles(brush, border);
            g.DrawRectangles(new Pen(brush), border);

            // draw players
            foreach (Player player in _playerlist)
            {
                foreach (int[] playerbody in player.playerbody)
                {
                    g.DrawRectangle(new Pen(player.color, (float)1), (offset_west + playerbody[0] * scale), (offset_north + playerbody[1] * scale), scale, scale);
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
    }
}
