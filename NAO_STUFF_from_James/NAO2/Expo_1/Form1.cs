using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using MLApp;
using Aldebaran.Proxies;

namespace Expo_1
{
    public partial class Form1 : Form
    {
        public TextToSpeechProxy tts;
        public MotionProxy motion;
        public RobotPostureProxy posture;
        public NavigationProxy navigation;

        //
        public List<float> previous_angles;

        public void run_frame( List<float> vector = null , List<float> angles = null)
        {
            MLApp.MLApp matlab = new MLApp.MLApp();
            string result;

            string arguments = "";


            arguments += "gradientdescent([ ";
            foreach (float point in vector)
            {
                arguments += point.ToString() + " ";

            }
            arguments += " ]";
            

            foreach (float angle in angles)
            {
                arguments += "," + angle.ToString();
            }
            arguments += ")";

            this.label1.Text = arguments;

            result = matlab.Execute(arguments);
            this.label1.Text += "\n:" + result;
            
        }

        public Form1()
        {
            InitializeComponent();

            this.previous_angles = new List<float> { 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f };
        }

        private void button_go_Click(object sender, EventArgs e)
        {
            float speed = 1.0f;
            
            List<float> angles = new List<float> { -1.41f, -1.07f, 1.03f, 0.06f, -0.15f, 0.30f };
            this.posture.goToPosture("Stand", speed);
            Console.Beep();

            
            this.motion.post.moveTo(0.0f, -0.2f, 0.0f);
            Console.Beep();
            this.motion.post.moveTo(-0.3f, 0.0f, 0.0f);
            Console.Beep();
            this.motion.post.moveTo(0.3f, 0.0f, 0.0f);
            Console.Beep();
            this.motion.post.moveTo(0.0f, 0.2f, 0.0f);
            Console.Beep();
            this.motion.post.setAngles("RArm", angles, 0.4f);
            Console.Beep();
            this.motion.post.setAngles("RArm", angles, 0.4f);
            Console.Beep();
            this.motion.post.setAngles("RArm", angles, 0.4f);
            Console.Beep();
             

    


            this.posture.goToPosture("Sit", speed);
            Console.Beep();

            this.motion.setStiffnesses("Body", 0.0f);
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            string ip = this.textBox1.Text;

            this.tts = new TextToSpeechProxy(ip, 9559);
            this.motion = new MotionProxy(ip, 9559);
            this.posture = new RobotPostureProxy(ip, 9559);
            this.navigation = new NavigationProxy(ip, 9559);

            Console.Beep();
            this.label1.Text += "\nconnected.";

        }

        private void button_info_Click(object sender, EventArgs e)
        {
            //List<float> angles;

            //angles = this.motion.getAngles("RArm", true);

            //foreach (float angle in angles)
            //{
            //    this.label1.Text += "\n" + angle.ToString();
            //}

            List<float> vector = new List<float>();

            StreamReader objInput = new StreamReader("C:\\Users\\james_000\\Documents\\nao\\Debug\\SkeletonData0_000.dat", System.Text.Encoding.Default);
            string contents = objInput.ReadToEnd().Trim();
            string[] split = System.Text.RegularExpressions.Regex.Split(contents, "\\s+" );//, RegexOptions.None);
            foreach (string s in split)
            {
                label1.Text += "\n" + s;
                vector.Add(float.Parse(s));
            }

            run_frame(vector, this.previous_angles);


        }

        private void button_keyboard_KeyDown(object sender, KeyEventArgs e)
        {
            //List<float> angles = new List<float> { -1.41f, -1.07f, 1.03f, 0.06f, -0.15f, 0.30f };
            //this.motion.post.setAngles("RArm", angles, 0.4f);


            //Console.Beep();
            if (e.KeyCode == Keys.W)
            {
                //forward
                this.motion.post.moveTo(0.2f, 0.0f, 0.0f);

            }
            else if (e.KeyCode == Keys.A)
            {
                this.motion.post.moveTo(0.0f, -0.2f, 0.0f);
            }
            else if (e.KeyCode == Keys.S)
            {
                this.motion.post.moveTo(-0.2f, 0.0f, 0.0f);
            }
            else if (e.KeyCode == Keys.D)
            {
                this.motion.post.moveTo(0.0f, 0.2f, 0.0f);
            }
            else if (e.KeyCode == Keys.Q)
            {
                this.motion.post.moveTo(0.0f, 0.0f, 0.1f);
            }
            else if (e.KeyCode == Keys.E)
            {
                this.motion.post.moveTo(0.0f, 0.0f, -0.1f);
            }
            else if (e.KeyCode == Keys.Space)
            {
                this.posture.goToPosture("Stand", 1.0f);
            }
            else if (e.KeyCode == Keys.C)
            {
                this.posture.goToPosture("Sit", 1.0f);
            }
        }
    }
}
