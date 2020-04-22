using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace PatchMaker.App
{
    
    public static class HelpSpawner
    {
        private static string _helpFile = ".\\PatchMaker.App.Help.html";

        // This hack is necessary because Process.Start() doesn't like urls with "#" in them,
        // and using "explorer <url>" strips the hash off the end before opening it.
        // https://stackoverflow.com/a/7231876/847953
        private static string GetDefaultBrowserPath()
        {
            string key = @"HTTP\shell\open\command";
            using (RegistryKey registrykey = Registry.ClassesRoot.OpenSubKey(key, false))
            {
                return ((string)registrykey.GetValue(null, null)).Split('"')[1];
            }
        }

        public static void SpawnLocalFile(string part)
        {
            var file = Path.GetFullPath(_helpFile);

            if (!string.IsNullOrEmpty(part))
            {
                file = $"{file}#{part}";
            }

            ProcessStartInfo sInfo = new ProcessStartInfo()
            {
                FileName = GetDefaultBrowserPath(),
                Arguments = $"file:///{file}"
            };
            Process.Start(sInfo);
        }

        public static void SpawnUrl(string url)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo() { 
                FileName = url
            };
            Process.Start(sInfo);
        }
    }

}