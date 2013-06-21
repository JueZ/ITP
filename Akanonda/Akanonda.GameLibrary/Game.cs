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
        private Guid _localplayer;

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
            
            _field.setSize(100, 100);
        }

        public void setFieldSize(int x, int y)
        {
            _field.setSize(x, y);
        }
        
        public void addlocalPlayer(string name, Color color)
        {
            Player player = new Player(name, color);
            _localplayer = player.guid; 
            _playerlist.Add(player);
        }
        
        public void setlocalsteering(PlayerSteering playersteering)
        {
            for(int i = 0; i < _playerlist.Count; i++)
            {
                if(_playerlist[i].guid.Equals(_localplayer))
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
            for(int i = 0; i < _playerlist.Count; i++)
            {
                _playerlist[i].playerMove();
            }
        }
        
        public void gamepaint(Graphics g)
        {
            Pen drawingPen = new Pen(Color.Black, 1);
            
            //g.DrawRectangle(drawingPen, new Rectangle(new Point(0, 0), this.ClientSize));
            
            //g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), this.ClientSize));
            
            foreach(Player player in _playerlist)
            {
                foreach (int[] playerbody in player.playerbody) 
                {
                    g.DrawRectangle(new Pen(player.color, 1), playerbody[0], playerbody[1], 1, 1);
                }    
            }
        }
    }
}
