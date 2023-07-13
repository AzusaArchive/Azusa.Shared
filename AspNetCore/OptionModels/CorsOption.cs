namespace Azusa.Shared.AspNetCore.OptionModels
{
    public class CorsPolicyOptions
    {

        public class CorsPolicy
        {
            public required string Url { get; set; }
            public bool AllowAnyHeader { get; set; }
            public bool AllowCredentials { get; set; }
            public bool AllowAnyMethod { get; set; }
        }

        public required List<CorsPolicy> Policies { get; set; }
    }
}
