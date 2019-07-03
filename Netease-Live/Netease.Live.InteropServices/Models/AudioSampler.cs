using Netease.Live.InteropServices.Enums;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// PCM音频数据参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class AudioSampler
    {
        /// <summary>
        /// 音频数据的采集率
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// 音频数据的通道数
        /// </summary>
        public int NumOfChannels { get; set; }

        /// <summary>
        /// 音频单样本位数
        /// </summary>
        public int BitsPerSample { get; set; }

        /// <summary>
        /// 音频数据的字节数
        /// </summary>
        public int DataSize { get; set; }

        /// <summary>
        /// 音频数据
        /// </summary>
        public IntPtr Data { get; set; }
    }
}
