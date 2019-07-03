using Netease.Live.InteropServices.Enums;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播推流视频输出参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VideoOutParam
    {
        /// <summary>
        /// 视频的输出宽.要求是4的倍数
        /// </summary>
        public int Width;

        /// <summary>
        /// 视频的输出高.要求是4的倍数
        /// </summary>
        public int Height;

        /// <summary>
        /// 视频的帧率
        /// </summary>
        public int Fps;

        /// <summary>
        /// 码率
        /// </summary>
        public int BitRate;

        /// <summary>
        /// 视频编码器.可以选择X264，Openh264
        /// </summary>
        public VideoOutCodec Codec;

        /// <summary>
        /// 是否使用视频硬件编码
        /// </summary>
        public bool HardEncode;

        /// <summary>
        /// 是否自动调整分辨率
        /// </summary>
        public bool QosAutoChangeRatio;
    }
}
