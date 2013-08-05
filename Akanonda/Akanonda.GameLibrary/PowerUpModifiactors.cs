using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akanonda.GameLibrary
{

        public interface PowerUpModificator
        {
            int getCount();
            void setCount(int counter);
            void reduceCounterBy1();
        }

    [Serializable()]
        public class othersGoSlowModificator : PowerUpModificator
        {
            private int _counter = 0;
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
        public class iGoSlowModificator : PowerUpModificator
        {
            private int _counter = 0;
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
        public class othersGoFastModificator : PowerUpModificator
        {
            private int _counter = 0;
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
        public class iGoFastModificator : PowerUpModificator
        {
            private int _counter = 0;
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
        public class goldenAppleModificator : PowerUpModificator
        {
            private int _counter = 0;
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
        public class redAppleModificator : PowerUpModificator
        {
            private int _counter = 0;
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
        public class iGoThroughWallsModificator : PowerUpModificator
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
        public class rabiesModificator : PowerUpModificator
        {
            private int _counter = 0;
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
