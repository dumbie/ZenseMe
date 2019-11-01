using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ZenseMe.Client.Forms
{
    public partial class Settings : Form
    {
        private Main _Frontend;

        public Settings(Main Frontend)
        {
            InitializeComponent();
            _Frontend = Frontend;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            lbl_version.Text = "By Arnold Vink\nv" + Application.ProductVersion;
            string LastFM_Username = ConfigurationManager.AppSettings["LastFM_Username"];
            string LastFM_Password = ConfigurationManager.AppSettings["LastFM_Password"];
            string Scrobble_BetweenTime = ConfigurationManager.AppSettings["Scrobble_BetweenTime"];
            string FetchAlbumArtist = ConfigurationManager.AppSettings["FetchAlbumArtist"];
            string FixUtcNowTime = ConfigurationManager.AppSettings["FixUtcNowTime"];
            string HttpsConnection = ConfigurationManager.AppSettings["HttpsConnection"];
            string version = Application.ProductVersion;

            txt_LastFMUsername.Text = LastFM_Username;
            txt_LastFMPassword.Text = LastFM_Password;
            txt_Scrobble_BetweenTime.Text = Scrobble_BetweenTime;
            cb_FetchAlbumArtist.Checked = FetchAlbumArtist == "1";
            cb_FixUtcNowTime.Checked = FixUtcNowTime == "1";
            cb_HttpsConnection.Checked = HttpsConnection == "1";
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void btn_Save_Click(object sender, EventArgs e)
        {
            Configuration Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string LastFM_Username = txt_LastFMUsername.Text;
            string LastFM_Password = CalculateMD5(txt_LastFMPassword.Text);
            string Scrobble_BetweenTime = txt_Scrobble_BetweenTime.Text;
            int FetchAlbumArtist = cb_FetchAlbumArtist.Checked ? 1 : 0;
            int FixUtcNowTime = cb_FixUtcNowTime.Checked ? 1 : 0;
            int HttpsConnection = cb_HttpsConnection.Checked ? 1 : 0;

            Configuration.AppSettings.Settings["LastFM_Username"].Value = LastFM_Username.ToString();

            if (Configuration.AppSettings.Settings["LastFM_Password"].Value != txt_LastFMPassword.Text.ToString())
            {
                Configuration.AppSettings.Settings["LastFM_Password"].Value = LastFM_Password.ToString();
            }

            Configuration.AppSettings.Settings["FetchAlbumArtist"].Value = FetchAlbumArtist.ToString();
            Configuration.AppSettings.Settings["FixUtcNowTime"].Value = FixUtcNowTime.ToString();
            Configuration.AppSettings.Settings["HttpsConnection"].Value = HttpsConnection.ToString();

            if (Convert.ToUInt32(Scrobble_BetweenTime) > 0 && Scrobble_BetweenTime != "")
            {
                Configuration.AppSettings.Settings["Scrobble_BetweenTime"].Value = Scrobble_BetweenTime.ToString();
            }
            else
            {
                MessageBox.Show("Double scrobble time must be atleast 1 second.", "ZenseMe");
                return;
            }

            Configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            Close();

            _Frontend.InitializeTabs();
            _Frontend.ToolBarStatusText = "Ready, your settings have been saved.";
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