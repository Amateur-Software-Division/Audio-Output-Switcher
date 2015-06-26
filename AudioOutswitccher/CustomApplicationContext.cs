using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using CoreAudioApi;




namespace AudioOutSwitcher
{

    /// <summary>
    /// Framework for running application as a tray app.
    /// </summary>
    /// <remarks>
    /// Tray app code adapted from "Creating Applications with NotifyIcon in Windows Forms", Jessica Fosler,
    /// http://windowsclient.net/articles/notifyiconapplications.aspx
    /// </remarks>
    public class CustomApplicationContext : ApplicationContext
    {
    
        private static readonly string IconFileName = "audio switcher.ico";
        private static readonly string DefaultTooltip = "Audio Output switcher";
     
        audioSwitch switcher;
        Timer timer;
        /// <summary>
		/// This class should be created and passed into Application.Run( ... )
		/// </summary>
		public CustomApplicationContext() 
		{
            InitializeContext();
            switcher = new audioSwitch();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            timer.Start();
            //switcher.Changed += switcher_Changed;
		}

        void timer_Tick(object sender, EventArgs e)
        {
            if (switcher.devices==null) return;
            bool found;
            int newDeviceIndex = 0;

            int prevCount=switcher.devices.Count;
            MMDeviceCollection devicesCopy = switcher.devices;
            switcher.updateDevices();

            if (switcher.devices.Count > prevCount)
            {

                for (int i = 0; i < switcher.devices.Count;i++ )
                {
                     found= false;
                    for (int j = 0; j < devicesCopy.Count; j++)
                    {
                        if (devicesCopy[j].ID == switcher.devices[i].ID)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        newDeviceIndex = i;
                        break;
                    }
                }
                //switch to new device               
                switcher.currentDevice = newDeviceIndex;
            }
        }

     

        private Timer timerHide;
        void notifyIcon_Click(object sender, EventArgs e)
        {        
                   
            int currentDevice=switcher.currentDevice;                    
           ToolStripMenuItem TSitem;

            notifyIcon.ContextMenuStrip.Items.Clear();

            for (int i = 0; i < switcher.devices.Count;i++ )
            {
                TSitem = new ToolStripMenuItem("", null, menuItem_Click);
                          
                TSitem.Checked = (currentDevice == i);
                TSitem.Text = switcher.devices[i].FriendlyName;

                notifyIcon.ContextMenuStrip.Items.Add(TSitem);
         
            }
        
            Point menuPos = new Point(Control.MousePosition.X, Control.MousePosition.Y);
            menuPos.Offset(-notifyIcon.ContextMenuStrip.Width, -notifyIcon.ContextMenuStrip.Height);
            notifyIcon.ContextMenuStrip.Show(menuPos);
            timerHide.Start();
        }

        

        private void menuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < switcher.devices.Count; i++)
            {
                if (sender == notifyIcon.ContextMenuStrip.Items[i])
                {
                    switcher.currentDevice=i;
                    notifyIcon.ContextMenuStrip.Visible = false;
                }
            }
            
        }

      

        # region generic code framework

        private System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        private NotifyIcon notifyIcon;				            // the icon that sits in the system tray
       

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
                             {
                                 ContextMenuStrip = new ContextMenuStrip(),                             
                                 Icon = new Icon(IconFileName),
                                 Text = DefaultTooltip,
                                 Visible = true,
                                 
                             };
                                
            notifyIcon.Click += notifyIcon_Click;
            notifyIcon.ContextMenuStrip.MouseLeave += ContextMenuStrip_MouseLeave;
            notifyIcon.ContextMenuStrip.MouseMove += ContextMenuStrip_MouseMove;
            
            timerHide = new Timer();
            timerHide.Interval = 5000;
            timerHide.Tick += timerHide_Tick;
        }

        void timerHide_Tick(object sender, EventArgs e)
        {
            notifyIcon.ContextMenuStrip.Visible = false;
            timerHide.Enabled = false;
        }

        void ContextMenuStrip_MouseMove(object sender, MouseEventArgs e)
        {
            timerHide.Enabled = false;
        }

        

        void ContextMenuStrip_MouseLeave(object sender, EventArgs e)
        {
            notifyIcon.ContextMenuStrip.Visible = false;
        }

      

      

      

        /// <summary>
		/// When the application context is disposed, dispose things like the notify icon.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && components != null) { components.Dispose(); }
		}

		/// <summary>
		/// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void exitItem_Click(object sender, EventArgs e) 
		{
			ExitThread();
		}

        /// <summary>
        /// If we are presently showing a form, clean it up.
        /// </summary>
        protected override void ExitThreadCore()
        { 

            notifyIcon.Visible = false; // should remove lingering tray icon
            base.ExitThreadCore();
        }

        # endregion generic code framework

    }
}
