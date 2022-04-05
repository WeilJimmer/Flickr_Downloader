using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Flickr_Downloader {
    public partial class pick_albums_form : Form {

        public bool form_must_close = false;

        public int options_5_thread_num = 4;

        public int load_finish = 1;
        
        delegate void StringArgReturningVoidDelegate(string text);

        public static string CrLf = (((char)(13)).ToString() + ((char)(10)).ToString());
        public static string LF = Convert.ToChar(10).ToString();
        public static string CR = Convert.ToChar(13).ToString();

        public System.Threading.Thread main_thread;
        public System.Threading.Thread[] s = new System.Threading.Thread[4];
        public WebClient_auto[] wbcl_ = new WebClient_auto[4];
        public int[] thread_download_number = { (-1), (-1), (-1), (-1) };
        public int[] retry_times = { };
        public bool[] live_X = { };
        public bool[] download_ok = { false, false, false, false };
        public bool[] thread_busy = { false, false, false, false };
        public string[] current_downloader_key = { "", "", "", "" };
        public Dictionary<string, bool> dead_or_not = new Dictionary<string, bool>();
        static Random rand_ = new Random();
        public byte[][] bytes = new byte[4][];
        public static string header_req_USER_SET = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8" + (((char)(13)).ToString() + ((char)(10)).ToString()) + "Accept-Encoding=gzip, deflate" + (((char)(13)).ToString() + ((char)(10)).ToString()) + "Accept-Language=zh-TW,zh;q=0.8,en;q=0.6" + (((char)(13)).ToString() + ((char)(10)).ToString()) + "User-Agent=Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.130 Safari/537.36";

        public bool download_started = false;

        public string[] id_;
        public string[] title_;
        public string[] des_;
        public string[] img_;
        public string[] count_;
        public string[] owner_;
        public string[] ownerid_;

        public string[] url_X_selected;

        public Size temp_size = new Size(240, 240);
        public bool temp_detail = false;

        public pick_albums_form() {
            InitializeComponent();
            listView1.DoubleBuffered(true);
        }

        public string[] fast_split(string str, string key) {
            return System.Text.RegularExpressions.Regex.Split(str, System.Text.RegularExpressions.Regex.Escape(key));
        }

        public string[] split_two_piece(string str, string key) {
            string[] target_str = new string[2];
            int search_ = str.IndexOf(key);
            if (search_ < 0) {
                target_str[0] = "";
                target_str[1] = "";
                return target_str;
            }
            target_str[0] = str.Substring(0, search_);
            target_str[1] = str.Substring(search_ + key.Length);
            return target_str;
        }

        public void header_split(out string[] header_key, out string[] header_value, string header_req) {
            string[] header_line = fast_split(header_req.Trim((char)10), LF.ToString());
            header_key = new string[header_line.Length];
            header_value = new string[header_line.Length];
            for (int i = 0; i < header_line.Length; i++) {
                string[] header_line_line = split_two_piece(header_line[i], "=");
                header_key[i] = header_line_line[0];
                header_value[i] = header_line_line[1];
            }
        }

        public void set_header(ref WebClient_auto wbcl_, ref string header_req, string header_rep_set_cookie, int index_thread) {
            string[] header_key;
            string[] header_value;
            string[] header_key_cookie;
            string[] header_value_cookie;
            header_req = header_req.Trim((char)10);
            wbcl_.Encoding = Encoding.UTF8;
            header_split(out header_key_cookie, out header_value_cookie, header_rep_set_cookie);
            int search_set_cookie = Array.IndexOf(header_key_cookie, "Set-Cookie");
            if (search_set_cookie >= 0) {
                if (header_req == "") {
                    header_req = "Cookie=" + header_value_cookie[search_set_cookie];
                } else {
                    header_req += LF + "Cookie=" + header_value_cookie[search_set_cookie];
                }
            }
            header_split(out header_key, out header_value, header_req);
            for (int i = 0; i < header_key.Length; i++) {
                if (header_key[i] == "") {
                    continue;
                }
                wbcl_.Headers.Add(header_key[i], header_value[i]);
            }
            current_downloader_key[index_thread] = creat_rnd_key();
            wbcl_.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            wbcl_.Headers.Add("WBFT-WebBrowser-Tag-Num", current_downloader_key[index_thread]);
            wbcl_.Headers.Add("WBFT-WebBrowser-Tag-Thread", index_thread.ToString());
        }

        public string creat_rnd_key() {
            string pw_str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0";
            string str = "";
            for (int i = 0; i < 30; i++) {
                str += pw_str[(rand_.Next(0, 62))];
            }
            return str;
        }

        private void SetStatusText(string text) {
            try {
                if (this.statusStrip1.InvokeRequired) {
                    StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetStatusText);
                    this.Invoke(d, new object[] { text });
                } else {
                    this.toolStripStatusLabel1.Text = text;
                }
            } catch {

            }
        }

        private void download_file(string url_X_download, int index_thread) {
            Debug.WriteLine("InThread_num" + index_thread + ":" + thread_download_number[index_thread] + ":" + url_X_download);
            int this_number_ = thread_download_number[index_thread];
            int total_amount = live_X.Length;
            thread_busy[index_thread] = true;
            live_X[this_number_] = false;
            download_ok[index_thread] = false;
            bytes[index_thread] = new byte[] { };
            if (download_started) {
                if (wbcl_[index_thread] != null && wbcl_[index_thread].IsBusy) {
                    try {
                        wbcl_[index_thread].CancelAsync();
                        wbcl_[index_thread].Dispose();
                        Debug.WriteLine("Success - Force to kill last wbcl" + (index_thread + 1));
                    } catch {
                        Debug.WriteLine("FAIL - Force to kill last wbcl" + (index_thread + 1));
                    }
                }
                wbcl_[index_thread] = new WebClient_auto();
                string header_req_X = header_req_USER_SET.ToString();
                set_header(ref wbcl_[index_thread], ref header_req_X, "", index_thread);
                wbcl_[index_thread].DownloadDataCompleted += DownloadDataCallback;
                switch (index_thread) {
                    case 0:
                        Debug.WriteLine("S1");
                        //wbcl_[index_thread].DownloadProgressChanged += DownloadProgressChanged;
                        break;
                    case 1:
                        Debug.WriteLine("S2");
                        //wbcl_[index_thread].DownloadProgressChanged += DownloadProgressChanged2;
                        break;
                    case 2:
                        Debug.WriteLine("S3");
                        //wbcl_[index_thread].DownloadProgressChanged += DownloadProgressChanged3;
                        break;
                    case 3:
                        Debug.WriteLine("S4");
                        //wbcl_[index_thread].DownloadProgressChanged += DownloadProgressChanged4;
                        break;
                }
                try {
                    Debug.WriteLine(url_X_download);
                    wbcl_[index_thread].DownloadDataAsync(new Uri(url_X_download));
                    while (true) {
                        System.Threading.Thread.Sleep(1);
                        if (download_ok[index_thread] == true) {
                            break;
                        }
                        if (download_started == false || form_must_close == true) {
                            wbcl_[index_thread].CancelAsync();
                            wbcl_[index_thread].Dispose();
                            break;
                        }
                    }
                } catch {

                }
            } else {
                thread_busy[index_thread] = false;
                return;
            }
            if (live_X[this_number_] == false) {
                try {
                    wbcl_[index_thread].CancelAsync();
                    wbcl_[index_thread].Dispose();
                } catch {

                }
                this.SetStatusText("下載失敗！FAIL(" + (this_number_ + 1) + "/" + total_amount + ")");
                if (retry_times[this_number_] < 3) {
                    retry_times[this_number_] += 1;
                    download_file(url_X_download, index_thread);
                    return;
                } else {
                    retry_times[this_number_] = 0;
                    this.SetStatusText("下載失敗！Fatal FAIL(" + (this_number_ + 1) + "/" + total_amount + ")\n" + url_X_download);
                }
            } else {
                //this.SetStatusText("下載完成Success(" + (this_number_ + 1) + "/" + total_amount + ")");
                Image temp_image;
                try {
                    temp_image = new System.Drawing.Bitmap(new System.IO.MemoryStream(bytes[index_thread]));
                } catch (Exception) {
                    toolStripStatusLabel1.Text = "轉換圖片檔失敗！";
                    temp_image = (Properties.Resources.dead_img_png);
                }
                Debug.WriteLine("thisnum:" + this_number_);
                try {
                    Debug.WriteLine("photoid:" + id_[this_number_]);
                } catch {
                    for (int x=0;x< id_.Length;x++) {
                        Debug.WriteLine(id_[x]);
                    }
                }
                Image temp_img = ScaleImage(temp_image, 240);
                add_Imagelist(imageList_large, id_[this_number_], temp_img);
                add_Imagelist(imageList_small, id_[this_number_], temp_img);
            }
            thread_busy[index_thread] = false;
        }

        private void button1_Click(object sender, EventArgs e) {
            url_X_selected = new string[] { };
            form_must_close = true;
            download_started = false;
            int j = 0;
            for (int i = 0; i < listView1.Items.Count; i++) {
                if (listView1.Items[i].Checked) {
                    Array.Resize(ref url_X_selected, (j + 1));
                    url_X_selected[j] = listView1.Items[i].SubItems["ID"].Text.ToString();
                    j += 1;
                }
            }
            this.Close();
        }

        public bool check_is_my_key_dead(string mykey) {
            // true = > dead;
            if (dead_or_not.ContainsKey(mykey)) {
                if (dead_or_not[mykey]) {
                    return true;
                }
            }
            return false;
        }

        private void DownloadDataCallback(Object sender, System.Net.DownloadDataCompletedEventArgs e) {
            string mykey = "";
            int this_thread_num = (-1);
            try {
                mykey = ((WebClient_auto)sender).Headers["WBFT-WebBrowser-Tag-Num"].ToString();
                this_thread_num = int.Parse(((WebClient_auto)sender).Headers["WBFT-WebBrowser-Tag-Thread"].ToString());
            } catch {

            }
            if (mykey != "") {
                if (check_is_my_key_dead(mykey)) {
                    Debug.WriteLine("this is dead. abandon.");
                    bytes[this_thread_num] = new byte[] { };
                    live_X[thread_download_number[this_thread_num]] = false;
                    dead_or_not.Remove(mykey);
                    ((WebClient_auto)sender).Dispose();
                    return;
                }
            } else {
                Debug.WriteLine("why is my key empty.");
            }
            try {

                if (e.Cancelled == false && e.Error == null) {
                    bytes[this_thread_num] = e.Result;
                    live_X[thread_download_number[this_thread_num]] = true;
                } else {
                    bytes[this_thread_num] = new byte[] { };
                    live_X[thread_download_number[this_thread_num]] = false;
                }
            } finally {
            }
            download_ok[this_thread_num] = true;
        }

        public static Image ScaleImage(Image image, int maxWidthHeight) {
            var ratioX = (double)maxWidthHeight / image.Width;
            var ratioY = (double)maxWidthHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            int x_start = (int)((maxWidthHeight - newWidth) / 2);
            int y_start = (int)((maxWidthHeight - newHeight) / 2);

            var newImage = new Bitmap(maxWidthHeight, maxWidthHeight);

            using (var graphics = Graphics.FromImage(newImage)) {
                graphics.DrawImage(image, x_start, y_start, newWidth, newHeight);
            }
            return newImage;
        }

        public int switch_get_busy_count() {
            int total = 0;
            if (thread_busy[0]) {
                total += 1;
            } else if (thread_busy[1]) {
                total += 1;
            } else if (thread_busy[2]) {
                total += 1;
            } else if (thread_busy[3]) {
                total += 1;
            }
            return total;
        }

        private int switch_get_NOT_busy() {
            if (!thread_busy[0]) {
                thread_busy[0] = true;
                return 0;
            } else if (!thread_busy[1] && options_5_thread_num >= 2) {
                thread_busy[1] = true;
                return 1;
            } else if (!thread_busy[2] && options_5_thread_num >= 3) {
                thread_busy[2] = true;
                return 2;
            } else if (!thread_busy[3] && options_5_thread_num >= 4) {
                thread_busy[3] = true;
                return 3;
            } else {
                return (-1);
            }
        }

        public void main_download_FUNC() {
            try {
                download_started = true;
                int countt = id_.Length;
                Form1.setToolStripProgressBar_value(toolStripProgressBar1, 0);
                Form1.setToolStripProgressBar_max(toolStripProgressBar1, countt);

                for (int i = 0; i < countt; i++) {
                    Form1.setToolStripProgressBar_value(toolStripProgressBar1, (i + 1));
                    SetStatusText("下載 第 " + (i + 1) + " 個 檔案中…… 共：" + (countt) + " 個");
                    System.Threading.Thread.Sleep(100);
                    if (this.form_must_close) {
                        load_finish = 1;
                        return;
                    }
                    int free_thread = switch_get_NOT_busy();
                    while (free_thread == (-1)) {
                        if (!download_started) {
                            break;
                        }
                        try {
                            System.Threading.Thread.Sleep(10);
                        } catch (NullReferenceException ex) {
                            Debug.WriteLine(ex.ToString());
                        }
                        free_thread = switch_get_NOT_busy();
                    }
                    if (free_thread != (-1)) {
                        thread_busy[free_thread] = true;
                        int temp_i = i;
                        thread_download_number[free_thread] = int.Parse(temp_i.ToString());
                        while (s[free_thread] != null && s[free_thread].IsAlive && download_started) {
                            System.Threading.Thread.Sleep(1);
                        }
                        s[free_thread] = null;
                        string temp_url_X = img_[i].Clone().ToString();
                        s[free_thread] = new System.Threading.Thread(() => download_file(temp_url_X, free_thread));
                        s[free_thread].Start();
                    }

                    if ((img_.Length - 1) == i) {
                        Debug.WriteLine("Waiting");
                        while (switch_get_busy_count() != 0) {
                            System.Threading.Thread.Sleep(10);
                            if (!download_started) {
                                break;
                            }
                        }
                    }
                }
                SetStatusText("完成下載！");
                this.load_finish = 1;
            } catch {

            }
        }

        public void add_Imagelist(ImageList imglist, string key, Image img) {
            Action append = () => add_Imagelist_X(imglist, key,img);
            if (listView1.InvokeRequired) {
                listView1.Invoke(append);
            } else {
                append();
            }
        }

        public static void add_Imagelist_X(ImageList imglist, string key, Image img) {
            imglist.Images.Add(key, img);
        }

        public void form2_handle_data() {

            try {
                listView1.Items.Clear();
                listView1.Groups.Clear();
                imageList_large.Images.Clear();
                imageList_small.Images.Clear();
                Application.DoEvents();

                int countt = img_.Length;

                this.live_X = new bool[countt];
                this.retry_times = new int[countt];
                Form1.set_all_False(ref live_X);
                Form1.set_all_Zero(ref retry_times);

                listView1.Visible = false;
                listView1.SuspendDrawing();

                for (int i = 0; i < countt; i++) {
                    ListViewItem itemx = new ListViewItem(title_[i], id_[i]);
                    ListViewItem.ListViewSubItem subitemx = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx2 = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx3 = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx4 = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx5 = new ListViewItem.ListViewSubItem();
                    subitemx.Name = "ID";
                    subitemx.Text = id_[i];
                    subitemx2.Name = "Description";
                    subitemx2.Text = des_[i];
                    subitemx3.Name = "Count";
                    subitemx3.Text = count_[i];
                    subitemx4.Name = "Owner";
                    subitemx4.Text = owner_[i];
                    subitemx5.Name = "OwnerID";
                    subitemx5.Text = ownerid_[i];
                    itemx.Name = "i";
                    itemx.ToolTipText = "Count:" + count_[i] + "\r\nOwner:" + owner_[i];
                    itemx.SubItems.Add(subitemx);
                    itemx.SubItems.Add(subitemx2);
                    itemx.SubItems.Add(subitemx3);
                    itemx.SubItems.Add(subitemx4);
                    itemx.SubItems.Add(subitemx5);
                    itemx.UseItemStyleForSubItems = false;
                    listView1.Items.Add(itemx);
                    if (i % 500 == 0) {
                        Application.DoEvents();
                    }
                }
                this.Text = "Select albums you want.";
                listView1.ResumeDrawing();
                listView1.Visible = true;
                Application.DoEvents();

                while (main_thread != null && main_thread.IsAlive) {
                    Application.DoEvents();
                    SetStatusText("等待上個進度停止…(Waiting...)");
                    System.Threading.Thread.Sleep(25);
                }

                main_thread = new System.Threading.Thread(() => main_download_FUNC());
                main_thread.Start();
                load_finish = 0;

            } catch(Exception ex) {
                Debug.WriteLine(ex.ToString());
            }


        }

        private void timer1_Tick(object sender, EventArgs e) {
            timer1.Stop();
            timer1.Enabled = false;
            form2_handle_data();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e) {
            download_started = false;
            this.form_must_close = true;
            while (switch_get_busy_count() != 0 || this.load_finish != 1) {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
        }

        private void 全選ToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < listView1.Items.Count; i++) {
                listView1.Items[i].Checked = true;
            }
        }

        private void 全不選ToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < listView1.Items.Count; i++) {
                listView1.Items[i].Checked = false;
            }
        }

        public bool string_to_bool(string str) {
            str = str.ToLower();
            if (str == "1") {
                return true;
            } else if (str == "0") {
                return false;
            } else if (str == "true") {
                return true;
            } else if (str == "false") {
                return false;
            } else {
                return false;
            }
        }

        private void 反選ToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < listView1.Items.Count; i++) {
                if (listView1.Items[i].Checked) {
                    listView1.Items[i].Checked = false;
                } else {
                    listView1.Items[i].Checked = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            url_X_selected = new string[] { };
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (listView1.LargeImageList==imageList_large) {
                listView1.LargeImageList = imageList_small;
                temp_size = imageList_small.ImageSize;
            } else {
                listView1.LargeImageList = imageList_large;
                temp_size = imageList_large.ImageSize;
            }
            listView1.Refresh();
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e) {
            if (e.Item.Checked) {
                e.Item.BackColor = Color.Blue;
                e.Item.ForeColor = Color.Yellow;
            } else {
                e.Item.BackColor = this.BackColor;
                e.Item.ForeColor = this.ForeColor;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            if (listView1.View == View.Details) {
                temp_detail = false;
                listView1.View = View.LargeIcon;
            } else {
                temp_detail = true;
                listView1.View = View.Details;
            }
        }

        private void pick_albums_form_ResizeEnd(object sender, EventArgs e) {
            listView1.Refresh();
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e) {
            e.DrawDefault = true;
            if (temp_detail) {
                return;
            }
            if (e.Item.Checked) {
                Size sz = temp_size;
                int w = sz.Width + 2;
                int h = sz.Height + 3;
                int x = (e.Bounds.Width - sz.Width) / 2 + e.Bounds.X + 7;
                int y = e.Bounds.Top + 1;
                using (Pen pen = new Pen(Color.Lime, 2f)) {
                    pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen, x, y, w, h);
                }
            }
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {
            
            e.DrawDefault = true;

            if (e.Item.Checked && e.SubItem.Name=="i") {
                Size sz = e.Item.Bounds.Size;
                int w = sz.Width;
                int h = sz.Height;
                int x = e.Bounds.X;
                int y = e.Bounds.Top;
                using (Pen pen = new Pen(Color.Lime, 5f)) {
                    pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Left;
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawLine(pen, x, y, x, (y+h));
                }
            }

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            //listView1.Refresh();
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {
            e.DrawDefault = true;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            List<string> id_list = new List<string>();
            for (int i = 0; i < listView1.Items.Count; i++) {
                if (listView1.Items[i].Checked) {
                    id_list.Add(listView1.Items[i].SubItems["ID"].Text.ToString());
                }
            }
            if (id_list.Count != 0) {
                Clipboard.SetText(String.Join(CrLf, id_list.ToArray()));
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            List<string> id_list = new List<string>();
            for (int i = 0; i < listView1.Items.Count; i++) {
                if (listView1.Items[i].Checked) {
                    string temp_str = listView1.Items[i].SubItems["OwnerID"].Text.ToString();
                    if (!id_list.Contains(temp_str)) {
                        id_list.Add(temp_str);
                    }
                }
            }
            if (id_list.Count != 0) {
                Clipboard.SetText(String.Join(CrLf, id_list.ToArray()));
            }
        }
    }
}
