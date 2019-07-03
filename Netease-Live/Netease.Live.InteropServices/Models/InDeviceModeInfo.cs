using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// DeckLink设备支持的模式，一个DeckLink设备可能支持多套模式
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _InDeviceModeInfo
    {
        /// <summary>
        /// 模式名称
        /// </summary>
        public IntPtr ModeName;

        /// <summary>
        /// 模式值
        /// </summary>
        public int Mode;

        public void Initialize()
        {
            ModeName = Utilities.UTF8StringToIntPtr(new string('\0', 1024));
        }

        public void Dispose()
        {
            if (ModeName != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(ModeName);

                ModeName = IntPtr.Zero;
            }
        }

        public static explicit operator InDeviceModeInfo(_InDeviceModeInfo info)
        {
            return new InDeviceModeInfo
            {
                ModeName = Utilities.IntPtrToUTF8String(info.ModeName),
                Mode = info.Mode
            };
        }
    }

    /// <summary>
    /// DeckLink设备支持的模式，一个DeckLink设备可能支持多套模式
    /// </summary>
    public class InDeviceModeInfo
    {
        /// <summary>
        /// 模式名称
        /// </summary>
        public string ModeName { get; set; }

        /// <summary>
        /// 模式值
        /// </summary>
        public int Mode { get; set; }
    }
}
