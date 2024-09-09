using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Robot.Common;

namespace MisaroshIvan.RobotChallange.Test
{
    [TestClass]
    public class TestDistanceHelper
    {
        [TestMethod]
        public void TestDistance()
        {
            var a = new Robot.Common.Position() { X = 0, Y = 0 };
            var b = new Robot.Common.Position() { X = 0, Y = 0 };
            var result = DistanceHelper.GetDistance(a, b);
            Assert.AreEqual(0, result);
            
        }
    }
}
