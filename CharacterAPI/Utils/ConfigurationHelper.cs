using Newtonsoft.Json;

namespace CharacterAPI.Utils
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;

        // 初始化IConfiguration对象
        public static void InitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 获取DatabaseSettings
        public static T GetDatabaseSettings<T>(string key)
        {
           return _configuration.GetSection(key).Get<T>();
        }
    }
}
