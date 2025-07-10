using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;

namespace CefSharpDownloader
{
    class Program
    {

        public static void Main(string[] args)
        {
            Cef.Initialize(new CefSettings());
            MainAsync().Wait();
            Cef.Shutdown();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("!!!!!!!!!!!!!! THE END !!!!!!!!!!!!!!");
            Console.ReadKey();
        }

        public static async Task MainAsync()
        {
            var urls = new List<string>();
            for (var k = 0; k < 369; k++)
            {
                urls.Add($"https://uakino.best/filmy/page/{k + 1}/");
            }

            Console.WriteLine($"!!!!!!!!!! Before run application run vpn (Psiphon3 - Poland) !!!!!!!!");
            Console.WriteLine("Press any key to cntinue ...");
            Console.ReadKey();

            await DownloadPagesAsync(urls);
        }

        public static async Task DownloadPagesAsync(List<string> urls)
        {
            foreach (var url in urls.Take(3))
            {
                var ss = url.Split('/');
                var no = int.Parse(ss[ss.Length - 2]);
                var fileName = Path.Combine(@"D:\Temp\film\uakino-filmy", "uakino-filmy" + no.ToString("D5") + ".html");
                if (File.Exists(fileName)) continue;

                using (var browser = new ChromiumWebBrowser(url))
                {
                    var loaded = await browser.WaitForInitialLoadAsync();

                    if (loaded.Success)
                    {
                        var html = await browser.GetSourceAsync();
                        Debug.Print($"{html.Length} {url}");
                        Console.WriteLine($"{html.Length} {url}");
                        // File.WriteAllText(fileName, html);
                    }
                    else
                    {
                        Debug.Print($"Failed to load {url}");
                        Console.WriteLine($"Failed to load {url}");
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}
