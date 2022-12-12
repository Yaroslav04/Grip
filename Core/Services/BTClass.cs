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
        static int period = 15;
        List<SensorClass> list;
        DateTime span;
        BluetoothSocket _socket;
        byte[] buffer = new byte[256];

        public BTClass()
        {
            span = DateTime.Now;
            list = new List<SensorClass>();
        }

        public async void RunAsync()
        {

            while (true)
            {
                if (_socket == null)
                {
                    try
                    {
                        await ConnectBT();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine($"is null ex {e.Message}");
                        await Task.Delay(10000);
                        continue;
                    }
                }

                if (!_socket.IsConnected)
                {
                    try
                    {
                        await ConnectBT();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine($"is connect ex {e.Message}");
                        await Task.Delay(10000);
                        continue;
                    }
                }

                if (_socket.IsConnected)
                {
                    if (_socket.InputStream != null)
                    {
                        await _socket.InputStream.ReadAsync(buffer, 0, buffer.Length);

                        string s = Encoding.UTF8.GetString(buffer);
                        var jsonString = "";

                        jsonString = s.Replace("\r", "").Replace("\n", "").Replace("\0", "");
                        if (jsonString != "")
                        {
                            System.Diagnostics.Debug.WriteLine($"json {jsonString}");
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
                                            list.Add(new SensorClass
                                            {
                                                Sensor = item.name,
                                                Value = Convert.ToInt32(item.value),
                                                SaveDate = dt,
                                            });
                                            System.Diagnostics.Debug.WriteLine($"add SensorClass {item.name} {item.value}");
                                        }
                                        catch (Exception e )
                                        {
                                            System.Diagnostics.Debug.WriteLine($"add SensorClass ex {e.Message}");
                                        }
                                    }

                                    
                                }
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine($"json converter ex {e.Message}/n {jsonString}");
                            }

                            buffer = new byte[256];
                            await _socket.InputStream.FlushAsync();
                        }                                            
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"soket is null");
                    }
                }


                if ((DateTime.Now - span).Minutes >= period)
                {
                    DateTime saveDate = DateTime.Now;                   
                    if (list.Count > 0)
                    {
                        foreach (var sensor in App.SensorTypes)
                        {
                            try
                            {
                                var sens = GetMediane(sensor, saveDate);
                                if (sens != null)
                                {
                                    System.Diagnostics.Debug.WriteLine($"save data base {sens.Sensor} {sens.Value}");
                                    await App.DataBase.SensorDB.SaveAsync(sens);
                                }
                            }
                            catch
                            {
                            }                       
                        }

                        list.Clear();

                    }
                    span = DateTime.Now;
                }

                buffer = new byte[256];
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

        private SensorClass GetMediane(string _sensor, DateTime _saveDate)
        {
            var sublist = list.Where(x => x.Sensor == _sensor).ToList();

            if (sublist.Count > 0)
            {
                if (_sensor == App.SensorTypes[2])
                {
                    sublist = sublist.Where(x => x.Value > 400 & x.Value < 2000).ToList();
                }

                if (sublist.Count == 0)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }


            if (sublist.Count > 0)
            {
                var sum = 0;
                foreach (var item in sublist)
                {
                    sum = sum + item.Value;
                }

                sum = Convert.ToInt32(sum / sublist.Count);

                SensorClass sensor = new SensorClass()
                {
                    Sensor = _sensor,
                    Value = sum,
                    SaveDate = _saveDate,
                };

                return sensor;
            }
            else
            {
                return null;
            }
        }
    }

    public class JsonRoot
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
