namespace Gateways.Cognito.Dtos.Response
{
    public record TokenUsuario
    {
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTimeOffset Expiry { get; set; }
    }
}
