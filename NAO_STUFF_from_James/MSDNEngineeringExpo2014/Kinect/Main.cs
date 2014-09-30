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
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kinect
{
    public partial class Main : Form
    {
        private byte[] colorImage;
        private WriteableBitmap colorBitmap;

        public Main()
        {
            InitializeComponent();

            SensorConnect connect = new SensorConnect();
            connect.ShowDialog();

            if (Sensor.Status == SensorStatus.NotInitialized)
            {
                Close();
            }

            if (Sensor.PixelDataLength > 0)
            {
                colorImage = new byte[Sensor.PixelDataLength];
            }
            else
            {
                ShowMessageDialog("Error: The program was unable to receive appropriate pixel data.", "Error");
            }

            colorBitmap = new WriteableBitmap(Sensor.Width, Sensor.Height, 96.0, 96.0, PixelFormats.Bgr32, null);
            //cameraBox.Image = colorBitmap;
            Sensor.RegisterColorFrameReadyEvent(SensorColorFrameReady);
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(colorImage);

                    // Write the pixel data into our bitmap
                    colorBitmap.WritePixels(
                        new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight),
                        colorImage,
                        colorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }

        public static bool ShowDialog(string text, string caption)
        {
            bool value = false;
            Form prompt = new Form();

            prompt.Width = 500;
            prompt.Height = 150;
            prompt.Text = caption;

            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 100, Top = 50 };
            Button cancelation = new Button() { Text = "Cancel", Left = 170, Width = 100, Top = 50 };

            confirmation.Click += (sender, e) => { prompt.Close(); value = true; };
            cancelation.Click += (sender, e) => { prompt.Close(); value = false; };

            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancelation);
            prompt.Controls.Add(textLabel);

            prompt.ShowDialog();

            return value;
        }

        public static bool ShowMessageDialog(string text, string caption)
        {
            bool value = false;
            Form prompt = new Form();

            prompt.Width = 500;
            prompt.Height = 150;
            prompt.Text = caption;

            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 100, Top = 50 };

            confirmation.Click += (sender, e) => { prompt.Close(); value = true; };

            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);

            prompt.ShowDialog();

            return value;
        }
    }
}
