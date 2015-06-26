using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CoreAudioApi;



namespace AudioOutSwitcher
{
    class audioSwitch
    {
        public MMDeviceCollection devices;  
        private int current;
        //public delegate void ChangedEventHandler(object sender, EventArgs e);
        //public event ChangedEventHandler Changed;
        public int currentDevice
        {
            get { updateDevices(); return current; }
            set {setAudioOutput(value);}
        }
        
        public audioSwitch()
        {
            updateDevices();
        }

        //protected virtual void OnChanged(EventArgs e) 
        //{
        //    //if (Changed != null)
        //    //   Changed(this, e);
        //}

       
        public void updateDevices()
        {            

            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            devices = DevEnum.EnumerateAudioEndPoints(EDataFlow.eRender, EDeviceState.DEVICE_STATE_ACTIVE);
            MMDevice DefaultDevice = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);

            for (int i = 0; i < devices.Count; i++)
            {             
                if (DefaultDevice.FriendlyName==devices[i].FriendlyName)
                   current=i;
            }      
            
        }

        private void setAudioOutput(int deviceNum)
        {        
            PolicyConfigClient client = new PolicyConfigClient();

            client.SetDefaultEndpoint(devices[deviceNum].ID, ERole.eCommunications);
            client.SetDefaultEndpoint(devices[deviceNum].ID, ERole.eMultimedia);
        }

    }
}
//Process p = new Process();
//ProcessStartInfo startInfo = new ProcessStartInfo();
//startInfo.FileName = "EndPointController.exe";
//startInfo.UseShellExecute = false;
//startInfo.RedirectStandardOutput = true;
//startInfo.CreateNoWindow = true;

//p.StartInfo = startInfo;
//p.Start();
//string output = p.StandardOutput.ReadToEnd();
//p.WaitForExit();

//devices = output.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
//for (int i = 0; i < devices.Length; i++)
//{
//    if (devices[i][0] == '*')
//        current = i;
//}


//private void setAudioOutput1(int deviceNum)
//{          

//    if (deviceNum < 0 && deviceNum >= devices.Count()) return;

//    Process p = new Process();
//    ProcessStartInfo startInfo = new ProcessStartInfo();
//    startInfo.FileName = "EndPointController.exe";
//    startInfo.Arguments = deviceNum.ToString();
//    startInfo.UseShellExecute = false;
//    startInfo.RedirectStandardOutput = true;
//    startInfo.CreateNoWindow = true;

//    p.StartInfo = startInfo;
//    p.Start();
//    string output = p.StandardOutput.ReadToEnd();
//    p.WaitForExit();

//    if (p.ExitCode != 0)
//    {
//        throw new Exception("Failure setting audio output device: "+p.ExitCode);                
//    }
//}