using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{
    [Serializable]
    public struct Field
    {
        private int _x;
        private int _y;
        private int _scale;
        private int[] _offset;

        public int x
        {
            get { return _x; }
        }

        public int y
        {
            get { return _y; }
        }

        public void setSize(int x, int y)
        {
            if (x < 1)
                throw new ArgumentOutOfRangeException("x","Gamefield must have a size greater than 0");
            if (y < 1)
                throw new ArgumentOutOfRangeException("y", "Gamefield must have a size greater than 0");

            this._x = x;
            this._y = y;
        }

        public int Scale
        {
            get { return _scale; }
            set 
            { 
                if (value >= 1)
                    _scale = value;
                else
                    throw new ArgumentOutOfRangeException("scale", "Gamefield scale must have a value greater than 1");
            }
        }

        public int[] Offset
        {
            get { return _offset; }
            set
            {
                foreach (int i in value)
                {
                    if (i < 0)
                        throw new ArgumentOutOfRangeException("offset", "Offset must have all values greater than 0");

                    if (value.Length != 4)
                        throw new ArgumentOutOfRangeException("offset", "Offset must have 4 values");
                }
                _offset = value;
            }
        }
    }
}
