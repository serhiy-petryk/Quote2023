using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ProxyChecker
{
    // https://www.proxy-list.download/api/v1/get?type=https
    // https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all
    // sites: https://www.proxy-list.download/HTTP, https://free-proxy-list.net/

    // https://www.proxy-list.download/api/v2/get?l=en&t=http

    public partial class Main : Form
    {
        private bool isClosing;
        public Main()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void StartCheckProxies(string[] proxies)
        {
            var uniqueProxies = proxies.Distinct().ToArray();

            label_TotalProxy.Text = int.Parse(label_TotalProxy.Text) + uniqueProxies.Length.ToString();

            var tasks =
                from proxy in uniqueProxies.Where(a => !string.IsNullOrEmpty(a))
                select Task.Factory.StartNew(() => Checker(proxy));
            Task.WaitAll(tasks.ToArray());
        }

        private void SetProxiesFromProxyScrape()
        {
            var url = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all";
            string[] proxies;
            using (var request = new WebClient())
            {
                var response = request.DownloadString(url).ToString();
                proxies = response.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
            }
            StartCheckProxies(proxies);
        }

        private void SetProxiesFromProxyListDownload()
        {
            var url = "https://www.proxy-list.download/api/v2/get?l=en&t=http";
            string[] proxies;
            using (var request = new WebClient())
            {
                var response = request.DownloadString(url).ToString();
                var oo = JsonConvert.DeserializeObject<cRoot>(response);
                proxies = oo.LISTA.Select(a => $"{a.IP}:{a.PORT}").ToArray();
            }
            StartCheckProxies(proxies);
        }

        private void SetProxiesFromFreeProxyList()
        {
            var url = "https://free-proxy-list.net/";
            string[] proxies;
            using (var request = new WebClient())
            {
                var response = request.DownloadString(url).ToString();
                var i1 = response.IndexOf("<textarea class=\"form-control\"", StringComparison.InvariantCultureIgnoreCase);
                var i2 = response.IndexOf("</textarea>", i1 + 30, StringComparison.InvariantCultureIgnoreCase);
                proxies = response.Substring(i1 + 30, i2 - i1 - 30)
                    .Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(a => !string.IsNullOrEmpty(a) && char.IsDigit(a[0])).ToArray();
            }
            StartCheckProxies(proxies);
        }

        [DebuggerStepThrough]
        private void Checker(string proxy)
        {
            // string testUrl = "https://google.com?a=" + cnt.ToString(); // 60K
            var testUrl = "https://www.find-ip.net";
            using (var request = new WebClient())
            {
                try
                {
                    request.Headers[HttpRequestHeader.UserAgent] = @"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
                    // request.Headers[HttpRequestHeader.UserAgent] = @"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
                    // request.Headers[HttpRequestHeader.UserAgent] = @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                    // request.Proxy = HttpProxyClient.Parse(proxy);
                    request.Proxy = new WebProxy(proxy);

                    var sw = new Stopwatch();
                    sw.Start();
                    var response = request.DownloadString(testUrl).ToString();
                    sw.Stop();
                    textBox_Results.Text += proxy + "\t" + sw.ElapsedMilliseconds + Environment.NewLine;
                    label_GoodProxy.Text = (int.Parse(label_GoodProxy.Text) + 1).ToString();
                    WriteToSql(proxy, sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    if (!isClosing)
                        label_BadProxy.Text = (int.Parse(label_BadProxy.Text) + 1).ToString();
                }
            }
        }

        private void WriteToSql(string proxy, long? swElapsedMilliseconds)
        {
            var DbConnectionString = "Data Source=localhost;Initial Catalog=dbQuote2022;Integrated Security=True;Connect Timeout=150;";
            using (var conn = new SqlConnection(DbConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = $"INSERT into ProxyChecker (Proxy, Date) VALUES ('{proxy}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}')";
                if (swElapsedMilliseconds.HasValue)
                    cmd.CommandText = $"INSERT into ProxyChecker (Proxy, Date, Duration) VALUES ('{proxy}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}', {swElapsedMilliseconds})";
                cmd.ExecuteNonQuery();
            }
        }

        private void button_StartApi_Click(object sender, EventArgs e)
        {
            label_TotalProxy.Text = "0";
            label_GoodProxy.Text = "0";
            label_BadProxy.Text = "0";
            Task.Factory.StartNew(() =>
            {
                btnStartApi.Enabled = false;

                if (cbProxyScrape.Checked)
                {
                    SetProxiesFromProxyScrape();
                    cbProxyScrape.Checked = false;
                }

                if (cbProxyListDownload.Checked)
                {
                    SetProxiesFromProxyListDownload();
                    cbProxyListDownload.Checked = false;
                }

                if (cbFreeProxyList.Checked)
                {
                    SetProxiesFromFreeProxyList();
                    cbFreeProxyList.Checked = false;
                }

                MessageBox.Show("Finished");
                btnStartApi.Enabled = true;
            });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => isClosing = true;

        // =========================
        public class cRoot
        {
            public cProxyItem[] LISTA;
        }
        public class cProxyItem
        {
            public string IP;
            public string PORT;
        }
    }
}
