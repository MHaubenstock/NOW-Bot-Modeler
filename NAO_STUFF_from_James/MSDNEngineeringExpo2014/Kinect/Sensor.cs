using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

namespace Kinect
{
    public enum SensorStatus
    {
        NotInitialized, Enabled, Running, NotRunning
    }

    public class Sensor
    {
        private static KinectSensor sensor = null;
        private static SensorStatus status = SensorStatus.NotInitialized;

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
                    sensor.SkeletonStream.Enable();
                    sensor.ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);

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
    }
}
