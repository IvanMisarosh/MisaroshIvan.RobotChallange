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
        public int stepLenght { get; set; }
        public Move(int moveCost, int stepLenght)
        {
            this.moveCost = moveCost;
            this.stepLenght = stepLenght;
        }
    }
}
