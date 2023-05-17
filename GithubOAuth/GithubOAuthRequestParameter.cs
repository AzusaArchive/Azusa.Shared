namespace Azusa.Shared.GithubOAuth;

/// <summary>
/// 用户重定向到Github登录URL上的参数模型，此对象中的属性应当作为查询字符串添加到URL上
/// </summary>
public class GithubOAuthRequestParameter
{
    /// <summary>
    /// “必需”。 注册时从 GitHub 收到的客户端 ID。
    /// </summary>
    public required string ClientId { get; set; }
    /// <summary>
    /// 用户获得授权后被发送到的应用程序中的 URL。 请参阅以下有关重定向 URL 的详细信息。
    /// </summary>
    public string? RedirectUri { get; set; }
    /// <summary>
    /// 提供用于登录和授权应用程序的特定账户。
    /// </summary>
    public string? Login { get; set; }
    /// <summary>
    /// 范围的空格分隔列表。 如果未提供，则 scope 默认为未授权应用程序的任何范围的用户的空列表。 对于已向应用程序授权作用域的用户，不会显示含作用域列表的 OAuth 授权页面。 相反，通过用户向应用程序授权的作用域集，此流程步骤将自动完成。 例如，如果用户已经执行了两次 Web 流，并且已授权一个具有 user 范围的令牌和另一个具有 repo 范围的令牌，则不提供 scope 的第三个 Web 流将收到具有 user 和 repo 范围的令牌。
    /// </summary>
    public string? Scope { get; set; }
    /// <summary>
    /// 不可猜测的随机字符串。 它用于防止跨站请求伪造攻击。
    /// </summary>
    public string? State { get; set; }
    /// <summary>
    /// 在 OAuth 流程中，是否向经过验证的用户提供注册 GitHub 的选项。 默认为 true。 在策略禁止注册时使用 false。
    /// </summary>
    public bool AllowSignup { get; set; }
}