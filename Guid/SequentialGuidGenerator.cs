using System;
using System.Security.Cryptography;

namespace Azusa.Shared.Guid;

public static class SequentialGuidGenerator
{
    /// <summary>
    /// 生成顺序的Guid
    /// 前12位由当前时间戳构成，后20位为随机数
    /// 并不符合RFC 4122
    /// </summary>
    /// <returns></returns>
    public static System.Guid Create()
    {
        //参考ABP框架的Guid生成算法
        
        var randomBytes = RandomNumberGenerator.GetBytes(10);//生成10位的强随机字节

        long timestamp = DateTime.UtcNow.Ticks / 10000L;//取当前时间毫秒数作为时间戳

        // Then get the bytes
        byte[] timestampBytes = BitConverter.GetBytes(timestamp);

        //如果是小端系统，则要反转时间戳字节数组
        // Since we're converting from an Int64, we have to reverse on
        // little-endian systems.
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampBytes);
        }

        byte[] guidBytes = new byte[16];

        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);//前6字节为转换成字节的时间戳
        Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);//后10字节为随机数

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(guidBytes, 0, 4);
            Array.Reverse(guidBytes, 4, 2);
        }

        return new System.Guid(guidBytes);
    }
}