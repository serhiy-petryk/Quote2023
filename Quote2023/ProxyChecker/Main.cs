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
    public partial class Main : Form
    {
        private bool isClosing;
        public Main()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private string[] proxies;

        private void GetProxies(string proxyType)
        {
            if (proxyType == "api")
                GetProxiesFromApi();
            else
                GetProxiesFromFile(@"E:\Temp\ProjectTests\ProxyList1.txt");

            proxies = proxies.Distinct().ToArray();
            label_TotalProxy.Text = proxies.Length.ToString();

            var thread = new Thread(SubTask);
            thread.Start();
            thread.IsBackground = true;
        }

        private void GetProxiesFromApi()
        {
            var url = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all";
            // string url = @"https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list-raw.txt";

            using (var request = new WebClient())
            {
                var response = request.DownloadString(url).ToString();
                proxies = response.Split(new[] {Environment.NewLine, "\n"}, StringSplitOptions.None);
            }
        }

        private void GetProxiesFromFile(string filename)
        {
            proxies = File.ReadAllLines(filename).Where(a => !string.IsNullOrEmpty(a) && !a.StartsWith("#")).Select(a => a.Split('\t')[0].Trim()).ToArray();
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

        private void button_StartApi_Click(object sender, EventArgs e) => GetProxies("api");
        private void btnStartFile_Click(object sender, EventArgs e) => GetProxies("file");

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => isClosing = true;
    }
}
