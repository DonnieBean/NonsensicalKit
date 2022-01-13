namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 管理类相关消息枚举
    /// </summary>
    public enum NonsensicalManagerEnum
    {
        ManagerSubscribe = 23234,
        InitStart,
        InitComlete,
        LateInitStart,
        LateInitComlete ,
        FinalInitStart,
        FinalInitComlete,
        AllInitComplete ,

        ReceviedProtocolsMessage,

        ABLoadCountChanged,
    }
}
