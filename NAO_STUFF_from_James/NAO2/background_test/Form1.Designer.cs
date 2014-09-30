namespace background_test
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
            this.richTextBox_script = new System.Windows.Forms.RichTextBox();
            this.button_run = new System.Windows.Forms.Button();
            this.listView_results = new System.Windows.Forms.ListView();
            this.backgroundWorker_delayed_out = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // richTextBox_script
            // 
            this.richTextBox_script.Location = new System.Drawing.Point(87, 12);
            this.richTextBox_script.Name = "richTextBox_script";
            this.richTextBox_script.Size = new System.Drawing.Size(185, 237);
            this.richTextBox_script.TabIndex = 0;
            this.richTextBox_script.Text = "";
            // 
            // button_run
            // 
            this.button_run.Location = new System.Drawing.Point(6, 12);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 23);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "button1";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // listView_results
            // 
            this.listView_results.Location = new System.Drawing.Point(283, 14);
            this.listView_results.Name = "listView_results";
            this.listView_results.Size = new System.Drawing.Size(186, 234);
            this.listView_results.TabIndex = 2;
            this.listView_results.UseCompatibleStateImageBehavior = false;
            // 
            // backgroundWorker_delayed_out
            // 
            this.backgroundWorker_delayed_out.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_delayed_out_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 257);
            this.Controls.Add(this.listView_results);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.richTextBox_script);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_script;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.ListView listView_results;
        private System.ComponentModel.BackgroundWorker backgroundWorker_delayed_out;
    }
}

