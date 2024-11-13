namespace Azusa.Shared.ModelValidation.FluentValidation;

/// <summary>
/// 自动校验特性，在ASP.NETCore操作方法的参数上标注，使用筛选器进行自动校验
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class AutoFluentValidateAttribute : Attribute
{
    
}