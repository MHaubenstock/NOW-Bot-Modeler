namespace Expo_1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.button_go = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.button_info = new System.Windows.Forms.Button();
            this.button_keyboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "192.168.0.1";
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(12, 39);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 1;
            this.button_connect.Text = "connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // button_go
            // 
            this.button_go.Location = new System.Drawing.Point(12, 68);
            this.button_go.Name = "button_go";
            this.button_go.Size = new System.Drawing.Size(75, 23);
            this.button_go.TabIndex = 2;
            this.button_go.Text = "GO";
            this.button_go.UseVisualStyleBackColor = true;
            this.button_go.Click += new System.EventHandler(this.button_go_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // button_info
            // 
            this.button_info.Location = new System.Drawing.Point(12, 226);
            this.button_info.Name = "button_info";
            this.button_info.Size = new System.Drawing.Size(75, 23);
            this.button_info.TabIndex = 4;
            this.button_info.Text = "info";
            this.button_info.UseVisualStyleBackColor = true;
            this.button_info.Click += new System.EventHandler(this.button_info_Click);
            // 
            // button_keyboard
            // 
            this.button_keyboard.Location = new System.Drawing.Point(197, 226);
            this.button_keyboard.Name = "button_keyboard";
            this.button_keyboard.Size = new System.Drawing.Size(75, 23);
            this.button_keyboard.TabIndex = 5;
            this.button_keyboard.Text = "keyboard";
            this.button_keyboard.UseVisualStyleBackColor = true;
            this.button_keyboard.KeyDown += new System.Windows.Forms.KeyEventHandler(this.button_keyboard_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button_keyboard);
            this.Controls.Add(this.button_info);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_go);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Button button_go;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_info;
        private System.Windows.Forms.Button button_keyboard;
    }
}

