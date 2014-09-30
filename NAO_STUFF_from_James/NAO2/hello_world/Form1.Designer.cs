namespace hello_world
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_hello = new System.Windows.Forms.Button();
            this.button_abort = new System.Windows.Forms.Button();
            this.button_toggle_Larm = new System.Windows.Forms.Button();
            this.button_toggle_Rarm = new System.Windows.Forms.Button();
            this.button_toggle_head = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_toggle_legs = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_toggle_Lhand = new System.Windows.Forms.Button();
            this.button_toggle_Rhand = new System.Windows.Forms.Button();
            this.label_raw_output = new System.Windows.Forms.Label();
            this.listView_timeline = new System.Windows.Forms.ListView();
            this.button_save_to_file = new System.Windows.Forms.Button();
            this.button_load_file = new System.Windows.Forms.Button();
            this.textBox_filename = new System.Windows.Forms.TextBox();
            this.button_stop_playback = new System.Windows.Forms.Button();
            this.button_start_playback = new System.Windows.Forms.Button();
            this.button_connect_to_IP = new System.Windows.Forms.Button();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.button_browse_open = new System.Windows.Forms.Button();
            this.openFileDialog_one = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog_one = new System.Windows.Forms.SaveFileDialog();
            this.button_browse_save = new System.Windows.Forms.Button();
            this.TextBox_min_timeline_value = new System.Windows.Forms.TextBox();
            this.TextBox_max_timeline_value = new System.Windows.Forms.TextBox();
            this.button_bake_timeline = new System.Windows.Forms.Button();
            this.textBox_FPS = new System.Windows.Forms.TextBox();
            this.label_FPS = new System.Windows.Forms.Label();
            this.background_talking_thread = new System.ComponentModel.BackgroundWorker();
            this.timer_one = new System.Windows.Forms.Timer(this.components);
            this.background_moving_thread = new System.ComponentModel.BackgroundWorker();
            this.richTextBox_script = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton_french = new System.Windows.Forms.RadioButton();
            this.radioButton_english = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_hello
            // 
            this.button_hello.Location = new System.Drawing.Point(13, 13);
            this.button_hello.Name = "button_hello";
            this.button_hello.Size = new System.Drawing.Size(75, 23);
            this.button_hello.TabIndex = 0;
            this.button_hello.Text = "hello world";
            this.button_hello.UseVisualStyleBackColor = true;
            this.button_hello.Click += new System.EventHandler(this.button_hello_Click);
            // 
            // button_abort
            // 
            this.button_abort.Cursor = System.Windows.Forms.Cursors.No;
            this.button_abort.ForeColor = System.Drawing.Color.DarkRed;
            this.button_abort.Location = new System.Drawing.Point(582, 7);
            this.button_abort.Name = "button_abort";
            this.button_abort.Size = new System.Drawing.Size(75, 23);
            this.button_abort.TabIndex = 1;
            this.button_abort.Text = "ABORT";
            this.button_abort.UseVisualStyleBackColor = true;
            this.button_abort.Click += new System.EventHandler(this.button_abort_Click);
            // 
            // button_toggle_Larm
            // 
            this.button_toggle_Larm.Location = new System.Drawing.Point(3, 74);
            this.button_toggle_Larm.Name = "button_toggle_Larm";
            this.button_toggle_Larm.Size = new System.Drawing.Size(75, 23);
            this.button_toggle_Larm.TabIndex = 2;
            this.button_toggle_Larm.Text = "Left Arm";
            this.button_toggle_Larm.UseVisualStyleBackColor = true;
            this.button_toggle_Larm.Click += new System.EventHandler(this.button_toggle_Larm_Click);
            // 
            // button_toggle_Rarm
            // 
            this.button_toggle_Rarm.Location = new System.Drawing.Point(3, 45);
            this.button_toggle_Rarm.Name = "button_toggle_Rarm";
            this.button_toggle_Rarm.Size = new System.Drawing.Size(75, 23);
            this.button_toggle_Rarm.TabIndex = 3;
            this.button_toggle_Rarm.Text = "Right Arm";
            this.button_toggle_Rarm.UseVisualStyleBackColor = true;
            this.button_toggle_Rarm.Click += new System.EventHandler(this.button_toggle_Rarm_Click);
            // 
            // button_toggle_head
            // 
            this.button_toggle_head.Location = new System.Drawing.Point(3, 16);
            this.button_toggle_head.Name = "button_toggle_head";
            this.button_toggle_head.Size = new System.Drawing.Size(75, 23);
            this.button_toggle_head.TabIndex = 4;
            this.button_toggle_head.Text = "Head";
            this.button_toggle_head.UseVisualStyleBackColor = true;
            this.button_toggle_head.Click += new System.EventHandler(this.button_toggle_head_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Set Stiffness";
            // 
            // button_toggle_legs
            // 
            this.button_toggle_legs.Location = new System.Drawing.Point(3, 103);
            this.button_toggle_legs.Name = "button_toggle_legs";
            this.button_toggle_legs.Size = new System.Drawing.Size(75, 23);
            this.button_toggle_legs.TabIndex = 6;
            this.button_toggle_legs.Text = "Lower Body";
            this.button_toggle_legs.UseVisualStyleBackColor = true;
            this.button_toggle_legs.Click += new System.EventHandler(this.button_toggle_legs_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_toggle_Lhand);
            this.panel1.Controls.Add(this.button_toggle_Rhand);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button_toggle_legs);
            this.panel1.Controls.Add(this.button_toggle_head);
            this.panel1.Controls.Add(this.button_toggle_Larm);
            this.panel1.Controls.Add(this.button_toggle_Rarm);
            this.panel1.Location = new System.Drawing.Point(13, 242);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(192, 129);
            this.panel1.TabIndex = 7;
            // 
            // button_toggle_Lhand
            // 
            this.button_toggle_Lhand.Location = new System.Drawing.Point(83, 74);
            this.button_toggle_Lhand.Name = "button_toggle_Lhand";
            this.button_toggle_Lhand.Size = new System.Drawing.Size(76, 23);
            this.button_toggle_Lhand.TabIndex = 8;
            this.button_toggle_Lhand.Text = "Left Hand";
            this.button_toggle_Lhand.UseVisualStyleBackColor = true;
            this.button_toggle_Lhand.Click += new System.EventHandler(this.button_toggle_Lhand_Click);
            // 
            // button_toggle_Rhand
            // 
            this.button_toggle_Rhand.Location = new System.Drawing.Point(84, 45);
            this.button_toggle_Rhand.Name = "button_toggle_Rhand";
            this.button_toggle_Rhand.Size = new System.Drawing.Size(75, 23);
            this.button_toggle_Rhand.TabIndex = 7;
            this.button_toggle_Rhand.Text = "Right Hand";
            this.button_toggle_Rhand.UseVisualStyleBackColor = true;
            this.button_toggle_Rhand.Click += new System.EventHandler(this.button_toggle_Rhand_Click);
            // 
            // label_raw_output
            // 
            this.label_raw_output.AutoSize = true;
            this.label_raw_output.Location = new System.Drawing.Point(93, 46);
            this.label_raw_output.Name = "label_raw_output";
            this.label_raw_output.Size = new System.Drawing.Size(35, 13);
            this.label_raw_output.TabIndex = 8;
            this.label_raw_output.Text = "label2";
            // 
            // listView_timeline
            // 
            this.listView_timeline.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listView_timeline.Location = new System.Drawing.Point(302, 322);
            this.listView_timeline.Name = "listView_timeline";
            this.listView_timeline.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.listView_timeline.Size = new System.Drawing.Size(355, 49);
            this.listView_timeline.TabIndex = 11;
            this.listView_timeline.UseCompatibleStateImageBehavior = false;
            this.listView_timeline.DoubleClick += new System.EventHandler(this.listView_timeline_DoubleClick);
            this.listView_timeline.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_timeline_KeyDown);
            this.listView_timeline.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_timeline_MouseClick);
            // 
            // button_save_to_file
            // 
            this.button_save_to_file.Location = new System.Drawing.Point(582, 121);
            this.button_save_to_file.Name = "button_save_to_file";
            this.button_save_to_file.Size = new System.Drawing.Size(75, 23);
            this.button_save_to_file.TabIndex = 14;
            this.button_save_to_file.Text = "SAVE";
            this.button_save_to_file.UseVisualStyleBackColor = true;
            this.button_save_to_file.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_save_to_file_MouseClick);
            // 
            // button_load_file
            // 
            this.button_load_file.Location = new System.Drawing.Point(582, 150);
            this.button_load_file.Name = "button_load_file";
            this.button_load_file.Size = new System.Drawing.Size(75, 23);
            this.button_load_file.TabIndex = 15;
            this.button_load_file.Text = "LOAD";
            this.button_load_file.UseVisualStyleBackColor = true;
            this.button_load_file.Click += new System.EventHandler(this.button_load_file_Click);
            // 
            // textBox_filename
            // 
            this.textBox_filename.Location = new System.Drawing.Point(582, 37);
            this.textBox_filename.Name = "textBox_filename";
            this.textBox_filename.Size = new System.Drawing.Size(75, 20);
            this.textBox_filename.TabIndex = 16;
            this.textBox_filename.Text = ".\\test.xml";
            // 
            // button_stop_playback
            // 
            this.button_stop_playback.Location = new System.Drawing.Point(211, 348);
            this.button_stop_playback.Name = "button_stop_playback";
            this.button_stop_playback.Size = new System.Drawing.Size(85, 23);
            this.button_stop_playback.TabIndex = 17;
            this.button_stop_playback.Text = "STOP/PAUSE";
            this.button_stop_playback.UseVisualStyleBackColor = true;
            // 
            // button_start_playback
            // 
            this.button_start_playback.Location = new System.Drawing.Point(211, 322);
            this.button_start_playback.Name = "button_start_playback";
            this.button_start_playback.Size = new System.Drawing.Size(85, 23);
            this.button_start_playback.TabIndex = 18;
            this.button_start_playback.Text = "PLAY";
            this.button_start_playback.UseVisualStyleBackColor = true;
            this.button_start_playback.Click += new System.EventHandler(this.button_start_playback_Click);
            // 
            // button_connect_to_IP
            // 
            this.button_connect_to_IP.Location = new System.Drawing.Point(211, 13);
            this.button_connect_to_IP.Name = "button_connect_to_IP";
            this.button_connect_to_IP.Size = new System.Drawing.Size(75, 23);
            this.button_connect_to_IP.TabIndex = 19;
            this.button_connect_to_IP.Text = "Connect";
            this.button_connect_to_IP.UseVisualStyleBackColor = true;
            this.button_connect_to_IP.Click += new System.EventHandler(this.button_connect_to_IP_Click);
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(292, 15);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(173, 20);
            this.textBox_IP.TabIndex = 20;
            this.textBox_IP.Text = "169.254.213.252";
            // 
            // button_browse_open
            // 
            this.button_browse_open.Location = new System.Drawing.Point(582, 63);
            this.button_browse_open.Name = "button_browse_open";
            this.button_browse_open.Size = new System.Drawing.Size(75, 23);
            this.button_browse_open.TabIndex = 22;
            this.button_browse_open.Text = "find file...";
            this.button_browse_open.UseVisualStyleBackColor = true;
            this.button_browse_open.Click += new System.EventHandler(this.button_browse_files_Click);
            // 
            // openFileDialog_one
            // 
            this.openFileDialog_one.FileName = "openFileDialog1";
            // 
            // button_browse_save
            // 
            this.button_browse_save.Location = new System.Drawing.Point(582, 92);
            this.button_browse_save.Name = "button_browse_save";
            this.button_browse_save.Size = new System.Drawing.Size(75, 23);
            this.button_browse_save.TabIndex = 23;
            this.button_browse_save.Text = "save as...";
            this.button_browse_save.UseVisualStyleBackColor = true;
            this.button_browse_save.Click += new System.EventHandler(this.button_browse_save_Click);
            // 
            // TextBox_min_timeline_value
            // 
            this.TextBox_min_timeline_value.Location = new System.Drawing.Point(302, 296);
            this.TextBox_min_timeline_value.Name = "TextBox_min_timeline_value";
            this.TextBox_min_timeline_value.Size = new System.Drawing.Size(100, 20);
            this.TextBox_min_timeline_value.TabIndex = 24;
            this.TextBox_min_timeline_value.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_min_timeline_value_KeyDown);
            // 
            // TextBox_max_timeline_value
            // 
            this.TextBox_max_timeline_value.Location = new System.Drawing.Point(555, 296);
            this.TextBox_max_timeline_value.Name = "TextBox_max_timeline_value";
            this.TextBox_max_timeline_value.Size = new System.Drawing.Size(102, 20);
            this.TextBox_max_timeline_value.TabIndex = 25;
            this.TextBox_max_timeline_value.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_max_timeline_value_KeyDown);
            // 
            // button_bake_timeline
            // 
            this.button_bake_timeline.Location = new System.Drawing.Point(433, 293);
            this.button_bake_timeline.Name = "button_bake_timeline";
            this.button_bake_timeline.Size = new System.Drawing.Size(90, 23);
            this.button_bake_timeline.TabIndex = 26;
            this.button_bake_timeline.Text = "bake timeline";
            this.button_bake_timeline.UseVisualStyleBackColor = true;
            this.button_bake_timeline.Click += new System.EventHandler(this.button_bake_timeline_Click);
            // 
            // textBox_FPS
            // 
            this.textBox_FPS.Location = new System.Drawing.Point(592, 239);
            this.textBox_FPS.Name = "textBox_FPS";
            this.textBox_FPS.Size = new System.Drawing.Size(65, 20);
            this.textBox_FPS.TabIndex = 27;
            this.textBox_FPS.Text = "0";
            this.textBox_FPS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_FPS_KeyDown);
            // 
            // label_FPS
            // 
            this.label_FPS.AutoSize = true;
            this.label_FPS.Location = new System.Drawing.Point(589, 223);
            this.label_FPS.Name = "label_FPS";
            this.label_FPS.Size = new System.Drawing.Size(27, 13);
            this.label_FPS.TabIndex = 28;
            this.label_FPS.Text = "FPS";
            // 
            // background_talking_thread
            // 
            this.background_talking_thread.WorkerSupportsCancellation = true;
            this.background_talking_thread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.background_talking_thread_DoWork);
            // 
            // background_moving_thread
            // 
            this.background_moving_thread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.background_moving_thread_DoWork);
            // 
            // richTextBox_script
            // 
            this.richTextBox_script.Location = new System.Drawing.Point(211, 46);
            this.richTextBox_script.Name = "richTextBox_script";
            this.richTextBox_script.Size = new System.Drawing.Size(354, 241);
            this.richTextBox_script.TabIndex = 29;
            this.richTextBox_script.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButton_french);
            this.panel2.Controls.Add(this.radioButton_english);
            this.panel2.Location = new System.Drawing.Point(12, 121);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(193, 103);
            this.panel2.TabIndex = 30;
            // 
            // radioButton_french
            // 
            this.radioButton_french.AutoSize = true;
            this.radioButton_french.Location = new System.Drawing.Point(7, 29);
            this.radioButton_french.Name = "radioButton_french";
            this.radioButton_french.Size = new System.Drawing.Size(55, 17);
            this.radioButton_french.TabIndex = 1;
            this.radioButton_french.TabStop = true;
            this.radioButton_french.Text = "french";
            this.radioButton_french.UseVisualStyleBackColor = true;
            this.radioButton_french.Click += new System.EventHandler(this.radioButton_french_Click);
            // 
            // radioButton_english
            // 
            this.radioButton_english.AutoSize = true;
            this.radioButton_english.Location = new System.Drawing.Point(7, 5);
            this.radioButton_english.Name = "radioButton_english";
            this.radioButton_english.Size = new System.Drawing.Size(58, 17);
            this.radioButton_english.TabIndex = 0;
            this.radioButton_english.TabStop = true;
            this.radioButton_english.Text = "english";
            this.radioButton_english.UseVisualStyleBackColor = true;
            this.radioButton_english.Click += new System.EventHandler(this.radioButton_english_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(669, 383);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.richTextBox_script);
            this.Controls.Add(this.label_FPS);
            this.Controls.Add(this.textBox_FPS);
            this.Controls.Add(this.button_bake_timeline);
            this.Controls.Add(this.TextBox_max_timeline_value);
            this.Controls.Add(this.TextBox_min_timeline_value);
            this.Controls.Add(this.button_browse_save);
            this.Controls.Add(this.button_browse_open);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.button_connect_to_IP);
            this.Controls.Add(this.button_start_playback);
            this.Controls.Add(this.button_stop_playback);
            this.Controls.Add(this.textBox_filename);
            this.Controls.Add(this.button_load_file);
            this.Controls.Add(this.button_save_to_file);
            this.Controls.Add(this.listView_timeline);
            this.Controls.Add(this.label_raw_output);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_abort);
            this.Controls.Add(this.button_hello);
            this.Name = "Form1";
            this.Text = "NAO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_hello;
        private System.Windows.Forms.Button button_abort;
        private System.Windows.Forms.Button button_toggle_Larm;
        private System.Windows.Forms.Button button_toggle_Rarm;
        private System.Windows.Forms.Button button_toggle_head;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_toggle_legs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_raw_output;
        private System.Windows.Forms.ListView listView_timeline;
        private System.Windows.Forms.Button button_save_to_file;
        private System.Windows.Forms.Button button_load_file;
        private System.Windows.Forms.TextBox textBox_filename;
        private System.Windows.Forms.Button button_stop_playback;
        private System.Windows.Forms.Button button_start_playback;
        private System.Windows.Forms.Button button_connect_to_IP;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.Button button_browse_open;
        private System.Windows.Forms.OpenFileDialog openFileDialog_one;
        private System.Windows.Forms.SaveFileDialog saveFileDialog_one;
        private System.Windows.Forms.Button button_browse_save;
        private System.Windows.Forms.TextBox TextBox_min_timeline_value;
        private System.Windows.Forms.TextBox TextBox_max_timeline_value;
        private System.Windows.Forms.Button button_bake_timeline;
        private System.Windows.Forms.TextBox textBox_FPS;
        private System.Windows.Forms.Label label_FPS;
        private System.Windows.Forms.Button button_toggle_Lhand;
        private System.Windows.Forms.Button button_toggle_Rhand;
        private System.ComponentModel.BackgroundWorker background_talking_thread;
        private System.Windows.Forms.Timer timer_one;
        private System.ComponentModel.BackgroundWorker background_moving_thread;
        private System.Windows.Forms.RichTextBox richTextBox_script;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButton_french;
        private System.Windows.Forms.RadioButton radioButton_english;
    }
}

