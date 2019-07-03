using Netease.Live.InteropServices.Enums;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 摄像头设备支持的采集参数，一个摄像头设备可能支持多套采集参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CameraCaptureParam
    {
        /// <summary>
        /// 采集的宽
        /// </summary>
        public int Width;

        /// <summary>
        /// 采集的高
        /// </summary>
        public int Height;

        /// <summary>
        /// 采集的帧率
        /// </summary>
        public int Fps;

        /// <summary>
        /// 当输入源为yuv数据时，视频格式
        /// </summary>
        public VideoInFormat Format;
    }
}
