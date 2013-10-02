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
        List<KeyValuePair<int[], int[]>> _bigPlayerLocation;
        //private List<int, int[]> _bigPlayerLocation;
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

        public List<KeyValuePair<int[], int[]>> bigPlayerLocation { get { return _bigPlayerLocation; } set { _bigPlayerLocation = value; } }

        public Player(string name, Color color, Guid guid = new Guid(), int score = 0, int duplicateIndex = 0)
        {
            this._playerbody = new List<int[]>();
            this._bigPlayerLocation = new List<KeyValuePair<int[], int[]>>();
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

            List<Player> duplicateCount = Game.Instance.PLayerList.FindAll(x => x.guid == this.guid);
            if (duplicateCount.Count == 0)
            {
                switch (Game.getRandomNumber(0, 4))
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
            else
            {
                List<Player> playerToFind = Game.Instance.PLayerList.FindAll(item => item.guid == this.guid);
                int[] headCoordinates = playerToFind[duplicateIndex].playerbody[playerToFind[duplicateIndex].playerbody.Count - 1];
                this._name += duplicateCount.Count;
                switch (playerToFind[duplicateIndex].playersteering)
                {
                    case PlayerSteering.Up:
                    case PlayerSteering.Down:
                        if (duplicateCount.Count % 2 == 0)
                        {
                            this._playerbody.Add(new int[2] { headCoordinates[0] - 1, headCoordinates[1] });
                            this._playerbody.Add(new int[2] { headCoordinates[0] - 2, headCoordinates[1] });
                        }
                        else
                        {
                            this._playerbody.Add(new int[2] { headCoordinates[0] + 1, headCoordinates[1] });
                            this._playerbody.Add(new int[2] { headCoordinates[0] + 2, headCoordinates[1] });
                        }
                        this._playersteering = duplicateCount.Count == 2 ? PlayerSteering.Left : PlayerSteering.Right;
                        break;
                    case PlayerSteering.Right:
                    case PlayerSteering.Left:
                        if (duplicateCount.Count % 2 != 0)
                        {
                            this._playerbody.Add(new int[2] { headCoordinates[0], headCoordinates[1] - 1 });
                            this._playerbody.Add(new int[2] { headCoordinates[0], headCoordinates[1] - 2 });
                        }
                        else
                        {
                            this._playerbody.Add(new int[2] { headCoordinates[0], headCoordinates[1] + 1 });
                            this._playerbody.Add(new int[2] { headCoordinates[0], headCoordinates[1] + 2 });
                        }
                        this._playersteering = duplicateCount.Count == 2 ? PlayerSteering.Up : PlayerSteering.Down;
                        break;
                }
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
            // not used List<int[]> checkHead = new List<int[]>();
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
                    break;
                case PlayerSteering.Down:
                    y++;
                    if (playerGoesFast(x, y))
                    {
                        y++;
                        this._playerbody.RemoveAt(0);
                    }
                    break;
                case PlayerSteering.Left:
                    x--;
                    if (playerGoesFast(x, y))
                    {
                        x--;
                        this._playerbody.RemoveAt(0);
                    }
                    break;
                case PlayerSteering.Right:
                    x++;
                    if (playerGoesFast(x, y))
                    {
                        x++;
                        this._playerbody.RemoveAt(0);
                    }
                    break;
            }
            makeSnakeHoles();
            int index =PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.makePlayersBigModifier, this.guid);
            if (index > -1)
            {
                if (Game.Instance.powerUpModificationList[this.guid][index].getCount() > 0)
                this._bigPlayerLocation.Add(new KeyValuePair<int[], int[]>(new int[2] { PowerUp.countBigModifiers(this.guid), (int)this._playersteering},new int[2] { x, y }));
            }

            this._playerbody.Add(new int[2] { x, y });
            makeSnakeSmallerIfOtherPlayerAteRedApple();
            checkIfPlayerShouldGrowThenGivePoint(grow);

        }


        private bool playerGoesFast(int x, int y)
        {

            if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.othersGoFastModifier, this.guid) > -1 || PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.iGoFastModifier, this.guid) > -1)
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
                    _playerbody[_playerbody.Count - 1][0] = -40;
                    _playerbody[_playerbody.Count - 1][1] = -40;
                    if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.makePlayersBigModifier, this._guid) > -1)
                    {
                        _playerbody[_playerbody.Count - 2][0] = -40;
                        _playerbody[_playerbody.Count - 2][1] = -40;
                    }

            }
        }

        private void makeSnakeSmallerIfOtherPlayerAteRedApple()
        {
            if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.redAppleModifier, this._guid) > -1)
            {
                if (playerbody.Count - 1 > 1)
                    this._playerbody.RemoveAt(0);
            }
        }

        private void checkIfPlayerShouldGrowThenGivePoint(bool grow)
        {
            
            if (PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.goldenAppleModifier, this._guid) > -1)
                grow = true;

            if (!grow)
            {
                //this._bigPlayerLocation.Remove(playerbody[0]);
                this._playerbody.RemoveAt(0);

                if (this._bigPlayerLocation.Count > 0)
                {
                    List<int> deleteLocation = new List<int>();
                    for (int i = 0; i < this._bigPlayerLocation.Count; i++)
                    {
                        bool stillInPlayerBody = false;

                        foreach (int[] checkPlayerLocation in this.playerbody)
                        {
                            if (checkPlayerLocation[0] == this._bigPlayerLocation[i].Value[0] && checkPlayerLocation[1] == this._bigPlayerLocation[i].Value[1])
                            {
                                stillInPlayerBody = true;
                                break;
                            }

                        }
                        if (!stillInPlayerBody)
                            deleteLocation.Add(i);
                    }

                    for (int i = deleteLocation.Count - 1; i >= 0; i--)
                    {
                        this._bigPlayerLocation.RemoveAt(deleteLocation[i]);
                    }
                }
                else
                {
                    int modificationIndex = PowerUp.checkIfPlayerHasModification(PowerUpModifierKind.makePlayersBigModifier, this._guid);
                    if(modificationIndex > -1)
                    Game.Instance.powerUpModificationList[this.guid][modificationIndex].setCount(-1);
                }



            }
            else
            {
                this._score++;
            }
        }

        public int getPlayerLocationSize(int[] playerLocation){
            foreach(KeyValuePair<int[], int[]> bigPartLocation in this._bigPlayerLocation)
                    {
                        if (playerLocation[0] == bigPartLocation.Value[0] && playerLocation[1] == bigPartLocation.Value[1])
                            {
                                return bigPartLocation.Key[0];
                            }
                    }
            return 1;
        }

    }
}
