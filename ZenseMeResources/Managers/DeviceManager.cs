using PortableDeviceApiLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZenseMe.Lib.Constants;
using ZenseMe.Lib.Objects;

namespace ZenseMe.Lib.Managers
{
    public class DeviceManager
    {
        private PortableDeviceManager _hPortableDeviceManager;
        private IPortableDeviceValues _hClientDeviceValues;
        private uint _uDeviceCount;
        private string[] _aDeviceIds;

        public DeviceManager()
        {
            _hPortableDeviceManager = new PortableDeviceManager();
            _hClientDeviceValues = ClientValues;
        }

        public static IPortableDeviceValues ClientValues
        {
            get
            {
                IPortableDeviceValues __hClientDeviceValues = new PortableDeviceTypesLib.PortableDeviceValues() as IPortableDeviceValues;
                __hClientDeviceValues.SetStringValue(ref PropertyKeys.WPD_CLIENT_NAME, "ZenseMe");
                return __hClientDeviceValues;
            }
        }

        public PortableDeviceClass GetDevice(string deviceId)
        {
            PortableDeviceClass deviceClass = new PortableDeviceClass();
            deviceClass.Open(deviceId, _hClientDeviceValues);
            return deviceClass;
        }

        public List<Device> GetDevices()
        {
            _hPortableDeviceManager.GetDevices(null, ref _uDeviceCount);

            if (_uDeviceCount > 0)
            {
                _aDeviceIds = new string[_uDeviceCount];
                _hPortableDeviceManager.GetDevices(_aDeviceIds, ref _uDeviceCount);

                List<Device> availableDevices = new List<Device>();

                for (uint i = 0; i < _uDeviceCount; i++)
                {
                    Device device = new Device();
                    device.Id = _aDeviceIds[i];

                    Console.WriteLine("Device[{0}]: {1}", i + 1, _aDeviceIds[i]);

                    UpdateDeviceProperties(ref device);
                    availableDevices.Add(device);
                }
                return availableDevices;
            }
            else
            {
                Console.WriteLine("No device has been found, please connect a device.");
            }
            return null;
        }

        public void UpdateDeviceProperties(ref Device device)
        {
            Hashtable properties = GetDeviceProperties(device.Id, "DEVICE");

            if (properties.Count != 0)
            {
                device.FriendlyName = (string)properties[PropertyNames.DEVICE_FRIENDLY_NAME];
                device.Model = (string)properties[PropertyNames.DEVICE_MODEL];
                device.Manufacturer = (string)properties[PropertyNames.DEVICE_MANUFACTURER];
                device.SerialNumber = (string)properties[PropertyNames.DEVICE_SERIAL_NUMBER];
                device.BatteryLevel = (int)((uint)properties[PropertyNames.DEVICE_BATTERY_LEVEL]);
            }
        }

        public Hashtable GetDeviceProperties(string deviceId, string objectId)
        {
            Hashtable properties = new Hashtable();

            PortableDeviceClass deviceClass = new PortableDeviceClass();
            IPortableDeviceContent deviceContent;

            deviceClass.Open(deviceId, _hClientDeviceValues);
            deviceClass.Content(out deviceContent);

            IPortableDeviceProperties deviceProperties;
            deviceContent.Properties(out deviceProperties);

            IPortableDeviceValues deviceValues;
            deviceProperties.GetValues(objectId, null, out deviceValues);

            uint devicePropertyCount = 0;
            deviceValues.GetCount(ref devicePropertyCount);

            for (uint i = 0; i < devicePropertyCount; i++)
            {
                _tagpropertykey tagPropertyKey = new _tagpropertykey();
                tag_inner_PROPVARIANT tagPropVariant = new tag_inner_PROPVARIANT();

                deviceValues.GetAt(i, ref tagPropertyKey, ref tagPropVariant);

                IntPtr ptrValue = Marshal.AllocHGlobal(Marshal.SizeOf(tagPropVariant));

                Marshal.StructureToPtr(tagPropVariant, ptrValue, false);

                PropVariant pvValue = (PropVariant)Marshal.PtrToStructure(ptrValue, typeof(PropVariant));

                if (pvValue.variantType == VariantTypes.VT_LPWSTR)
                {
                    string stringValue = Marshal.PtrToStringUni(pvValue.pointerValue);
                    properties.Add(PropertyManager.GetKeyName(tagPropertyKey.pid, tagPropertyKey.fmtid), stringValue);
                }
                else if (pvValue.variantType == VariantTypes.VT_DATE)
                {
                    DateTime datetime = DateTime.FromOADate(pvValue.dateValue);
                    properties.Add(PropertyManager.GetKeyName(tagPropertyKey.pid, tagPropertyKey.fmtid), datetime);
                }
                else if (pvValue.variantType == VariantTypes.VT_UI4)
                {
                    uint intValue = pvValue.byteValue;
                    properties.Add(PropertyManager.GetKeyName(tagPropertyKey.pid, tagPropertyKey.fmtid), intValue);
                }
                else if (pvValue.variantType == VariantTypes.VT_UI8)
                {
                    long intValue = pvValue.longValue;
                    properties.Add(PropertyManager.GetKeyName(tagPropertyKey.pid, tagPropertyKey.fmtid), intValue);
                }
                else if (pvValue.variantType == VariantTypes.VT_UINT)
                {
                    long intValue = pvValue.longValue;
                    properties.Add(PropertyManager.GetKeyName(tagPropertyKey.pid, tagPropertyKey.fmtid), intValue);
                }
            }
            return properties;
        }
    }
}