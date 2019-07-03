using Netease.Live.InteropServices.Enums;
using Netease.Live.InteropServices.Models;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices
{
    /// <summary>
    /// 直播发生错误回调，当直播过程中发生错误，通知应用层，应用层可以做相应的处理
    /// </summary>
    /// <param name="service">直播推流实例</param>
    /// <param name="status">直播状态</param>
    /// <param name="errorCode">错误码</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void StatusNotifyCallback(IntPtr service, Status status, ErrorCode errorCode);

    /// <summary>
    /// 获取最新一帧合并子视频画面后的视频截图后的回调
    /// </summary>
    /// <param name="service">直播推流实例</param>
    /// <param name="sampler">最新一帧合并子视频画面后的视频截图</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MergedVideoSamplerCallback(IntPtr service, VideoSampler sampler);

    /// <summary>
    /// 获取单个子视频截图后的回调
    /// </summary>
    /// <param name="service">直播推流实例</param>
    /// <param name="childService">子视频对象句柄</param>
    /// <param name="sampler">最新一帧单个子视频截图</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ChildVideoSamplerCallback(IntPtr service, IntPtr childService, VideoSampler sampler);

    /// <summary>
    /// 获取最新一段混音后的音频数据的回调
    /// </summary>
    /// <param name="service">直播推流实例</param>
    /// <param name="sampler">最新一段混音后的音频数据</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MixedAudioSamplerCallback(IntPtr service, AudioSampler sampler);

    /// <summary>
    /// 获取单个子音频数据的回调
    /// </summary>
    /// <param name="service">直播推流实例</param>
    /// <param name="childService">子音频对象句柄</param>
    /// <param name="sampler">最新一段子音频数据</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ChildAudioSamplerCallback(IntPtr service, IntPtr childService, AudioSampler sampler);
}
