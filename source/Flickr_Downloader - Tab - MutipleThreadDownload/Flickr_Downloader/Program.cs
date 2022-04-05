using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Flickr_Downloader {
    static class Program {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main () {
            //Application.EnableVisualStyles();

            bool update_msg_show = false;

            Control.CheckForIllegalCrossThreadCalls = false;

            if (Environment.GetCommandLineArgs().Length > 1 && Environment.GetCommandLineArgs()[1].ToString() == "new") {
                update_msg_show = true;
                try {
                    System.IO.File.Delete(Application.StartupPath + "/Microsoft.WindowsAPICodePack.dll");
                } catch {

                }
                try {
                    System.IO.File.Delete(Application.StartupPath + "/Microsoft.WindowsAPICodePack.Shell.dll");
                } catch {

                }
                try {
                    System.IO.File.Delete(Application.StartupPath + "/Newtonsoft.Json.dll");
                } catch {

                }
                try {
                    System.IO.File.Delete(Application.StartupPath + "/FolderBrowserDialogEx.dll");
                } catch {

                }
                try {
                    System.IO.File.Delete(Application.StartupPath + "/SevenZipSharp.dll");
                } catch {

                }
            }

            if ( !System.IO.File.Exists(Application.StartupPath + "/Microsoft.WindowsAPICodePack.dll") ) {
                try {
                    System.IO.File.WriteAllBytes(Application.StartupPath + "/Microsoft.WindowsAPICodePack.dll", Flickr_Downloader.Properties.Resources.Microsoft_WindowsAPICodePack);
                } catch ( Exception ) {

                }
            }
            if ( !System.IO.File.Exists(Application.StartupPath + "/Microsoft.WindowsAPICodePack.Shell.dll") ) {
                try {
                    System.IO.File.WriteAllBytes(Application.StartupPath + "/Microsoft.WindowsAPICodePack.Shell.dll", Flickr_Downloader.Properties.Resources.Microsoft_WindowsAPICodePack_Shell);
                } catch ( Exception ) {

                }
            }
            if ( !System.IO.File.Exists(Application.StartupPath + "/Newtonsoft.Json.dll") ) {
                try {
                    System.IO.File.WriteAllBytes(Application.StartupPath + "/Newtonsoft.Json.dll", Flickr_Downloader.Properties.Resources.Newtonsoft_Json);
                } catch ( Exception ) {

                }
            }
            if (!System.IO.File.Exists(Application.StartupPath + "/FolderBrowserDialogEx.dll")) {
                try {
                    System.IO.File.WriteAllBytes(Application.StartupPath + "/FolderBrowserDialogEx.dll", Flickr_Downloader.Properties.Resources.FolderBrowserDialogEx);
                } catch (Exception) {

                }
            }
            if (!System.IO.File.Exists(Application.StartupPath + "/SevenZipSharp.dll")) {
                try {
                    System.IO.File.WriteAllBytes(Application.StartupPath + "/SevenZipSharp.dll", Flickr_Downloader.Properties.Resources.SevenZipSharp);
                } catch (Exception) {

                }
            }
            if (!System.IO.File.Exists(Application.StartupPath + "/7z.dll")) {
                try {
                    if (Environment.Is64BitProcess) {
                        Debug.WriteLine("x64");
                        System.IO.File.WriteAllBytes(Application.StartupPath + "/7z.dll", Flickr_Downloader.Properties.Resources._7z);
                    } else {
                        Debug.WriteLine("x86");
                        System.IO.File.WriteAllBytes(Application.StartupPath + "/7z.dll", Flickr_Downloader.Properties.Resources._7zx86);
                    }
                } catch (Exception) {

                }
            }
            if (Environment.GetCommandLineArgs().Length > 1) {
                if (Environment.GetCommandLineArgs()[1].ToString() == "update") {
                    int times = 0;
                    while (true) {
                        try {
                            System.Threading.Thread.Sleep(10);
                            try {
                                System.IO.File.Copy(Application.ExecutablePath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe", true);
                            } catch {
                                bool copy_fail = true;
                                int times_dead = 0;
                                while (copy_fail) {
                                    System.Threading.Thread.Sleep(10);
                                    try {
                                        System.IO.File.Copy(Application.ExecutablePath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe", true);
                                        copy_fail = false;
                                    } catch {

                                    }
                                    times_dead += 1;
                                    if (times_dead>500) {
                                        break;
                                    }
                                }
                            }
                            Process.Start(Application.StartupPath + "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe", "new \"" + Application.ExecutablePath + "\"");
                            Process.GetCurrentProcess().Kill();
                            break;
                        } catch {
                        }
                        times += 1;
                        if (times > 1000) {
                            Process.GetCurrentProcess().Kill();
                        }
                    }
                } else if (Environment.GetCommandLineArgs()[1].ToString() == "new" && Environment.GetCommandLineArgs().Length == 3) {
                    try {
                        System.IO.File.Delete(Environment.GetCommandLineArgs()[2].ToString());
                    } catch {
                        bool delete_fail = true;
                        int times_dead = 0;
                        while (delete_fail) {
                            System.Threading.Thread.Sleep(10);
                            try {
                                System.IO.File.Delete(Environment.GetCommandLineArgs()[2].ToString());
                                delete_fail = false;
                            } catch {

                            }
                            times_dead += 1;
                            if (times_dead > 500) {
                                break;
                            }
                        }
                    }
                }
            }
            System.Net.ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            System.Net.ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.CheckCertificateRevocationList = false;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate {
                return true;
            };
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1() { update_msg_show_ = update_msg_show });
        }
    }

}
