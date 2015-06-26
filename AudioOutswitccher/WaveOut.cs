//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace AudioOutswitccher
//{
//    public class Audio
//    {
//        [DllImport("winmm.dll", SetLastError = true)]
//        static extern uint waveOutGetNumDevs();

//        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
//        public static extern uint waveOutGetDevCaps(uint hwo, ref WAVEOUTCAPS pwoc, uint cbwoc);

//        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
//        public struct WAVEOUTCAPS
//        {
//            public ushort wMid;
//            public ushort wPid;
//            public uint vDriverVersion;
//            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
//            public string szPname;
//            public uint dwFormats;
//            public ushort wChannels;
//            public ushort wReserved1;
//            public uint dwSupport;
//        }

//        public string[] GetSoundDevices()
//        {
//            uint devices = waveOutGetNumDevs();
//            string[] result = new string[devices];
//            WAVEOUTCAPS caps = new WAVEOUTCAPS();

//            for (uint i = 0; i < devices; i++)
//            {
//                waveOutGetDevCaps(i, ref caps, (uint)Marshal.SizeOf(caps));
//                result[i] = caps.szPname;
//            }
//            return result;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace AudioOutswitccher
{
    public static class Audio
    {
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        public struct WaveOutCaps {
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
            public string szProductName;
            public uint dwFormats;
            public ushort wChannels;
            public ushort wReserved1;
            public uint dwSupport;
        }



//        typedef struct {
//  WORD      wMid;
//  WORD      wPid;
//  MMVERSION vDriverVersion;
//  TCHAR     szPname[MAXPNAMELEN];
//  DWORD     dwFormats;
//  WORD      wChannels;
//  WORD      wReserved1;
//  DWORD     dwSupport;
//} WAVEOUTCAPS;

        [DllImport("winmm.dll", CharSet=CharSet.Auto)]
        static extern uint waveOutGetNumDevs();
       
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        static extern uint waveOutGetDevCaps(IntPtr uDeviceID, out WaveOutCaps pWoc, uint cbwoc);

        // Gets the number of installed wave Output devices
        public static uint GetNumDevices()
        {
            return waveOutGetNumDevs();
        }

        // Gets the device name assosicated with the deviceID
        public static String GetDeviceName(int deviceID)
        {
            WaveOutCaps waveCaps = new WaveOutCaps();
            waveOutGetDevCaps((IntPtr)deviceID, out waveCaps, (uint)Marshal.SizeOf(waveCaps));
            return waveCaps.szProductName;
        }

        private static string registryRoot = "HKEY_CURRENT_USER\\Software\\Microsoft\\Multimedia\\Sound Mapper";
       
        // Gets the Default Playback Device from the Registry
        public static string GetCurrentPlaybackDevice()
        {
            return (string)Registry.GetValue(registryRoot, "Playback", "");
        }

        // Sets the Default Playback Sound Mapper device in the registry
        public static void SetCurrentPlaybackDevice(string deviceName)
        {
            Registry.SetValue(registryRoot, "Playback", deviceName, RegistryValueKind.String);
        }

        // Switches the Ouput Device
        public static void switchOuputDevice()
        {
            for(int i=0; i<GetNumDevices(); i++)
            {
                if (GetCurrentPlaybackDevice() != GetDeviceName(i))
                {
                    SetCurrentPlaybackDevice(GetDeviceName(i));
                }
            }
        }

        public static string[] GetSoundDevices()
        {
            uint devices = waveOutGetNumDevs();
            string[] result = new string[devices];
         //   WaveOutCaps caps = new WaveOutCaps();

            for (int i = 0; i < devices; i++)
            {
                //waveOutGetDevCaps(i, ref caps, (uint)Marshal.SizeOf(caps));
                
                result[i] = GetDeviceName(i);//caps.szPname;
            }
            return result;

        }

    }
}
