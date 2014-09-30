using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KinectWPF
{
    /// <summary>
    /// Interaction logic for SensorConnect.xaml
    /// </summary>
    public partial class SensorConnect : Window
    {
        public SensorConnect()
        {
            InitializeComponent();

            if (Sensor.EnumDevices())
            {
                Close();
            }
            else
            {
                if (!MainWindow.ShowDialog("Please reconnect Kinect device to computer and try again.", "Device Connection Failed"))
                {
                    Close();
                }
            }
        }
    }
}
