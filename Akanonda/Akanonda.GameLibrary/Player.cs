using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Akanonda.GameLibrary
{
    [Serializable()]
    public class Player
    {
        private string _name;
        private Color _color;
        private Guid _guid;
        private List<int[]> _playerbody;
        private PlayerStatus _playerstatus;
        private PlayerSteering _playersteering;

        public string name
        {
            get { return _name; }
            //set { _name = value; }
        }

        public Color color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Guid guid
        {
            get { return _guid; }
        }

        public PlayerStatus playerstatus
        {
            get { return _playerstatus; }
            set { _playerstatus = value; }
        }

        public PlayerSteering playersteering
        {
            get { return _playersteering; }
            set { _playersteering = value; }
        }
        
        public List<int[]> playerbody
        {
            get { return _playerbody; }
        }

        public Player(string name, Color color)
        {
            this._playerbody = new List<int[]>();

            this._playerbody.Add(new int[2] { 106, 100 });
            this._playerbody.Add(new int[2] { 105, 101 });
            this._playerbody.Add(new int[2] { 104, 102 });
            this._playerbody.Add(new int[2] { 103, 103 });
            this._playerbody.Add(new int[2] { 102, 104 });
            this._playerbody.Add(new int[2] { 101, 105 });

            this._name = name;
            this._color = color;
            this._guid = Guid.NewGuid();
            this._playerstatus = PlayerStatus.None;
            this._playersteering = PlayerSteering.Up;
        }

        public Player(string name, Color color, Guid guid)
        {
            this._playerbody = new List<int[]>();

            this._playerbody.Add(new int[2] { 106, 100 });
            this._playerbody.Add(new int[2] { 105, 101 });
            this._playerbody.Add(new int[2] { 104, 102 });
            this._playerbody.Add(new int[2] { 103, 103 });
            this._playerbody.Add(new int[2] { 102, 104 });
            this._playerbody.Add(new int[2] { 101, 105 });
            
            this._name = name;
            this._color = color;
            this._guid = guid;
            this._playerstatus = PlayerStatus.None;
            this._playersteering = PlayerSteering.Up;
        }

        public Guid initPlayer(string name, Color color)
        {
            this._name = name;
            this._color = color;
            this._guid = Guid.NewGuid();
            this._playerstatus = PlayerStatus.None;
            
            return this._guid;
        }
        
        public void playerMove(bool grow)
        {
            int x = _playerbody[_playerbody.Count-1][0];
            int y = _playerbody[_playerbody.Count-1][1];
            
            switch (this._playersteering) 
            {
                case PlayerSteering.Up:
                    y--;
                    break;
                case PlayerSteering.Down:
                    y++;
                    break;
                case PlayerSteering.Left:
                    x--;
                    break;
                case PlayerSteering.Right:
                    x++;
                    break;
            }
            
            this._playerbody.Add(new int[2] {x, y});
            
            if (!grow)
                this._playerbody.RemoveAt(0);
        }
        
//        public void initPlayer(string name, Color color, Guid guid)
//        {
//            this._name = name;
//            this._color = color;
//            this._guid = guid;
//            this._playerstatus = PlayerStatus.None;
//        }

    }
}
