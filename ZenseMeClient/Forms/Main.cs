using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using ZenseMe.Lib.DataAccessObjects;
using ZenseMe.Lib.Managers;
using ZenseMe.Lib.Objects;

namespace ZenseMe.Client.Forms
{
    public partial class Main : Form
    {
        private Device Device;
        private List<Device> DeviceList;
        private EntryObjectDAO EntryObjectDAO;

        public Main()
        {
            InitializeComponent();
        }

        public string ToolBarStatusText
        {
            get { return main_StatusText.Text; }
            set { main_StatusText.Text = value; }
        }

        public void GetDevice()
        {
            if (DeviceList == null)
            {
                DeviceManager DeviceManager = new DeviceManager();
                DeviceList = DeviceManager.GetDevices();
                Device = null;
            }

            if (DeviceList != null)
            {
                combo_DeviceSelect.Items.Clear();
                for (int i = 0; i < DeviceList.Count; i++)
                {
                    combo_DeviceSelect.Items.AddRange(new object[] { DeviceList[i].Manufacturer + " " + DeviceList[i].Model });
                }
            }
            else
            {
                ToolBarStatusText = "No device has been found, please connect a device.";
            }
        }

        private void DeviceComboBoxChanged(object sender, EventArgs e)
        {
            ComboBox ComboBoxSender = sender as ComboBox;
            Device = DeviceList[ComboBoxSender.SelectedIndex];
            InitializeTabs();
        }

        public void InitializeSummaryTab()
        {
            // Information
            EntryObjectDAO = new EntryObjectDAO();
            try
            {
                int TotalTracksStored = EntryObjectDAO.FetchAll() != null ? EntryObjectDAO.FetchAll().Count : 0;
                lbl_TotalTracksStored.Text = "Total tracks stored: " + TotalTracksStored;

                int TracksScrobbled = EntryObjectDAO.FetchSubmitted() != null ? EntryObjectDAO.FetchSubmitted().Count : 0;
                lbl_TracksScrobbled.Text = "Tracks scrobbled: " + TracksScrobbled;

                int TracksToScrobble = EntryObjectDAO.FetchNotSubmitted() != null ? EntryObjectDAO.FetchNotSubmitted().Count : 0;
                lbl_TracksToScrobble.Text = "Tracks to scrobble: " + TracksToScrobble;

                int IgnoredTracks = EntryObjectDAO.FetchIgnored() != null ? EntryObjectDAO.FetchIgnored().Count : 0;
                lbl_IgnoredTracks.Text = "Ignored tracks: " + IgnoredTracks;
            }
            catch
            {
                ToolBarStatusText = "There is nothing in your database, please fetch content from your device.";
            }

            // Device
            GetDevice();
            if (Device != null)
            {
                lbl_DevStatus.Text = "Status: Connected";
                lbl_DevName.Text = "Name: " + Device.FriendlyName;
                lbl_DevModel.Text = "Model: " + Device.Model;
                lbl_DevManufacturer.Text = "Manufacturer: " + Device.Manufacturer;

                if (Device.BatteryLevel > 0)
                {
                    lbl_DevBatteryLevel.Text = "Battery level: " + Device.BatteryLevel + "%";
                }
                else
                {
                    lbl_DevBatteryLevel.Text = "Battery level: Not supported";
                }
            }
            else
            {
                lbl_DevStatus.Text = "Status: Not connected";
                lbl_DevName.Text = "Name: ";
                lbl_DevModel.Text = "Model: ";
                lbl_DevManufacturer.Text = "Manufacturer: ";
                lbl_DevBatteryLevel.Text = "Battery level: ";
            }

            // Last.fm
            if (ConfigurationManager.AppSettings["LastFM_Username"] == "")
            {
                lbl_FMUsername.Text = "Username: None";
            }
            else
            {
                lbl_FMUsername.Text = "Username: " + ConfigurationManager.AppSettings["LastFM_Username"];
            }

            if (ConfigurationManager.AppSettings["Scrobble_LastDate"] == "")
            {
                lbl_FMLastScrobble.Text = "Last scrobble: Never";
            }
            else
            {
                lbl_FMLastScrobble.Text = "Last scrobble: " + ConfigurationManager.AppSettings["Scrobble_LastDate"];
            }
        }

        public void InitializeAllTracksTab()
        {
            EntryObjectDAO entryDAO = new EntryObjectDAO();

            try
            {
                main_AllTracksContentView.BindData(entryDAO.FetchAll(), false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Initialize error: " + ex);
            }
        }

        public void InitializeIgnoredTracksTab()
        {
            EntryObjectDAO entryDAO = new EntryObjectDAO();

            try
            {
                main_IgnoredTracksContentView.BindData(entryDAO.FetchIgnored(), true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Initialize error: " + ex);
            }
        }

        public void InitializeHistoryTracksTab()
        {
            EntryObjectDAO entryDAO = new EntryObjectDAO();

            try
            {
                main_HistoryContentView.BindData(entryDAO.FetchSubmitted(), false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Initialize error: " + ex);
            }
        }

        public void InitializeScrobbleTracksTab()
        {
            if (ConfigurationManager.AppSettings["Scrobble_LastDate"] == "Never")
            {
                main_SubmitDate.Value = DateTime.UtcNow;
            }
            EntryObjectDAO entryDAO = new EntryObjectDAO();

            try
            {
                main_SubmitContentView.BindData(entryDAO.FetchNotSubmitted(), true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Initialize error: " + ex);
            }
        }

        public void ScanDeviceForContent()
        {
            EntryObjectDAO = new EntryObjectDAO();
            GetDevice();

            EntryManager EntryManager = new EntryManager(Device);
            EntryManager.FoundNewMusicEntryEvent += new EntryManager.FoundTrackEntryDelegate(FoundEntryObject);
            EntryManager.BeginFetchContent();
        }

        int foundCount = 0;
        public void FoundEntryObject(EntryObject entry)
        {
            foundCount++;
            ToolBarStatusText = "Found " + foundCount.ToString() + " tracks - current artist: " + entry.Artist + " - album: " + entry.Album;
            EntryObjectDAO.SaveObject(entry);
        }

        private void _cSummaryTab_Enter(object sender, EventArgs e)
        {
            InitializeSummaryTab();
        }

        private void _cSubmitTab_Enter(object sender, EventArgs e)
        {
            InitializeScrobbleTracksTab();
        }

        private void _cAllTracksTab_Enter(object sender, EventArgs e)
        {
            InitializeAllTracksTab();
        }

        private void _cIgnoredTracksTab_Enter(object sender, EventArgs e)
        {
            InitializeIgnoredTracksTab();
        }

        private void _cHistoryTab_Enter(object sender, EventArgs e)
        {
            InitializeHistoryTracksTab();
        }

        private void _cSubmitTracksButton_Click(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["LastFM_Username"] == "" || ConfigurationManager.AppSettings["LastFM_Password"] == "")
            {
                MessageBox.Show("Please set your username and password in the settings menu.", "ZenseMe");

                Forms.Settings SettingsForm = new Forms.Settings(this);
                SettingsForm.Show();

                return;
            }
            if (main_SubmitContentView.CheckedItems.Count > 0)
            {
                ConfirmScrobble ConfirmScrobble = new ConfirmScrobble(this);
                PlayedTrackManager PlayedTrackManager = new PlayedTrackManager();
                List<EntryObject> PlayedTracks = PlayedTrackManager.GenerateScrobbleTimes(main_SubmitContentView.CheckedItems, main_SubmitDate.Value);

                ConfirmScrobble.Show();
                ConfirmScrobble.DataBind(PlayedTracks);
            }
            else
            {
                MessageBox.Show("You need to select a track to scrobble first.", "ZenseMe");
                return;
            }
        }

        private void _cSkipTracksButton_Click(object sender, EventArgs e)
        {
            if (main_SubmitContentView.CheckedItems.Count == 0)
            {
                MessageBox.Show("You need to select a track to skip first.", "ZenseMe");
                return;
            }

            DialogResult SkipYesNo = MessageBox.Show("Are you sure that you want to skip the selected tracks?\nYou won't be able to scrobble this track until it's next play.", "ZenseMe", MessageBoxButtons.YesNo);
            if (SkipYesNo == DialogResult.Yes)
            {
                ToolBarStatusText = "Now skipping tracks...";
                if (main_SubmitContentView.CheckedItems.Count > 0)
                {
                    IgnoreTrackDAO IgnoreTrackDAO = new IgnoreTrackDAO();
                    List<EntryObject> SkipTracks = main_SubmitContentView.CheckedItems;
                    IgnoreTrackDAO.SkipTrack(SkipTracks);

                    InitializeTabs();
                    ToolBarStatusText = "The selected tracks are now skipped.";
                }
                else
                {
                    MessageBox.Show("You need to select a track to skip first.", "ZenseMe");
                    return;
                }
            }
        }

        private void _cIgnoreTracksButton_Click(object sender, EventArgs e)
        {
            ToolBarStatusText = "Now ignoring tracks...";
            if (main_SubmitContentView.CheckedItems.Count > 0)
            {
                IgnoreTrackDAO IgnoreTrackDAO = new IgnoreTrackDAO();
                List<EntryObject> IgnoredTracks = main_SubmitContentView.CheckedItems;
                IgnoreTrackDAO.IgnoreTrack(IgnoredTracks);

                InitializeTabs();
                ToolBarStatusText = "The selected tracks are now ignored.";
            }
            else
            {
                MessageBox.Show("You need to select a track to ignore first.", "ZenseMe");
                return;
            }
        }

        private void _cUnignoreTracksButton_Click(object sender, EventArgs e)
        {
            ToolBarStatusText = "Now un-ignoring tracks...";
            if (main_IgnoredTracksContentView.CheckedItems.Count > 0)
            {
                IgnoreTrackDAO IgnoreTrackDAO = new IgnoreTrackDAO();
                List<EntryObject> IgnoredTracks = main_IgnoredTracksContentView.CheckedItems;
                IgnoreTrackDAO.UnignoreTrack(IgnoredTracks);

                InitializeTabs();
                ToolBarStatusText = "The selected tracks are now un-ignored.";
            }
            else
            {
                MessageBox.Show("You need to select a track to un-ignore first.", "ZenseMe");
                return;
            }
        }

        private void main_SelectAllButton_Click(object sender, EventArgs e)
        {
            main_SubmitContentView.SelectAll();
            main_IgnoredTracksContentView.SelectAll();
        }

        private void main_UnignoreSelectAllButton_Click(object sender, EventArgs e)
        {
            main_SubmitContentView.SelectAll();
            main_IgnoredTracksContentView.SelectAll();
        }

        private void _cSubmitSelectAllContextItem_Click(object sender, EventArgs e)
        {
            main_SubmitContentView.SelectAll();
            main_IgnoredTracksContentView.SelectAll();
        }

        private void _cSubmitSelectArtistContextItem_Click(object sender, EventArgs e)
        {
            main_SubmitContentView.SelectAllForArtist();
            main_IgnoredTracksContentView.SelectAllForArtist();
        }

        private void _cSubmitSelectAlbumContextItem_Click(object sender, EventArgs e)
        {
            main_SubmitContentView.SelectAllForAlbum();
            main_IgnoredTracksContentView.SelectAllForAlbum();
        }

        private void _cSubmitSelectDeviceContextItem_Click(object sender, EventArgs e)
        {
            main_SubmitContentView.SelectAllForDevice();
            main_IgnoredTracksContentView.SelectAllForDevice();
        }

        public void InitializeTabs()
        {
            InitializeSummaryTab();
            InitializeScrobbleTracksTab();
            InitializeHistoryTracksTab();
            InitializeIgnoredTracksTab();
            InitializeAllTracksTab();
        }

        private void ll_Help_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forms.Help TipsForm = new Forms.Help();
            TipsForm.Show();
        }

        private void ll_website_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://projects.arnoldvink.com");
        }

        private void ll_settings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forms.Settings SettingsForm = new Forms.Settings(this);
            SettingsForm.Show();
        }

        private void ll_refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            InitializeTabs();
        }

        private void ll_profile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string setProfileName = ConfigurationManager.AppSettings["LastFM_Username"];
            if (ConfigurationManager.AppSettings["LastFM_Username"] == "" || ConfigurationManager.AppSettings["LastFM_Password"] == "")
            {
                MessageBox.Show("Please set your username and password in the settings menu.", "ZenseMe");

                Forms.Settings SettingsForm = new Forms.Settings(this);
                SettingsForm.Show();

                return;
            }
            System.Diagnostics.Process.Start("http://last.fm/user/" + setProfileName);
        }

        private void ll_fetch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Device == null)
            {
                MessageBox.Show("Please connect or select a device before fetching songs.", "ZenseMe");
                return;
            }

            if (Process.GetProcessesByName("Zune").Length == 1)
            {
                MessageBox.Show("Please close Zune before fetching.", "ZenseMe");
                Environment.Exit(1);
            }

            if (Process.GetProcessesByName("songbird").Length == 1)
            {
                MessageBox.Show("Please close Songbird before fetching.", "ZenseMe");
                Environment.Exit(1);
            }

            if (Process.GetProcessesByName("MediaGo").Length == 1)
            {
                MessageBox.Show("Please close Media Go before fetching.", "ZenseMe");
                Environment.Exit(1);
            }

            if (Process.GetProcessesByName("MediaMonkey").Length == 1)
            {
                MessageBox.Show("Please close Media Monkey before fetching.", "ZenseMe");
                Environment.Exit(1);
            }

            foundCount = 0;
            ToolBarStatusText = "Starting to fetch songs, this can take a short while.";
            Console.WriteLine("Starting to fetch songs, this can take a short while.");
            ScanDeviceForContent();
            InitializeTabs();
        }
    }
}