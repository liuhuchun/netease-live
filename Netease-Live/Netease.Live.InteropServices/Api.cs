using Netease.Live.InteropServices.Enums;
using Netease.Live.InteropServices.Models;
using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices
{
    public static class Api
    {
        //--------------------- 设备管理API --------------------
        //---------------- 获取可供采集资源列表 ----------------
        //------------------------------------------------------

        /// <summary>
        /// 获取可采集应用图像的app个数
        /// </summary>
        /// <param name="appWindNum">可采集图像的app数量</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetAvailableAppWindNum",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult GetAvailableAppWindowNum([Out] out int appWindNum);

        /// <summary>
        /// 获取可采集图像的app列表信息
        /// </summary>
        /// <param name="infos">可采集图像的app信息</param>
        /// <param name="maxNum">最大容量</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetAvailableAppWind",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult GetAvailableAppWindow([Out] IntPtr infos, int maxNum);

        /// <summary>
        /// 获取可用多媒体设备列表个数
        /// </summary>
        /// <param name="videoDeviceNum">视频设备数量</param>
        /// <param name="audioDeviceNum">音频设备数量</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetFreeDevicesNum",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult GetFreeDevicesNum([Out] out int videoDeviceNum, [Out] out int audioDeviceNum);

        /// <summary>
        /// 获取可用多媒体设备列表名称，暂时只支持DShow采集音视频
        /// </summary>
        /// <param name="videoDeviceInfos">视频设备信息</param>
        /// <param name="maxVideoDeviceNum">VideoDevices最大容量</param>
        /// <param name="audioDeviceInfos">音频设备信息</param>
        /// <param name="maxAudioDeviceNum">AudioDevices最大容量</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetFreeDeviceInf",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult GetFreeDeviceInfos([Out] IntPtr videoDeviceInfos, int maxVideoDeviceNum, [Out] IntPtr audioDeviceInfos, int maxAudioDeviceNum);

        /// <summary>
        /// 获取摄像头设备支持的采集参数
        /// </summary>
        /// <param name="camera">摄像头对象</param>
        /// <param name="captureParams">摄像头支持的采集参数数组（请自己申请和释放内存）</param>
        /// <param name="num">
        /// 作为入参表示传入的captureParams数组capacity；
        /// 作为出参表示实际取到的captureParams数组size
        /// </param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_DeviceGetCamereCaptureInf",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult GetCamereCaptureInfo(ref _InDeviceInfo camera, [Out] IntPtr captureParams, ref int num);

        /// <summary>
        /// 获取所有DeckLink设备信息
        /// </summary>
        /// <param name="deckLinkDevices">
        /// DeckLink设备信息（请自己申请和释放内存）；
        /// 当此参数为空时，num返回可用的DeckLink设备个数
        /// </param>
        /// <param name="num">
        /// 作为入参表示传入的deckLinkDevices数组capacity；
        /// 作为出参表示实际取到的deckLinkDevices数组size
        /// </param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetDeckLinkDeviceList",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult GetDeckLinkDeviceList([Out] IntPtr deckLinkDevices, ref int num);

        /// <summary>
        /// 获取指定DeckLink设备的所有模式信息
        /// </summary>
        /// <param name="deviceId">指定的DeckLink设备id</param>
        /// <param name="deckLinkDeviceModes">DeckLink设备的模式</param>
        /// <param name="num">
        /// 作为入参表示传入的deckLinkDeviceModes数组capacity；
        /// 作为出参表示实际取到的deckLinkDeviceModes数组size
        /// </param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetDeckLinkDeviceModeListById",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult GetDeckLinkDeviceModeListById(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            string deviceId,
            [Out]
            IntPtr deckLinkDeviceModes,
            ref int num);

        //---------------- 初始化和参数设置API -----------------
        //------------------------------------------------------

        /// <summary>
        /// 获取sdk版本号
        /// </summary>
        /// <param name="version">版本号</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetSDKVersion",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void GetSDKVersion([Out] out IntPtr version);

        /// <summary>
        /// 创建直播推流实例，推流对象只允许存在一个，多次直播可以只调用一次
        /// </summary>
        /// <param name="workPath">NLSS work目录(要求UTF-8编码)，会从work目录下面动态加载所需要的dll。如果设置为空，则在当前所在目录下面 ，操作上述事情</param>
        /// <param name="cachePath">NLSS cache目录，会在该目录下面生成日志,需要有文件创建和写入权限。如果设置为空，则在当前所在目录下面 ，操作上述事情。</param>
        /// <param name="service">直播推流实例</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_Create",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult Create(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            string workPath,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            string cachePath,
            [Out]
            out IntPtr service);

        /// <summary>
        /// 销毁直播推流实例，可以程序退出再销毁
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_Destroy",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Destroy(IntPtr service);

        /// <summary>
        /// 获取直播默认参数
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">直播参数</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetDefaultParam",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult GetDefaultParam(IntPtr service, [Out] out _Param param);

        /// <summary>
        /// 初始化直播参数
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">直播参数</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_InitParam",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult InitParam(IntPtr service, ref _Param param);

        /// <summary>
        /// 清除直播参数
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_UninitParam",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UninitParam(IntPtr service);

        /// <summary>
        /// 设置视频水印，默认是无水印
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">水印参数</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_SetVideoWaterMark",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetVideoWaterMark(IntPtr service, ref _VideoWaterParam param);

        /// <summary>
        /// 设置视频截图的的回调
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="callback">视频截图图像回调</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_SetVideoSamplerCB",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetVideoSamplerCallback(IntPtr service, MergedVideoSamplerCallback callback);

        /// <summary>
        /// 设置直播过程中状态回调
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="callback">直播状态通知回调</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_SetStatusCB",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetStatusCallback(IntPtr service, StatusNotifyCallback callback);

        //----------------- 修改PARAM中的参数 ------------------
        //------------------------------------------------------

        /// <summary>
        /// 修改视频输出参数，必须在未推流状态下执行
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">新的视频输出参数</param>
        /// <returns>修改成功返回true，失败返回false</returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_UpdateVideoOutParam",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool UpdateVideoOutParam(IntPtr service, ref VideoOutParam param);

        /// <summary>
        /// 修改音频输出参数，必须在未推流状态下执行
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">新的音频输出参数</param>
        /// <returns>修改成功返回true，失败返回false</returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_UpdateAudioOutParam",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool UpdateAudioOutParam(IntPtr service, ref AudioOutParam param);

        /// <summary>
        /// 修改推流地址，必须在未推流状态下执行
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="pushUrl">新的推流地址</param>
        /// <returns>修改成功返回true，失败返回false</returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_UpdatePushUrl",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool UpdatePushUrl(
            IntPtr service,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            string pushUrl);

        /// <summary>
        /// 修改透传同步时间戳类型
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="type">新的同步时间戳类型</param>
        /// <returns>修改成功返回true，失败返回false</returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_UpdateSyncTimestampType",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool UpdateSyncTimestampType(IntPtr service, SyncTimestampType type);

        //------------------ 启动|停止处理API ------------------
        //------------------------------------------------------

        /// <summary>
        /// 启动处理，在各类初始化之后，在预览和直播启动之前
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_Start",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult Start(IntPtr service);

        /// <summary>
        /// 停止处理，在预览和直播停止之后，在UninitParam之前
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_Stop",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Stop(IntPtr service);

        //------------------- 输出视频预览API ------------------
        //------------------------------------------------------

        /// <summary>
        /// 打开视频预览
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_StartVideoPreview",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult StartVideoPreview(IntPtr service);

        /// <summary>
        /// 暂停视频预览
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_PauseVideoPreview",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void PauseVideoPreview(IntPtr service);

        /// <summary>
        /// 恢复视频预览
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ResumeVideoPreview",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ResumeVideoPreview(IntPtr service);

        /// <summary>
        /// 停止视频预览
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_StopVideoPreview",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StopVideoPreview(IntPtr service);

        //--------------------- 直播推流API --------------------
        //------------------------------------------------------

        /// <summary>
        /// 启动直播推流
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_StartLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult StartLiveStream(IntPtr service);

        /// <summary>
        /// 停止直播推流
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_StopLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StopLiveStream(IntPtr service);

        /// <summary>
        /// 暂停视频直播推流
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_PauseVideoLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void PauseVideoLiveStream(IntPtr service);

        /// <summary>
        /// 恢复视频直播推流
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ResumeVideoLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ResumeVideoLiveStream(IntPtr service);

        /// <summary>
        /// 暂停音频直播推流
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_PauseAudioLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void PauseAudioLiveStream(IntPtr service);

        /// <summary>
        /// 恢复音频直播推流
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_ResumeAudioLiveStream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ResumeAudioLiveStream(IntPtr service);

        /// <summary>
        /// 开始直播录制
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">录制参数（录制文件存放路径、录制格式等）</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_StartRecord",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult StartRecord(IntPtr service, ref _RecordParam param);

        /// <summary>
        /// 停止直播录制
        /// </summary>
        /// <param name="service">直播推流实例</param>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_StopRecord",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StopRecord(IntPtr service);

        /// <summary>
        /// 获取当前直播流的pts（实际上是音频流的dts），单位毫秒
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <returns>前直播流的pts，单位毫秒</returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetStreamPts",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern long GetStreamPts(IntPtr service);

        /// <summary>
        /// 获取当前透传的同步时间戳（通过视频流透传，Param.SyncTimestampType不为None时，此接口有效），单位毫秒
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <returns>当前透传的同步时间戳，单位毫秒</returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetSyncTimestamp",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern long GetSyncTimestamp(IntPtr service);

        /// <summary>
        /// 发送透传的自定义数据
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">要发送的自定义数据参数</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_SendCustomData",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult SendCustomData(IntPtr service, ref _CustomDataParam param);

        /// <summary>
        /// 获取直播推流状态信息
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="status">直播推流统计信息</param>
        /// <returns></returns>
        [DllImport(DllPathConfig.DllName,
            EntryPoint = "Nlss_GetStaticInfo",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern FuncResult GetStaticInfo(IntPtr service, [Out] out StatusInfo status);
    }
}
