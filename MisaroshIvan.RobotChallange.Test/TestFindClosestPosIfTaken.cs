using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;
using System;
using System.Collections.Generic;

namespace MisaroshIvan.RobotChallange.Test
{
    [TestClass]
    public class TestFindClosestPosIfTaken
    {
        [TestMethod]
        public void FindClosestPosIfTaken_TargetPositionOccupied_FindsNearestFreeCell()
        {
            // Arrange
            var algorithm = new MisaroshIvanAlgorithm();
            var movingRobot = new Robot.Common.Robot
            {
                Position = new Position(10, 10),
                Energy = 100,
                OwnerName = "RobotA"
            };

            // Other robot occupies the target position
            IList<Robot.Common.Robot> robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot { Position = new Position(15, 15), OwnerName = "RobotB" }
            };

            Position targetPosition = new Position(15, 15);

            // Act
            Position newPosition = MisaroshIvanAlgorithm.FindClosestPosIfTaken(movingRobot, robots, targetPosition);

            // Assert
            Assert.IsNotNull(newPosition);
            Assert.AreNotEqual(targetPosition, newPosition, "Expected new position since target was taken.");
        }

        [TestMethod]
        public void FindClosestPosIfTaken_TargetPositionFree_ReturnsTargetPosition()
        {
            // Arrange
            var algorithm = new MisaroshIvanAlgorithm();
            var movingRobot = new Robot.Common.Robot
            {
                Position = new Position(10, 10),
                Energy = 100,
                OwnerName = "RobotA"
            };

            // No robot occupies the target position
            IList<Robot.Common.Robot> robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot { Position = new Position(12, 12), OwnerName = "RobotB" }
            };

            Position targetPosition = new Position(15, 15);

            // Act
            Position newPosition = MisaroshIvanAlgorithm.FindClosestPosIfTaken(movingRobot, robots, targetPosition);


            Assert.AreEqual(targetPosition, newPosition, "Expected target position as it was free.");
        }

        [TestMethod]
        public void FindClosestPosIfTaken_OwnRobotOccupiesTargetPosition_ReturnsSamePosition()
        {
            // Arrange
            var algorithm = new MisaroshIvanAlgorithm();
            var movingRobot = new Robot.Common.Robot
            {
                Position = new Position(10, 10),
                Energy = 100,
                OwnerName = "RobotA"
            };

            // Robot with the same owner occupies the target position
            IList<Robot.Common.Robot> robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot { Position = new Position(15, 15), OwnerName = "RobotA" } // Same owner
            };

            Position targetPosition = new Position(15, 15);

            // Act
            Position newPosition = MisaroshIvanAlgorithm.FindClosestPosIfTaken(movingRobot, robots, targetPosition);

            // Assert
            Assert.AreEqual(movingRobot.Position, newPosition, "Expected no movement if own robot occupies target position.");
        }
    }
}
