namespace BookManagementAPI.Utilities
{

    static class ConfigurationManager
    {
        public static IConfiguration AppSetting { get; set; }
        static ConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
    public static class Utility
    {
        #region Get App Settings
        /// <summary>
        /// Get the appsetting value from the web.config
        /// </summary>
        /// <param name="appKey">appsettings key</param>
        /// <returns></returns>
        public static string GetAppSettings(string key)
        {
            var value = ConfigurationManager.AppSetting.GetSection("AppSettings:" + key).Value;
            if (value != null)
            {
                return value.ToString();
            }
            return "-";
        }
        #endregion
    }
}
