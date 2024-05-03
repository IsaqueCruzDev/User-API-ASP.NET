namespace BukiApi.Data
{
    public class AppSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string JWT_Secret { get; set; } = null!;
    }
}
