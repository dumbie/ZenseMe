using System.Collections;
using System.Collections.Generic;
using ZenseMe.Lib.Objects;

namespace ZenseMe.Lib.Collections
{
    public class TrackEntries : IEnumerable<EntryObject>, IEnumerable
    {
        private Device _hDevice;

        public TrackEntries(Device device)
        {
            _hDevice = device;
        }

        public IEnumerator<EntryObject> GetEnumerator()
        {
            return new EntryObjectEnumerator(_hDevice);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EntryObjectEnumerator(_hDevice);
        }
    }
}