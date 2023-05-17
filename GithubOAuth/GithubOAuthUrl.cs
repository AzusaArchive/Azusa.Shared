using System.Text;

namespace Azusa.Shared.GithubOAuth;

public static class GithubOAuthUrl
{
    /// <summary>
    /// 你应当让用户重定向到此URL进行登录
    /// </summary>
    public const string RequestIdentityUrl = "https://github.com/login/oauth/authorize";
    /// <summary>
    /// Post此URL向Github发送令牌
    /// </summary>
    public const string AccessTokenExchangeUrl = "https://github.com/login/oauth/access_token";
    /// <summary>
    /// Get此URL从Github获取用户信息
    /// </summary>
    public const string UserProfileUrl = "https://api.github.com/user";

    public static string BuildRequestIdentityUrl(GithubOAuthRequestParameter parameters)
    {
        var sb = new StringBuilder(RequestIdentityUrl);
        sb.Append("?client_id=").Append(parameters.ClientId);
        if (parameters.RedirectUri is not null) sb.Append("&redirect_url=").Append(parameters.RedirectUri);
        if (parameters.Login is not null) sb.Append("&login=").Append(parameters.Login);
        if (parameters.Scope is not null) sb.Append("&scope=").Append(parameters.Scope);
        if (parameters.State is not null) sb.Append("&state=").Append(parameters.State);
        sb.Append("&allow_signup=").Append(parameters.AllowSignup);
        return sb.ToString();
    }
}