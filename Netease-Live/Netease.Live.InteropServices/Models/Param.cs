using Netease.Live.InteropServices.Enums;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播推流参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _Param
    {
        /// <summary>
        /// 推流流内容
        /// </summary>
        public OutContent Content;

        /// <summary>
        /// 推流流的格式
        /// </summary>
        public OutFormat Format;

        /// <summary>
        /// 直播地址对象
        /// </summary>
        public IntPtr Url;

        /// <summary>
        /// 推流视频输出相关参数
        /// </summary>
        public VideoOutParam VideoParam;

        /// <summary>
        /// 推流音频输出相关参数
        /// </summary>
        public AudioOutParam AudioParam;

        /// <summary>
        /// 透传同步时间戳类型，默认不传。
        /// 推流类型必须包含视频，同时需要网易云播放器支持
        /// </summary>
        public SyncTimestampType SyncTimestampType;
        
        public void Dispose()
        {
            if (Url != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(Url);

                Url = IntPtr.Zero;
            }
        }

        public static explicit operator Param(_Param param)
        {
            return new Param
            {
                Content = param.Content,
                Format = param.Format,
                Url = Utilities.IntPtrToUTF8String(param.Url),
                VideoParam = param.VideoParam,
                AudioParam = param.AudioParam,
                SyncTimestampType = param.SyncTimestampType
            };
        }

        public static explicit operator _Param(Param param)
        {
            return new _Param
            {
                Content = param.Content,
                Format = param.Format,
                Url = Utilities.UTF8StringToIntPtr(param.Url),
                VideoParam = param.VideoParam,
                AudioParam = param.AudioParam,
                SyncTimestampType = param.SyncTimestampType
            };
        }
    }

    /// <summary>
    /// 直播推流参数
    /// </summary>
    public class Param
    {
        /// <summary>
        /// 推流流内容
        /// </summary>
        public OutContent Content { get; set; }

        /// <summary>
        /// 推流流的格式
        /// </summary>
        public OutFormat Format { get; set; }

        /// <summary>
        /// 直播地址对象
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 推流视频输出相关参数
        /// </summary>
        public VideoOutParam VideoParam { get; set; }

        /// <summary>
        /// 推流音频输出相关参数
        /// </summary>
        public AudioOutParam AudioParam { get; set; }

        /// <summary>
        /// 透传同步时间戳类型，默认不传。
        /// 推流类型必须包含视频，同时需要网易云播放器支持
        /// </summary>
        public SyncTimestampType SyncTimestampType { get; set; }
    }
}
