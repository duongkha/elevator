using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElevatorSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ElevatorController controller ;
        public MainWindow()
        {
            InitializeComponent();
            controller = new ElevatorController(0, "Elevator 1", 5);
            controller.OnUpdateStatus += Controller_OnUpdateStatus;
            controller.Start();
        }
       
        private void Controller_OnUpdateStatus(string message)
        {
            this.txtMessage.Dispatcher.Invoke(new Action(() => {
                this.txtMessage.Text += message + "\n";
                txtMessage.Focus();
                txtMessage.CaretIndex = txtMessage.Text.Length;
                txtMessage.ScrollToEnd();
            }));
            
        }

        private void ButtonOutSideRequest_Click(object sender, RoutedEventArgs e)
        {
            string btnName = sender != null?((Button)sender).Name:string.Empty;
            if (!string.IsNullOrEmpty(btnName))
            {
                string[] arr = btnName.Split("_");
                int floor = Int32.Parse(arr[1]);
                string direction = arr[0].Substring(3);
                controller.FloorPressedOutside(floor,direction == "Up"? Constants.Direction.UP: Constants.Direction.DOWN);
             }
        }
        private void ButtonInsideRequest_Click(object sender, RoutedEventArgs e)
        {
            string btnName = sender != null ? ((Button)sender).Name : string.Empty;
            if (!string.IsNullOrEmpty(btnName))
            {
                string[] arr = btnName.Split("_");
                int floor = Int32.Parse(arr[1]);
                controller.FloorPressedInside( floor);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.controller.Stop();
        }

        private void btnUp_Automate_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                controller.FloorPressedOutside(0, ElevatorSystem.Constants.Direction.UP);
                Thread.Sleep(1000);
                controller.FloorPressedOutside(3, ElevatorSystem.Constants.Direction.UP);
                //Thread.Sleep(100);
                controller.FloorPressedInside(1);
                Thread.Sleep(1000);
                controller.FloorPressedInside(4);
                Thread.Sleep(500);
                controller.FloorPressedOutside(0, ElevatorSystem.Constants.Direction.UP);
                Thread.Sleep(1000);
                controller.FloorPressedOutside(4, ElevatorSystem.Constants.Direction.DOWN);
                controller.FloorPressedOutside(3, ElevatorSystem.Constants.Direction.DOWN);
                Thread.Sleep(300);
                controller.FloorPressedInside(1);
            }).Start();
            
        }
    }
}