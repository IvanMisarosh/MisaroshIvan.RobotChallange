using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Robot.Common;
using System.Collections.Generic;

namespace MisaroshIvan.RobotChallange.Test
{
    [TestClass]
    public class TestFindClosestStationToTake
    {
        private MisaroshIvanAlgorithm algorithm;
        private Map map;
        private IList<Robot.Common.Robot> robots;

        [TestInitialize]
        public void SetUp()
        {
            // Initialize algorithm
            algorithm = new MisaroshIvanAlgorithm();

            // Initialize map with energy stations
            map = new Map();
            map.Stations = new List<EnergyStation>
            {
                new EnergyStation { Position = new Position(10, 10) },
                new EnergyStation { Position = new Position(30, 30) },
                new EnergyStation { Position = new Position(50, 50) }
            };

            // Initialize robots list
            robots = new List<Robot.Common.Robot>();
        }

        [TestMethod]
        public void FindClosestStationToTake_ReturnsClosestStation()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position(15, 15) };

            // Act
            var closestStation = algorithm.FindClosestStationToTake(robot, map);

            // Assert
            Assert.IsNotNull(closestStation, "The method should return a station.");
            Assert.AreEqual(10, closestStation.Position.X, "Closest station X coordinate is incorrect.");
            Assert.AreEqual(10, closestStation.Position.Y, "Closest station Y coordinate is incorrect.");
        }

        [TestMethod]
        public void FindClosestStationToTake_MultipleStations_ReturnsCorrectStation()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position(45, 45) };

            // Act
            var closestStation = algorithm.FindClosestStationToTake(robot, map);

            // Assert
            Assert.IsNotNull(closestStation, "The method should return a station.");
            Assert.AreEqual(50, closestStation.Position.X, "Closest station X coordinate is incorrect.");
            Assert.AreEqual(50, closestStation.Position.Y, "Closest station Y coordinate is incorrect.");
        }

        [TestMethod]
        public void FindClosestStationToTake_NoStations_ReturnsNull()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position(15, 15) };
            map.Stations = new List<EnergyStation>(); // Empty list of stations

            // Act
            var closestStation = algorithm.FindClosestStationToTake(robot, map);

            // Assert
            Assert.IsNull(closestStation, "The method should return null if no stations are present.");
        }

        [TestMethod]
        public void FindClosestStationToTake_RobotOnStation_ReturnsSameStation()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position(10, 10) }; // On the same position as the first station

            // Act
            var closestStation = algorithm.FindClosestStationToTake(robot, map);

            // Assert
            Assert.IsNotNull(closestStation, "The method should return the station.");
            Assert.AreEqual(10, closestStation.Position.X, "Station X coordinate is incorrect.");
            Assert.AreEqual(10, closestStation.Position.Y, "Station Y coordinate is incorrect.");
        }
    }

}
