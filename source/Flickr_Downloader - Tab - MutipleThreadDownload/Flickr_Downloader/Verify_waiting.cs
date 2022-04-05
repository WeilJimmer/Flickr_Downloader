using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Flickr_Downloader {
    public partial class Verify_waiting : Form {

        public Thread listen_service;
        HttpListener web = new HttpListener();

        public bool lock_form = true;
        public bool cancel_auth = false;
        public string url_XX = "";
        public short timer_count = 900;

        public string oauth_verifier = "";

        public Verify_waiting() {
            InitializeComponent();
        }

        public void stop_server() {
            web.Stop();
        }

        public void start_server() {
            oauth_verifier = "";
            web.Prefixes.Add("http://localhost:18080/");
            Debug.WriteLine("listening localhost");
            try {
                web.Start();
                HttpListenerContext context = web.GetContext();
                string url_ = context.Request.Url.ToString();
                string[] url_par = Form1.fast_split(url_.Substring(24), "&");
                for (int i = 0; i < url_par.Length; i++) {
                    string[] temp_par = Form1.split_two_piece(url_par[i], "=");
                    if (temp_par[0] == "oauth_verifier") {
                        oauth_verifier = temp_par[1];
                    }
                }
                Debug.WriteLine(oauth_verifier);
                HttpListenerResponse response = context.Response;
                string responseString = "<!DOCTYPE HTML>\n<html lang=\"en\">\n<head>\n<title>Flickr API授權 Auth</title>\n<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">\n<meta name=\"viewport\" content=\"width=400,initial-scale=1,user-scalable=yes\">\n</head>\n<body style=\"background-color: #000000;\">\n<div style=\"margin:auto;text-align:center;color:#00FF00;font-size:20pt;\">應用程式已被授權，請返回Flickr Downloader查看！<br>The application has already been authorized, Please go back to click continue button.<br><br><br><span style=\"font-size:12pt\">此網頁已可以關閉。This Page may close now.</span></div>\n</body>\n</html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                web.Stop();
                lock_form = false;
                label4.Visible = false;
                this.Text = "已授權(Authorized!)";
                label2.Text = "已授權！";
                textBox1.Visible = false;
                timer1.Stop();
                button1.Text = "繼續Continue";
            } catch {
                Debug.WriteLine("listen error");
                if (!cancel_auth) {
                    MessageBox.Show(this, "無法綁定本機通訊連接阜18080，請確定此連接阜無被使用或被封鎖！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cancel_auth = true;
                lock_form = false;
                this.Close();
            }
        }

        private void Verify_waiting_FormClosing(object sender, FormClosingEventArgs e) {
            if (lock_form) {
                e.Cancel = true;
            }
            stop_server();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (int.Parse(timer1.Tag.ToString()) >= 1) {
                timer1.Tag = (int.Parse(timer1.Tag.ToString())-1);
                button1.Text = "繼續Continue (" + (timer1.Tag.ToString()) + ")";
            } else {
                button1.Text = "繼續Continue";
                lock_form = false;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            cancel_auth = false;
            if (lock_form) {
                return;
            } else {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            cancel_auth = true;
            lock_form = false;
            this.Close();
        }

        private void Verify_waiting_Load(object sender, EventArgs e) {
            textBox1.Text = url_XX;
            oauth_verifier = "";
            if (listen_service!=null && listen_service.IsAlive) {
                try {
                    listen_service.Abort();
                } catch {

                }
            }
            listen_service = new Thread(() => start_server());
            listen_service.Start();
            //timer1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e) {
            timer_count = (short) (timer_count - 1);
            if (timer_count<=0) {
                timer2.Stop();
                cancel_auth = true;
                lock_form = false;
                this.Close();
            } else {
                label4.Text = "(" + Math.Floor((decimal)(timer_count/60)) + ":" + (timer_count%60) + ")";
            }
        }

        private void textBox1_Enter(object sender, EventArgs e) {
            textBox1.SelectAll();
        }
    }
}
