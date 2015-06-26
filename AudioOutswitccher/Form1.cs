
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioOutswitccher
{
    public partial class Form1 : Form
    {
        string[] devices;

        public Form1()
        {
            InitializeComponent();

            



        }

        private void Form1_Load(object sender, EventArgs e)
        {         

            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "EndPointController.exe";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;

            p.StartInfo = startInfo;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            devices = output.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);


            foreach (string dev in devices)
            {
                listBox1.Items.Add(dev);
            }
           
        
        
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "EndPointController.exe";
            startInfo.Arguments = listBox1.SelectedIndex.ToString();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;

            p.StartInfo = startInfo;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            if (p.ExitCode == 0)
            {
                label1.Text = devices[listBox1.SelectedIndex];
            }

        }
    }
}
