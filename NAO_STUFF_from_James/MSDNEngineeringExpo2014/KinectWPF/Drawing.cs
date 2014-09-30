using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KinectWPF
{
    public static class Drawing
    {
        public static DrawingImage DrawSkeleton(Skeleton[] skeletonData, WriteableBitmap bitmap, double jointThickness = 3.0, double bodyCenterThickness = 10.0)
        {
            Rect screenSize = new Rect(0.0, 0.0, bitmap.Width, bitmap.Height);
            DrawingGroup drawingGroup = new DrawingGroup();
            DrawingImage drawingImage = new DrawingImage(drawingGroup);

            using (DrawingContext drawingContext = drawingGroup.Open())
            {
                drawingContext.DrawRectangle(Brushes.Black, null, screenSize);
                drawingContext.DrawImage(bitmap, screenSize);

                if (skeletonData.Length != 0)
                {
                    foreach (Skeleton skeleton in skeletonData)
                    {
                        RenderClippedEdges(skeleton, drawingContext, screenSize);

                        if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            DrawBonesAndJoints(skeleton, drawingContext, jointThickness);
                        }
                        else if (skeleton.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            drawingContext.DrawEllipse(
                            Brushes.Blue,
                            null,
                            Sensor.MapSkeletonPointToPoint(skeleton.Position),
                            bodyCenterThickness,
                            bodyCenterThickness);
                        }
                    }
                }

                drawingGroup.ClipGeometry = new RectangleGeometry(screenSize);
            }

            return drawingImage;

            /*var _drawingImage = new Image { Source = drawingImage };
            _drawingImage.Arrange(screenSize);
            var _bitmap = new RenderTargetBitmap((int)screenSize.Width, (int)screenSize.Height, 96, 96, PixelFormats.Pbgra32);
            _bitmap.Render(_drawingImage);
            //WriteableBitmap bmp = new WriteableBitmap(_bitmap); // new WriteableBitmap(new FormatConvertedBitmap(_bitmap, PixelFormats.Bgr32, null, 0));
            return new WriteableBitmap(_bitmap); // bmp;*/
        }

        private static void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext, double jointThickness = 3.0)
        {
            // Render Torso
            DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Left Leg
            DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = Brushes.Yellow;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, Sensor.MapSkeletonPointToPoint(joint.Position), jointThickness, jointThickness);
                }
            }
        }

        private static void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = new Pen(Brushes.Gray, 1);
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = new Pen(Brushes.Green, 6);
            }

            drawingContext.DrawLine(drawPen, Sensor.MapSkeletonPointToPoint(joint0.Position), Sensor.MapSkeletonPointToPoint(joint1.Position));
        }

        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext, Rect screenSize, int lineSize = 3)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, screenSize.Height - lineSize, screenSize.Width, lineSize));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, screenSize.Width, lineSize));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, lineSize, screenSize.Height));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(screenSize.Width - lineSize, 0, lineSize, screenSize.Height));
            }
        }
    }
}
