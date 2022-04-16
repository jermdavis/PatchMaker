using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchMaker.Sitecore
{

    //https://devblogs.microsoft.com/nuget/Play-with-packages/
    public static class NugetDownloadWrapper
    {
        //ID of the package to be looked up
        private static readonly string packageID = "Sitecore.Kernel.NoReferences";
        private static readonly string feedUrl = "https://sitecore.myget.org/F/sc-packages/api/v2";
        private static readonly string downloadFolder = @".\nuget";
        private static readonly string version = "9.0.180604";
        private static readonly string kernelFile = @"Sitecore.Kernel.dll";

        public static bool DownloadRequired()
        {
            return !System.IO.File.Exists($@".\{kernelFile}");
        }

        public static bool PerformDownload()
        {
            try
            {
                // configure download
                var repo = PackageRepositoryFactory.Default.CreateRepository(feedUrl);
                PackageManager packageManager = new PackageManager(repo, downloadFolder);

                // perform download
                packageManager.InstallPackage(packageID, SemanticVersion.Parse(version));

                // tidy up after download
                System.IO.File.Move($@"{downloadFolder}\Sitecore.Kernel.NoReferences.{version}\lib\NET462\{kernelFile}", $@".\{kernelFile}");
                System.IO.Directory.Delete(downloadFolder, true);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }

}
