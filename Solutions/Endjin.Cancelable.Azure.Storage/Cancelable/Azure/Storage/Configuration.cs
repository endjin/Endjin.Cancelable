namespace Endjin.Cancelable.Azure.Storage
{
    #region Using Directives

    using System.Collections.Concurrent;

    using Microsoft.Azure;

    #endregion

    /// <summary>
    /// Wrapper around the <see cref="CloudConfigurationManager"/> which provides a cache to improve performance.
    /// </summary>
    internal class Configuration
    {
        private static readonly ConcurrentDictionary<string, string> Cache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Retrieves setting via <see cref="CloudConfigurationManager"/> and caches the result for performance.
        /// </summary>
        /// <param name="settingName">Setting to retrieve.</param>
        /// <returns>Setting value.</returns>
        public static string GetSettingFor(string settingName)
        {
            string result;

            if (!Cache.TryGetValue(settingName, out result))
            {
                var setting = CloudConfigurationManager.GetSetting(settingName);
                Cache.TryAdd(settingName, setting);
                result = setting;
            }
            
            return result;
        }
    }
}