using Netease.Live.InteropServices.Enums;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播视频源为摄像头模式时，输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _CameraParam
    {
        /// <summary>
        /// 摄像头设备路径
        /// </summary>
        public IntPtr DevicePath;

        /// <summary>
        /// 以指定清晰度开启摄像头
        /// </summary>
        public VideoQualityLevel QualityLevel;

        public void Dispose()
        {
            if (DevicePath != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(DevicePath);

                DevicePath = IntPtr.Zero;
            }
        }

        public static explicit operator _CameraParam(CameraParam param)
        {
            return new _CameraParam
            {
                DevicePath = Utilities.UTF8StringToIntPtr(param.DevicePath),
                QualityLevel = param.QualityLevel
            };
        }
    }

    /// <summary>
    /// 直播视频源为摄像头模式时，输入参数
    /// </summary>
    public class CameraParam
    {
        /// <summary>
        /// 摄像头设备路径
        /// </summary>
        public string DevicePath { get; set; }

        /// <summary>
        /// 以指定清晰度开启摄像头
        /// </summary>
        public VideoQualityLevel QualityLevel { get; set; }
    }
}
