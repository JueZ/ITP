using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{

    public enum PowerUpModifierKind
    {
        iGoSlowModifier,
        iGoFastModifier,
        goldenAppleModifier,
        redAppleModifier,
        iGoThroughWallsModifier,
        rabiesModifier,
        makePlayersBigModifier,
        changeColorModifier,
        iGoDiagonalModifier,
    }
        
        public interface PowerUpModifier
        {
            int getCount();
            void setCount(int counter);
            void reduceCounterBy1();
            PowerUpModifierKind getType();
        }

    [Serializable()]
        public class iGoSlowModifier : PowerUpModifier
        {
            private int _counter = 100;
            public int getCount()
            {
                return _counter;
            }
            public void setCount(int counter)
            {
                _counter = counter;
            }
            public void reduceCounterBy1()
            {
                if (_counter > 0)
                _counter--;
            }
            public PowerUpModifierKind getType()
            {
                return PowerUpModifierKind.iGoSlowModifier;
            }
        }

    [Serializable()]
        public class iGoFastModifier : PowerUpModifier
        {
            private int _counter = 100;
            public int getCount()
            {
                return _counter;
            }
            public void setCount(int counter)
            {
                _counter = counter;
            }
            public void reduceCounterBy1()
            {
                if (_counter > 0)
                _counter--;
            }
            public PowerUpModifierKind getType()
            {
                return PowerUpModifierKind.iGoFastModifier;
            }
        }

    [Serializable()]
        public class goldenAppleModifier : PowerUpModifier
        {
            private int _counter = 20;
            public int getCount()
            {
                return _counter;
            }
            public void setCount(int counter)
            {
                _counter = counter;
            }
            public void reduceCounterBy1()
            {
                if (_counter > 0)
                _counter--;
            }
            public PowerUpModifierKind getType()
            {
                return PowerUpModifierKind.goldenAppleModifier;
            }
        }

    [Serializable()]
        public class redAppleModifier : PowerUpModifier
        {
            private int _counter = 20;
            public int getCount()
            {
                return _counter;
            }
            public void setCount(int counter)
            {
                _counter = counter;
            }
            public void reduceCounterBy1()
            {
                if (_counter > 0)
                _counter--;
            }
            public PowerUpModifierKind getType()
            {
                return PowerUpModifierKind.redAppleModifier;
            }
        }

    [Serializable()]
        public class iGoThroughWallsModifier : PowerUpModifier
        {
            private int _counter = 100;
            public int getCount()
            {
                return _counter;
            }
            public void setCount(int counter)
            {
                _counter = counter;
            }
            public void reduceCounterBy1()
            {
                if (_counter > 0)
                _counter--;
            }
            public PowerUpModifierKind getType()
            {
                return PowerUpModifierKind.iGoThroughWallsModifier;
            }
        }

    [Serializable()]
        public class rabiesModifier : PowerUpModifier
        {
            private int _counter = 50;
            public int getCount()
            {
                return _counter;
            }
            public void setCount(int counter)
            {
                _counter = counter;
            }
            public void reduceCounterBy1()
            {
                if (_counter > 0)
                _counter--;
            }
            public PowerUpModifierKind getType()
            {
                return PowerUpModifierKind.rabiesModifier;
            }
        }

    [Serializable()]
    public class makePlayersBigModifier : PowerUpModifier
    {
        private int _counter = 100;

        public int getCount()
        {
            return _counter;
        }
        public void setCount(int counter)
        {
            _counter = counter;
        }
        public void reduceCounterBy1()
        {
            if (_counter > 0)
                _counter--;
        }
        public PowerUpModifierKind getType()
        {
            return PowerUpModifierKind.makePlayersBigModifier;
        }
    }

    [Serializable()]
    public class changeColorModifier : PowerUpModifier
    {
        private int _counter = 50;
        public int getCount()
        {
            return _counter;
        }
        public void setCount(int counter)
        {
            _counter = counter;
        }
        public void reduceCounterBy1()
        {
            if (_counter > 0)
                _counter--;
        }
        public PowerUpModifierKind getType()
        {
            return PowerUpModifierKind.changeColorModifier;
        }
    }
    [Serializable()]
    public class iGoDiagonalModifier : PowerUpModifier
    {
        private int _counter = 100;
        public int getCount()
        {
            return _counter;
        }
        public void setCount(int counter)
        {
            _counter = counter;
        }
        public void reduceCounterBy1()
        {
            if (_counter > 0)
                _counter--;
        }
        public PowerUpModifierKind getType()
        {
            return PowerUpModifierKind.iGoDiagonalModifier;
        }
    }
    
}
