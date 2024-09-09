using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace MisaroshIvan.RobotChallange
{
    public class MisaroshIvanAlgorithm : IRobotAlgorithm
    {
        public MisaroshIvanAlgorithm()
        {
            Logger.OnLogRound += Logger_OnLogRound;
        }

        private void Logger_OnLogRound(object sender, LogRoundEventArgs e)
        {
            RoundCount++;
        }

        public string Author
        {
            get { return "Misarosh Ivan"; }
        }

        public int RoundCount { get; set; }
        private const int CollectRadius = 2;

        public Position FindNearestFreeStation(Robot.Common.Robot movingRobot, Map map, IList<Robot.Common.Robot> robots)
        {
            EnergyStation nearest = null;
            int minDistance = int.MaxValue;
            foreach (var station in map.Stations)
            {
                if (IsStationFree(station, movingRobot, robots))
                {
                    int d = DistanceHelper.GetDistance(station.Position, movingRobot.Position);
                    if (d < minDistance)
                    {
                        minDistance = d;
                        nearest = station;
                    }
                }
            }
            return nearest == null ? null : nearest.Position;
        }
        public bool IsStationFree(EnergyStation station, Robot.Common.Robot movingRobot,
        IList<Robot.Common.Robot> robots)
        {
            return IsCellFree(station.Position, movingRobot, robots);
        }

        public bool IsCellFree(Position cell, Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
        {
            // Define the radius within which we want to check for other robots
            const int radius = 2;

            // Iterate over all positions within the 2-block radius around the target cell
            for (int x = cell.X - radius; x <= cell.X + radius; x++)
            {
                for (int y = cell.Y - radius; y <= cell.Y + radius; y++)
                {
                    // Make sure we are still within the boundaries of the map
                    if (x >= 0 && y >= 0 && x < 100 && y < 100)  // Assuming the map is 100x100
                    {
                        Position checkPos = new Position(x, y);

                        // Check if any robot occupies this cell within the radius
                        foreach (var robot in robots)
                        {
                            if (robot != movingRobot && robot.Position == checkPos)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // If no robot was found in the 2-block radius, return true
            return true;
        }

        public Position GetAcceptableNewPos(Position stationPos, Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
        {
            /* TODO: modify to account for competitors (to spedup in case someone trying to steal resources)*/
            int distance = DistanceHelper.GetDistance(movingRobot.Position, stationPos);
            int robotEnergy = movingRobot.Energy;

            if (distance > robotEnergy)
            {
                return null;
            }


            /*Account for collecting radius (2)*/
            Position nextP = new Position() { X = movingRobot.Position.X, Y = movingRobot.Position.Y };
            int deltaX = stationPos.X - movingRobot.Position.X;
            int deltaY = stationPos.Y - movingRobot.Position.Y;

            if (deltaX != 0)
            {
                nextP.X += deltaX > 0 ? 1 : -1; // Move right if deltaX > 0, left if deltaX < 0
            }
            
            if (deltaY != 0)
            {
                nextP.Y += deltaY > 0 ? 1 : -1; // Move down if deltaY > 0, up if deltaY < 0
            }

            return nextP;
        }

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            Robot.Common.Robot movingRobot = robots[robotToMoveIndex];
            
            if ((movingRobot.Energy > 500) && (robots.Count < map.Stations.Count) && this.RoundCount < 35)
            {
                return new CreateNewRobotCommand();
            }

            Position stationPosition = FindNearestFreeStation(robots[robotToMoveIndex], map, robots);
        
            if (stationPosition == null)
                return null;
            if (stationPosition == movingRobot.Position)
                return new CollectEnergyCommand();
            else
            {
                Position newPos = GetAcceptableNewPos(stationPosition, movingRobot, robots);
                newPos = GetAcceptableNewPos(stationPosition, movingRobot, robots) == null ? movingRobot.Position : newPos;
                return new MoveCommand() { NewPosition = newPos };
            }
        }
    }
}
