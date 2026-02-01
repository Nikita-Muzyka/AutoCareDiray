using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models.DeviceOBD
{
    public class BleDevice
    {
        public string Name { get; set; } = "Unknown";
        public int Id { get; set; }
        public IPeripheral Peripheral { get; set; } = null!;
    }
}
