using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace Flickr_Downloader {
    public partial class update_msgbox : Form {

        public bool download_ok = false;
        public bool live_X = false;
        public string target_url = "";
        public bool form_must_close = false;
        public byte[] bytes = { };
        public string filename = "update_.exe";
        System.Net.WebClient wbcl_ = new System.Net.WebClient();
        public string timestamp_ = "";

        public update_msgbox() {
            InitializeComponent();
        }

        private void update_msgbox_Load(object sender, EventArgs e) {
            live_X = false;
            download_ok = false;
            timestamp_ = HttpUtility.UrlEncode(timestamp_);
            wbcl_.DownloadDataCompleted += DownloadDataCallback;
            wbcl_.DownloadProgressChanged += DownloadProgressChanged;
            try {
                wbcl_.DownloadDataAsync(new Uri(target_url));
                while (true) {
                    System.Threading.Thread.Sleep(50);
                    Application.DoEvents();
                    if (download_ok == true) {
                        break;
                    }
                    if (form_must_close == true) {
                        wbcl_.CancelAsync();
                        wbcl_.Dispose();
                        break;
                    }
                }
                if (live_X == false) {
                    label1.Text = "狀態：錯誤！下載失敗 Error！Download Fail.";
                    return;
                }
                try {
                    filename = "update_" + (new Random().Next(0, 9999)) + ".7z";
                    System.IO.File.WriteAllBytes(Application.StartupPath + "\\" + filename, bytes);
                    this.Tag = "1";
                    this.Close();
                } catch {
                    label1.Text = "狀態：錯誤！寫入失敗 Error! Writing Fail.";
                }
            } catch {
                label1.Text = "狀態：錯誤！下載失敗 Error！Download Fail.";
            }
        }

        private void DownloadDataCallback(Object sender, System.Net.DownloadDataCompletedEventArgs e) {
            try {

                if (e.Cancelled == false && e.Error == null) {
                    bytes = e.Result;
                    live_X = true;
                } else {
                    bytes = new byte[] { };
                    live_X = false;
                }
            } finally {
            }
            download_ok = true;
        }

        private void DownloadProgressChanged(Object sender, System.Net.DownloadProgressChangedEventArgs e) {
            //WebClient wbcl_ = (WebClient) sender;
            long download_byte_count = e.BytesReceived;
            long download_byte_count_total = e.TotalBytesToReceive;
            if (download_byte_count_total > 0) {
                try {
                    progressBar1.Maximum = (int)download_byte_count_total;
                    if (download_byte_count <= progressBar1.Maximum) {
                        progressBar1.Value = (int)download_byte_count;
                    }
                } catch {

                }
            }
            try {
                label1.Text = "狀態：（" + Math.Round((decimal)download_byte_count / 1024, 1) + " KBytes / " + Math.Round((decimal)download_byte_count_total / 1024, 1) + " KBytes）- " + e.ProgressPercentage + "%";
            } catch (Exception) {

            }
        }

        private void update_msgbox_FormClosing(object sender, FormClosingEventArgs e) {
            form_must_close = true;
        }

        private void button1_Click(object sender, EventArgs e) {
            wbcl_.CancelAsync();
            wbcl_.Dispose();
            download_ok = true;
            live_X = false;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            timer1.Stop();
            update_msgbox_Load(new object(),new EventArgs());
        }
    }
}
