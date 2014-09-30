using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;
using System.Windows;
using System.Windows.Media;

namespace KinectWPF
{
    public struct SensorData
    {
        public List<SkeletonData> skeletonData;
        public DepthImagePoint[][] DepthData;
        public ColorPoint[][] ColorData;
    }

    public struct ColorPoint
    {
        public int X;
        public int Y;
        public Color color;
    }

    public struct JointPosition
    {
        public JointType Type;
        public SkeletonPoint Position;
        public JointTrackingState TrackingState;
    }

    public struct SkeletonData
    {
        public int TrackingId;
        public SkeletonTrackingState TrackingState;
        public SkeletonPoint Position;
        public List<JointPosition> Joints;
    }

    public static class Data
    {
        public static void ConvertToColorImagePointArray(byte[] colorImage, ref ColorPoint[][] pointArray, int width, int height)
        {
            //ColorPoint[][] pointArray = new ColorPoint[height][];

            for (int i = 0; i < height; i++)
            {
                //pointArray[i] = new ColorPoint[width];
                
                for (int j = 0; j < width; j++)
                {
                    ColorPoint point = new ColorPoint();
                    int index = (i * width + j) * 3;

                    point.X = j;
                    point.Y = i;
                    point.color = Color.FromRgb(colorImage[index], colorImage[index + 1], colorImage[index + 2]);

                    pointArray[i][j] = point;
                }
            }

            //return pointArray;
        }

        public static DepthImagePoint[][] ConvertToDepthImagePointArray(DepthImagePixel[] depthImage, int width, int height)
        {
            DepthImagePoint[][] pointArray = new DepthImagePoint[height][];

            for (int i = 0; i < height; i++)
            {
                pointArray[i] = new DepthImagePoint[width];
                
                for (int j = 0; j < width; j++)
                {
                    DepthImagePoint point = new DepthImagePoint();
                    int index = i * width + j;

                    point.X = j;
                    point.Y = i;
                    point.Depth = depthImage[index].Depth;

                    pointArray[i][j] = point;
                }
            }

            return pointArray;
        }

        public static SkeletonData MapSkeletonToOrigin(Skeleton skeleton, JointType originType)
        {
            JointPosition origin = new JointPosition();
            SkeletonData mapSkeleton = new SkeletonData();

            mapSkeleton.TrackingId = skeleton.TrackingId;
            mapSkeleton.TrackingState = skeleton.TrackingState;
            mapSkeleton.Position = skeleton.Position; // new SkeletonPoint() { X = 0f, Y = 0f, Z = 0f };
            mapSkeleton.Joints = new List<JointPosition>();

            foreach (Joint joint in skeleton.Joints)
            {
                JointPosition jointPos = new JointPosition()
                {
                    Position = joint.Position,
                    TrackingState = joint.TrackingState,
                    Type = joint.JointType
                };

                mapSkeleton.Joints.Add(jointPos);

                if (jointPos.Type == originType)
                {
                    origin = jointPos;
                }
            }

            if (origin.Type != originType)
            {
                return new SkeletonData();
            }

            for (int i = 0; i < mapSkeleton.Joints.Count; i++)
            {
                JointPosition jointPos = mapSkeleton.Joints[i];

                // Make relative to origin
                jointPos.Position.X -= origin.Position.X;
                jointPos.Position.Y -= origin.Position.Y;
                jointPos.Position.Z -= origin.Position.Z;

                // Flip orientation (x-axis and z-axis)
                //jointPos.Position.X *= -1;
                jointPos.Position.Z *= -1;

                mapSkeleton.Joints[i] = jointPos;
            }

            return mapSkeleton;
        }
    }
}
