using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播视频源为DeckLink采集卡设备时，输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _DeckLinkParam
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public IntPtr DeviceId;

        /// <summary>
        /// 设备打开模式
        /// </summary>
        public int DeviceMode;

        public void Dispose()
        {
            if (DeviceId != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(DeviceId);

                DeviceId = IntPtr.Zero;
            }
        }

        public static explicit operator _DeckLinkParam(DeckLinkParam param)
        {
            return new _DeckLinkParam
            {
                DeviceId = Utilities.UTF8StringToIntPtr(param.DeviceId),
                DeviceMode = param.DeviceMode
            };
        }
    }

    /// <summary>
    /// 直播视频源为DeckLink采集卡设备时，输入参数
    /// </summary>
    public class DeckLinkParam
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 设备打开模式
        /// </summary>
        public int DeviceMode { get; set; }
    }
}
