using System.Security.Cryptography;
using System.Text;
// ReSharper disable InconsistentNaming

namespace Azusa.Shared.Helpers
{
    public static class HashExtensions
    {
        /// <summary>
        /// 计算32位MD5码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string ToMd5Hash(this string word, bool toUpper = true)
        {
            try
            {
                var MD5CSP = MD5.Create();
                var bytValue = Encoding.UTF8.GetBytes(word);
                var bytHash = MD5CSP.ComputeHash(bytValue);
                MD5CSP.Clear();
                //根据计算得到的Hash码翻译为MD5码
                var sHash = "";
                foreach (var t in bytHash)
                {
                    long i = t / 16;
                    var sTemp = i > 9 ? ((char)(i - 10 + 0x41)).ToString() : ((char)(i + 0x30)).ToString();
                    i = t % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public static string ToMd5Hash(this Stream stream, bool toUpper = true)
        {
            using var md5Hash = MD5.Create();
            var bytes = md5Hash.ComputeHash(stream);
            return ToHashString(bytes, toUpper);
        }

        /// <summary>
        /// 计算SHA-1码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string ToSHA1Hash(this string word, bool toUpper = true)
        {
            try
            {
                var SHA1CSP = SHA1.Create();
                var bytValue = Encoding.UTF8.GetBytes(word);
                var bytHash = SHA1CSP.ComputeHash(bytValue);
                SHA1CSP.Clear();
                //根据计算得到的Hash码翻译为SHA-1码
                var sHash = "";
                foreach (var t in bytHash)
                {
                    long i = t / 16;
                    var sTemp = i > 9 ? ((char)(i - 10 + 0x41)).ToString() : ((char)(i + 0x30)).ToString();
                    i = t % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算SHA-256码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string ToSHA256Hash(this string word, bool toUpper = true)
        {
            try
            {
                var SHA256CSP = SHA256.Create();
                var bytValue = Encoding.UTF8.GetBytes(word);
                var bytHash = SHA256CSP.ComputeHash(bytValue);
                SHA256CSP.Clear();
                //根据计算得到的Hash码翻译为SHA-1码
                var sHash = "";
                foreach (var t in bytHash)
                {
                    long i = t / 16;
                    var sTemp = i > 9 ? ((char)(i - 10 + 0x41)).ToString() : ((char)(i + 0x30)).ToString();
                    i = t % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算SHA-256码
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="toUpper"></param>
        /// <returns></returns>
        public static string ToSHA256Hash(this Stream stream, bool toUpper = true)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(stream);
            return ToHashString(bytes, toUpper);
        }

        /// <summary>
        /// 计算SHA-384码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string ToSHA384Hash(this string word, bool toUpper = true)
        {
            try
            {
                var SHA384CSP = SHA384.Create();
                var bytValue = Encoding.UTF8.GetBytes(word);
                var bytHash = SHA384CSP.ComputeHash(bytValue);
                SHA384CSP.Clear();
                //根据计算得到的Hash码翻译为SHA-1码
                var sHash = "";
                foreach (var t in bytHash)
                {
                    long i = t / 16;
                    var sTemp = i > 9 ? ((char)(i - 10 + 0x41)).ToString() : ((char)(i + 0x30)).ToString();
                    i = t % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算SHA-512码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string ToSHA512Hash(this string word, bool toUpper = true)
        {
            try
            {
                var SHA512CSP = SHA512.Create();
                var bytValue = Encoding.UTF8.GetBytes(word);
                var bytHash = SHA512CSP.ComputeHash(bytValue);
                SHA512CSP.Clear();
                //根据计算得到的Hash码翻译为SHA-1码
                var sHash = "";
                foreach (var t in bytHash)
                {
                    long i = t / 16;
                    var sTemp = i > 9 ? ((char)(i - 10 + 0x41)).ToString() : ((char)(i + 0x30)).ToString();
                    i = t % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        private static string ToHashString(byte[] bytes, bool toUpper = true)
        {
            var builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }
            var str = builder.ToString();
            return toUpper ? str.ToUpper() : str.ToLower();
        }
    }
}
