namespace driveX_Api.CommonClasses
{
    public static class Utils
    {
        public static string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
