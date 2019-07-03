using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 水印参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _VideoWaterParam
    {
        /// <summary>
        /// 水印的文件路径
        /// </summary>
        public IntPtr FilePath;

        /// <summary>
        /// 水印的起始X坐标
        /// </summary>
        public uint StartX;

        /// <summary>
        /// 水印的起始Y坐标
        /// </summary>
        public uint StartY;
        
        public void Dispose()
        {
            if (FilePath != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(FilePath);

                FilePath = IntPtr.Zero;
            }
        }

        public static explicit operator _VideoWaterParam(VideoWaterParam param)
        {
            return new _VideoWaterParam
            {
                FilePath = Utilities.UTF8StringToIntPtr(param.FilePath),
                StartX = param.StartX,
                StartY = param.StartY
            };
        }
    }

    /// <summary>
    /// 水印参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class VideoWaterParam
    {
        /// <summary>
        /// 水印的文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 水印的起始X坐标
        /// </summary>
        public uint StartX { get; set; }

        /// <summary>
        /// 水印的起始Y坐标
        /// </summary>
        public uint StartY { get; set; }
    }
}
