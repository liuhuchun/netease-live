using Netease.Live.InteropServices.Enums;
using Netease.Live.InteropServices.Models;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices
{
    /// <summary>
    /// 子音频开启/采集/调节/关闭API
    /// </summary>
    public static class ChildAudioApi
    {
        /// <summary>
        /// 以默认值初始化子音频输入参数
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">子音频参数</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioGetDefaultParam",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void GetDefaultParam(IntPtr service, out _AudioInParam param);

        /// <summary>
        /// 打开子音频实例
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">子音频采集参数</param>
        /// <returns>childService：非NULL成功， NULL失败</returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioOpen",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Open(IntPtr service, ref _AudioInParam param);

        /// <summary>
        /// 关闭子音频实例
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioClose",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Close(IntPtr childService);

        /// <summary>
        /// 打开子音频采集，当需要改变采集设备的时候需要先停止再重新打开
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioStartCapture",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult StartCapture(IntPtr childService);

        /// <summary>
        /// 关闭子音频采集
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioStopCapture",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StopCapture(IntPtr childService);

        /// <summary>
        /// 暂停/继续子音频采集
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        /// <param name="pause">是否暂停子音频采集</param>
        /// <remarks>
        /// 此方法必须在StartCapture和StopCapture之间执行
        /// </remarks>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioSwitchCapture",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SwitchCapture(IntPtr childService, bool pause);

        /// <summary>
        /// 设置子音频音量
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        /// <param name="ratio">暂定0-100；0表示静音，100表示原始音量</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioSetVolume",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetVolume(IntPtr childService, int ratio);

        //--------------------- 直播推流API --------------------
        //------------------------------------------------------

        /// <summary>
        /// 暂停子音频推流（不会暂停采集）
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioPauseLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void PauseLiveStream(IntPtr childService);

        /// <summary>
        /// 恢复子音频推流
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioResumeLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ResumeLiveStream(IntPtr childService);

        /// <summary>
        /// 用户指定音频流推流发送接口
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        /// <param name="audioData">用户指定音频流数据</param>
        /// <param name="length">音频流数据长度</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioSendCustomData",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult SendCustomData(
            IntPtr childService,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)]
            byte[] audioData,
            int length);

        /// <summary>
        /// 用户指定音频流推流发送接口，支持直播过程中音频数据重采样
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        /// <param name="audioData">用户指定音频流数据</param>
        /// <param name="length">音频流数据长度</param>
        /// <param name="sampleRate">音频流采样率，支持中间变化入参</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioSendCustomData",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult SendCustomData(
            IntPtr childService,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)]
            byte[] audioData,
            int length,
            int sampleRate);
    }
}
