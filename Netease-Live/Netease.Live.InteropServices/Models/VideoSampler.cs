using Netease.Live.InteropServices.Enums;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 视频截图参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class VideoSampler
    {
        /// <summary>
        /// 视频截图图像的宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 视频截图图像的高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 视频截图图像的格式
        /// </summary>
        public VideoInFormat Format { get; set; }

        /// <summary>
        /// 视频截图图像的数据大小
        /// </summary>
        public int DataSize { get; set; }

        /// <summary>
        /// 视频截图图像的数据
        /// </summary>
        public IntPtr Data { get; set; }

        /// <summary>
        /// 无滤镜
        /// </summary>
        public IntPtr Ref { get; set; }
    }
}
