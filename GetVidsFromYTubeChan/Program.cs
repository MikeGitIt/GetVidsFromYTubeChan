using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace GetVidsFromYTubeChan
{
    class Program
    {
        static void Main()
        {
            const string vidUrl = @"https://www.youtube.com/user/CodeFurnace/videos"; //@"http://www.youtube.com/channel/UC_URNKseCV0eoi2Sfox8sNQ/videos";
            //var wc = new WebClient();
            var vidUrlHtml = new WebClient().DownloadString(vidUrl);
            const string vidRegex = @"(?<yturl>watch\?v[A-Za-z0-9(=_\-)]*)";
            var vidsToDwnLd = GetVidUrls(vidUrlHtml, vidRegex);
            const string dwnldDir = @"C:/Downloads/CodeFurnace";

            if (!Directory.Exists(dwnldDir))
            {
                Directory.CreateDirectory(dwnldDir);
            }
            else
            {
                Console.Write("Directory {0} already exists.", dwnldDir);
            }
            Parallel.ForEach(vidsToDwnLd, vtd =>

                {

                    var nmatch = "http://www.youtube.com/" + vtd;

                    IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(nmatch);
                    VideoInfo video = videoInfos.First(info =>  info.VideoType == VideoType.Mp4 && info.Resolution == 720 || info.Resolution == 480 || info.Resolution == 360);

                    var videoDownloader = new VideoDownloader(video, Path.Combine(dwnldDir, video.Title + video.VideoExtension));
                    videoDownloader.DownloadProgressChanged += (sender, args2) => Console.WriteLine(args2.ProgressPercentage);
                     Console.WriteLine(videoDownloader.ToString());
                    if (!File.Exists(videoDownloader.ToString()))
                    {
                        if (video.Title == @"Introduction to jQuery and AJAX Web Forms")


                        Console.WriteLine("The name of the dowloading video is \"{0}\" and the resolution is {1}.\n", video.Title, video.Resolution);
                        videoDownloader.Execute();
                    }
                    else
                    {
                        Console.WriteLine("File {0} already exists");
                    }
                    Console.WriteLine(vtd);



                });

            Console.Read();
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


}}
}
