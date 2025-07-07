namespace driveX_Api.Repository.Auth
{
    public interface IJwtToken
    {
        public string GenerateToken(string userId);
    }
}
