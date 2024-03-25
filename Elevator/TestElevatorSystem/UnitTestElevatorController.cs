using ElevatorSystem;
using ElevatorSystem.Model;

namespace TestElevatorSystem
{
    public class Tests
    {
        ElevatorController controller ;
        [SetUp]
        public void Setup()
        {
            controller = new ElevatorController(0, "Elevator 1", 5);
        }

        [Test]
        public void TestFloorPressedInside()
        {
            controller = new ElevatorController(0, "Elevator 1", 5);
            controller.FloorPressedInside(1);
            Elevator elevator = controller.GetElevator();
            Assert.True(elevator != null);
            Assert.True(elevator?.IsUpRequestEmpty() == false);
           
        }

        [Test]
        public void TestFloorPressedOutside()
        {
            controller = new ElevatorController(0, "Elevator 1", 5);
            controller.FloorPressedOutside(0, ElevatorSystem.Constants.Direction.UP);
            controller.FloorPressedOutside(4, ElevatorSystem.Constants.Direction.DOWN);
            Elevator elevator = controller.GetElevator();
            Assert.True(elevator != null);
            Assert.False(elevator?.IsUpRequestEmpty());
            Assert.True(elevator?.IsDownRequestEmpty());
        }
    }
}