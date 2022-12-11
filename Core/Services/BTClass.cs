using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.OS;
using Android.Runtime;
using Java.Util;

namespace Grip.Core.Services
{
    public class BTClass
    {
        BluetoothSocket _socket;
        byte[] buffer = new byte[256];

        public async void RunAsync()
        {
            bool sw = true;

            while (sw)
            {
                try
                {
                    await ConnectBT();
                    sw = false;
                }
                catch
                {
                    await Task.Delay(10000);
                }
            }

            while (true)
            {
                try
                {
                    if (!_socket.IsConnected)
                    {
                        await ConnectBT();
                    }
                  
                    while (_socket.IsConnected)
                    {
                        int i = await _socket.InputStream.ReadAsync(buffer, 0, buffer.Length);
                        string s = Encoding.UTF8.GetString(buffer);
                        var jsonString = s.Replace("\r", "").Replace("\n", "").Replace("\0", "");
                        try
                        {
                            List<JsonRoot> items = JsonConvert.DeserializeObject<List<JsonRoot>>(jsonString);
                            if (items.Count == 6)
                            {
                                DateTime dt = DateTime.Now;
                                foreach (var item in items)
                                {
                                    try
                                    {
                                        await App.DataBase.SensorDB.SaveAsync(new SensorClass
                                        {
                                            Sensor = item.name,
                                            Value = item.value,
                                            SaveDate = dt,
                                            DateToShow = dt.ToString()
                                        });
                                    }
                                    catch
                                    {

                                    }
                                }
                            }                           
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                        finally
                        {
                            buffer = new byte[256];
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    await Task.Delay(10000);
                }
                finally
                {
                    buffer = new byte[256];
                }
            }
        }

        private async Task ConnectBT()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!adapter.IsEnabled)
                throw new Exception("Bluetooth adapter is not enabled.");

            BluetoothDevice device = (from bd in adapter.BondedDevices
                                      where bd.Name == "ESP32test"
                                      select bd).FirstOrDefault();

            if (device == null)
                throw new Exception("Named device not found.");

            _socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
            await _socket.ConnectAsync();
        }
    }

    public class JsonRoot
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
