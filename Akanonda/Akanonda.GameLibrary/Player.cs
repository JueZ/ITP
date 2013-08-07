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
        int startX, startY;
        private int _score;
        private int _survivalTime = 1;

        public string name
        {
            get { return _name; }
        }

        public int score
        {
            get { return _score; }
            set { _score = value; }
        }

        public int SurvivalTime
        {
            get { return _survivalTime; }
            set { _survivalTime = value; }
        }

        public Color color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Guid guid
        {
            get { return _guid; }
            set { _guid = value; }
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

        public Player(string name, Color color, Guid guid = new Guid(), int score = 0)
        {
            this._playerbody = new List<int[]>();

            startX = Game.getRandomNumber(20, Game.Instance.getFieldX() - 20);
            startY = Game.getRandomNumber(20, Game.Instance.getFieldY() - 20);

            this._name = name;
            this._color = color;
            this._score = score;
            this._survivalTime = 1;

            var guidIsEmpty = guid == Guid.Empty;
            if (guidIsEmpty)
                this._guid = Guid.NewGuid();
            else
                this._guid = guid;

            this._playerstatus = PlayerStatus.None;


            int direction = Game.getRandomNumber(0, 4);
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
            bool bigSnake = false;
            List<int[]> checkHead = new List<int[]>();
            if (PowerUp.checkIfPlayerHasModification(new makePlayersBigModifier().GetType(), this.guid) > -1)
                bigSnake = true;

            if (bigSnake)
            {
                for (int i = playerbody.Count - 1; i > playerbody.Count - 7; i--)
                {
                    checkHead.Add(playerbody[i]);
                }

            }

            int x = _playerbody[_playerbody.Count - 1][0];
            int y = _playerbody[_playerbody.Count - 1][1];



            switch (this._playersteering)
            {
                case PlayerSteering.Up:
                    y--;
                    if (playerGoesFast(x, y))
                    {
                        y--;
                        this._playerbody.RemoveAt(0);
                    }
                    if (bigSnake)
                    {
                        if (!checkHead.Exists(item => item[0] == x - 1 && item[1] == y))
                            this._playerbody.Add(new int[2] { x - 1, y });
                        if (!checkHead.Exists(item => item[0] == x + 1 && item[1] == y))
                            this._playerbody.Add(new int[2] { x + 1, y });
                    }
                    break;
                case PlayerSteering.Down:
                    y++;
                    if (playerGoesFast(x, y))
                    {
                        y++;
                        this._playerbody.RemoveAt(0);
                    }
                    if (bigSnake)
                    {
                        if (!checkHead.Exists(item => item[0] == x - 1 && item[1] == y))
                            this._playerbody.Add(new int[2] { x - 1, y });
                        if (!checkHead.Exists(item => item[0] == x + 1 && item[1] == y))
                            this._playerbody.Add(new int[2] { x + 1, y });
                    }
                    break;
                case PlayerSteering.Left:
                    x--;
                    if (playerGoesFast(x, y))
                    {
                        x--;
                        this._playerbody.RemoveAt(0);
                    }
                    if (bigSnake)
                    {
                        if (!checkHead.Exists(item => item[0] == x && item[1] == y - 1))
                            this._playerbody.Add(new int[2] { x, y - 1 });
                        if (!checkHead.Exists(item => item[0] == x && item[1] == y + 1))
                            this._playerbody.Add(new int[2] { x, y + 1 });
                    }
                    break;
                case PlayerSteering.Right:
                    x++;
                    if (playerGoesFast(x, y))
                    {
                        x++;
                        this._playerbody.RemoveAt(0);
                    }
                    if (bigSnake)
                    {
                        if (!checkHead.Exists(item => item[0] == x && item[1] == y - 1))
                            this._playerbody.Add(new int[2] { x, y - 1 });
                        if (!checkHead.Exists(item => item[0] == x && item[1] == y + 1))
                            this._playerbody.Add(new int[2] { x, y + 1 });
                    }
                    break;
            }
            makeSnakeHoles();
            if (!checkHead.Exists(item => item[0] == x && item[1] == y))
                this._playerbody.Add(new int[2] { x, y });

            makeSnakeSmallerIfOtherPlayerAteRedApple();
            checkIfPlayerShouldGrowThenGivePoint(grow);

        }


        private bool playerGoesFast(int x, int y)
        {

            if (PowerUp.checkIfPlayerHasModification(new othersGoFastModifier().GetType(), this.guid) > -1 || PowerUp.checkIfPlayerHasModification(new iGoFastModifier().GetType(), this.guid) > -1)
            {
                this._playerbody.Add(new int[2] { x, y });
                return true;
            }
            return false;
        }

        private void makeSnakeHoles()
        {
            if (Game.getRandomNumber(0, 20) % 20 == 0)
            {
                if (PowerUp.checkIfPlayerHasModification(new makePlayersBigModifier().GetType(), this.guid) > -1)
                {
                    _playerbody[_playerbody.Count - 3][0] = -40;
                    _playerbody[_playerbody.Count - 3][1] = -40;
                    _playerbody[_playerbody.Count - 4][0] = -40;
                    _playerbody[_playerbody.Count - 4][1] = -40;
                    _playerbody[_playerbody.Count - 5][0] = -40;
                    _playerbody[_playerbody.Count - 5][1] = -40;
                    _playerbody[_playerbody.Count - 6][0] = -40;
                    _playerbody[_playerbody.Count - 6][1] = -40;
                    _playerbody[_playerbody.Count - 7][0] = -40;
                    _playerbody[_playerbody.Count - 7][1] = -40;
                    _playerbody[_playerbody.Count - 8][0] = -40;
                    _playerbody[_playerbody.Count - 8][1] = -40;
                }
                else
                {
                    _playerbody[_playerbody.Count - 1][0] = -40;
                    _playerbody[_playerbody.Count - 1][1] = -40;
                }
            }
        }

        private void makeSnakeSmallerIfOtherPlayerAteRedApple()
        {
            if (PowerUp.checkIfPlayerHasModification(new redAppleModifier().GetType(), this._guid) > -1)
            {
                if (playerbody.Count - 1 > 1)
                    this._playerbody.RemoveAt(0);
            }
        }

        private void checkIfPlayerShouldGrowThenGivePoint(bool grow)
        {
            if (PowerUp.checkIfPlayerHasModification(new goldenAppleModifier().GetType(), this._guid) > -1)
                grow = true;

            if (!grow)
            {
                this._playerbody.RemoveAt(0);
                if (PowerUp.checkIfPlayerHasModification(new makePlayersBigModifier().GetType(), this.guid) > -1)
                {
                    this._playerbody.RemoveAt(0);
                    this._playerbody.RemoveAt(0);
                }
            }
            else
            {
                this._score++;
            }
        }
    }
}
