// ReSharper disable InconsistentNaming
namespace Azusa.Shared.GithubOAuth;

/// <summary>
/// <para>用于请求Github访问令牌的数据模型</para>
/// <para>https://docs.github.com/zh/apps/oauth-apps/building-oauth-apps/authorizing-oauth-apps#1-request-a-users-github-identity</para>
/// </summary>
public class GithubGetAccessTokenRequest
{
    /// <summary>
    /// 必填。 从 GitHub 收到的 OAuth App 的客户端 ID。
    /// </summary>
    public string client_id { get; set; }
    /// <summary>
    /// 必填。 从 GitHub 收到的 OAuth App 的客户端密码。
    /// </summary>
    public string client_secret { get; set; }
    /// <summary>
    /// 必填。 收到的作为对步骤 1 的响应的代码。
    /// </summary>
    public string code { get; set; }
    /// <summary>
    /// 用户获得授权后被发送到的应用程序中的 URL。
    /// </summary>
    public string? redirect_uri { get; set; }

    public GithubGetAccessTokenRequest(string clientId, string clientSecret, string code, string? redirectUri)
    {
        client_id = clientId;
        client_secret = clientSecret;
        this.code = code;
        redirect_uri = redirectUri;
    }
}