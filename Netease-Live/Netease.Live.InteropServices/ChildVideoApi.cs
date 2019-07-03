using Netease.Live.InteropServices.Enums;
using Netease.Live.InteropServices.Models;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices
{
    /// <summary>
    /// 子视频开启/采集/调整/关闭API
    /// </summary>
    public static class ChildVideoApi
    {
        /// <summary>
        /// 检查是否为其他采集设备（如视频采集卡）
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoIsOtherDevice",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool IsOtherDevice(IntPtr childService);

        /// <summary>
        /// 打开采集设备的高级设置
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        /// <returns></returns>
        /// <remarks>
        /// 该接口可能会弹出采集系统配置窗口，所以建议在IsOtherDevice检测到之后，让用户手动选择是否开启
        /// </remarks>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoOpenOtherDeviceConf",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult OpenOtherDeviceConfig(IntPtr childService);

        /// <summary>
        /// 打开子视频实例，用于推流直播
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">子视频采集参数</param>
        /// <returns>childService：非NULL成功， NULL失败</returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoOpen",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Open(IntPtr service, ref _VideoInParam param);

        /// <summary>
        /// 关闭子视频实例
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoClose",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Close(IntPtr childService);

        /// <summary>
        /// 将该子视频设为背景层，同时将子视频显示全部铺开
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoSetBackLayer",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetBackLayer(IntPtr childService);

        /// <summary>
        /// 将该子视频层级调降或者调升一层来显示
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        /// <param name="adjustUp">True上升一层，False降一层</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoAdjustLayer",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void AdjustLayer(IntPtr childService, bool adjustUp);

        /// <summary>
        /// 设置该子视频窗口在主窗口中的显示位置和大小
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        /// <param name="param">显示位置参数</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoSetDisplayRect",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetDisplayRect(IntPtr childService, ref RectScreenParam param);

        /// <summary>
        /// 临时开|关该子视频窗口是否显示
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        /// <param name="hide">子视频窗口单独是否隐藏</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoSwitchDisplay",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SwitchDisplay(IntPtr childService, bool hide);

        /// <summary>
        /// 打开子视频采集，需要在视频预览前调用，当需要改变采集设备的时候需要先停止再重新打开
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoStartCapture",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult StartCapture(IntPtr childService);

        /// <summary>
        /// 关闭视频采集
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoStopCapture",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StopCapture(IntPtr childService);

        /// <summary>
        /// 设置单独预览的视频流buffer回调函数，可用于预览进行显示
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        /// <param name="callback">单独推流的buffer回调</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoSetSoloPreviewCB",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetSoloPreviewCallback(IntPtr childService, ChildVideoSamplerCallback callback);

        /// <summary>
        /// 开|关子视频单独预览
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        /// <param name="on">子视频窗口单独预览开关</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoSwitchSoloPreview",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SwitchSoloPreview(IntPtr childService, bool on);

        //------------------------------------------------------
        //--------------------- 直播推流API --------------------

        /// <summary>
        /// 暂停视频直播推流
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoPauseLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void PauseLiveStream(IntPtr childService);

        /// <summary>
        /// 恢复视频直播推流
        /// </summary>
        /// <param name="childService">对应子视频实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildVideoResumeLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ResumeLiveStream(IntPtr childService);

        /// <summary>
        /// 用户指定视频流推流发送接口
        /// </summary>
        /// <param name="childService">对应子音频实例</param>
        /// <param name="videoData">用户指定视频流数据</param>
        /// <param name="length">视频流数据长度</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ChildAudioSendCustomData",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult SendCustomData(
            IntPtr childService,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)]
            byte[] videoData,
            int length);
    }
}
