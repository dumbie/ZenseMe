namespace ZenseMe.Lib.Objects
{
    public class Device
    {
        private string _sId;
        private string _sFriendlyName;
        private string _sModel;
        private string _sManufacturer;
        private string _sSerialNumber;
        private int _iBatteryLevel;

        public string Id
        {
            get { return _sId; }
            set { _sId = value; }
        }

        public string FriendlyName
        {
            get { return _sFriendlyName; }
            set { _sFriendlyName = value; }
        }

        public string Model
        {
            get { return _sModel; }
            set { _sModel = value; }
        }

        public string Manufacturer
        {
            get { return _sManufacturer; }
            set { _sManufacturer = value; }
        }

        public string SerialNumber
        {
            get { return _sSerialNumber; }
            set { _sSerialNumber = value; }
        }

        public int BatteryLevel
        {
            get { return _iBatteryLevel; }
            set { _iBatteryLevel = value; }
        }
    }
}