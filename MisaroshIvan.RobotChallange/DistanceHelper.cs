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

        public static int GetStepLength(Position a, Position b, int steps)
        {
            int distance = Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

            int stepLength = distance / steps;

            return stepLength;
        }

    }
}
