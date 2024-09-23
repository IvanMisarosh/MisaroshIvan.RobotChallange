using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string Description => throw new NotImplementedException();

        public bool IsStationFree(EnergyStation station, Robot.Common.Robot movingRobot,
        IList<Robot.Common.Robot> robots)
        {
            int radius = 2;
            for(int x = station.Position.X - radius; x <= station.Position.X + radius; x++)
            {
                for(int y = station.Position.Y - radius; y <= station.Position.Y + radius; y++)
                {
                    if (x >= 0 && y >= 0 && x < 100 && y < 100)
                    {
                        foreach (var robot in robots)
                        {
                            if (robot.Position.X == x && robot.Position.Y == y)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static bool IsCellFree(Position cell, Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
        {
            foreach (var robot in robots)
            {
                if (robot != movingRobot)
                {
                    if (robot.Position == cell)
                        return false;
                }
            }

            return true;
        }


        public bool IsMyRobot(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots)
        {
            foreach (var r in robots)
            {
                if (r.OwnerName == robot.OwnerName && r != robot)
                {
                    return true;
                }
            }
            return false;
        }


        public static int MoveCost(Position pos, Position newPos)
        {
            return (int)Math.Abs(Math.Pow((newPos.X - pos.X), 2) + Math.Abs(Math.Pow((newPos.Y - pos.Y), 2)));
        }

        public Robot.Common.Robot CheckForViableTarget(Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
        {
            robots = robots.Where(r => MoveCost(movingRobot.Position, r.Position) <= movingRobot.Energy).ToList();
            int currentReward = 0;
            Robot.Common.Robot target = null;
            foreach (var robot in robots)
            {
                if (robot != movingRobot && !IsMyRobot(robot, robots))
                {
                    int moveCost = MoveCost(movingRobot.Position, robot.Position) + 30;

                    if (moveCost > movingRobot.Energy)
                    {
                        continue;
                    }

                    int newReward = (int)((robot.Energy * 0.05) - moveCost);
                    if (currentReward < newReward)
                    {
                        currentReward = newReward;
                        target = robot;
                    }
                }
            }
            return target;
        }

        public static int GetAcceptableStepLenght(Robot.Common.Robot movingRobot, EnergyStation station)
        {
            int stepsNum = 1;

            int stepLenght = DistanceHelper.GetDistance(movingRobot.Position, station.Position);
            if (stepLenght == 0)
            {
                return 0;
            }

            if (stepLenght > movingRobot.Energy)
            {
                return -1;
            }

            int moveCost = MoveCost(movingRobot.Position, station.Position);

            while (stepsNum * moveCost > movingRobot.Energy)
            {
                stepsNum++;
                stepLenght = DistanceHelper.GetStepLength(movingRobot.Position, station.Position, stepsNum); 
                moveCost = stepLenght * stepLenght;
            }
            return stepLenght;

        }

        public static Position MoveByStepLength(Robot.Common.Robot movingRobot, EnergyStation station, int stepLenght)
        {
            int dx = station.Position.X - movingRobot.Position.X;
            int dy = station.Position.Y - movingRobot.Position.Y;
            int absDx = Math.Abs(dx);
            int absDy = Math.Abs(dy);
            int newX = movingRobot.Position.X;
            int newY = movingRobot.Position.Y;

            if (absDx == 0 && absDy == 0)
            {
                return new Position(newX, newY);
            }

            if (MoveCost(movingRobot.Position, station.Position) <= movingRobot.Energy)
            {
                return station.Position;
            }

            if (stepLenght * stepLenght > movingRobot.Energy)
            {
                return new Position(newX, newY);
            }

            int remainingLenght = stepLenght;
            if (absDx > 0)
            {
                stepLenght = Math.Min(stepLenght, absDx);
                remainingLenght -= stepLenght;
                if (dx > 0)
                {
                    newX += stepLenght;
                }
                else
                {
                    newX -= stepLenght;
                }
            }

            if (absDy > 0 && remainingLenght != 0)
            {
                stepLenght = Math.Min(remainingLenght, absDy);
                if (dy > 0)
                {
                    newY += stepLenght;
                }
                else
                {
                    newY -= stepLenght;
                }
            }
            return new Position(newX, newY);
        }

        public static IList<EnergyStation> FindEnergyStationsInRadius(Robot.Common.Robot movingRobot, Map map, int radius = 2)
        {
            IList<EnergyStation> stations = new List<EnergyStation>();
            foreach (var station in map.Stations)
            {
                if (DistanceHelper.GetDistance(movingRobot.Position, station.Position) <= radius)
                {
                    stations.Add(station);
                }
            }
            return stations;
        }
        public EnergyStation FindBestStation(IList<Robot.Common.Robot> robots, Robot.Common.Robot movingRobot, Map map)
        {
            EnergyStation bestStation = null;
            int bestStationCost = int.MaxValue;
            foreach (var station in map.Stations)
            {
                if (IsStationFree(station, movingRobot, robots))
                {
                    int stationCost = DistanceHelper.GetDistance(station.Position, movingRobot.Position);
                    if (stationCost < bestStationCost)
                    {
                        bestStationCost = stationCost;
                        bestStation = station;
                    }

                }
            }
            return bestStation;
        }

        public bool ShouldCollectEnergyS(Robot.Common.Robot movingRobot, Map map, int radius = 0)
        {
            foreach (var station in map.Stations)
            {
                if (DistanceHelper.GetDistance(movingRobot.Position, station.Position) == radius)
                {
                    return true;
                }
            }
            return false;
        }


        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            Robot.Common.Robot movingRobot = robots[robotToMoveIndex];
            

            if (ShouldCollectEnergyS(movingRobot, map))
            {
                if (ShouldCreateNewRobot(movingRobot, robots, map))
                {
                    return new CreateNewRobotCommand() { NewRobotEnergy = 100 };
                }

                return new CollectEnergyCommand();
            }

            EnergyStation bestStation = FindBestStation(robots, movingRobot, map);
            Robot.Common.Robot bestEnemyToAttack = CheckForViableTarget(movingRobot, robots);

            if (bestStation == null && bestEnemyToAttack != null)
            {
                return new MoveCommand() { NewPosition = bestEnemyToAttack.Position };
            }

            if (bestStation != null)
            {
                int stepLenght = GetAcceptableStepLenght(movingRobot, bestStation);
               
                if(stepLenght == 0)
                {
                    return new CollectEnergyCommand();
                }
                else if (stepLenght != -1)
                {
                    return new MoveCommand() { NewPosition = MoveByStepLength(movingRobot, bestStation, stepLenght) };
                }
            }

            var closestStation = FindClosestStationToTake(movingRobot, map);
            int step = GetAcceptableStepLenght(movingRobot, closestStation);
            Position newPosition = MoveByStepLength(movingRobot, closestStation, step);
            newPosition = FindClosestPosIfTaken(movingRobot, robots, newPosition);
            return new MoveCommand() { NewPosition = newPosition};
        }

        public static Position FindClosestPosIfTaken(Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots, Position position)
        {
            foreach (var robot in robots)
            {
                if (robot != movingRobot && robot.Position == position)
                {
                    if(robot.OwnerName == movingRobot.OwnerName)
                    {
                        if(DistanceHelper.GetDistance(movingRobot.Position, position) < 2)
                        {
                            return new Position(movingRobot.Position.X, movingRobot.Position.Y);
                        }
                    }
                    return FindNearestFreeCellInRadius(movingRobot, robots);
                }
            }
            return position;
        }

        public static Position FindNearestFreeCellInRadius(Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots, int radius = 2)
        {
            int minDistance = int.MaxValue;
            Position nearest = null;
            for (int i = movingRobot.Position.X - radius; i <= movingRobot.Position.X + radius; i++)
            {
                for (int j = movingRobot.Position.Y - radius; j <= movingRobot.Position.Y + radius; j++)
                {
                    if (i >= 0 && j >= 0 && i < 100 && j < 100)
                    {
                        Position pos = new Position(i, j);
                        if (IsCellFree(pos, movingRobot, robots))
                        {
                            int d = DistanceHelper.GetDistance(movingRobot.Position, pos);
                            if (d < minDistance)
                            {
                                minDistance = d;
                                nearest = pos;
                            }
                        }
                    }
                }
            }
            return nearest;
        }

        public EnergyStation FindClosestStationToTake(Robot.Common.Robot robot, Map map)
        {
            EnergyStation closestStation = null;
            int minDistance = int.MaxValue;
            foreach (var station in map.Stations)
            {
                int distance = DistanceHelper.GetDistance(robot.Position, station.Position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestStation = station;
                }
            }
            return closestStation;
        }

        private bool ShouldCreateNewRobot(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, Map map)
        {
            return robot.Energy > 300 && robots.Count < map.Stations.Count * 2 && RoundCount < 30;
            //return robot.Energy > 300 && RoundCount < 30;
        }

    }
}
