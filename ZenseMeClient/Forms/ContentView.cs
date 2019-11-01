using System.Collections.Generic;
using System.Windows.Forms;
using ZenseMe.Lib.Objects;

namespace ZenseMe.Client.Forms
{
    public partial class ContentView : UserControl
    {
        private List<EntryObject> ConfirmScrobbleList;
        private bool _bAllSelected = false;

        public List<EntryObject> Entries
        {
            get { return ConfirmScrobbleList; }
            set { ConfirmScrobbleList = value; }
        }

        public List<EntryObject> CheckedItems
        {
            get
            {
                List<EntryObject> entries = new List<EntryObject>();

                foreach (ListViewItem listViewItem in _cTrackContentView.CheckedItems)
                {
                    entries.Add((EntryObject)listViewItem.Tag);
                }
                return entries;
            }
        }

        public List<EntryObject> SelectedItems
        {
            get
            {
                List<EntryObject> selectedEntries = new List<EntryObject>();

                foreach (ListViewItem item in _cTrackContentView.SelectedItems)
                {
                    selectedEntries.Add((EntryObject)item.Tag);
                }
                return selectedEntries;
            }
        }

        public ContentView()
        {
            InitializeComponent();
            ConfirmScrobbleList = new List<EntryObject>();
        }

        public void BindData(List<EntryObject> entries, bool withCheckboxes)
        {
            ConfirmScrobbleList = entries;
            _cTrackContentView.CheckBoxes = withCheckboxes;

            if (ConfirmScrobbleList == null || ConfirmScrobbleList.Count == 0)
            {
                _cTrackContentView.Items.Clear();
            }

            if (ConfirmScrobbleList != null && ConfirmScrobbleList.Count > 0)
            {
                _cTrackContentView.Items.Clear();
                foreach (EntryObject entry in ConfirmScrobbleList)
                {
                    string[] items = new string[] {
                        entry.Name,
                        entry.Artist,
                        entry.Album,
                        entry.LengthSeconds + " sec",
                        entry.PlayCount + "/" + entry.PlayCountHis,
                        entry.Device
                    };

                    ListViewItem listItem = new ListViewItem(items);
                    listItem.Tag = entry;

                    _cTrackContentView.Items.Add(listItem);
                }
            }
        }

        public void SelectAll()
        {
            foreach (ListViewItem listItem in _cTrackContentView.Items)
            {
                listItem.Checked = !_bAllSelected;
            }
            _bAllSelected = !_bAllSelected;
        }

        public void SelectAllForArtist()
        {
            List<string> alreadySelected = new List<string>();

            foreach (EntryObject entry in SelectedItems)
            {
                if (!alreadySelected.Contains(entry.Artist))
                {
                    SelectAllForArtist(entry.Artist);
                    alreadySelected.Add(entry.Artist);
                }
            }
        }

        public void SelectAllForArtist(string artist)
        {
            foreach (ListViewItem item in _cTrackContentView.Items)
            {
                if (((EntryObject)item.Tag).Artist == artist)
                {
                    item.Checked = true;
                }
            }
        }

        public void SelectAllForAlbum()
        {
            List<string> alreadySelected = new List<string>();

            foreach (EntryObject entry in SelectedItems)
            {
                if (!alreadySelected.Contains(entry.Album))
                {
                    SelectAllForAlbum(entry.Album);
                    alreadySelected.Add(entry.Album);
                }
            }
        }

        public void SelectAllForAlbum(string album)
        {
            foreach (ListViewItem item in _cTrackContentView.Items)
            {
                if (((EntryObject)item.Tag).Album == album)
                {
                    item.Checked = true;
                }
            }
        }

        public void SelectAllForDevice()
        {
            List<string> alreadySelected = new List<string>();

            foreach (EntryObject entry in SelectedItems)
            {
                if (!alreadySelected.Contains(entry.Device))
                {
                    SelectAllForDevice(entry.Device);
                    alreadySelected.Add(entry.Device);
                }
            }
        }

        public void SelectAllForDevice(string device)
        {
            foreach (ListViewItem item in _cTrackContentView.Items)
            {
                if (((EntryObject)item.Tag).Device == device)
                {
                    item.Checked = true;
                }
            }
        }

    }
}