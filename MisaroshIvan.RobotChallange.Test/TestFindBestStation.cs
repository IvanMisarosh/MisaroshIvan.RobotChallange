using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;
using System.Collections.Generic;

namespace MisaroshIvan.RobotChallange.Test
{
    [TestClass]
    public class TestFindBestStation
    {
        [TestMethod]
        public void Test_NoOtherRobots_StationFound()
        {
            var algorithm = new MisaroshIvanAlgorithm();
            var movingRobot = new Robot.Common.Robot()
            {
                Position = new Position(10, 10),
                Energy = 100
            };

            var map = new Map();
            map.Stations.Add(new EnergyStation { Position = new Position(12, 12) });
            map.Stations.Add(new EnergyStation { Position = new Position(15, 15) });
            map.Stations.Add(new EnergyStation { Position = new Position(25, 25) });

            IList<Robot.Common.Robot> robots = new List<Robot.Common.Robot>();

            EnergyStation bestStation = algorithm.FindBestStation(robots, movingRobot, map);

            Assert.IsNotNull(bestStation);
            Assert.AreEqual(12, bestStation.Position.X);
            Assert.AreEqual(12, bestStation.Position.Y);
        }

        [TestMethod]
        public void Test_SomeStationOccupied()
        {
            var algorithm = new MisaroshIvanAlgorithm();
            var movingRobot = new Robot.Common.Robot()
            {
                Position = new Position(10, 10),
                Energy = 100
            };

            var map = new Map();
            map.Stations.Add(new EnergyStation { Position = new Position(12, 12) });
            map.Stations.Add(new EnergyStation { Position = new Position(15, 15) });

            IList<Robot.Common.Robot> robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot { Position = new Position(12, 12) }
            };

            EnergyStation bestStation = algorithm.FindBestStation(robots, movingRobot, map);


            Assert.AreEqual(bestStation, map.Stations[1]); 
        }

        [TestMethod]
        public void Test_ManyFreeStations()
        {
            var algorithm = new MisaroshIvanAlgorithm();
            var movingRobot = new Robot.Common.Robot()
            {
                Position = new Position(10, 10),
                Energy = 100
            };

            var map = new Map();
            map.Stations.Add(new EnergyStation { Position = new Position(12, 12) }); // Occupied
            map.Stations.Add(new EnergyStation { Position = new Position(15, 15) }); // Free
            map.Stations.Add(new EnergyStation { Position = new Position(25, 25) }); // Free

            IList<Robot.Common.Robot> robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot { Position = new Position(12, 12) }
            };

            EnergyStation bestStation = algorithm.FindBestStation(robots, movingRobot, map);

            Assert.IsNotNull(bestStation);
            Assert.AreEqual(15, bestStation.Position.X);
            Assert.AreEqual(15, bestStation.Position.Y); // Returns the next nearest free station
        }

        [TestMethod]
        public void Test_EnemyInStationRadius()
        {
            var algorithm = new MisaroshIvanAlgorithm();
            var movingRobot = new Robot.Common.Robot()
            {
                Position = new Position(10, 10),
                Energy = 100
            };

            var map = new Map();
            map.Stations.Add(new EnergyStation { Position = new Position(12, 12) }); // Enemy in radius
            map.Stations.Add(new EnergyStation { Position = new Position(15, 15) }); // Enemy in radius
            map.Stations.Add(new EnergyStation { Position = new Position(25, 25) }); // Free

            IList<Robot.Common.Robot> robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot { Position = new Position(14, 14) }
            };

            EnergyStation bestStation = algorithm.FindBestStation(robots, movingRobot, map);

            Assert.IsNotNull(bestStation);
            Assert.AreEqual(25, bestStation.Position.X);
            Assert.AreEqual(25, bestStation.Position.Y); // Returns the next nearest free station

        }
    }
}
