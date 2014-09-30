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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Kinect;

namespace KinectWPF
{
    public partial class MainWindow : Window
    {
        private byte[] colorImage;
        private ColorPoint[][] colorPointArray;
        private WriteableBitmap colorBitmap;

        private DepthImagePixel[] depthImage;
        private DepthImagePoint[][] depthPointArray;
        private byte[] depthColorImage;
        private WriteableBitmap depthBitmap;

        private Skeleton[] skelData;
        private List<SkeletonData> skeletonData;
        private byte[] skeletonColorImage;
        private WriteableBitmap skeletonBitmap;

        private bool colorBusy = false;
        private bool depthBusy = false;
        private bool skeletonBusy = false;

        public SensorData sensorData;

        public MainWindow()
        {
            InitializeComponent();

            //SensorConnect connect = new SensorConnect();
            //connect.ShowDialog();
            while (!Sensor.EnumDevices())
            {
                if (!MainWindow.ShowDialog("Please connect the Kinect device to the computer and try again.", "Device Connection Failed"))
                {
                    Close();
                    break;
                }
            }

            if (Sensor.Status == SensorStatus.NotInitialized)
            {
                Close();
            }

            if (!Sensor.EnableDevice())
            {
                ShowMessageDialog("Error: Device could not be enabled.", "Error");
            }

            if (Sensor.PixelDataLength > 0)
            {
                colorImage = new byte[Sensor.PixelDataLength];
                colorPointArray = new ColorPoint[Sensor.Height][];

                for (int i = 0; i < Sensor.Height; i++)
                {
                    colorPointArray[i] = new ColorPoint[Sensor.Width];
                }
            }
            else
            {
                ShowMessageDialog("Error: The program was unable to receive appropriate pixel data.", "Error");
            }

            colorBitmap = new WriteableBitmap(Sensor.Width, Sensor.Height, 96.0, 96.0, PixelFormats.Bgr32, null);
            KinectColor.Source = colorBitmap;

            if (Sensor.DepthPixelDataLength > 0)
            {
                depthImage = new DepthImagePixel[Sensor.DepthPixelDataLength];
                depthColorImage = new byte[Sensor.DepthPixelDataLength * sizeof(int)];
                depthPointArray = new DepthImagePoint[Sensor.DepthHeight][];

                for (int i = 0; i < Sensor.DepthHeight; i++)
                {
                    depthPointArray[i] = new DepthImagePoint[Sensor.DepthWidth];
                }

                skeletonColorImage = new byte[Sensor.DepthPixelDataLength * sizeof(int)];
            }
            else
            {
                ShowMessageDialog("Error: The program was unable to receive appropriate pixel data.", "Error");
            }

            depthBitmap = new WriteableBitmap(Sensor.DepthWidth, Sensor.DepthHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
            KinectDepth.Source = depthBitmap;

            if (Sensor.SkeletonArrayLength > 0)
            {
                skelData = new Skeleton[Sensor.SkeletonArrayLength];
                skeletonData = new List<SkeletonData>();
            }
            else
            {
                ShowMessageDialog("Error: The program was unable to receive appropriate skeleton data.", "Error");
            }

            skeletonBitmap = new WriteableBitmap(Sensor.DepthWidth, Sensor.DepthHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
            KinectSkeleton.Source = skeletonBitmap;

            if (!Sensor.RegisterColorFrameReadyEvent(SensorColorFrameReady) || !Sensor.RegisterDepthFrameReadyEvent(SensorDepthFrameReady) || !Sensor.RegisterSkeletonFrameReadyEvent(SensorSkeletonFrameReady))
            {
                ShowMessageDialog("Error: The program was unable to register the proper events to the Kinect sensor.", "Error");
            }

            if (!Sensor.StartDevice())
            {
                ShowMessageDialog("Error: The program was unable to start the Kinect sensor.", "Error");
            }
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null) // && colorBusy == false)
                {
                    //colorBusy = true;

                    colorFrame.CopyPixelDataTo(colorImage);
                    //Data.ConvertToColorImagePointArray(colorImage, ref colorPointArray, Sensor.Width, Sensor.Height);
                    //colorPointArray = Data.ConvertToColorImagePointArray(colorImage, Sensor.Width, Sensor.Height);
                    
                    //if (tabControl.TabIndex == 0)
                    {
                        if (colorImage != null)
                        {
                            colorBitmap.WritePixels(
                                new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight),
                                colorImage,
                                colorBitmap.PixelWidth * sizeof(int),
                                0);
                        }
                    }

                    //colorBusy = false;
                }
            }
        }

        private void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame != null && depthBusy == false)
                {
                    /*
                    depthBusy = true;

                    depthFrame.CopyDepthImagePixelDataTo(depthImage);
                    depthPointArray = Data.ConvertToDepthImagePointArray(depthImage, Sensor.DepthWidth, Sensor.DepthHeight);

                    //if (tabControl.TabIndex == 1)
                    {
                        int minDepth = depthFrame.MinDepth;
                        int maxDepth = depthFrame.MaxDepth;

                        int colorPixelIndex = 0;
                        for (int i = 0; i < this.depthImage.Length; ++i)
                        {
                            short depth = depthImage[i].Depth;

                            byte intensity = (byte)(((double)(depth >= minDepth && depth <= maxDepth ? depth - depthFrame.MinDepth : 0) / (depthFrame.MaxDepth - depthFrame.MinDepth)) * 255);

                            depthColorImage[colorPixelIndex++] = intensity;
                            depthColorImage[colorPixelIndex++] = intensity;
                            depthColorImage[colorPixelIndex++] = intensity;

                            ++colorPixelIndex;
                        }

                        if (depthColorImage != null)
                        {
                            depthBitmap.WritePixels(
                                new Int32Rect(0, 0, depthBitmap.PixelWidth, depthBitmap.PixelHeight),
                                depthColorImage,
                                depthBitmap.PixelWidth * sizeof(int),
                                0);
                        }
                    }

                    depthBusy = false;
                    */
                }
            }
        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null && skelData != null && skeletonBusy == false)
                {
                    skeletonBusy = true;

                    skeletonFrame.CopySkeletonDataTo(skelData);

                    if (skeletonData != null)
                    {
                        skeletonBitmap.WritePixels(
                            new Int32Rect(0, 0, skeletonBitmap.PixelWidth, skeletonBitmap.PixelHeight),
                            colorImage,
                            //depthColorImage,
                            skeletonBitmap.PixelWidth * sizeof(int),
                            0);
                        
                        //if (tabControl.TabIndex == 2)
                        {
                            KinectSkeleton.Source = Drawing.DrawSkeleton(skelData, skeletonBitmap);
                            //skeletonBitmap = Drawing.DrawSkeleton(skelData, skeletonBitmap);
                        }

                        // Declare new array for SkeletonData that is more manipulable; clean up array of position-only attributed entities
                        int i = 0;
                        skeletonData.Clear();

                        foreach (Skeleton skeleton in skelData)
                        {
                            if (skeleton.TrackingState == SkeletonTrackingState.Tracked && i < 2)
                            {
                                skeletonData.Add(Data.MapSkeletonToOrigin(skeleton, JointType.ShoulderCenter));
                                i++;
                            }
                        }

                        sensorData = new SensorData { skeletonData = skeletonData, DepthData = depthPointArray, ColorData = colorPointArray };

                        if (sensorData.skeletonData.Count > 0)
                        {
                            foreach (JointPosition joint in sensorData.skeletonData[0].Joints)
                            {
                                if (joint.Type == JointType.HandRight)
                                {
                                    Console.WriteLine("X: " + joint.Position.X.ToString());
                                    Console.WriteLine("Y: " + joint.Position.Y.ToString());
                                    Console.WriteLine("Z: " + joint.Position.Z.ToString());
                                }
                            }
                        }

                        // NOTICE: Use Data within the variable, sensorData, here
                    }

                    skeletonBusy = false;
                }
            }
        }

        public static bool ShowDialog(string text, string caption)
        {
            bool value = false;
            Prompt prompt = new Prompt();

            prompt.SetText(text);
            prompt.SetCaption(caption);
            prompt.Confirm.Click += (sender, e) => { value = true; };
            prompt.Cancel.Click += (sender, e) => { value = false; };

            prompt.ShowDialog();

            return value;
        }

        public static bool ShowMessageDialog(string text, string caption)
        {
            bool value = false;
            Prompt prompt = new Prompt();

            prompt.SetText(text);
            prompt.SetCaption(caption);
            prompt.Confirm.Click += (sender, e) => { value = true; };
            //prompt.Cancel.Click += (sender, e) => { value = false; };
            prompt.Cancel.Visibility = Visibility.Hidden;

            prompt.ShowDialog();

            return value;
        }
    }
}
