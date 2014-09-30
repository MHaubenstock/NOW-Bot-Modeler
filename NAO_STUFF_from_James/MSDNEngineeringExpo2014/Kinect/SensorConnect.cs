using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Kinect;

namespace Kinect
{
    public partial class SensorConnect : Form
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
                if (!Main.ShowDialog("Please reconnect Kinect device to computer and try again.", "Device Connection Failed"))
                {
                    Close();
                }
            }
        }
    }
}
