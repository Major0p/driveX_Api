namespace driveX_Api.DTOs.JWT
{
    public class JwtToken
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expiration { get; set; }
    }
}
