using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 发送自定义数据参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _CustomDataParam
    {
        /// <summary>
        /// 发送数据：
        /// 字符串，最大长度 50KB (估计全角字数在1600左右)（需要校验长度）
        /// </summary>
        public IntPtr Content;

        /// <summary>
        /// 发送间隔：
        /// 同一个数据的多次发送之间 间隔的视频帧数，0表示每帧都发，最大50
        /// </summary>
        public int SendInterval;

        /// <summary>
        /// 发送总次数：
        /// 目前暂定最大500
        /// </summary>
        /// <remarks>
        /// </remarks>
        public int SendTotalTimes;

        public void Dispose()
        {
            if (Content != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(Content);

                Content = IntPtr.Zero;
            }
        }

        public static explicit operator _CustomDataParam(CustomDataParam param)
        {
            return new _CustomDataParam
            {
                Content = Utilities.UTF8StringToIntPtr(param.Content),
                SendInterval = param.SendInterval,
                SendTotalTimes = param.SendTotalTimes
            };
        }
    }

    /// <summary>
    /// 发送自定义数据参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class CustomDataParam
    {
        /// <summary>
        /// 发送数据：
        /// 字符串，最大长度 50KB (估计全角字数在1600左右)（需要校验长度）
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送间隔：
        /// 同一个数据的多次发送之间 间隔的视频帧数，0表示每帧都发，最大50
        /// </summary>
        public int SendInterval { get; set; }

        /// <summary>
        /// 发送总次数：
        /// 目前暂定最大500
        /// </summary>
        /// <remarks>
        /// </remarks>
        public int SendTotalTimes { get; set; }
    }
}
