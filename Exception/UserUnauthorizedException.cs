using System.Runtime.Serialization;

namespace Azusa.Shared.Exception
{
    public class UserUnauthorizedException : System.Exception
    {
        public bool NotAuthentication { get; }

        public UserUnauthorizedException(bool notAuthentication = false) : base(notAuthentication
            ? "你需要登录才能请求这个接口"
            : "你所登录的用户没有权限请求这个接口")
        {
            NotAuthentication = notAuthentication;
        }

        public UserUnauthorizedException(string? message, bool notAuthentication = false) : base(message)
        {
            NotAuthentication = notAuthentication;
        }

        public UserUnauthorizedException(string? message, System.Exception? innerException) : base(message,
            innerException) { }

        protected UserUnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}