using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{
        public interface PowerUpModifier
        {
            int getCount();
            void setCount(int counter);
            void reduceCounterBy1();
        }

        

    [Serializable()]
        public class othersGoSlowModifier : PowerUpModifier
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
                if(_counter > 0)
                _counter--;
            }
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
        }

    [Serializable()]
        public class othersGoFastModifier : PowerUpModifier
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
    }
    
}
