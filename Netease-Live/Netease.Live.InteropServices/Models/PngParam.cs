using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播视频源为PNG图片时，输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _PngParam
    {
        /// <summary>
        /// png图片路径
        /// </summary>
        public IntPtr PngPath;

        public void Dispose()
        {
            if (PngPath != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(PngPath);

                PngPath = IntPtr.Zero;
            }
        }

        public static explicit operator _PngParam(PngParam param)
        {
            return new _PngParam
            {
                PngPath = Utilities.UTF8StringToIntPtr(param.PngPath),
            };
        }
    }

    /// <summary>
    /// 直播视频源为PNG图片时，输入参数
    /// </summary>
    public class PngParam
    {
        /// <summary>
        /// png图片路径
        /// </summary>
        public string PngPath { get; set; }
    }
}
