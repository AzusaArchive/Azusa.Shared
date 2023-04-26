using System.Runtime.Serialization;

namespace Azusa.Shared.Exception
{
    public class UserUnauthorizedException : System.Exception
    {
        public UserUnauthorizedException() : base("用户授权失败，请验证登录状态。")
        {
        }

        public UserUnauthorizedException(string? message) : base(message)
        {
        }

        public UserUnauthorizedException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected UserUnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
