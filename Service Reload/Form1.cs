using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;

namespace Service_Reload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ServiceController[] allservices;
        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = SystemIcons.Application;
            listBox1.Items.Clear();
            allservices = ServiceController.GetServices();
            foreach (var service in allservices)
            {
                listBox1.Items.Add(service.ServiceName);
            }
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        }
        private void RestartService(string servicename)
        {
            ServiceController serviceController = new ServiceController(servicename);
            try
            {
                if (serviceController.Status == ServiceControllerStatus.Running || serviceController.Status == ServiceControllerStatus.StartPending)
                {
                    serviceController.Stop();
                }
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running);
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.BalloonTipText = "Service reloaded!";
                notifyIcon1.ShowBalloonTip(20);
            }
            catch
            {
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon1.BalloonTipText = "Service reloading failed!";
                notifyIcon1.ShowBalloonTip(20);
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            reloadLastChosenServiceToolStripMenuItem.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceController service = new ServiceController(listBox1.SelectedItem.ToString());
                RestartService(service.ServiceName);
            }
            catch
            {
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon1.BalloonTipText = "No service chosen";
                notifyIcon1.ShowBalloonTip(20);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                listBox1.SelectedIndex = listBox1.FindString(textBox1.Text) + 10;
                listBox1.SelectedIndex = listBox1.FindString(textBox1.Text);
            }
            catch {
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = listBox1.FindString(textBox1.Text); }

            }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            Show();
            notifyIcon1.Visible = false;
            this.TopMost = true;
            TopMost = false;
        }

        private void reloadLastChosenServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                    ServiceController service = new ServiceController(listBox1.SelectedItem.ToString());
                    RestartService(service.ServiceName);          
            }
            catch
            {
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon1.BalloonTipText = "No service chosen";
                notifyIcon1.ShowBalloonTip(20);
            }
        }
    }
}
