using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 设备信息（摄像头、麦克风、DeckLink采集卡设备等）
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _InDeviceInfo
    {
        /// <summary>
        /// 设备友好名称
        /// </summary>
        public IntPtr FriendlyName;

        /// <summary>
        /// 设备路径（对于DeckLink设备是设备Id）
        /// </summary>
        public IntPtr Path;

        public void Initialize()
        {
            FriendlyName = Utilities.UTF8StringToIntPtr(new string('\0', 1024));

            Path = Utilities.UTF8StringToIntPtr(new string('\0', 1024));
        }

        public void Dispose()
        {
            if (FriendlyName != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(FriendlyName);

                FriendlyName = IntPtr.Zero;
            }

            if (Path != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(Path);

                Path = IntPtr.Zero;
            }
        }

        public static explicit operator InDeviceInfo(_InDeviceInfo info)
        {
            return new InDeviceInfo
            {
                FriendlyName = Utilities.IntPtrToUTF8String(info.FriendlyName),
                Path = Utilities.IntPtrToUTF8String(info.Path)
            };
        }

        public static explicit operator _InDeviceInfo(InDeviceInfo info)
        {
            return new _InDeviceInfo
            {
                FriendlyName = Utilities.UTF8StringToIntPtr(info.FriendlyName),
                Path = Utilities.UTF8StringToIntPtr(info.Path)
            };
        }
    }

    /// <summary>
    /// 设备信息（摄像头、麦克风、DeckLink采集卡设备等）
    /// </summary>
    public class InDeviceInfo
    {
        /// <summary>
        /// 设备友好名称
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// 设备路径（对于DeckLink设备是设备Id）
        /// </summary>
        public string Path { get; set; }
    }
}
