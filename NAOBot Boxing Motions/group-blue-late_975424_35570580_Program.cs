using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Aldebaran.Proxies;

//using J2i.Net.XInputWrapper;

namespace Final_Lab_Get_Angles
{
    class Program
    {

        /*XboxController cuntroller;

        void button_pressed(object sender, XboxControllerStateChangedEventArgs e)
        {
            if(cuntroller.IsAPressed)
                Console.WriteLine();
        }*/

        static void Main(string[] args)
        {
            //cuntroller = XboxController.RetrieveController(0);
            //cuntroller.StateChanged += button_pressed;

            string IP = "192.168.0.1";
            //string IP = "169.254.226.148";
            //string IP = "127.0.0.1";

            int port = 9559;

            /*List<float> RAngle = new List<float>{-0.4662941f, -0.4632261f, -0.558418f, 1.326952f, -0.628898f, 0.3977605f};
            List<float> LAngle = new List<float> { -0.4662941f, 0.009245962f, -0.866668f, 0.6427041f, 0.371186f, -0.07205604f };
            List<float> LLeg = new List<float> { -0.02143404f, 0.02765396f, -1.53589f, -0.09232792f, 0.233126f, -0.01683204f };*/

            List<float> RLeg = new List<float> { -0.461692f, -0.5092461f, -0.208666f, 0.6903419f, -0.21932f, 0.3977605f };
            List<float> LLeg = new List<float> { -0.461692f, 0.174918f, -0.400332f, 0.57214f, 0.118076f, -0.147222f };
            List<float> RArm = new List<float> { 0.22554f, 0.151824f, 1.17807f, 1.544616f, 1.263974f, 0.322f };
            List<float> LArm = new List<float> { 0.4463521f, -0.280764f, -1.075376f, -1.544616f, -0.736362f, 0.3216f };
            List<float> LPunch = new List<float> { -0.190258f, -0.05679996f, 0.191708f, -0.16563f, -0.44797f, 0.3216f };
            List<float> RPunch = new List<float> { -0.02296804f, 0.3141593f, -0.85448f, 0.04299396f, -0.05373196f, 0.03f };
            List<float> RHookA = new List<float> { 0.162646f, -1.16128f, 0.294486f, 1.5141f, 0.277612f, 0.2f };
            List<float> RHookB = new List<float> { -0.27301f, 0.3141593f, 0.200912f, 0.259288f, -1.127532f, 0.1996f };
            List<float> RHookL = new List<float> { -0.27301f, 0.3141593f, 0.200912f, 0.259288f, -1.127532f, 0.1996f };


            List<float> LHookA = new List<float> { 0.8329201f, 1.233294f, -1.112192f, -1.524754f, 0.004560038f, 0.3216f };
            List<float> LHookB = new List<float> { -0.23321f, -0.21787f, -0.467912f, -0.122678f, 0.38039f, 0.3216f };
            List<float> LuA = new List<float> { 1.246329f, -0.2638353f, -1.11164f, -1.202817f, -1.6675f, 0.3252f };
            List<float> LuB = new List<float> { 0.08279404f, -0.29457f, -1.247184f, -0.8221821f, -1.810162f, 0.3256f };

            List<float> STimeA = new List<float> { .1f, .1f, .1f, .1f, .1f, .1f };
            List<float> RTimeA = new List<float> { .3f, .3f, .3f, .3f, .3f, .3f };
            List<float> LTimeA = new List<float> { .7f, .7f, .7f, .7f, .7f, .7f };

            List<float> LDrop = new List<float> { -0.05986796f, -0.285366f, -1.807094f, -0.16563f, -1.241048f, 0.4088f };
            List<float> RDrop = new List<float> { -0.07359004f, 0.06592004f, 1.360616f, 0.09975196f, 1.056884f, 0.3212f };

            List<float> Rsmash = new List<float> { -0.66418f, 0.231592f, 1.518618f, 0.07520796f, 1.245566f, 0.3224f };
            List<float> Lsmash = new List<float> { -0.64739f, -0.3141593f, -1.40672f, -0.03490658f, -0.85448f, 1f };

            List<float> ROut = new List<float> { 0.57836f, -1.303942f, 1.958876f, 0.09668396f, 0.6319661f, 0.3224f };
            List<float> LOut = new List<float> { 0.7654241f, 1.156594f, -2.00498f, -0.118076f, -0.730226f, 0.322f };

            GamePadState cs;
            GamePadState ps = new GamePadState();

            Boolean upper = false;
            Boolean hook = false;
            Boolean jab = false;
            Boolean high = false;

            Random random = new Random();
            int rNum = 0;

            TextToSpeechProxy tts = new TextToSpeechProxy(IP, port);
            MotionProxy motion = new MotionProxy(IP, port);
            RobotPostureProxy pos = new RobotPostureProxy(IP, port);
            LedsProxy led = new LedsProxy(IP, port);

            Console.WriteLine("hello");

            motion.setStiffnesses("Body", 1.0f);
            //pos.goToPosture("Sit", 1.0f);
            pos.goToPosture("Stand", 1.0f);
            //pos.goToPosture("StandZero", 1f);

            pos.stopMove();

            motion.setWalkArmsEnable(false, false);

            //motion.walkInit();
            motion.post.setAngles("RArm", RArm, 0.2f);
            motion.setAngles("LArm", LArm, 0.2f);

            motion.post.angleInterpolation("RLeg", RLeg, new List<float> { 1.5f, 1.5f, 1f, 1f, 1f, 1f }, true);
            motion.post.angleInterpolation("LLeg", LLeg, new List<float> { 1f, 1f, 1f, 1f, 1f, 1f }, true);

            //motion.post.setAngles("RLeg", RLeg, 0.1f);
            //motion.setAngles("LLeg", LLeg, 0.1f);

            //Console.ReadKey();

            Console.WriteLine();
            //motion.setAngles("LLeg", LLeg, 0.2f);

            //motion.setStiffnesses("Body", 0.0f);
            Console.WriteLine("Set Stiffness");
            string enter = null;// = Console.ReadLine().ToLower();

            #region GAMELOOP
            cs = GamePad.GetState(PlayerIndex.One);

            while (cs.Buttons.Start != ButtonState.Pressed)
            {
                cs = GamePad.GetState(PlayerIndex.One);
                if (!cs.IsConnected)
                    Console.WriteLine("Cuntroller ain't connected");
                #region DEVTOOLS
                if (enter == "stiff")
                {
                    motion.setStiffnesses("Body", 0.5f);
                    //motion.setStiffnesses("RArm", 0.1f);
                    //motion.setStiffnesses("LArm", 0.1f);
                }
                if (enter == "unstiff")
                {
                    //motion.setStiffnesses("Body", 0.0f);
                    motion.setStiffnesses("RLeg", 0.0f);
                    //motion.setStiffnesses("LArm", 0.0f);
                }

                if (enter == "cool")
                {
                    motion.setStiffnesses("Body", 0.0f);
                }

                if (enter == "RLeg")
                {
                    foreach (float x in motion.getAngles("RLeg", false))
                        Console.Write("{0}f, ", x);
                }

                if (enter == "LLeg")
                {
                    foreach (float x in motion.getAngles("LLeg", false))
                        Console.Write("{0}f, ", x);
                }

                if (enter == "RArm")
                {
                    foreach (float x in motion.getAngles("RArm", false))
                        Console.Write("{0}f, ", x);
                }

                if (enter == "LArm")
                {
                    foreach (float x in motion.getAngles("LArm", false))
                        Console.Write("{0}f, ", x);
                }
                #endregion
                #region POSTURES
                //POSITION BLOCKS
                if (enter == "b")
                {
                    motion.setAngles("LArm", LArm, 0.2f);
                    motion.setAngles("RArm", RArm, 0.2f);
                }

                if (cs.Buttons.Back == ButtonState.Pressed && ps.Buttons.Back != cs.Buttons.Back)
                {
                    pos.goToPosture("Sit", .5f);
                    motion.setStiffnesses("Body", 0.0f);
                }

                if (cs.Buttons.LeftStick == ButtonState.Pressed && ps.Buttons.LeftStick != cs.Buttons.LeftStick)
                {
                    //tts.post.say("No one makes me bleed my own blood");
                    motion.stopMove();
                    motion.moveInit();
                    //pos.goToPosture("Stand", .8f);
                    //pos.stopMove();
                    motion.post.angleInterpolation("RLeg", RLeg, new List<float> { 1.5f, 1.5f, 1f, 1f, 1f, 1f }, true);
                    motion.post.angleInterpolation("LLeg", LLeg, new List<float> { 1f, 1f, 1f, 1f, 1f, 1f }, true);
                    motion.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);

                }

                #endregion
                #region COMBAT

                //COMBAT BLOCKS
                if (cs.Buttons.A == ButtonState.Pressed && ps.Buttons.A != cs.Buttons.A)
                {
                    upper = false;
                    hook = false;
                    jab = true;
                    high = false;
                }

                if (cs.Buttons.B == ButtonState.Pressed && ps.Buttons.B != cs.Buttons.B)
                {
                    upper = true;
                    hook = false;
                    jab = false;
                    high = false;
                }

                if (cs.Buttons.X == ButtonState.Pressed && ps.Buttons.X != cs.Buttons.X)
                {
                    upper = false;
                    hook = true;
                    jab = false;
                    high = false;
                }

                if (cs.Buttons.Y == ButtonState.Pressed && ps.Buttons.Y != cs.Buttons.Y)
                {
                    upper = false;
                    hook = false;
                    jab = false;
                    high = true;
                }

                if (cs.Triggers.Left != 0 && jab)
                {
                    //Console.WriteLine("lt pressed");
                    motion.angleInterpolation("LArm", LPunch, RTimeA, true);
                    motion.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                    //motion.post.setAngles("RArm", RPunch, 0.5f);
                    ps = cs;

                }

                if (cs.Triggers.Right != 0 && jab)
                {
                    motion.post.angleInterpolation("RArm", RPunch, RTimeA, true);
                    motion.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                }

                if (cs.Triggers.Right != 0 && hook)
                {
                    //motion.post.setAngles("RArm", RHookA, 0.3f);
                    // motion.angleInterpolation("RLeg", RHookL, RTimeA, true);
                    motion.angleInterpolation("RArm", RHookA, RTimeA, true);
                    motion.angleInterpolation("RArm", RHookB, RTimeA, true);
                    //motion.angleInterpolation("RLeg", RLeg, RTimeA, true);
                    motion.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                }

                if (cs.Triggers.Left != 0 && hook)
                {
                    //motion.post.setAngles("RArm", RHookA, 0.3f);
                    motion.angleInterpolation("LArm", LHookA, RTimeA, true);
                    motion.angleInterpolation("LArm", LHookB, RTimeA, true);
                    motion.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                }

                if ((cs.Triggers.Right != 0 || cs.Triggers.Left != 0) && upper)
                {

                    //motion.post.setAngles("RArm", RHookA, 0.3f);
                    motion.angleInterpolation("LArm", LuA, RTimeA, true);
                    motion.angleInterpolation("LArm", LuB, RTimeA, true);
                    motion.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                }

                if (cs.Buttons.RightShoulder == ButtonState.Pressed && ps.Buttons.RightShoulder != cs.Buttons.RightShoulder)
                {
                    //motion.post.setAngles("RArm", RHookA, 0.3f);
                    motion.post.angleInterpolation("LArm", LDrop, RTimeA, true);
                    motion.angleInterpolation("RArm", RDrop, RTimeA, true);
                    motion.post.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                }

                if (cs.Buttons.LeftShoulder == ButtonState.Pressed && ps.Buttons.LeftShoulder != cs.Buttons.LeftShoulder)
                {

                    //motion.post.setAngles("RArm", RHookA, 0.3f);
                    motion.post.angleInterpolation("LArm", LOut, RTimeA, true);
                    motion.angleInterpolation("RArm", ROut, RTimeA, true);
                    motion.post.angleInterpolation("LArm", LDrop, RTimeA, true);
                    motion.angleInterpolation("RArm", RDrop, RTimeA, true);
                    motion.post.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                }

                if (cs.Triggers.Left != 0 && high)
                {
                    motion.angleInterpolation("LArm", Lsmash, RTimeA, true);
                    motion.angleInterpolation("LArm", LDrop, RTimeA, true);
                    motion.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                    //motion.post.setAngles("RArm", RPunch, 0.5f);
                }

                if (cs.Triggers.Right != 0 && high)
                {
                    motion.angleInterpolation("RArm", Rsmash, RTimeA, true);
                    motion.angleInterpolation("RArm", RDrop, RTimeA, true);
                    motion.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                }
                #endregion
                #region MOVEMMENT
                //MOVEMENT BLOCKS
                if (enter == "walk")
                {
                    motion.moveInit();

                    motion.move(3f, 0f, 0f);
                    Console.ReadKey();
                    motion.stopMove();

                    motion.post.angleInterpolation("RLeg", RLeg, new List<float> { 1.5f, 1.5f, 1f, 1f, 1f, 1f }, true);
                    motion.post.angleInterpolation("LLeg", LLeg, new List<float> { 1f, 1f, 1f, 1f, 1f, 1f }, true);
                }

                if (cs.ThumbSticks.Left.Y > .8)
                {
                    if (ps.ThumbSticks.Left.Y <= 0)
                        motion.stopMove();
                    if (ps.ThumbSticks.Left.Y == 0)
                        motion.moveInit();

                    motion.move(.1f, 0f, 0f);

                    //motion.post.angleInterpolation("RLeg", RLeg, new List<float> { 1.5f, 1.5f, 1f, 1f, 1f, 1f }, true);
                    //motion.post.angleInterpolation("LLeg", LLeg, new List<float> { 1f, 1f, 1f, 1f, 1f, 1f }, true);
                }
                if (cs.ThumbSticks.Left.Y < -.8)
                {
                    if (ps.ThumbSticks.Left.Y >= 0)
                        motion.stopMove();
                    if (ps.ThumbSticks.Left.Y == 0)
                        motion.moveInit();

                    motion.move(-.15f, 0f, 0f);

                    //motion.post.angleInterpolation("RLeg", RLeg, new List<float> { 1.5f, 1.5f, 1f, 1f, 1f, 1f }, true);
                    //motion.post.angleInterpolation("LLeg", LLeg, new List<float> { 1f, 1f, 1f, 1f, 1f, 1f }, true);
                }
                if (cs.ThumbSticks.Left.X > .8)
                {
                    Console.WriteLine(cs.ThumbSticks.Left.X);
                    if (ps.ThumbSticks.Left.X <= 0)
                        motion.stopMove();
                    if (ps.ThumbSticks.Left.X == 0)
                        motion.moveInit();

                    motion.move(0f, -.15f, 0f);

                    //motion.post.angleInterpolation("RLeg", RLeg, new List<float> { 1.5f, 1.5f, 1f, 1f, 1f, 1f }, true);
                    //motion.post.angleInterpolation("LLeg", LLeg, new List<float> { 1f, 1f, 1f, 1f, 1f, 1f }, true);
                }
                if (cs.ThumbSticks.Left.X < -.8)
                {
                    if (ps.ThumbSticks.Left.X >= 0)
                        motion.stopMove();
                    if (ps.ThumbSticks.Left.X == 0)
                        motion.moveInit();

                    motion.move(0f, .15f, 0f);

                    //motion.post.angleInterpolation("RLeg", RLeg, new List<float> { 1.5f, 1.5f, 1f, 1f, 1f, 1f }, true);
                    //motion.post.angleInterpolation("LLeg", LLeg, new List<float> { 1f, 1f, 1f, 1f, 1f, 1f }, true);
                }

                if ((cs.ThumbSticks.Left.Y == 0 && ps.ThumbSticks.Left.Y != 0) || (cs.ThumbSticks.Left.X == 0 && ps.ThumbSticks.Left.X != 0))
                {
                    motion.stopMove();

                }

                if (cs.ThumbSticks.Right.X < -.75)
                {
                    if (ps.ThumbSticks.Right.X >= 0)
                    {
                        motion.stopMove();
                        motion.moveInit();
                    }

                    motion.move(0f, 0f, .3f);
                }

                if (cs.ThumbSticks.Right.X > .75)
                {
                    if (ps.ThumbSticks.Right.X <= 0)
                    {
                        motion.stopMove();
                        motion.moveInit();
                    }

                    motion.move(0f, 0f, -.3f);
                }

                if (cs.ThumbSticks.Right.X == 0 && ps.ThumbSticks.Right.X != 0)
                {
                    motion.stopMove();

                }

                if (cs.DPad.Up == ButtonState.Pressed && ps.DPad.Up != cs.DPad.Up)
                {
                    //tts.post.say("No one makes me bleed my own blood");
                    motion.stopMove();
                    motion.moveInit();
                    pos.goToPosture("Stand", .8f);
                    pos.stopMove();
                }

                if (cs.DPad.Right == ButtonState.Pressed && ps.DPad.Right != cs.DPad.Right)
                {
                    //tts.post.say("No one makes me bleed my own blood");
                    motion.setWalkArmsEnabled(true, true);
                }

                if (cs.DPad.Left == ButtonState.Pressed && ps.DPad.Left != cs.DPad.Left)
                {
                    //tts.post.say("No one makes me bleed my own blood");
                    motion.angleInterpolation("RArm", RArm, RTimeA, true);
                    motion.angleInterpolation("LArm", LArm, RTimeA, true);
                }

                #endregion
                #region TRASHTALKING
                if (cs.Buttons.RightStick == ButtonState.Pressed && ps.Buttons.RightStick != cs.Buttons.RightStick)
                {
                    led.post.rasta(5f);
                    rNum = random.Next(0, 14);
                    switch (rNum)
                    {
                        case 0:
                            tts.post.say("I'm going to put my hard drive in your floopy disk");
                            break;
                        case 1:
                            tts.post.say("I will haunt your dreams, fool. See you tonite.");
                            break;
                        case 2:
                            tts.post.say("Mess with us and we will dismember you");
                            break;
                        case 3:
                            tts.post.say("Everyone Must Die!!!!");
                            break;
                        case 4:
                            tts.post.say("FOOLS");
                            break;
                        case 5:
                            tts.post.say("Meow");
                            break;
                        case 6:
                            tts.post.say("DIE");
                            break;
                        case 7:
                            tts.post.say("Hi I'm Catbug");
                            break;
                        case 8:
                            tts.post.say("I'll bash yer head in, I swear on my mum");
                            break;
                        case 9:
                            tts.post.say("I am I Robot");
                            break;
                        case 10:
                            tts.post.say("Rawr");
                            break;
                        case 11:
                            tts.post.say("Gas Powered Stick");
                            break;
                        case 12:
                            tts.post.say("Locally Grown Butter Lettuce. Butter Lettuce PARTY");
                            break;
                        case 13:
                            tts.post.say("Cake and grief counseling will be available after the fight.");
                            break;
                    }
                }
                #endregion
                //Console.Write("\n");
                //Console.WriteLine(motion.getAngles("RArm", false));
                //enter = Console.ReadLine();
                ps = cs;
            }
            #endregion

        }
    }
}
