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
    public partial class pick_photos_form : Form {
        
        public bool form_must_close = false;

        public int options_1_orginal_names = 1;
        public string options_7_time_format = "yyyy-MM-dd HH_mm_ss";
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

        public string[] url_X;
        public string[] img_X;
        public string[] photo_title_X;
        public string[] photo_id_X;
        public string[] group_title_X;
        public string[] group_id_X;
        public string[] owner_X;
        public string[] ownerid_X;
        public string[] dateTaken_X;

        public string[] url_X_selected = new string[] { };

        public Size temp_size = new Size(240, 240);
        public bool temp_detail = false;

        public bool hidden_album_col = false;
        public ListViewItem[] myCache; //array to cache items for the virtual list
        public bool[] checked_item;
        public int firstItem; //stores the index of the first item in the cache
        public int visible_first_index = 0;
        public int page = 1;
        public int per_page = 50;
        public int max_page = 1;
        public Color default_bg_color = Color.Black;
        public Color default_fe_color = Color.White;

        public pick_photos_form() {
            InitializeComponent();
            listView1.DoubleBuffered(true);
        }

        public int calculated_checked(bool[] arr) {
            return arr.Count(c => c);
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
                        System.Threading.Thread.Sleep(50);
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
                    Debug.WriteLine("photoid:" + photo_id_X[this_number_]);
                } catch {
                    for (int x=0;x< photo_id_X.Length;x++) {
                        Debug.WriteLine(photo_id_X[x]);
                    }
                }
                add_Imagelist(listView1, visible_first_index, page, per_page, imageList_large, photo_id_X[this_number_], ScaleImage(temp_image, 240));
                add_Imagelist(listView1, visible_first_index, page, per_page, imageList_small, photo_id_X[this_number_], ScaleImage(temp_image, 120));
            }
            thread_busy[index_thread] = false;
        }

        public string filename_filter(string str) {
            str = str.Replace("*", "＊").Replace("|", "｜").Replace("\\", "＼").Replace(":", "：").Replace("\"", "'").Replace("<", "＜").Replace(">", "＞").Replace("?", "？").Replace("/", "／").Replace(LF, "").Replace(CR, "");
            if (str.Length > 50) {
                str = str.Substring(0, 50) + "…";
            }
            return str;
        }

        public string get_time_by_str_and_type(string datetime_str) {
            try {
                DateTime date = DateTime.ParseExact(datetime_str, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                return filename_filter(date.ToString(options_7_time_format));
            } catch {
                return "Date_Error";
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            url_X_selected = new string[] { };
            this.form_must_close = true;
            download_started = false;
            int j = 0;
            string temp_title = "";
            for (int i = 0; i < myCache.Length; i++) {
                if (checked_item[i]) {
                    Array.Resize(ref url_X_selected, (j + 1));
                    if (options_1_orginal_names == 1) {
                        try {
                            temp_title = myCache[i].Text;
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 0) {
                        temp_title = (j+1).ToString();
                    } else if (options_1_orginal_names == 2) {
                        temp_title = myCache[i].SubItems["ID"].Text.ToString();
                    } else if (options_1_orginal_names == 3) {
                        try {
                            temp_title = myCache[i].Text + " - " + myCache[i].SubItems["ID"].Text.ToString();
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 4) {
                        try {
                            temp_title = myCache[i].SubItems["ID"].Text.ToString() + " - " + myCache[i].Text;
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 5) {
                        try {
                            string album_text = myCache[i].SubItems["gTitle"].Text.ToString();
                            if (album_text!="") {
                                temp_title = album_text + " - " + myCache[i].Text + " - " + myCache[i].SubItems["ID"].Text.ToString();
                            } else {
                                temp_title = myCache[i].Text + " - " + myCache[i].SubItems["ID"].Text.ToString();
                            }
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 6) {
                        try {
                            string album_text = myCache[i].SubItems["gTitle"].Text.ToString();
                            if (album_text != "") {
                                temp_title = album_text + " - " + myCache[i].SubItems["ID"].Text.ToString();
                            } else {
                                temp_title = myCache[i].SubItems["ID"].Text.ToString();
                            }
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 7) {
                        try {
                            string album_text = myCache[i].SubItems["gTitle"].Text.ToString();
                            if (album_text != "") {
                                temp_title = album_text + " - " + myCache[i].Text;
                            } else {
                                temp_title = myCache[i].Text;
                            }
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 8) {
                        try {
                            temp_title = get_time_by_str_and_type(myCache[i].SubItems["dateTaken"].Text.ToString()) + " - " + myCache[i].Text + " - " + myCache[i].SubItems["ID"].Text.ToString();
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 9) {
                        try {
                            temp_title = get_time_by_str_and_type(myCache[i].SubItems["dateTaken"].Text.ToString()) + " - " + myCache[i].Text;
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 10) {
                        try {
                            temp_title = get_time_by_str_and_type(myCache[i].SubItems["dateTaken"].Text.ToString()) + " - " + myCache[i].SubItems["ID"].Text.ToString();
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 11) {
                        try {
                            temp_title = myCache[i].Text + " - " + myCache[i].SubItems["ID"].Text.ToString() + " - " + get_time_by_str_and_type(myCache[i].SubItems["dateTaken"].Text.ToString());
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 12) {
                        try {
                            temp_title = myCache[i].Text + " - " + get_time_by_str_and_type(myCache[i].SubItems["dateTaken"].Text.ToString()) + " - " + myCache[i].SubItems["ID"].Text.ToString();
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 13) {
                        try {
                            temp_title = myCache[i].Text + " - " + get_time_by_str_and_type(myCache[i].SubItems["dateTaken"].Text.ToString());
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 14) {
                        try {
                            temp_title = myCache[i].SubItems["ID"].Text.ToString() + " - " + get_time_by_str_and_type(myCache[i].SubItems["dateTaken"].Text.ToString()) + " - " + myCache[i].Text;
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 15) {
                        try {
                            temp_title = myCache[i].SubItems["ID"].Text.ToString() + " - " + get_time_by_str_and_type(myCache[i].SubItems["dateTaken"].Text.ToString());
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    } else if (options_1_orginal_names == 16) {
                        try {
                            temp_title = get_time_by_str_and_type(myCache[i].SubItems["dateTaken"].Text.ToString());
                        } catch {
                            temp_title = "未命名 NoName";
                        }
                    }
                    url_X_selected[j] = myCache[i].SubItems["url_X"].Text.ToString() + "\r\n" + temp_title;
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
                int countt = img_X.Length;
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
                            System.Threading.Thread.Sleep(50);
                        }
                        s[free_thread] = null;
                        string temp_url_X = img_X[i].Clone().ToString();
                        s[free_thread] = new System.Threading.Thread(() => download_file(temp_url_X, free_thread));
                        s[free_thread].Start();
                    }

                    if ((img_X.Length - 1) == i) {
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

        public void add_Imagelist(ListView listView, int visible_first_index, int page, int per_page, ImageList imglist, string key, Image img) {
            Action append = () => add_Imagelist_X(listView, visible_first_index, page, per_page, imglist, key,img);
            if (listView1.InvokeRequired) {
                listView1.Invoke(append);
            } else {
                append();
            }
        }

        public static void add_Imagelist_X(ListView listView, int visible_first_index,int page, int per_page, ImageList imglist, string key, Image img) {
            imglist.Images.Add(key, img);
            int image_count = imglist.Images.Count;
            int target_listview_index = image_count - (per_page * (page - 1)) - 1;
            if ((page)==((int) Math.Ceiling((decimal)image_count/per_page))) {
                if ((target_listview_index)<=(visible_first_index+4)) {
                    listView.Refresh();
                }
            }
        }

        //The basic VirtualMode function.  Dynamically returns a ListViewItem
        //with the required properties; in this case, the square of the index.
        void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) {
            //Caching is not required but improves performance on large sets.
            //To leave out caching, don't connect the CacheVirtualItems event 
            //and make sure myCache is null.

            int now_index = e.ItemIndex + (page-1) * per_page;

            //check to see if the requested item is currently in the cache
            if (myCache != null && now_index >= firstItem && now_index < firstItem + myCache.Length) {
                //A cache hit, so get the ListViewItem from the cache instead of making a new one.
                e.Item = myCache[now_index - firstItem];
                e.Item.ImageIndex = imageList_small.Images.IndexOfKey(myCache[now_index - firstItem].SubItems["ID"].Text);
            } else {
                //A cache miss, so create a new ListViewItem and pass it back.
                ListViewItem itemx = new ListViewItem("(Null)");
                ListViewItem.ListViewSubItem subitemx = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subitemx2 = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subitemx3 = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subitemx4 = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subitemx5 = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subitemx6 = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subitemx7 = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subitemx8 = new ListViewItem.ListViewSubItem();
                subitemx.Name = "ID";
                subitemx.Text = "-1";
                subitemx2.Name = "gTitle";
                subitemx2.Text = "None";
                subitemx3.Name = "gID";
                subitemx3.Text = "-1";
                subitemx4.Name = "owner";
                subitemx4.Text = "None";
                subitemx5.Name = "ownerID";
                subitemx5.Text = "None";
                subitemx6.Name = "url_X";
                subitemx6.Text = "";
                subitemx7.Name = "dateTaken";
                subitemx7.Text = "None";
                subitemx8.Name = "index";
                subitemx8.Text = "-1";
                itemx.Name = "x";
                itemx.UseItemStyleForSubItems = false;
                itemx.SubItems.Add(subitemx);
                itemx.SubItems.Add(subitemx2);
                itemx.SubItems.Add(subitemx3);
                itemx.SubItems.Add(subitemx4);
                itemx.SubItems.Add(subitemx5);
                itemx.SubItems.Add(subitemx6);
                itemx.SubItems.Add(subitemx7);
                itemx.SubItems.Add(subitemx8);
                itemx.Checked = false;
                e.Item = itemx;
                e.Item.ImageIndex = -1;
            }
        }

        //This event handler enables search functionality, and is called
        //for every search request when in Virtual mode.
        void listView1_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e) {
            //We've gotten a search request.
            //In this example, finding the item is easy since it's
            //just the square of its index.  We'll take the square root
            //and round.
            double x = 0;
            if (Double.TryParse(e.Text, out x)) //check if this is a valid search
            {
                x = Math.Sqrt(x);
                x = Math.Round(x);
                e.Index = (int)x;

            }
            //If e.Index is not set, the search returns null.
            //Note that this only handles simple searches over the entire
            //list, ignoring any other settings.  Handling Direction, StartIndex,
            //and the other properties of SearchForVirtualItemEventArgs is up
            //to this handler.
        }

        public void form2_handle_data() {
            try {
                listView1.VirtualMode = false;
                myCache = null;
                checked_item = null;
                listView1.Items.Clear();
                listView1.Groups.Clear();
                imageList_large.Images.Clear();
                imageList_small.Images.Clear();
                Application.DoEvents();

                int countt = img_X.Length;

                this.live_X = new bool[countt];
                this.retry_times = new int[countt];
                Form1.set_all_False(ref live_X);
                Form1.set_all_Zero(ref retry_times);

                listView1.Visible = false;
                listView1.SuspendDrawing();

                hidden_album_col = false;
                myCache = new ListViewItem[countt];
                checked_item = new bool[countt];
                max_page = (int)Math.Ceiling((decimal)countt / per_page);
                numericUpDown1.Maximum = max_page;
                label1.Text = "/" + max_page + ")";
                for (int i = 0; i < countt; i++) {
                    ListViewItem itemx = new ListViewItem(photo_title_X[i]);
                    ListViewItem.ListViewSubItem subitemx = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx2 = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx3 = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx4 = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx5 = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx6 = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx7 = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem subitemx8 = new ListViewItem.ListViewSubItem();
                    subitemx.Name = "ID";
                    subitemx.Text = photo_id_X[i];
                    subitemx2.Name = "gTitle";
                    subitemx2.Text = group_title_X[i];
                    subitemx3.Name = "gID";
                    subitemx3.Text = group_id_X[i];
                    subitemx4.Name = "owner";
                    subitemx4.Text = owner_X[i];
                    subitemx5.Name = "ownerID";
                    subitemx5.Text = ownerid_X[i];
                    subitemx6.Name = "url_X";
                    subitemx6.Text = url_X[i];
                    subitemx7.Name = "dateTaken";
                    subitemx7.Text = dateTaken_X[i];
                    subitemx8.Name = "index";
                    subitemx8.Text = (i.ToString());
                    itemx.Name = "i";
                    if (group_id_X[i]!="") {
                        itemx.ToolTipText = group_title_X[i];
                    } else {
                        hidden_album_col = true;
                        itemx.ToolTipText = owner_X[i];
                    }
                    itemx.UseItemStyleForSubItems = false;
                    itemx.SubItems.Add(subitemx);
                    itemx.SubItems.Add(subitemx2);
                    itemx.SubItems.Add(subitemx3);
                    itemx.SubItems.Add(subitemx4);
                    itemx.SubItems.Add(subitemx5);
                    itemx.SubItems.Add(subitemx6);
                    itemx.SubItems.Add(subitemx7);
                    itemx.SubItems.Add(subitemx8);
                    itemx.Checked = false;
                    myCache[i] = (itemx);
                    checked_item[i] = false;
                    if (i % 500 == 0) {
                        Application.DoEvents();
                    }
                }

                this.Text = "Select photos you want. [1/" + max_page + "] per_page=" + per_page;
                listView1.ResumeDrawing();
                listView1.Visible = true;
                default_bg_color = this.listView1.BackColor;
                default_fe_color = this.listView1.ForeColor;
                listView1.OwnerDraw = true;
                listView1.DrawItem += new DrawListViewItemEventHandler(listView1_DrawItem);
                listView1.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(listView1_RetrieveVirtualItem);
                listView1.SearchForVirtualItem += new SearchForVirtualItemEventHandler(listView1_SearchForVirtualItem);
                if (countt< per_page) {
                    listView1.VirtualListSize = countt;
                } else {
                    listView1.VirtualListSize = per_page;
                }
                listView1.VirtualMode = true;
                Application.DoEvents();

                while (main_thread != null && main_thread.IsAlive) {
                    Application.DoEvents();
                    SetStatusText("等待上個進度停止…(Waiting...)");
                    System.Threading.Thread.Sleep(25);
                }
                main_thread = new System.Threading.Thread(() => main_download_FUNC());
                main_thread.Start();
                load_finish = 0;
            } catch (Exception ex) {
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
            for (int i = 0; i < checked_item.Length; i++) {
                checked_item[i] = true;
                myCache[i].Checked = checked_item[i];
            }
            title_changed_func();
            //listView1.Refresh();
        }

        private void 全不選ToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < checked_item.Length; i++) {
                checked_item[i] = false;
                myCache[i].Checked = checked_item[i];
            }
            title_changed_func();
            //listView1.Refresh();
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
            for (int i = 0; i < checked_item.Length; i++) {
                checked_item[i] = !checked_item[i];
                myCache[i].Checked = checked_item[i];
            }
            title_changed_func();
            //listView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e) {
            url_X_selected = new string[] { };
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (listView1.LargeImageList == imageList_large) {
                listView1.LargeImageList = imageList_small;
                temp_size = imageList_small.ImageSize;
            } else {
                listView1.LargeImageList = imageList_large;
                temp_size = imageList_large.ImageSize;
            }
            listView1.Refresh();
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e) {
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            if (listView1.View == View.Details) {
                temp_detail = false;
                listView1.View = View.LargeIcon;
            } else {
                temp_detail = true;
                listView1.View = View.Details;
                if (hidden_album_col) {
                    try {
                        listView1.Columns[2].Width = 0;
                        listView1.Columns[3].Width = 0;
                    } catch (Exception ex) {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }
            listView1.Refresh();
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {
            e.DrawDefault = true;
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e) {
            e.DrawDefault = true;
            visible_first_index = e.Item.Index;
            if (temp_detail) {
                return;
            }
            if (e.Item.Checked) {
                Size sz = temp_size;
                int w = sz.Width + 2;
                int h = sz.Height + 3;
                int x = (e.Bounds.Width - sz.Width) / 2 + e.Bounds.X - 1;
                int y = e.Bounds.Top + 1;
                using (Pen pen = new Pen(Color.Lime, 2f)) {
                    pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen, x, y, w, h);
                }
                if (e.Item.BackColor!= Color.Blue) {
                    e.Item.BackColor = Color.Blue;
                    e.Item.ForeColor = Color.Yellow;
                }
            } else {
                e.Item.Checked = true;
                e.Item.Checked = false;
                if (e.Item.BackColor != default_bg_color) {
                    e.Item.BackColor = default_bg_color;
                    e.Item.ForeColor = default_fe_color;
                }
            }
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {
            visible_first_index = e.Item.Index;
            e.DrawDefault = true;
            if (e.Item.Checked && e.SubItem.Name == "i") {
                Size sz = e.Item.Bounds.Size;
                int w = sz.Width;
                int h = sz.Height;
                int x = e.Bounds.X;
                int y = e.Bounds.Top;
                using (Pen pen = new Pen(Color.Lime, 5f)) {
                    pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Left;
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawLine(pen, x, y, x, (y + h));
                }
                if (e.SubItem.BackColor != Color.Blue) {
                    e.SubItem.BackColor = Color.Blue;
                    e.SubItem.ForeColor = Color.Yellow;
                }
            } else {
                if (e.SubItem.BackColor != default_bg_color) {
                    e.SubItem.BackColor = default_bg_color;
                    e.SubItem.ForeColor = default_fe_color;
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            List<string> id_list = new List<string>();
            string temp_id = "";
            for (int i = 0; i < myCache.Length; i++) {
                if (myCache[i].Checked) {
                    temp_id = myCache[i].SubItems["gID"].Text.ToString();
                    if (temp_id!="" && !id_list.Contains(temp_id)) {
                        id_list.Add(temp_id);
                    }
                }
            }
            if (id_list.Count!=0) {
                Clipboard.SetText(String.Join(CrLf, id_list.ToArray()));
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            List<string> id_list = new List<string>();
            string temp_id = "";
            for (int i = 0; i < myCache.Length; i++) {
                if (myCache[i].Checked) {
                    temp_id = myCache[i].SubItems["OwnerID"].Text.ToString();
                    if (temp_id != "" && !id_list.Contains(temp_id)) {
                        id_list.Add(temp_id);
                    }
                }
            }
            if (id_list.Count != 0) {
                Clipboard.SetText(String.Join(CrLf, id_list.ToArray()));
            }
        }

        private void pick_photos_form_ResizeEnd(object sender, EventArgs e) {
            listView1.Refresh();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e) {
            ListViewItem item = listView1.GetItemAt(e.X, e.Y);
            int target_item_index = (-1);
            int.TryParse(item.SubItems["index"].Text, out target_item_index);
            Debug.WriteLine(item);
            Debug.WriteLine(target_item_index);
            if (target_item_index < checked_item.Length && target_item_index != (-1)) {
                checked_item[target_item_index] = item.Checked;
            }
            title_changed_func();
            //listView1.Refresh();
        }

        private void 打勾選擇toolStripMenuItem5_Click(object sender, EventArgs e) {
            for (int i=0;i<listView1.SelectedIndices.Count; i++) {
                int index_ = (listView1.SelectedIndices[i]+((page-1)*per_page));
                if (index_< checked_item.Length && index_>(-1)) {
                    checked_item[index_] = !checked_item[index_];
                    myCache[index_].Checked = checked_item[index_];
                }
            }
            title_changed_func();
            //listView1.Refresh();
        }

        private void first_page_button_Click(object sender, EventArgs e) {
            page = 1;
            numericUpDown1.Value = page;
        }

        private void prev_page_button_Click(object sender, EventArgs e) {
            page -= 1;
            if (page < 1) {
                page = 1;
            }
            numericUpDown1.Value = page;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            page = (int) numericUpDown1.Value;
            if (page == max_page) {
                int last_test = checked_item.Length % per_page;
                if (last_test!=0) {
                    listView1.VirtualListSize = last_test;
                }
            } else if( page==1 ) {
                if (checked_item.Length < per_page) {
                    listView1.VirtualListSize = checked_item.Length;
                } else {
                    listView1.VirtualListSize = per_page;
                }
            } else {
                listView1.VirtualListSize = per_page;
            }
            title_changed_func();
            listView1.Refresh();
            if (listView1.Items.Count!=0) {
                listView1.EnsureVisible(0);
            }
        }

        public void title_changed_func() {
            int count_checked = calculated_checked(checked_item);
            button1.Text = "[" + count_checked + "✓] 確定 (OK)";
            this.Text = "Select photos you want. (" + count_checked + "✓) [" + page + "/" + max_page + "] per_page=" + per_page;
        }

        private void next_page_button_Click(object sender, EventArgs e) {
            page += 1;
            if (page > max_page) {
                page = max_page;
            }
            numericUpDown1.Value = page;
        }

        private void final_page_button_Click(object sender, EventArgs e) {
            page = max_page;
            numericUpDown1.Value = page;
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e) {
        }

        private void 重新整理toolStripMenuItem5_Click(object sender, EventArgs e) {
            listView1.Refresh();
        }
    }
}
