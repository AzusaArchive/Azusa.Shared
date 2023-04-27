using System;
using System.Runtime.Serialization;

namespace Azusa.Shared.Exception
{
    public class EntityNotFoundException : System.Exception
    {
        public Type? EntityType { get; set; }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(Type entityType):base($"无法找到{entityType.Name}类型实体")
        {
            EntityType = entityType;
        }

        public EntityNotFoundException(Type entityType,string? message) : base($"{message}，实体类型{entityType.Name}")
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
