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
        // We start with 16 bytes of cryptographically strong random data.
        var randomBytes = RandomNumberGenerator.GetBytes(10);//生成10位的强随机字节

        // An alternate method: use a normally-created GUID to get our initial
        // random data:
        // byte[] randomBytes = Guid.NewGuid().ToByteArray();
        // This is faster than using RNGCryptoServiceProvider, but I don't
        // recommend it because the .NET Framework makes no guarantee of the
        // randomness of GUID data, and future versions (or different
        // implementations like Mono) might use a different method.

        // Now we have the random basis for our GUID.  Next, we need to
        // create the six-byte block which will be our timestamp.

        // We start with the number of milliseconds that have elapsed since
        // DateTime.MinValue.  This will form the timestamp.  There's no use
        // being more specific than milliseconds, since DateTime.Now has
        // limited resolution.

        // Using millisecond resolution for our 48-bit timestamp gives us
        // about 5900 years before the timestamp overflows and cycles.
        // Hopefully this should be sufficient for most purposes. :)
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

        // For string and byte-array version, we copy the timestamp first, followed
        // by the random data.
        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);//前6字节为转换成字节的时间戳
        Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);//后10字节为随机数

        // If formatting as a string, we have to compensate for the fact
        // that .NET regards the Data1 and Data2 block as an Int32 and an Int16,
        // respectively.  That means that it switches the order on little-endian
        // systems.  So again, we have to reverse.
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(guidBytes, 0, 4);
            Array.Reverse(guidBytes, 4, 2);
        }

        return new System.Guid(guidBytes);
    }
}