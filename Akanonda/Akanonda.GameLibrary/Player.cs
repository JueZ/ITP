﻿using System;
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
        int startX, startY;

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

        public Player(string name, Color color, Guid guid = new Guid())
        {
            this._playerbody = new List<int[]>();

            startX = Game.getRandomNumber(20, 100);
            startY = Game.getRandomNumber(20, 100);

            this._name = name;
            this._color = color;

            var guidIsEmpty = guid == Guid.Empty;
            if (guidIsEmpty)
                this._guid = Guid.NewGuid();
            else
                this._guid = guid;

            this._playerstatus = PlayerStatus.None;


            int direction = Game.getRandomNumber(0,4);
            switch (direction)
            {
                case 0:
                    this._playerbody.Add(new int[2] { startX, startY });
                    this._playerbody.Add(new int[2] { startX, startY - 1 });
                    this._playerbody.Add(new int[2] { startX, startY - 2 });
                    this._playerbody.Add(new int[2] { startX, startY - 3 });
                    this._playerbody.Add(new int[2] { startX, startY - 4 });
                    this._playerbody.Add(new int[2] { startX, startY - 5 });
                    this._playersteering = PlayerSteering.Up;
                    break;
                case 1:
                    this._playerbody.Add(new int[2] { startX, startY });
                    this._playerbody.Add(new int[2] { startX + 1, startY });
                    this._playerbody.Add(new int[2] { startX + 2, startY });
                    this._playerbody.Add(new int[2] { startX + 3, startY });
                    this._playerbody.Add(new int[2] { startX + 4, startY });
                    this._playerbody.Add(new int[2] { startX + 5, startY });
                    this._playersteering = PlayerSteering.Right;
                    break;
                case 2:
                    this._playerbody.Add(new int[2] { startX, startY });
                    this._playerbody.Add(new int[2] { startX, startY + 1 });
                    this._playerbody.Add(new int[2] { startX, startY + 2 });
                    this._playerbody.Add(new int[2] { startX, startY + 3 });
                    this._playerbody.Add(new int[2] { startX, startY + 4 });
                    this._playerbody.Add(new int[2] { startX, startY + 5 });
                    this._playersteering = PlayerSteering.Down;
                    break;
                case 3:
                    this._playerbody.Add(new int[2] { startX, startY });
                    this._playerbody.Add(new int[2] { startX - 1, startY });
                    this._playerbody.Add(new int[2] { startX - 2, startY });
                    this._playerbody.Add(new int[2] { startX - 3, startY });
                    this._playerbody.Add(new int[2] { startX - 4, startY });
                    this._playerbody.Add(new int[2] { startX - 5, startY });
                    this._playersteering = PlayerSteering.Left;
                    break;
            }


            
        }

        //public Player(string name, Color color, Guid guid)
        //{

        //    this._playerbody = new List<int[]>();

        //    startX = rndX.Next(20, 100);
        //    startY = rndY.Next(20, 100);
            
        //    this._name = name;
        //    this._color = color;
        //    this._guid = guid;
        //    this._playerstatus = PlayerStatus.None;

        //    Random rnd = new Random();
        //    int direction = rnd.Next(4);
        //    switch (direction)
        //    {
        //        case 0:
        //            this._playerbody.Add(new int[2] { startX, startY });
        //            this._playerbody.Add(new int[2] { startX, startY - 1 });
        //            this._playerbody.Add(new int[2] { startX, startY - 2 });
        //            this._playerbody.Add(new int[2] { startX, startY - 3 });
        //            this._playerbody.Add(new int[2] { startX, startY - 4 });
        //            this._playerbody.Add(new int[2] { startX, startY - 5 });
        //            this._playersteering = PlayerSteering.Up;
        //            break;
        //        case 1:
        //            this._playerbody.Add(new int[2] { startX, startY });
        //            this._playerbody.Add(new int[2] { startX + 1, startY });
        //            this._playerbody.Add(new int[2] { startX + 2, startY });
        //            this._playerbody.Add(new int[2] { startX + 3, startY });
        //            this._playerbody.Add(new int[2] { startX + 4, startY });
        //            this._playerbody.Add(new int[2] { startX + 5, startY });
        //            this._playersteering = PlayerSteering.Right;
        //            break;
        //        case 2:
        //            this._playerbody.Add(new int[2] { startX, startY });
        //            this._playerbody.Add(new int[2] { startX, startY + 1 });
        //            this._playerbody.Add(new int[2] { startX, startY + 2 });
        //            this._playerbody.Add(new int[2] { startX, startY + 3 });
        //            this._playerbody.Add(new int[2] { startX, startY + 4 });
        //            this._playerbody.Add(new int[2] { startX, startY + 5 });
        //            this._playersteering = PlayerSteering.Down;
        //            break;
        //        case 3:
        //            this._playerbody.Add(new int[2] { startX, startY });
        //            this._playerbody.Add(new int[2] { startX - 1, startY });
        //            this._playerbody.Add(new int[2] { startX - 2, startY });
        //            this._playerbody.Add(new int[2] { startX - 3, startY });
        //            this._playerbody.Add(new int[2] { startX - 4, startY });
        //            this._playerbody.Add(new int[2] { startX - 5, startY });
        //            this._playersteering = PlayerSteering.Left;
        //            break;
        //    }

        //}

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
                    if (Game.Instance.goFast)
                        // oder if (!grow)
                        //this._playerbody.RemoveAt(0);
                        //y--;
                        //this._playerbody.RemoveAt(0);
                        //finds aber eig cool so
                        y--;
                    break;
                case PlayerSteering.Down:
                    y++;
                    if (Game.Instance.goFast)
                        y++;
                    break;
                case PlayerSteering.Left:
                    x--;
                    if (Game.Instance.goFast)
                        x--;
                    break;
                case PlayerSteering.Right:
                    x++;
                    if (Game.Instance.goFast)
                        x++;
                    break;
            }
            
            this._playerbody.Add(new int[2] {x, y});

            foreach (KeyValuePair<Guid,int> item in Game.Instance.goldenAppleDict)
            {
                if (item.Key.Equals(this.guid))
                {
                    grow = true;
                    Game.Instance.goldenAppleDict.Remove(item.Key);
                    if(item.Value - 1 > 0)
                    Game.Instance.goldenAppleDict.Add(item.Key, item.Value - 1);
                    break;
                }
            }

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
