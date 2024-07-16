namespace API.Shared
{
    public sealed class ContextSettings
    {
        public const string ConnectionString = "Data Source=.;Initial Catalog=ApplicationDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    }

    public sealed class JWTSettings
    {
        public const string Secret = "7xnC3Ew6zjQAfgyqWaCrvFqSEb6zFEVC285FCbBNm4YsansX9TYeaeWF2SQj8CLLmtgPuHdEn3Hp2YWLvLQfBDLe7dp6FKGR3zg8S6p6KV5BLRXRdDWF7kw5pJLEj4mX";
    }

    public sealed class AuthorizeRoles
    {
        public const string Admin = "Admin";
        public const string Writer = "Writer";
        public const string User = "User";
    }

    public sealed class AdminSafeListIPs
    {
        public const string IPList = "127.0.0.1;::1;194.147.142.136;192.168.1.7";
    }
}
