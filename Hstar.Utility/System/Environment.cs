using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Hstar.Utility.System
{
    public static class Environment
    {
        /// <summary>
        /// 获取硬件信息标识
        /// </summary>
        public static HardwareInfo GetHardwareInfo()
        {
            var cpuId = new ManagementClass(WMIPath.Win32_Processor.ToString()).GetInstances()
                .Cast<ManagementObject>().Select(cpu => cpu.Properties["ProcessorId"].Value).First();
            var boardId = new ManagementClass(WMIPath.Win32_BaseBoard.ToString()).GetInstances()
                .Cast<ManagementObject>().Select(disk => disk.Properties["SerialNumber"].Value).First();
            var diskId = new ManagementClass(WMIPath.Win32_DiskDrive.ToString()).GetInstances()
                .Cast<ManagementObject>().Select(disk => disk.Properties["SerialNumber"].Value).First();
            return new HardwareInfo
            {
                CpuId = cpuId.ToString(),
                BoardId = boardId.ToString(),
                DiskId = diskId.ToString()
            };
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        public static ComputerInfo GetSystemInfo()
        {
            var info = new ComputerInfo();
            //CPU
            var objects = new ManagementClass(WMIPath.Win32_Processor.ToString()).GetInstances().Cast<ManagementObject>().ToArray();
            info.CpuName = objects.Select(m => (string)m.Properties["Name"].Value).FirstOrDefault();
            info.CpuId = objects.Select(m => (string)m.Properties["ProcessorId"].Value).FirstOrDefault();

            //主板
            objects = new ManagementClass(WMIPath.Win32_BaseBoard.ToString()).GetInstances().Cast<ManagementObject>().ToArray();
            info.BoardName = objects.Select(m => (string)m.Properties["Manufacturer"].Value + " " +
                                                 (string)m.Properties["Product"].Value + " " +
                                                 (string)m.Properties["Version"].Value).FirstOrDefault();
            info.BoardId = objects.Select(m => (string)m.Properties["SerialNumber"].Value).FirstOrDefault();

            //硬盘
            objects = new ManagementClass(WMIPath.Win32_DiskDrive.ToString()).GetInstances().Cast<ManagementObject>().ToArray();
            info.DiskName = objects.Select(m => (string)m.Properties["Model"].Value + " " +
                                                (Convert.ToDouble(m.Properties["Size"].Value) / (1024 * 1024 * 1024)) + " GB").FirstOrDefault();
            info.DiskId = objects.Select(m => (string)m.Properties["SerialNumber"].Value).FirstOrDefault();

            //操作系统
            objects = new ManagementClass(WMIPath.Win32_OperatingSystem.ToString()).GetInstances().Cast<ManagementObject>().ToArray();
            info.OSName = objects.Select(m => (string)m.Properties["Caption"].Value).FirstOrDefault();
            info.OSCsdVersion = objects.Select(m => (string)m.Properties["CSDVersion"].Value).FirstOrDefault();
            info.OSIs64Bit = objects.Select(m => (string)m.Properties["OSArchitecture"].Value == "64-bit").FirstOrDefault();
            info.OSVersion = objects.Select(m => (string)m.Properties["Version"].Value).FirstOrDefault();
            info.OSPath = objects.Select(m => (string)m.Properties["WindowsDirectory"].Value).FirstOrDefault();

            //内存
            info.PhysicalMemoryFree = objects.Select(m => Convert.ToDouble(m.Properties["FreePhysicalMemory"].Value) / (1024)).FirstOrDefault();
            info.PhysicalMemoryTotal = objects.Select(m => Convert.ToDouble(m.Properties["TotalVisibleMemorySize"].Value) / (1024)).FirstOrDefault();

            //屏幕
            objects = new ManagementClass(WMIPath.Win32_VideoController.ToString()).GetInstances().Cast<ManagementObject>().ToArray();
            info.ScreenWith = objects.Select(m => Convert.ToInt32(m.Properties["CurrentHorizontalResolution"].Value)).FirstOrDefault();
            info.ScreenHeight = objects.Select(m => Convert.ToInt32(m.Properties["CurrentVerticalResolution"].Value)).FirstOrDefault();
            info.ScreenColorDepth = objects.Select(m => Convert.ToInt32(m.Properties["CurrentBitsPerPixel"].Value)).FirstOrDefault();
            return info;
        }

        /// <summary>
        /// 获取当前系统运行的进程列表
        /// </summary>
        public static IEnumerable<string> GetProcessNames()
        {
            var objects = new ManagementClass(WMIPath.Win32_Process.ToString()).GetInstances().Cast<ManagementObject>().ToArray();
            return objects.Select(m => (string)m.Properties["Caption"].Value).OrderBy(m => m);
        }

        /// <summary>
        /// 获取当前系统正在运行的服务列表
        /// </summary>
        public static IEnumerable<string> GetStartedServiceNamesEnumerable()
        {
            var objects = new ManagementClass(WMIPath.Win32_Service.ToString()).GetInstances().Cast<ManagementObject>().ToArray();
            return objects.Where(m => (bool)m.Properties["Started"].Value)
                .Select(m => (string)m.Properties["Caption"].Value).OrderBy(m => m);
        }

        /// <summary>
        /// 获取剩余空间最大的逻辑磁盘名称
        /// </summary>
        public static string GetMaxFreeSizeLogicalDisk()
        {
            var objects = new ManagementClass(WMIPath.Win32_LogicalDisk.ToString()).GetInstances().Cast<ManagementObject>().ToArray();
            return objects.OrderByDescending(m => Convert.ToInt64(m.Properties["FreeSpace"].Value))
                .Select(m => (string)m.Properties["Caption"].Value)
                .FirstOrDefault();
        }
    }
}
