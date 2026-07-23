using System;
using System.Collections.Generic;
using System.IO;
using IniParser;
using IniParser.Model;
using IniParser.Model;

namespace Revive.Services
{
    public static class UpdateINI
    {
        private static string GetFilePath()
        {
            string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dataFolder = Path.Combine(baseFolder, "Revive");
            Directory.CreateDirectory(dataFolder);
            return Path.Combine(dataFolder, "Settings.ini");
        }

        public static void WriteToConfig(string section, string key, string value)
        {
            string filePath = GetFilePath();
            FileIniDataParser parser = new FileIniDataParser();

            IniData data = File.Exists(filePath) ? parser.ReadFile(filePath) : new IniData();
            data[section][key] = value;
            parser.WriteFile(filePath, data, null);
        }

        public static Dictionary<string, string> GetSection(string section)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string filePath = GetFilePath();

            if (!File.Exists(filePath))
            {
                return result;
            }

            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            foreach (KeyData key in data[section])
            {
                result[key.KeyName] = key.Value;
            }

            return result;
        }

        public static void RemoveKey(string section, string key)
        {
            string filePath = GetFilePath();
            if (!File.Exists(filePath))
            {
                return;
            }

            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);
            data[section].RemoveKey(key);
            parser.WriteFile(filePath, data, null);
        }

        public static string ReadValue(string section, string key)
        {
            string filePath = GetFilePath();
            FileIniDataParser parser = new FileIniDataParser();

            if (!File.Exists(filePath))
            {
                return "NONE";
            }

            IniData data = parser.ReadFile(filePath);
            string value = data[section][key];
            return string.IsNullOrEmpty(value) ? "NONE" : value;
        }
    }
}
