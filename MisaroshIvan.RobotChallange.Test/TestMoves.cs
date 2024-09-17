using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Robot.Common;

namespace MisaroshIvan.RobotChallange.Test
{
    /// <summary>
    /// Summary description for TestMoves
    /// </summary>
    [TestClass]
    public class TestMoves
    {
        public TestMoves()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestMethod] public void TestMoveCostS() 
        {
            var algorithm = new MisaroshIvanAlgorithm();
            Map map = new Map();
            map.Stations.Add(new EnergyStation() { Energy = 400, Position = new Position(10, 10) });

            Robot.Common.Robot robot = new Robot.Common.Robot() { Energy = 100, Position = new Position(0, 0) };

            int res = MisaroshIvanAlgorithm.MoveCostS(robot, map.Stations[0]);
            Assert.AreEqual(100, res);

            map.Stations.Add(new EnergyStation() { Energy = 100, Position = new Position(5, 5) });

            res = MisaroshIvanAlgorithm.MoveCostS(robot, map.Stations[1]);
            Assert.AreEqual(100, res);

        }

        [TestMethod]
        public void TestMoveCost()
        {
            Position pos1 = new Position(1, 1);
            Position pos2 = new Position(1, 6);

            Assert.AreEqual(25, MisaroshIvanAlgorithm.MoveCost(pos1, pos2));

            pos1 = new Position(1, 1);
            pos2 = new Position(1, 2);

            Assert.AreEqual(1, MisaroshIvanAlgorithm.MoveCost(pos1, pos2));
        }

        [TestMethod]
        public void TestMoveByStepLenght()
        {
            Robot.Common.Robot robot = new Robot.Common.Robot() { Energy = 1000, Position = new Position(0, 0) };
            EnergyStation station = new EnergyStation() { Energy = 100, Position = new Position(10, 10) };

            int stepLenght = 3;
            Position result = MisaroshIvanAlgorithm.MoveByStepLenght(robot, station, stepLenght);
            Assert.AreEqual(3, result.Y);

            stepLenght = 12;
            result = MisaroshIvanAlgorithm.MoveByStepLenght(robot, station, stepLenght);
            Assert.AreEqual(10, result.Y);
        }

        [TestMethod]
        public void TestGetAcceptableStepLenght()
        {
            Robot.Common.Robot robot = new Robot.Common.Robot() { Energy = 25, Position = new Position(0, 0) };
            EnergyStation station = new EnergyStation() { Energy = 100, Position = new Position(0, 5) };

            int result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);
            Assert.AreEqual(5, result);

            robot = new Robot.Common.Robot() { Energy = 1000, Position = new Position(0, 0) };
            station = new EnergyStation() { Energy = 100, Position = new Position(1, 1) };

            result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);
            Assert.AreEqual(2, result);

            robot = new Robot.Common.Robot() { Energy = 100, Position = new Position(0, 0) };
            station = new EnergyStation() { Energy = 100, Position = new Position(10, 10) };

            result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void TestViableTargetMove()
        {
            var algorithm = new MisaroshIvanAlgorithm();

            Robot.Common.Robot robot = new Robot.Common.Robot() { Energy = 1000, Position = new Position(0, 0) };
            Robot.Common.Robot robot1 = new Robot.Common.Robot() { Energy = 2000, Position = new Position(4, 5) };
            Robot.Common.Robot robot2 = new Robot.Common.Robot() { Energy = 10, Position = new Position(1, 1) };

            List<Robot.Common.Robot> robots = new List<Robot.Common.Robot>();
            robots.Add(robot);

            // no viable target
            Position result = algorithm.CheckForViableTarget(robot, robots);
            Assert.AreEqual(null, result);

            robots.Add(robot1);
            robots.Add(robot2);

            // viable target
            result = algorithm.CheckForViableTarget(robot, robots);
            Assert.AreEqual(robot1.Position, result);

            Robot.Common.Robot robot3 = new Robot.Common.Robot() { Energy = 2000, Position = new Position(2, 2) };
            robots.Add(robot3);

            // viable is closer target
            result = algorithm.CheckForViableTarget(robot, robots);
            Assert.AreEqual(robot3.Position, result);

            // not enough energy
            result = algorithm.CheckForViableTarget(robot2, robots);
            Assert.AreEqual(null, result);
            
        }

    }
}
