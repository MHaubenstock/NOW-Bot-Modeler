


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//NAO
using Aldebaran.Proxies;
//

//XML
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
//

namespace hello_world
{
    

    public partial class Form1 : Form
    {
        public TextToSpeechProxy tts;
        public MotionProxy motion;


        public bool head_is_stiff = false;
        public bool Rarm_is_stiff = false;
        public bool Larm_is_stiff = false;
        public bool legs_is_stiff = false;

        public instance current_instance;

        

        public Form1()
        {
            InitializeComponent();
            this.current_instance = new instance();
            /*
            this.tts = new TextToSpeechProxy("169.254.226.148", 9559);
            this.motion = new MotionProxy("169.254.226.148", 9559);
            this.motion.setStiffnesses("Body", 0);

            this.label_raw_output.Text = "...";
             */
            this.BackColor = Color.Red;
            this.TextBox_max_timeline_value.Text = this.current_instance.timeline_end_frame.ToString();
            this.TextBox_min_timeline_value.Text = this.current_instance.timeline_start_frame.ToString();
            this.textBox_FPS.Text = this.current_instance.frames_per_second.ToString();

            for (int i = this.current_instance.timeline_start_frame ; i < this.current_instance.timeline_end_frame ; i = i + 1)
            {
                this.listView_timeline.Items.Add(i.ToString());
            }
        }

        private void button_hello_Click(object sender, EventArgs e)
        {
            /*
            this.tts.setVoice("Kenny22Enhanced");
            this.tts.setVoice("Julie22Enhanced");
             * 
            this.tts.setVolume(1.0F);
            this.tts.say("Hello World.");
             */

            Console.Beep();

            foreach (string line in this.tts.getAvailableVoices())
            {
                this.label_raw_output.Text += "\n" + line;
            }
            



            //RobotPostureProxy posture = new RobotPostureProxy(this.textBox_IP.Text, 9559);
            //posture.goToPosture("Stand", 0.1f);

            motion.moveTo(0.1f, 0f, 0f);
            

        }

        private void button_abort_Click(object sender, EventArgs e)
        {
            this.motion.killAll();
            this.tts.stopAll();
            this.motion.setStiffnesses("Body", 0);
            this.button_toggle_Rarm.BackColor = Color.White;
            this.button_toggle_Larm.BackColor = Color.White;
            this.button_toggle_head.BackColor = Color.White;
            this.button_toggle_legs.BackColor = Color.White;
            this.button_toggle_Lhand.BackColor = Color.White;
            this.button_toggle_Rhand.BackColor = Color.White;

            this.background_talking_thread.CancelAsync();
            //this.background_talking_thread.Dispose();
        }

        private void button_toggle_head_Click(object sender, EventArgs e)
        {
            if (this.head_is_stiff)
            {
                this.head_is_stiff = false;
                this.motion.setStiffnesses("Head", 0);
                this.button_toggle_head.BackColor = Color.White;
            }
            else
            {
                this.head_is_stiff = true;
                this.motion.setStiffnesses("Head", 1);
                this.button_toggle_head.BackColor = Color.Red;
            }
        }

        private void button_toggle_Rarm_Click(object sender, EventArgs e)
        {
            if (this.Rarm_is_stiff)
            {
                this.Rarm_is_stiff = false;
                this.motion.setStiffnesses("RArm", 0);
                this.button_toggle_Rarm.BackColor = Color.White;
            }
            else
            {
                this.Rarm_is_stiff = true;
                this.motion.setStiffnesses("RArm", 1);
                this.button_toggle_Rarm.BackColor = Color.Red;
            }
        }

        private void button_toggle_Larm_Click(object sender, EventArgs e)
        {
            if (this.Larm_is_stiff)
            {
                this.Larm_is_stiff = false;
                this.motion.setStiffnesses("LArm", 0);
                this.button_toggle_Larm.BackColor = Color.White;
            }
            else
            {
                this.Larm_is_stiff = true;
                this.motion.setStiffnesses("LArm", 1);
                this.button_toggle_Larm.BackColor = Color.Red;
            }
        }

        private void button_toggle_legs_Click(object sender, EventArgs e)
        {
            if (this.legs_is_stiff)
            {
                this.legs_is_stiff = false;
                this.motion.setStiffnesses("Legs", 0);
                this.button_toggle_legs.BackColor = Color.White;
            }
            else
            {
                this.legs_is_stiff = true;
                this.motion.setStiffnesses("Legs", 1);
                this.button_toggle_legs.BackColor = Color.Red;
            }
        }

        private void listView_timeline_DoubleClick(object sender, EventArgs e)
        {
            //if key does not exist in dict:
            if (!this.current_instance.dict_timeline_script.ContainsKey(this.listView_timeline.FocusedItem.Text))
            {
                this.listView_timeline.FocusedItem.Font = new Font(this.listView_timeline.FocusedItem.Font,
                this.listView_timeline.FocusedItem.Font.Style | FontStyle.Bold | FontStyle.Underline);

                this.current_instance.dict_timeline_script.Add(this.listView_timeline.FocusedItem.Text, new pose());

              
                //add description
                this.current_instance.dict_timeline_script[this.listView_timeline.FocusedItem.Text].description = "description: " + this.listView_timeline.FocusedItem.Text;
                
                //add angles
                //this.current_instance.dict_timeline_script[this.listView_timeline.FocusedItem.Text].angles.Add(10.0f);
                this.current_instance.dict_timeline_script[this.listView_timeline.FocusedItem.Text].angles = this.motion.getAngles("Body", true);

                //rich debug info
                this.label_raw_output.Text += this.current_instance.dict_timeline_script[this.listView_timeline.FocusedItem.Text].description + " t:" + this.current_instance.dict_timeline_script[this.listView_timeline.FocusedItem.Text].angles[0];
               
            }
            else
            {
                this.label_raw_output.Text = "EXIST";

                //move to the position
                List<float> timing = new List<float>();
                for (int i = 0; i < this.current_instance.dict_timeline_script[this.listView_timeline.FocusedItem.Text].angles.Count(); ++i)
                {
                    timing.Add(1.0f);
                }

                this.motion.angleInterpolation("Body" , this.current_instance.dict_timeline_script[this.listView_timeline.FocusedItem.Text].angles, timing , true); 
            }
        }

        private void listView_timeline_MouseClick(object sender, MouseEventArgs e)
        {
            //DEBUG INFO
            //this.label_raw_output.Text = this.listView_timeline.FocusedItem.Text;
            this.label_raw_output.Text = "item selected: ";
            this.label_raw_output.Text += this.listView_timeline.FocusedItem.Text + "\n";
            //this.label_raw_output.Text += this.dict_timeline_script["2"].description;
            //END

            //if dict contains key
            if (this.current_instance.dict_timeline_script.ContainsKey(this.listView_timeline.FocusedItem.Text))
            {
                this.label_raw_output.Text += this.current_instance.dict_timeline_script[this.listView_timeline.FocusedItem.Text].description;
            }


        }

        private void button_save_to_file_MouseClick(object sender, MouseEventArgs e)
        {
            string file_name = this.textBox_filename.Text;

            this.current_instance.script = this.richTextBox_script.Text;

            //write to XML:
            this.current_instance.dict_to_list(true);

            //DEBUG OUT
            foreach (string key in this.current_instance.keys)
            {
                this.label_raw_output.Text += key;
            }
            foreach (pose my_pose in this.current_instance.poses)
            {
                this.label_raw_output.Text += my_pose.description;
            }
            //DEBUG OUT END

            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(instance));

            System.IO.StreamWriter file_o = new System.IO.StreamWriter(
                @file_name);
            writer.Serialize(file_o, this.current_instance);
            file_o.Close();
             
        }

        private void button_load_file_Click(object sender, EventArgs e)
        {
            
            
            string file_name = this.textBox_filename.Text;
            //now read from XML:

            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(instance));

            System.IO.StreamReader file_i = new System.IO.StreamReader(
                @file_name);
            this.current_instance = (instance)reader.Deserialize(file_i);
            file_i.Close();

            this.current_instance.dict_to_list(false);

            //FILE IS LOADED, FILL FEILDS PROPERLY
            this.textBox_FPS.Text = this.current_instance.frames_per_second.ToString();
            this.TextBox_max_timeline_value.Text = this.current_instance.timeline_end_frame.ToString();
            this.TextBox_min_timeline_value.Text = this.current_instance.timeline_start_frame.ToString();

            this.listView_timeline.Items.Clear();
            for (int i = this.current_instance.timeline_start_frame; i < this.current_instance.timeline_end_frame; i = i + 1)
            {
                this.listView_timeline.Items.Add(i.ToString());
            }

            //bold the active keyframes
            for (int i = 0; i < this.listView_timeline.Items.Count ; ++i)
            {
                if (this.current_instance.keys.Contains(this.listView_timeline.Items[i].Text))
                {
                    this.listView_timeline.Items[i].Font = new Font(this.listView_timeline.Items[i].Font,
                    this.listView_timeline.Items[i].Font.Style | FontStyle.Bold | FontStyle.Underline);
                }
            }

            this.richTextBox_script.Text = this.current_instance.script;
        }

        private void button_connect_to_IP_Click(object sender, EventArgs e)
        {



            this.tts = new TextToSpeechProxy(this.textBox_IP.Text, 9559);
            this.motion = new MotionProxy(this.textBox_IP.Text, 9559);


            this.BackColor = Color.AliceBlue;
            this.tts.say("connected.");
            this.motion.setStiffnesses("Body", 0);
        }

        private void button_start_playback_Click(object sender, EventArgs e)
        {
            this.button_start_playback.BackColor = Color.Blue;

            this.label_raw_output.Text = this.richTextBox_script.Text;
            this.current_instance.populate_script( this.richTextBox_script.Text );
            //now we should have a list of timing info and a list
            //of poses to do our thing with.

            
            /*
            //DEBUG INFO
            for (int i = 0; i < this.current_instance.timing_for_interpolation[0].Count; ++i)
            {
                label_raw_output.Text +=
                    this.current_instance.timing_for_interpolation[0][i].ToString() + " : ";
                label_raw_output.Text +=
                    this.current_instance.angles_for_interpolation[0][i].ToString() + "\n";

            }
             */
            
            //movement


            //voices
            this.background_talking_thread.CancelAsync();
            this.tts.setVolume(this.current_instance.volume);
            this.background_talking_thread.RunWorkerAsync();

            this.motion.post.angleInterpolation("Body", this.current_instance.angles_for_interpolation,
                    this.current_instance.timing_for_interpolation, true);
            this.button_start_playback.BackColor = Color.White;


            /*
            this.no_interpolation();
            this.button_start_playback.BackColor = Color.White;
            */

        }

        private void button_browse_files_Click(object sender, EventArgs e)
        {
            this.openFileDialog_one.ShowDialog();

            this.textBox_filename.Text = this.openFileDialog_one.FileName;
        }

        private void button_browse_save_Click(object sender, EventArgs e)
        {
     
            this.saveFileDialog_one.ShowDialog();

            this.label_raw_output.Text = this.saveFileDialog_one.FileName;

            this.textBox_filename.Text = this.saveFileDialog_one.FileName;
        }

        

        private void listView_timeline_KeyDown(object sender, KeyEventArgs e)
        {
            this.label_raw_output.Text = "ha.";
            if (e.KeyCode == Keys.Delete)
            {
                this.label_raw_output.Text = "DELETE.";
                this.listView_timeline.FocusedItem.Font = new Font(this.listView_timeline.FocusedItem.Font,
                 FontStyle.Regular);

                //remove pose.
                this.current_instance.dict_timeline_script.Remove(this.listView_timeline.FocusedItem.Text);
            }
            else if (e.KeyCode == Keys.Space)
            {
                this.label_raw_output.Text = "SPACE.";
            }
        }


        private void TextBox_min_timeline_value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int new_start = Convert.ToInt32(this.TextBox_min_timeline_value.Text);

                if (new_start > 0 && new_start < this.current_instance.timeline_end_frame)
                {
                    this.current_instance.timeline_start_frame = new_start;
                }
                else
                {
                    this.TextBox_min_timeline_value.Text = this.current_instance.timeline_start_frame.ToString();
                }

                //Reefill the timeline
                this.listView_timeline.Items.Clear();
                for (int i = this.current_instance.timeline_start_frame; i < this.current_instance.timeline_end_frame; i = i + 1)
                {
                    this.listView_timeline.Items.Add(i.ToString());
                }

                //bold the active keyframes
                for (int i = 0; i < this.listView_timeline.Items.Count; ++i)
                {
                    if (this.current_instance.keys.Contains(this.listView_timeline.Items[i].Text))
                    {
                        this.listView_timeline.Items[i].Font = new Font(this.listView_timeline.Items[i].Font,
                        this.listView_timeline.Items[i].Font.Style | FontStyle.Bold | FontStyle.Underline);
                    }
                }
            }
        }

        private void TextBox_max_timeline_value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int new_end = Convert.ToInt32(this.TextBox_max_timeline_value.Text);

                if (new_end > this.current_instance.timeline_start_frame)
                {
                    this.current_instance.timeline_end_frame = new_end;
                }
                else
                {
                    this.TextBox_max_timeline_value.Text = this.current_instance.timeline_end_frame.ToString();
                }

                //Reefill the timeline
                this.listView_timeline.Items.Clear();
                for (int i = this.current_instance.timeline_start_frame; i < this.current_instance.timeline_end_frame; i = i + 1)
                {
                    this.listView_timeline.Items.Add(i.ToString());
                }

                //bold the active keyframes
                for (int i = 0; i < this.listView_timeline.Items.Count; ++i)
                {
                    if (this.current_instance.keys.Contains(this.listView_timeline.Items[i].Text))
                    {
                        this.listView_timeline.Items[i].Font = new Font(this.listView_timeline.Items[i].Font,
                        this.listView_timeline.Items[i].Font.Style | FontStyle.Bold | FontStyle.Underline);
                    }
                }
            }
        }

        private void button_bake_timeline_Click(object sender, EventArgs e)
        {
            List<string> possible_keys = new List<string>();
            for(int i = 0 ; i < this.listView_timeline.Items.Count; ++i)
            {
                possible_keys.Add(this.listView_timeline.Items[i].Text);
            }

            List<string> current_keys =new List<string>(this.current_instance.dict_timeline_script.Keys);
            int index = 0;
            foreach (string key in current_keys)
            {
                if (!possible_keys.Contains(key))
                {
                    this.current_instance.dict_timeline_script.Remove(key);

                    index = this.current_instance.keys.IndexOf(key);
                    this.current_instance.keys.RemoveAt(index);
                    this.current_instance.poses.RemoveAt(index);
                    
                }
            }


        }

        private void textBox_FPS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Convert.ToInt32(this.textBox_FPS.Text) <= 0)
                {
                    this.textBox_FPS.Text = this.current_instance.frames_per_second.ToString();
                }
                else
                {
                    this.current_instance.frames_per_second = Convert.ToInt32(this.textBox_FPS.Text);
                }
            }
        }

        private void button_toggle_Rhand_Click(object sender, EventArgs e)
        {
            if (this.button_toggle_Rhand.BackColor != Color.Red)
            {
                this.button_toggle_Rhand.BackColor = Color.Red;
                //motion.openHand("RHand");
                

                this.motion.setAngles("RHand", 1.0f, 0.1f);
                this.motion.setStiffnesses("RHand", 0.0f);
                this.label_raw_output.Text = motion.getAngles("RHand", true).Count().ToString();
                this.label_raw_output.Text += "\n" +  motion.getAngles("RHand",true)[0].ToString();
                //this.motion.angleInterpolation( "RHand" , 
            }
            else
            {
                this.button_toggle_Rhand.BackColor = Color.White;
                //motion.closeHand("RHand");

                this.motion.setStiffnesses("RHand", 1.0f);
                this.motion.setAngles("RHand", 0.0f, 0.1f);
                
                this.label_raw_output.Text = motion.getAngles("RHand", true).Count().ToString();
                this.label_raw_output.Text += "\n" + motion.getAngles("RHand", true)[0].ToString();
            }
        }

        private void button_toggle_Lhand_Click(object sender, EventArgs e)
        {
            if (this.button_toggle_Lhand.BackColor != Color.Red)
            {
                this.button_toggle_Lhand.BackColor = Color.Red;
                
                this.motion.setAngles("LHand", 1.0f, 0.1f);
                this.motion.setStiffnesses("LHand", 0.0f);
                //motion.openHand("LHand");
            }
            else
            {
                this.button_toggle_Lhand.BackColor = Color.White;
                this.motion.setStiffnesses("LHand", 1.0f);
                this.motion.setAngles("LHand", 0.0f, 0.1f);
                
                //motion.closeHand("LHand");
            }
        }

       

        private void background_talking_thread_DoWork(object sender, DoWorkEventArgs e)
        {
            //this.tts.post.say(this.current_instance.times_to_say.Count.ToString());



            //this.label_raw_output.Text += "TALK:";
            Console.Beep();

            //this.label_raw_output.Text += "TALK:";
            //this.label_raw_output.Text += "\n" + this.current_instance.times_to_say.Count.ToString()
            //    + " : " + this.current_instance.times_to_say.Count.ToString();

            for (int i = 0; i < this.current_instance.times_to_say.Count(); ++i)
            {
                //this.label_raw_output.Text += "\n" + ((int)(this.current_instance.times_to_say[i] * 1000.0f)).ToString() + " : ";
                //this.label_raw_output.Text += this.current_instance.things_to_say[i];
            }

            if (this.current_instance.times_to_say.Count() > 0)
            {
                //Thread.Sleep((int)(this.current_instance.times_to_say[0] * 1000.0f));
                //this.tts.stopAll();
                
                //Console.Beep();
                Thread.Sleep((int)((this.current_instance.times_to_say[0]  )* 1000.0f ));/// (float)(this.current_instance.frames_per_second)));
                this.tts.post.say(this.current_instance.things_to_say[0]);
                //this.tts.post.say(((int)this.current_instance.times_to_say[0]).ToString());
                

                for (int i = 1; i < this.current_instance.times_to_say.Count(); ++i)
                {
                    //this.tts.stopAll();
                    
                    //this.label_raw_output.Text = (this.current_instance.things_to_say[i]);
                    //Console.Beep();
                    //Thread.Sleep((int)((this.current_instance.times_to_say[i] - this.current_instance.times_to_say[i - 1]) * 1000.0f));
                    Thread.Sleep((int)((this.current_instance.times_to_say[i] - this.current_instance.times_to_say[i - 1])* 1000.0f ));// / (float)(this.current_instance.frames_per_second)));
                    this.tts.post.say(this.current_instance.things_to_say[i]);
                    //this.tts.post.say(this.current_instance.things_to_say[i]);
                }
            }

            Console.Beep();
            Console.Beep();


            
        }

        public void no_interpolation()
        {
            this.label_raw_output.Text = "MODE = NO INTERPOLATION.";
            this.background_moving_thread.RunWorkerAsync();
            this.background_talking_thread.RunWorkerAsync();
        }

        private void background_moving_thread_DoWork(object sender, DoWorkEventArgs e)
        {
            //debug out
            if (this.current_instance.poses.Count() > 0){
                this.label_raw_output.Text += " \n " + ((int)(float.Parse(this.current_instance.keys[0]) * 1000.0f)).ToString();
                for (int i = 1; i < this.current_instance.poses.Count(); ++i){
                    this.label_raw_output.Text += " : " + ((int)((float.Parse(this.current_instance.keys[i]) - float.Parse(this.current_instance.keys[i - 1])) * 1000.0f)).ToString();
                }
            }

            Console.Beep();
            Console.Beep();

            if (this.current_instance.poses.Count() > 0)
            {
                this.motion.setAngles("Body" , this.current_instance.poses[0].angles , 0.2f );
                
                //this.tts.post.stopAll();
                Console.Beep();
                this.tts.post.say(this.current_instance.things_to_say[0]);

                Thread.Sleep((int)((float.Parse(this.current_instance.keys[0])) * 1000.0f / (float)(this.current_instance.frames_per_second)));


                for (int i = 1; i < this.current_instance.poses.Count(); ++i)
                {
                    this.motion.setAngles("Body" , this.current_instance.poses[i].angles , 0.2f );
                    
                    //this.tts.post.stopAll();
                    Console.Beep();
                    this.tts.post.say(this.current_instance.things_to_say[i]);

                    Thread.Sleep((int)((float.Parse(this.current_instance.keys[i]) - float.Parse(this.current_instance.keys[i - 1])) * 1000.0f / (float)(this.current_instance.frames_per_second)));
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void radioButton_english_Click(object sender, EventArgs e)
        {
            this.tts.setVoice("Kenny22Enhanced");
        }

        private void radioButton_french_Click(object sender, EventArgs e)
        {
            this.tts.setVoice("Julie22Enhanced");
        }
        

        

        


    }

    [Serializable]
    public class pose
    {
        public string say = null;
        public float delay = 0.0F;
        public string description = "NO_DESCRIPTION";
        
        
        public List<float> angles;

        public pose()
        {
            this.angles = new List<float>();
        }
    }

    [Serializable]
    public class instance
    {
        //TIMELINE CONTROLS
        // 2 frames per second
        public int timeline_start_frame = 1;
        public int timeline_end_frame = 121;
        public int frames_per_second = 2;

        //for storage
        public List<string> keys;
        public List<pose> poses;
        public float volume = 1;
        public string script;


        //for use in interpolation playback
        public List<List<float>> angles_for_interpolation;
        public List<List<float>> timing_for_interpolation;
        public List<string> things_to_say;
        public List<float> times_to_say;

        public List<float> times_to_do;
        public List<string> things_to_do;

        public instance()
        {
            keys = new List<string>();
            poses = new List<pose>();
           
        }

        public void populate_script( string speech_script = "" ) //for plaback interpolation
        {
            //exclusively used to populate the lists
            //for keyframe playback

            //prepare data and instatiate the list
            this.dict_to_list(true);
            this.angles_for_interpolation = new List<List<float>>();
            this.timing_for_interpolation = new List<List<float>>();

            //BEGIN CHUNCK
            //add the timing info in
            if ( this.poses.Count == 0)
            {
                return;
            }

            this.timing_for_interpolation.Add(new List<float>());
            foreach (string frame in this.keys) //frame is a string
            {
                this.timing_for_interpolation[0].Add(float.Parse(frame) / this.frames_per_second);
            }

            //there is one timing item in the list, now multiply it!
            for (int i = 1; i < this.poses[0].angles.Count(); ++i)
            {
                this.timing_for_interpolation.Add(this.timing_for_interpolation[0]);
            }
            //END CHUNK

            //BEGIN CHUNK
            //instantiate the sub list
            for (int i = 0; i < this.poses[0].angles.Count(); ++i)
            {
                this.angles_for_interpolation.Add(new List<float>());
            }

            //add the angle (posing) info in
            foreach (pose pose_n in this.poses)
            {
                for ( int i = 0 ; i < pose_n.angles.Count ; ++i )
                {
                    this.angles_for_interpolation[i].Add(pose_n.angles[i]);
                }
            }
            //END CHUNK

            read_speech_script(speech_script);


        }

        public void read_speech_script(string speech_script = "")
        {
            

            //BEGIN CHUNK
            //VOICE BUISINESS:
            /*
            this.things_to_say = new List<string>();
            this.times_to_say = new List<float>();
            foreach (string key in this.keys)
            {
                if (this.dict_timeline_script[key].say == null)
                {
                    this.things_to_say.Add(key);
                    this.times_to_say.Add((float.Parse(key) + this.dict_timeline_script[key].delay) / (float)this.frames_per_second);

                }
            }
             */
            //END CHUNK
            this.things_to_say = new List<string>();
            this.times_to_say = new List<float>();

            List<string> script = speech_script.Split('\n').ToList();
            List<string> words;

            foreach (string line in script)
            {
                words = line.Split(' ').ToList();


                if (words.Count() >= 3)
                {
                    //set commands
                    if (words[0] == "set")
                    {
                        if (words[1] == "volume")
                        {
                            this.volume = float.Parse(words[2]);
                        }
                    }

                    //at commands
                    if (words[0] == "at") //at a time
                    {
                        float time = float.Parse(words[1]);

                        if (words[2] == "say") //say at time, rest
                        {
                            this.times_to_say.Add(time);
                            words.RemoveRange(0, 3);
                            this.things_to_say.Add( string.Join(" " , words) );
                            
                        }
                        else if (words[2] == "english")
                        {
                            this.times_to_do.Add(time);
                            this.things_to_do.Add("set_english");
                        }
                        else if (words[2] == "french")
                        {
                            this.times_to_do.Add(time);
                            this.things_to_do.Add("set_french");
                        }
                    }
                }
            }

            //this.things_to_say[this.things_to_say.Count() - 1] = speech_script;
        }

        public void dict_to_list(bool forward = true)
        {

            if (forward)
            {
                this.keys = new List<string>();
                List<float> keys_float = new List<float>();

                poses = new List<pose>();


                this.keys = this.dict_timeline_script.Keys.ToList();
                //SORT KEYS (THE BAD WAY)
                foreach (string key in this.keys)
                {
                    keys_float.Add(float.Parse(key));
                }
                keys_float.Sort();
                this.keys = new List<string>();
                foreach (float key in keys_float)
                {
                    this.keys.Add(key.ToString());
                }
                //ENDED SORT
                
                foreach (string key in this.keys)
                {
                    //ADD THE POSES IN
                    this.poses.Add(this.dict_timeline_script[key]);

                
                }
                
                //this.poses = this.dict_timeline_script.Values.ToList();
            }
            else
            {
                for (int i = 0; i < this.keys.Count(); i = i + 1)
                {
                    this.dict_timeline_script.Add(this.keys[i], this.poses[i]);
                }
            }

        }

        [XmlIgnore]
        public Dictionary<string, pose> dict_timeline_script =
            new Dictionary<string, pose>();

        
        
    }

     
}

/*
        private void hScrollBar_timeline_Scroll(object sender, ScrollEventArgs e)
        {
            this.label_raw_output.Text = this.hScrollBar_timeline.Value.ToString();
            this.listView_timeline.Items.Add(this.hScrollBar_timeline.Value.ToString());
            this.listView_timeline.Items[0].Font = new Font(this.listView_timeline.Items[0].Font,
                this.listView_timeline.Items[0].Font.Style | FontStyle.Bold);
        }
         */

/*
 //write:
            int example_int = 5;
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(int));

            System.IO.StreamWriter file_o = new System.IO.StreamWriter(
                @".\test.xml");
            writer.Serialize(file_o, example_int);
            file_o.Close();




            //now read:
            example_int = 0;

            System.IO.StreamReader file_i = new System.IO.StreamReader(
                @".\test.xml");
            example_int = (int)writer.Deserialize(file_i);
            this.label_raw_output.Text = example_int.ToString();
            file_i.Close();
*/