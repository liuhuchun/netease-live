using Netease.Live.InteropServices.Enums;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 录制参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct _RecordParam
    {
        /// <summary>
        /// 录制文件保存路径
        /// 请传入UTF-8编码字符串以防中文乱码
        /// </summary>
        public IntPtr Path;

        /// <summary>
        /// 录制格式
        /// </summary>
        public RecordFormat Format;

        public void Dispose()
        {
            if (Path != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(Path);

                Path = IntPtr.Zero;
            }
        }

        public static explicit operator _RecordParam(RecordParam param)
        {
            return new _RecordParam
            {
                Path = Utilities.UTF8StringToIntPtr(param.Path),
                Format = param.Format
            };
        }
    }

    /// <summary>
    /// 录制参数
    /// </summary>
    public class RecordParam
    {
        /// <summary>
        /// 录制文件保存路径
        /// 请传入UTF-8编码字符串以防中文乱码
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 录制格式
        /// </summary>
        public RecordFormat Format { get; set; }
    }
}
