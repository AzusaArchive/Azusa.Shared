namespace Azusa.Shared.DDD
{
    /// <summary>
    /// 工作单元，参数必须继承自DbContext，在方法结束后自动调用参数类型DbContext的SaveChangesAsync方法，以保持事务最终一致性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class, AllowMultiple = false,Inherited = true)]
    public class UnitOfWorkAttribute : Attribute
    {
        public Type[] DbContextTypes { get; set; }

        public UnitOfWorkAttribute(params Type[] dbContextTypes)
        {
            DbContextTypes = dbContextTypes;
            foreach (var type in dbContextTypes)
            {
                //使用IsAssignableTo而不是IsSubClassOf，此处要考虑Type传入的参数是否恰好为DbContext类型或者间接继承自DbContext
                if (!type.IsAssignableTo(typeof(Microsoft.EntityFrameworkCore.DbContext)))
                {
                    throw new ArgumentException($"{type}类型不是一个DbContext，参数{nameof(dbContextTypes)}元素的对象必须都继承自DbContext");
                }
            }
        }
    }
}
