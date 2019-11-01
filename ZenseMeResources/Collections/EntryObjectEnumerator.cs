using PortableDeviceApiLib;
using System.Collections;
using System.Collections.Generic;
using ZenseMe.Lib.Managers;
using ZenseMe.Lib.Objects;

namespace ZenseMe.Lib.Collections
{
    public class EntryObjectEnumerator : IEnumerator<EntryObject>
    {
        private IPortableDeviceContent _hDeviceContent;
        private EntryManager _hEntryManager;
        private EntryObject _oCurrentEntryObject;
        private string _sCurrentObjectId;
        private int _iCurrentLevel;
        private List<EntryObject> _lEntryObjects;
        private int _iLastPosition = 0;

        public EntryObjectEnumerator(Device device)
        {
            Reset();

            _hEntryManager = new EntryManager(device);
            PortableDeviceClass deviceClass = new PortableDeviceClass();
            deviceClass.Open(device.Id, DeviceManager.ClientValues);
            deviceClass.Content(out _hDeviceContent);

            _lEntryObjects = new List<EntryObject>();

            FindChild(_sCurrentObjectId, _iCurrentLevel);
        }

        public EntryObject Current
        {
            get
            {
                return _oCurrentEntryObject;
            }
        }

        public void Dispose()
        {
            Reset();
            _hEntryManager = null;
            _hDeviceContent = null;
            _oCurrentEntryObject = null;
        }

        object IEnumerator.Current
        {
            get
            {
                return _oCurrentEntryObject as object;
            }
        }

        public bool MoveNext()
        {
            while (_lEntryObjects.Count < _iLastPosition + 1) { }
            _oCurrentEntryObject = _lEntryObjects[_iLastPosition];
            _iLastPosition++;
            return true;
        }

        public void FindChild(string parentId, int currentLevel)
        {
            EntryObject entry = new EntryObject();
            _hEntryManager.UpdateEntryProperties(parentId, ref entry);

            if (entry.Track >= 0 || (entry.Filename != null && entry.Filename.EndsWith(".")))
            {
                if (_sCurrentObjectId != parentId)
                {
                    _sCurrentObjectId = parentId;
                    _iCurrentLevel = currentLevel;
                    _lEntryObjects.Add(entry);
                }
            }

            IEnumPortableDeviceObjectIDs deviceObjectIds;
            _hDeviceContent.EnumObjects(0, parentId, null, out deviceObjectIds);
            uint fetchedCount = 0;

            do
            {
                string objectId;
                deviceObjectIds.Next(1, out objectId, ref fetchedCount);

                if (fetchedCount > 0)
                {
                    FindChild(objectId, currentLevel + 1);
                }
            }
            while (fetchedCount > 0);
        }

        public void Reset()
        {
            _iCurrentLevel = 0;
            _sCurrentObjectId = "DEVICE";
        }
    }
}