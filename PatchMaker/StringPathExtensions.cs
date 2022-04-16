using System;

namespace PatchMaker
{
    
    public static class StringPathExtensions
    {
        public static string GetLastPart(this string url)
        {
            if(Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return uri.Segments[uri.Segments.Length - 1].Replace("/", "");
            }

            return url;
            
        }
    }

}