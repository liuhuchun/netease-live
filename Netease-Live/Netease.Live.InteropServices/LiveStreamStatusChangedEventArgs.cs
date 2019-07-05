using Netease.Live.InteropServices.Enums;
using System;

namespace Netease.Live.InteropServices
{
    /// <summary>
    /// 直播推流状态发生改变参数
    /// </summary>
    public class LiveStreamStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 状态值
        /// </summary>
        public Status Status { get; private set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public ErrorCode Code { get; private set; }

        public LiveStreamStatusChangedEventArgs(Status status, ErrorCode code)
        {
            Status = status;
            Code = code;
        }
    }
}
