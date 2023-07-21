namespace BiboCareServices.Constants
{
    /// <summary>
    /// cái gì dùng chung cho cả project thì vất vào đây
    /// </summary>
    public class Global
    {
        public static string GetAppSetting(string variable)
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false)
                .Build();

            string myVariable = configuration[variable];

            return myVariable;
        }
    }
}
