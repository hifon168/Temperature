using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Collections;
using System.Collections.Specialized;
using OpenHardwareMonitor.Hardware;
using Newtonsoft;
using Newtonsoft.Json;

namespace GPUDetection
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            GPUData gd = new GPUData();
            gd.Update();
        }
        public class GPUData
        {
            public void Update()
            {
                CheckGPU();
                string json = JsonConvert.SerializeObject(GPUlist);
                string gpucom = "getgpu";
                string input = Console.ReadLine().ToLower();
                if (gpucom == input)
                {
                    Console.WriteLine(json);
                }
                else
                {
                    Console.WriteLine("Input error.please check!");
                }

                Console.ReadKey();
            }

            public class GPUStates
            {
                public string type;
                public string speed;
                public string temp;

            }
            ArrayList GPUlist = new ArrayList() { };
            private void GPUinfo(string name, string Type, string tempvalue, string fanvalue)
            {
                GPUStates GP = new GPUStates();
                if (Type == "GpuAti" || Type == "GpuNvidia")
                {
                    GP.type = name;
                    GP.temp = tempvalue + " °C";
                    GP.speed = fanvalue + " RPM";
                }
                GPUlist.Add(GP);
            }

            private OpenHardwareMonitor.Hardware.Computer computerHardware = new OpenHardwareMonitor.Hardware.Computer();

            private void CheckGPU()
            {
                string name = string.Empty;
                string gpuType = string.Empty;
                string sensortype = string.Empty;
                string tempvalue = string.Empty;
                string fanvalue = string.Empty;
                int x, y;
                int hardwareCount;
                int sensorcount;
                computerHardware.FanControllerEnabled = true;
                computerHardware.GPUEnabled = true;
                computerHardware.Open();
                hardwareCount = computerHardware.Hardware.Count();
                for (x = 0; x < hardwareCount; x++)
                  {
                    name = computerHardware.Hardware[x].Name;
                    gpuType = computerHardware.Hardware[x].HardwareType.ToString();//判断是A卡还是N卡条件              
                    sensorcount = computerHardware.Hardware[x].Sensors.Count();
                    if (sensorcount > 0)
                    {
                        for (y = 0; y < sensorcount; y++)
                        {
                            if (computerHardware.Hardware[x].Sensors[y].SensorType.ToString() == "Temperature")
                            {
                                tempvalue = computerHardware.Hardware[x].Sensors[y].Value.ToString();
                            }
                            if (computerHardware.Hardware[x].Sensors[y].SensorType.ToString() == "Fan")
                            {
                                fanvalue = computerHardware.Hardware[x].Sensors[y].Value.ToString();
                            }
                        }

                    }
                    GPUinfo(name, gpuType, tempvalue, fanvalue);
                }
            }
        }

    }

}
