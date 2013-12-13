using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DownloadPodcasts
{
    class Program
    {
        static void Main(string[] args)
        {
            var pcastRgx = @"(?<pcastvidurl>http:\/\/[\w+\+d\-\/\.]*).(mp4|mp3|m4a)";
            var pcastUrl = @"https://itunes.apple.com/us/podcast/lifehacker-hd-mp4-30fps/id427598852"; //@"https://itunes.apple.com/podcast/tech-report-podcast-enhanced/id273853335?mt=2";  
            var vidHtml = new WebClient().DownloadString(pcastUrl);
            var vidUrls = GetVidUrls(vidHtml, pcastRgx);
            var pcastDir = @"C:\Downloads\PodCasts\LifeHacker\";
            int vidCnt = 1;

            if (!Directory.Exists(pcastDir))
            {
                Directory.CreateDirectory(pcastDir);
            }
            else
            {
                Console.WriteLine("The directory {0} already exists!", pcastDir);
            }
            try
            {

            
            Parallel.ForEach(vidUrls, match =>
                {

                    var fileName = Path.GetFileName(match.ToString());
                    Console.WriteLine("The file to be downloaded is {0}.\n It is file {1} of {2}.", fileName, vidCnt++, vidUrls.Count());
                    var wc = new WebClient();
                    
                    
                    wc.DownloadFileAsync(new Uri(match.ToString()), pcastDir + fileName);
                    Console.Read();

                });
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        // handle the 404 here
                    }
                }
                 
                throw;
            }
        }
        public static IEnumerable<string> GetVidUrls(string urlHtml, string urlRegex)
        {

            var rgx = new Regex(urlRegex);
            var content = rgx.Matches(urlHtml);
            var cleanMatches = content
                .OfType<Match>()
                .Select(c => c.Value)
                .Distinct();
            cleanMatches.ToList();

            foreach (var match in cleanMatches)
            {
                yield return match;
            }


        }

    }
}
