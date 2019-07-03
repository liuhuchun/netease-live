using Netease.Live.InteropServices.Enums;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播推流状态信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct StatusInfo
    {
        /// <summary>
        /// 视频发送帧率信息
        /// </summary>
        public uint VideoSendFrameRate;

        /// <summary>
        /// 视频发送码率信息
        /// </summary>
        public uint VideoSendBitRate;

        /// <summary>
        /// 视频宽度信息
        /// </summary>
        public uint VideoSendWidth;

        /// <summary>
        /// 视频高度信息
        /// </summary>
        public uint VideoSendHeight;

        /// <summary>
        /// 视频设置的帧率信息
        /// </summary>
        public uint VideoSetFrameRate;

        /// <summary>
        /// 视频设置的码率信息
        /// </summary>
        public uint VideoSetBitRate;

        /// <summary>
        /// 视频设置的宽度信息
        /// </summary>
        public uint VideoSetWidth;

        /// <summary>
        /// 视频设置的高度信息
        /// </summary>
        public uint VideoSetHeight;

        /// <summary>
        /// 音频发送码率信息
        /// </summary>
        public uint AudioSendBitRate;

        /// <summary>
        /// 网络状况等级
        /// </summary>
        public NetworkLevel NetworkLevel;
    }
}
