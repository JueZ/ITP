using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{
    public struct Field
    {
        private int _x;
        private int _y;

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
    }
}
