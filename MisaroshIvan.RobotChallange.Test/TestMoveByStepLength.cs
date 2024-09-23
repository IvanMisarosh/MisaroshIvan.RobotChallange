using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;
using System;

namespace MisaroshIvan.RobotChallange.Test
{
    [TestClass]
    public class TestMoveByStepLength
    {
        [TestMethod]
        public void TestNoMovementWhenAlreadyOnStation()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position { X = 0, Y = 0 }, Energy = 100 };
            var station = new EnergyStation { Position = new Position { X = 0, Y = 0 } };

            // Act
            var result = MisaroshIvanAlgorithm.MoveByStepLength(robot, station, 5);

            // Assert
            Assert.AreEqual(new Position(0, 0), result);
        }

        [TestMethod]
        public void TestMoveAlongX_WhenOnlyXDiffers()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position { X = 0, Y = 0 }, Energy = 100 };
            var station = new EnergyStation { Position = new Position { X = 12, Y = 0 } };

            // Act
            var result = MisaroshIvanAlgorithm.MoveByStepLength(robot, station, 5);

            // Assert
            Assert.AreEqual(new Position(5, 0), result);
        }

        [TestMethod]
        public void TestMoveAlongY_WhenOnlyYDiffers()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position { X = 0, Y = 0 }, Energy = 100 };
            var station = new EnergyStation { Position = new Position { X = 0, Y = 12 } };

            // Act
            var result = MisaroshIvanAlgorithm.MoveByStepLength(robot, station, 5);

            // Assert
            Assert.AreEqual(new Position(0, 5), result);
        }

        [TestMethod]
        public void TestMoveAlongDiagonal()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position { X = 0, Y = 0 }, Energy = 100 };
            var station = new EnergyStation { Position = new Position { X = 4, Y = 10} };

            // Act
            var result = MisaroshIvanAlgorithm.MoveByStepLength(robot, station, 9);

            // Assert
            Assert.AreEqual(new Position(4, 5), result);
        }

        [TestMethod]
        public void TestMoveIfStationInOneMoveRadius()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position { X = 0, Y = 0 }, Energy = 100 };
            var station = new EnergyStation { Position = new Position { X = 10, Y = 0 } };

            // Act
            var result = MisaroshIvanAlgorithm.MoveByStepLength(robot, station, 6);

            // Assert
            Assert.AreEqual(new Position(10, 0), result); 
        }

        [TestMethod]
        public void TestZeroStepLength_NoMovement()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position { X = 3, Y = 3 }, Energy = 100 };
            var station = new EnergyStation { Position = new Position { X = 14, Y = 10 } };

            // Act
            var result = MisaroshIvanAlgorithm.MoveByStepLength(robot, station, 0);

            // Assert
            Assert.AreEqual(new Position(3, 3), result); // No movement if step length is 0
        }

        [TestMethod]
        public void TestStepLengthGreaterThanDistance_ExactStationPosition()
        {
            // Arrange
            var robot = new Robot.Common.Robot { Position = new Position { X = 0, Y = 0 }, Energy = 100 };
            var station = new EnergyStation { Position = new Position { X = 5, Y = 3 } };

            // Act
            var result = MisaroshIvanAlgorithm.MoveByStepLength(robot, station, 10);

            // Assert
            Assert.AreEqual(new Position(5, 3), result); // Reaches exact station since step length exceeds distance
        }
    }
}
