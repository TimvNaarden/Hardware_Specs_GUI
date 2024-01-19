using Hardware_Specs_GUI.Json;
using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;

namespace Hardware_Specs_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PCinfo info = getSystemInfo();
            label1.Text = info.ToJson().FormatJson();
        }



        private static PCinfo getSystemInfo()
        {
            // Create an object that stores all the info 
            PCinfo info = new PCinfo();

            // Look all the required CPU info up
            ManagementObjectSearcher CPUSearcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            ManagementObjectCollection CPU = CPUSearcher.Get();

            //Store all the found cpu info
            foreach (ManagementObject CPUObject in CPU)
            {
                info.Systemname = CPUObject["systemname"].ToString();
                info.CPUName = CPUObject["name"].ToString();
                info.ThreadCount = CPUObject["ThreadCount"].ToString();
                info.BaseClockSpeed = CPUObject["maxclockspeed"].ToString();
                info.Cores = CPUObject["numberofcores"].ToString();
            }

            // Now we do the same for the memory information
            ManagementObjectSearcher MemorySearcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            ManagementObjectCollection Memory = MemorySearcher.Get();

            foreach (ManagementObject MemoryObject in Memory)
            {
                info.MemoryCapacity += (Convert.ToDouble(MemoryObject["capacity"]) / 1048576);
                info.MemoryType = MemoryObject["memorytype"].ToString();
                info.MemorySpeed = Convert.ToDouble(MemoryObject["Speed"]);
                info.MemoryDimms += 1;
            }

            // Video informaton
            ManagementObjectSearcher VideoSearcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            ManagementObjectCollection Video = VideoSearcher.Get();

            foreach (ManagementObject VideoObject in Video)
            {
                info.VideoName.Add(VideoObject["name"].ToString());
                info.Vram.Add(Convert.ToDouble(VideoObject["AdapterRam"]) / 1048576);

            }
            // Network Information
            ManagementObjectSearcher NetworkSearcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection Network = NetworkSearcher.Get();

            foreach (ManagementObject NetworkObject in Network)
            {
                if (NetworkObject["MACAddress"] != null)
                {
                    info.MACAddres.Add(NetworkObject["macaddress"]?.ToString());
                }


                if (NetworkObject["ipaddress"] != null)
                {
                    info.NetworkAddresses.Add((string[])NetworkObject["ipaddress"]);
                }
            }
            ManagementObjectSearcher StorageSearcher =
                new ManagementObjectSearcher("SELECT * From Win32_DiskDrive");
            ManagementObjectCollection Storage = StorageSearcher.Get();

            //Storage Information
            foreach (ManagementObject StorageObject in Storage)
            {
                info.StorageNames.Add(StorageObject["model"].ToString());
            }
            return info;

        }


        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(183, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(572, 697);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Hardware Specs Gui";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
    class PCinfo
    {

        public string CPUName { get; set; }
        public string Systemname { get; set; }
        public string ThreadCount { get; set; }
        public string BaseClockSpeed { get; set; }
        public double MemoryCapacity { get; set; }
        public double MemorySpeed { get; set; }
        public string MemoryType { get; set; }
        public int MemoryDimms { get; set; }
        public string Cores { get; set; }
        public List<string> MACAddres { get; set; }
        public List<Array> NetworkAddresses { get; set; }
        public List<string> StorageNames { get; set; }
        public List<string> VideoName { get; set; }
        public List<double> Vram { get; set; }


        public PCinfo()
        {
            VideoName = new List<string>();
            Vram = new List<double>();
            NetworkAddresses = new List<Array>();
            StorageNames = new List<string>();
            MACAddres = new List<string>();
        }

    }
}




