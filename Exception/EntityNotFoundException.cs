using System;
using System.Runtime.Serialization;

namespace Azusa.Shared.Exception
{
    public class EntityNotFoundException : System.Exception
    {
        public Type? EntityType { get; set; }

        public EntityNotFoundException():base("无法找到请求的对象")
        {
        }

        public EntityNotFoundException(Type entityType):base($"无法找到请求的{entityType.Name}对象")
        {
            EntityType = entityType;
        }

        public EntityNotFoundException(Type entityType,string? message) : base(message)
        {
            EntityType = entityType;
        }

        public EntityNotFoundException(string? message) : base(message)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public EntityNotFoundException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
