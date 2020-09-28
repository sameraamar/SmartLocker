using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;

namespace MySmartLockWatchDog
{
    using System.IO;

    public partial class SmartLockWatchDogService : ServiceBase
    {
        private readonly bool debug;
        int count;
        System.Timers.Timer timeDelay;

        public SmartLockWatchDogService(bool debug = false)
        {
            this.debug = debug;
            InitializeComponent();

            timeDelay = new System.Timers.Timer();
            timeDelay.Elapsed += WorkProcess;
            this.timeDelay.Interval = 1000; 
        }

        public void WorkProcess(object sender, EventArgs e)
        {
            if (this.timeDelay.Interval.Equals(1000))
            {
                this.timeDelay.Interval = 1000 * 60 * 5; // 5 minutes
            }


            count++;
            //string msg = "Timer Tick(1): " + count;
            //LogService(msg);

            if (count == 0 || (count % 5 == 0))
            {
                LogService($"starting process: \n\t({System.AppDomain.CurrentDomain.BaseDirectory})\n\t{System.Reflection.Assembly.GetEntryAssembly().Location}" );

                string lockerUi = $@"{System.AppDomain.CurrentDomain.BaseDirectory}\MySmartLockUI.exe";

                if (!File.Exists(lockerUi))
                {
                    LogService("Error: " + lockerUi + " is missing!");
                }
                else
                {
                    LogService("Running: " + lockerUi);
                }

                var processApps = new Process();
                //processApps.StartInfo = new ProcessStartInfo { FileName = @"C:\WINDOWS\system32\notepad.exe", Arguments = "-arg1 -arg2" };
                processApps.StartInfo = new ProcessStartInfo { FileName = lockerUi, Arguments = "" };
                processApps.StartAsActiveUser();
                LogService("started process.");
            }

            /*
            if (count == 10)
            {
                LogService("Locking... ");

                if (!ProcessExtensionsV1.LockWorkStation())
                {
                    var winEx = new Win32Exception(Marshal.GetLastWin32Error());
                    LogService(winEx.ToString());
                }
                else
                {
                    LogService("Locked!");
                }
            }

            msg = "Timer Tick (3): " + count;
            LogService(msg);
            */
        }

        protected override void OnStart(string[] args)
        {
            LogService("Service is Started");
            WorkProcess(null, null);
            timeDelay.Enabled = true;
        }

        protected override void OnStop()
        {
            LogService("Service Stoped");
            timeDelay.Enabled = false;
        }

        private void LogService(string content)
        {
            if (debug)
            {
                FileStream fs = new FileStream(@"c:\log\MySmartLock.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
            }
        }
        private string ReadPassCode()
        {
            string content = "";
            try
            {
                FileStream fs = new FileStream(@"c:\log\PassCode.txt", FileMode.Open, FileAccess.ReadWrite);
                StreamReader sw = new StreamReader(fs);

                content = sw.ReadToEnd();
                
                sw.Close();
            }
            catch (Exception e)
            {
                LogService(e.ToString());
            }

            return content;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LogService("Wake up...");
        }
    }
}
