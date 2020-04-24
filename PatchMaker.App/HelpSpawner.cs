using Microsoft.Win32;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace PatchMaker.App
{

    public static class HelpSpawner
    {
        private static readonly string _helpFile = ".\\PatchMaker.App.Help.html";
        private static readonly string _oldInternetIconRegKey = @"HTTP\shell\open\command";
        private static readonly string _defaultBrowserRegKey = @"SOFTWARE\Microsoft\Windows\Shell\Associations\URLAssociations\http\UserChoice";

        private static string FallbackBrowserPath()
        {
            using (RegistryKey registrykey = Registry.ClassesRoot.OpenSubKey(_oldInternetIconRegKey, false))
            {
                var value = (string)registrykey.GetValue(null, null);

                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ConfigurationErrorsException($"Cannot retrieve a path to a web browser. Fallback '{_oldInternetIconRegKey}' reg key returned null.");
                }

                return value.Split('"')[1];
            }
        }

        private static string FetchBrowserProgId()
        {
            using (RegistryKey registrykey = Registry.CurrentUser.OpenSubKey(_defaultBrowserRegKey, false))
            {
                var browserProgId = (string)registrykey.GetValue("ProgId", null);

                if(string.IsNullOrWhiteSpace(browserProgId))
                {
                    browserProgId = FallbackBrowserPath();
                }

                return browserProgId;
            }
        }

        private static string FetchBrowserPath(string progId)
        {
            string browserKey = $@"{progId}\shell\open\command";
            using (RegistryKey registrykey = Registry.ClassesRoot.OpenSubKey(browserKey, false))
            {
                var cmd = (string)registrykey.GetValue(null, null);

                if(string.IsNullOrWhiteSpace(cmd))
                {
                    throw new ConfigurationErrorsException($"Cannot read executable path for browser ProgId '{progId}'");
                }

                return cmd.Split('"')[1];
            }            
        }

        // This hack is necessary because Process.Start() doesn't like urls with "#" in them,
        // and using "explorer <url>" strips the hash off the end before opening it.
        // https://stackoverflow.com/a/7231876/847953
        // https://stackoverflow.com/a/32355457/847953
        private static string GetDefaultBrowserPath()
        {
            var defaultBrowserProgId = FetchBrowserProgId();
            
            return FetchBrowserPath(defaultBrowserProgId);
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