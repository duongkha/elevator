using ElevatorSystem.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Model
{
    public class Elevator
    { 
        /// <summary>
        /// speed of elevator in millisecond
        /// </summary>
        private const int SPEED = 100;
        /// <summary>
        /// Current status of elevator
        /// </summary>
        public ElevatorStatus Status {get;set;}
        /// <summary>
        /// Current position of elevator
        /// </summary>
        public int CurrentFloor { get; set; }
        /// <summary>
        /// ID of elevator
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of elevator
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// num of floor
        /// </summary>
        public int NumOfFloor { get; set; }

        /// <summary>
        /// Store up requested floors
        /// </summary>
        private Boolean[] upRequests;
        /// <summary>
        /// Store down requested floors
        /// </summary>
        private Boolean[] downRequests;

        public delegate void UpdateStatus(string message);
        public event UpdateStatus? OnUpdateStatus;

        public Elevator(int id, string name, int numOfFloor)
        {
            this.ID = id;
            this.Name = name;
            this.NumOfFloor = numOfFloor;
            this.CurrentFloor = 0;
            this.Status = ElevatorStatus.Stopped;

            this.upRequests = new Boolean[numOfFloor];
            this.downRequests = new Boolean[numOfFloor];
        }
        public int GetFirstUpRequest()
        {
            return Array.IndexOf(this.upRequests, true);
        }
        public int GetFirstDownRequest()
        {
            return Array.LastIndexOf(this.downRequests, true);
        }
        public Boolean[] GetUpRequests()
        {
            return this.upRequests;
        }
        public Boolean GetUpRequest(int floor)
        {
            return this.upRequests[floor];
        }
        public Boolean[] GetDownRequests()
        {
            return this.downRequests;
        }
        public Boolean GetDownRequest(int floor)
        {
            return this.downRequests[floor];
        }
        public void RemoveDownRequest(int floor)
        {
            downRequests[floor] = false;
        }
        public void RemoveUpRequest(int floor)
        {
            this.upRequests[floor] = false;
        }
        public int GetSpeed()
        {
            return SPEED;
        }
        /// <summary>
        /// add new request
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="location"></param>
        /// <param name="direction"></param>
        public void AddRequest(int floor, Location location, Direction direction)
        {
            //TODO: can utilize Request to implement more features.
            Request request = new Request(floor, location, direction);
            if (request.Direction == Direction.DOWN)
            {
                this.downRequests[floor] = true;
                Debug.WriteLine("Added down request {0}", request);

            }
            else
            {
                this.upRequests[floor] = true;
                Debug.WriteLine("Added up request {0}", request);
            }

            Notification(string.Format("Added request {0}", request));
        }
        /// <summary>
        /// move to floor
        /// </summary>
        /// <param name="floor"></param>
        public void MoveToFloor(int floor)
        {
            if (floor < this.CurrentFloor)
            {
                this.Status = ElevatorStatus.GoingDown;
                Notification(string.Format("Elevator moving down to floor {0}", floor + 1));
            }
            else if (floor > this.CurrentFloor)
            {
                this.Status = ElevatorStatus.GoingUp;
                Notification(string.Format("Elevator moving up to floor {0}", floor + 1));
            }
            Thread.Sleep(this.GetSpeed() * Math.Abs(floor - this.CurrentFloor));
            CurrentFloor = floor;
            Notification(string.Format("Elevator stopped at floor {0}", floor + 1));
        }
        private void Notification(string message)
        {
            if (OnUpdateStatus != null)
            {
                OnUpdateStatus(message);
            }
        }
    }
}
