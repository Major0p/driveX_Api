namespace driveX_Api.CommonClasses
{
    public class Utils
    {
        public string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
