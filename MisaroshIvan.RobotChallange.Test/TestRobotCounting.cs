using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Robot.Common;

namespace MisaroshIvan.RobotChallange.Test
{
    /// <summary>
    /// Summary description for TestRobotCounting
    /// </summary>
    [TestClass]
    public class TestRobotCounting
    {
        public TestRobotCounting()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        [TestMethod]
        public void TestAlgoritmRobotCounting()
        { 
            MisaroshIvanAlgorithm algorithm = new MisaroshIvanAlgorithm();
            List<Robot.Common.Robot> robots = new List<Robot.Common.Robot>();
            Robot.Common.Robot robot = new Robot.Common.Robot() { Energy = 1000, Position = new Robot.Common.Position(0, 0) };
            robots.Add(robot);
            Map map = new Map();
            map.Stations.Add(new EnergyStation() { Energy = 100, Position = new Position(10, 10) });
            algorithm.DoStep(robots, 0, map);
            robots.Add(new Robot.Common.Robot() { Energy = 1000, Position = new Robot.Common.Position(0, 0) });
            algorithm.DoStep(robots, 1, map);
            algorithm.DoStep(robots, 0, map);
            algorithm.DoStep(robots, 1, map);
            Assert.AreEqual(algorithm.myRobots.Count, 2);

        }
    }
}
