using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 应用程序窗口信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _AppWindowInfo
    {
        /// <summary>
        /// 窗口句柄
        /// </summary>
        public IntPtr Hwnd;

        /// <summary>
        /// 窗口标题（可能会随时变化）
        /// </summary>
        public IntPtr Title;

        public void Initialize()
        {
            Hwnd = IntPtr.Zero;

            Title = Utilities.UTF8StringToIntPtr(new string('\0', 1024));
        }

        public void Dispose()
        {
            Hwnd = IntPtr.Zero;

            if (Title != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(Title);

                Title = IntPtr.Zero;
            }
        }

        public static explicit operator AppWindowInfo(_AppWindowInfo info)
        {
            if (info.Hwnd == IntPtr.Zero)
            {
                return null;
            }

            return new AppWindowInfo
            {
                Hwnd = info.Hwnd,
                Title = Utilities.IntPtrToUTF8String(info.Title)
            };
        }
    }

    /// <summary>
    /// 应用程序窗口信息
    /// </summary>
    public class AppWindowInfo
    {
        /// <summary>
        /// 窗口句柄
        /// </summary>
        public IntPtr Hwnd { get; set; }

        /// <summary>
        /// 窗口标题（可能会随时变化）
        /// </summary>
        public string Title { get; set; }
    }
}
