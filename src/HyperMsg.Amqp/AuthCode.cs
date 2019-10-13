namespace HyperMsg.Amqp
{
    public enum AuthCode : byte
    {
        Ok,
        Auth,
        Sys,
        SysPerm,
        SysTemp
    }
}