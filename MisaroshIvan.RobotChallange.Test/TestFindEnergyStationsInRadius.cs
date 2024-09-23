using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;
using System;
using System.Collections.Generic;

namespace MisaroshIvan.RobotChallange.Test
{
    [TestClass]
    public class TestFindEnergyStationsInRadius
    {
        [TestMethod]
        public void StationsWithinRadius_ReturnsCorrectStations()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position(10, 10), Energy = 100 };
            var station1 = new EnergyStation { Position = new Position(11, 11) }; // Within radius
            var station2 = new EnergyStation { Position = new Position(12, 13) }; // Within radius
            var station3 = new EnergyStation { Position = new Position(20, 20) }; // Outside radius
            var map = new Map { Stations = new List<EnergyStation> { station1, station2, station3 } };

            // Act
            var result = MisaroshIvanAlgorithm.FindEnergyStationsInRadius(robot, map, 5);

            // Assert
            Assert.AreEqual(2, result.Count, "Two stations should be within radius.");
            Assert.IsTrue(result.Contains(station1), "Station1 should be in the result.");
            Assert.IsTrue(result.Contains(station2), "Station2 should be in the result.");
            Assert.IsFalse(result.Contains(station3), "Station3 should not be in the result.");
        }

        [TestMethod]
        public void NoStationsWithinRadius_ReturnsEmptyList()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position(50, 50), Energy = 100 };
            var station1 = new EnergyStation { Position = new Position(10, 10) }; // Outside radius
            var station2 = new EnergyStation { Position = new Position(90, 90) }; // Outside radius
            var map = new Map { Stations = new List<EnergyStation> { station1, station2 } };

            // Act
            var result = MisaroshIvanAlgorithm.FindEnergyStationsInRadius(robot, map, 5);

            // Assert
            Assert.AreEqual(0, result.Count, "No stations should be within radius.");
        }

        [TestMethod]
        public void StationsOnEdgeOfMapWithinRadius_ReturnsCorrectStations()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position(98, 98), Energy = 100 };
            var station1 = new EnergyStation { Position = new Position(99, 99) }; // Within radius
            var station2 = new EnergyStation { Position = new Position(95, 96) }; // Within radius
            var station3 = new EnergyStation { Position = new Position(80, 80) }; // Outside radius
            var map = new Map { Stations = new List<EnergyStation> { station1, station2, station3 } };

            // Act
            var result = MisaroshIvanAlgorithm.FindEnergyStationsInRadius(robot, map, 5);

            // Assert
            Assert.AreEqual(2, result.Count, "Two stations should be within radius.");
            Assert.IsTrue(result.Contains(station1), "Station1 should be in the result.");
            Assert.IsTrue(result.Contains(station2), "Station2 should be in the result.");
            Assert.IsFalse(result.Contains(station3), "Station3 should not be in the result.");
        }

        [TestMethod]
        public void NoStationsInMap_ReturnsEmptyList()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position(10, 10), Energy = 100 };
            var map = new Map { Stations = new List<EnergyStation>() }; // No stations

            // Act
            var result = MisaroshIvanAlgorithm.FindEnergyStationsInRadius(robot, map, 5);

            // Assert
            Assert.AreEqual(0, result.Count, "No stations should be returned when map has no stations.");
        }
    }
}
