using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProxyChecker
{
    // https://www.proxy-list.download/api/v1/get?type=https
    // https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all
    // sites: https://www.proxy-list.download/HTTP, https://free-proxy-list.net/

    public partial class Main : Form
    {
        private bool isClosing;
        public Main()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private string[] proxies;

        private void StartCheckProxies()
        {
            /*if (proxyType == "api")
                GetProxiesFromApi();
            else
                GetProxiesFromFile(@"E:\Temp\ProjectTests\ProxyList1.txt");*/

            proxies = proxies.Distinct().ToArray();
            label_TotalProxy.Text = proxies.Length.ToString();
            label_GoodProxy.Text = "0";
            label_BadProxy.Text = "0";

            var thread = new Thread(SubTask);
            thread.Start();
            thread.IsBackground = true;
        }

        private void GetProxiesFromApi(string url)
        {
            // var url = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all";
            // string url = @"https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list-raw.txt";

            using (var request = new WebClient())
            {
                var response = request.DownloadString(url).ToString();
                proxies = response.Split(new[] {Environment.NewLine, "\n"}, StringSplitOptions.None);
            }

            StartCheckProxies();
        }

        private void GetProxiesFromFile(string filename)
        {
            proxies = File.ReadAllLines(filename).Where(a => !string.IsNullOrEmpty(a) && !a.StartsWith("#")).Select(a => a.Split('\t')[0].Trim()).ToArray();
            StartCheckProxies();
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

        private void SubTask()
        {
            var tasks =
                from proxy in proxies.Where(a => !string.IsNullOrEmpty(a))
                select Task.Factory.StartNew(() => Checker(proxy));
            Task.WaitAll(tasks.ToArray());

            // Parallel.ForEach(proxies, new ParallelOptions { MaxDegreeOfParallelism = 8 }, (s) => checker(s));

            MessageBox.Show("Finished");
        }

        private void button_StartApi_Click(object sender, EventArgs e) => GetProxiesFromApi( "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all");

        private void btnStartFile_Click(object sender, EventArgs e)
        {
            var folder = @"E:\Temp\ProjectTests";
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = folder;
                ofd.RestoreDirectory = true;
                ofd.Multiselect = false;
                ofd.Filter = "Text|*.txt|All|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    GetProxiesFromFile(ofd.FileName);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => isClosing = true;
    }
}
