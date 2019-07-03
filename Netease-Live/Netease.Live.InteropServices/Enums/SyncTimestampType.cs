
namespace Netease.Live.InteropServices.Enums
{
    /// <summary>
    /// 同步时间戳类型
    /// </summary>
    public enum SyncTimestampType
    {
        /// <summary>
        /// 不透传同步时间戳
        /// </summary>
        None,

        /// <summary>
        /// 同步时间戳基于推流开始时间
        /// </summary>
        BaseOnStreamStart,

        /// <summary>
        /// 同步时间戳基于开机时间
        /// </summary>
        BaseOnMachineStart
    }
}
