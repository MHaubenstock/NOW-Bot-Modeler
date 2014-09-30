using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;
using System.Windows.Media;
using System.Windows;

namespace KinectWPF
{
    public enum SensorStatus
    {
        NotInitialized, Enabled, Running, NotRunning
    }

    public class Sensor
    {
        public static KinectSensor sensor = null; // Needs to be private
        private static SensorStatus status = SensorStatus.NotInitialized;

        private static TransformSmoothParameters smoothingParam = new TransformSmoothParameters()
        {
            Smoothing = 0.5f,
            Correction = 0.1f,
            Prediction = 0.5f,
            JitterRadius = 0.1f,
            MaxDeviationRadius = 0.1f
        };

        public static SensorStatus Status
        {
            get
            {
                return status;
            }
        }

        public static bool EnumDevices()
        {
            if (sensor == null && status == SensorStatus.NotInitialized)
            {
                foreach (var potentialSensor in KinectSensor.KinectSensors)
                {
                    if (potentialSensor.Status == KinectStatus.Connected)
                    {
                        sensor = potentialSensor;
                        status = SensorStatus.Enabled;

                        return true;
                    }
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        public static bool EnableDevice()
        {
            if (sensor != null)
            {
                try
                {
                    sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    sensor.SkeletonStream.Enable(smoothingParam);
                    //sensor.ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);

                    status = SensorStatus.Enabled;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public static bool DisableDevice()
        {
            if (sensor != null && status != SensorStatus.NotInitialized)
            {
                if (status == SensorStatus.Running)
                    StopDevice();

                status = SensorStatus.NotRunning;

                sensor.ColorStream.Disable();
                sensor.DepthStream.Disable();
                sensor.SkeletonStream.Disable();
                sensor.ColorStream.Disable();

                sensor = null;

                status = SensorStatus.NotInitialized;

                return true;
            }

            return false;
        }

        public static bool StartDevice()
        {
            if (sensor != null && status != SensorStatus.Running && (status == SensorStatus.Enabled || status == SensorStatus.NotRunning))
            {
                sensor.Start();
                status = SensorStatus.Running;

                return true;
            }

            return false;
        }

        public static bool StopDevice()
        {
            if (sensor != null && status == SensorStatus.Running)
            {
                sensor.Stop();
                status = SensorStatus.NotRunning;

                return true;
            }

            return false;
        }

        public static bool RegisterColorFrameReadyEvent(Action<object, ColorImageFrameReadyEventArgs> method)
        {
            if (sensor != null && status != SensorStatus.NotInitialized)
            {
                sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(method);

                return true;
            }

            return false;
        }

        public static bool RegisterDepthFrameReadyEvent(Action<object, DepthImageFrameReadyEventArgs> method)
        {
            if (sensor != null && status != SensorStatus.NotInitialized)
            {
                sensor.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(method);

                return true;
            }

            return false;
        }

        public static bool RegisterSkeletonFrameReadyEvent(Action<object, SkeletonFrameReadyEventArgs> method)
        {
            if (sensor != null && status != SensorStatus.NotInitialized)
            {
                sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(method);

                return true;
            }

            return false;
        }

        public static int PixelDataLength
        {
            get
            {
                if (sensor != null && status != SensorStatus.NotInitialized)
                {
                    return sensor.ColorStream.FramePixelDataLength;
                }

                return -1;
            }
        }

        public static int Width
        {
            get
            {
                if (sensor != null && status != SensorStatus.NotInitialized)
                {
                    return sensor.ColorStream.FrameWidth;
                }

                return -1;
            }
        }

        public static int Height
        {
            get
            {
                if (sensor != null && status != SensorStatus.NotInitialized)
                {
                    return sensor.ColorStream.FrameHeight;
                }

                return -1;
            }
        }

        public static int DepthPixelDataLength
        {
            get
            {
                if (sensor != null && status != SensorStatus.NotInitialized)
                {
                    return sensor.DepthStream.FramePixelDataLength;
                }

                return -1;
            }
        }

        public static int DepthWidth
        {
            get
            {
                if (sensor != null && status != SensorStatus.NotInitialized)
                {
                    return sensor.DepthStream.FrameWidth;
                }

                return -1;
            }
        }

        public static int DepthHeight
        {
            get
            {
                if (sensor != null && status != SensorStatus.NotInitialized)
                {
                    return sensor.DepthStream.FrameHeight;
                }

                return -1;
            }
        }

        public static int SkeletonArrayLength
        {
            get
            {
                if (sensor != null && status != SensorStatus.NotInitialized)
                {
                    return sensor.SkeletonStream.FrameSkeletonArrayLength;
                }

                return -1;
            }
        }

        public static DepthImagePoint MapSkeletonPointToDepthPoint(SkeletonPoint point)
        {
            return sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(point, sensor.DepthStream.Format);
        }

        public static Point MapSkeletonPointToPoint(SkeletonPoint point)
        {
            DepthImagePoint depthPoint = MapSkeletonPointToDepthPoint(point);
            return new Point(depthPoint.X, depthPoint.Y);
        }
    }
}
