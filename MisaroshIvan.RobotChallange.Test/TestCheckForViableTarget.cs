using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;
using System;
using System.Collections.Generic;

namespace MisaroshIvan.RobotChallange.Test
{
    [TestClass]
    public class TestCheckForViableTarget
    {
        private MisaroshIvanAlgorithm algorithm;
        private Robot.Common.Robot movingRobot;
        private IList<Robot.Common.Robot> robots;

        [TestInitialize]
        public void SetUp()
        {
            algorithm = new MisaroshIvanAlgorithm();

            // Initialize the moving robot with some energy
            movingRobot = new Robot.Common.Robot()
            {
                Position = new Position(10, 10),
                Energy = 100,
                OwnerName = "Player1"
            };

            // Initialize robots list
            robots = new List<Robot.Common.Robot>();
        }

        [TestMethod]
        public void NoEnemyRobots_ReturnsNull()
        {
            // No robots except the moving robot
            robots.Add(movingRobot);

            // Test the CheckForViableTarget function
            var result = algorithm.CheckForViableTarget(movingRobot, robots);

            // Assert that no viable target is found
            Assert.IsNull(result);
        }

        [TestMethod]
        public void EnemyInRangeWithPositiveReward_ReturnsEnemyRobot()
        {
            // Create an enemy robot within attack range
            var enemyRobot = new Robot.Common.Robot()
            {
                Position = new Position(15, 10), // 5 units away
                Energy = 5000,
                OwnerName = "Player2" // Different owner
            };
            robots.Add(movingRobot);
            robots.Add(enemyRobot);

            // Test the CheckForViableTarget function
            var result = algorithm.CheckForViableTarget(movingRobot, robots);

            // Assert that the enemy robot is found
            Assert.IsNotNull(result);
            Assert.AreEqual(enemyRobot, result);
        }

        [TestMethod]
        public void EnemyOutOfRange_ReturnsNull()
        {
            // Create an enemy robot outside the attack range
            var enemyRobot = new Robot.Common.Robot()
            {
                Position = new Position(50, 50), // Far away
                Energy = 50,
                OwnerName = "Player2"
            };
            robots.Add(movingRobot);
            robots.Add(enemyRobot);

            // Test the CheckForViableTarget function
            var result = algorithm.CheckForViableTarget(movingRobot, robots);

            // Assert that no viable target is found
            Assert.IsNull(result);
        }

        [TestMethod]
        public void EnemyInRangeButNoPositiveReward_ReturnsNull()
        {
            // Create an enemy robot within attack range but with low energy
            var enemyRobot = new Robot.Common.Robot()
            {
                Position = new Position(12, 12), // 2 units away
                Energy = 10, // Low energy
                OwnerName = "Player2"
            };
            robots.Add(movingRobot);
            robots.Add(enemyRobot);

            // Test the CheckForViableTarget function
            var result = algorithm.CheckForViableTarget(movingRobot, robots);

            // Assert that no viable target is found since the reward is negative
            Assert.IsNull(result);
        }
    }
}

