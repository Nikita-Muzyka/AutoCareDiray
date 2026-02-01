using AutoCareDiray.Models.DeviceOBD;
using AutoCareDiray.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shiny;
using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels.Diagnostics
{
    public partial class DiagnosticsViewModel : BaseViewModel
    {
        private readonly IBleManager _ble;
        [ObservableProperty]
        private ObservableCollection<BleDevice> device = new();



        public DiagnosticsViewModel(IApiService apiService,IDialogService dialog, IBleManager ble) : base(apiService,dialog) 
        {    
            _ble = ble;

        }

        [RelayCommand]
        public async Task ScanBLE()
        {
            var access = await _ble.RequestAccessAsync();
            if(access != AccessState.Available)
            {
            
            }
            else
            {
                var scanner = _ble.Scan().Subscribe(scanResult =>
                {
                    if(scanResult.Rssi > -80)
                    {
                        Device.Add(new BleDevice
                        {
                            Id = scanResult.Rssi,
                            Peripheral = scanResult.Peripheral,
                            Name = scanResult.Peripheral.Name ?? scanResult.AdvertisementData.LocalName ?? "Unknown"
                        }
                   );
                    }
                });

                
                await Task.Delay(3000);
                scanner.Dispose();
            }
        }
    }
}
