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

        public List<Robot.Common.Robot> myRobots = new List<Robot.Common.Robot>();

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

        private const int CollectRadius = 2;

        public EnergyStation FindNearestFreeStation(Robot.Common.Robot movingRobot, Map map, IList<Robot.Common.Robot> robots)
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
            return nearest == null ? null : nearest;
        }
        public bool IsStationFree(EnergyStation station, Robot.Common.Robot movingRobot,
        IList<Robot.Common.Robot> robots)
        {
            return IsCellFree(station.Position, movingRobot, robots);
        }

        public bool IsCellFree(Position cell, Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
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

        public IList<Robot.Common.Robot> CheckCellsInRadius(Position pos, int radius, IList<Robot.Common.Robot> robots)
        {
            IList<Robot.Common.Robot> robotsInRadius = new List<Robot.Common.Robot>();
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    int newX = pos.X + dx;
                    int newY = pos.Y + dy;

                    // Check if the new position is within the grid bounds
                    if (newX >= 0 && newY >= 0 && newX < 100 && newY < 100)
                    {
                        foreach (var robot in robots)
                        {
                            if (robot.Position.X == newX && robot.Position.Y == newY)
                            {
                                robotsInRadius.Add(robot);
                            }
                        }
                    }
                }
            }
            return robotsInRadius;
        }

        public bool IsMyRobot(Robot.Common.Robot robot)
        {
            foreach (var r in myRobots)
            {
                if (r == robot)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AnyAllyRobotInList(IList<Robot.Common.Robot> robots, Robot.Common.Robot movingRobot)
        {
            foreach (var robot in robots)
            {
                if (movingRobot != robot && IsMyRobot(robot))
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

        public static int MoveCostS(Robot.Common.Robot robot, EnergyStation station, int maxSteps = 2)
        {
            int currentSteps = 1;
            int currentCost = MoveCost(robot.Position, station.Position);

            while (currentSteps <= maxSteps)
            {

                int stepLeght = DistanceHelper.GetStepLength(robot.Position, station.Position, currentSteps);
                int newCost = stepLeght * stepLeght;
                if (newCost <= robot.Energy)
                {
                    return newCost;
                }
                currentSteps++;
            }
            return currentCost;
        }

        public Position CheckForViableTarget(Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
        {
            robots = robots.Where(r => MoveCost(movingRobot.Position, r.Position) <= movingRobot.Energy).ToList();
            int currentReward = 0;
            Position target = null;
            foreach (var robot in robots)
            {
                if (robot != movingRobot && !IsMyRobot(robot))
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
                        target = robot.Position;
                    }
                }
            }
            return target;
        }

        public static int GetAcceptableStepLenght(Robot.Common.Robot movingRobot, EnergyStation station)
        {
            int stepLenght = DistanceHelper.GetDistance(movingRobot.Position, station.Position);
            int stepsNum = 1;
            int moveCost = MoveCost(movingRobot.Position, station.Position);
            while (stepsNum * moveCost > movingRobot.Energy)
            {
                stepsNum++;
                stepLenght = DistanceHelper.GetStepLength(movingRobot.Position, station.Position, stepsNum); 
                moveCost = stepLenght * stepLenght;
            }
            return stepLenght;

        }

        public static Position MoveByStepLenght(Robot.Common.Robot movingRobot, EnergyStation station, int stepLenght)
        {
            int dx = station.Position.X - movingRobot.Position.X;
            int dy = station.Position.Y - movingRobot.Position.Y;
            int absDx = Math.Abs(dx);
            int absDy = Math.Abs(dy);
            int newX = movingRobot.Position.X;
            int newY = movingRobot.Position.Y;
            if (absDx > absDy)
            {
                stepLenght = Math.Min(stepLenght, absDx);
                if (dx > 0)
                {
                    newX += stepLenght;
                }
                else
                {
                    newX -= stepLenght;
                }
            }
            else
            {
                stepLenght = Math.Min(stepLenght, absDy);
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

        public static Move FindOptimalMove(Robot.Common.Robot movingRobot, EnergyStation station, int maxSteps = 2)
        {
            int currentSteps = 1;
            int currentCost = MoveCost(movingRobot.Position, station.Position);
            int currenStepLenght = DistanceHelper.GetStepLength(movingRobot.Position, station.Position, currentSteps);
            float optimalRewardImprovement = 1.5f;

            while (currentSteps <= maxSteps)
            {
                int newStepLenght = DistanceHelper.GetStepLength(movingRobot.Position, station.Position, currentSteps);
                int newCost = newStepLenght * newStepLenght;
                if (newCost > movingRobot.Energy)
                {
                    currentSteps++;
                    continue;
                }
                if ((newCost / currentCost) >= optimalRewardImprovement)
                {
                    currentCost = newCost;
                    currenStepLenght = newStepLenght;
                }
                currentSteps++;
            }
            return new Move(currentCost, currenStepLenght);
        }

        public EnergyStation FindBestStation(IList<Robot.Common.Robot> robots, Robot.Common.Robot movingRobot, Map map)
        {
            IList<EnergyStation> stations = map.Stations.Where(station => MoveCostS(movingRobot, station) <= movingRobot.Energy).ToList();
            //IList<EnergyStation> stations = map.Stations.Where(station => MoveCost(movingRobot.Position, station.Position) <= movingRobot.Energy).ToList();
            int currentReward = 0;
            EnergyStation target = null;
            foreach (var station in stations)
            {
                if (AnyAllyRobotInList(CheckCellsInRadius(station.Position, 2, robots), movingRobot))
                {
                    continue;
                }
                int moveCost = MoveCost(movingRobot.Position, station.Position);
                if (moveCost > movingRobot.Energy)
                {
                    continue;
                }
                int newReward = (int)(station.Energy - moveCost);
                if (currentReward < newReward)
                {
                    currentReward = newReward;
                    target = station;
                }
            }
            return target;
        }

        public Position CalculateMove(IList<Robot.Common.Robot> robots, Robot.Common.Robot movingRobot, Map map)
        {
            Position newPos = null;
            EnergyStation bestStation = FindBestStation(robots, movingRobot, map);
            if (bestStation == null)
            {
                return movingRobot.Position;
            }

            if ((DistanceHelper.GetDistance(movingRobot.Position, bestStation.Position) <= CollectRadius && bestStation.Energy < 30))
            {
                newPos = CheckForViableTarget(movingRobot, robots);
                if (newPos != null)
                {
                    return newPos;
                }
                //TODO: подумати як обрати іншу станцію

            }

            Move move = FindOptimalMove(movingRobot, bestStation);
            //TODO: подумати як обрати іншу станцію

            // TODO: find optimal distance to move
            if (move.moveCost > movingRobot.Energy)
            {
                //return MoveByStepLenght(movingRobot, bestStation, GetAcceptableStepLenght(movingRobot, bestStation));
                return MoveByStepLenght(movingRobot, bestStation, move.stepLenght);
            }

            if (move.moveCost < movingRobot.Energy)
            {
                return bestStation.Position;
            }

            return newPos;
        }


        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            Robot.Common.Robot movingRobot = robots[robotToMoveIndex];
            updateRobots(movingRobot);
            
            if ((movingRobot.Energy > 500) && (robots.Count < map.Stations.Count) && this.RoundCount < 35)
            {
                return new CreateNewRobotCommand();
            }

            Position newPos = CalculateMove(robots, movingRobot, map);
        

            return new MoveCommand() { NewPosition = newPos};
            
        }

        public void updateRobots(Robot.Common.Robot robot)
        {
            foreach (var r in myRobots)
            {
                if (r == robot)
                {
                    return;
                }
            }
            myRobots.Add(robot);
        }
    }
}
