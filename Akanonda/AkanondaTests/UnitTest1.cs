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
            Akanonda.GameLibrary.iGoThroughWallsModifier iGTW = new Akanonda.GameLibrary.iGoThroughWallsModifier();
            Akanonda.GameLibrary.iGoFastModifier iGF = new Akanonda.GameLibrary.iGoFastModifier();
            List<Akanonda.GameLibrary.PowerUpModifier> temp = new List<Akanonda.GameLibrary.PowerUpModifier>();
            temp.Add(iGF);
            temp.Add(iGTW);
            //int test = Akanonda.GameLibrary.PowerUp.checkIfPlayerhas(new Akanonda.GameLibrary.iGoThroughWallsModificator().GetType(), temp);
            //test = temp[test].getCount();
            //Assert.AreEqual(100, test);
        }


    }
}
