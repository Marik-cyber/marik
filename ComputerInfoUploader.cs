using Microsoft.Win32;
using System;
using System.Management;

namespace ComputerInfoUploader.Services
{
    public class ComputerInfoService
    {
        public string GetComputerDetails()
        {
            try
            {
                var computerDetails = "";

                // System Name
                var systemName = Environment.MachineName;
                computerDetails += $"System Name: {systemName}\n";

                // OS Version
                var osVersion = Environment.OSVersion.ToString();
                computerDetails += $"OS Version: {osVersion}\n";

                // Processor Info
                var processorcount = Environment.ProcessorCount;
                computerDetails += $"Processorcount: {processorcount}\n";
                var processorid = Environment.ProcessId;
                computerDetails += $"processorid: {processorid}\n";

                //RAM Info
                //var ramInfo = GetManagementObject("Win32_ComputerSystem", "TotalPhysicalMemory");
                //if (long.TryParse(ramInfo, out long ramBytes))
                //{
                //    computerDetails += $"RAM: {ramBytes / (1024 * 1024)} MB\n";
                //}
                //else
                //{
                //    computerDetails += "RAM: N/A\n";
                //}

                return computerDetails;
            }
            catch (Exception ex)
            {
                return $"Error fetching computer details: {ex.Message}";
            }
        }

        //private string GetManagementObject(string className, string propertyName)
        //{
        //    try
        //    {
        //        using (var searcher = new ManagementObjectSearcher($"SELECT {propertyName} FROM {className}"))
        //        {
        //            foreach (var obj in searcher.Get())
        //            {
        //                var value = obj[propertyName]?.ToString();
        //                if (!string.IsNullOrEmpty(value))
        //                {
        //                    return value;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error retrieving {propertyName} from {className}: {ex.Message}");
        //    }
        //    return "N/A";
        //}

    }
}
