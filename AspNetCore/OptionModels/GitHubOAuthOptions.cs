namespace Azusa.Shared.AspNetCore.OptionModels;

/// <summary>
/// Github认证配置
/// 详见 https://docs.github.com/zh/apps/oauth-apps/building-oauth-apps/authorizing-oauth-apps
/// </summary>
public class GitHubOAuthOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string RedirectUri { get; set; }
    public required string ApplicationName { get; set; }
}