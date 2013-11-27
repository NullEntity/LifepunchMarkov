using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;
using System.Configuration;
using LinqToTwitter;

namespace LifepunchMarkov
{
    class Program
    {
        static readonly int DAYS = 5;   // load 5 days of logs

        static Chain chain = new Chain();

        static void Main(string[] args)
        {
            var start = DateTime.Now;
            Load();
            var elapsed = DateTime.Now - start;
            Console.WriteLine("Loaded in {0}", elapsed);

            for (; ; )
            {
                String msg;
                do
                    msg = chain.BuildString();
                while (msg.StartsWith("*EXACT MATCH!!!*"));

                Console.WriteLine("Output: " + msg);
                Console.ReadKey(true);
            }
        }

        static WebClient web = new WebClient();
        static void Load()
        {
            String[] logURLs = {
                                   // Deathrun
                                   "http://chatlogs.lifepunch.net/Deathrun%20LifePunch/",
                                   "http://chatlogs.lifepunch.net/Deathrun%20LifePunch%202/",
                                   "http://chatlogs.lifepunch.net/Deathrun%20LifePunch%203/",
                                   "http://chatlogs.lifepunch.net/Deathrun%20LifePunch%204/",
                                   "http://chatlogs.lifepunch.net/Deathrun%20LifePunch%205/",
                                   // Gungame
                                   "http://chatlogs.lifepunch.net/Gungame%20LifePunch/",
                                   "http://chatlogs.lifepunch.net/Gungame%20LifePunch%202/",
                                   // Cinema
                                   "http://chatlogs.lifepunch.net/Cinema%20LifePunch/",
                                   // Jailbreak
                                   "http://jbchatlogs.lifepunch.net/Jailbreak%20LifePunch/",
                                   "http://jbchatlogs.lifepunch.net/Jailbreak%20LifePunch%202/",
                                   "http://jbchatlogs.lifepunch.net/Jailbreak%20LifePunch%203/",
                                   "http://jbchatlogs.lifepunch.net/Jailbreak%20LifePunch%204/",
                                   "http://jbchatlogs.lifepunch.net/Jailbreak%20LifePunch%205/",
                                   "http://jbchatlogs.lifepunch.net/Jailbreak%20LifePunch%206/",
                                   "http://jbchatlogs.lifepunch.net/Jailbreak%20LifePunch%207/",
                                   "http://jbchatlogs.lifepunch.net/Jailbreak%20LifePunch%208/",
                                   "http://jbchatlogs.lifepunch.net/Jailbreak%20LifePunch%209/",
                               };

            Console.WriteLine("Downloading chat logs...");

            foreach (String url in logURLs)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(web.DownloadString(url));

                var nodes = doc.DocumentNode.SelectNodes("//a[@href and not(.='Go Back')]");
                foreach (var node in nodes.Take(DAYS))
                //foreach (var node in nodes)
                    ParseLog(url + node.Attributes["href"].Value);
            }
        }

        static Regex logParser = new Regex(@"[\d/]{8} - [\d:]{8} - .* \(STEAM_[01]:[01]:\d{1,10}\): (.*)");
        static void ParseLog(String url)
        {
            Console.WriteLine("Downloading " + url);

            var doc = new HtmlDocument();
            var html = web.DownloadString(url);
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes("//p");
            foreach(var node in nodes)
            {
                // 11/21/13 - 00:00:02 - xoNinja (STEAM_0:0:72405340): wow
                // /[\d/]{8} - [\d:]{8} - .* \(STEAM_[01]:[01]:\d{1,10}\): (.*)/
                String message = logParser.Match(node.InnerText).Groups[1].Value;
                message = message.Replace("\\\'", "\'").Replace("&lt;", "<").Replace("&gt;", ">");
                chain.AddText(message.ToLower());
            }

            Console.WriteLine("Done parsing!");
        }
    }
}
