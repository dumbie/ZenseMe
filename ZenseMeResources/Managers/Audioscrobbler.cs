using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace ZenseMe.Lib.Managers
{
    public class Audioscrobbler
    {
        private string ConfigUsername = ConfigurationManager.AppSettings["LastFM_Username"];
        private string ConfigPassword = ConfigurationManager.AppSettings["LastFM_Password"];
        private string Challenge;
        private string NowPlayUrl;
        private string SubmitUrl;

        public bool SubmitTrack(string artist, string name, string album, int length, DateTime dateSubmitted)
        {
            try
            {
                DateTimeFormatInfo DateTimeInfo = new DateTimeFormatInfo();
                DateTimeInfo.ShortDatePattern = @"dd/MM/yyyy HH:mm:ss";

                TimeSpan MinTimeZone = DateTime.Now - DateTime.UtcNow;
                long TrackUnixTime = dateSubmitted.Ticks - MinTimeZone.Ticks - DateTime.Parse("01/01/1970 00:00:00", DateTimeInfo).Ticks;
                TrackUnixTime /= 10000000;

                string s = HttpUtility.UrlEncode(Challenge);
                string i = HttpUtility.UrlEncode(TrackUnixTime.ToString());
                string o = "P";
                string r = "";
                string n = "";
                string m = "";

                HttpWebResponse HttpWebResponse = runRequest(SubmitUrl, string.Format("&s={0}&a[0]={1}&t[0]={2}&i[0]={3}&o[0]={4}&r[0]={5}&l[0]={6}&b[0]={7}&n[0]={8}&m[0]={9}", new object[] { s, HttpUtility.UrlEncode(artist), HttpUtility.UrlEncode(name), i, o, r, length, HttpUtility.UrlEncode(album), n, m }));
                List<string> SubmitResponse = getResponseList(HttpWebResponse);

                if (SubmitResponse[0].Contains("OK"))
                {
                    Console.WriteLine("The track has been scrobbled!");
                    return true;
                }
                else if (SubmitResponse[0].Contains("BADSESSION"))
                {
                    Console.WriteLine("Invalid Session ID!");
                    MessageBox.Show("Invalid Session ID!", "ZenseMe");
                    return false;
                }
                else if (SubmitResponse[0].Contains("FAILED"))
                {
                    Console.WriteLine("Failed last.fm error: " + SubmitResponse[0]);
                    MessageBox.Show("Failed last.fm error.", "ZenseMe");
                    return false;
                }
                else
                {
                    Console.WriteLine("Failed last.fm unknown error.");
                    MessageBox.Show("Failed last.fm unknown error.", "ZenseMe");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Submit error: " + ex);
                MessageBox.Show("Failed to submit track to the last.fm server.", "ZenseMe");
                return false;
            }
        }

        public bool ConnectLastfm()
        {
            try
            {
                DateTimeFormatInfo DateTimeInfo = new DateTimeFormatInfo();
                DateTimeInfo.ShortDatePattern = @"dd/MM/yyyy HH:mm:ss";

                DateTime TimeZone = DateTime.UtcNow;
                if (int.Parse(ConfigurationManager.AppSettings["FixUtcNowTime"]) >= 1) { TimeZone = DateTime.UtcNow.AddHours(1); }

                long ConnectUnixTime = TimeZone.Ticks - DateTime.Parse("01/01/1970 00:00:00", DateTimeInfo).Ticks;
                ConnectUnixTime /= 10000000;

                string LastFmToken = CalculateMD5(ConfigPassword + ConnectUnixTime.ToString());
                string hs = "true";
                string p = "1.2.1";
                string c = "wmp";
                string v = "1.0";
                string u = HttpUtility.UrlEncode(ConfigUsername);
                string t = HttpUtility.UrlEncode(ConnectUnixTime.ToString());
                string a = HttpUtility.UrlEncode(LastFmToken);
                string postData = string.Format("hs={0}&p={1}&c={2}&v={3}&u={4}&t={5}&a={6}", new object[] { hs, p, c, v, u, t, a });

                string ApiUrl = "http://post.audioscrobbler.com/";
                if (int.Parse(ConfigurationManager.AppSettings["HttpsConnection"]) >= 1) { ApiUrl = "https://post.audioscrobbler.com/"; }

                HttpWebResponse HttpWebResponse = runRequest(ApiUrl, postData);
                List<string> SubmitResponse = getResponseList(HttpWebResponse);

                if (SubmitResponse[0].Contains("OK"))
                {
                    Console.WriteLine("Authed ok with Last.fm servers.");
                    Challenge = SubmitResponse[1];
                    NowPlayUrl = SubmitResponse[2];
                    SubmitUrl = SubmitResponse[3];
                    return true;
                }
                else if (SubmitResponse[0].Contains("BANNED"))
                {
                    Console.WriteLine("This client is banned from last.fm.");
                    MessageBox.Show("Failed to authenticate with Last.fm, this client is banned from last.fm.", "ZenseMe");
                    return false;
                }
                else if (SubmitResponse[0].Contains("BADAUTH"))
                {
                    Console.WriteLine("Wrong username or password.");
                    MessageBox.Show("Failed to authenticate with Last.fm, wrong username or password is used.", "ZenseMe");
                    return false;
                }
                else if (SubmitResponse[0].Contains("BADTIME"))
                {
                    Console.WriteLine("Invalid time stamp.");
                    MessageBox.Show("Failed to authenticate with Last.fm, invalid time stamp check your time.", "ZenseMe");
                    return false;
                }
                else if (SubmitResponse[0].Contains("FAILED"))
                {
                    Console.WriteLine("Failed last.fm error: " + SubmitResponse[0]);
                    MessageBox.Show("Failed to authenticate with Last.fm, failed last.fm error.", "ZenseMe");
                    return false;
                }
                else
                {
                    Console.WriteLine("Failed last.fm unknown error.");
                    MessageBox.Show("Failed to authenticate with Last.fm, failed last.fm unknown error.", "ZenseMe");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connect error: " + ex);
                MessageBox.Show("Failed to authenticate with Last.fm, please try again.", "ZenseMe");
                return false;
            }
        }

        private List<string> getResponseList(HttpWebResponse HttpWebResponse)
        {
            try
            {
                List<string> toReturn = new List<string>();
                StreamReader StreamReader = new StreamReader(HttpWebResponse.GetResponseStream());
                char[] Buffer = new char[1024];
                for (int StreamCount = StreamReader.Read(Buffer, 0, 1024); StreamCount > 0; StreamCount = StreamReader.Read(Buffer, 0, 1024))
                {
                    string resultData = new string(Buffer, 0, StreamCount);
                    if (resultData.EndsWith("\n")) { toReturn.Add(resultData.Replace("\n", "")); }
                }
                return toReturn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Response list error: " + ex);
                //MessageBox.Show("No response from the last.fm server.", "ZenseMe");
                return null;
            }
        }

        public HttpWebResponse runRequest(string url, string postData)
        {
            try
            {
                byte[] Buffer = new UTF8Encoding().GetBytes(postData);
                HttpWebRequest HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebRequest.Method = "POST";
                HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                HttpWebRequest.Timeout = 10000;
                HttpWebRequest.ContentLength = Buffer.Length;
                Stream Stream = HttpWebRequest.GetRequestStream();
                Stream.Write(Buffer, 0, Buffer.Length);
                Stream.Close();
                return (HttpWebResponse)HttpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Run request error: " + ex);
                //MessageBox.Show("Failed to request from the last.fm server.", "ZenseMe");
                return null;
            }
        }

        private string CalculateMD5(string input)
        {
            MD5 md = MD5.Create();
            byte[] buffer = new UTF8Encoding().GetBytes(input);
            byte[] hash = md.ComputeHash(buffer);
            string md5 = string.Empty;
            for (int i = 0; i < hash.Length; i++)
            {
                md5 = md5 + hash[i].ToString("x2");
            }
            return md5;
        }
    }
}