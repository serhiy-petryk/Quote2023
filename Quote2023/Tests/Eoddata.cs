using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using SQLitePCL;

namespace Tests
{
    class Eoddata

    {
        public static void Test()
        {
            var url = "https://www.eoddata.com/";
            var urls = new[]
            {
                "https://www.eoddata.com/Data/symbollist.aspx?e=AMEX",
                "https://www.eoddata.com/Data/symbollist.aspx?e=NASDAQ",
                "https://www.eoddata.com/Data/symbollist.aspx?e=NYSE"
            };
            // var files = new[] { "https://www.eoddata.com/Data/symbollist.aspx?e=AMEX", "https://www.eoddata.com/Data/symbollist.aspx?e=NASDAQ", "https://www.eoddata.com/Data/symbollist.aspx?e=NYSE" }
            var cookies = GetCookies(url);
            var result = new List<string>();

            using (var wc = new WebClientExt())
            {
                if (cookies.Count > 0)
                {
                    wc.cookieContainer = new CookieContainer();
                    foreach (var cookie in cookies)
                        wc.cookieContainer.Add(cookie);
                }

                var uri = new Uri(url);
                wc.Headers.Add(HttpRequestHeader.Referer, uri.Host);

                foreach (var urlDownload in urls)
                {
                    var bb = wc.DownloadData(urlDownload);
                    var response = Encoding.UTF8.GetString(bb);
                    result.Add(response);
                }
            }
        }

        public static List<Cookie> GetCookies(string hostname)
        {
            // Source: https://stackoverflow.com/questions/68643057/decrypt-google-cookies-in-c-sharp-net-framework

            if (hostname.EndsWith("/")) hostname = hostname.Substring(0, hostname.Length - 1);
            if (hostname.StartsWith("https://")) hostname = hostname.Substring(8);
            if (hostname.StartsWith("http://")) hostname = hostname.Substring(7);
            var hostKey = new List<string> {hostname};
            if (hostname.StartsWith("www.")) hostKey.Add(hostname.Substring(3));

            var whereString = string.Join("' or host_key = '", hostKey);

            var ChromeCookiePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Network\Cookies";
            var data = new List<Cookie>();
            // SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_dynamic_cdecl());
            if (File.Exists(ChromeCookiePath))
            {
                try
                {
                    using (var conn = new SqliteConnection($"Data Source={ChromeCookiePath}"))
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT name,encrypted_value,host_key FROM cookies WHERE host_key = '{whereString}'";
                        // cmd.CommandText = $"SELECT name,encrypted_value,host_key FROM cookies";
                        byte[] key = AesGcm256.GetKey();

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!data.Any(a => a.Name == reader.GetString(0)))
                                {
                                    // Debug.Print($"{reader[2]}\t{reader[0]}");
                                    byte[] encryptedData = GetBytes(reader, 1);
                                    byte[] nonce, ciphertextTag;
                                    AesGcm256.prepare(encryptedData, out nonce, out ciphertextTag);
                                    string value = AesGcm256.decrypt(ciphertextTag, key, nonce);

                                    data.Add(new Cookie()
                                    {
                                        Name = reader.GetString(0),
                                        Value = value,
                                        Domain = (string)reader["host_key"]
                                    });
                                }
                            }
                        }

                        conn.Close();
                    }
                }
                catch
                {
                }
            }

            return data;

        }

        private static byte[] GetBytes(SqliteDataReader reader, int columnIndex)
        {
            const int CHUNK_SIZE = 2 * 1024;
            byte[] buffer = new byte[CHUNK_SIZE];
            long bytesRead;
            long fieldOffset = 0;
            using (MemoryStream stream = new MemoryStream())
            {
                while ((bytesRead = reader.GetBytes(columnIndex, fieldOffset, buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, (int)bytesRead);
                    fieldOffset += bytesRead;
                }

                return stream.ToArray();
            }
        }

        /*public class Cookie
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }*/

        class AesGcm256
        {
            public static byte[] GetKey()
            {
                string sR = string.Empty;
                // var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                // string path = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Local State";
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +@"\Google\Chrome\User Data\Local State";
                //Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)

                string v = File.ReadAllText(path);

                dynamic json = JsonConvert.DeserializeObject(v);
                string key = json.os_crypt.encrypted_key;

                byte[] src = Convert.FromBase64String(key);
                byte[] encryptedKey = src.Skip(5).ToArray();

                byte[] decryptedKey = ProtectedData.Unprotect(encryptedKey, null, DataProtectionScope.CurrentUser);

                return decryptedKey;
            }

            public static string decrypt(byte[] encryptedBytes, byte[] key, byte[] iv)
            {
                string sR = String.Empty;
                try
                {
                    GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
                    AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, iv, null);

                    cipher.Init(false, parameters);
                    byte[] plainBytes = new byte[cipher.GetOutputSize(encryptedBytes.Length)];
                    Int32 retLen = cipher.ProcessBytes(encryptedBytes, 0, encryptedBytes.Length, plainBytes, 0);
                    cipher.DoFinal(plainBytes, retLen);

                    sR = Encoding.UTF8.GetString(plainBytes).TrimEnd("\r\n\0".ToCharArray());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }

                return sR;
            }

            public static void prepare(byte[] encryptedData, out byte[] nonce, out byte[] ciphertextTag)
            {
                nonce = new byte[12];
                ciphertextTag = new byte[encryptedData.Length - 3 - nonce.Length];

                System.Array.Copy(encryptedData, 3, nonce, 0, nonce.Length);
                System.Array.Copy(encryptedData, 3 + nonce.Length, ciphertextTag, 0, ciphertextTag.Length);
            }
        }
    }
}