using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbDetectorWpf.Enum;

namespace UsbDetectorWpf.EventArgs
{

    public class DeviceChangeEventArgs : System.EventArgs
    {
        public DeviceChange DeviceChange { get; set; }

    }
}
