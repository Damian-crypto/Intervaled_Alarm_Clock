using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Alarm_Clock
{
    public partial class Form1 : Form
    {
        System.Timers.Timer timer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        delegate void UpdateLabel(Label lbl, string value);
        void UpdateDataLabel(Label lbl, string value)
        {
            lbl.Text = value;
        }

        SoundPlayer player = null;
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            DateTime userTime = dateTimePicker1.Value;
            if(currentTime.Hour == userTime.Hour && currentTime.Minute == userTime.Minute && currentTime.Second == userTime.Second)
            {
                timer.Stop();
                try
                {
                    UpdateLabel upd = UpdateDataLabel;
                    if (label1.InvokeRequired)
                        Invoke(upd, label1, "Stop");

                    player = new SoundPlayer();
                    player.SoundLocation = @"C:\Windows\Media\Alarm09.wav";
                    player.PlayLooping();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Start();
            label1.Text = "Running ...";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Close();
            if(player != null)
            {
                player.Stop();
                player = null;
            }
            label1.Text = "Stopped!";
        }

        bool end = false;
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                while(!end)
                {
                    Thread.Sleep(Convert.ToInt32(textBox1.Text) * 1000);
                    playAudio();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void playAudio()
        {
            SoundPlayer media = new SoundPlayer();
            media.SoundLocation = @"C:\Windows\Media\Alarm09.wav";
            media.PlaySync();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(!end)
                end = true;
            if(Thread.CurrentThread.ThreadState == ThreadState.Running)
                Thread.CurrentThread.Abort();
        }
    }
}
