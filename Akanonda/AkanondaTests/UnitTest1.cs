using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Akanonda;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkanondaTests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            Akanonda.GameLibrary.iGoThroughWallsModificator iGTW = new Akanonda.GameLibrary.iGoThroughWallsModificator();
            Akanonda.GameLibrary.iGoFastModificator iGF = new Akanonda.GameLibrary.iGoFastModificator();
            List<Akanonda.GameLibrary.PowerUpModificator> temp = new List<Akanonda.GameLibrary.PowerUpModificator>();
            temp.Add(iGF);
            temp.Add(iGTW);
            int test = Akanonda.GameLibrary.PowerUp.checkIfPlayerhas(new Akanonda.GameLibrary.iGoThroughWallsModificator().GetType(), temp);
            test = temp[test].getCount();
            Assert.AreEqual(100, test);
        }


    }
}
