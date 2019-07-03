using Netease.Live.InteropServices.Enums;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播推流音频输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _AudioInParam
    {
        /// <summary>
        /// 音频推流采集源
        /// </summary>
        public AudioInType Type;

        /// <summary>
        /// 当音频源为麦克风设备时，需设置麦克风设备路径或Id；
        /// 当音频源为DeckLink采集卡设备时，需设置DeckLink设备Id
        /// </summary>
        public IntPtr DevicePath;

        /// <summary>
        /// 音频的样本采集率. 参考值：44100
        /// </summary>
        public int SampleRate;

        /// <summary>
        /// 音频采集的通道数：单声道，双声道. 参考值：1
        /// </summary>
        public int NumOfChannels;

        /// <summary>
        /// 音频采集的每帧大小. 参考值：2048
        /// </summary>
        public int FrameSize;

        /// <summary>
        /// 音频单样本位数
        /// </summary>
        public int BitsPerSample;

        /// <summary>
        /// 音频输入格式，参考值：AudioInFormat.S16
        /// </summary>
        public AudioInFormat Format;

        public void Dispose()
        {
            if (DevicePath != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(DevicePath);

                DevicePath = IntPtr.Zero;
            }
        }

        public static explicit operator AudioInParam(_AudioInParam param)
        {
            return new AudioInParam
            {
                Type = param.Type,
                DevicePath = Utilities.IntPtrToUTF8String(param.DevicePath),
                SampleRate = param.SampleRate,
                NumOfChannels = param.NumOfChannels,
                FrameSize = param.FrameSize,
                BitsPerSample = param.BitsPerSample,
                Format = param.Format
            };
        }

        public static explicit operator _AudioInParam(AudioInParam param)
        {
            return new _AudioInParam
            {
                Type = param.Type,
                DevicePath = Utilities.UTF8StringToIntPtr(param.DevicePath),
                SampleRate = param.SampleRate,
                NumOfChannels = param.NumOfChannels,
                FrameSize = param.FrameSize,
                BitsPerSample = param.BitsPerSample,
                Format = param.Format
            };
        }
    }

    /// <summary>
    /// 直播推流音频输入参数
    /// </summary>
    public class AudioInParam
    {
        /// <summary>
        /// 音频推流采集源
        /// </summary>
        public AudioInType Type { get; set; }

        /// <summary>
        /// 当音频源为麦克风设备时，需设置麦克风设备路径或Id；
        /// 当音频源为DeckLink采集卡设备时，需设置DeckLink设备Id
        /// </summary>
        public string DevicePath { get; set; }

        /// <summary>
        /// 音频的样本采集率. 参考值：44100
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// 音频采集的通道数：单声道，双声道. 参考值：1
        /// </summary>
        public int NumOfChannels { get; set; }

        /// <summary>
        /// 音频采集的每帧大小. 参考值：2048
        /// </summary>
        public int FrameSize { get; set; }

        /// <summary>
        /// 音频单样本位数
        /// </summary>
        public int BitsPerSample { get; set; }

        /// <summary>
        /// 音频输入格式，参考值：AudioInFormat.S16
        /// </summary>
        public AudioInFormat Format { get; set; }
    }
}
