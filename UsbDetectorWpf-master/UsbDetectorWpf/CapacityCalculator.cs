using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace UsbDetectorWpf
{
    class CapacityCalculator
    {
        // Class to crudely get the device capacity by querying adb shell df -h command to see drive partition and sizes
        // Turning the result into an array, then going to the last line, where it has the /data/media attributes , removing white spaces and using substring to get the 
        // first instance of G which indicates the size capacity.
        // Then determine if that is under 8GB, 16GB, 32GB, 64GB, 128GB, 256GB, 512GB
        // I know its crude but its the best way I could think of right now - CarloJS ;)


        //string[] capacity_arr = Regex.Split(getCapacity, "\n");
        //int capacity_arr_length = capacity_arr.Length - 2;
        //string capacityLine = capacity_arr[capacity_arr_length];
        //string cap = string.Join("", capacityLine.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        //cap = cap.Remove(0, 11);
        //int index = cap.IndexOf('G');
        //capacity = cap.Substring(0, index);

        public static string CalculateCapacity(string capacityLine)
        {
            // Splits the result into array with a delimiter of \n (new line)
            string[] capacity_arr = Regex.Split(capacityLine, "\n");
            // Getting the index of the data/media line in array
            int data_media_index = capacity_arr.Length - 2;
            // assinging the /data/media line in a string
            string cap = capacity_arr[data_media_index];
            // Removing White spaces inside
            cap = string.Join("", cap.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            // Removing the /data/media in string
            cap = cap.Remove(0, 11);
            //Getting index of first G(to indicate the GB)
            int index = cap.IndexOf('G');
            // Using substring to get capacity value
            cap = cap.Substring(0, index);
            return cap;

        }

    }
}
