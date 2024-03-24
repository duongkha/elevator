using ElevatorSystem.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Model
{
    public class Request
    {
        public int RequestedFloor{ get; set; }
        public Location Location { get; set; }
        public Direction Direction { get; set; }
        public Request(int requestedFloor, Location location, Direction direction)
        {
            this.RequestedFloor = requestedFloor;   
            this.Location = location;
            this.Direction = direction;
        }
        public override string ToString()
        {
            return string.Format("requested floor {0}, location {1}, direction {2}", RequestedFloor + 1, Location, Direction);
        }
    }
}
