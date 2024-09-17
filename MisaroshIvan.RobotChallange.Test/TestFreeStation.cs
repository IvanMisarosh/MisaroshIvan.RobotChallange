using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Robot.Common;

namespace MisaroshIvan.RobotChallange.Test
{
    [TestClass]
    public class TestFreeStation
    {
        public TestFreeStation()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestMethod]
        public void TestFreeStationMove()
        {
            Map map = new Map();
            map.Stations.Add(new EnergyStation() { Energy = 100, Position = new Position(10, 10) });

            Robot.Common.Robot robot = new Robot.Common.Robot() { Energy = 1000, Position = new Position(0, 0) };
            Robot.Common.Robot robot1 = new Robot.Common.Robot() { Energy = 1000, Position = new Position(4, 5) };
            List<Robot.Common.Robot> robots = new List<Robot.Common.Robot>();
            robots.Add(robot1);
            robots.Add(robot);

            MisaroshIvanAlgorithm algorithm = new MisaroshIvanAlgorithm();
            bool res = algorithm.IsCellFree(map.Stations[0].Position, robot, robots);
            Assert.AreEqual(true, res);
            
        }
    }
}
