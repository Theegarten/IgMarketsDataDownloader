using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace IgMarketsDataDownloader.Configuration
{
    public static class Config
    {
        //Location of the configuration file.
        private static readonly string CONFIGURATION_FILE_NAME = Environment.GetEnvironmentVariable(CONFIG_ENVIRONMENT_VARIABLE, EnvironmentVariableTarget.Machine);

        private const string DEFAULT_ENVIRONMENT = "testing";

        private const string CONFIG_ENVIRONMENT_VARIABLE = "LOCAL_CONFIG";

        private static readonly Lazy<JObject> JsonSettings = new Lazy<JObject>(() => JObject.Parse(File.ReadAllText(CONFIGURATION_FILE_NAME)));

        /// <summary>
        /// Gets the currently selected environment. If sub-environments are defined,
        /// they'll be returned as {env1}.{env2}
        /// </summary>
        /// <returns>The fully qualified currently selected environment</returns>
        public static string GetEnvironment()
        {
            try
            {
                var environments = new List<string>();
                JToken currentEnvironment = JsonSettings.Value;
                var env = currentEnvironment["environment"];
                while (env != null)
                {
                    var currentEnv = env.Value<string>();
                    environments.Add(currentEnv);
                    var moreEnvironments = currentEnvironment["environments"];
                    if (moreEnvironments == null)
                    {
                        break;
                    }

                    currentEnvironment = moreEnvironments[currentEnv];
                    env = currentEnvironment["environment"];
                }
                return string.Join(".", environments);
            }
            catch (Exception)
            {
                return DEFAULT_ENVIRONMENT;
            }
        }

        /// <summary>
        /// Get the matching config setting from the file searching for this key.
        /// </summary>
        /// <param name="key">String key value we're seaching for in the config file.</param>
        /// <param name="defaultValue"></param>
        /// <returns>String value of the configuration setting or empty string if nothing found.</returns>
        public static string Get(string key, string defaultValue = "")
        {
            // special case environment requests
            if (key == "environment") return GetEnvironment();

            var token = GetToken(JsonSettings.Value, key);
            return token.Value<string>();
        }

        /// <summary>
        /// Gets the underlying JToken for the specified key
        /// </summary>
        public static JToken GetToken(string key)
        {
            return GetToken(JsonSettings.Value, key);
        }

        /// <summary>
        /// Sets a configuration value. This is really only used to help testing. The key heye can be
        /// specified as {environment}.key to set a value on a specific environment
        /// </summary>
        /// <param name="key">The key to be set</param>
        /// <param name="value">The new value</param>
        public static void Set(string key, string value)
        {
            JToken environment = JsonSettings.Value;
            while (key.Contains("."))
            {
                var envName = key.Substring(0, key.IndexOf(".", StringComparison.Ordinal));
                key = key.Substring(key.IndexOf(".", StringComparison.Ordinal) + 1);
                environment = environment["environments"][envName];
            }
            environment[key] = value;
        }

        /// <summary>
        /// Get a boolean value configuration setting by a configuration key.
        /// </summary>
        /// <param name="key">String value of the configuration key.</param>
        /// <param name="defaultValue">The default value to use if not found in configuration</param>
        /// <returns>Boolean value of the config setting.</returns>
        public static bool GetBool(string key, bool defaultValue = false)
        {
            return GetValue(key, defaultValue);
        }

        /// <summary>
        /// Get the int value of a config string.
        /// </summary>
        /// <param name="key">Search key from the config file</param>
        /// <param name="defaultValue">The default value to use if not found in configuration</param>
        /// <returns>Int value of the config setting.</returns>
        public static int GetInt(string key, int defaultValue = 0)
        {
            return GetValue(key, defaultValue);
        }

        /// <summary>
        /// Get the double value of a config string.
        /// </summary>
        /// <param name="key">Search key from the config file</param>
        /// <param name="defaultValue">The default value to use if not found in configuration</param>
        /// <returns>Double value of the config setting.</returns>
        public static double GetDouble(string key, double defaultValue = 0.0)
        {
            return GetValue(key, defaultValue);
        }

        /// <summary>
        /// Gets a value from configuration and converts it to the requested type, assigning a default if
        /// the configuration is null or empty
        /// </summary>
        /// <typeparam name="T">The requested type</typeparam>
        /// <param name="key">Search key from the config file</param>
        /// <param name="defaultValue">The default value to use if not found in configuration</param>
        /// <returns>Converted value of the config setting.</returns>
        public static T GetValue<T>(string key, T defaultValue = default(T))
        {
            // special case environment requests
            if (key == "environment" && typeof(T) == typeof(string)) return (T)(object)GetEnvironment();

            var token = GetToken(JsonSettings.Value, key);
            if (token == null)
            {
                return defaultValue;
            }

            var type = typeof(T);
            var value = token.Value<string>();
            if (type.IsEnum)
            {
                return (T)Enum.Parse(type, value);
            }

            if (typeof(IConvertible).IsAssignableFrom(type))
            {
                return (T)Convert.ChangeType(value, type);
            }

            // try and find a static parse method
            try
            {
                var parse = type.GetMethod("Parse", new[] { typeof(string) });
                var result = parse.Invoke(null, new object[] { value });
                return (T)result;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Tries to find the specified key and parse it as a T, using
        /// default(T) if unable to locate the key or unable to parse it
        /// </summary>
        /// <typeparam name="T">The desired output type</typeparam>
        /// <param name="key">The configuration key</param>
        /// <param name="value">The output value</param>
        /// <returns>True on successful parse, false when output value is default(T)</returns>
        public static bool TryGetValue<T>(string key, out T value)
        {
            return TryGetValue(key, default(T), out value);
        }

        /// <summary>
        /// Tries to find the specified key and parse it as a T, using
        /// defaultValue if unable to locate the key or unable to parse it
        /// </summary>
        /// <typeparam name="T">The desired output type</typeparam>
        /// <param name="key">The configuration key</param>
        /// <param name="defaultValue">The default value to use on key not found or unsuccessful parse</param>
        /// <param name="value">The output value</param>
        /// <returns>True on successful parse, false when output value is defaultValue</returns>
        public static bool TryGetValue<T>(string key, T defaultValue, out T value)
        {
            try
            {
                value = GetValue(key, defaultValue);
                return true;
            }
            catch
            {
                value = defaultValue;
                return false;
            }
        }

        private static JToken GetToken(JToken settings, string key)
        {
            return GetToken(settings, key, settings.SelectToken(key));
        }

        private static JToken GetToken(JToken settings, string key, JToken current)
        {
            var environmentSetting = settings.SelectToken("environment");
            var environmentSettingValue = environmentSetting?.Value<string>();
            if (!string.IsNullOrWhiteSpace(environmentSettingValue))
            {
                var environment = settings.SelectToken("environments." + environmentSettingValue);
                var setting = environment.SelectToken(key);
                if (setting != null)
                {
                    current = setting;
                }
                return GetToken(environment, key, current);
            }
            if (current == null)
            {
                return settings.SelectToken(key);
            }
            return current;
        }
    }
}
