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


namespace background_test
{
    public partial class Form1 : Form
    {
        public List<string> script;

        public Form1()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            if (this.button_run.BackColor != Color.Red)
            {
                this.button_run.BackColor = Color.Red;

                char[] delimeters = new char[] { '\n' };
                this.script = this.richTextBox_script.Text.Split(delimeters).ToList();

                //debug
                
            }
            else
            {

            }

            this.backgroundWorker_delayed_out.RunWorkerAsync();
            
        }

        private void backgroundWorker_delayed_out_DoWork(object sender, DoWorkEventArgs e)
        {

            this.listView_results.Items.Clear();
            foreach (string line in this.script)
            {
                Thread.Sleep(1000);
                this.listView_results.Items.Add(line);
            }
            this.button_run.BackColor = Color.White;

        }
    }
}
