using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisaroshIvan.RobotChallange
{
    public class Move
    {
        public int moveCost { get; set; }
        public int stepLength { get; set; }
        public int potentialReward { get; set; }
        public Move(int moveCost, int stepLength, int potentialReward)
        {
            this.moveCost = moveCost;
            this.stepLength = stepLength;
            this.potentialReward = potentialReward;
        }
    }
}
