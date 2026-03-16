using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.Configuration
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        [Required]
        public string Key { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Required]
        public int ExpireMinutes { get; set; } = 60;
    }
}
