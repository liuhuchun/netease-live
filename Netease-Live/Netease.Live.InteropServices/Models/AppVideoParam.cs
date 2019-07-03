using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播视频源为应用程序窗口时，输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AppVideoParam
    {
        /// <summary>
        /// 窗口句柄
        /// </summary>
        public IntPtr Hwnd;
    }
}
