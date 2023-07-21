using dotenv.net;

namespace BiboCareServices.Middleware
{
    public static class EnvMiddleware
    {
        //public static string? GetValue(string key)
        //{
        //    return Environment.GetEnvironmentVariable(key);
        //}

        //static EnvMiddleware()
        //{

        //}
        //public static string? GetValue(string key)
        //{
        //    return Environment.GetEnvironmentVariable(key);
        //}

        public static readonly IDictionary<string, string> ENV;
        static EnvMiddleware()
        {
            DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false));
            ENV = DotEnv.Read();
        }
        public static string? GetValue(string key)
        {
            return Environment.GetEnvironmentVariable(key) == null ? ENV[key] : Environment.GetEnvironmentVariable(key);
        }

    }
}