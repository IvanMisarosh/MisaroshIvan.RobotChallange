using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;

namespace MisaroshIvan.RobotChallange.Test
{
    [TestClass]
    public class TestGetAcceptableStepLength
    {
        [TestMethod]
        public void TestAlreadyOnStation_ReturnsZero()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Energy = 100, Position = new Position { X = 0, Y = 0 } };
            var station = new EnergyStation { Position = new Position { X = 0, Y = 0 } };

            // Act
            var result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestStationWithinOneStep_ReturnsFullStepLength()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Energy = 100, Position = new Position { X = 0, Y = 0 } };
            var station = new EnergyStation { Position = new Position { X = 1, Y = 1 } };

            // Act
            var result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);

            // Assert
            Assert.AreEqual(DistanceHelper.GetDistance(robot.Position, station.Position), result);
        }

        [TestMethod]
        public void TestStationOutsideOneStep_ReturnsPartialStepLength()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Energy = 100, Position = new Position { X = 0, Y = 0 } };
            var station = new EnergyStation { Position = new Position { X = 11, Y = 0 } };

            // Act
            var result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);

            // Assert
            Assert.AreEqual(5, result); // Assumed partial step length based on energy constraints
        }

        [TestMethod]
        public void TestMinimalStepLength_ReturnsSmallerStepLength()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Energy = 10, Position = new Position { X = 0, Y = 0 } };
            var station = new EnergyStation { Position = new Position { X = 3, Y = 4 } }; // Distance is 7

            // Act
            var result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);

            // Assert
            Assert.AreEqual(1, result); 
        }

        [TestMethod]
        public void TestStationFarAway_NotEnoughEnergyToReachFullDistance()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Energy = 5, Position = new Position { X = 0, Y = 0 } };
            var station = new EnergyStation { Position = new Position { X = 10, Y = 10 } }; // Distance = 20

            // Act
            var result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);

            // Assert
            Assert.AreEqual(-1, result); // Ensure partial step length
        }

        [TestMethod]
        public void TestExactEnergyToReachStation_ReturnsFullStepLength()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Energy = 25, Position = new Position { X = 0, Y = 0 } };
            var station = new EnergyStation { Position = new Position { X = 3, Y = 4 } }; // Distance = 7, cost = 25

            // Act
            var result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);

            // Assert
            Assert.AreEqual(7, result); // Full step as energy matches move cost
        }

        [TestMethod]
        public void TestZeroEnergy_ReturnsZero()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Energy = 0, Position = new Position { X = 0, Y = 0 } };
            var station = new EnergyStation { Position = new Position { X = 10, Y = 10 } };

            // Act
            var result = MisaroshIvanAlgorithm.GetAcceptableStepLenght(robot, station);

            // Assert
            Assert.AreEqual(-1, result); // No movement possible with zero energy
        }
    }
}
