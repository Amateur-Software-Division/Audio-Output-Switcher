using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using CoreAudioApi;

namespace AudioOutSwitcher
{
    public enum ERole { 
  eConsole,
  eMultimedia,
  eCommunications,
  ERole_enum_count
    } 

    [Guid("f8679f50-850a-41cf-9c72-430f290290c8"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPolicyConfig
    {
        [PreserveSig]
        int GetMixFormat(string pszDeviceName, IntPtr ppFormat);

        [PreserveSig]
        int GetDeviceFormat(string pszDeviceName, bool bDefault, IntPtr ppFormat);

        [PreserveSig]
        int ResetDeviceFormat(string pszDeviceName);

        [PreserveSig]
        int SetDeviceFormat(string pszDeviceName, IntPtr pEndpointFormat, IntPtr MixFormat);

        [PreserveSig]
        int GetProcessingPeriod(string pszDeviceName, bool bDefault, IntPtr pmftDefaultPeriod, IntPtr pmftMinimumPeriod);

        [PreserveSig]
        int SetProcessingPeriod(string pszDeviceName, IntPtr pmftPeriod);

        [PreserveSig]
        int GetShareMode(string pszDeviceName, IntPtr pMode);

        [PreserveSig]
        int SetShareMode(string pszDeviceName, IntPtr mode);

        [PreserveSig]
        int GetPropertyValue(string pszDeviceName, bool bFxStore, IntPtr key, IntPtr pv);

        [PreserveSig]
        int SetPropertyValue(string pszDeviceName, bool bFxStore, IntPtr key, IntPtr pv);

        [PreserveSig]
        int SetDefaultEndpoint(string pszDeviceName, ERole role);

        [PreserveSig]
        int SetEndpointVisibility(string pszDeviceName, bool bVisible);
    }


    [ComImport, Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9")]
    internal class _PolicyConfigClient
    {
    }

    public class PolicyConfigClient
    {
        private IPolicyConfig _PolicyConfig = new _PolicyConfigClient() as IPolicyConfig;

        public void SetDefaultEndpoint(string devID, ERole eRole)
        {
            Marshal.ThrowExceptionForHR(_PolicyConfig.SetDefaultEndpoint(devID, eRole));
        }
    }
}