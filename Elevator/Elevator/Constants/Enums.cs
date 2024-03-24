using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Constants
{
    public enum Direction
    {
        UP,
        DOWN,
        IDLE
    }
    public enum Location
    {
        INSIDE,
        OUTSIDE
    }
    public enum ElevatorStatus
    {
        Stopped,
        DoorOpened,
        DoorClosed,
        GoingUp,
        GoingDown
    }
}
