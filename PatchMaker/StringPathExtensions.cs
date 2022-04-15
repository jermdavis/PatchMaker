using System;

namespace PatchMaker
{
    
    public static class StringPathExtensions
    {
        public static string GetLastPart(this string url)
        {
            var uri = new Uri(url);

            return uri.Segments[uri.Segments.Length - 1].Replace("/", "");
        }
    }

}