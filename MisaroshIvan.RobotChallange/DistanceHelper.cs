using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace MisaroshIvan.RobotChallange
{
    public class DistanceHelper
    {
        public static int GetDistance(Position a, Position b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public static int GetSteps(Position a, Position b, int stepLength)
        {
            // Calculate the Manhattan distance
            int distance = Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

            // Calculate the number of steps needed, ensuring we round up
            int steps = (int)Math.Ceiling((double)distance / stepLength);

            return steps;
        }

        public static int GetStepLength(Position a, Position b, int steps)
        {
            // Calculate the Manhattan distance
            int distance = Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

            // Calculate the step length needed
            int stepLength = distance / steps;

            return stepLength;
        }

    }
}
