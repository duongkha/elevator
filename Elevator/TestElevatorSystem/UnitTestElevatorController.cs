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
            controller.FloorPressedInside(1);
            Elevator elevator = controller.GetElevator();
            Assert.True(elevator != null);
            Assert.True(elevator?.GetFirstUpRequest() > -1);
           
        }

        [Test]
        public void TestFloorPressedOutside()
        {
            controller.FloorPressedOutside(0, ElevatorSystem.Constants.Direction.UP);
            controller.FloorPressedOutside(4, ElevatorSystem.Constants.Direction.DOWN);
            Elevator elevator = controller.GetElevator();
            Assert.True(elevator != null);
            Assert.True(elevator?.GetFirstUpRequest() > -1);
            Assert.True(elevator?.GetFirstDownRequest() > -1);
        }
    }
}