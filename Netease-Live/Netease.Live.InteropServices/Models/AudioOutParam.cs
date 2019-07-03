using Netease.Live.InteropServices.Enums;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播推流音频输出参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioOutParam
    {
        /// <summary>
        /// 音频编码器
        /// </summary>
        public AudioOutCodec Codec;

        /// <summary>
        /// 音频编码码率. 参考值：64000
        /// </summary>
        public int BitRate;

        /// <summary>
        /// 是否使用视频硬件编码
        /// </summary>
        public bool HardEncode;
    }
}
