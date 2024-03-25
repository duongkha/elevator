using ElevatorSystem.Constants;
using ElevatorSystem.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem
{
    public class ElevatorController
    {
        public delegate void UpdateStatus(string message);
        public event UpdateStatus? OnUpdateStatus;
        private Elevator elevator ;
        private Thread? thread;
        private Boolean stopped = false;
        /// <summary>
        /// Create new elevator
        /// </summary>
        /// <param name="numOfFloor"></param>
        public ElevatorController(int id, string name,int numOfFloor)
        {
            elevator = new Elevator(id, name, numOfFloor);
            elevator.OnUpdateStatus += Elevator_OnUpdateStatus;
            Start();
        }

        public Elevator GetElevator() { return elevator; }

        private void Elevator_OnUpdateStatus(string message)
        {
            Notification(message); 
        }

        private void Notification(string message)
        {
            Debug.WriteLine(message);
            if (OnUpdateStatus != null)
            {
                OnUpdateStatus(message);
            }
        }
        public void Run()
        {
            while (!stopped)
                ProcessRequests();
        }
        public void Start()
        {
            this.stopped = false;
            thread = new Thread(Run);
            thread.Start();
        }
        public void Stop()
        {
            this.stopped = true;
        }
        /// <summary>
        /// handle request inside elevator
        /// </summary>
        /// <param name="floor"></param>
        public void FloorPressedInside(int floor)
        {
            Debug.WriteLine("Request inside to floor {0}, current floor {1}", floor + 1, this.elevator.CurrentFloor);
            if (floor == this.elevator.CurrentFloor)
            {
                return;
            }

            AddRequest(floor, Location.INSIDE, floor < this.elevator.CurrentFloor ? Direction.DOWN : Direction.UP);
        }
        /// <summary>
        /// Handle request outside elevator
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="direction"></param>
        public void FloorPressedOutside(int floor, Direction direction)
        {
            Debug.WriteLine("Request outside from floor {0}, direction {1}, current floor {2}", floor + 1, direction, this.elevator.CurrentFloor);
            AddRequest(floor, Location.OUTSIDE, direction);
        }
        private void AddRequest(int floor, Location location, Direction direction)
        {
           this.elevator.AddRequest(floor, location, direction);
        }

        private void ProcessRequests()
        {
            if (this.elevator.Status == ElevatorStatus.GoingUp || this.elevator.Status == ElevatorStatus.Stopped)
            {
                ProcessMovingUp();
                ProcessMovingDown();
            }
            else
            {
                ProcessMovingDown();
                ProcessMovingUp();
            }
        }

        private void ProcessMovingUp()
        {
           
            if (!this.elevator.IsUpRequestEmpty())
            {
                Debug.WriteLine("Processing moving up requests");
                while(!this.elevator.IsUpRequestEmpty())
                {
                    Request request = this.elevator.NextUpRequest();
                    this.elevator.MoveToFloor(request.RequestedFloor);
                    OpenDoor(this.elevator.CurrentFloor);
                    Thread.Sleep(200);
                    CloseDoor();
                    
                }
                this.elevator.Status = !this.elevator.IsDownRequestEmpty() ? ElevatorStatus.GoingDown : ElevatorStatus.Stopped;
                Debug.WriteLine("Finished processing moving up requests");
            }

        }
        private void ProcessMovingDown()
        {
           
            if (!this.elevator.IsDownRequestEmpty())
            {
                Debug.WriteLine("Processing moving down requests");
                while(!this.elevator.IsDownRequestEmpty())
                {

                    Request request = this.elevator.NextDownRequest();
                    this.elevator.MoveToFloor(request.RequestedFloor);
                    OpenDoor(this.elevator.CurrentFloor);
                    Thread.Sleep(200);
                    CloseDoor();
                }
                this.elevator.Status = !this.elevator.IsUpRequestEmpty() ? ElevatorStatus.GoingUp : ElevatorStatus.Stopped;
                Debug.WriteLine("Finished processing moving down requests");
            }
        }

        private void OpenDoor(int floor)
        {
            this.elevator.CurrentFloor = floor;
            this.elevator.Status = ElevatorStatus.DoorOpened;
            Notification(string.Format("Door Opened at floor {0}", floor + 1));
        }
        private void CloseDoor()
        {
            this.elevator.Status = ElevatorStatus.DoorClosed;
            Notification("Door closed.");
        }
    }
}
