using UsbDetectorWpf.Enum;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Diagnostics;




namespace UsbDetectorWpf
{
    /// <summary>
    /// An Application extending UsbDetectorWpf From https://github.com/dtwk1/UsbDetectorWpf, and using libimobiledevice and adb modules 
    /// to get device info and saving it into a php backend by Carlo JS Nayve
    /// </summary>
    public partial class MainWindow : Window
    {
        private IntPtr windowHandle;
        private string postURL = "https://portal-ca.sbe-ltd.ca/screening_new/device_info/actions.php";
        private string imei;
        private string serialNumber;
        private string oem;
        private string model;
        private string workStation = System.Environment.MachineName;
        private string softwareVersion;
        private string color;
        private string capacity;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(System.EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Adds the windows message processing hook and registers USB device add/removal notification.
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            if (source != null)
            {
                windowHandle = source.Handle;
                source.AddHook(HwndHandler);
               UsbNotification.RegisterUsbDeviceNotification(windowHandle);
            }
        }

        public event EventHandler<DeviceChange> DeviceChanged;
       
        /// <summary>
        /// Method that receives window messages.
        /// https://stackoverflow.com/questions/16245706/check-for-device-change-add-remove-events
        /// </summary>
        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == UsbNotification.WmDevicechange)
            {
                switch ((int)wparam)
                {
                    case UsbNotification.DbtDeviceremovecomplete:
                        // Outputs imei into imei textbox
                        imeiTextBox.Text = "";
                        serialNumberTextBox.Text = "";
                        oemTextBox.Text = "";
                        modelTextBox.Text = "";
                        colorTextBox.Text = "";
                        capacityTextBox.Text = "";
                        softwareVersionTextBox.Text = "";
                        //this.DeviceChanged?.Invoke(this, DeviceChange.Remove);
                        //OnThresholdReached();
                        break;
                    case UsbNotification.DbtDevicearrival:
                        string appleResult = ExecuteCommandSync("wmic path Win32_PnPEntity where \" Description like 'Apple%' \" get /value");
                        int appleResultLength = appleResult.Length;
                        if (appleResultLength <= 9)
                        {
                            // If result count for apple query is less than 9 then it is not apple device
                            string androidResult = ExecuteCommandSync("wmic path Win32_PnPEntity where \" Description like 'ADB%' \" get /value");                            
                            if (androidResult.Length > 9)
                            {
                                // query to parse service call to get imei
                                string imei_query = "\"service call iphonesubinfo 1 | grep -o '[0-9a-f]\\{8\\} ' | tail -n+3 | while read a; do echo -n \\\\u${a:4:4}\\\\u${a:0:4}; done\"";
                                oem = ExecuteAndroidDeviceInfo("getprop ro.product.vendor.manufacturer");
                                imei = ExecuteAndroidDeviceInfo(imei_query);
                                imei = imei.Trim();                                
                                serialNumber = ExecuteAndroidDeviceInfo("getprop ro.serialno");


                                if (oem.Trim() == "HUAWEI")
                                {
                                    model = ExecuteAndroidDeviceInfo("getprop ro.product.model");
                                }
                                else 
                                {
                                    model = ExecuteAndroidDeviceInfo("getprop ro.product.vendor.model");
                                }

                                if (oem.Trim() == "Google")
                                {
                                    color = ExecuteAndroidDeviceInfo("getprop ro.boot.hardware.color");
                                    capacity = ExecuteAndroidDeviceInfo("getprop ro.boot.hardware.ufs");
                                }
                                else 
                                {
                                    string getCapacity  = ExecuteAndroidDeviceInfo("df -h");
                                    capacity = CapacityCalculator.CalculateCapacity(getCapacity);

                                    //MessageBox.Show(capacity_arr[capacity_arr_length]);
                                }
                                
                                
                                //MessageBox.Show(start);

                                if (oem.Trim() == "samsung")
                                {
                                    softwareVersion = ExecuteAndroidDeviceInfo("getprop ro.bootloader");
                                    //MessageBox.Show("samsung device");
                                }
                                else if(oem.Trim() == "HUAWEI")
                                {
                                    softwareVersion = ExecuteAndroidDeviceInfo("getprop ro.build.display.id");
                                }
                                else
                                {
                                    softwareVersion = ExecuteAndroidDeviceInfo("getprop ro.build.id");
                                    
                                }
                                //output to wpf
                                imeiTextBox.Text = imei;
                                softwareVersionTextBox.Text = softwareVersion;
                                modelTextBox.Text = model;
                                oemTextBox.Text = oem;
                                serialNumberTextBox.Text = serialNumber;
                                colorTextBox.Text = color;
                                capacityTextBox.Text = capacity;
                            }
                            else 
                            {
                                MessageBox.Show("unknown device");
                            }   
                        }
                        else 
                        {
                            // It means an apple device is connected
                            imei = ExecuteAppleDeviceInfo("ideviceinfo -k  InternationalMobileEquipmentIdentity");                            
                            // Outputs imei into imei textbox
                            imeiTextBox.Text = imei;
                            serialNumber = ExecuteAppleDeviceInfo("ideviceinfo -k SerialNumber");
                            serialNumberTextBox.Text = serialNumber;
                            //MessageBox.Show(appleDeviceInfo);
                            oem = "Apple";
                            oemTextBox.Text = oem;
                            model = ExecuteAppleDeviceInfo("ideviceinfo -k ModelNumber");
                            modelTextBox.Text = model; 
                            softwareVersion = ExecuteAppleDeviceInfo("ideviceinfo -k ProductVersion");
                            softwareVersionTextBox.Text = softwareVersion;
                            //string[] capacityArray = capacityResult.Split();
                            //MessageBox.Show(capacityResult);

                            if (string.IsNullOrEmpty(imei) || string.IsNullOrEmpty(serialNumber) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(softwareVersion))
                            {
                                imei = ExecuteAppleDeviceInfo("ideviceinfo -k  InternationalMobileEquipmentIdentity");
                                imeiTextBox.Text = imei;
                                serialNumber = ExecuteAppleDeviceInfo("ideviceinfo -k SerialNumber");
                                serialNumberTextBox.Text = serialNumber;
                                model = ExecuteAppleDeviceInfo("ideviceinfo -k ModelNumber");
                                modelTextBox.Text = model;
                                softwareVersion = ExecuteAppleDeviceInfo("ideviceinfo -k ProductVersion");
                                softwareVersionTextBox.Text = softwareVersion;

                            }
                            else
                            {
                                string data = callToPHP.GetPost(postURL, "save_device_info", "1", "imei", imei,
                                "serial_number", serialNumber, "OEM", oem, "model", model, "software_version", softwareVersion, "workStation", workStation);
                                MessageBox.Show(data);
                            }

                            
                            



                        }
                        //MessageBox.Show(appleResultLength.ToString());
                        break;
                }
            }

            handled = false;
            return IntPtr.Zero;
        }

      

        public string ExecuteCommandSync(object command)
        {
            // create the ProcessStartInfo using "cmd" as the program to be run,
            // and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows,
            // and then exit.
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            // Do not create the black window.
            procStartInfo.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            // Get the output into a string
            string result = proc.StandardOutput.ReadToEnd();
            // Display the command output.
            //Console.WriteLine(result);
            return result;
        }

        public string ExecuteAppleDeviceInfo(object command)
        {
            // create the ProcessStartInfo using "cmd" as the program to be run,
            // and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows,
            // and then exit.
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procStartInfo.WorkingDirectory = @"C:\apple_cycle\Resources\deviceinfo";
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            // Do not create the black window.
            procStartInfo.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            // Get the output into a string
            string result = proc.StandardOutput.ReadToEnd();
            // Display the command output.
            //Console.WriteLine(result);
            return result;
        }


        public string ExecuteAndroidDeviceInfo(object command )
        {
            // create the ProcessStartInfo using "cmd" as the program to be run,
            // and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows,
            // and then exit.
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c adb shell " + command);

            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procStartInfo.WorkingDirectory = @"C:\platform-tools\";
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            // Do not create the black window.
            procStartInfo.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            // Get the output into a string
            string result = proc.StandardOutput.ReadToEnd();
            // Display the command output.
            //Console.WriteLine(result);
            return result;
        }

        //public static readonly RoutedEvent AddClickEvent = EventManager.RegisterRoutedEvent("AddClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(object));

        //public event RoutedEventHandler AddClick
        //{
        //    add { AddHandler(AddClickEvent, value); }
        //    remove { RemoveHandler(AddClickEvent, value); }
        //}

        //void RaiseAddClickEvent()
        //{
        //    RoutedEventArgs newEventArgs = new RoutedEventArgs(null);
        //}

        //protected void OnAddClick()
        //{
        //    RaiseAddClickEvent();
        //}



    }
}
