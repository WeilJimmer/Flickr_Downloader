using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Runtime.InteropServices;
using DaveChambers.FolderBrowserDialogEx;
using SevenZip;
using Goheer.EXIF;
using System.Drawing.Imaging;

namespace Flickr_Downloader {

    public partial class Form1 : Form {
        
        public static bool beta_version = true;

        public int this_app_exist_number = (-1);

        private readonly object _thisLock_main_func = new object();
        private readonly object _thisLock_get_thead_id = new object();
        private static readonly object _thisLock = new object();
        public bool timer1_START = false;
        public bool timer1_STOP = false;

        public int temp_value_taskbar = 0;
        public int temp_max_taskbar = 0;

        delegate void StringArgReturningVoidDelegate(string text);

        public class Append_quen_class {
            public string String_ADD { get; set; }
            public Color Color1 { get; set; }
            public Color Color2 { get; set; }
        }

        public bool inited = false;

        public static string exe_flickr_download_password = "本白樺論壇團隊之特殊算法，請不要亂用我的Key好嗎？";
        public static byte[] hash_byte = creat_hash(System.Text.UTF8Encoding.UTF8.GetBytes(exe_flickr_download_password));
        public bool custom_key = false;
        public static string default_api_key = "021e1fd66f561b265eac365661879785";
        public static string default_api_secret = "48685f9b9271b284";
        public string api_key = "6f92fee8b4a726215b827c0af43afc70";
        public string api_secret = "4c4fa981ad30e375";

        public bool signed_auth_bool = false;

        public string oauth_secret = "";
        public string oauth_token = "";

        public string userinfo_fullname = "";
        public string userinfo_nsid = "";
        public string userinfo_username = "";

        public static string CrLf = (((char)(13)).ToString() + ((char)(10)).ToString());
        public static string LF = Convert.ToChar(10).ToString();
        public static string CR = Convert.ToChar(13).ToString();

        public string save_dir = Application.StartupPath; //結尾沒有斜線
        public static string header_req_USER_SET = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8" + (((char)(13)).ToString() + ((char)(10)).ToString()) + "Accept-Encoding=gzip, deflate" + (((char)(13)).ToString() + ((char)(10)).ToString()) + "Accept-Language=zh-TW,zh;q=0.8,en;q=0.6" + (((char)(13)).ToString() + ((char)(10)).ToString()) + "User-Agent=Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.130 Safari/537.36";

        public int tabs_ = 0;

        //Tab 1
        public int types_ = 0;//config
        // Tab 1 END

        //Tab 2
        public string search_keyword = "";
        public string search_user_id = "";
        public string search_group_id = "";
        public int search_userid_type = 0;
        public int search_userid_privacy = 0;
        public int search_safe_search = 0;
        public int search_perpage = 50;//config
        public int search_startpage = 1;//config
        public int search_fetchpage = 1;//config
        //Tab 2 END

        /*
        00	-	預設數字化 (Default Index Number)
        01	-	原相片標題 (Title)
        02	-	原相片ID (ID)
        03	-	原相片標題 - ID (Title - ID)
        04	-	原相片ID - 標題 (ID - Title)
        05	-	相簿名稱 - 標題 - ID (Album - Title - ID)
        06	-	相簿名稱 - ID (Album - ID)
        07	-	相簿名稱 - 標題 (Album - Title)
        08	-	拍攝日期 - 標題 - ID (DateTaken - Title - ID)
        09	-	拍攝日期 - 標題 (DateTaken - Title)
        10	-	拍攝日期 - ID (DateTaken - ID)
        11	-	標題 - ID - 拍攝日期 (Title - ID - DateTaken)
        12	-	標題 - 拍攝日期 - ID (Title - DateTaken - ID)
        13	-	標題 - 拍攝日期 (Title - DateTaken)
        14	-	ID - 拍攝日期 - 標題 (ID - DateTaken - Title)
        15	-	ID - 拍攝日期 (ID - DateTaken)
        16	-	拍攝日期 (DateTaken)
        */

        public int options_1_orginal_names = 3;
        public bool options_2_auto_make_dirs = true;
        public bool options_3_preview = true;
        public int options_4_size = 0;
        public int options_5_thread_num = 4;
        public bool options_6_skip_download = false;
        public string options_7_time_format = "yyyy-MM-dd HH_mm_ss";
        public bool options_8_video_download = false;

        public bool download_started = false;
        public bool pause_download = false;

        public System.Threading.Thread main_thread;
        public System.Threading.Thread[] s = new System.Threading.Thread[4];
        public System.Threading.Thread[] s_p = new System.Threading.Thread[4];
        public bool[] s_p_running = new bool[] { false, false, false ,false };
        public WebClient_auto[] wbcl_ = new WebClient_auto[4];
        public int[] thread_download_number = { (-1), (-1), (-1), (-1) };
        public int[] retry_times = { };
        public bool[] live_X = { };
        public string[] photo_url_i_ARR_save = { };
        public string[] photo_name_i_ARR_save = { };
        public string album_name_X_SAVE = "";
        public bool[] download_ok = { false, false, false, false };
        public bool[] thread_busy = { false, false, false, false };
        public string[] current_downloader_key = { "", "", "", "" };

        public Dictionary<string, bool> dead_or_not = new Dictionary<string, bool>();
        public List<String> fail_list_name = new List<string>();
        public List<String> fail_list_url = new List<string>();
        static Random rand_ = new Random();

        public long[] last_byte = { 0, 0, 0, 0 };
        public long[] now_byte = { 0, 0, 0, 0 };
        public long[] total_byte = { 0, 0, 0, 0 };
        public double[] speed_kbyte = { 1, 1, 1, 1 };
        public int[] freeze_time = { 0, 0, 0, 0 };

        public bool update_msg_show_ = false;

        public bool form_must_close = false;
        public bool allow_this_close = false;
        public byte[][] bytes = new byte[4][];

        public string[] url_ = new string[] { };
        public bool read_from_save_file = false;
        public bool save_file_20 = false;
        public int save_v2 = 0;
        public int save_v3 = 0;

        public Color color_f_w = Color.White;
        public Color color_f_g = Color.Lime;
        public Color color_f_m = Color.Magenta;
        public int exe_bg_color = -16777216;
        public int exe_f_color = -16711936;
        public int exe_button_color = -256;
        public int exe_start_first = 1;
        public int exe_allow_exit = 0;
        public int exe_width = 0;
        public int exe_height = 0;
        public bool exe_clipboard_mon_enabled = true;
        public bool exe_show_detail_enabled = false;
        public bool exe_remember_save_path_enabled = false;
        public string exe_remember_save_path = "";

        public Color bg_color_exe = Color.Black;

        public bool exe_sound_enabled = true;
        System.Media.SoundPlayer snd;

        public int break_point_num = 0;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        private const int WM_DRAWCLIPBOARD = 0x0308;
        private IntPtr _clipboardViewerNext;

        FolderBrowserDialogEx cfbd = new FolderBrowserDialogEx();

        public Form1() {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
            if (System.IO.File.Exists(Application.StartupPath + "\\key.log")) {
                custom_key = true;
                try {
                    string[] custom_key_arr = fast_split(System.Text.UTF8Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(Application.StartupPath + "\\key.log")), CrLf);
                    api_key = custom_key_arr[0];
                    api_secret = custom_key_arr[1];
                } catch (Exception) {
                    api_key = default_api_key;
                    api_secret = default_api_secret;
                }
            }
            if (System.IO.File.Exists(Application.StartupPath + "\\config.ini")) {
                exe_start_first = 0;
                read_config();
                set_from_config();
            } else {
                write_config();
                photos_sizes_setting_obj.SelectedIndex = options_4_size;
                file_name_setting_obj.SelectedIndex = options_1_orginal_names;
                toolStripComboBox1.SelectedIndex = (options_5_thread_num-1);
            }
            System.IO.Stream str = Properties.Resources.finish;
            snd = new System.Media.SoundPlayer(str);
            if (System.IO.File.Exists(Application.StartupPath + "\\userinfo.log")) {
                read_oauth_file();
            }
            this.Icon = get_icon_();
            notifyIcon1.Icon = this.Icon;
            //this.statusStrip1.Renderer= new CustomRenderer();
            //bottom_status_prog.Spring = true;
            statusStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;
            statusStrip1.CanOverflow = true;
            _clipboardViewerNext = SetClipboardViewer(this.Handle);
            cfbd.Title = "選擇存檔資料夾 Choose a dir to save files.";
            cfbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            cfbd.ShowEditbox = true;
            cfbd.ShowNewFolderButton = true;
            cfbd.RootFolder = Environment.SpecialFolder.Desktop;
            cfbd.StartPosition = FormStartPosition.CenterScreen;
            toolStripTextBox1.TextChanged += new EventHandler(this.toolStripTextBox1_TextChanged);
            toolStripTextBox1.MaxLength = 50;
            inited = true;
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);    // Process the message 

            if (m.Msg == WM_DRAWCLIPBOARD) {
                IDataObject iData = Clipboard.GetDataObject();      // Clipboard's data
                Debug.WriteLine("in clip");
                if (iData.GetDataPresent(DataFormats.Text)) {
                    string text = (string)iData.GetData(DataFormats.Text);      // Clipboard text
                    if (exe_clipboard_mon_enabled) {
                        Debug.WriteLine(text);
                        do_ADD_url_check(fast_split(text,LF));
                    }
                }
            }
        }

        public static Image handle_orientation(string path_) {
            var bmp = new Bitmap(path_);
            try {
                var exif = new EXIFextractor(ref bmp, "n");
                if (exif["Orientation"] != null) {
                    RotateFlipType flip = OrientationToFlipType(exif["Orientation"].ToString());
                    Debug.WriteLine("exif:"+exif["Orientation"].ToString());
                    if (flip != RotateFlipType.RotateNoneFlipNone) {
                        bmp.RotateFlip(flip);
                        exif.setTag(0x112, "1");
                    }
                }
                return bmp;
            } catch {
                Debug.WriteLine("error");
            }
            return bmp;
        }

        public static RotateFlipType OrientationToFlipType(string orientation) {
            try {
                switch (int.Parse(orientation)) {
                    case 1:
                        return RotateFlipType.RotateNoneFlipNone;
                    case 2:
                        return RotateFlipType.RotateNoneFlipX;
                    case 3:
                        return RotateFlipType.Rotate180FlipNone;
                    case 4:
                        return RotateFlipType.Rotate180FlipX;
                    case 5:
                        return RotateFlipType.Rotate90FlipX;
                    case 6:
                        return RotateFlipType.Rotate90FlipNone;
                    case 7:
                        return RotateFlipType.Rotate270FlipX;
                    case 8:
                        return RotateFlipType.Rotate270FlipNone;
                    default:
                        return RotateFlipType.RotateNoneFlipNone;
                }
            } catch {
                return RotateFlipType.RotateNoneFlipNone;
            }
        }

        public string get_time_by_str_and_type(string datetime_str) {
            try {
                DateTime date = DateTime.ParseExact(datetime_str, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                return filename_filter(date.ToString(options_7_time_format));
            } catch {
                return "Date_Error";
            }
        }

        public void do_ADD_url_check(string[] new_url) {
            if (!download_started) {
                string[] url_X_test = fast_split(tab1_textBox1.Text,CrLf);
                url_X_test=remove_empty_array_element(url_X_test);
                for (int i=0;i<new_url.Length;i++) {
                    string new_url_item_ = new_url[i].Trim(' ').Trim('\t').Trim('\r');
                    if ((new_url_item_.StartsWith("https://flickr.com/") || new_url_item_.StartsWith("http://flickr.com/") || new_url_item_.StartsWith("https://www.flickr.com/") || new_url_item_.StartsWith("http://www.flickr.com/") || new_url_item_.StartsWith("https://flic.kr/") || new_url_item_.StartsWith("http://flic.kr/")) && Array.IndexOf(url_X_test, new_url_item_) < 0) {
                        Array.Resize<string>(ref url_X_test, (url_X_test.Length + 1));
                        url_X_test[url_X_test.Length - 1] = new_url_item_;
                    }
                }
                tab1_textBox1.Text = string.Join(CrLf, url_X_test);
            }
        }

        public static void set_all_False(ref bool[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = false;
            }
        }

        public static void set_all_Zero(ref int[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = 0;
            }
        }

        public static void set_all_Zero(ref long[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = 0;
            }
        }

        public byte check_version(string version_from_server) {
            version_from_server = version_from_server.Trim(' ').Trim('\n').Trim('\r');
            string[] version_from_server_array = fast_split(version_from_server, ".");
            if (version_from_server_array.Length != 4) {
                return 2;
            } else {
                string[] version_now = fast_split(Application.ProductVersion, ".");
                for (int i = 0; i < 4; i++) {
                    try {
                        if (int.Parse(version_from_server_array[i]) > int.Parse(version_now[i])) {
                            return 1;
                        } else if (int.Parse(version_from_server_array[i]) == int.Parse(version_now[i])) {
                        } else {
                            return 0;
                        }
                    } catch {
                        return 2;
                    }
                }
                if (beta_version && version_from_server==Application.ProductVersion.ToString()) {
                    return 1;
                }
                return 0;
            }
        }

        public Icon get_icon_() {
            int exists_app = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count();

            if (exists_app <= 1) {
                return Properties.Resources.EXE;
            } else {
                this_app_exist_number = exists_app;
                this.Text = exists_app + " - " + this.Text;
            }

            Bitmap bmp = Properties.Resources.EXE.ToBitmap();

            RectangleF rectf = new RectangleF(0, 0, 64, 64);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.DrawString(exists_app.ToString(), new Font("Tahoma", 40), Brushes.Cyan, rectf);
            g.Flush();

            var thumb = (Bitmap)bmp.GetThumbnailImage(64, 64, null, IntPtr.Zero);
            thumb.MakeTransparent();
            var icon = Icon.FromHandle(thumb.GetHicon());

            return icon;
        }

        public static Icon get_icon_(Icon icon_X,int exists_app) {
            if (exists_app <= 1) {
                return icon_X;
            }

            Bitmap bmp = icon_X.ToBitmap();

            RectangleF rectf = new RectangleF(0, 0, 64, 64);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.DrawString(exists_app.ToString(), new Font("Tahoma", 40), Brushes.Cyan, rectf);
            g.Flush();

            var thumb = (Bitmap)bmp.GetThumbnailImage(64, 64, null, IntPtr.Zero);
            thumb.MakeTransparent();
            var icon = Icon.FromHandle(thumb.GetHicon());

            return icon;
        }

        public void read_oauth_file() {
            string decode_str = weil_decode_str(System.Text.UTF8Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(Application.StartupPath + "\\userinfo.log")));
            string[] userinfo_array = fast_split(decode_str, "&");
            for (int i = 0; i < userinfo_array.Length; i++) {
                if (userinfo_array[i].StartsWith("fullname=")) {
                    userinfo_fullname = HttpUtility.UrlDecode(userinfo_array[i].Substring(9));
                } else if (userinfo_array[i].StartsWith("oauth_token=")) {
                    oauth_token = userinfo_array[i].Substring(12);
                } else if (userinfo_array[i].StartsWith("oauth_token_secret=")) {
                    oauth_secret = userinfo_array[i].Substring(19);
                } else if (userinfo_array[i].StartsWith("user_nsid=")) {
                    userinfo_nsid = HttpUtility.UrlDecode(userinfo_array[i].Substring(10));
                } else if (userinfo_array[i].StartsWith("username=")) {
                    userinfo_username = HttpUtility.UrlDecode(userinfo_array[i].Substring(9));
                }
            }
            if (oauth_token == "" || oauth_secret == "") {
                append_msg_THREAD("授權有誤！資料錯誤！Error, userinfo.log unknown format!", Color.Red, Color.Yellow);
                return;
            }
            login_test_timer.Start();
        }

        public void test_login() {
            try {
                append_msg_THREAD("登入中...Logging in...", color_f_w, bg_color_exe);
                WebClient_auto wbcl_ = new WebClient_auto();
                string url = "https://api.flickr.com/services/rest/?method=flickr.test.login&api_key=" + api_key + "&format=json";
                int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                url = url.Replace("&api_key=" + api_key, "") + "&nojsoncallback=1&oauth_nonce=" + rand_.Next(111111, 99999999) + "&oauth_consumer_key=" + api_key + "&oauth_timestamp=" + unixTimestamp + "&oauth_signature_method=HMAC-SHA1&oauth_version=1.0&oauth_token=" + oauth_token;
                string oauth_signature = get_sign_hmacsha1(url, api_secret + "&" + oauth_secret);
                url = url + "&oauth_signature=" + url_encode(oauth_signature);
                string response = wbcl_.DownloadString(url);
                JObject jObjxt = (JObject)JsonConvert.DeserializeObject(response);//dynamic
                Debug.WriteLine(response);
                if (jObjxt["stat"].ToString() == "ok") {
                    signed_auth_bool = true;
                    toolStripDropDownButton2.Text = "已授權為" + userinfo_username;
                    toolStripDropDownButton2.ToolTipText = "Authorized as " + userinfo_username;
                    toolStripDropDownButton3.Visible = true;
                    append_msg_THREAD("已登入！Successfully Logged in!", color_f_g, bg_color_exe);
                } else {
                    signed_auth_bool = false;
                    toolStripDropDownButton2.Text = "授權此應用(Auth)";
                    toolStripDropDownButton2.ToolTipText = "Authorize this app.";
                    toolStripDropDownButton3.Visible = false;
                    append_msg_THREAD("登入失敗，請重新授權！Logged in FAIL.\n" + jObjxt["message"].ToString(), Color.Red, Color.Yellow);
                }
            } catch (Exception) {
                signed_auth_bool = false;
                toolStripDropDownButton2.Text = "授權此應用(Auth)";
                toolStripDropDownButton2.ToolTipText = "Authorize this app.";
                toolStripDropDownButton3.Visible = false;
                append_msg_THREAD("登入失敗，請重新授權！Log in FAIL.", Color.Red, Color.Yellow);
            }
        }

        public void get_auth_url() {

            if (download_started) {
                append_msg_THREAD("請先停止下載！Please stop download first!", Color.Red, Color.Yellow);
                return;
            } else {
                download_started = true;
            }

            bool already_login = false;

            if (signed_auth_bool) {
                already_login = true;
            }

            signed_auth_bool = false;
            toolStripDropDownButton2.Text = "授權此應用(Auth)";
            toolStripDropDownButton3.Visible = false;

            string oauth_nonce = rand_.Next(0, int.MaxValue).ToString();
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            
            string versionsx = get_response_from_url_DO_event("http://flickr_downloader.weil.app.wbftw.org/api/flickr/version.txt").Trim(new char[] { (char)10, (char)13, (char)32 });
            if (check_version(versionsx) == 1) {
                MessageBox.Show(this, "版本號不一致！請更新程式，否則可能無法正常使用！\n\n當前版本(Current Version)：" + ProductVersion.ToString() + "\n最低版本(Minimum Version)：" + versionsx, "版本不同 Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            string full_url_to_request_token = "https://www.flickr.com/services/oauth/request_token?oauth_nonce=" + oauth_nonce + "&oauth_timestamp=" + unixTimestamp.ToString() + "&oauth_consumer_key=" + api_key + "&oauth_signature_method=HMAC-SHA1&oauth_version=1.0&oauth_callback=" + url_encode("https://weils.net/api/flickr/redirect_only.php");
            string oauth_signature = get_sign_hmacsha1(full_url_to_request_token, api_secret + "&");
            string full_REQ_URL = full_url_to_request_token + "&oauth_signature=" + url_encode(oauth_signature);

            string str_response = get_response_from_url_DO_event(full_REQ_URL);

            if (str_response.IndexOf("oauth_callback_confirmed=true") >= 0) {
                append_msg_THREAD("取得OAuth回應！Successfully get OAuth Response.", color_f_g, bg_color_exe);
                string[] temp_str_array = fast_split(str_response, "&");
                for (int i = 0; i < temp_str_array.Length; i++) {
                    if (temp_str_array[i].StartsWith("oauth_token=")) {
                        oauth_token = temp_str_array[i].Substring(12);
                    }
                    if (temp_str_array[i].StartsWith("oauth_token_secret=")) {
                        oauth_secret = temp_str_array[i].Substring(19);
                    }
                }
                if (oauth_token == "" || oauth_secret == "") {
                    append_msg_THREAD("嚴重錯誤！密鑰遺失！Fatal Error!", Color.Red, Color.Yellow);
                    if (already_login) {
                        append_msg_THREAD("重登原本帳號。(Log in as the same account.)", color_f_m, bg_color_exe);
                        read_oauth_file();
                    }
                    download_started = false;
                    return;
                } else {
                    append_msg_THREAD("取得OAuth金鑰！Successfully get OAuth Token and Secret!", color_f_g, bg_color_exe);
                }
            } else {
                append_msg_THREAD("嚴重錯誤！無法取得授權！Fatal Error!", Color.Red, Color.Yellow);
                if (already_login) {
                    append_msg_THREAD("重登原本帳號。(Log in as the same account.)", color_f_m, bg_color_exe);
                    read_oauth_file();
                }
                download_started = false;
                return;
            }

            full_url_to_request_token = "https://www.flickr.com/services/oauth/authorize?perms=read&oauth_token=" + oauth_token;
            oauth_signature = get_sign_hmacsha1(full_url_to_request_token, api_secret + "&" + oauth_secret);
            full_REQ_URL = full_url_to_request_token + "&oauth_signature=" + url_encode(oauth_signature);

            Process.Start(full_REQ_URL);

            string oauth_verifier = "";

            Verify_waiting vw_form = new Verify_waiting();
            vw_form.timer_count = 900;
            vw_form.url_XX = full_REQ_URL;
            vw_form.ShowDialog();

            if (vw_form.cancel_auth) {
                append_msg_THREAD("已取消！Canceled!", color_f_m, bg_color_exe);
                if (already_login) {
                    append_msg_THREAD("重登原本帳號。(Log in as the same account.)", color_f_m, bg_color_exe);
                    read_oauth_file();
                }
                download_started = false;
                return;
            } else {
                oauth_verifier = vw_form.oauth_verifier;
            }

            try {
                vw_form.listen_service.Abort();
            } catch {

            }

            if (oauth_verifier=="") {
                append_msg_THREAD("嚴重錯誤！無法取得授權！Fatal Error!", Color.Red, Color.Yellow);
                if (already_login) {
                    append_msg_THREAD("重登原本帳號。(Log in as the same account.)", color_f_m, bg_color_exe);
                    read_oauth_file();
                }
                download_started = false;
                return;
            } else {
                append_msg_THREAD("取得OAuth驗證！Successfully get OAuth verifier!", color_f_g, bg_color_exe);
            }

            oauth_nonce = rand_.Next(0, int.MaxValue).ToString();
            unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            full_url_to_request_token = "https://www.flickr.com/services/oauth/access_token?oauth_nonce=" + oauth_nonce + "&oauth_timestamp=" + unixTimestamp.ToString() + "&oauth_verifier=" + oauth_verifier + "&oauth_consumer_key=" + api_key + "&oauth_signature_method=HMAC-SHA1&oauth_version=1.0&oauth_token=" + oauth_token;
            oauth_signature = get_sign_hmacsha1(full_url_to_request_token, api_secret + "&" + oauth_secret);
            full_REQ_URL = full_url_to_request_token + "&oauth_signature=" + url_encode(oauth_signature);

            str_response = get_response_from_url_DO_event(full_REQ_URL);

            if (str_response.IndexOf("user_nsid=") >= 0) {
                append_msg_THREAD("取得OAuth交換！Successfully get OAuth Access!", color_f_g, bg_color_exe);
                Debug.WriteLine(str_response);
                try {
                    System.IO.File.WriteAllBytes(Application.StartupPath + "\\userinfo.log", System.Text.UTF8Encoding.UTF8.GetBytes(weil_encode_str(str_response)));
                    append_msg_THREAD("已儲存！Successfully saved!", color_f_g, bg_color_exe);
                    read_oauth_file();
                } catch {
                    append_msg_THREAD("儲存登入資訊失敗！Saving File Error!", Color.Red, Color.Yellow);
                    if (already_login) {
                        append_msg_THREAD("重登原本帳號。(Log in as the same account.)", color_f_m, bg_color_exe);
                        read_oauth_file();
                    }
                }
            } else {
                append_msg_THREAD("嚴重錯誤！無法取得交換！Fatal Error!", Color.Red, Color.Yellow);
                if (already_login) {
                    append_msg_THREAD("重登原本帳號。(Log in as the same account.)", color_f_m, bg_color_exe);
                    read_oauth_file();
                }
                download_started = false;
                return;
            }

            download_started = false;

        }

        public string md5_en(string str) {
            MD5 md5Hasher = MD5.Create();
            Byte[] data = { };
            data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().ToLower();
        }

        public static string hmac_sha1_en(string input, string key) {
            HMACSHA1 myhmacsha1 = new HMACSHA1(System.Text.UTF8Encoding.UTF8.GetBytes(key));
            byte[] byteArray = System.Text.UTF8Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            return Convert.ToBase64String(myhmacsha1.ComputeHash(stream));
        }

        public static string weil_decode_str(string str) {
            if (str == "") {
                return "";
            }
            string[] str_import = System.Text.RegularExpressions.Regex.Split(str, System.Text.RegularExpressions.Regex.Escape(":"));
            byte[] str_byte = new byte[str_import.Length - 4];
            int i4 = int.Parse(weil_90_code_de(str_import[str_import.Length - 4], 90));
            int i3 = int.Parse(weil_90_code_de(str_import[str_import.Length - 3], 90));
            int i2 = int.Parse(weil_90_code_de(str_import[str_import.Length - 2], 90));
            int i1 = int.Parse(weil_90_code_de(str_import[str_import.Length - 1], 90));
            for (int i = 0; i < (str_byte.Length); i++) {
                short pw_x = (byte)((hash_byte[i4] * 7 + hash_byte[i3] * 5 + hash_byte[i2] * 3 + hash_byte[i1]) % 256);
                str_byte[i] = (byte)(((int.Parse(weil_90_code_de(str_import[i], 90))) + 256 - pw_x) % 256);
                i4 += 1;
                if (i4 >= hash_byte.Length) {
                    i4 = 0;
                    i3 += 1;
                    if (i3 >= hash_byte.Length) {
                        i3 = 0;
                        i2 += 1;
                        if (i2 >= hash_byte.Length) {
                            i2 = 0;
                            i1 += 1;
                            if (i1 >= hash_byte.Length) {
                                i4 = 0;
                                i3 = 0;
                                i2 = 0;
                                i1 = 0;
                            }
                        }
                    }
                }
            }
            return System.Text.UTF8Encoding.UTF8.GetString(str_byte);
        }

        public int rand(int min, int max) {
            return (int)(rand_.NextDouble() * (max - min + 1) + min);
        }

        public string weil_90_code_en(int ax) {
            BigInteger a = new BigInteger(ax);
            string str_table = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ+/=-*~!@#$%^&()_{}<>[]'`,.?|";
            Byte n = 90;
            int xa = 20;
            if (a.ToString() == "0") {
                xa = 0;
            } else {
                xa = (int)(Math.Floor(BigInteger.Log(a, n)));
            }
            BigInteger[] b = new BigInteger[xa + 1];
            for (int i = 0; i <= xa; i++) {
                BigInteger.DivRem(a, n, out b[i]);
                a = (BigInteger.Divide(a, n));
                if (a.ToString() == "0") {
                    break;
                }
            }

            string[] x = new string[xa + 1];

            for (int i = 0; i <= xa; i++) {
                x[i] = str_table[int.Parse(b[i].ToString())].ToString();
            }

            string c = "";

            for (int i = 0; i < x.Length; i++) {
                c = x[i].ToString() + c;
            }

            return c;

        }

        public string weil_encode_str(string str) {
            byte[] str_byte = System.Text.UTF8Encoding.UTF8.GetBytes(str);
            string[] str_return = new string[str_byte.Length];
            int i4 = rand(0, hash_byte.Length - 1);
            int i3 = rand(0, hash_byte.Length - 1);
            int i2 = rand(0, hash_byte.Length - 1);
            int i1 = rand(0, hash_byte.Length - 1);
            string tag_X = weil_90_code_en(i4) + ":" + weil_90_code_en(i3) + ":" + weil_90_code_en(i2) + ":" + weil_90_code_en(i1);
            for (int i = 0; i < str_byte.Length; i++) {
                byte pw_x = (byte)((hash_byte[i4] * 7 + hash_byte[i3] * 5 + hash_byte[i2] * 3 + hash_byte[i1]) % 256);
                str_return[i] = weil_90_code_en((pw_x + str_byte[i]) % 256);
                i4 += 1;
                if (i4 >= hash_byte.Length) {
                    i4 = 0;
                    i3 += 1;
                    if (i3 >= hash_byte.Length) {
                        i3 = 0;
                        i2 += 1;
                        if (i2 >= hash_byte.Length) {
                            i2 = 0;
                            i1 += 1;
                            if (i1 >= hash_byte.Length) {
                                i4 = 0;
                                i3 = 0;
                                i2 = 0;
                                i1 = 0;
                            }
                        }
                    }
                }
            }
            return String.Join(":", str_return) + ":" + tag_X;
        }

        public static byte[] creat_hash(byte[] pw) {
            if (pw.Length == 0) {
                return new byte[0];
            }
            int d = 0;
            int d_L = pw.Length;

            byte[] return_byte = { };

            byte p = pw[0];
            byte start_ = pw[0];

            int i = 0;

            while (true) {
                p = (byte)((p + pw[d]) % 251);
                Array.Resize(ref return_byte, return_byte.Length + 1);
                return_byte[return_byte.Length - 1] = p;
                if (d == 0 && p == start_ || i >= 10000) {
                    break;
                } else {
                    d += 1;
                    i += 1;
                    if (d == d_L) {
                        d = 0;
                    }
                }
            }

            i = 0;

            while (true) {
                p = (byte)(((p + 1) * (pw[d] + 1)) % 251);
                Array.Resize(ref return_byte, return_byte.Length + 1);
                return_byte[return_byte.Length - 1] = p;
                if ((d == 0 && p == start_) || i >= 10000) {
                    break;
                } else {
                    d += 1;
                    i += 1;
                    if (d == d_L) {
                        d = 0;
                    }
                }
            }

            return return_byte;

        }

        public static string weil_90_code_de(string str, int v) {
            string password90_str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ+/=-*~!@#$%^&()_{}<>[]'`,.?|";
            BigInteger g = new BigInteger(-1);
            BigInteger n = new BigInteger(v);
            int k = str.Length;
            List<string> m = new List<string>();
            for (int i = 0; i < k; i++) {
                char x = str[i];
                for (int ix = 0; ix < 90; ix++) {
                    if (x == password90_str[ix]) {
                        m.Add(ix.ToString());
                        if (ix >= v) {
                            return ":ERROR:";
                        }
                        break;
                    }
                }
            }
            v = k - 1;
            List<string> c = new List<string>();
            string[] m1 = new string[m.Count];
            m1 = m.ToArray();
            for (int i = 0; i < k; i++) {
                c.Add(BigInteger.Multiply(BigInteger.Pow(n, (v)), (BigInteger.Parse(m1[i]))).ToString());
                v = v - 1;
            }
            BigInteger t = new BigInteger(0);
            string[] c1 = new string[c.Count];
            c1 = c.ToArray();
            for (int i = 0; i < k; i++) {
                BigInteger ci = BigInteger.Parse(c1[i]);
                t = BigInteger.Add(ci, t);
            }
            return t.ToString();
        }

        /*
         * 
         *00 - exe_bg_color
         *01 - exe_f_color
         *02 - exe_button_color
         *03 - types_
         *04 - options_1_orginal_names
         *05 - options_2_auto_make_dirs
         *06 - options_3_preview
         *07 - "0"
         *08 - exe_allow_exit
         *09 - this.Width
         *10 - this.Height
         *11 - options_4_size
         *12 - options_5_thread_num
         *13 - exe_sound_enabled
         *14 - exe_clipboard_mon_enabled
         *15 - exe_remember_save_path_enabled
         *16 - exe_remember_save_path
         *17 - options_6_skip_download
         *18 - exe_show_detail_enabled
         *19 - search_PerPage
         *20 - search_StartPage
         *21 - search_FetchPage
         *22 - options_7_time_format
         *23 - options_8_video_download
         * 
         */

        public void write_config() {
            System.IO.File.WriteAllText(Application.StartupPath + "\\config.ini", exe_bg_color + CrLf + exe_f_color + CrLf + exe_button_color + CrLf + types_ + CrLf + (options_1_orginal_names) + CrLf + bool_to_str(options_2_auto_make_dirs) + CrLf + bool_to_str(options_3_preview) + CrLf + "0" + CrLf + exe_allow_exit + CrLf + this.Width + CrLf + this.Height + CrLf + options_4_size + CrLf + options_5_thread_num + CrLf + bool_to_str(exe_sound_enabled) + CrLf + bool_to_str(exe_clipboard_mon_enabled) + CrLf + bool_to_str(exe_remember_save_path_enabled) + CrLf + exe_remember_save_path + CrLf + bool_to_str(options_6_skip_download) + CrLf + bool_to_str(exe_show_detail_enabled) + CrLf + search_perpage + CrLf + search_startpage + CrLf + search_fetchpage + CrLf + options_7_time_format + CrLf + bool_to_str(options_8_video_download));
        }

        public void read_config() {
            if (System.IO.File.Exists(Application.StartupPath + "\\config.ini")) {
                try {
                    string[] config_ini = fast_split(System.Text.UTF8Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(Application.StartupPath + "\\config.ini")), CrLf);
                    exe_bg_color = safe_int_parse(config_ini[0]);
                    exe_f_color = safe_int_parse(config_ini[1]);
                    exe_button_color = safe_int_parse(config_ini[2]);
                    types_ = safe_int_parse(config_ini[3]);
                    options_1_orginal_names = get_options_by_str_special(config_ini[4]);
                    options_2_auto_make_dirs = str_to_bo(config_ini[5]);
                    options_3_preview = str_to_bo(config_ini[6]);
                    try {
                        options_4_size = int.Parse(config_ini[11]);
                    } catch {
                        options_4_size = 0;
                    }
                    try {
                        options_5_thread_num = int.Parse(config_ini[12]);
                    } catch {
                        options_5_thread_num = 4;
                    }
                    exe_allow_exit = safe_int_parse(config_ini[8]);
                    exe_width = safe_int_parse(config_ini[9]);
                    exe_height = safe_int_parse(config_ini[10]);
                    try {
                        exe_sound_enabled = str_to_bo(config_ini[13]);
                    } catch {

                    }
                    try {
                        exe_clipboard_mon_enabled = str_to_bo(config_ini[14]);
                    } catch {

                    }
                    try {
                        exe_remember_save_path_enabled = str_to_bo(config_ini[15]);
                        exe_remember_save_path = config_ini[16];
                        options_6_skip_download = str_to_bo(config_ini[17]);
                    } catch {

                    }
                    try {
                        exe_show_detail_enabled = str_to_bo(config_ini[18]);
                    } catch {

                    }
                    try {
                        search_perpage = safe_int_parse(config_ini[19]);
                        search_startpage = safe_int_parse(config_ini[20]);
                        search_fetchpage = safe_int_parse(config_ini[21]);
                        if (search_perpage<1 || search_perpage>500) {
                            search_perpage = 1;
                        }
                        if (search_startpage < 1) {
                            search_perpage = 1;
                        }
                        if (search_fetchpage < 1) {
                            search_perpage = 1;
                        }
                    } catch {

                    }
                    try {
                        options_7_time_format = config_ini[22];
                    } catch {

                    }
                    try {
                        options_8_video_download = str_to_bo(config_ini[23]);
                    } catch {
                        options_8_video_download = false;
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(ex.ToString());
                }
            } else {
                options_4_size = 0;
                options_5_thread_num = 4;
            }
        }

        public static int return_difference(Color color1,Color color2) {
            int r_x = Math.Abs(color1.R-color2.R);
            int g_x = Math.Abs(color1.G - color2.G);
            int b_x = Math.Abs(color1.B- color2.B);
            return (r_x + g_x + b_x);
        }

        private void tabControl1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e) {
            TabPage CurrentTab = tabControl1.TabPages[e.Index];
            Rectangle ItemRect = tabControl1.GetTabRect(e.Index);
            SolidBrush FillBrush = new SolidBrush(Color.FromArgb(exe_bg_color));
            SolidBrush TextBrush = new SolidBrush(Color.FromArgb(exe_f_color));
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            //If we are currently painting the Selected TabItem we'll
            //change the brush colors and inflate the rectangle.
            if (System.Convert.ToBoolean(e.State & DrawItemState.Selected)) {
                FillBrush.Color = Color.FromArgb(exe_bg_color);
                TextBrush.Color = Color.FromArgb(exe_f_color);
                ItemRect.Inflate(2, 2);
            }

            //Set up rotation for left and right aligned tabs
            if (tabControl1.Alignment == TabAlignment.Left || tabControl1.Alignment == TabAlignment.Right) {
                float RotateAngle = 90;
                if (tabControl1.Alignment == TabAlignment.Left)
                    RotateAngle = 270;
                PointF cp = new PointF(ItemRect.Left + (ItemRect.Width / 2), ItemRect.Top + (ItemRect.Height / 2));
                e.Graphics.TranslateTransform(cp.X, cp.Y);
                e.Graphics.RotateTransform(RotateAngle);
                ItemRect = new Rectangle(-(ItemRect.Height / 2), -(ItemRect.Width / 2), ItemRect.Height, ItemRect.Width);
            }

            //Next we'll paint the TabItem with our Fill Brush
            e.Graphics.FillRectangle(FillBrush, ItemRect);

            //Now draw the text.
            e.Graphics.DrawString(CurrentTab.Text, e.Font, TextBrush, (RectangleF)ItemRect, sf);

            //Reset any Graphics rotation
            e.Graphics.ResetTransform();

            //Finally, we should Dispose of our brushes.
            FillBrush.Dispose();
            TextBrush.Dispose();
        }

        public void set_from_config() {
            try {
                video_download_setting_obj.Tag = "s";
                if (return_difference(Color.White,Color.FromArgb(exe_bg_color)) <= 64) {
                    color_f_w = Color.FromArgb(exe_f_color);
                } else if (return_difference(Color.Lime, Color.FromArgb(exe_bg_color)) <= 64) {
                    color_f_g = Color.Yellow;
                } else if (return_difference(Color.Magenta, Color.FromArgb(exe_bg_color)) <= 64) {
                    color_f_m = Color.Yellow;
                }
                if (return_difference(Color.Red, Color.FromArgb(exe_bg_color)) <= 64) {
                    version_name.ForeColor = Color.Blue;
                    toolStripStatusLabel1.LinkColor = Color.Blue;
                    toolStripStatusLabel2.LinkColor = Color.Blue;
                    toolStripStatusLabel3.LinkColor = Color.Blue;
                } else {
                    version_name.ForeColor = Color.Red;
                    toolStripStatusLabel1.LinkColor = Color.Red;
                    toolStripStatusLabel2.LinkColor = Color.Red;
                    toolStripStatusLabel3.LinkColor = Color.Red;
                }
                this.BackColor = Color.FromArgb(exe_bg_color);
                tableLayoutPanel1.BackColor = Color.FromArgb(exe_bg_color);
                tableLayoutPanel2.BackColor = Color.FromArgb(exe_bg_color);
                tableLayoutPanel3.BackColor = Color.FromArgb(exe_bg_color);
                tableLayoutPanel4.BackColor = Color.FromArgb(exe_bg_color);
                tableLayoutPanel5.BackColor = Color.FromArgb(exe_bg_color);
                tableLayoutPanel6.BackColor = Color.FromArgb(exe_bg_color);
                tableLayoutPanel7.BackColor = Color.FromArgb(exe_bg_color);
                tableLayoutPanel8.BackColor = Color.FromArgb(exe_bg_color);
                tabPage1.BackColor = Color.FromArgb(exe_bg_color);
                tabPage2.BackColor = Color.FromArgb(exe_bg_color);
                tab1_textBox1.BackColor = Color.FromArgb(exe_bg_color);
                tab1_textBox1.ForeColor = Color.FromArgb(exe_f_color);
                richTextBox1.BackColor = Color.FromArgb(exe_bg_color);
                tab2_search_userid_textbox.BackColor = Color.FromArgb(exe_bg_color);
                tab2_search_groupid_textbox.BackColor = Color.FromArgb(exe_bg_color);
                tab2_search_textbox.BackColor = Color.FromArgb(exe_bg_color);
                tab2_userid_selectbox_type.BackColor = Color.FromArgb(exe_bg_color);
                tab2_userid_selectbox_privacyfilter.BackColor = Color.FromArgb(exe_bg_color);
                tab2_safesearch_selectbox.BackColor = Color.FromArgb(exe_bg_color);
                tab2_search_per_page_obj.BackColor = Color.FromArgb(exe_bg_color);
                tab2_search_start_page_obj.BackColor = Color.FromArgb(exe_bg_color);
                tab2_search_fetch_page_obj.BackColor = Color.FromArgb(exe_bg_color);
                photos_sizes_setting_obj.BackColor = Color.FromArgb(exe_bg_color);
                file_name_setting_obj.BackColor = Color.FromArgb(exe_bg_color);
                bg_color_exe = Color.FromArgb(exe_bg_color);
                this.ForeColor = Color.FromArgb(exe_f_color);
                photos_sizes_setting_obj.ForeColor = Color.FromArgb(exe_f_color);
                file_name_setting_obj.ForeColor = Color.FromArgb(exe_f_color);
                tab2_search_userid_textbox.ForeColor = Color.FromArgb(exe_f_color);
                tab2_search_groupid_textbox.ForeColor = Color.FromArgb(exe_f_color);
                tab2_search_textbox.ForeColor = Color.FromArgb(exe_f_color);
                tab2_userid_selectbox_type.ForeColor = Color.FromArgb(exe_f_color);
                tab2_userid_selectbox_privacyfilter.ForeColor = Color.FromArgb(exe_f_color);
                tab2_safesearch_selectbox.ForeColor = Color.FromArgb(exe_f_color);
                tab2_search_per_page_obj.ForeColor = Color.FromArgb(exe_f_color);
                tab2_search_start_page_obj.ForeColor = Color.FromArgb(exe_f_color);
                tab2_search_fetch_page_obj.ForeColor = Color.FromArgb(exe_f_color);
                button1.ForeColor = Color.FromArgb(exe_button_color);
                button2.ForeColor = Color.FromArgb(exe_button_color);
                button3.ForeColor = Color.FromArgb(exe_button_color);
                button4.ForeColor = Color.FromArgb(exe_button_color);
                switch (types_) {
                    case 0:
                        tab1_radioButton1.Checked = true;
                        break;
                    case 1:
                        tab1_radioButton2.Checked = true;
                        break;
                    case 2:
                        tab1_radioButton3.Checked = true;
                        break;
                    case 3:
                        tab1_radioButton4.Checked = true;
                        break;
                    case 4:
                        tab1_radioButton5.Checked = true;
                        break;
                    case 5:
                        tab1_radioButton6.Checked = true;
                        break;
                    case 6:
                        tab1_radioButton7.Checked = true;
                        break;
                    case 7:
                        tab1_radioButton8.Checked = true;
                        break;
                    default:
                        tab1_radioButton1.Checked = true;
                        break;
                }
                file_name_setting_obj.SelectedIndex = options_1_orginal_names;
                auto_making_dir_setting_obj.Checked = options_2_auto_make_dirs;
                //preview_photos_setting_obj.Checked = options_3_preview;
                skip_download_setting_obj.Checked = options_6_skip_download;
                video_download_setting_obj.Checked = options_8_video_download;
                photos_sizes_setting_obj.SelectedIndex = options_4_size;
                toolStripComboBox1.SelectedIndex = (options_5_thread_num-1);
                tab2_search_per_page_obj.Value = search_perpage;
                tab2_search_start_page_obj.Value = search_startpage;
                tab2_search_fetch_page_obj.Value = search_fetchpage;
                if (exe_width >= 900 && exe_height >= 640) {
                    this.Size = new Size(exe_width, exe_height);
                }
                if (exe_allow_exit == 1) {
                    直接關閉未啟用ToolStripMenuItem.Text = "直接關閉=啟用";
                } else {
                    直接關閉未啟用ToolStripMenuItem.Text = "直接關閉=未啟用";
                }
                if (exe_sound_enabled) {
                    音效開關開ToolStripMenuItem.Text = "音效開關=開On";
                } else {
                    音效開關開ToolStripMenuItem.Text = "音效開關=關Off";
                }
                if (exe_clipboard_mon_enabled) {
                    剪貼簿偵測ToolStripMenu.Text = "剪貼簿偵測=開On";
                } else {
                    剪貼簿偵測ToolStripMenu.Text = "剪貼簿偵測=關Off";
                }
                if (exe_remember_save_path_enabled) {
                    記住存檔位置toolStripMenuItem.Text = "記住存檔位置=開On";
                } else {
                    記住存檔位置toolStripMenuItem.Text = "記住存檔位置=關Off";
                }
                if (exe_show_detail_enabled) {
                    Skip_append_text_toolStripMenuItem.Text = "詳細顯示所有資訊=開On";
                } else {
                    Skip_append_text_toolStripMenuItem.Text = "詳細顯示所有資訊=關Off";
                }
                try {
                    toolStripTextBox1.Text = options_7_time_format;
                } catch {

                }
                video_download_setting_obj.Tag = "";
            } catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
            }
        }

        public int select_if_new_save_i(int i_number) {
            //1最外2中
            if (read_from_save_file) {
                if (i_number == 1) {
                    return save_v3;
                }
                if (i_number == 2) {
                    return save_v2;
                }
            }
            return 0;
        }

        private void Form1_Load(object sender, EventArgs e) {
            richTextBox1.Clear();
            richTextBox1.Text = "This app is developed by Weil Jimmer @ White Birch Forum Team (c) \nhttps://weils.net/\n\n";
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Select(0, 0);
            toolTip1.SetToolTip(tab1_radioButton1, "Example URL:https://www.flickr.com/photos/keyword/(sets|albums)/12345678901234567、https://flic.kr/s/aaaaaaaaaa、12345678901234567");
            toolTip1.SetToolTip(tab1_radioButton2, "Example URL:https://www.flickr.com/photos/keyword/1234567890/、https://flic.kr/p/aaaaaa、12345678901");
            toolTip1.SetToolTip(tab1_radioButton3, "Example URL:https://www.flickr.com/people/keyword/、https://www.flickr.com/photos/keyword/*、1234567@N00、Username");
            toolTip1.SetToolTip(tab1_radioButton4, "Example URL:https://www.flickr.com/people/keyword/、https://www.flickr.com/photos/keyword/*、1234567@N00、Username");
            toolTip1.SetToolTip(tab1_radioButton5, "Example URL:https://www.flickr.com/groups/1234567@N00/、https://www.flickr.com/groups/keyword/、https://flic.kr/g/aaaaa、1234567@N00、Groupname");
            toolTip1.SetToolTip(tab1_radioButton6, "Example URL:https://www.flickr.com/people/keyword/、https://www.flickr.com/photos/keyword/*、1234567@N00、Username");
            toolTip1.SetToolTip(tab1_radioButton7, "Example URL:https://www.flickr.com/photos/keyword/(sets|albums)/12345678901234567、https://flic.kr/s/aaaaaaaaaa、12345678901234567");
            toolTip1.SetToolTip(tab1_radioButton8, "Example URL:https://www.flickr.com/people/keyword/、https://www.flickr.com/photos/keyword/*、1234567@N00、Username");
            version_name.Text = "版本號：" + ProductVersion;
            if (beta_version) {
                version_name.Text = version_name.Text + " beta";
            } else {
                System.Threading.Thread s = new System.Threading.Thread(() => silent_check_and_download());
                s.Start();
            }
            tab2_userid_selectbox_type.SelectedIndex = 0;
            tab2_userid_selectbox_privacyfilter.SelectedIndex = 0;
            tab2_safesearch_selectbox.SelectedIndex = 0;
            //register_Protocol();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            if (tab1_radioButton1.Checked) {
                types_ = 0;
            } else if (tab1_radioButton2.Checked) {
                types_ = 1;
            } else if (tab1_radioButton3.Checked) {
                types_ = 2;
            } else if (tab1_radioButton4.Checked) {
                types_ = 3;
            } else if (tab1_radioButton5.Checked) {
                types_ = 4;
            } else if (tab1_radioButton6.Checked) {
                types_ = 5;
            } else if (tab1_radioButton7.Checked) {
                types_ = 6;
            } else {
                types_ = 7;
            }
            write_config();
        }

        private void filename_options_CheckedChanged(object sender, EventArgs e) {
            options_1_orginal_names = file_name_setting_obj.SelectedIndex;
            write_config();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            options_2_auto_make_dirs = auto_making_dir_setting_obj.Checked;
            write_config();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            if (options_3_preview && !preview_photos_setting_obj.Checked) {
                setPictureImage(pictureBox1, null);
            }
            options_3_preview = preview_photos_setting_obj.Checked;
            write_config();
        }

        public void msgbox_error(string title, string context) {
            try {
                MessageBox.Show(this, context, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } catch {

            }
        }

        public static void setPictureImage(PictureBox pictureBox, Image img) {
            Action append = () => set_IMG_X(pictureBox,img);
            if (pictureBox.InvokeRequired) {
                pictureBox.Invoke(append);
            } else {
                append();
            }
        }

        public static void set_IMG_X(PictureBox pictureBox, Image img) {
            pictureBox.Image = img;
        }

        public static void setTextbox(TextBox textBox, string str) {
            Action append = () => setTextbox_X(textBox, str);
            if (textBox.InvokeRequired) {
                textBox.Invoke(append);
            } else {
                append();
            }
        }

        public static void setTextbox_X(TextBox textBox, string str) {
            textBox.Text = str;
        }
        
        public static void setStatus_icon(Form parent_X,NotifyIcon notify_X, bool working_X, int this_app_number) {
            Action append = () => setStatus_icon_X(parent_X,notify_X, working_X, this_app_number);
            if (parent_X.InvokeRequired) {
                parent_X.Invoke(append);
            } else {
                append();
            }
        }

        public static void setStatus_icon_X(Form parent_X, NotifyIcon notify_X, bool working_X, int this_app_number) {
            if (working_X) {
                notify_X.Icon = get_icon_(Properties.Resources.EXE2, this_app_number);
                parent_X.Icon = notify_X.Icon;
            } else {
                notify_X.Icon = get_icon_(Properties.Resources.EXE, this_app_number);
                parent_X.Icon = notify_X.Icon;
            }
        }

        public static void setButtonEnabled(Button buttonx, bool boolx) {
            Action append = () => setButtonEnabled_X(buttonx, boolx);
            if (buttonx.InvokeRequired) {
                buttonx.Invoke(append);
            } else {
                append();
            }
        }

        public static void setButtonEnabled_X(Button buttonx, bool boolx) {
            buttonx.Enabled = boolx;
        }
        
        public static void setCheckboxEnabled(CheckBox buttonx, bool boolx) {
            Action append = () => setCheckboxEnabled_X(buttonx, boolx);
            if (buttonx.InvokeRequired) {
                buttonx.Invoke(append);
            } else {
                append();
            }
        }

        public static void setCheckboxEnabled_X(CheckBox buttonx, bool boolx) {
            buttonx.Enabled = boolx;
        }

        public static void setNumericUpDownEnabled(NumericUpDown buttonx, bool boolx) {
            Action append = () => setNumericUpDownEnabled_X(buttonx, boolx);
            if (buttonx.InvokeRequired) {
                buttonx.Invoke(append);
            } else {
                append();
            }
        }

        public static void setNumericUpDownEnabled_X(NumericUpDown buttonx, bool boolx) {
            buttonx.Enabled = boolx;
        }

        public static void setRadioboxEnabled(RadioButton buttonx, bool boolx) {
            Action append = () => setRadioboxEnabled_X(buttonx, boolx);
            if (buttonx.InvokeRequired) {
                buttonx.Invoke(append);
            } else {
                append();
            }
        }

        public static void setRadioboxEnabled_X(RadioButton buttonx, bool boolx) {
            buttonx.Enabled = boolx;
        }

        public static void setComboBoxEnabled(ComboBox buttonx, bool boolx) {
            Action append = () => setComboBoxEnabled_X(buttonx, boolx);
            if (buttonx.InvokeRequired) {
                buttonx.Invoke(append);
            } else {
                append();
            }
        }

        public static void setComboBoxEnabled_X(ComboBox buttonx, bool boolx) {
            buttonx.Enabled = boolx;
        }

        public static void setToolStripSpringTextBoxEnabled(ToolStripSpringTextBox buttonx, bool boolx) {
            Action append = () => setToolStripSpringTextBoxEnabled_X(buttonx, boolx);
            if (buttonx.GetCurrentParent().InvokeRequired) {
                buttonx.GetCurrentParent().Invoke(append);
            } else {
                append();
            }
        }

        public static void setToolStripSpringTextBoxEnabled_X(ToolStripSpringTextBox buttonx, bool boolx) {
            buttonx.Enabled = boolx;
        }

        public static void setButtonText(Button buttonx, string str) {
            Action append = () => setButtonText_X(buttonx, str);
            if (buttonx.InvokeRequired) {
                buttonx.Invoke(append);
            } else {
                append();
            }
        }

        public static void setButtonText_X(Button buttonx, string str) {
            buttonx.Text = str;
        }

        public static void setTextboxReadonly(TextBox textbox, bool boolx) {
            Action append = () => setTextboxReadonly_X(textbox, boolx);
            if (textbox.InvokeRequired) {
                textbox.Invoke(append);
            } else {
                append();
            }
        }

        public static void setTextboxReadonly_X(TextBox textbox, bool boolx) {
            textbox.ReadOnly = boolx;
        }

        public static void setToolStripProgressBar_max(ToolStripProgressBar progressBar, int max) {
            Action append = () => setToolStripProgressBar_max_X(progressBar, max);
            if (progressBar.GetCurrentParent().InvokeRequired) {
                progressBar.GetCurrentParent().Invoke(append);
            } else {
                append();
            }
        }

        public static void setToolStripProgressBar_max_X(ToolStripProgressBar progressBar, int max) {
            try {
                progressBar.Maximum = max;
            } catch {
                Debug.WriteLine("DEBUG: ProgressBar Max ERROR!!!");
            }
        }

        public static void setToolStripProgressBar_value(ToolStripProgressBar progressBar, int value) {
            Action append = () => setToolStripProgressBar_value_X(progressBar, value);
            if (progressBar.GetCurrentParent().InvokeRequired) {
                progressBar.GetCurrentParent().Invoke(append);
            } else {
                append();
            }
        }

        public static void setToolStripProgressBar_value_X(ToolStripProgressBar progressBar, int value) {
            if (progressBar.Maximum < value) {
                return;
            }
            progressBar.Value = value;
        }

        public static void setToolStripProgressBar_tooltiptext(ToolStripProgressBar progressBar, string str) {
            Action append = () => setToolStripProgressBar_tooltiptext_X(progressBar, str);
            if (progressBar.GetCurrentParent().InvokeRequired) {
                progressBar.GetCurrentParent().Invoke(append);
            } else {
                append();
            }
        }

        public static void setToolStripProgressBar_tooltiptext_X(ToolStripProgressBar progressBar, string str) {
            progressBar.ToolTipText = str;
        }

        public static void settextbox_action(TextBox textbox) {
            Action append = () => settextbox_action_X(textbox);
            if (textbox.InvokeRequired) {
                textbox.Invoke(append);
            } else {
                append();
            }
        }

        public static void settextbox_action_X(TextBox textbox) {
            textbox.Select();
            textbox.Focus();
        }

        public void append_msg_THREAD(string msg, Color f_color, Color bg_color) {
            append_msg_THREAD(richTextBox1, msg, f_color, bg_color);
            //settextbox_action_X(textBox1);
        }

        public static void append_msg_THREAD(RichTextBox richtextBox, string msg, Color f_color, Color bg_color) {
            Action append = () => append_X(richtextBox, msg, f_color, bg_color);
            if (richtextBox.InvokeRequired) { 
                richtextBox.Invoke(append);
            } else{
                append();
            }
        }

        public static void append_X(RichTextBox richtextBox, string msg, Color f_color, Color bg_color) {
            try {
                if (richtextBox.TextLength > 50000) {
                    richtextBox.Clear();
                }
                int s1 = richtextBox.TextLength;
                richtextBox.AppendText(msg + LF);
                int s2 = richtextBox.TextLength;
                richtextBox.SelectionStart = s1;
                richtextBox.SelectionLength = msg.Length;
                richtextBox.SelectionBackColor = bg_color;
                richtextBox.SelectionColor = f_color;
                richtextBox.SelectionStart = s2;
                richtextBox.ScrollToCaret();
            } catch {

            }
        }

        public static string[] fast_split ( string str, string key ) {
            return System.Text.RegularExpressions.Regex.Split(str, System.Text.RegularExpressions.Regex.Escape(key));
        }

        public string base58_decode (string num) {
            string alphabet = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            BigInteger decoded = new BigInteger(0);
            BigInteger multi = new BigInteger(1);
            char digit = '0';
            while ( ( num.Length ) > 0 ) {
                digit = num[( num.Length ) - 1];
                decoded = BigInteger.Add(decoded, BigInteger.Multiply(multi, (alphabet.IndexOf(digit))));
                multi = BigInteger.Multiply(multi, alphabet.Length);
                num = num.Substring(0, num.Length - 1);
            }
            return decoded.ToString();
        }

        public string[] reset_url ( string[] url ) {
            url = remove_empty_array_element(url);
            url = remove_duplicate_array_element(url);
            for (int i=0;i<url.Length;i++ ) {
                int s1 = url[i].IndexOf("?");
                if ( s1 >= 0 ) {
                    url[i]=url[i].Substring(0, s1) + "/";
                }
            }
            return url;
        }

        public string[] remove_empty_array_element (string[] url) {
            return url.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        public string[] remove_duplicate_array_element ( string[] url ) {
            return url.Distinct().ToArray();
        }

        public string[] array_remove ( string[] arrayx, int index ) {
            string[] head_array = arrayx;
            Array.Resize(ref head_array, index);
            string[] tail_array = arrayx;
            Array.Resize(ref tail_array, arrayx.Length);
            Array.Reverse(tail_array);
            Array.Resize(ref tail_array, arrayx.Length - index - 1);
            Array.Reverse(tail_array);

            string[] newarray = head_array;
            Array.Resize(ref newarray, arrayx.Length - 1);

            for ( int i = 0; i < tail_array.Length; i++ ) {
                newarray[i + head_array.Length] = tail_array[i];
            }
            return newarray;
        }

        public void check_url_for_pre_download_step (ref string[] title, ref string[] url, ref int remove_count) {
            append_msg_THREAD("檢查網址中…Checking URL list.", color_f_w, bg_color_exe);
            //Application.DoEvents();
            if (title.Length != url.Length) {
                append_msg_THREAD("嚴重錯誤！Fatal Error!", Color.Red, Color.Yellow);
                title = new string[] { };
                url = new string[] { };
                remove_count = 0;
                return;
            }
            if ( title.Length == 0 || url.Length == 0 ) {
                return;
            }
            int i = 0;
            remove_count = 0;
            while (true) {
                if (download_started==false) {
                    break;
                }
                if ( url[i] == null || url[i] == "" || url[i] == "0" ) {
                    title = array_remove(title, i);
                    url = array_remove(url, i);
                    i -= 1;
                    remove_count += 1;
                } else {
                    int search_target_not = url[i].IndexOf("gne?id=");
                    int search_target = url[i].IndexOf("?");
                    if (search_target>=0 && search_target_not<0){
                        url[i] = url[i].Substring(0,search_target);
                    }
                }
                i += 1;
                if (i>=url.Length) {
                    break;
                }
            }
        }

        public void check_url_for_pre_download_step(ref string[] title, ref string[] url, ref string[] url_img, ref string[] arr1, ref string[] arr2, ref string[] arr3, ref string[] arr4, ref string[] arr5, ref string[] arr6, ref int remove_count) {
            append_msg_THREAD("檢查網址中…Checking URL list.", color_f_w, bg_color_exe);
            //Application.DoEvents();
            if (title.Length != url.Length) {
                append_msg_THREAD("嚴重錯誤！Fatal Error!", Color.Red, Color.Yellow);
                title = new string[] { };
                url = new string[] { };
                url_img = new string[] { };
                arr1 = new string[] { };
                arr2 = new string[] { };
                arr3 = new string[] { };
                remove_count = 0;
                return;
            }
            if (title.Length == 0 || url.Length == 0) {
                return;
            }
            int i = 0;
            remove_count = 0;
            while (true) {
                if (download_started == false) {
                    break;
                }
                if (url[i] == null || url[i] == "" || url[i] == "0") {
                    url = array_remove(url, i);
                    url_img = array_remove(url_img, i);
                    title = array_remove(title, i);
                    arr1 = array_remove(arr1, i);
                    arr2 = array_remove(arr2, i);
                    arr3 = array_remove(arr3, i);
                    arr4 = array_remove(arr4, i);
                    arr5 = array_remove(arr5, i);
                    arr6 = array_remove(arr6, i);
                    i -= 1;
                    remove_count += 1;
                } else {
                    int search_target_not = url[i].IndexOf("gne?id=");
                    int search_target = url[i].IndexOf("?");
                    if (search_target >= 0 && search_target_not<0) {
                        url[i] = url[i].Substring(0, search_target);
                    }
                    if (url_img[i] == null || url_img[i] == "" || url_img[i] == "0") {
                        url_img[i] = url[i];
                    }
                }
                i += 1;
                if (i >= url.Length) {
                    break;
                }
            }
        }

        public string creat_rnd_key () {
            string pw_str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0";
            string str = "";
            for (int i=0;i<30;i++ ) {
                str += pw_str[(rand_.Next(0,62))];
            }
            return str;
        }

        public void pause () {
            append_msg_THREAD("暫停！Pausing...",color_f_m, bg_color_exe);
            //Application.DoEvents();
            while (true) {
                if ( !pause_download || !download_started || form_must_close) {
                    break;
                } else {
                    System.Threading.Thread.Sleep(50);
                    //Application.DoEvents();
                }
            }
        }

        public void pause_in_thread() {
            append_msg_THREAD(richTextBox1,"此線程暫停下載！Pausing...", color_f_m, bg_color_exe);
            //Application.DoEvents();
            while (true) {
                if (!pause_download || !download_started || form_must_close) {
                    break;
                } else {
                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        public int get_max_number(int a,int b) {
            if (a>b) {
                return a;
            } else {
                return b;
            }
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
            lock(_thisLock_get_thead_id) {
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
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight) {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        
        private void SetStatusBottomText(string text) {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.statusStrip1.InvokeRequired) {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetStatusBottomText);
                this.Invoke(d, new object[] { text });
            } else {
                this.bottom_status_prog.Text = text;
            }
        }

        private void SetStatusText(string text) {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.statusStrip1.InvokeRequired) {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetStatusText);
                this.Invoke(d, new object[] { text });
            } else {
                this.status_text.Text = text;
            }
        }

        public static bool test_if_file_exist(string path_ , string album_name_ , string photo_name_ , string ext_name_) {
            if (album_name_ == "") {
                path_ = path_ + "\\";
            } else {
                path_ = path_ + "\\" + album_name_ + "\\";
            }
            if (System.IO.File.Exists(path_ + photo_name_ + ext_name_)) {
                return true;
            }
            return false;
        }

        public void preview_image_thread(string file_path, int index) {
            /*Image temp_img = ScaleImage(new Bitmap(System.Drawing.Image.FromStream(stream)), 950, 420);
            setPictureImage(pictureBox1, temp_img);
            stream.Close();*/
            s_p_running[index] = true;
            try {
                if (!file_path.EndsWith(".mov") && !file_path.EndsWith(".mp4") && !file_path.EndsWith(".avi")) {
                    Image temp_img = ScaleImage(handle_orientation(file_path), 950, 420);
                    setPictureImage(pictureBox1, temp_img);
                }
            } catch {

            }
            s_p_running[index] = false;
        }

        public string find_ext_from_header(string vid_header_value) {
            var regex = new System.Text.RegularExpressions.Regex(@".*?filename=.+?\.([a-zA-z0-9]+).*");
            var match = regex.Match(vid_header_value);
            if (match.Success) {
                return "." + match.Groups[1].Value;
            } else {
                return "";
            }
        }

        public string get_id_by_video_url(string video_url) {
            var regex = new System.Text.RegularExpressions.Regex(@"(https|http):\/\/www\.flickr\.com\/photos\/.+?\/([0-9]+)\/play\/.+");
            var match = regex.Match(video_url);
            if (match.Success) {
                return match.Groups[2].Value;
            } else {
                return "";
            }
        }

        private void download_file_and_save(string url_X_download, int index_thread, string album_name_X, string[] photo_name, bool disable_resolve) {
            Debug.WriteLine("InThread_num" + index_thread + ":" + url_X_download);
            int this_number_ = thread_download_number[index_thread];
            int total_amount = live_X.Length;
            string temp_save_path = "";
            thread_busy[index_thread] = true;
            freeze_time[index_thread] = 0;
            live_X[this_number_] = false;
            download_ok[index_thread] = false;
            bytes[index_thread] = new byte[] { };
            if (options_6_skip_download) {
                if (test_if_file_exist(save_dir, album_name_X, photo_name[this_number_], System.IO.Path.GetExtension(url_X_download))) {
                    append_msg_THREAD("已存在相同檔名檔案，跳過。(The same filename occurred,Skip)(" + (this_number_ + 1) + "/" + total_amount + ")", color_f_m, bg_color_exe);
                    Debug.WriteLine("Exist File, SKIP!!");
                    thread_busy[index_thread] = false;
                    return;
                }
            }
            if (download_started) {
                if (wbcl_[index_thread]!=null && wbcl_[index_thread].IsBusy) {
                    try {
                        wbcl_[index_thread].CancelAsync();
                        wbcl_[index_thread].Dispose();
                        Debug.WriteLine("Success - Force to kill last wbcl" + (index_thread + 1));
                    } catch {
                        Debug.WriteLine("FAIL - Force to kill last wbcl" + (index_thread+1));
                    }
                }
                wbcl_[index_thread] = new WebClient_auto();
                string header_req_X = header_req_USER_SET.ToString();
                set_header(ref wbcl_[index_thread], ref header_req_X, "", index_thread);
                wbcl_[index_thread].DownloadDataCompleted += DownloadDataCallback;
                switch (index_thread) {
                    case 0:
                        Debug.WriteLine("S1");
                        wbcl_[index_thread].DownloadProgressChanged += DownloadProgressChanged;
                        break;
                    case 1:
                        Debug.WriteLine("S2");
                        wbcl_[index_thread].DownloadProgressChanged += DownloadProgressChanged2;
                        break;
                    case 2:
                        Debug.WriteLine("S3");
                        wbcl_[index_thread].DownloadProgressChanged += DownloadProgressChanged3;
                        break;
                    case 3:
                        Debug.WriteLine("S4");
                        wbcl_[index_thread].DownloadProgressChanged += DownloadProgressChanged4;
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
                        if (pause_download) {
                            live_X[this_number_] = false;
                            download_ok[index_thread] = true;
                            freeze_time[index_thread] = 0;
                            speed_kbyte[index_thread] = 1;
                            wbcl_[index_thread].CancelAsync();
                            wbcl_[index_thread].Dispose();
                            Debug.WriteLine("pause.");
                            if (current_downloader_key[index_thread] != "") {
                                dead_or_not.Add(current_downloader_key[index_thread], true);
                            } else {
                                Debug.WriteLine("current key is empty!why?");
                            }
                            break;
                        }
                        if (download_started == false || form_must_close == true) {
                            wbcl_[index_thread].CancelAsync();
                            wbcl_[index_thread].Dispose();
                            break;
                        }
                    }
                } catch {
                    msgbox_error("錯誤 Error", "錯誤 Error！");
                }
            } else {
                return;
            }
            if (live_X[this_number_] == false && pause_download != true) {
                try {
                    wbcl_[index_thread].CancelAsync();
                    switch (index_thread) {
                        case 0:
                            Debug.WriteLine("unset S1");
                            wbcl_[index_thread].DownloadProgressChanged -= DownloadProgressChanged;
                            break;
                        case 1:
                            Debug.WriteLine("unset S2");
                            wbcl_[index_thread].DownloadProgressChanged -= DownloadProgressChanged2;
                            break;
                        case 2:
                            Debug.WriteLine("unset S3");
                            wbcl_[index_thread].DownloadProgressChanged -= DownloadProgressChanged3;
                            break;
                        case 3:
                            Debug.WriteLine("unset S4");
                            wbcl_[index_thread].DownloadProgressChanged -= DownloadProgressChanged4;
                            break;
                    }
                    wbcl_[index_thread].Dispose();
                } catch {

                }
                this.SetStatusText("下載失敗！FAIL(" + (this_number_+1) + "/" + total_amount + ")");
                append_msg_THREAD(this.richTextBox1, "下載失敗(FAIL)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], Color.Red, Color.Yellow);
                if (retry_times[this_number_] < 3) {
                    if (!disable_resolve) {
                        if (wbcl_[index_thread].StatusCode()==HttpStatusCode.NotFound) {
                            if (url_X_download.Contains("/orig/")) {
                                url_X_download = url_X_download.Replace("/orig/", "/hd/");
                                append_msg_THREAD(this.richTextBox1, "404-無原檔-重解析影片網址(Resolve video URL)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_m, bg_color_exe);
                                append_msg_THREAD(this.richTextBox1, url_X_download, color_f_m, bg_color_exe);
                                retry_times[this_number_] -= 1;
                            } else if (url_X_download.Contains("/hd/")) {
                                url_X_download = url_X_download.Replace("/hd/", "/site/");
                                append_msg_THREAD(this.richTextBox1, "404-無HD-重解析影片網址(Resolve video URL)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_m, bg_color_exe);
                                append_msg_THREAD(this.richTextBox1, url_X_download, color_f_m, bg_color_exe);
                                retry_times[this_number_] -= 1;
                            } else if (url_X_download.Contains("/site/")) {
                                url_X_download = "http://www.flickr.com/video_download.gne?id=" + get_id_by_video_url(url_X_download);
                                append_msg_THREAD(this.richTextBox1, "404-無site-重解析影片網址(Resolve video URL)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_m, bg_color_exe);
                                append_msg_THREAD(this.richTextBox1, url_X_download, color_f_m, bg_color_exe);
                                retry_times[this_number_] -= 1;
                            }
                        }
                    }
                    append_msg_THREAD(this.richTextBox1, "重試(Retrying)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_m, bg_color_exe);
                    retry_times[this_number_] += 1;
                    download_file_and_save(url_X_download, index_thread, album_name_X, photo_name, false);
                    return;
                } else {
                    retry_times[this_number_] = 0;
                    this.SetStatusText("下載失敗！Fatal FAIL(" + (this_number_ + 1) + "/" + total_amount + ")\n" + url_X_download);
                    fail_list_name.Add(photo_name[this_number_]);
                    fail_list_url.Add(url_X_download);
                }
            } else if (live_X[this_number_] == false && pause_download) {
                append_msg_THREAD(this.richTextBox1, "暫停下載，取消當前進度，等待重試(Retrying)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_m, bg_color_exe);
                retry_times[this_number_] = 0;
                pause_in_thread();
                append_msg_THREAD(this.richTextBox1, "繼續下載...(Resume Downloading)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_m, bg_color_exe);
                download_file_and_save(url_X_download, index_thread, album_name_X, photo_name, false);
                return;
            } else {
                this.SetStatusText("下載完成Success(" + (this_number_ + 1) + "/" + total_amount + ")");
                //status_text.Text = "下載完成Success(" + (this_number_ + 1) + "/" + total_amount + ")";
                append_msg_THREAD(this.richTextBox1, "下載完成(Success)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_g, bg_color_exe);
                string ext_name = System.IO.Path.GetExtension(url_X_download);
                WebHeaderCollection myWebHeaderCollection = wbcl_[index_thread].ResponseHeaders;
                for (int i = 0; i < myWebHeaderCollection.Count; i++) {
                    if (myWebHeaderCollection.GetKey(i).ToLower().Equals("content-disposition")) {
                        ext_name = find_ext_from_header(myWebHeaderCollection.Get(i));
                        break;
                    }
                }
                try {
                    if (album_name_X == "") {
                        temp_save_path = save_dir + "\\";
                    } else {
                        temp_save_path = save_dir + "\\" + album_name_X + "\\";
                    }
                    System.IO.DirectoryInfo fi = new System.IO.DirectoryInfo(temp_save_path);
                    fi.Create();
                    photo_name[this_number_] = find_new_file_name(photo_name[this_number_], ext_name, temp_save_path);
                    System.IO.File.WriteAllBytes(temp_save_path + (photo_name[this_number_] + ext_name), bytes[index_thread]);
                    retry_times[this_number_] = 0;
                    try {
                        if (options_3_preview) {
                            //System.IO.FileStream stream = new System.IO.FileStream(temp_save_path + (photo_name[this_number_] + System.IO.Path.GetExtension(url_X_download)), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                            //Image temp_img = ScaleImage(handle_orientation(new Bitmap(System.Drawing.Image.FromStream(stream))),950,420);
                            //Image temp_img = ScaleImage(handle_orientation(temp_save_path + (photo_name[this_number_] + System.IO.Path.GetExtension(url_X_download))), 950, 420);
                            //setPictureImage(pictureBox1, temp_img);
                            //stream.Close();
                            if (!s_p_running[index_thread]) {
                                try {
                                    if (s_p[index_thread]!=null && s_p[index_thread].IsAlive) {
                                        s_p[index_thread].Abort();
                                    }
                                } catch {

                                }
                                s_p[index_thread] = null;
                                s_p[index_thread] = new System.Threading.Thread(() => preview_image_thread(temp_save_path + (photo_name[this_number_] + System.IO.Path.GetExtension(url_X_download)), index_thread));
                                s_p[index_thread].Start();
                            }
                        } else {
                            try {
                                setPictureImage(pictureBox1, null);
                            } catch {

                            }
                        }
                    } catch (Exception) {

                    }
                } catch (Exception ex1) {
                    Debug.WriteLine(ex1.ToString());
                    this.SetStatusText("存檔失敗！(" + (this_number_ + 1) + "/" + total_amount + ")");
                    append_msg_THREAD(this.richTextBox1, "存檔失敗(FAIL)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], Color.Red, Color.Yellow);
                    try {
                        append_msg_THREAD(this.richTextBox1, "重試(Retrying)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_m, bg_color_exe);
                        System.IO.File.WriteAllBytes(temp_save_path + (photo_name[this_number_] + ext_name), bytes[index_thread]);
                        append_msg_THREAD(this.richTextBox1, "下載完成(Success)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_g, bg_color_exe);
                    } catch {
                        try {
                            append_msg_THREAD(this.richTextBox1, "重試(Retrying)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_m, bg_color_exe);
                            System.IO.File.WriteAllBytes(temp_save_path + (photo_name[this_number_] + ext_name), bytes[index_thread]);
                            append_msg_THREAD(this.richTextBox1, "下載完成(Success)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_], color_f_g, bg_color_exe);
                        } catch (Exception ex) {
                            Debug.WriteLine(ex.ToString());
                            this.SetStatusText("存檔失敗！！(" + (this_number_ + 1) + "/" + total_amount + ")");
                            append_msg_THREAD(this.richTextBox1,"存檔失敗(Fatal FAIL)(" + (this_number_ + 1) + "/" + total_amount + ")" + photo_name[this_number_] + "\n" + url_X_download, Color.Red, Color.Yellow);
                            fail_list_name.Add(photo_name[this_number_]);
                            fail_list_url.Add(url_X_download);
                        }
                    }
                }
            }
            last_byte[index_thread] = 0;
            now_byte[index_thread] = 0;
            total_byte[index_thread] = 0;
            thread_busy[index_thread] = false;
        }

        delegate string[] callbyUI();

        string[] show_form2(string[] id_X, string[] title_X, string[] des_X, string[] img_X, string[] count_X, string[] owner_X, string[] ownerid_X) {
            if (this.InvokeRequired) {
                callbyUI cb = new callbyUI(()=>show_form2(id_X, title_X, des_X, img_X, count_X, owner_X, ownerid_X));
                return (string[])this.Invoke(cb);
            } else {
                pick_albums_form s = new pick_albums_form();
                s.BackColor = Color.FromArgb(exe_bg_color);
                s.statusStrip1.BackColor = Color.FromArgb(exe_bg_color);
                s.ForeColor = Color.FromArgb(exe_f_color);
                s.listView1.BackColor = Color.FromArgb(exe_bg_color);
                s.listView1.ForeColor = Color.FromArgb(exe_f_color);
                s.button1.ForeColor = Color.FromArgb(exe_button_color);
                s.button2.ForeColor = Color.FromArgb(exe_button_color);
                s.id_ = id_X;
                s.title_ = title_X;
                s.des_ = des_X;
                s.img_ = img_X;
                s.count_ = count_X;
                s.owner_ = owner_X;
                s.ownerid_ = ownerid_X;
                s.options_5_thread_num = options_5_thread_num;
                s.ShowDialog();
                tab1_textBox1.Text = String.Join(CrLf, s.url_X_selected);
                return (s.url_X_selected);
            }
        }

        string[] show_form3_pick_photo(string[] url_X, string[] img_X, string[] photo_title_X, string[] photo_id_X, string[] group_title_X, string[] group_id_X, string[] owner_X, string[] ownerid_X, string[] dateTaken_X, int search_perpage) {
            if (this.InvokeRequired) {
                callbyUI cb = new callbyUI(() => show_form3_pick_photo(url_X, img_X, photo_title_X, photo_id_X, group_title_X, group_id_X, owner_X, ownerid_X, dateTaken_X, search_perpage));
                return (string[])this.Invoke(cb);
            } else {
                pick_photos_form s = new pick_photos_form();
                s.BackColor = Color.FromArgb(exe_bg_color);
                s.ForeColor = Color.FromArgb(exe_f_color);
                s.statusStrip1.BackColor = Color.FromArgb(exe_bg_color);
                s.listView1.BackColor = Color.FromArgb(exe_bg_color);
                s.listView1.ForeColor = Color.FromArgb(exe_f_color);
                s.button1.ForeColor = Color.FromArgb(exe_button_color);
                s.button2.ForeColor = Color.FromArgb(exe_button_color);
                s.url_X = url_X;
                s.img_X = img_X;
                s.photo_title_X = photo_title_X;
                s.photo_id_X = photo_id_X;
                s.group_title_X = group_title_X;
                s.group_id_X = group_id_X;
                s.owner_X = owner_X;
                s.ownerid_X = ownerid_X;
                s.dateTaken_X = dateTaken_X;
                s.options_5_thread_num = options_5_thread_num;
                s.options_1_orginal_names = options_1_orginal_names;
                s.options_7_time_format = options_7_time_format;
                s.per_page = search_perpage;
                s.ShowDialog();
                return (s.url_X_selected);
            }
        }

        public string resolve_user_id(string str) {
            if (!str.StartsWith("http://") && !str.StartsWith("https://")) {
                return "https://www.flickr.com/people/" + str + "/";
            } else {
                return str;
            }
        }

        public string resolve_group_id(string str) {
            if (!str.StartsWith("http://") && !str.StartsWith("https://")) {
                return "https://www.flickr.com/groups/" + str + "/";
            } else {
                return str;
            }
        }

        public string get_video_url_from_image_url(string image_url, string user_id, ref bool succ) {
            var regex = new System.Text.RegularExpressions.Regex(@"(https|http):\/\/farm[0-9]+\.staticflickr\.com\/[0-9]+\/([0-9]+)_([0-9a-z]+)(_|)([0-9a-z]|)\.[a-z]+");
            var match = regex.Match(image_url);
            if (match.Success && !user_id.Equals("")) {
                succ = true;
                string id = match.Groups[2].Value;
                string secret = match.Groups[3].Value;
                string type_image = match.Groups[5].Value;
                if (type_image.Equals("o")) {
                    return "https://www.flickr.com/photos/" + user_id + "/" + id + "/play/orig/" + secret + "/";
                } else if (type_image.Equals("k") || type_image.Equals("h") || type_image.Equals("l") || type_image.Equals("b") || type_image.Equals("z")) {
                    return "https://www.flickr.com/photos/" + user_id + "/" + id + "/play/hd/" + secret + "/";
                } else if (type_image.Equals("b") || type_image.Equals("z")) {
                    return "https://www.flickr.com/photos/" + user_id + "/" + id + "/play/site/" + secret + "/";
                } else {
                    return "https://www.flickr.com/photos/" + user_id + "/" + id + "/play/mobile/" + secret + "/";
                    //return "http://www.flickr.com/video_download.gne?id=" + id;
                }
            } else {
                Debug.WriteLine("error not get vid user_id=" + user_id + "/url=" + image_url);
                succ = false;
                return image_url;
            }
        }

        private void run_main_download_FUNC(string save_dir) {
            lock (_thisLock_main_func) {
                if (tabs_==0 && types_ == 2 && read_from_save_file == false) {
                    //一位或多位藝術家相簿-非讀檔
                    //轉成photosetIDArray
                    string jsonx_str;
                    JObject jObjx;
                    string[][] temp_url_url_;
                    temp_url_url_ = new string[url_.Length][];
                    init_string_2di_array(ref temp_url_url_);
                    for (int x = 0; x < url_.Length; x++) {
                        if (pause_download) {
                            pause();
                        }
                        if (download_started == false) {
                            break;
                        }
                        jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.urls.lookupUser&api_key=" + api_key + "&url=" + resolve_user_id(url_[x]) + "&format=json", signed_auth_bool);
                        jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                        string user_id = "";
                        int pages = 1;
                        int total_photosets = 0;
                        try {
                            if (jObjx["stat"].ToString() == "ok") {
                                user_id = jObjx["user"]["id"].ToString();
                                append_msg_THREAD("用戶名：" + unicode_to_string(jObjx["user"]["username"]["_content"].ToString()) + "\n用戶ID：" + user_id, color_f_w, bg_color_exe);
                            } else {
                                temp_url_url_[x] = new string[0];
                                append_msg_THREAD("資訊錯誤！Error! Can't get user info.\n" + jObjx["message"].ToString(), Color.Red, Color.Yellow);
                                continue;
                            }
                        } catch (Exception) {
                            temp_url_url_[x] = new string[0];
                            append_msg_THREAD("資訊錯誤！Error! Can't get user info.", Color.Red, Color.Yellow);
                            continue;
                        }
                        if (download_started) {
                            if (pause_download) {
                                pause();
                            }
                            jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photosets.getList&api_key=" + api_key + "&user_id=" + user_id + "&per_page=500&page=1&format=json", signed_auth_bool);
                            jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                            string[] temp_url_;
                            try {
                                if (jObjx["stat"].ToString() == "ok") {
                                    pages = int.Parse(jObjx["photosets"]["pages"].ToString());
                                    total_photosets = int.Parse(jObjx["photosets"]["total"].ToString());
                                    temp_url_ = new string[total_photosets];
                                } else {
                                    Debug.WriteLine("error1");
                                    append_msg_THREAD("資訊錯誤！Error! Can't get album list info.\n" + jObjx["message"].ToString(), Color.Red, Color.Yellow);
                                    temp_url_url_[x] = new string[0];
                                    continue;
                                }
                                for (int ix = 0; ix < 500; ix++) {
                                    if (ix >= total_photosets) {
                                        break;
                                    }
                                    try {
                                        temp_url_[ix] = jObjx["photosets"]["photoset"][ix]["id"].ToString();
                                    } catch {
                                        temp_url_[ix] = "";
                                    }
                                }
                            } catch (Exception ex) {
                                temp_url_url_[x] = new string[0];
                                append_msg_THREAD("資訊錯誤！Error! Can't get album list info." + ex.ToString(), Color.Red, Color.Yellow);
                                continue;
                            }
                            if (total_photosets == 0) {
                                append_msg_THREAD("0個相簿(Zero Album)！", Color.Red, bg_color_exe);
                                temp_url_url_[x] = new string[0];
                                continue;
                            } else {
                                append_msg_THREAD("共(Total)：" + total_photosets + "個相簿(Album)！", color_f_g, bg_color_exe);
                                append_msg_THREAD("已下載第1頁/共" + pages + "頁", color_f_w, bg_color_exe);
                                for (int page = 2; page <= pages; page++) {
                                    jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photosets.getList&primary_photo_extras=url_s&api_key=" + api_key + "&user_id=" + user_id + "&per_page=500&page=" + page + "&format=json", signed_auth_bool);
                                    jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                                    append_msg_THREAD("已下載第" + page + "頁/共" + pages + "頁", color_f_w, bg_color_exe);
                                    for (int ix = 0; ix < 500; ix++) {
                                        if (ix + ((page - 1) * 500) >= total_photosets) {
                                            break;
                                        }
                                        try {
                                            temp_url_[ix + ((page - 1) * 500)] = jObjx["photosets"]["photoset"][ix]["id"].ToString();
                                        } catch {
                                            temp_url_[ix + ((page - 1) * 500)] = "";
                                        }
                                    }
                                }
                                temp_url_url_[x] = temp_url_;
                            }
                        }
                    }
                    url_ = string_2di_array_to_1di_array(temp_url_url_);
                    setTextbox(tab1_textBox1, String.Join(CrLf, url_));
                }

                if (tabs_ == 0 && types_ == 3 && read_from_save_file == false) {
                    //人工挑選一位或多位藝術家相簿-非讀檔
                    //轉成photosetIDArray
                    string jsonx_str;
                    JObject jObjx;
                    string[][] temp_url_url_ = new string[url_.Length][];
                    string[][] temp_title_title_ = new string[url_.Length][];
                    string[][] temp_des_des_ = new string[url_.Length][];
                    string[][] temp_img_img_ = new string[url_.Length][];
                    string[][] temp_count_count_ = new string[url_.Length][];
                    string[][] temp_owner_owner_ = new string[url_.Length][];
                    string[][] temp_ownerid_ownerid_ = new string[url_.Length][];
                    init_string_2di_array(ref temp_url_url_);
                    init_string_2di_array(ref temp_title_title_);
                    init_string_2di_array(ref temp_des_des_);
                    init_string_2di_array(ref temp_img_img_);
                    init_string_2di_array(ref temp_count_count_);
                    init_string_2di_array(ref temp_owner_owner_);
                    init_string_2di_array(ref temp_ownerid_ownerid_);
                    for (int x = 0; x < url_.Length; x++) {
                        if (pause_download) {
                            pause();
                        }
                        if (download_started == false) {
                            break;
                        }
                        jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.urls.lookupUser&api_key=" + api_key + "&url=" + resolve_user_id(url_[x]) + "&format=json", signed_auth_bool);
                        jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                        string user_id = "";
                        string user_name = "";
                        int pages = 1;
                        int total_photosets = 0;
                        try {
                            if (jObjx["stat"].ToString() == "ok") {
                                user_id = jObjx["user"]["id"].ToString();
                                user_name = unicode_to_string(jObjx["user"]["username"]["_content"].ToString());
                                append_msg_THREAD("用戶名：" + user_name + "\n用戶ID：" + user_id, color_f_w, bg_color_exe);
                            } else {
                                append_msg_THREAD("資訊錯誤！Error! Can't get user info.\n" + jObjx["message"].ToString(), Color.Red, Color.Yellow);
                                temp_url_url_[x] = new string[0];
                                temp_title_title_[x] = new string[0];
                                temp_des_des_[x] = new string[0];
                                temp_img_img_[x] = new string[0];
                                temp_count_count_[x] = new string[0];
                                temp_owner_owner_[x] = new string[0];
                                temp_ownerid_ownerid_[x] = new string[0];
                                continue;
                            }
                        } catch (Exception) {
                            append_msg_THREAD("資訊錯誤！Error! Can't get user info.", Color.Red, Color.Yellow);
                            temp_url_url_[x] = new string[0];
                            temp_title_title_[x] = new string[0];
                            temp_des_des_[x] = new string[0];
                            temp_img_img_[x] = new string[0];
                            temp_count_count_[x] = new string[0];
                            temp_owner_owner_[x] = new string[0];
                            temp_ownerid_ownerid_[x] = new string[0];
                            continue;
                        }
                        if (download_started) {
                            if (pause_download) {
                                pause();
                            }
                            jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photosets.getList&primary_photo_extras=url_s&api_key=" + api_key + "&user_id=" + user_id + "&per_page=500&page=1&format=json", signed_auth_bool);
                            jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                            string[] temp_url_;
                            string[] temp_title_;
                            string[] temp_des_;
                            string[] temp_img_;
                            string[] temp_count_;
                            string[] temp_owner_;
                            string[] temp_ownerid_;
                            try {
                                if (jObjx["stat"].ToString() == "ok") {
                                    pages = int.Parse(jObjx["photosets"]["pages"].ToString());
                                    total_photosets = int.Parse(jObjx["photosets"]["total"].ToString());
                                    temp_url_ = new string[total_photosets];
                                    temp_title_ = new string[total_photosets];
                                    temp_des_ = new string[total_photosets];
                                    temp_img_ = new string[total_photosets];
                                    temp_count_ = new string[total_photosets];
                                    temp_owner_ = new string[total_photosets];
                                    temp_ownerid_ = new string[total_photosets];
                                } else {
                                    Debug.WriteLine("error1");
                                    append_msg_THREAD("資訊錯誤！Error! Can't get album list info.\n" + jObjx["message"].ToString(), Color.Red, Color.Yellow);
                                    temp_url_url_[x] = new string[0];
                                    temp_title_title_[x] = new string[0];
                                    temp_des_des_[x] = new string[0];
                                    temp_img_img_[x] = new string[0];
                                    temp_count_count_[x] = new string[0];
                                    temp_owner_owner_[x] = new string[0];
                                    temp_ownerid_ownerid_[x] = new string[0];
                                    continue;
                                }
                                for (int ix = 0; ix < 500; ix++) {
                                    if (ix >= total_photosets) {
                                        break;
                                    }
                                    try {
                                        temp_url_[ix] = jObjx["photosets"]["photoset"][ix]["id"].ToString();
                                    } catch {
                                        temp_url_[ix] = "";
                                    }
                                    try {
                                        temp_title_[ix] = unicode_to_string(jObjx["photosets"]["photoset"][ix]["title"]["_content"].ToString());
                                    } catch {
                                        temp_title_[ix] = "";
                                    }
                                    try {
                                        temp_des_[ix] = unicode_to_string(jObjx["photosets"]["photoset"][ix]["description"]["_content"].ToString());
                                    } catch {
                                        temp_des_[ix] = "";
                                    }
                                    try {
                                        temp_img_[ix] = jObjx["photosets"]["photoset"][ix]["primary_photo_extras"]["url_s"].ToString();
                                    } catch {
                                        temp_img_[ix] = "";
                                    }
                                    try {
                                        temp_count_[ix] = jObjx["photosets"]["photoset"][ix]["photos"].ToString();
                                    } catch {
                                        temp_count_[ix] = "0";
                                    }
                                    temp_owner_[ix] = user_name;
                                    temp_ownerid_[ix] = user_id;
                                }
                            } catch (Exception ex) {
                                append_msg_THREAD("資訊錯誤！Error! Can't get album list info." + ex.ToString(), Color.Red, Color.Yellow);
                                temp_url_url_[x] = new string[0];
                                temp_title_title_[x] = new string[0];
                                temp_des_des_[x] = new string[0];
                                temp_img_img_[x] = new string[0];
                                temp_count_count_[x] = new string[0];
                                temp_owner_owner_[x] = new string[0];
                                temp_ownerid_ownerid_[x] = new string[0];
                                continue;
                            }
                            if (total_photosets == 0) {
                                append_msg_THREAD("0個相簿(Zero Album)！", Color.Red, bg_color_exe);
                                temp_url_url_[x] = new string[0];
                                temp_title_title_[x] = new string[0];
                                temp_des_des_[x] = new string[0];
                                temp_img_img_[x] = new string[0];
                                temp_count_count_[x] = new string[0];
                                temp_owner_owner_[x] = new string[0];
                                temp_ownerid_ownerid_[x] = new string[0];
                                continue;
                            } else {
                                append_msg_THREAD("共(Total)：" + total_photosets + "個相簿(Album)！", color_f_g, bg_color_exe);
                                append_msg_THREAD("已下載第1頁/共" + pages + "頁", color_f_w, bg_color_exe);
                                for (int page = 2; page <= pages; page++) {
                                    jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photosets.getList&primary_photo_extras=url_s&api_key=" + api_key + "&user_id=" + user_id + "&per_page=500&page=" + page + "&format=json", signed_auth_bool);
                                    jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                                    append_msg_THREAD("已下載第" + page + "頁/共" + pages + "頁", color_f_w, bg_color_exe);
                                    for (int ix = 0; ix < 500; ix++) {
                                        if (ix + ((page - 1) * 500) >= total_photosets) {
                                            break;
                                        }
                                        try {
                                            temp_url_[ix + ((page - 1) * 500)] = jObjx["photosets"]["photoset"][ix]["id"].ToString();
                                            temp_title_[ix + ((page - 1) * 500)] = unicode_to_string(jObjx["photosets"]["photoset"][ix]["title"]["_content"].ToString());
                                            temp_des_[ix + ((page - 1) * 500)] = unicode_to_string(jObjx["photosets"]["photoset"][ix]["description"]["_content"].ToString());
                                            temp_count_[ix + ((page - 1) * 500)] = jObjx["photosets"]["photoset"][ix]["photos"].ToString();
                                        } catch {
                                            temp_url_[ix + ((page - 1) * 500)] = "";
                                            temp_title_[ix + ((page - 1) * 500)] = "";
                                            temp_des_[ix + ((page - 1) * 500)] = "";
                                            temp_count_[ix + ((page - 1) * 500)] = "";
                                        }
                                        try {
                                            temp_img_[ix + ((page - 1) * 500)] = jObjx["photosets"]["photoset"][ix]["primary_photo_extras"]["url_s"].ToString();
                                        } catch {
                                            temp_img_[ix + ((page - 1) * 500)] = "";
                                        }
                                        temp_owner_[ix + ((page - 1) * 500)] = user_name;
                                        temp_ownerid_[ix + ((page - 1) * 500)] = user_id;
                                    }
                                }
                                temp_url_url_[x] = temp_url_;
                                temp_title_title_[x] = temp_title_;
                                temp_des_des_[x] = temp_des_;
                                temp_img_img_[x] = temp_img_;
                                temp_count_count_[x] = temp_count_;
                                temp_owner_owner_[x] = temp_owner_;
                                temp_ownerid_ownerid_[x] = temp_ownerid_;
                            }
                        }
                    }
                    string[] id_X = string_2di_array_to_1di_array(temp_url_url_);
                    string[] title_X = string_2di_array_to_1di_array(temp_title_title_);
                    string[] des_X = string_2di_array_to_1di_array(temp_des_des_);
                    string[] img_X = string_2di_array_to_1di_array(temp_img_img_);
                    string[] count_X = string_2di_array_to_1di_array(temp_count_count_);
                    string[] owner_X = string_2di_array_to_1di_array(temp_owner_owner_);
                    string[] ownerid_X = string_2di_array_to_1di_array(temp_ownerid_ownerid_);

                    if (img_X.Length != 0) {
                        url_ = show_form2(id_X, title_X, des_X, img_X, count_X, owner_X, ownerid_X);
                    }
                }

                if (tabs_ == 0 && types_ == 6 && read_from_save_file == false) {
                    //人工挑選一位或多個相簿之相片-非讀檔
                    //轉成photo_URL_Array
                    string jsonx_str;
                    JObject jObjx;
                    string[][] temp_url_url_ = new string[url_.Length][];
                    string[][] temp_img_img_ = new string[url_.Length][];
                    string[][] temp_photo_title_title_ = new string[url_.Length][];
                    string[][] temp_photo_id_id_ = new string[url_.Length][];
                    string[][] temp_group_title_title_ = new string[url_.Length][];
                    string[][] temp_group_id_id_ = new string[url_.Length][];
                    string[][] temp_owner_owner_ = new string[url_.Length][];
                    string[][] temp_ownerid_ownerid_ = new string[url_.Length][];
                    string[][] temp_dateTaken_dateTaken_ = new string[url_.Length][];
                    init_string_2di_array(ref temp_url_url_);
                    init_string_2di_array(ref temp_img_img_);
                    init_string_2di_array(ref temp_photo_title_title_);
                    init_string_2di_array(ref temp_photo_id_id_);
                    init_string_2di_array(ref temp_group_title_title_);
                    init_string_2di_array(ref temp_group_id_id_);
                    init_string_2di_array(ref temp_owner_owner_);
                    init_string_2di_array(ref temp_ownerid_ownerid_);
                    init_string_2di_array(ref temp_dateTaken_dateTaken_);
                    string id = "";
                    bool error = false;
                    for (int x = 0; x < url_.Length; x++) {
                        error = false;
                        if (pause_download) {
                            pause();
                        }
                        if (download_started == false) {
                            break;
                        }
                        id = get_photoset_id(url_[x]);
                        if (id == "0" || id=="") {
                            error = true;
                        }
                        if (error == false) {
                            append_msg_THREAD("相簿ID：" + id, color_f_w, bg_color_exe);
                            jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photosets.getPhotos&api_key=" + api_key + "&per_page=1&photoset_id=" + id + "&format=json", signed_auth_bool);
                            jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                            if (jObjx["stat"].ToString() != "ok") {
                                append_msg_THREAD("資訊錯誤！Error! Can't get info.\n" + jObjx["message"].ToString(), Color.Red, Color.Yellow);
                                continue;
                            }
                            string owner_name = unicode_to_string(jObjx["photoset"]["ownername"].ToString());
                            string owner_id = jObjx["photoset"]["owner"].ToString();
                            append_msg_THREAD("相簿名稱(Name)：" + unicode_to_string(jObjx["photoset"]["title"].ToString()) + "\n擁有者(owner)：" + unicode_to_string(jObjx["photoset"]["ownername"].ToString()) + "\n擁有者ID(owner ID)：" + jObjx["photoset"]["owner"].ToString() + "\n共 " + jObjx["photoset"]["total"].ToString() + " 張相片(Total:" + jObjx["photoset"]["total"].ToString() + ")", color_f_w, bg_color_exe);
                            int total_photo = int.Parse(jObjx["photoset"]["total"].ToString());
                            int total_pages = (int)Math.Ceiling((decimal)total_photo / 500);
                            int totol_photo_test = 0;
                            string[] temp_url_ = new string[total_photo];
                            string[] temp_img_ = new string[total_photo];
                            string[] temp_photo_title_ = new string[total_photo];
                            string[] temp_photo_id_ = new string[total_photo];
                            string[] temp_group_title_ = new string[total_photo];
                            string[] temp_group_id_ = new string[total_photo];
                            string[] temp_owner_ = new string[total_photo];
                            string[] temp_ownerid_ = new string[total_photo];
                            string[] temp_dateTaken_ = new string[total_photo];
                            for (int page = 1; page <= total_pages; page++) {
                                if (pause_download) {
                                    pause();
                                }
                                jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photosets.getPhotos&per_page=500&api_key=" + api_key + "&photoset_id=" + id + "&page=" + page + "&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,media,date_taken&format=json", signed_auth_bool);
                                jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                                totol_photo_test = int.Parse(jObjx["photoset"]["total"].ToString());
                                if (totol_photo_test != total_photo) {
                                    Debug.WriteLine("Flickr API mismatch number! photosetnum=" + total_photo + " & urlnum=" + totol_photo_test);
                                    total_photo = get_max_number(total_photo, totol_photo_test);
                                    total_pages = (int)Math.Ceiling((decimal)total_photo / 500);
                                    append_msg_THREAD("相簿數量異常！自動修正！Auto Fixed！\n總相片數(Total)：" + total_photo, color_f_w, bg_color_exe);
                                    temp_url_ = new string[total_photo];
                                    temp_img_ = new string[total_photo];
                                    temp_photo_title_ = new string[total_photo];
                                    temp_photo_id_ = new string[total_photo];
                                    temp_group_title_ = new string[total_photo];
                                    temp_group_id_ = new string[total_photo];
                                    temp_owner_ = new string[total_photo];
                                    temp_ownerid_ = new string[total_photo];
                                    temp_dateTaken_ = new string[total_photo];
                                }
                                string photoset_title = unicode_to_string(jObjx["photoset"]["title"].ToString());
                                append_msg_THREAD("已下載第" + page + "頁/共" + total_pages + "頁", color_f_w, bg_color_exe);
                                for (int ix = 0; ix < 500; ix++) {
                                    if ((ix + ((page - 1) * 500)) >= total_photo) {
                                        break;
                                    }
                                    try {
                                        temp_url_[ix + ((page - 1) * 500)] = get_url((JObject)jObjx["photoset"]["photo"][ix], false, owner_id);
                                        temp_img_[ix + ((page - 1) * 500)] = get_url((JObject)jObjx["photoset"]["photo"][ix], 8, true, owner_id);
                                        try {
                                            temp_photo_title_[ix + ((page - 1) * 500)] = filename_filter(unicode_to_string(jObjx["photoset"]["photo"][ix]["title"].ToString())).Trim(' ');
                                        } catch {
                                            temp_photo_title_[ix + ((page - 1) * 500)] = "未命名 NoName";
                                        }
                                        temp_photo_id_[ix + ((page - 1) * 500)] = jObjx["photoset"]["photo"][ix]["id"].ToString();
                                        temp_group_title_[ix + ((page - 1) * 500)] = photoset_title;
                                        temp_group_id_[ix + ((page - 1) * 500)] = id.ToString();
                                        temp_owner_[ix + ((page - 1) * 500)] = owner_name;
                                        temp_ownerid_[ix + ((page - 1) * 500)] = owner_id;
                                        temp_dateTaken_[ix + ((page - 1) * 500)] = jObjx["photoset"]["photo"][ix]["datetaken"].ToString();
                                    } catch {
                                        temp_url_[ix + ((page - 1) * 500)] = "";
                                        temp_img_[ix + ((page - 1) * 500)] = "";
                                        temp_photo_title_[ix + ((page - 1) * 500)] = "Error";
                                        temp_photo_id_[ix + ((page - 1) * 500)] = "error_id_unknow";
                                        temp_group_title_[ix + ((page - 1) * 500)] = photoset_title;
                                        temp_group_id_[ix + ((page - 1) * 500)] = id.ToString();
                                        temp_owner_[ix + ((page - 1) * 500)] = "Error";
                                        temp_ownerid_[ix + ((page - 1) * 500)] = "error_id_unknow";
                                        temp_dateTaken_[ix + ((page - 1) * 500)] = "0000-00-00 00:00:00";
                                    }
                                }
                            }
                            temp_url_url_[x] = temp_url_;
                            temp_img_img_[x] = temp_img_;
                            temp_photo_title_title_[x] = temp_photo_title_;
                            temp_photo_id_id_[x] = temp_photo_id_;
                            temp_group_title_title_[x] = temp_group_title_;
                            temp_group_id_id_[x] = temp_group_id_;
                            temp_owner_owner_[x] = temp_owner_;
                            temp_ownerid_ownerid_[x] = temp_ownerid_;
                            temp_dateTaken_dateTaken_[x] = temp_dateTaken_;
                        } else {
                            temp_url_url_[x] = new string[0];
                            temp_img_img_[x] = new string[0];
                            temp_photo_title_title_[x] = new string[0];
                            temp_photo_id_id_[x] = new string[0];
                            temp_group_title_title_[x] = new string[0];
                            temp_group_id_id_[x] = new string[0];
                            temp_owner_owner_[x] = new string[0];
                            temp_ownerid_ownerid_[x] = new string[0];
                            temp_dateTaken_dateTaken_[x] = new string[0];
                        }
                    }
                    string[] url_X = string_2di_array_to_1di_array(temp_url_url_);
                    string[] img_X = string_2di_array_to_1di_array(temp_img_img_);
                    string[] photo_title_X = string_2di_array_to_1di_array(temp_photo_title_title_);
                    string[] photo_id_X = string_2di_array_to_1di_array(temp_photo_id_id_);
                    string[] group_title_X = string_2di_array_to_1di_array(temp_group_title_title_);
                    string[] group_id_X = string_2di_array_to_1di_array(temp_group_id_id_);
                    string[] owner_X = string_2di_array_to_1di_array(temp_owner_owner_);
                    string[] ownerid_X = string_2di_array_to_1di_array(temp_ownerid_ownerid_);
                    string[] dateTaken_X = string_2di_array_to_1di_array(temp_dateTaken_dateTaken_);

                    int remove_count = 0;
                    check_url_for_pre_download_step(ref photo_title_X, ref url_X, ref img_X, ref photo_id_X, ref group_title_X, ref group_id_X, ref owner_X, ref ownerid_X, ref dateTaken_X, ref remove_count);

                    if (remove_count != 0) {
                        append_msg_THREAD("錯誤，有" + remove_count + "組URL不合法！Error!" + remove_count + " url is incorrect!", Color.Red, Color.Yellow);
                    }

                    if (img_X.Length != 0) {
                        url_ = show_form3_pick_photo(url_X, img_X, photo_title_X, photo_id_X, group_title_X, group_id_X, owner_X, ownerid_X, dateTaken_X, search_perpage);
                    }
                }

                if (tabs_ == 1 && read_from_save_file == false) {
                    //人工挑選搜尋之相片-非讀檔
                    //轉成photo_URL_Array
                    string jsonx_str;
                    JObject jObjx;
                    string[][] temp_url_url_ = new string[search_fetchpage][];
                    string[][] temp_img_img_ = new string[search_fetchpage][];
                    string[][] temp_photo_title_title_ = new string[search_fetchpage][];
                    string[][] temp_photo_id_id_ = new string[search_fetchpage][];
                    string[][] temp_group_title_title_ = new string[search_fetchpage][];
                    string[][] temp_group_id_id_ = new string[search_fetchpage][];
                    string[][] temp_owner_owner_ = new string[search_fetchpage][];
                    string[][] temp_ownerid_ownerid_ = new string[search_fetchpage][];
                    string[][] temp_dateTaken_dateTaken_ = new string[search_fetchpage][];
                    init_string_2di_array(ref temp_url_url_);
                    init_string_2di_array(ref temp_img_img_);
                    init_string_2di_array(ref temp_photo_title_title_);
                    init_string_2di_array(ref temp_photo_id_id_);
                    init_string_2di_array(ref temp_group_title_title_);
                    init_string_2di_array(ref temp_group_id_id_);
                    init_string_2di_array(ref temp_owner_owner_);
                    init_string_2di_array(ref temp_ownerid_ownerid_);
                    init_string_2di_array(ref temp_dateTaken_dateTaken_);
                    string text_QUERY = "";
                    string user_id_QUERY = "";
                    string group_id_QUERY = "";
                    string privacy_filter_QUERY = "";
                    string safe_search_QUERY = "";
                    if (search_keyword != "") {
                        text_QUERY = "&text=" + (search_keyword.Replace("&"," "));
                    }
                    if (search_user_id != "") {
                        user_id_QUERY = "&user_id=" + search_user_id;
                    }
                    if (search_group_id != "") {
                        group_id_QUERY = "&group_id=" + search_group_id;
                    }
                    if (search_userid_type == 1 && search_userid_privacy!=0) {
                        privacy_filter_QUERY = "&privacy_filter=" + search_userid_privacy;
                    }
                    if (signed_auth_bool && search_safe_search != 0) {
                        safe_search_QUERY = "&safe_search=" + search_safe_search;
                    }
                    int index_j = 1;
                    int total_pages = (-1);
                    for (int page = search_startpage; page < (search_startpage + search_fetchpage); page++) {
                        if (pause_download) {
                            pause();
                        }
                        if (download_started == false) {
                            break;
                        }
                        jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key=" + api_key + text_QUERY + user_id_QUERY + group_id_QUERY + privacy_filter_QUERY + safe_search_QUERY + "&page=" + page + "&per_page=" + search_perpage + "&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,media,owner_name,date_taken&format=json", signed_auth_bool);
                        jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                        if (jObjx["stat"].ToString() != "ok") {
                            append_msg_THREAD("資訊錯誤！Error! Can't get info.\n" + jObjx["message"].ToString(), Color.Red, Color.Yellow);
                            continue;
                        }
                        int total_photo = jObjx["photos"]["photo"].Count();
                        total_pages = int.Parse(jObjx["photos"]["pages"].ToString());
                        append_msg_THREAD("共 " + jObjx["photos"]["total"].ToString() + " 張相片(Total Photos:" + jObjx["photos"]["total"].ToString() + ")", color_f_w, bg_color_exe);
                        append_msg_THREAD("共 " + total_pages + " 頁(Total Pages:" + total_pages + ")", color_f_w, bg_color_exe);
                        if (page>total_pages) {
                            append_msg_THREAD("錯誤，頁數超過！Error!Page error!\n當前頁數(Current Page)：" + page, Color.Red, Color.Yellow);
                            break;
                        }
                        append_msg_THREAD("本頁共 " + total_photo + " 張相片(This Page Total:" + total_photo + ")", color_f_w, bg_color_exe);
                        append_msg_THREAD("本頁為搜尋結果第 " + page + " 頁(This Page is No." + page + ")", color_f_w, bg_color_exe);
                        append_msg_THREAD("已下載" + (index_j) + "頁/共" + search_fetchpage + "頁", color_f_w, bg_color_exe);
                        string[] temp_url_ = new string[total_photo];
                        string[] temp_img_ = new string[total_photo];
                        string[] temp_photo_title_ = new string[total_photo];
                        string[] temp_photo_id_ = new string[total_photo];
                        string[] temp_group_title_ = new string[total_photo];
                        string[] temp_group_id_ = new string[total_photo];
                        string[] temp_owner_ = new string[total_photo];
                        string[] temp_ownerid_ = new string[total_photo];
                        string[] temp_dateTaken_ = new string[total_photo];
                        for (int ix=0; ix< total_photo; ix++) {
                            try {
                                temp_url_[ix] = get_url((JObject)jObjx["photos"]["photo"][ix], true, jObjx["photos"]["photo"][ix]["owner"].ToString());
                                temp_img_[ix] = get_url((JObject)jObjx["photos"]["photo"][ix], 8, true, jObjx["photos"]["photo"][ix]["owner"].ToString());
                                try {
                                    temp_photo_title_[ix] = filename_filter(unicode_to_string(jObjx["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                } catch {
                                    temp_photo_title_[ix] = "未命名 NoName";
                                }
                                temp_photo_id_[ix] = jObjx["photos"]["photo"][ix]["id"].ToString();
                                temp_group_title_[ix] = "";
                                temp_group_id_[ix] = "";
                                try {
                                    temp_owner_[ix] = jObjx["photos"]["photo"][ix]["ownername"].ToString();
                                } catch {
                                    temp_owner_[ix] = "";
                                }
                                try {
                                    temp_ownerid_[ix] = jObjx["photos"]["photo"][ix]["owner"].ToString();
                                } catch {
                                    temp_ownerid_[ix] = "";
                                }
                                try {
                                    temp_dateTaken_[ix] = jObjx["photos"]["photo"][ix]["datetaken"].ToString();
                                } catch {
                                    temp_dateTaken_[ix] = "";
                                }
                            } catch {
                                temp_url_[ix] = "";
                                temp_img_[ix] = "";
                                temp_photo_title_[ix] = "";
                                temp_photo_id_[ix] = "";
                                temp_group_title_[ix] = "";
                                temp_group_id_[ix] = "";
                                temp_owner_[ix] = "";
                                temp_ownerid_[ix] = "";
                                temp_dateTaken_[ix] = "";
                            }
                        }
                        temp_url_url_[index_j - 1] = temp_url_;
                        temp_img_img_[index_j - 1] = temp_img_;
                        temp_photo_title_title_[index_j - 1] = temp_photo_title_;
                        temp_photo_id_id_[index_j - 1] = temp_photo_id_;
                        temp_group_title_title_[index_j - 1] = temp_group_title_;
                        temp_group_id_id_[index_j - 1] = temp_group_id_;
                        temp_owner_owner_[index_j - 1] = temp_owner_;
                        temp_ownerid_ownerid_[index_j - 1] = temp_ownerid_;
                        temp_dateTaken_dateTaken_[index_j - 1] = temp_dateTaken_;
                        index_j += 1;
                    }
                    string[] url_X = string_2di_array_to_1di_array(temp_url_url_);
                    string[] img_X = string_2di_array_to_1di_array(temp_img_img_);
                    string[] photo_title_X = string_2di_array_to_1di_array(temp_photo_title_title_);
                    string[] photo_id_X = string_2di_array_to_1di_array(temp_photo_id_id_);
                    string[] group_title_X = string_2di_array_to_1di_array(temp_group_title_title_);
                    string[] group_id_X = string_2di_array_to_1di_array(temp_group_id_id_);
                    string[] owner_X = string_2di_array_to_1di_array(temp_owner_owner_);
                    string[] ownerid_X = string_2di_array_to_1di_array(temp_ownerid_ownerid_);
                    string[] dateTaken_X = string_2di_array_to_1di_array(temp_dateTaken_dateTaken_);

                    int remove_count = 0;
                    check_url_for_pre_download_step(ref photo_title_X, ref url_X, ref img_X, ref photo_id_X, ref group_title_X, ref group_id_X, ref owner_X, ref ownerid_X, ref dateTaken_X, ref remove_count);

                    if (remove_count != 0) {
                        append_msg_THREAD("錯誤，有" + remove_count + "組URL不合法！Error!" + remove_count + " url is incorrect!", Color.Red, Color.Yellow);
                    }

                    if (img_X.Length != 0) {
                        url_ = show_form3_pick_photo(url_X, img_X, photo_title_X, photo_id_X, group_title_X, group_id_X, owner_X, ownerid_X, dateTaken_X, search_perpage);
                    }

                }

                int prog_total_max = url_.Length;

                setToolStripProgressBar_max(toolStripProgressBar3, url_.Length);
                setToolStripProgressBar_value(toolStripProgressBar3, 0);

                Debug.WriteLine(url_.Length);

                Debug.WriteLine("URLlength:Textbox1length:" + select_if_new_save_i(1) + "/" + url_.Length);
                if (save_v3 == url_.Length && save_v3 != 0 && read_from_save_file) {
                    save_v3 = (save_v3 - 1);
                    append_msg_THREAD("修正上一版記錄檔錯誤！", color_f_w, bg_color_exe);
                    setToolStripProgressBar_value(toolStripProgressBar3, save_v3);
                }

                if (url_.Length==0 && read_from_save_file && tabs_==1 && photo_url_i_ARR_save.Length!=0) {
                    url_ =(string[]) photo_url_i_ARR_save.Clone() ;
                }

                //開始Run URL 迴圈

                for (int i = select_if_new_save_i(1); i < url_.Length; i++) {
                    if (!download_started) {
                        append_msg_THREAD("\n取消 Cancel\n\n", color_f_w, bg_color_exe);
                        break;
                    }
                    if (pause_download) {
                        pause();
                    }
                    append_msg_THREAD("URL:" + url_[i], color_f_w, bg_color_exe);
                    //status_text.Text = "分析中…(Analysing...)";
                    SetStatusText("分析中…(Analysing...)");
                    append_msg_THREAD("分析中…(Analysing...)", color_f_w, bg_color_exe);
                    bool error = false;
                    string album_name_X = "";
                    string id = "";
                    string temp_title = "";
                    string temp_url = "";
                    string json_str;
                    JObject jObj;
                    string[] photo_name = new string[] { };
                    string[] photo_url = new string[] { };
                    try {
                        if (save_file_20 && photo_name_i_ARR_save.Length != 0) {

                            Debug.WriteLine("進入讀檔下載模式");
                            photo_url = photo_url_i_ARR_save;
                            photo_name = photo_name_i_ARR_save;
                            album_name_X = album_name_X_SAVE;
                            save_file_20 = false;

                            //Special Download START
                            retry_times = new int[photo_url.Length];
                            thread_download_number = new int[] { 0, 0, 0, 0 };
                            set_all_False(ref thread_busy);
                            set_all_Zero(ref retry_times);
                            //timer1.Start();
                            timer1_START = true;

                            //toolStripProgressBar2.Maximum = photo_url.Length;
                            setToolStripProgressBar_max(toolStripProgressBar2, photo_url.Length);

                            for (int i2 = 0; i2 < photo_url.Length; i2++) {
                                if (pause_download) {
                                    pause();
                                }
                                if (!download_started) {
                                    append_msg_THREAD("\n取消 Cancel\n\n", color_f_w, bg_color_exe);
                                    break;
                                }
                                string total_amount = prog_corrct(i2, photo_url.Length, i, url_.Length);

                                if (photo_url[i2] == "") {
                                    append_msg_THREAD("下載失敗(FAIL)(" + total_amount + ")" + photo_name[i2], Color.Red, Color.Yellow);
                                    continue;
                                }

                                if (live_X[i2]) {
                                    append_msg_THREAD("已下載，跳過。(Already Downloaded,Skip)(" + (i2 + 1) + "/" + total_amount + ")", color_f_m, bg_color_exe);
                                    continue;
                                }

                                //DOWNLOAD FUNC
                                int free_thread = switch_get_NOT_busy();
                                while (free_thread == (-1)) {
                                    if (!download_started) {
                                        break;
                                    }
                                    if (pause_download) {
                                        pause();
                                    }
                                    try {
                                        System.Threading.Thread.Sleep(50);
                                        //Application.DoEvents();
                                    } catch (NullReferenceException ex) {
                                        Debug.WriteLine(ex.ToString());
                                    }
                                    free_thread = switch_get_NOT_busy();
                                }
                                if (free_thread != (-1)) {
                                    thread_busy[free_thread] = true;
                                    thread_download_number[free_thread] = i2;
                                    while (s[free_thread] != null && s[free_thread].IsAlive) {
                                        System.Threading.Thread.Sleep(1);
                                    }
                                    s[free_thread] = null;
                                    string temp_url_X = photo_url[i2].Clone().ToString();
                                    s[free_thread] = new System.Threading.Thread(() => download_file_and_save(temp_url_X, free_thread, album_name_X, photo_name, false));
                                    s[free_thread].Start();
                                    append_msg_THREAD("下載中..(Downloading..)(" + total_amount + ")" + photo_name[i2], color_f_w, bg_color_exe);
                                }

                                if ((photo_url.Length - 1) == i2) {
                                    Debug.WriteLine("Waiting");
                                    while (switch_get_busy_count() != 0) {
                                        //Application.DoEvents();
                                        System.Threading.Thread.Sleep(10);
                                        if (pause_download) {
                                            pause();
                                        }
                                        if (!download_started) {
                                            break;
                                        }
                                    }
                                }

                                setToolStripProgressBar_value(toolStripProgressBar2, (i2 + 1));
                                //toolStripProgressBar2.Value = (i2 + 1);
                                setToolStripProgressBar_tooltiptext(toolStripProgressBar2, "(" + (i2 + 1) + "/" + toolStripProgressBar2.Maximum.ToString() + ")");
                                //toolStripProgressBar2.ToolTipText = "(" + (i2 + 1) + "/" + toolStripProgressBar2.Maximum + ")";
                                set_status_taskbar((i2 + 1), photo_url.Length, (i + 1), prog_total_max);
                                read_from_save_file = false;

                            }

                            Debug.WriteLine("Finished FIRST turn");

                            timer1_STOP = true;
                            setToolStripProgressBar_value(toolStripProgressBar3, (i + 1));
                            //toolStripProgressBar3.Value = (i + 1);
                            setToolStripProgressBar_tooltiptext(toolStripProgressBar3, "(" + (i + 1) + "/" + toolStripProgressBar3.Maximum + ")");

                            if (tabs_==0 && (types_ == 1 || types_ == 6)) {
                                //單張或多張相片不需要跑外圍迴圈，內部迴圈直接跑完。
                                break;
                            }
                            if (tabs_ == 1) {
                                //搜尋相片不需要跑外圍迴圈，內部迴圈直接跑完。
                                break;
                            }

                            continue;

                            //Special Download END
                        } else {
                            //OTHER!!!
                            switch (tabs_) {
                                //General Tab START
                                case 0:
                                    switch (types_) {
                                        //0,2,3 抓取相簿
                                        case 0://單或多相簿
                                        case 2://藝術家
                                        case 3://挑選一或多相簿從藝術家
                                            error = false;
                                            id = get_photoset_id(url_[i]);
                                            if (id == "0" || id == "") {
                                                error = true;
                                            }
                                            if (error == false) {
                                                append_msg_THREAD("相簿ID：" + id, color_f_w, bg_color_exe);
                                                json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photosets.getPhotos&api_key=" + api_key + "&photoset_id=" + id + "&per_page=1&format=json", signed_auth_bool);
                                                jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                if (jObj["stat"].ToString() != "ok") {
                                                    append_msg_THREAD("資訊錯誤！Error! Can't get info.\n" + jObj["message"].ToString(), Color.Red, Color.Yellow);
                                                    continue;
                                                }
                                                string owner_username = jObj["photoset"]["owner"].ToString();
                                                append_msg_THREAD("相簿名稱(Name)：" + unicode_to_string(jObj["photoset"]["title"].ToString()) + "\n擁有者(owner)：" + unicode_to_string(jObj["photoset"]["ownername"].ToString()) + "\n擁有者ID(owner ID)：" + jObj["photoset"]["owner"].ToString() + "\n共 " + jObj["photoset"]["total"].ToString() + " 張相片(Total:" + jObj["photoset"]["total"].ToString() + ")", color_f_w, bg_color_exe);
                                                if (options_2_auto_make_dirs) {
                                                    string temp_dirname = filename_filter(unicode_to_string(jObj["photoset"]["title"].ToString())).Trim(' ');
                                                    if (System.IO.Directory.Exists(save_dir + "\\" + temp_dirname) && read_from_save_file != true && options_6_skip_download != true) {
                                                        temp_dirname = find_new_dir_name(temp_dirname, save_dir);
                                                        try {
                                                            System.IO.Directory.CreateDirectory(save_dir + "\\" + temp_dirname);
                                                            append_msg_THREAD("創建資料夾成功(Success)！名稱(dir name)：" + save_dir + "\\" + temp_dirname, color_f_g, bg_color_exe);
                                                        } catch (Exception) {
                                                            append_msg_THREAD("錯誤無法創資料夾！Error! Can't create dir.", Color.Red, Color.Yellow);
                                                        }
                                                    }
                                                    album_name_X = temp_dirname;
                                                } else {
                                                    album_name_X = "";
                                                }
                                                int total_photo = int.Parse(jObj["photoset"]["total"].ToString());
                                                int total_pages = (int)Math.Ceiling((decimal)total_photo / 500);
                                                int totol_photo_test = 0;
                                                photo_name = new string[total_photo];
                                                photo_url = new string[total_photo];
                                                for (int page = 1; page <= total_pages; page++) {
                                                    if (pause_download) {
                                                        pause();
                                                    }
                                                    if (download_started == false) {
                                                        break;
                                                    }
                                                    json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photosets.getPhotos&per_page=500&api_key=" + api_key + "&photoset_id=" + id + "&page=" + page + "&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,media,date_taken&format=json", signed_auth_bool);
                                                    jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                    totol_photo_test = int.Parse(jObj["photoset"]["total"].ToString());
                                                    if (totol_photo_test != total_photo) {
                                                        Debug.WriteLine("Flickr API mismatch number! photosetnum=" + total_photo + " & urlnum=" + totol_photo_test);
                                                        total_photo = get_max_number(total_photo, totol_photo_test);
                                                        total_pages = (int)Math.Ceiling((decimal)total_photo / 500);
                                                        append_msg_THREAD("相簿數量異常！自動修正！Auto Fixed！\n總相片數(Total)：" + total_photo, color_f_w, bg_color_exe);
                                                        photo_name = new string[total_photo];
                                                        photo_url = new string[total_photo];
                                                    }
                                                    append_msg_THREAD("已下載第" + page + "頁/共" + total_pages + "頁", color_f_w, bg_color_exe);
                                                    for (int ix = 0; ix < 500; ix++) {
                                                        if ((ix + ((page - 1) * 500)) >= total_photo) {
                                                            break;
                                                        }
                                                        try {
                                                            if (options_1_orginal_names == 1) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 0) {
                                                                temp_title = (ix + 1 + ((page - 1) * 500)).ToString();
                                                            } else if (options_1_orginal_names == 2) {
                                                                temp_title = jObj["photoset"]["photo"][ix]["id"].ToString();
                                                            } else if (options_1_orginal_names == 3) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photoset"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 4) {
                                                                try {
                                                                    temp_title = jObj["photoset"]["photo"][ix]["id"].ToString() + " - " + filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 5) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photoset"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 6) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + jObj["photoset"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 7) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 8) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photoset"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photoset"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 9) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photoset"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 10) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photoset"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photoset"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 11) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photoset"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photoset"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 12) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photoset"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photoset"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 13) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photoset"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 14) {
                                                                try {
                                                                    temp_title = jObj["photoset"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photoset"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photoset"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 15) {
                                                                try {
                                                                    temp_title = jObj["photoset"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photoset"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 16) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photoset"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            }
                                                            temp_url = get_url((JObject)jObj["photoset"]["photo"][ix], false, owner_username);
                                                            if (exe_show_detail_enabled) {
                                                                append_msg_THREAD("(" + (ix + 1 + ((page - 1) * 500)) + "/" + total_photo + ")" + temp_title + "\n" + temp_url, color_f_w, bg_color_exe);
                                                            }
                                                            photo_name[ix + ((page - 1) * 500)] = temp_title;
                                                            photo_url[ix + ((page - 1) * 500)] = temp_url;
                                                        } catch {

                                                        }
                                                    }
                                                }

                                            }
                                            break;
                                        case 5://藝術家最愛的相片
                                            error = false;
                                            string jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.urls.lookupUser&api_key=" + api_key + "&url=" + resolve_user_id(url_[i]) + "&format=json", signed_auth_bool);
                                            JObject jObjx = (JObject)JsonConvert.DeserializeObject(jsonx_str);//dynamic
                                            string user_id = "";
                                            string user_name = "";
                                            try {
                                                if (jObjx["stat"].ToString() == "ok") {
                                                    user_id = jObjx["user"]["id"].ToString();
                                                    user_name = unicode_to_string(jObjx["user"]["username"]["_content"].ToString());
                                                    append_msg_THREAD("用戶名：" + user_name + "\n用戶ID：" + user_id, color_f_w, bg_color_exe);
                                                } else {
                                                    append_msg_THREAD("資訊錯誤！Error! Can't get user info.\n" + jObjx["message"].ToString(), Color.Red, Color.Yellow);
                                                    continue;
                                                }
                                            } catch (Exception) {
                                                append_msg_THREAD("資訊錯誤！Error! Can't get user info.", Color.Red, Color.Yellow);
                                                continue;
                                            }
                                            if (error == false) {
                                                json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.favorites.getList&api_key=" + api_key + "&user_id=" + user_id + "&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,date_taken,media&per_page=500&page=1&min_fave_date=0&max_fave_date=" + ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds) + "&format=json", signed_auth_bool);
                                                jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                if (jObj["stat"].ToString() != "ok") {
                                                    append_msg_THREAD("資訊錯誤！Error! Can't get info.\n" + jObj["message"].ToString(), Color.Red, Color.Yellow);
                                                    continue;
                                                }
                                                append_msg_THREAD("共 " + jObj["photos"]["total"].ToString() + " 張相片(Total:" + jObj["photos"]["total"].ToString() + ")", color_f_w, bg_color_exe);
                                                if (options_2_auto_make_dirs) {
                                                    string temp_dirname = filename_filter(user_name + " - Favorite").Trim(' ');
                                                    if (System.IO.Directory.Exists(save_dir + "\\" + temp_dirname) && read_from_save_file != true && options_6_skip_download != true) {
                                                        temp_dirname = find_new_dir_name(temp_dirname, save_dir);
                                                        try {
                                                            System.IO.Directory.CreateDirectory(save_dir + "\\" + temp_dirname);
                                                            append_msg_THREAD("創建資料夾成功(Success)！名稱(dir name)：" + save_dir + "\\" + temp_dirname, color_f_g, bg_color_exe);
                                                        } catch (Exception) {
                                                            append_msg_THREAD("錯誤無法創資料夾！Error! Can't create dir.", Color.Red, Color.Yellow);
                                                        }
                                                    }
                                                    album_name_X = temp_dirname;
                                                } else {
                                                    album_name_X = "";
                                                }
                                                int total_photo = int.Parse(jObj["photos"]["total"].ToString());
                                                int total_pages = (int)Math.Ceiling((decimal)total_photo / 500);
                                                photo_name = new string[total_photo];
                                                photo_url = new string[total_photo];
                                                append_msg_THREAD("已下載第1頁/共" + total_pages + "頁", color_f_w, bg_color_exe);
                                                for (int ix = 0; ix < 500; ix++) {
                                                    if ((ix) >= total_photo) {
                                                        break;
                                                    }
                                                    try {
                                                        if (options_1_orginal_names == 1) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 0) {
                                                            temp_title = (ix + 1).ToString();
                                                        } else if (options_1_orginal_names == 2) {
                                                            temp_title = jObj["photos"]["photo"][ix]["id"].ToString();
                                                        } else if (options_1_orginal_names == 3) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 4) {
                                                            try {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 5) {
                                                            try {
                                                                temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 6) {
                                                            try {
                                                                temp_title = album_name_X + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 7) {
                                                            try {
                                                                temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 8) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 9) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 10) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 11) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 12) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 13) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 14) {
                                                            try {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 15) {
                                                            try {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 16) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        }
                                                        string owner_id = "";
                                                        try {
                                                            owner_id = ((JObject)jObj["photos"]["photo"][ix])["owner"].ToString();
                                                        } catch (Exception) {
                                                            
                                                        }
                                                        temp_url = get_url((JObject)jObj["photos"]["photo"][ix], false, owner_id);
                                                        if (exe_show_detail_enabled) {
                                                            append_msg_THREAD("(" + (ix + 1) + "/" + total_photo + ")" + temp_title + "\n" + temp_url, color_f_w, bg_color_exe);
                                                        }
                                                        photo_name[ix] = temp_title;
                                                        photo_url[ix] = temp_url;
                                                    } catch {

                                                    }
                                                }
                                                for (int page = 2; page <= total_pages; page++) {
                                                    if (pause_download) {
                                                        pause();
                                                    }
                                                    if (download_started == false) {
                                                        break;
                                                    }
                                                    json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.favorites.getList&api_key=" + api_key + "&user_id=" + user_id + "&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,date_taken,media&per_page=500&page=" + page + "&min_fave_date=0&max_fave_date=" + ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds) + "&format=json", signed_auth_bool);
                                                    jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                    append_msg_THREAD("已下載第" + page + "頁/共" + total_pages + "頁", color_f_w, bg_color_exe);
                                                    for (int ix = 0; ix < 500; ix++) {
                                                        if ((ix + ((page - 1) * 500)) >= total_photo) {
                                                            break;
                                                        }
                                                        try {
                                                            if (options_1_orginal_names == 1) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 0) {
                                                                temp_title = (ix + 1 + ((page - 1) * 500)).ToString();
                                                            } else if (options_1_orginal_names == 2) {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } else if (options_1_orginal_names == 3) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 4) {
                                                                try {
                                                                    temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 5) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 6) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 7) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 8) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 9) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 10) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 11) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 12) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 13) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 14) {
                                                                try {
                                                                    temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 15) {
                                                                try {
                                                                    temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 16) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            }
                                                            string owner_id = "";
                                                            try {
                                                                owner_id = ((JObject)jObj["photos"]["photo"][ix])["owner"].ToString();
                                                            } catch (Exception) {

                                                            }
                                                            temp_url = get_url((JObject)jObj["photos"]["photo"][ix], false, owner_id);
                                                            if (exe_show_detail_enabled) {
                                                                append_msg_THREAD("(" + (ix + 1 + ((page - 1) * 500)) + "/" + total_photo + ")" + temp_title + "\n" + temp_url, color_f_w, bg_color_exe);
                                                            }
                                                            photo_name[ix + ((page - 1) * 500)] = temp_title;
                                                            photo_url[ix + ((page - 1) * 500)] = temp_url;
                                                        } catch {

                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 7://藝術家全部的相片
                                            error = false;
                                            string jsonx2_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.urls.lookupUser&api_key=" + api_key + "&url=" + resolve_user_id(url_[i]) + "&format=json", signed_auth_bool);
                                            JObject jObjx2 = (JObject)JsonConvert.DeserializeObject(jsonx2_str);//dynamic
                                            string user_id_ = "";
                                            string user_name_ = "";
                                            try {
                                                if (jObjx2["stat"].ToString() == "ok") {
                                                    user_id_ = jObjx2["user"]["id"].ToString();
                                                    user_name_ = unicode_to_string(jObjx2["user"]["username"]["_content"].ToString());
                                                    append_msg_THREAD("用戶名：" + user_name_ + "\n用戶ID：" + user_id_, color_f_w, bg_color_exe);
                                                } else {
                                                    append_msg_THREAD("資訊錯誤！Error! Can't get user info.\n" + jObjx2["message"].ToString(), Color.Red, Color.Yellow);
                                                    continue;
                                                }
                                            } catch (Exception) {
                                                append_msg_THREAD("資訊錯誤！Error! Can't get user info.", Color.Red, Color.Yellow);
                                                continue;
                                            }
                                            if (error == false) {
                                                json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.people.getPhotos&api_key=" + api_key + "&user_id=" + user_id_ + "&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,date_taken,media&per_page=500&page=1&min_upload_date=0&content_type=7&max_upload_date=" + ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds) + "&format=json", signed_auth_bool);
                                                jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                if (jObj["stat"].ToString() != "ok") {
                                                    append_msg_THREAD("資訊錯誤！Error! Can't get info.\n" + jObj["message"].ToString(), Color.Red, Color.Yellow);
                                                    continue;
                                                }
                                                append_msg_THREAD("共 " + jObj["photos"]["total"].ToString() + " 張相片(Total:" + jObj["photos"]["total"].ToString() + ")", color_f_w, bg_color_exe);
                                                if (options_2_auto_make_dirs) {
                                                    string temp_dirname = filename_filter(user_name_ + " - Photos").Trim(' ');
                                                    if (System.IO.Directory.Exists(save_dir + "\\" + temp_dirname) && read_from_save_file != true && options_6_skip_download != true) {
                                                        temp_dirname = find_new_dir_name(temp_dirname, save_dir);
                                                        try {
                                                            System.IO.Directory.CreateDirectory(save_dir + "\\" + temp_dirname);
                                                            append_msg_THREAD("創建資料夾成功(Success)！名稱(dir name)：" + save_dir + "\\" + temp_dirname, color_f_g, bg_color_exe);
                                                        } catch (Exception) {
                                                            append_msg_THREAD("錯誤無法創資料夾！Error! Can't create dir.", Color.Red, Color.Yellow);
                                                        }
                                                    }
                                                    album_name_X = temp_dirname;
                                                } else {
                                                    album_name_X = "";
                                                }
                                                int total_photo = int.Parse(jObj["photos"]["total"].ToString());
                                                int total_pages = (int)Math.Ceiling((decimal)total_photo / 500);
                                                photo_name = new string[total_photo];
                                                photo_url = new string[total_photo];
                                                append_msg_THREAD("已下載第1頁/共" + total_pages + "頁", color_f_w, bg_color_exe);
                                                for (int ix = 0; ix < 500; ix++) {
                                                    if ((ix) >= total_photo) {
                                                        break;
                                                    }
                                                    try {
                                                        if (options_1_orginal_names == 1) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 0) {
                                                            temp_title = (ix + 1).ToString();
                                                        } else if (options_1_orginal_names == 2) {
                                                            temp_title = jObj["photos"]["photo"][ix]["id"].ToString();
                                                        } else if (options_1_orginal_names == 3) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 4) {
                                                            try {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 5) {
                                                            try {
                                                                temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 6) {
                                                            try {
                                                                temp_title = album_name_X + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 7) {
                                                            try {
                                                                temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 8) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 9) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 10) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 11) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 12) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 13) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 14) {
                                                            try {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 15) {
                                                            try {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 16) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        }
                                                        string owner_id = "";
                                                        try {
                                                            owner_id = ((JObject)jObj["photos"]["photo"][ix])["owner"].ToString();
                                                        } catch (Exception) {

                                                        }
                                                        temp_url = get_url((JObject)jObj["photos"]["photo"][ix], false, owner_id);
                                                        if (exe_show_detail_enabled) {
                                                            append_msg_THREAD("(" + (ix + 1) + "/" + total_photo + ")" + temp_title + "\n" + temp_url, color_f_w, bg_color_exe);
                                                        }
                                                        photo_name[ix] = temp_title;
                                                        photo_url[ix] = temp_url;
                                                    } catch {

                                                    }
                                                }
                                                for (int page = 2; page <= total_pages; page++) {
                                                    if (pause_download) {
                                                        pause();
                                                    }
                                                    if (download_started == false) {
                                                        break;
                                                    }
                                                    json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.people.getPhotos&api_key=" + api_key + "&user_id=" + user_id_ + "&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,date_taken,media&per_page=500&page=" + page + "&min_upload_date=0&content_type=7&max_upload_date=" + ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds) + "&format=json", signed_auth_bool);
                                                    jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                    append_msg_THREAD("已下載第" + page + "頁/共" + total_pages + "頁", color_f_w, bg_color_exe);
                                                    for (int ix = 0; ix < 500; ix++) {
                                                        if ((ix + ((page - 1) * 500)) >= total_photo) {
                                                            break;
                                                        }
                                                        try {
                                                            if (options_1_orginal_names == 1) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 0) {
                                                                temp_title = (ix + 1 + ((page - 1) * 500)).ToString();
                                                            } else if (options_1_orginal_names == 2) {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } else if (options_1_orginal_names == 3) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 4) {
                                                                try {
                                                                    temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 5) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 6) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 7) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 8) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 9) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 10) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 11) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 12) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 13) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 14) {
                                                                try {
                                                                    temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 15) {
                                                                try {
                                                                    temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 16) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            }
                                                            string owner_id = "";
                                                            try {
                                                                owner_id = ((JObject)jObj["photos"]["photo"][ix])["owner"].ToString();
                                                            } catch (Exception) {

                                                            }
                                                            temp_url = get_url((JObject)jObj["photos"]["photo"][ix], false, owner_id);
                                                            if (exe_show_detail_enabled) {
                                                                append_msg_THREAD("(" + (ix + 1 + ((page - 1) * 500)) + "/" + total_photo + ")" + temp_title + "\n" + temp_url, color_f_w, bg_color_exe);
                                                            }
                                                            photo_name[ix + ((page - 1) * 500)] = temp_title;
                                                            photo_url[ix + ((page - 1) * 500)] = temp_url;
                                                        } catch {

                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 1://單或多照片
                                            photo_name = new string[url_.Length];
                                            photo_url = new string[url_.Length];
                                            //相片之內部迴圈
                                            for (int ix = 0; ix < url_.Length; ix++) {
                                                error = false;
                                                Debug.WriteLine("in FOR " + ix);
                                                id = get_photo_id(url_[ix]);
                                                if (id == "0" || id == "") {
                                                    photo_url[ix] = "";
                                                    photo_name[ix] = "";
                                                    error = true;
                                                }
                                                if (error == false) {
                                                    if (pause_download) {
                                                        pause();
                                                    }
                                                    append_msg_THREAD("相片ID：" + id, color_f_w, bg_color_exe);
                                                    json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photos.getSizes&api_key=" + api_key + "&photo_id=" + id + "&format=json", signed_auth_bool);
                                                    jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                    if (jObj["stat"].ToString() != "ok") {
                                                        append_msg_THREAD("資訊錯誤！Error! Can't get info.\n" + jObj["message"].ToString(), Color.Red, Color.Yellow);
                                                        photo_url[ix] = "";
                                                        continue;
                                                    }
                                                    photo_url[ix] = get_url_photo(jObj, false);
                                                    append_msg_THREAD("相片URL：" + photo_url[ix], color_f_w, bg_color_exe);
                                                    if (options_1_orginal_names!=0 && options_1_orginal_names != 2 && options_1_orginal_names != 6) {
                                                        json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.photos.getInfo&api_key=" + api_key + "&photo_id=" + id + "&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,media&format=json", signed_auth_bool);
                                                        jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                        try {
                                                            if (options_1_orginal_names == 1) {
                                                                try {
                                                                    photo_name[ix] = filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ');
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 3) {
                                                                try {
                                                                    photo_name[ix] = filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ') + " - " + id;
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 4) {
                                                                try {
                                                                    photo_name[ix] = id + " - " + filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ');
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 5) {
                                                                try {
                                                                    photo_name[ix] = filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ') + " - " + id;
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 7) {
                                                                try {
                                                                    photo_name[ix] = filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ');
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 8) {
                                                                try {
                                                                    photo_name[ix] =  get_time_by_str_and_type(jObj["photo"]["dates"]["taken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ') + " - " + id;
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 9) {
                                                                try {
                                                                    photo_name[ix] = get_time_by_str_and_type(jObj["photo"]["dates"]["taken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ');
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 10) {
                                                                try {
                                                                    photo_name[ix] = get_time_by_str_and_type(jObj["photo"]["dates"]["taken"].ToString()) + " - " + id;
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 11) {
                                                                try {
                                                                    photo_name[ix] = filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ') + " - " + id + " - " + get_time_by_str_and_type(jObj["photo"]["dates"]["taken"].ToString());
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 12) {
                                                                try {
                                                                    photo_name[ix] = filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photo"]["dates"]["taken"].ToString()) + " - " + id;
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 13) {
                                                                try {
                                                                    photo_name[ix] = filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photo"]["dates"]["taken"].ToString());
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 14) {
                                                                try {
                                                                    photo_name[ix] = id + " - " + get_time_by_str_and_type(jObj["photo"]["dates"]["taken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photo"]["title"]["_content"].ToString())).Trim(' ');
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 15) {
                                                                try {
                                                                    photo_name[ix] = id + " - " + get_time_by_str_and_type(jObj["photo"]["dates"]["taken"].ToString());
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 16) {
                                                                try {
                                                                    photo_name[ix] = get_time_by_str_and_type(jObj["photo"]["dates"]["taken"].ToString());
                                                                } catch {
                                                                    photo_name[ix] = "未命名 NoName";
                                                                }
                                                            }
                                                        } catch (Exception) {
                                                            photo_name[ix] = (ix + 1).ToString();
                                                        }
                                                        append_msg_THREAD("相片標題(title)：" + photo_name[ix], color_f_w, bg_color_exe);
                                                        try {
                                                            append_msg_THREAD("相片擁有者(owner)：" + unicode_to_string(jObj["photo"]["owner"]["username"].ToString()) + "\n擁有者ID(ownerID)：" + jObj["photo"]["owner"]["nsid"].ToString(), color_f_w, bg_color_exe);
                                                        } catch (Exception) {

                                                        }
                                                    } else if (options_1_orginal_names == 0) {
                                                        photo_name[ix] = (ix + 1).ToString();
                                                        append_msg_THREAD("相片標題(title)：" + photo_name[ix], color_f_w, bg_color_exe);
                                                    } else if (options_1_orginal_names == 2) {
                                                        photo_name[ix] = id;
                                                        append_msg_THREAD("相片標題(title)：" + id, color_f_w, bg_color_exe);
                                                    } else if (options_1_orginal_names == 6) {
                                                        photo_name[ix] = id;
                                                        append_msg_THREAD("相片標題(title)：" + id, color_f_w, bg_color_exe);
                                                    }
                                                }
                                                //toolStripProgressBar3.Value = (ix + 1);
                                                setToolStripProgressBar_value(toolStripProgressBar3, (ix + 1));
                                            }
                                            if (photo_url.Length != 0) {
                                                append_msg_THREAD("分析完成，開始下載！(Start Downloading!)", color_f_m, bg_color_exe);
                                            }
                                            break;
                                        case 4://群組
                                            string group_name = "null";
                                            error = false;
                                            id = get_group_id(url_[i], ref group_name);
                                            if (id == "0") {
                                                error = true;
                                            }
                                            if (error == false) {
                                                append_msg_THREAD("群組ID：" + id, color_f_w, bg_color_exe);
                                                if (options_2_auto_make_dirs && group_name == "null") {
                                                    append_msg_THREAD("查詢Group Name..." + url_[i], color_f_w, bg_color_exe);
                                                    json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.groups.getInfo&api_key=" + api_key + "&group_id=" + id + "&format=json", signed_auth_bool);
                                                    jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                    if (jObj["stat"].ToString() != "ok") {
                                                        append_msg_THREAD("資訊錯誤！Error! Can't get info.\n" + jObj["message"].ToString(), Color.Red, Color.Yellow);
                                                        continue;
                                                    } else {
                                                        group_name = unicode_to_string(jObj["group"]["name"]["_content"].ToString());
                                                        append_msg_THREAD("群組名：" + group_name, color_f_w, bg_color_exe);
                                                    }
                                                }
                                                if (options_2_auto_make_dirs) {
                                                    string temp_dirname = filename_filter(group_name).Trim(' ');
                                                    if (System.IO.Directory.Exists(save_dir + "\\" + temp_dirname) && read_from_save_file != true && options_6_skip_download != true) {
                                                        temp_dirname = find_new_dir_name(temp_dirname, save_dir);
                                                        try {
                                                            System.IO.Directory.CreateDirectory(save_dir + "\\" + temp_dirname);
                                                            append_msg_THREAD("創建資料夾成功(Success)！名稱(dir name)：" + save_dir + "\\" + temp_dirname, color_f_g, bg_color_exe);
                                                        } catch (Exception) {
                                                            append_msg_THREAD("錯誤無法創資料夾！Error! Can't create dir.", Color.Red, Color.Yellow);
                                                        }
                                                    }
                                                    album_name_X = temp_dirname;
                                                } else {
                                                    album_name_X = "";
                                                }
                                                json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.groups.pools.getPhotos&api_key=" + api_key + "&group_id=" + id + "&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,date_taken,media&per_page=500&format=json", signed_auth_bool);
                                                jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                if (jObj["stat"].ToString() != "ok") {
                                                    append_msg_THREAD("資訊錯誤！Error! Can't get info.\n" + jObj["message"].ToString(), Color.Red, Color.Yellow);
                                                    continue;
                                                }
                                                append_msg_THREAD("共 " + jObj["photos"]["total"].ToString() + " 張相片(Total:" + jObj["photos"]["total"].ToString() + ")", color_f_w, bg_color_exe);
                                                int total_photo = int.Parse(jObj["photos"]["total"].ToString());
                                                int total_pages = (int)Math.Ceiling((decimal)total_photo / 500);
                                                photo_name = new string[total_photo];
                                                photo_url = new string[total_photo];
                                                append_msg_THREAD("已下載第1頁/共" + total_pages + "頁", color_f_w, bg_color_exe);
                                                for (int ix = 0; ix < 500; ix++) {
                                                    if (ix >= total_photo) {
                                                        break;
                                                    }
                                                    try {
                                                        if (options_1_orginal_names == 1) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 0) {
                                                            temp_title = (ix).ToString();
                                                        } else if (options_1_orginal_names == 2) {
                                                            temp_title = jObj["photos"]["photo"][ix]["id"].ToString();
                                                        } else if (options_1_orginal_names == 3) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 4) {
                                                            try {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 5) {
                                                            try {
                                                                temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 6) {
                                                            try {
                                                                temp_title = album_name_X + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 7) {
                                                            try {
                                                                temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 8) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 9) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 10) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 11) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 12) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 13) {
                                                            try {
                                                                temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 14) {
                                                            try {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 15) {
                                                            try {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        } else if (options_1_orginal_names == 16) {
                                                            try {
                                                                temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                            } catch {
                                                                temp_title = "未命名 NoName";
                                                            }
                                                        }
                                                        string owner_id = "";
                                                        try {
                                                            owner_id = ((JObject)jObj["photos"]["photo"][ix])["owner"].ToString();
                                                        } catch (Exception) {

                                                        }
                                                        temp_url = get_url((JObject)jObj["photos"]["photo"][ix], false, owner_id);
                                                        if (exe_show_detail_enabled) {
                                                            append_msg_THREAD("(" + (ix + 1) + "/" + total_photo + ")" + temp_title + "\n" + temp_url, color_f_w, bg_color_exe);
                                                        }
                                                        photo_name[ix] = temp_title;
                                                        photo_url[ix] = temp_url;
                                                    } catch {

                                                    }
                                                }
                                                for (int page = 2; page <= total_pages; page++) {
                                                    if (pause_download) {
                                                        pause();
                                                    }
                                                    if (download_started == false) {
                                                        break;
                                                    }
                                                    json_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.groups.pools.getPhotos&api_key=" + api_key + "&group_id=" + id + "&page=" + page + "&per_page=500&extras=url_o,url_6k,url_5k,url_4k,url_3k,url_k,url_h,url_l,url_c,url_z,url_m,url_n,url_s,date_taken,media&format=json", signed_auth_bool);
                                                    jObj = (JObject)JsonConvert.DeserializeObject(json_str);//dynamic
                                                    append_msg_THREAD("已下載第" + page + "頁/共" + total_pages + "頁", color_f_w, bg_color_exe);
                                                    for (int ix = 0; ix < 500; ix++) {
                                                        if (pause_download) {
                                                            pause();
                                                        }
                                                        if (download_started == false) {
                                                            break;
                                                        }
                                                        if ((ix + ((page - 1) * 500)) >= total_photo) {
                                                            break;
                                                        }
                                                        try {
                                                            if (options_1_orginal_names == 1) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 0) {
                                                                temp_title = (ix + 1 + ((page - 1) * 500)).ToString();
                                                            } else if (options_1_orginal_names == 2) {
                                                                temp_title = jObj["photos"]["photo"][ix]["id"].ToString();
                                                            } else if (options_1_orginal_names == 3) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 4) {
                                                                try {
                                                                    temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 5) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 6) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 7) {
                                                                try {
                                                                    temp_title = album_name_X + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 8) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 9) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 10) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 11) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 12) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + jObj["photos"]["photo"][ix]["id"].ToString();
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 13) {
                                                                try {
                                                                    temp_title = filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ') + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 14) {
                                                                try {
                                                                    temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString()) + " - " + filename_filter(unicode_to_string(jObj["photos"]["photo"][ix]["title"].ToString())).Trim(' ');
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 15) {
                                                                try {
                                                                    temp_title = jObj["photos"]["photo"][ix]["id"].ToString() + " - " + get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            } else if (options_1_orginal_names == 16) {
                                                                try {
                                                                    temp_title = get_time_by_str_and_type(jObj["photos"]["photo"][ix]["datetaken"].ToString());
                                                                } catch {
                                                                    temp_title = "未命名 NoName";
                                                                }
                                                            }
                                                        } catch {

                                                        }
                                                        try {
                                                            string owner_id = "";
                                                            try {
                                                                owner_id = ((JObject)jObj["photos"]["photo"][ix])["owner"].ToString();
                                                            } catch (Exception) {

                                                            }
                                                            temp_url = get_url((JObject)jObj["photos"]["photo"][ix], false, owner_id);
                                                            if (exe_show_detail_enabled) {
                                                                append_msg_THREAD("(" + (ix + 1 + ((page - 1) * 500)) + "/" + total_photo + ")" + temp_title + "\n" + temp_url, color_f_w, bg_color_exe);
                                                            }
                                                            photo_name[ix + ((page - 1) * 500)] = temp_title;
                                                            photo_url[ix + ((page - 1) * 500)] = temp_url;
                                                        } catch {
                                                            temp_url = "";
                                                        }
                                                    }
                                                }

                                            }
                                            break;
                                        case 6://人工挑選多個相簿內的相片
                                            photo_name = new string[url_.Length];
                                            photo_url = new string[url_.Length];
                                            string[] temp_arr = { };
                                            for (int ix = 0; ix < url_.Length; ix++) {
                                                temp_arr = split_two_piece(url_[ix], "\r\n");
                                                photo_url[ix] = temp_arr[0];
                                                photo_name[ix] = temp_arr[1];
                                            }
                                            break;
                                    }
                                    break;
                                //Search Tab START
                                case 1:
                                    //人工挑選搜尋之相片
                                    photo_name = new string[url_.Length];
                                    photo_url = new string[url_.Length];
                                    string[] temp_arr2 = { };
                                    for (int ix = 0; ix < url_.Length; ix++) {
                                        temp_arr2 = split_two_piece(url_[ix], "\r\n");
                                        photo_url[ix] = temp_arr2[0];
                                        photo_name[ix] = temp_arr2[1];
                                    }
                                    break;
                            }
                        }
                    } catch (Exception ex) {
                        Debug.WriteLine(ex.ToString());
                    }

                    int remove_count = 0;
                    check_url_for_pre_download_step(ref photo_name, ref photo_url, ref remove_count);

                    if (remove_count != 0) {
                        append_msg_THREAD("錯誤，有" + remove_count + "組URL不合法！Error!" + remove_count + " url is incorrect!", Color.Red, Color.Yellow);
                    }

                    live_X = new bool[photo_url.Length];
                    retry_times = new int[photo_url.Length];
                    thread_download_number = new int[] { 0, 0, 0, 0 };
                    set_all_False(ref live_X);
                    set_all_False(ref thread_busy);
                    set_all_Zero(ref retry_times);
                    //timer1.Start();
                    timer1_START = true;

                    photo_url_i_ARR_save = (string[])photo_url.Clone();
                    photo_name_i_ARR_save = (string[])photo_name.Clone();
                    album_name_X_SAVE = album_name_X.Replace("|", "-");

                    setToolStripProgressBar_max(toolStripProgressBar2, photo_url.Length);
                    //toolStripProgressBar2.Maximum = photo_url.Length;

                    for (int i2 = select_if_new_save_i(2); i2 < photo_url.Length; i2++) {
                        if (pause_download) {
                            pause();
                        }
                        if (!download_started) {
                            append_msg_THREAD("\n取消 Cancel\n\n", color_f_w, bg_color_exe);
                            break;
                        }
                        string total_amount = prog_corrct(i2, photo_url.Length, i, url_.Length);

                        if (photo_url[i2] == "") {
                            append_msg_THREAD("下載失敗(FAIL)(" + total_amount + ")" + photo_name[i2], Color.Red, Color.Yellow);
                            continue;
                        }

                        //DOWNLOAD FUNC
                        int free_thread = switch_get_NOT_busy();
                        while (free_thread == (-1)) {
                            if (!download_started) {
                                break;
                            }
                            if (pause_download) {
                                pause();
                            }
                            try {
                                System.Threading.Thread.Sleep(10);
                                //Application.DoEvents();
                            } catch (NullReferenceException ex) {
                                Debug.WriteLine(ex.ToString());
                            }
                            free_thread = switch_get_NOT_busy();
                        }
                        if (free_thread != (-1)) {
                            thread_busy[free_thread] = true;
                            thread_download_number[free_thread] = i2;
                            while (s[free_thread] != null && s[free_thread].IsAlive) {
                                System.Threading.Thread.Sleep(1);
                            }
                            s[free_thread] = null;
                            string temp_url_X = photo_url[i2].Clone().ToString();
                            s[free_thread] = new System.Threading.Thread(() => download_file_and_save(temp_url_X, free_thread, album_name_X, photo_name, false));
                            s[free_thread].Start();
                            append_msg_THREAD("下載中..(Downloading..)(" + total_amount + ")" + photo_name[i2], color_f_w, bg_color_exe);
                        }

                        if ((photo_url.Length - 1) == i2) {
                            Debug.WriteLine("Waiting");
                            while (switch_get_busy_count() != 0) {
                                //Application.DoEvents();
                                System.Threading.Thread.Sleep(10);
                                if (pause_download) {
                                    pause();
                                }
                                if (!download_started) {
                                    break;
                                }
                            }
                        }

                        setToolStripProgressBar_value(toolStripProgressBar2, (i2 + 1));
                        setToolStripProgressBar_tooltiptext(toolStripProgressBar2, "(" + (i2 + 1) + "/" + toolStripProgressBar2.Maximum + ")");
                        set_status_taskbar((i2 + 1), photo_url.Length, (i + 1), prog_total_max);
                        read_from_save_file = false;
                    }

                    photo_name = null;
                    photo_url = null;
                    GC.Collect();

                    Debug.WriteLine("Finished");

                    timer1_STOP = true;
                    setToolStripProgressBar_value(toolStripProgressBar3, (i + 1));
                    //toolStripProgressBar3.Value = (i + 1);
                    setToolStripProgressBar_tooltiptext(toolStripProgressBar3, "(" + (i + 1) + "/" + toolStripProgressBar3.Maximum + ")");
                    //toolStripProgressBar3.ToolTipText = "(" + (i + 1) + "/" + toolStripProgressBar3.Maximum + ")";

                    if (tabs_==0 && (types_ == 1 || types_==6)) {
                        //單張或多張相片不需要跑外圍迴圈，內部迴圈直接跑完。
                        setToolStripProgressBar_value(toolStripProgressBar3, toolStripProgressBar3.Maximum);
                        setToolStripProgressBar_tooltiptext(toolStripProgressBar3, "(" + toolStripProgressBar3.Maximum + "/" + toolStripProgressBar3.Maximum + ")");
                        break;
                    }
                    if (tabs_ == 1 ) {
                        //搜尋相片不需要跑外圍迴圈，內部迴圈直接跑完。
                        setToolStripProgressBar_value(toolStripProgressBar3, toolStripProgressBar3.Maximum);
                        setToolStripProgressBar_tooltiptext(toolStripProgressBar3, "(" + toolStripProgressBar3.Maximum + "/" + toolStripProgressBar3.Maximum + ")");
                        break;
                    }

                }

                set_status_finish();

                switch (tabs_) {
                    case 0:
                        //Tab 1
                        //Tab1 UNLOCK START
                        setTextboxReadonly(tab1_textBox1, false);
                        setRadioboxEnabled(tab1_radioButton1, true);
                        setRadioboxEnabled(tab1_radioButton2, true);
                        setRadioboxEnabled(tab1_radioButton3, true);
                        setRadioboxEnabled(tab1_radioButton4, true);
                        setRadioboxEnabled(tab1_radioButton5, true);
                        setRadioboxEnabled(tab1_radioButton6, true);
                        setRadioboxEnabled(tab1_radioButton7, true);
                        setRadioboxEnabled(tab1_radioButton8, true);
                        //Tab1 UNLOCK END
                        break;
                    case 1:
                        //Tab2
                        //Tab2 UNLOCK START
                        setTextboxReadonly(tab2_search_textbox, false);
                        setTextboxReadonly(tab2_search_userid_textbox, false);
                        setTextboxReadonly(tab2_search_groupid_textbox, false);
                        setComboBoxEnabled(tab2_userid_selectbox_type, true);
                        setComboBoxEnabled(tab2_userid_selectbox_privacyfilter, true);
                        setComboBoxEnabled(tab2_safesearch_selectbox, true);
                        setNumericUpDownEnabled(tab2_search_per_page_obj, true);
                        setNumericUpDownEnabled(tab2_search_start_page_obj, true);
                        setNumericUpDownEnabled(tab2_search_fetch_page_obj, true);
                        //Tab2 UNLOCK END
                        break;
                }

                setToolStripSpringTextBoxEnabled(toolStripTextBox1, true);
                setCheckboxEnabled(auto_making_dir_setting_obj, true);
                setCheckboxEnabled(video_download_setting_obj, true);
                //setCheckboxEnabled(preview_photos_setting_obj, true);
                setCheckboxEnabled(skip_download_setting_obj, true);
                setComboBoxEnabled(photos_sizes_setting_obj, true);
                setComboBoxEnabled(file_name_setting_obj, true);

                setButtonText(button1, "下載 Download");

                download_started = false;
                read_from_save_file = false;

                unlock_all_tab(tabControl1);

                setStatus_icon(this, notifyIcon1, false, this_app_exist_number);

                if (System.IO.File.Exists(Application.StartupPath + "\\auto_save.fpsav")) {
                    try {
                        System.IO.File.Delete(Application.StartupPath + "\\auto_save.fpsav");
                    } catch {

                    }
                }
                try {
                    if (exe_sound_enabled) {
                        snd.Play();
                    }
                    append_msg_THREAD("下載完成！Download Complete.(錯誤數：" + fail_list_name.Count + ")", Color.Cyan, bg_color_exe);
                    if (fail_list_name.Count!=0) {
                        string value_to_show = "錯誤清單：(" + fail_list_name.Count + " 個)\r\n\r\n";
                        for (int i=0;i< fail_list_name.Count;i++) {
                            value_to_show = value_to_show + (i+1).ToString() + " - \r\n" + fail_list_name[i] + "\r\n" + fail_list_url[i] + "\r\n\r\n";
                        }
                        msgbox_nonBlock(value_to_show, "Error List");
                    }
                } catch {

                }

                fail_list_name.Clear();
                fail_list_url.Clear();
            }
        }

        public string url_type_auto_correct(string[] url_list) {
            int[] type_count = new int[] { 0, 0, 0 };
            for (int i = 0; i < url_list.Length; i++) {
                if ((url_list[i].IndexOf("/sets/") >= 0 || url_list[i].IndexOf("/albums/") >= 0 || url_list[i].IndexOf("flic.kr/s/") >= 0) && url_list[i].IndexOf("/in/album")<0) {
                    type_count[0] += 1;
                } else if ((url_list[i].IndexOf("flic.kr/p/") >= 0 || url_list[i].IndexOf("/photos/") >=0) && url_list[i].IndexOf("/in/album") < 0) {
                    type_count[1] += 1;
                } else if (url_list[i].IndexOf("/groups/") >= 0 || url_list[i].IndexOf("flic.kr/g/") >= 0 || url_list[i].IndexOf("/pool/") >= 0) {
                    type_count[2] += 1;
                }
            }
            int max_v = type_count.Max();
            if (max_v==0) {
                return "unknow";
            }
            for (int i=0;i<type_count.Length;i++) {
                if (type_count[i]==max_v) {
                    if ( i == 0 ) {
                        return "albums";
                    } else if ( i == 1 ) {
                        return "photos";
                    } else if ( i == 2 ) {
                        return "groups";
                    }
                }
            }
            return "unknow";
        }

        public void lock_all_tab(int current_tab) {
            for (int i=0;i<tabControl1.TabPages.Count;i++) {
                if (i!=current_tab) {
                    tabControl1.TabPages[i].Enabled = false;
                } else {
                    tabControl1.TabPages[i].Enabled = true;
                }
            }
            tabControl1.Refresh();
            Application.DoEvents();
        }

        public static void unlock_all_tab(TabControl buttonx) {
            Action append = () => unlock_all_tab_X(buttonx);
            if (buttonx.InvokeRequired) {
                buttonx.Invoke(append);
            } else {
                append();
            }
        }

        public static void unlock_all_tab_X(TabControl buttonx) {
            for (int i = 0; i < buttonx.TabPages.Count; i++) {
                buttonx.TabPages[i].Enabled = true;
            }
            buttonx.Refresh();
        }

        public void unlock_all_tab() {
            for (int i = 0; i < tabControl1.TabPages.Count; i++) {
                tabControl1.TabPages[i].Enabled = true;
            }
            tabControl1.Refresh();
        }

        private void button1_Click ( object sender, EventArgs e ) {
            if ( download_started ) {
                //開始下載了，將要結束

                auto_making_dir_setting_obj.Enabled = true;
                //preview_photos_setting_obj.Enabled = true;
                skip_download_setting_obj.Enabled = true;
                video_download_setting_obj.Enabled = true;
                photos_sizes_setting_obj.Enabled = true;
                file_name_setting_obj.Enabled = true;
                toolStripTextBox1.Enabled = true;

                switch (tabs_) {
                    case 0:
                        //Tab 1
                        //Tab1 UNLOCK START
                        tab1_textBox1.ReadOnly = false;
                        tab1_radioButton1.Enabled = true;
                        tab1_radioButton2.Enabled = true;
                        tab1_radioButton3.Enabled = true;
                        tab1_radioButton4.Enabled = true;
                        tab1_radioButton5.Enabled = true;
                        tab1_radioButton6.Enabled = true;
                        tab1_radioButton7.Enabled = true;
                        tab1_radioButton8.Enabled = true;
                        //Tab1 UNLOCK END
                        break;
                    case 1:
                        //Tab2
                        //Tab2 UNLOCK START
                        tab2_safesearch_selectbox.Enabled = true;
                        tab2_search_fetch_page_obj.Enabled = true;
                        tab2_search_groupid_textbox.ReadOnly = false;
                        tab2_search_per_page_obj.Enabled = true;
                        tab2_search_start_page_obj.Enabled = true;
                        tab2_search_textbox.ReadOnly = false;
                        tab2_search_userid_textbox.ReadOnly = false;
                        tab2_userid_selectbox_privacyfilter.Enabled = true;
                        tab2_userid_selectbox_type.Enabled = true;
                        //Tab2 UNLOCK END
                        break;
                }

                button1.Text = "下載 Download";
                read_from_save_file = false;

                setStatus_icon(this, notifyIcon1, false, this_app_exist_number);
                timer1.Stop();
                if ( System.IO.File.Exists(Application.StartupPath + "\\auto_save.fpsav") ) {
                    try {
                        System.IO.File.Delete(Application.StartupPath + "\\auto_save.fpsav");
                    } catch {

                    }
                }
                
                download_started = false;

                unlock_all_tab();

            } else {//===================================================================================================

                //還沒下載，將要開始

                //確認存檔路徑
                if (read_from_save_file) {
                    if (save_dir == "") {
                        if (cfbd.ShowDialog(this) != DialogResult.OK) {
                            return;
                        }
                    }
                } else {
                    if (exe_remember_save_path_enabled) {
                        cfbd.Title = "(將記住)選擇存檔資料夾 (Will Remember) Choose a dir to save files.";
                        if (exe_remember_save_path != "" && System.IO.Directory.Exists(exe_remember_save_path)) {
                            cfbd.SelectedPath = exe_remember_save_path;
                        } else {
                            if (cfbd.ShowDialog(this) != DialogResult.OK) {
                                return;
                            }
                            exe_remember_save_path = cfbd.SelectedPath;
                        }
                    } else {
                        cfbd.Title = "選擇存檔資料夾 Choose a dir to save files.";
                        if (cfbd.ShowDialog(this) != DialogResult.OK) {
                            return;
                        }
                    }
                }

                save_dir = cfbd.SelectedPath; //結尾沒有斜線

                switch (tabs_) {
                    case 0:
                        //Tab1 一般
                        if (tab1_textBox1.Text == "") {
                            msgbox_error("錯誤 Error", "請輸入正確的網址！Please input the right URL Address.");
                            return;
                        }

                        string test_url_type = url_type_auto_correct(fast_split(tab1_textBox1.Text, CrLf));

                        if (test_url_type == "albums" && (types_ == 1 || types_ == 4)) {
                            if (MessageBox.Show(this, "請問您是否要下載單/多個相簿？", "網址與下載類別無法匹配成功", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                                tab1_radioButton1.Checked = true;
                                types_ = 0;
                            }
                        } else if (test_url_type == "photos" && (types_ == 0 || types_ == 4 || types_ == 6)) {
                            if (MessageBox.Show(this, "請問您是否要下載單/多張相片？", "網址與下載類別無法匹配成功", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                                tab1_radioButton2.Checked = true;
                                types_ = 1;
                            }
                        } else if (test_url_type == "groups" && (types_ == 0 || types_ == 1 || types_ == 6)) {
                            if (MessageBox.Show(this, "請問您是否要下載單/多個群組？", "網址與下載類別無法匹配成功", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                                tab1_radioButton5.Checked = true;
                                types_ = 4;
                            }
                        }

                        //Tab1 LOCK START
                        tab1_textBox1.ReadOnly = true;
                        tab1_radioButton1.Enabled = false;
                        tab1_radioButton2.Enabled = false;
                        tab1_radioButton3.Enabled = false;
                        tab1_radioButton4.Enabled = false;
                        tab1_radioButton5.Enabled = false;
                        tab1_radioButton6.Enabled = false;
                        tab1_radioButton7.Enabled = false;
                        tab1_radioButton8.Enabled = false;
                        //Tab1 LOCK END

                        break;
                    case 1:
                        //Tab2 搜尋

                        if (tab2_search_textbox.Text == "" && tab2_search_userid_textbox.Text == "" && tab2_search_groupid_textbox.Text == "") {
                            msgbox_error("錯誤 Error", "請輸入正確的搜尋選項！Please input the right options in order to search.");
                            return;
                        }

                        //Tab2 LOCK
                        tab2_safesearch_selectbox.Enabled = false;
                        tab2_search_fetch_page_obj.Enabled = false;
                        tab2_search_groupid_textbox.ReadOnly = true;
                        tab2_search_per_page_obj.Enabled = false;
                        tab2_search_start_page_obj.Enabled = false;
                        tab2_search_textbox.ReadOnly = true;
                        tab2_search_userid_textbox.ReadOnly = true;
                        tab2_userid_selectbox_privacyfilter.Enabled = false;
                        tab2_userid_selectbox_type.Enabled = false;
                        //Tab2 LOCK END
                        break;
                }

                lock_all_tab(tabs_);

                download_started = true;
                pause_download = false;

                auto_making_dir_setting_obj.Enabled = false;
                //preview_photos_setting_obj.Enabled = false;
                skip_download_setting_obj.Enabled = false;
                photos_sizes_setting_obj.Enabled = false;
                video_download_setting_obj.Enabled = false;
                file_name_setting_obj.Enabled = false;
                toolStripTextBox1.Enabled = false;

                button1.Text = "停止 Stop";
                button2.Text = "暫停 Pause";
                setStatus_icon(this, notifyIcon1, true, this_app_exist_number);

                fail_list_name.Clear();
                fail_list_url.Clear();
                set_all_False(ref s_p_running);

                switch (tabs_) {
                    case 0:
                        //Tab 1
                        status_text.Text = "重設URL中…(Reset URL...)";
                        append_msg_THREAD("重設URL中…(Reset URL...)", color_f_w, bg_color_exe);

                        url_ = fast_split(tab1_textBox1.Text, "\r\n");

                        url_ = reset_url(url_);
                        tab1_textBox1.Text = String.Join(CrLf, url_);


                        break;
                    case 1:
                        //Tab 2
                        url_ = new string[] { };

                        search_keyword = tab2_search_textbox.Text;
                        search_group_id = tab2_search_groupid_textbox.Text;
                        search_user_id = tab2_search_userid_textbox.Text;

                        status_text.Text = "尋找開始…(Searching...)";

                        if (search_keyword!="") {
                            append_msg_THREAD("關鍵字(Keyword)：" + search_keyword, color_f_w, bg_color_exe);
                        }
                        if (search_user_id != "") {
                            append_msg_THREAD("過濾用戶(UserFilter)：" + search_user_id, color_f_w, bg_color_exe);
                        }
                        if (search_group_id != "") {
                            append_msg_THREAD("過濾群組(GroupFilter)：" + search_group_id, color_f_w, bg_color_exe);
                        }

                        break;
                }

                append_msg_THREAD("存檔位置(Save Path)：" + save_dir, color_f_w, bg_color_exe);

                //START_RUN_MAIN

                while (main_thread!=null && main_thread.IsAlive) {
                    Application.DoEvents();
                    status_text.Text = "等待上個進度停止…(Waiting...)";
                    System.Threading.Thread.Sleep(25);
                }
                main_thread = new System.Threading.Thread(() => run_main_download_FUNC(save_dir));
                main_thread.Start();

                //END

            }
        }

        public string url_encode(string input) {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"%[a-f0-9]{2}");
            return reg.Replace(HttpUtility.UrlEncode(input), m => m.Value.ToUpperInvariant());
        }

        public string get_sign_hmacsha1(string full_url,string key) {
            int search_target = full_url.IndexOf("?");
            if (search_target<0) {
                Debug.WriteLine("UnSupport URL");
                return "error";
            }
            string head_url = full_url.Substring(0, search_target);
            string parameter_string = full_url.Substring(search_target+1);
            string[] parameter_array = fast_split(parameter_string, "&");
            Array.Sort(parameter_array);
            string urlcode_url = "GET&" + url_encode(head_url) + "&" + url_encode(String.Join("&", parameter_array));
            Debug.WriteLine(urlcode_url);
            return hmac_sha1_en(urlcode_url, key);
        }

        public string prog_corrct (int i2, int i2l,int i, int il) {
            if ( types_ == 1 ) {
                return ((i+1) + "/" + il);
            } else {
                return ( (i2+1) + "/" + i2l );
            }
        }

        public string unsafe_get_group_id ( string url_ , ref string group_name) {
            url_ = resolve_group_id(url_);
            group_name = "null";
            string group_id = "";
            int ver1 = url_.IndexOf("flickr.com/groups/");
            int ver2 = url_.IndexOf("flic.kr/g/");
            int ver3 = url_.IndexOf("/pool/");
            if ( ver3 >= 0 ) {
                url_ = url_.Substring(0, (ver3+1));
            }
            int s1 = url_.IndexOf("groups/");
            int s2 = url_.IndexOf("@N");
            if ( ver1 < 0 && ver2 < 0 ) {
                group_id = "";
                return group_id;
            }
            if ( ver1 >= 0 && ver2 < 0 ) {
                if ( s1 >= 0 && s2 >= 0 ) {
                    int search_id = url_.IndexOf("/", s1 + 7);
                    if ( search_id >= 0 ) {
                        group_id = url_.Substring(s1 + 7, search_id- s1 - 7);
                    } else {
                        group_id = url_.Substring(s1 + 7);
                    }
                    return group_id;
                } else if ( s1 >= 0 && s2 < 0 ) {
                    append_msg_THREAD("查詢Group ID..." + url_, color_f_w, bg_color_exe);
                    string jsonx_str = get_json_from_url("https://api.flickr.com/services/rest/?method=flickr.urls.lookupGroup&api_key=" + api_key + "&url=" + url_ + "&format=json", signed_auth_bool);
                    JObject jObjx = (JObject) JsonConvert.DeserializeObject(jsonx_str);//dynamic
                    try {
                        if ( jObjx["stat"].ToString() == "ok" ) {
                            group_id = jObjx["group"]["id"].ToString();
                            group_name = unicode_to_string(jObjx["group"]["groupname"]["_content"].ToString());
                            append_msg_THREAD("群組名：" + group_name, color_f_w, bg_color_exe);
                        } else {
                            append_msg_THREAD("資訊錯誤！Error! Can't get group info.\n" + jObjx["message"].ToString(), Color.Red, Color.Yellow);
                            group_id = "";
                            return group_id;
                        }
                    } catch ( Exception ) {
                        append_msg_THREAD("資訊錯誤！Error! Can't get group info.", Color.Red, Color.Yellow);
                        group_id = "";
                        return group_id;
                    }
                } else {
                    group_id = "";
                }
            }
            if ( ver2 >= 0 && ver1< 0 ) {
                group_id = url_.Substring(ver2+10).TrimEnd('/');
                try {
                    group_id = base58_decode(group_id);
                    if ( group_id.Length > 2 ) {
                        group_id = group_id.Substring(0, group_id.Length - 2) + "@N" + group_id.Substring(group_id.Length - 2);
                        return group_id;
                    } else {
                        group_id = "";
                    }
                } catch {
                    group_id = "";
                }
            }
            return group_id;
        }

        public string get_group_id ( string url_, ref string group_name ) {
            string str = "0";
            try {
                str = unsafe_get_group_id(url_, ref group_name);
            } catch ( Exception ) {
                append_msg_THREAD("錯誤-無效的網址！\n" + url_, Color.Red, Color.Yellow);
                return str;
            }
            if ( str.IndexOf("@N") < 0 ) {
                append_msg_THREAD("錯誤-無效的網址！\n" + url_, Color.Red, Color.Yellow);
                return "0";
            }
            return str;
        }

        public string unsafe_get_photoset_id (string url_ ) {
            string photoset_id = "";
            if ( System.Text.RegularExpressions.Regex.IsMatch(url_, "^[0-9]+$") ) {
                return url_;
            }
            int ver1 = url_.IndexOf("flickr.com/photos/");
            int ver2 = url_.IndexOf("flic.kr/s/");
            int ver3 = url_.IndexOf("/in/album-");
            if (ver3 >= 0) {
                url_ = url_.Substring((ver3 + 10));
                int slash_remove_identify = url_.IndexOf("/");
                if (slash_remove_identify >= 0) {
                    url_ = url_.Substring(0, slash_remove_identify);
                }
                return url_;
            }
            url_ = url_.Replace("with/", "");
            int s1 = url_.IndexOf("sets/");
            int s2 = url_.IndexOf("albums/");
            int s3 = url_.IndexOf("flic.kr/s/");
            if ( ver1 < 0 && ver2 < 0 ) {
                photoset_id = "";
                return photoset_id;
            }
            if ( ver1 >= 0 && ver2 < 0 ) {
                if ( s1 >= 0 && s2 < 0 ) {
                    int search_ = url_.TrimEnd('/').IndexOf("/", s1 + 5);
                    if ( search_ < 0 ) {
                        photoset_id = url_.TrimEnd('/').Substring(s1 + 5);
                        if ( System.Text.RegularExpressions.Regex.IsMatch(photoset_id, "^[0-9]+$") ) {
                            return photoset_id;
                        } else {
                            photoset_id = "";
                        }
                    } else {
                        photoset_id = url_.Substring(s1 + 5, search_-s1-5);
                        if ( System.Text.RegularExpressions.Regex.IsMatch(photoset_id, "^[0-9]+$") ) {
                            return photoset_id;
                        } else {
                            photoset_id = "";
                        }
                    }
                } else {
                    photoset_id = "";
                }
                if ( s2 >= 0 && s1 < 0 ) {
                    int search_ = url_.TrimEnd('/').IndexOf("/", s2 + 7);
                    if ( search_ < 0 ) {
                        photoset_id = url_.TrimEnd('/').Substring(s2 + 7);
                        if ( System.Text.RegularExpressions.Regex.IsMatch(photoset_id, "^[0-9]+$") ) {
                            return photoset_id;
                        } else {
                            photoset_id = "";
                        }
                    } else {
                        photoset_id = url_.Substring(s2 + 7, search_ - s2 - 7);
                        if ( System.Text.RegularExpressions.Regex.IsMatch(photoset_id, "^[0-9]+$") ) {
                            return photoset_id;
                        } else {
                            photoset_id = "";
                        }
                    }
                } else {
                    photoset_id = "";
                }
            }
            if ( ver2 >= 0 && ver1 < 0 && s3 >= 0 ) {
                photoset_id = base58_decode(url_.Substring(s3 + 10).Replace("/",""));
                return photoset_id;
            }
            return photoset_id;
        }

        public string get_photoset_id ( string url_ ) {
            string str = "0";
            try {
                str = unsafe_get_photoset_id(url_);
            } catch (Exception) {
                append_msg_THREAD("錯誤-無效的網址！\n" + url_, Color.Red, Color.Yellow);
                return str;
            }
            if ( !System.Text.RegularExpressions.Regex.IsMatch(str, "^[0-9]+$") ) {
                append_msg_THREAD("錯誤-無效的網址！\n" + url_, Color.Red, Color.Yellow);
                return "0";
            }
            return str;
        }

        public string unsafe_get_photo_id ( string url_ ) {
            if (System.Text.RegularExpressions.Regex.IsMatch(url_, "^[0-9]+$")) {
                return url_;
            }
            string photo_id = "";
            int ver1 = url_.IndexOf("flickr.com/photos/");
            int ver2 = url_.IndexOf("flic.kr/p/");
            int ver3 = url_.IndexOf("/in/");
            if ( ver3 >=0 ) {
                url_ = url_.Substring(0, ver3);
            }
            int s1 = url_.IndexOf("photos/");
            int s2 = url_.IndexOf("flic.kr/p/");
            if ( ver1 < 0 && ver2 < 0 ) {
                photo_id = "";
                return photo_id;
            }
            if ( ver1 >= 0 && ver2 < 0 ) {
                if ( s1 >= 0 && s2 < 0 ) {
                    int search_id = url_.IndexOf("/", s1 + 8);
                    photo_id = url_.TrimEnd('/').Substring(search_id + 1);
                    return photo_id;
                } else {
                    photo_id = "";
                }
            }
            if ( ver2 >= 0 && ver1 < 0 && s2 >= 0 ) {
                if ( s2 >= 0 && s1 < 0 ) {
                    photo_id = base58_decode(url_.TrimEnd('/').Substring(s2 + 10));
                    return photo_id;
                } else {
                    photo_id = "";
                }
            }
            return photo_id;
        }

        public string get_photo_id ( string url_ ) {
            string str = "0";
            try {
                str = unsafe_get_photo_id(url_);
            } catch ( Exception ) {
                append_msg_THREAD("錯誤-無效的網址！\n" + url_, Color.Red, Color.Yellow);
                return str;
            }
            if ( !System.Text.RegularExpressions.Regex.IsMatch(str, "^[0-9]+$") ) {
                append_msg_THREAD("錯誤-無效的網址！\n" + url_, Color.Red, Color.Yellow);
                return "0";
            }
            return str;
        }

        public static string[] split_two_piece ( string str, string key ) {
            string[] target_str = new string[2];
            int search_ = str.IndexOf(key);
            if ( search_ < 0 ) {
                target_str[0] = "";
                target_str[1] = "";
                return target_str;
            }
            target_str[0] = str.Substring(0, search_);
            target_str[1] = str.Substring(search_ + key.Length);
            return target_str;
        }

        public void header_split ( out string[] header_key, out string[] header_value, string header_req ) {
            string[] header_line = fast_split(header_req.Trim((char) 10), LF.ToString());
            header_key = new string[header_line.Length];
            header_value = new string[header_line.Length];
            for ( int i = 0; i < header_line.Length; i++ ) {
                string[] header_line_line = split_two_piece(header_line[i], "=");
                header_key[i] = header_line_line[0];
                header_value[i] = header_line_line[1];
            }
        }

        public void set_header ( ref WebClient_auto wbcl_, ref string header_req, string header_rep_set_cookie ) {
            string[] header_key;
            string[] header_value;
            string[] header_key_cookie;
            string[] header_value_cookie;
            header_req = header_req.Trim((char) 10);
            wbcl_.Encoding = Encoding.UTF8;
            header_split(out header_key_cookie, out header_value_cookie, header_rep_set_cookie);
            int search_set_cookie = Array.IndexOf(header_key_cookie, "Set-Cookie");
            if ( search_set_cookie >= 0 ) {
                if ( header_req == "" ) {
                    header_req = "Cookie=" + header_value_cookie[search_set_cookie];
                } else {
                    header_req += LF + "Cookie=" + header_value_cookie[search_set_cookie];
                }
            }
            header_split(out header_key, out header_value, header_req);
            for ( int i = 0; i < header_key.Length; i++ ) {
                if ( header_key[i] == "" ) {
                    continue;
                }
                wbcl_.Headers.Add(header_key[i], header_value[i]);
            }
            last_byte[0] = 0;
            current_downloader_key[0] = creat_rnd_key();
            wbcl_.Headers.Add("WBFT-WebBrowser-Tag-Num", current_downloader_key[0]);
            wbcl_.Headers.Add("WBFT-WebBrowser-Tag-Thread", "0");
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
            last_byte[index_thread] = 0;
            current_downloader_key[index_thread] = creat_rnd_key();
            wbcl_.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            wbcl_.Headers.Add("WBFT-WebBrowser-Tag-Num", current_downloader_key[index_thread]);
            wbcl_.Headers.Add("WBFT-WebBrowser-Tag-Thread", index_thread.ToString());
        }

        public string get_json_from_url ( string url , bool signed) {
            if (signed) {
                int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                url = url.Replace("&api_key=" + api_key, "") + "&nojsoncallback=1&oauth_nonce=" + rand_.Next(111111, 99999999) + "&oauth_consumer_key=" + api_key + "&oauth_timestamp=" + unixTimestamp + "&oauth_signature_method=HMAC-SHA1&oauth_version=1.0&oauth_token=" + oauth_token;
                string oauth_signature = get_sign_hmacsha1(url, api_secret + "&" + oauth_secret);
                url = url + "&oauth_signature=" + url_encode(oauth_signature);
            }
            Debug.WriteLine(url);
            //Application.DoEvents();
            set_all_Zero(ref total_byte);
            set_all_Zero(ref now_byte);
            download_ok[0] = false;
            bytes[0] = new byte[] { };
            live_X = new bool[] { false };
            thread_download_number[0] = 0;
            wbcl_[0] = new WebClient_auto();
            string header_req_X = header_req_USER_SET;
            set_header(ref wbcl_[0], ref header_req_X, "",0);
            wbcl_[0].DownloadDataCompleted += DownloadDataCallback;
            wbcl_[0].DownloadProgressChanged += DownloadProgressChanged;
            wbcl_[0].DownloadDataAsync(new Uri(url));
            while ( true ) {
                System.Threading.Thread.Sleep(1);
                //Application.DoEvents();
                if ( download_ok[0] == true ) {
                    break;
                }
                if ( download_started == false || form_must_close == true ) {
                    wbcl_[0].CancelAsync();
                    wbcl_[0].Dispose();
                    break;
                }
            }
            if ( live_X[0] == false ) {
                status_text.Text = "下載JSON失敗！FAIL！";
                append_msg_THREAD("下載JSON失敗！FAIL！", Color.Red, Color.Yellow);
                return "";
            } else {
                status_text.Text = "下載JSON完成！Success！";
                append_msg_THREAD("下載JSON完成！Success！", color_f_g, bg_color_exe);
            }
            if (signed) {
                return (System.Text.UTF8Encoding.UTF8.GetString(bytes[0]));
            }
            return (System.Text.UTF8Encoding.UTF8.GetString(bytes[0])).TrimEnd(')').Substring(14);
        }

        public string get_response_from_url(string url) {
            Debug.WriteLine(url);
            //Application.DoEvents();
            set_all_Zero(ref total_byte);
            set_all_Zero(ref now_byte);
            download_ok[0] = false;
            bytes[0] = new byte[] { };
            live_X = new bool[] { false };
            thread_download_number[0] = 0;
            wbcl_[0] = new WebClient_auto();
            string header_req_X = header_req_USER_SET;
            set_header(ref wbcl_[0], ref header_req_X, "",0);
            wbcl_[0].Headers.Add("WBFT-App", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            wbcl_[0].DownloadDataCompleted += DownloadDataCallback;
            wbcl_[0].DownloadProgressChanged += DownloadProgressChanged;
            wbcl_[0].DownloadDataAsync(new Uri(url));
            while (true) {
                System.Threading.Thread.Sleep(1);
                //Application.DoEvents();
                if (download_ok[0] == true) {
                    break;
                }
                if (download_started == false || form_must_close == true) {
                    wbcl_[0].CancelAsync();
                    wbcl_[0].Dispose();
                    break;
                }
            }
            if (live_X[0] == false) {
                status_text.Text = "要求回應失敗！FAIL！";
                append_msg_THREAD("要求回應失敗！FAIL！", Color.Red, Color.Yellow);
                Debug.WriteLine(System.Text.UTF8Encoding.UTF8.GetString(bytes[0]));
                return "";
            } else {
                status_text.Text = "要求回應完成！Success！";
                append_msg_THREAD("要求回應完成！Success！", color_f_g, bg_color_exe);
            }
            return (System.Text.UTF8Encoding.UTF8.GetString(bytes[0]));
        }
        
        public string get_response_from_url_DO_event(string url) {
            Debug.WriteLine(url);
            Application.DoEvents();
            set_all_Zero(ref total_byte);
            set_all_Zero(ref now_byte);
            download_ok[0] = false;
            bytes[0] = new byte[] { };
            live_X = new bool[] { false };
            thread_download_number[0] = 0;
            wbcl_[0] = new WebClient_auto();
            string header_req_X = header_req_USER_SET;
            set_header(ref wbcl_[0], ref header_req_X, "", 0);
            wbcl_[0].Headers.Add("WBFT-App", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            wbcl_[0].DownloadDataCompleted += DownloadDataCallback;
            wbcl_[0].DownloadProgressChanged += DownloadProgressChanged;
            wbcl_[0].DownloadDataAsync(new Uri(url));
            while (true) {
                System.Threading.Thread.Sleep(25);
                Application.DoEvents();
                if (download_ok[0] == true) {
                    break;
                }
                if (download_started == false || form_must_close == true) {
                    wbcl_[0].CancelAsync();
                    wbcl_[0].Dispose();
                    break;
                }
            }
            if (live_X[0] == false) {
                status_text.Text = "要求回應失敗！FAIL！";
                append_msg_THREAD("要求回應失敗！FAIL！", Color.Red, Color.Yellow);
                Debug.WriteLine(System.Text.UTF8Encoding.UTF8.GetString(bytes[0]));
                return "";
            } else {
                status_text.Text = "要求回應完成！Success！";
                append_msg_THREAD("要求回應完成！Success！", color_f_g, bg_color_exe);
            }
            return (System.Text.UTF8Encoding.UTF8.GetString(bytes[0]));
        }

        private void DownloadDataCallback ( Object sender, System.Net.DownloadDataCompletedEventArgs e ) {
            string mykey = "";
            int this_thread_num = (-1);
            try {
                mykey = ( (WebClient_auto) sender ).Headers["WBFT-WebBrowser-Tag-Num"].ToString();
                this_thread_num = int.Parse(((WebClient_auto)sender).Headers["WBFT-WebBrowser-Tag-Thread"].ToString());
                freeze_time[this_thread_num] = 0;
            } catch {

            }
            if ( mykey != "" ) {
                if ( check_is_my_key_dead(mykey) ) {
                    Debug.WriteLine("this is dead. abandon.");
                    bytes[this_thread_num] = new byte[] { };
                    live_X[thread_download_number[this_thread_num]] = false;
                    dead_or_not.Remove(mykey);
                    ( (WebClient_auto) sender ).Dispose();
                    return;
                }
            } else {
                Debug.WriteLine("why is my key empty.");
            }
            try {

                if ( e.Cancelled == false && e.Error == null ) {
                    bytes[this_thread_num] = e.Result;
                    live_X[thread_download_number[this_thread_num]] = true;
                } else {
                    try {
                        Debug.WriteLine("取消狀態：" + e.Cancelled.ToString());
                        Debug.WriteLine(e.Error.Data.ToString());
                        Debug.WriteLine(e.Error.Message.ToString());
                        Debug.WriteLine(((WebClient_auto)sender).ResponseHeaders.ToString());
                        Debug.WriteLine(e.Error.ToString());
                    } catch {

                    }
                    bytes[this_thread_num] = new byte[] { };
                    live_X[thread_download_number[this_thread_num]] = false;
                }
            } finally {
            }
            download_ok[this_thread_num] = true;
            speed_kbyte[this_thread_num] = 1;
        }

        private void DownloadProgressChanged ( Object sender, System.Net.DownloadProgressChangedEventArgs e ) {
            //WebClient_auto wbcl_ = (WebClient_auto) sender;
            int this_thread_num = int.Parse(((WebClient_auto)sender).Headers["WBFT-WebBrowser-Tag-Thread"].ToString());
            now_byte[this_thread_num] = e.BytesReceived;
            total_byte[this_thread_num] = e.TotalBytesToReceive;
            if (total_byte[this_thread_num] > 0 ) {
                try {
                    setToolStripProgressBar_max(TProgbox1, (int)e.TotalBytesToReceive);
                    //TProgbox1.Maximum = (int)e.TotalBytesToReceive;
                    if (now_byte[this_thread_num] <= total_byte[this_thread_num]) {
                        setToolStripProgressBar_value(TProgbox1, (int)now_byte[this_thread_num]);
                        //TProgbox1.Value = (int)now_byte[this_thread_num];
                    }
                } catch {

                }
            }
            try {
                //bottom_status_prog.Text = "（" + Math.Round((decimal) now_byte.Sum() / 1024, 1) + " KBytes / " + Math.Round((decimal)total_byte.Sum() / 1024, 1) + " KBytes）- " + Math.Round((decimal) now_byte.Sum()/total_byte.Sum(),1) + "%" + " (網速：" + speed_kbyte.Sum() + " KBytes/sec)";
                string percentage = "";
                if (total_byte.Sum() > 0 ) {
                    decimal percentage_double =(decimal) Math.Round((decimal)now_byte.Sum() / total_byte.Sum() * 100, 1);
                    if (percentage_double < 0 || percentage_double>100) {
                        percentage = "--%";
                    } else {
                        percentage = percentage_double.ToString() + "%";
                    }
                }
                //this.SetStatusBottomText("（" + Math.Round((decimal)now_byte.Sum() / 1024, 1) + " KBytes / " + Math.Round((decimal)total_byte.Sum() / 1024, 1) + " KBytes）- " + percentage + " (總網速：" + speed_kbyte.Sum() + " KBytes/sec) 個別網速： (" + speed_kbyte[0] + "/" + speed_kbyte[1] + "/" + speed_kbyte[2] + "/" + speed_kbyte[3] + ")");
                this.SetStatusBottomText("（" + Math.Round((decimal)now_byte.Sum() / 1024, 1) + " KBytes / " + Math.Round((decimal)total_byte.Sum() / 1024, 1) + " KBytes）- " + percentage + " (總網速：" + speed_kbyte.Sum() + " KBytes/sec)");
            } catch ( Exception ) {

            }
        }

        private void DownloadProgressChanged2(Object sender, System.Net.DownloadProgressChangedEventArgs e) {
            int this_thread_num = int.Parse(((WebClient_auto)sender).Headers["WBFT-WebBrowser-Tag-Thread"].ToString());
            now_byte[this_thread_num] = e.BytesReceived;
            total_byte[this_thread_num] = e.TotalBytesToReceive;
            if (total_byte[this_thread_num] > 0) {
                try {
                    setToolStripProgressBar_max(TProgbox2, (int)e.TotalBytesToReceive);
                    //TProgbox2.Maximum = (int)e.TotalBytesToReceive;
                    if (now_byte[this_thread_num] <= total_byte[this_thread_num]) {
                        setToolStripProgressBar_value(TProgbox2, (int)now_byte[this_thread_num]);
                        //TProgbox2.Value = (int)now_byte[this_thread_num];
                    }
                } catch {

                }
            }
            try {
                if (!thread_busy[0]) {
                    string percentage = "";
                    if (total_byte.Sum() > 0) {
                        decimal percentage_double = (decimal)Math.Round((decimal)now_byte.Sum() / total_byte.Sum() * 100, 1);
                        if (percentage_double < 0 || percentage_double > 100) {
                            percentage = "--%";
                        } else {
                            percentage = percentage_double.ToString() + "%";
                        }
                    }
                    this.SetStatusBottomText("（" + Math.Round((decimal)now_byte.Sum() / 1024, 1) + " KBytes / " + Math.Round((decimal)total_byte.Sum() / 1024, 1) + " KBytes）- " + percentage + " (總網速：" + speed_kbyte.Sum() + " KBytes/sec)");
                }
            } catch (Exception) {

            }
        }


        private void DownloadProgressChanged3(Object sender, System.Net.DownloadProgressChangedEventArgs e) {
            int this_thread_num = int.Parse(((WebClient_auto)sender).Headers["WBFT-WebBrowser-Tag-Thread"].ToString());
            now_byte[this_thread_num] = e.BytesReceived;
            total_byte[this_thread_num] = e.TotalBytesToReceive;
            if (total_byte[this_thread_num] > 0) {
                try {
                    setToolStripProgressBar_max(TProgbox3, (int)e.TotalBytesToReceive);
                    //TProgbox3.Maximum = (int)e.TotalBytesToReceive;
                    if (now_byte[this_thread_num] <= total_byte[this_thread_num]) {
                        setToolStripProgressBar_value(TProgbox3, (int)now_byte[this_thread_num]);
                        //TProgbox3.Value = (int)now_byte[this_thread_num];
                    }
                } catch {

                }
            }
            try {
                if (!thread_busy[0] && !thread_busy[1]) {
                    string percentage = "";
                    if (total_byte.Sum() > 0) {
                        decimal percentage_double = (decimal)Math.Round((decimal)now_byte.Sum() / total_byte.Sum() * 100, 1);
                        if (percentage_double < 0 || percentage_double > 100) {
                            percentage = "--%";
                        } else {
                            percentage = percentage_double.ToString() + "%";
                        }
                    }
                    this.SetStatusBottomText("（" + Math.Round((decimal)now_byte.Sum() / 1024, 1) + " KBytes / " + Math.Round((decimal)total_byte.Sum() / 1024, 1) + " KBytes）- " + percentage + " (總網速：" + speed_kbyte.Sum() + " KBytes/sec)");
                }
            } catch (Exception) {

            }
        }


        private void DownloadProgressChanged4(Object sender, System.Net.DownloadProgressChangedEventArgs e) {
            int this_thread_num = int.Parse(((WebClient_auto)sender).Headers["WBFT-WebBrowser-Tag-Thread"].ToString());
            now_byte[this_thread_num] = e.BytesReceived;
            total_byte[this_thread_num] = e.TotalBytesToReceive;
            if (total_byte[this_thread_num] > 0) {
                try {
                    setToolStripProgressBar_max(TProgbox4, (int)e.TotalBytesToReceive);
                    //TProgbox4.Maximum = (int)e.TotalBytesToReceive;
                    if (now_byte[this_thread_num] <= total_byte[this_thread_num]) {
                        setToolStripProgressBar_value(TProgbox4, (int)now_byte[this_thread_num]);
                        //TProgbox4.Value = (int)now_byte[this_thread_num];
                    }
                } catch {

                }
            }
            try {
                if (!thread_busy[0] && !thread_busy[1] && !thread_busy[2]) {
                    string percentage = "";
                    if (total_byte.Sum() > 0) {
                        decimal percentage_double = (decimal)Math.Round((decimal)now_byte.Sum() / total_byte.Sum() * 100, 1);
                        if (percentage_double < 0 || percentage_double > 100) {
                            percentage = "--%";
                        } else {
                            percentage = percentage_double.ToString() + "%";
                        }
                    }
                    this.SetStatusBottomText("（" + Math.Round((decimal)now_byte.Sum() / 1024, 1) + " KBytes / " + Math.Round((decimal)total_byte.Sum() / 1024, 1) + " KBytes）- " + percentage + " (總網速：" + speed_kbyte.Sum() + " KBytes/sec)");
                }
            } catch (Exception) {

            }
        }

        public string unicode_to_string (string str) {
            return System.Text.RegularExpressions.Regex.Unescape(str);
        }

        public string filename_filter (string str) {
            str=str.Replace("*", "＊").Replace("|", "｜").Replace("\\", "＼").Replace(":", "：").Replace("\"", "'").Replace("<", "＜").Replace(">", "＞").Replace("?", "？").Replace("/", "／").Replace(LF,"").Replace(CR,"");
            if (str.Length>50) {
                str = str.Substring(0, 50) + "…";
            }
            return str;
        }

        public string find_new_dir_name(string str, string save_path) {
            int i = 1;
            while (true) {
                if (!System.IO.Directory.Exists(save_path + "\\" + str + " (" + i + ")")) {
                    return str + " (" + i + ")";
                }
                i += 1;
            }
        }

        private static string find_new_file_name ( string str, string ext_name, string save_path ) {
            lock(_thisLock) {
                Debug.WriteLine(save_path + "\\" + str + ext_name);
                if (!System.IO.File.Exists(save_path + "\\" + str + ext_name)) {
                    System.IO.File.WriteAllBytes(save_path + "\\" + str + ext_name, new byte[] { });
                    return str;
                }
                int i = 1;
                while (true) {
                    if (!System.IO.File.Exists(save_path + "\\" + str + " (" + i + ")" + ext_name)) {
                        System.IO.File.WriteAllBytes(save_path + "\\" + str + " (" + i + ")" + ext_name, new byte[] { });
                        return str + " (" + i + ")";
                    }
                    i += 1;
                }
            }
        }

        public void get_method_by_sizeoptions(int options_SIZE,ref int search_method, ref int size_id_number) {
            if (options_SIZE <= 12) {
                search_method = 1;
                size_id_number = options_SIZE;
            }else if (options_SIZE >= 26) {
                search_method = 2;
                size_id_number = (options_SIZE - 26);
            } else {
                search_method = 0;
                size_id_number = (options_SIZE - 13);
            }
        }

        public string get_url(JObject obj, int size_typeX, bool preview_image, string user_id) {
            string url_ = get_url(obj, size_typeX);
            bool succ = false;
            if (!preview_image && options_8_video_download) {
                try {
                    if (obj["media"].ToString().Equals("video")) {
                        url_ = get_video_url_from_image_url(url_, user_id, ref succ);
                        if (!succ) {
                            append_msg_THREAD("錯誤，無法解析影片網址。Error! Can't resolve video URL.", Color.Red, Color.Yellow);
                            Debug.WriteLine("video link resolve error, not geting succ");
                        }
                    }
                } catch (Exception) {
                    append_msg_THREAD("錯誤，無法解析影片網址。Error! Can't resolve video URL.", Color.Red, Color.Yellow);
                    Debug.WriteLine("video link resolve error, no obj maybe");
                }
            }
            return url_;
        }

        public string get_url(JObject obj, bool preview_image, string user_id) {
            string url_ = get_url(obj);
            bool succ = false;
            if (!preview_image && options_8_video_download) {
                try {
                    if (obj["media"].ToString().Equals("video")) {
                        url_ = get_video_url_from_image_url(url_, user_id, ref succ);
                        if (!succ) {
                            append_msg_THREAD("錯誤，無法解析影片網址。Error! Can't resolve video URL.", Color.Red, Color.Yellow);
                            Debug.WriteLine("video link resolve error");
                        }
                    }
                } catch (Exception) {
                    append_msg_THREAD("錯誤，無法解析影片網址。Error! Can't resolve video URL.", Color.Red, Color.Yellow);
                    Debug.WriteLine("video link resolve error");
                }
            }
            return url_;
        }

        public string get_url(JObject obj,int size_typeX) {
            int search_method = 1;
            get_method_by_sizeoptions(size_typeX, ref search_method, ref size_typeX);
            if (search_method==0) {
                //Only
                try {
                    if (obj.Property("url_o") != null && size_typeX == 0) {
                        return obj["url_o"].ToString();
                    } else if (obj.Property("url_6k") != null && size_typeX == 1) {
                        return obj["url_6k"].ToString();
                    } else if (obj.Property("url_5k") != null && size_typeX == 2) {
                        return obj["url_5k"].ToString();
                    } else if (obj.Property("url_4k") != null && size_typeX == 3) {
                        return obj["url_4k"].ToString();
                    } else if (obj.Property("url_3k") != null && size_typeX == 4) {
                        return obj["url_3k"].ToString();
                    } else if (obj.Property("url_k") != null && size_typeX == 5) {
                        return obj["url_k"].ToString();
                    } else if (obj.Property("url_h") != null && size_typeX == 6) {
                        return obj["url_h"].ToString();
                    } else if (obj.Property("url_l") != null && size_typeX == 7) {
                        return obj["url_l"].ToString();
                    } else if (obj.Property("url_c") != null && size_typeX == 8) {
                        return obj["url_c"].ToString();
                    } else if (obj.Property("url_z") != null && size_typeX == 9) {
                        return obj["url_z"].ToString();
                    } else if (obj.Property("url_m") != null && size_typeX == 10) {
                        return obj["url_m"].ToString();
                    } else if (obj.Property("url_n") != null && size_typeX == 11) {
                        return obj["url_n"].ToString();
                    } else if (obj.Property("url_s") != null && size_typeX == 12) {
                        return obj["url_s"].ToString();
                    } else {
                        append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL.", Color.Red, Color.Yellow);
                        Debug.WriteLine("all FALSE1");
                        return "";
                    }
                } catch (Exception ex) {
                    append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL." + ex.ToString(), Color.Red, Color.Yellow);
                    return "";
                }
            } else if(search_method==1) {
                //Auto
                try {
                    if (obj.Property("url_o") != null && size_typeX == 0) {
                        return obj["url_o"].ToString();
                    } else if (obj.Property("url_6k") != null && size_typeX <= 1) {
                        return obj["url_6k"].ToString();
                    } else if (obj.Property("url_5k") != null && size_typeX <= 2) {
                        return obj["url_5k"].ToString();
                    } else if (obj.Property("url_4k") != null && size_typeX <= 3) {
                        return obj["url_4k"].ToString();
                    } else if (obj.Property("url_3k") != null && size_typeX <= 4) {
                        return obj["url_3k"].ToString();
                    } else if (obj.Property("url_k") != null && size_typeX <= 5) {
                        return obj["url_k"].ToString();
                    } else if (obj.Property("url_h") != null && size_typeX <= 6) {
                        return obj["url_h"].ToString();
                    } else if (obj.Property("url_l") != null && size_typeX <= 7) {
                        return obj["url_l"].ToString();
                    } else if (obj.Property("url_c") != null && size_typeX <= 8) {
                        return obj["url_c"].ToString();
                    } else if (obj.Property("url_z") != null && size_typeX <= 9) {
                        return obj["url_z"].ToString();
                    } else if (obj.Property("url_m") != null && size_typeX <= 10) {
                        return obj["url_m"].ToString();
                    } else if (obj.Property("url_n") != null && size_typeX <= 11) {
                        return obj["url_n"].ToString();
                    } else if (obj.Property("url_s") != null && size_typeX <= 12) {
                        return obj["url_s"].ToString();
                    } else {
                        if (obj.Property("url_o") != null) {
                            return obj["url_o"].ToString();
                        } else {
                            append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL.", Color.Red, Color.Yellow);
                            Debug.WriteLine("all FALSE2");
                            return "";
                        }
                    }
                } catch (Exception ex) {
                    append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL." + ex.ToString(), Color.Red, Color.Yellow);
                    return "";
                }
            } else {
                //At least
                try {
                    if (obj.Property("url_o") != null) {
                        return obj["url_o"].ToString();
                    } else if (obj.Property("url_6k") != null) {
                        return obj["url_6k"].ToString();
                    } else if (obj.Property("url_5k") != null && size_typeX >= 1) {
                        return obj["url_5k"].ToString();
                    } else if (obj.Property("url_4k") != null && size_typeX >= 2) {
                        return obj["url_4k"].ToString();
                    } else if (obj.Property("url_3k") != null && size_typeX >= 3) {
                        return obj["url_3k"].ToString();
                    } else if (obj.Property("url_k") != null && size_typeX >= 4) {
                        return obj["url_k"].ToString();
                    } else if (obj.Property("url_h") != null && size_typeX >= 5) {
                        return obj["url_h"].ToString();
                    } else if (obj.Property("url_l") != null && size_typeX >= 6) {
                        return obj["url_l"].ToString();
                    } else if (obj.Property("url_c") != null && size_typeX >= 7) {
                        return obj["url_c"].ToString();
                    } else if (obj.Property("url_z") != null && size_typeX >= 8) {
                        return obj["url_z"].ToString();
                    } else if (obj.Property("url_m") != null && size_typeX >= 9) {
                        return obj["url_m"].ToString();
                    } else if (obj.Property("url_n") != null && size_typeX >= 10) {
                        return obj["url_n"].ToString();
                    } else if (obj.Property("url_s") != null && size_typeX >= 11) {
                        return obj["url_s"].ToString();
                    } else {
                        if (obj.Property("url_o") != null) {
                            return obj["url_o"].ToString();
                        } else {
                            append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL.", Color.Red, Color.Yellow);
                            Debug.WriteLine("all FALSE3");
                            return "";
                        }
                    }
                } catch (Exception ex) {
                    append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL." + ex.ToString(), Color.Red, Color.Yellow);
                    return "";
                }
            }

        }

        public string get_url (JObject obj) {
            int temp_size_type = 0;
            int search_method = 1;
            get_method_by_sizeoptions(options_4_size,ref search_method, ref temp_size_type);
            if (search_method==0) {
                //Only
                try {
                    if (obj.Property("url_o") != null && temp_size_type == 0) {
                        return obj["url_o"].ToString();
                    } else if (obj.Property("url_6k") != null && temp_size_type == 1) {
                        return obj["url_6k"].ToString();
                    } else if (obj.Property("url_5k") != null && temp_size_type == 2) {
                        return obj["url_5k"].ToString();
                    } else if (obj.Property("url_4k") != null && temp_size_type == 3) {
                        return obj["url_4k"].ToString();
                    } else if (obj.Property("url_3k") != null && temp_size_type == 4) {
                        return obj["url_3k"].ToString();
                    } else if (obj.Property("url_k") != null && temp_size_type == 5) {
                        return obj["url_k"].ToString();
                    } else if (obj.Property("url_h") != null && temp_size_type == 6) {
                        return obj["url_h"].ToString();
                    } else if (obj.Property("url_l") != null && temp_size_type == 7) {
                        return obj["url_l"].ToString();
                    } else if (obj.Property("url_c") != null && temp_size_type == 8) {
                        return obj["url_c"].ToString();
                    } else if (obj.Property("url_z") != null && temp_size_type == 9) {
                        return obj["url_z"].ToString();
                    } else if (obj.Property("url_m") != null && temp_size_type == 10) {
                        return obj["url_m"].ToString();
                    } else if (obj.Property("url_n") != null && temp_size_type == 11) {
                        return obj["url_n"].ToString();
                    } else if (obj.Property("url_s") != null && temp_size_type == 12) {
                        return obj["url_s"].ToString();
                    } else {
                        append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL.", Color.Red, Color.Yellow);
                        Debug.WriteLine("all FALSE4");
                        return "";
                    }
                } catch (Exception ex) {
                    append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL." + ex.ToString(), Color.Red, Color.Yellow);
                    return "";
                }
            } else if(search_method==1) {
                //Auto
                try {
                    if ( obj.Property("url_o") != null && temp_size_type==0) {
                        return obj["url_o"].ToString();
                    } else if (obj.Property("url_6k") != null && temp_size_type <= 1) {
                        return obj["url_6k"].ToString();
                    } else if (obj.Property("url_5k") != null && temp_size_type <= 2) {
                        return obj["url_5k"].ToString();
                    } else if (obj.Property("url_4k") != null && temp_size_type <= 3) {
                        return obj["url_4k"].ToString();
                    } else if (obj.Property("url_3k") != null && temp_size_type <= 4) {
                        return obj["url_3k"].ToString();
                    } else if ( obj.Property("url_k") != null && temp_size_type <= 5) {
                        return obj["url_k"].ToString();
                    } else if ( obj.Property("url_h") != null && temp_size_type <= 6) {
                        return obj["url_h"].ToString();
                    } else if ( obj.Property("url_l") != null && temp_size_type <= 7) {
                        return obj["url_l"].ToString();
                    } else if ( obj.Property("url_c") != null && temp_size_type <= 8) {
                        return obj["url_c"].ToString();
                    } else if ( obj.Property("url_z") != null && temp_size_type <= 9) {
                        return obj["url_z"].ToString();
                    } else if ( obj.Property("url_m") != null && temp_size_type <= 10) {
                        return obj["url_m"].ToString();
                    } else if ( obj.Property("url_n") != null && temp_size_type <= 11) {
                        return obj["url_n"].ToString();
                    } else if ( obj.Property("url_s") != null && temp_size_type <= 12) {
                        return obj["url_s"].ToString();
                    } else {
                        if (obj.Property("url_o") != null) {
                            return obj["url_o"].ToString();
                        } else {
                            append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL.", Color.Red, Color.Yellow);
                            Debug.WriteLine("all FALSE5");
                            return "";
                        }
                    }
                } catch (Exception ex) {
                    append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL." + ex.ToString(), Color.Red, Color.Yellow);
                    return "";
                }
            } else {
                //At least
                try {
                    if (obj.Property("url_o") != null) {
                        return obj["url_o"].ToString();
                    } else if (obj.Property("url_6k") != null) {
                        return obj["url_6k"].ToString();
                    } else if (obj.Property("url_5k") != null && temp_size_type >= 1) {
                        return obj["url_5k"].ToString();
                    } else if (obj.Property("url_4k") != null && temp_size_type >= 2) {
                        return obj["url_4k"].ToString();
                    } else if (obj.Property("url_3k") != null && temp_size_type >= 3) {
                        return obj["url_3k"].ToString();
                    } else if (obj.Property("url_k") != null && temp_size_type >= 4) {
                        return obj["url_k"].ToString();
                    } else if (obj.Property("url_h") != null && temp_size_type >= 5) {
                        return obj["url_h"].ToString();
                    } else if (obj.Property("url_l") != null && temp_size_type >= 6) {
                        return obj["url_l"].ToString();
                    } else if (obj.Property("url_c") != null && temp_size_type >= 7) {
                        return obj["url_c"].ToString();
                    } else if (obj.Property("url_z") != null && temp_size_type >= 8) {
                        return obj["url_z"].ToString();
                    } else if (obj.Property("url_m") != null && temp_size_type >= 9) {
                        return obj["url_m"].ToString();
                    } else if (obj.Property("url_n") != null && temp_size_type >= 10) {
                        return obj["url_n"].ToString();
                    } else if (obj.Property("url_s") != null && temp_size_type >= 11) {
                        return obj["url_s"].ToString();
                    } else {
                        if (obj.Property("url_o") != null) {
                            return obj["url_o"].ToString();
                        } else {
                            append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL.", Color.Red, Color.Yellow);
                            Debug.WriteLine("all FALSE6");
                            return "";
                        }
                    }
                } catch (Exception ex) {
                    append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL." + ex.ToString(), Color.Red, Color.Yellow);
                    return "";
                }
            }

        }

        public string get_user_name_from_photosize_source(string source_url) {
            var regex = new System.Text.RegularExpressions.Regex(@"(https|http):\/\/www\.flickr\.com\/photos\/(.+?)\/[0-9]+\/play\/.+");
            var match = regex.Match(source_url);
            if (match.Success) {
                return match.Groups[2].Value;
            } else {
                return "";
            }
        }

        private string get_url_photo (JObject obj, bool preview_image) {
            string url_x = get_url_photo(obj);
            if (!preview_image && options_8_video_download) {
                bool found_video = false;
                string mobile_mp4_source_url = "";
                string site_mp4_source_url = "";
                string hd_mp4_source_url = "";
                string original_mp4_source_url = "";
                for (int i = 0; i < obj["sizes"]["size"].Count(); i++) {
                    if (obj["sizes"]["size"][i]["label"].ToString() == "Site MP4") {
                        found_video = true;
                        site_mp4_source_url = obj["sizes"]["size"][i]["source"].ToString();
                    } else if (obj["sizes"]["size"][i]["label"].ToString() == "Mobile MP4") {
                        found_video = true;
                        mobile_mp4_source_url = obj["sizes"]["size"][i]["source"].ToString();
                    } else if (obj["sizes"]["size"][i]["label"].ToString() == "HD MP4") {
                        found_video = true;
                        hd_mp4_source_url = obj["sizes"]["size"][i]["source"].ToString();
                    } else if (obj["sizes"]["size"][i]["label"].ToString() == "Video Original") {
                        found_video = true;
                        original_mp4_source_url = obj["sizes"]["size"][i]["source"].ToString();
                    }
                }
                if (found_video) {
                    if (original_mp4_source_url!="") {
                        url_x = original_mp4_source_url;
                    } else if (hd_mp4_source_url!="") {
                        url_x = hd_mp4_source_url;
                    } else if (site_mp4_source_url != "") {
                        url_x = site_mp4_source_url;
                    } else if (mobile_mp4_source_url != "") {
                        url_x = mobile_mp4_source_url;
                    }
                }
            }
            return url_x;
        }

        private string get_url_photo ( JObject obj ) {
            int temp_size_type = 0;
            int search_method = 1;
            get_method_by_sizeoptions(options_4_size, ref search_method, ref temp_size_type);
            string[] array_size = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "" };
            for (int i = 0; i < obj["sizes"]["size"].Count(); i++) {
                if (obj["sizes"]["size"][i]["label"].ToString() == "Original") {
                    array_size[0] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "X-Large 6K") {
                    array_size[1] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "X-Large 5K") {
                    array_size[2] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "X-Large 4K") {
                    array_size[3] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "X-Large 3K") {
                    array_size[4] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "Large 2048") {
                    array_size[5] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "Large 1600") {
                    array_size[6] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "Large") {
                    array_size[7] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "Medium 800") {
                    array_size[8] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "Medium 640") {
                    array_size[9] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "Medium") {
                    array_size[10] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "Small 320") {
                    array_size[11] = obj["sizes"]["size"][i]["source"].ToString();
                } else if (obj["sizes"]["size"][i]["label"].ToString() == "Small") {
                    array_size[12] = obj["sizes"]["size"][i]["source"].ToString();
                }
            }
            if (search_method==0) {
                //Only
                try {
                    if (array_size[0] != "" && temp_size_type == 0) {
                        return array_size[0];
                    } else if (array_size[1] != "" && temp_size_type == 1) {
                        return array_size[1];
                    } else if (array_size[2] != "" && temp_size_type == 2) {
                        return array_size[2];
                    } else if (array_size[3] != "" && temp_size_type == 3) {
                        return array_size[3];
                    } else if (array_size[4] != "" && temp_size_type == 4) {
                        return array_size[4];
                    } else if (array_size[5] != "" && temp_size_type == 5) {
                        return array_size[5];
                    } else if (array_size[6] != "" && temp_size_type == 6) {
                        return array_size[6];
                    } else if (array_size[7] != "" && temp_size_type == 7) {
                        return array_size[7];
                    } else if (array_size[8] != "" && temp_size_type == 8) {
                        return array_size[8];
                    } else if (array_size[9] != "" && temp_size_type == 9) {
                        return array_size[9];
                    } else if (array_size[10] != "" && temp_size_type == 10) {
                        return array_size[10];
                    } else if (array_size[11] != "" && temp_size_type == 11) {
                        return array_size[11];
                    } else if (array_size[12] != "" && temp_size_type == 12) {
                        return array_size[12];
                    } else {
                        append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL.", Color.Red, Color.Yellow);
                        Debug.WriteLine("all FALSE7");
                        return "";
                    }
                } catch (Exception ex) {
                    append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL." + ex.ToString(), Color.Red, Color.Yellow);
                    return "";
                }
            } else if(search_method==1) {
                //Auto
                try {
                    if (array_size[0] != "" && temp_size_type == 0) {
                        return array_size[0];
                    } else if (array_size[1] != "" && temp_size_type <= 1) {
                        return array_size[1];
                    } else if (array_size[2] != "" && temp_size_type <= 2) {
                        return array_size[2];
                    } else if (array_size[3] != "" && temp_size_type <= 3) {
                        return array_size[3];
                    } else if (array_size[4] != "" && temp_size_type <= 4) {
                        return array_size[4];
                    } else if (array_size[5] != "" && temp_size_type <= 5) {
                        return array_size[5];
                    } else if (array_size[6] != "" && temp_size_type <= 6) {
                        return array_size[6];
                    } else if (array_size[7] != "" && temp_size_type <= 7) {
                        return array_size[7];
                    } else if (array_size[8] != "" && temp_size_type <= 8) {
                        return array_size[8];
                    } else if (array_size[9] != "" && temp_size_type <= 9) {
                        return array_size[9];
                    } else if (array_size[10] != "" && temp_size_type <= 10) {
                        return array_size[10];
                    } else if (array_size[11] != "" && temp_size_type <= 11) {
                        return array_size[11];
                    } else if (array_size[12] != "" && temp_size_type <= 12) {
                        return array_size[12];
                    } else {
                        if (array_size[0]!="") {
                            return array_size[0];
                        } else {
                            append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL.", Color.Red, Color.Yellow);
                            Debug.WriteLine("all FALSE8");
                            return "";
                        }
                    }
                } catch (Exception ex) {
                    append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL." + ex.ToString(), Color.Red, Color.Yellow);
                    return "";
                }
            } else {
                //At least
                try {
                    if (array_size[0] != "") {
                        return array_size[0];
                    } else if (array_size[1] != "") {
                        return array_size[1];
                    } else if (array_size[2] != "" && temp_size_type >= 1) {
                        return array_size[2];
                    } else if (array_size[3] != "" && temp_size_type >= 2) {
                        return array_size[3];
                    } else if (array_size[4] != "" && temp_size_type >= 3) {
                        return array_size[4];
                    } else if (array_size[5] != "" && temp_size_type >= 4) {
                        return array_size[5];
                    } else if (array_size[6] != "" && temp_size_type >= 5) {
                        return array_size[6];
                    } else if (array_size[7] != "" && temp_size_type >= 6) {
                        return array_size[7];
                    } else if (array_size[8] != "" && temp_size_type >= 7) {
                        return array_size[8];
                    } else if (array_size[9] != "" && temp_size_type >= 8) {
                        return array_size[9];
                    } else if (array_size[10] != "" && temp_size_type >= 9) {
                        return array_size[10];
                    } else if (array_size[11] != "" && temp_size_type >= 10) {
                        return array_size[11];
                    } else if (array_size[12] != "" && temp_size_type >= 11) {
                        return array_size[12];
                    } else {
                        if (array_size[0] != "") {
                            return array_size[0];
                        } else {
                            append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL.", Color.Red, Color.Yellow);
                            Debug.WriteLine("all FALSE9");
                            return "";
                        }
                    }
                } catch (Exception ex) {
                    append_msg_THREAD("錯誤，無法解析網址。Error! Can't resolve URL." + ex.ToString(), Color.Red, Color.Yellow);
                    return "";
                }
            }
        }

        public void init_string_2di_array(ref string[][] str) {
            for (int i=0;i<str.Length;i++) {
                str[i] = new string[] { };
            }
        }

        public string[] string_2di_array_to_1di_array (string[][] str) {
            try {
                int count_all_elelment = 0;
                for (int i=0;i<str.Length;i++ ) {
                    count_all_elelment += str[i].Length;
                }
                string[] str_output = new string[count_all_elelment];
                int j = 0;
                for ( int i = 0; i < str.Length; i++ ) {
                    for (int i2=0;i2<str[i].Length;i2++ ) {
                        str_output[j] = str[i][i2];
                        j += 1;
                    }
                }
                return str_output;
            } catch (Exception) {
                Debug.WriteLine("DI ERROR");
                return (new string[] { } );
            }
        }

        private void timer1_Tick ( object sender, EventArgs e ) {
            for (int i=0;i<4;i++) {
                if ((now_byte[i] - last_byte[i])<0) {

                } else {
                    speed_kbyte[i] = Math.Round((double)((now_byte[i] - last_byte[i]) * 2 / 1024), 3);
                }
                if (now_byte[i] >= last_byte[i]) {
                    last_byte[i] = now_byte[i];
                } else {
                    last_byte[i] = 0;
                }
            }
            Debug.WriteLine("timer1 - run");
        }

        private void check_download_is_freeze_Tick ( object sender, EventArgs e ) {
            if ( timer1.Enabled && !pause_download ) {
                for (int i=0;i<4;i++) {
                    if (!thread_busy[i]) {
                        continue;
                    }
                    if ( speed_kbyte[i] == 0 ) {
                        freeze_time[i] += 1;
                        Debug.WriteLine("Freeze t" + (i+1) + ":" + freeze_time[i]);
                    } else {
                        freeze_time[i] = 0;
                    }
                    if ( freeze_time[i] >= 15) {
                        live_X[thread_download_number[i]] = false;
                        download_ok[i] = true;
                        freeze_time[i] = 0;
                        speed_kbyte[i] = 1;
                        Debug.WriteLine("Freeze t" + (i + 1) + " to dead.");
                        if ( current_downloader_key[i] != "" ) {
                            dead_or_not.Add(current_downloader_key[i], true);
                        } else {
                            Debug.WriteLine("current key is empty!why?");
                        }
                    }
                }
            }
            if (switch_get_busy_count()==0) {
                timer1_STOP = true;
            }
        }

        public void set_status_taskbar (int v2,int m2,int v3,int m3) {
            if ( Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported ) {
                var taskbarInstance = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                taskbarInstance.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Normal);
                if ( (types_ == 1 || types_ == 6)  && tabs_==0 ) {
                    taskbarInstance.SetProgressValue(v2, m2);
                    temp_value_taskbar = v2;
                    temp_max_taskbar = m2;
                } else if ( tabs_ == 1 ) {
                    taskbarInstance.SetProgressValue(v2, m2);
                    temp_value_taskbar = v2;
                    temp_max_taskbar = m2;
                } else {
                    if ( m3 == 1 ) {
                        taskbarInstance.SetProgressValue(v2, m2);
                        temp_value_taskbar = v2;
                        temp_max_taskbar = m2;
                    } else if ( m3 != 1 ) {
                        taskbarInstance.SetProgressValue(v3, m3);
                        temp_value_taskbar = v3;
                        temp_max_taskbar = m3;
                    }
                }
            }
        }

        public void set_status_taskbar(int value_, int max_) {
            if (Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported) {
                var taskbarInstance = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                taskbarInstance.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Normal);
                taskbarInstance.SetProgressValue(value_, max_);
            }
        }

        public void set_status_finish () {
            if ( Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported ) {
                var taskbarInstance = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                taskbarInstance.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.NoProgress);
            }
        }

        private void Form1_FormClosing ( object sender, FormClosingEventArgs e ) {
            if ( allow_this_close || exe_allow_exit==1 ) {
                if (download_started) {
                    if (MessageBox.Show(this,"正在下載中，確定關閉？\nIt's downloading now, are you sure to close application?","確定關閉 Are you sure?",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No) {
                        e.Cancel = true;
                        return;
                    }
                }
                form_must_close = true;
            } else {
                notifyIcon1.Visible = true;
                e.Cancel = true;
                this.Hide();
                if ( exe_start_first == 1) {
                    notifyIcon1.ShowBalloonTip(5000);
                    exe_start_first = 0;
                }
            }
            write_config();
            if (!e.Cancel) {
                ChangeClipboardChain(this.Handle, _clipboardViewerNext);
            }
        }

        private void button2_Click ( object sender, EventArgs e ) {
            if (download_started!=true) {
                return;
            }
            if ( button2.Text == "暫停 Pause" ) {
                button2.Text = "繼續 Resume";
                pause_download = true;
                auto_save();
            } else {
                pause_download = false;
                button2.Text = "暫停 Pause";
                if (System.IO.File.Exists(Application.StartupPath + "\\auto_save.fpsav") ) {
                    try {
                        System.IO.File.Delete(Application.StartupPath + "\\auto_save.fpsav");
                    } catch {

                    }
                }
            }
            write_config();
        }

        public bool check_is_my_key_dead (string mykey) {
            // true = > dead;
            if (dead_or_not.ContainsKey(mykey)) {
                if ( dead_or_not[mykey] ) {
                    return true;
                }
            }
            return false;
        }

        public void auto_save () {
            //自動存檔
            if ( download_started && pause_download != true ) {
                return;
            }

            string file_content = "";
            if (download_started) {
                file_content = String.Join(",", fast_split(tab1_textBox1.Text, CrLf)) + "|||" + types_ + "|||" + (options_1_orginal_names) + "|||" + bool_to_str(options_2_auto_make_dirs) + "|||" + bool_to_str(options_3_preview) + "|||" + toolStripProgressBar2.Value + "|||" + toolStripProgressBar2.Maximum + "|||" + toolStripProgressBar3.Value + "|||" + toolStripProgressBar3.Maximum + "|||1|||" + options_4_size + "|||" + save_dir + "|||1|||" + String.Join(CrLf, photo_url_i_ARR_save) + "|||" + String.Join(CrLf, photo_name_i_ARR_save).Replace("|","-") + "|||" + String.Join(",", live_X) + "|||" + album_name_X_SAVE + "|||" + bool_to_str(options_6_skip_download) + "|||" + tabs_ + "|||" + search_keyword.Replace("|||", "") + "|||" + search_user_id + "|||" + search_group_id + "|||" + search_userid_type + "|||" + search_userid_privacy + "|||" + search_safe_search + "|||" + search_perpage + "|||" + search_startpage + "|||" + search_fetchpage + "|||" + bool_to_str(options_8_video_download);
            } else if (download_started == false) {
                file_content = String.Join(",", fast_split(tab1_textBox1.Text, CrLf)) + "|||" + types_ + "|||" + (options_1_orginal_names) + "|||" + bool_to_str(options_2_auto_make_dirs) + "|||" + bool_to_str(options_3_preview) + "|||" + toolStripProgressBar2.Value + "|||" + toolStripProgressBar2.Maximum + "|||" + toolStripProgressBar3.Value + "|||" + toolStripProgressBar3.Maximum + "|||0|||" + options_4_size + "|||" + save_dir + "|||1|||" + String.Join(CrLf, photo_url_i_ARR_save) + "|||" + String.Join(CrLf, photo_name_i_ARR_save).Replace("|", "-") + "|||" + String.Join(",", live_X) + "|||" + album_name_X_SAVE + "|||" + bool_to_str(options_6_skip_download) + "|||" + tabs_ + "|||" + search_keyword.Replace("|||", "") + "|||" + search_user_id + "|||" + search_group_id + "|||" + search_userid_type + "|||" + search_userid_privacy + "|||" + search_safe_search + "|||" + search_perpage + "|||" + search_startpage + "|||" + search_fetchpage + "|||" + "|||" + bool_to_str(options_8_video_download);
            }

            try {
                System.IO.File.WriteAllBytes(Application.StartupPath + "\\auto_save.fpsav", System.Text.UTF8Encoding.UTF8.GetBytes(file_content));
            } catch {

            }
        }

        /*
         * 00 - url_list_ARRAY
         * 01 - types_ 
         * 02 - options_1_orginal_names
         * 03 - options_2_auto_make_dirs
         * 04 - options_3_preview
         * 05 - toolStripProgressBar2.Value
         * 06 - toolStripProgressBar2.Maximum
         * 07 - toolStripProgressBar3.Value
         * 08 - toolStripProgressBar3.Maximum
         * 09 - download_started
         * 10 - options_4_size
         * 11 - save_dir
         * 12 - save_file_20 - always to be "1"
         * 13 - photo_url_i_ARR_save
         * 14 - photo_name_i_ARR_save
         * 15 - live_X
         * 16 - album_name_X_SAVE
         * 17 - options_6_skip_download
         * 18 - tabs_;
         * 19 - search keyword
         * 20 - search user_id
         * 21 - search group_id
         * 22 - search userid type
         * 23 - search userid privacyfilter
         * 24 - search safesearch
         * 25 - search per page number
         * 26 - search start page number
         * 27 - search fetch page number
         * 28 - options_8_video_download
         *
         */

        private void button3_Click ( object sender, EventArgs e ) {
            //存檔按鈕
            if (download_started && pause_download!=true) {
                append_msg_THREAD("請先暫停！(Please pause first.)",Color.Red, bg_color_exe);
                msgbox_nonBlock("請先暫停！\r\n(Please pause first.)", "儲存紀錄");
                return;
            }

            string file_content = "";
            if (download_started) {
                file_content = String.Join(",", fast_split(tab1_textBox1.Text, CrLf)) + "|||" + types_ + "|||" + (options_1_orginal_names) + "|||" + bool_to_str(options_2_auto_make_dirs) + "|||" + bool_to_str(options_3_preview) + "|||" + toolStripProgressBar2.Value + "|||" + toolStripProgressBar2.Maximum + "|||" + toolStripProgressBar3.Value + "|||" + toolStripProgressBar3.Maximum + "|||1|||" + options_4_size + "|||" + save_dir + "|||1|||" + String.Join(CrLf, photo_url_i_ARR_save) + "|||" + String.Join(CrLf, photo_name_i_ARR_save).Replace("|", "-") + "|||" + String.Join(",", live_X) + "|||" + album_name_X_SAVE + "|||" + bool_to_str(options_6_skip_download) + "|||" + tabs_ + "|||" + search_keyword.Replace("|||","") + "|||" + search_user_id + "|||" + search_group_id + "|||" + search_userid_type + "|||" + search_userid_privacy + "|||" + search_safe_search + "|||" + search_perpage + "|||" + search_startpage + "|||" + search_fetchpage + "|||" + bool_to_str(options_8_video_download);
            } else if (download_started == false) {
                file_content = String.Join(",", fast_split(tab1_textBox1.Text, CrLf)) + "|||" + types_ + "|||" + (options_1_orginal_names) + "|||" + bool_to_str(options_2_auto_make_dirs) + "|||" + bool_to_str(options_3_preview) + "|||" + toolStripProgressBar2.Value + "|||" + toolStripProgressBar2.Maximum + "|||" + toolStripProgressBar3.Value + "|||" + toolStripProgressBar3.Maximum + "|||0|||" + options_4_size + "|||" + save_dir + "|||1|||" + String.Join(CrLf, photo_url_i_ARR_save) + "|||" + String.Join(CrLf, photo_name_i_ARR_save).Replace("|", "-") + "|||" + String.Join(",", live_X) + "|||" + album_name_X_SAVE + "|||" + bool_to_str(options_6_skip_download) + "|||" + tabs_ + "|||" + search_keyword.Replace("|||", "") + "|||" + search_user_id + "|||" + search_group_id + "|||" + search_userid_type + "|||" + search_userid_privacy + "|||" + search_safe_search + "|||" + search_perpage + "|||" + search_startpage + "|||" + search_fetchpage + "|||" + bool_to_str(options_8_video_download);
            }

            if (saveFileDialog1.ShowDialog()==DialogResult.OK) {
                System.IO.File.WriteAllBytes(saveFileDialog1.FileName,System.Text.UTF8Encoding.UTF8.GetBytes(file_content));
                msgbox_nonBlock("存檔完畢！\r\nFile saved successfully.", "存檔");
            }

        }

        public void msgbox_nonBlock(string text_,string title_) {
            this.Invoke(new Action(() => {
                msgbox_form msgbox_f = new msgbox_form();
                msgbox_f.text_ = (text_.Replace("\n","\r\n"));
                msgbox_f.Text = title_;
                msgbox_f.Show();
            }));
        }

        private void clean_garbage_Tick ( object sender, EventArgs e ) {
            GC.Collect();
        }

        public int safe_int_parse (string str) {
            int s = 0;
            int.TryParse(str, out s);
            return s;
        }

        public string bool_to_str (bool bo) {
            if ( bo ) {
                return "1";
            } else {
                return "0";
            }
        }

        public bool str_to_bo ( string str ) {
            if ( str == "1" ) {
                return true;
            } else {
                return false;
            }
        }

        public bool[] get_BOOL_array(string str) {
            string[] strx = fast_split(str, ",");
            bool[] return_BOOL_arr = new bool[strx.Length];
            for (int i=0;i<strx.Length;i++) {
                if (strx[i].ToLower()=="true") {
                    return_BOOL_arr[i] = true;
                } else {
                    return_BOOL_arr[i] = false;
                }
            }
            return return_BOOL_arr;
        }

        public static int get_options_by_str_special(string str) {
            str = str.ToLower();
            if (str=="true" || str=="1") {
                return 1;
            }else if (str == "false" || str == "0") {
                return 0;
            } else {
                int temp_int = 3;
                int.TryParse(str, out temp_int);
                return temp_int;
            }
        }

        /*
         * 00 - url_list_ARRAY
         * 01 - types_ 
         * 02 - options_1_orginal_names
         * 03 - options_2_auto_make_dirs
         * 04 - options_3_preview
         * 05 - toolStripProgressBar2.Value
         * 06 - toolStripProgressBar2.Maximum
         * 07 - toolStripProgressBar3.Value
         * 08 - toolStripProgressBar3.Maximum
         * 09 - download_started
         * 10 - options_4_size
         * 11 - save_dir
         * 12 - save_file_20 - always to be "1"
         * 13 - photo_url_i_ARR_save
         * 14 - photo_name_i_ARR_save
         * 15 - live_X
         * 16 - album_name_X_SAVE
         * 17 - options_6_skip_download
         * 18 - tabs_;
         * 19 - search keyword
         * 20 - search user_id
         * 21 - search group_id
         * 22 - search userid type
         * 23 - search userid privacyfilter
         * 24 - search safesearch
         * 25 - search per page number
         * 26 - search start page number
         * 27 - search fetch page number
         * 28 - options_8_video_download
         * 
         */

        private void button4_Click ( object sender, EventArgs e ) {
            if (download_started) {
                append_msg_THREAD("請別在下載時讀進度。(Please do not read save file when you are downloading something.)",color_f_m, bg_color_exe);
                msgbox_nonBlock("請別在下載時讀進度。\r\n(Please do not read save file when you are downloading something.)", "讀取存檔");
                return;
            }
            if (openFileDialog1.ShowDialog()==DialogResult.OK) {
                try {
                    string file_content = System.Text.UTF8Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(openFileDialog1.FileName));
                    string[] file_split = fast_split(file_content,"|||");
                    if ( file_split.Length < 30 ) {
                        url_ = fast_split(file_split[0], ",");
                        tab1_textBox1.Text = String.Join(CrLf, url_);
                        types_ = safe_int_parse(file_split[1]);
                        if ( types_ == 0 ) {
                            tab1_radioButton1.Checked = true;
                        } else if ( types_ == 1 ) {
                            tab1_radioButton2.Checked = true;
                        } else if ( types_ == 2 ) {
                            tab1_radioButton3.Checked = true;
                        } else if ( types_ == 3 ) {
                            tab1_radioButton4.Checked = true;
                        } else if ( types_ == 4 ) {
                            tab1_radioButton5.Checked = true;
                        } else if (types_ == 5) {
                            tab1_radioButton6.Checked = true;
                        } else if (types_ == 6) {
                            tab1_radioButton7.Checked = true;
                        } else if (types_ == 7) {
                            tab1_radioButton8.Checked = true;
                        }
                        options_2_auto_make_dirs = str_to_bo(file_split[3]);
                        options_3_preview = str_to_bo(file_split[4]);
                        try {
                            options_4_size = int.Parse(file_split[10]);
                        } catch {
                            options_4_size = 0;
                        }
                        try {
                            save_dir = file_split[11];
                            cfbd.SelectedPath = file_split[11];
                        } catch {
                            save_dir = "";
                        }
                        try {
                            file_name_setting_obj.SelectedIndex = get_options_by_str_special(file_split[2]);
                            options_1_orginal_names = get_options_by_str_special(file_split[2]);
                        } catch {

                        }
                        try {
                            options_6_skip_download = str_to_bo(file_split[17]);
                        } catch {

                        }
                        auto_making_dir_setting_obj.Checked = options_2_auto_make_dirs;
                        //preview_photos_setting_obj.Checked = options_3_preview;
                        skip_download_setting_obj.Checked = options_6_skip_download;
                        photos_sizes_setting_obj.SelectedIndex = options_4_size;
                        int v2 = safe_int_parse(file_split[5]);
                        int m2 = safe_int_parse(file_split[6]);
                        int v3 = safe_int_parse(file_split[7]);
                        int m3 = safe_int_parse(file_split[8]);
                        bool started = str_to_bo(file_split[9]);
                        if (v2<=m2 && m2>0) {
                            toolStripProgressBar2.Maximum = m2;
                            toolStripProgressBar2.Value = v2;
                            toolStripProgressBar2.ToolTipText = "(" + v2 + "/" + m2 + ")";
                        }
                        if ( v3 <= m3 && m3 > 0 ) {
                            toolStripProgressBar3.Maximum = m3;
                            toolStripProgressBar3.Value = v3;
                            toolStripProgressBar3.ToolTipText = "(" + v3 + "/" + m3 + ")";
                        }
                        save_v2 = v2;
                        save_v3 = v3;
                        read_from_save_file = true;
                        save_file_20 = false;
                        try {
                            if (file_split.Length>=13 && file_split[12].ToString()=="1") {
                                save_file_20 = true;
                            }
                        } catch {

                        }
                        if (save_file_20) {
                            photo_url_i_ARR_save = fast_split(file_split[13], CrLf);
                            photo_name_i_ARR_save = fast_split(file_split[14], CrLf);
                            live_X = get_BOOL_array(file_split[15]);
                            album_name_X_SAVE = (file_split[16]);
                        }
                        Debug.WriteLine("save file 2.0 :" + save_file_20);
                        //Save File 3.0
                        tabs_ = 0;
                        try {
                            tabs_ = int.Parse(file_split[18]);
                            tabControl1.SelectedIndex = tabs_;
                        } catch {
                            tabs_ = 0;
                            tabControl1.SelectedIndex = tabs_;
                        }
                        try {
                            options_8_video_download = str_to_bo(file_split[28]);
                            video_download_setting_obj.Tag = "s";
                            video_download_setting_obj.Checked = options_8_video_download;
                        } catch (Exception) {

                        }
                        try {
                            if (!signed_auth_bool) {
                                if (safe_int_parse(file_split[22])==1 || safe_int_parse(file_split[24])!=0) {
                                    read_from_save_file = false;
                                    msgbox_nonBlock("您因未授權本程式，故無法使用此需要授權的紀錄檔！\r\nFile reading FAIL. This app must be authorized by you.", "讀取存檔");
                                    return;
                                }
                            }
                            search_keyword = file_split[19];
                            search_user_id = file_split[20];
                            search_group_id = file_split[21];
                            tab2_search_textbox.Text = search_keyword;
                            tab2_search_userid_textbox.Text = search_user_id;
                            tab2_search_groupid_textbox.Text = search_group_id;
                            search_userid_type = safe_int_parse(file_split[22]);
                            search_userid_privacy = safe_int_parse(file_split[23]);
                            search_safe_search = safe_int_parse(file_split[24]);
                            if (search_userid_type == 1) {
                                tab2_userid_selectbox_type.Tag = 1;
                                tab2_userid_selectbox_type.SelectedIndex = 1;
                                tab2_search_userid_textbox.Enabled = false;
                                tab2_search_userid_textbox.Text = "me";
                                tab2_userid_selectbox_privacyfilter.Tag = 1;
                                tab2_userid_selectbox_privacyfilter.SelectedIndex = search_userid_privacy;
                                tab2_userid_selectbox_privacyfilter.Visible = true;
                            } else {
                                tab2_userid_selectbox_type.Tag = 1;
                                tab2_userid_selectbox_type.SelectedIndex = 0;
                                tab2_search_userid_textbox.Enabled = true;
                                tab2_search_userid_textbox.Text = "";
                                search_userid_privacy = 0;
                                tab2_userid_selectbox_privacyfilter.Tag = 1;
                                tab2_userid_selectbox_privacyfilter.SelectedIndex = 0;
                                tab2_userid_selectbox_privacyfilter.Visible = false;
                            }
                            tab2_safesearch_selectbox.Tag = 1;
                            tab2_safesearch_selectbox.SelectedIndex = search_safe_search;
                            search_perpage = safe_int_parse(file_split[25]);
                            search_startpage = safe_int_parse(file_split[26]);
                            search_fetchpage = safe_int_parse(file_split[27]);
                            try {
                                tab2_search_per_page_obj.Value = search_perpage;
                                tab2_search_start_page_obj.Value = search_startpage;
                                tab2_search_fetch_page_obj.Value = search_fetchpage;
                            } catch {

                            }
                        } catch {

                        }
                        if ( started ) {
                            button1.PerformClick();
                        }
                    } else {
                        Debug.WriteLine("read file length error");
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(ex.ToString());
                }
                msgbox_nonBlock("讀檔完畢！\r\nFile read successfully.", "讀取存檔");
            }

        }

        private void timer2_Tick ( object sender, EventArgs e ) {
            read_last_file.Stop();
            if ( System.IO.File.Exists(Application.StartupPath + "\\auto_save.fpsav") ) {
                if ( MessageBox.Show("讀取自動存檔？Read last auto-save file?", "發現自動存檔", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes ) {
                    try {
                        string file_content = System.Text.UTF8Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(Application.StartupPath + "\\auto_save.fpsav"));
                        string[] file_split = fast_split(file_content, "|||");
                        if ( file_split.Length <= 30 ) {
                            url_ = fast_split(file_split[0], ",");
                            tab1_textBox1.Text = String.Join(CrLf, url_);
                            types_ = safe_int_parse(file_split[1]);
                            if (types_ == 0) {
                                tab1_radioButton1.Checked = true;
                            } else if (types_ == 1) {
                                tab1_radioButton2.Checked = true;
                            } else if (types_ == 2) {
                                tab1_radioButton3.Checked = true;
                            } else if (types_ == 3) {
                                tab1_radioButton4.Checked = true;
                            } else if (types_ == 4) {
                                tab1_radioButton5.Checked = true;
                            } else if (types_ == 5) {
                                tab1_radioButton6.Checked = true;
                            } else if (types_ == 6) {
                                tab1_radioButton7.Checked = true;
                            } else if (types_ == 7) {
                                tab1_radioButton8.Checked = true;
                            }
                            options_2_auto_make_dirs = str_to_bo(file_split[3]);
                            options_3_preview = str_to_bo(file_split[4]);
                            try {
                                options_4_size = int.Parse(file_split[10]);
                            } catch {
                                options_4_size = 0;
                            }
                            try {
                                save_dir = file_split[11];
                                cfbd.SelectedPath = file_split[11];
                            } catch {
                                save_dir = "";
                            }
                            try {
                                file_name_setting_obj.SelectedIndex = get_options_by_str_special(file_split[2]);
                                options_1_orginal_names = get_options_by_str_special(file_split[2]);
                            } catch {

                            }
                            try {
                                options_6_skip_download = str_to_bo(file_split[17]);
                            } catch {

                            }
                            auto_making_dir_setting_obj.Checked = options_2_auto_make_dirs;
                            //preview_photos_setting_obj.Checked = options_3_preview;
                            skip_download_setting_obj.Checked = options_6_skip_download;
                            photos_sizes_setting_obj.SelectedIndex = options_4_size;
                            int v2 = safe_int_parse(file_split[5]);
                            int m2 = safe_int_parse(file_split[6]);
                            int v3 = safe_int_parse(file_split[7]);
                            int m3 = safe_int_parse(file_split[8]);
                            bool started = str_to_bo(file_split[9]);
                            if (v2 <= m2 && m2 > 0) {
                                toolStripProgressBar2.Maximum = m2;
                                toolStripProgressBar2.Value = v2;
                                toolStripProgressBar2.ToolTipText = "(" + v2 + "/" + m2 + ")";
                            }
                            if (v3 <= m3 && m3 > 0) {
                                toolStripProgressBar3.Maximum = m3;
                                toolStripProgressBar3.Value = v3;
                                toolStripProgressBar3.ToolTipText = "(" + v3 + "/" + m3 + ")";
                            }
                            save_v2 = v2;
                            save_v3 = v3;
                            read_from_save_file = true;
                            save_file_20 = false;
                            try {
                                if (file_split.Length >= 13 && file_split[12].ToString() == "1") {
                                    save_file_20 = true;
                                }
                            } catch {

                            }
                            if (save_file_20) {
                                photo_url_i_ARR_save = fast_split(file_split[13], CrLf);
                                photo_name_i_ARR_save = fast_split(file_split[14], CrLf);
                                live_X = get_BOOL_array(file_split[15]);
                                album_name_X_SAVE = (file_split[16]);
                            }
                            Debug.WriteLine("save file 2.0 :" + save_file_20);
                            //Save File 3.0
                            tabs_ = 0;
                            try {
                                tabs_ = int.Parse(file_split[18]);
                                tabControl1.SelectedIndex = tabs_;
                            } catch {
                                tabs_ = 0;
                                tabControl1.SelectedIndex = tabs_;
                            }
                            try {
                                options_8_video_download = str_to_bo(file_split[28]);
                                video_download_setting_obj.Tag = "s";
                                video_download_setting_obj.Checked = options_8_video_download;
                            } catch (Exception) {

                            }
                            try {
                                if (!signed_auth_bool) {
                                    if (safe_int_parse(file_split[22]) == 1 || safe_int_parse(file_split[24]) != 0) {
                                        read_from_save_file = false;
                                        msgbox_nonBlock("您因未授權本程式，故無法使用此需要授權的紀錄檔！\r\nFile reading FAIL. This app must be authorized by you.", "讀取存檔");
                                        return;
                                    }
                                }
                                search_keyword = file_split[19];
                                search_user_id = file_split[20];
                                search_group_id = file_split[21];
                                tab2_search_textbox.Text = search_keyword;
                                tab2_search_userid_textbox.Text = search_user_id;
                                tab2_search_groupid_textbox.Text = search_group_id;
                                search_userid_type = safe_int_parse(file_split[22]);
                                search_userid_privacy = safe_int_parse(file_split[23]);
                                search_safe_search = safe_int_parse(file_split[24]);
                                if (search_userid_type == 1) {
                                    tab2_userid_selectbox_type.Tag = 1;
                                    tab2_userid_selectbox_type.SelectedIndex = 1;
                                    tab2_search_userid_textbox.Enabled = false;
                                    tab2_search_userid_textbox.Text = "me";
                                    tab2_userid_selectbox_privacyfilter.Tag = 1;
                                    tab2_userid_selectbox_privacyfilter.SelectedIndex = search_userid_privacy;
                                    tab2_userid_selectbox_privacyfilter.Visible = true;
                                } else {
                                    tab2_userid_selectbox_type.Tag = 1;
                                    tab2_userid_selectbox_type.SelectedIndex = 0;
                                    tab2_search_userid_textbox.Enabled = true;
                                    tab2_search_userid_textbox.Text = "";
                                    search_userid_privacy = 0;
                                    tab2_userid_selectbox_privacyfilter.Tag = 1;
                                    tab2_userid_selectbox_privacyfilter.SelectedIndex = 0;
                                    tab2_userid_selectbox_privacyfilter.Visible = false;
                                }
                                tab2_safesearch_selectbox.Tag = 1;
                                tab2_safesearch_selectbox.SelectedIndex = search_safe_search;
                                search_perpage = safe_int_parse(file_split[25]);
                                search_startpage = safe_int_parse(file_split[26]);
                                search_fetchpage = safe_int_parse(file_split[27]);
                                try {
                                    tab2_search_per_page_obj.Value = search_perpage;
                                    tab2_search_start_page_obj.Value = search_startpage;
                                    tab2_search_fetch_page_obj.Value = search_fetchpage;
                                } catch {

                                }
                            } catch {

                            }
                            if (started) {
                                button1.PerformClick();
                            }
                        } else {
                            Debug.WriteLine("read file length error");
                        }
                    } catch ( Exception ex ) {
                        Debug.WriteLine(ex.ToString());
                    }
                } else {
                    try {
                        System.IO.File.Delete(Application.StartupPath + "\\auto_save.fpsav");
                    } catch {

                    }
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick ( object sender, MouseEventArgs e ) {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.TopMost = true;
            this.BringToFront();
            this.TopMost = false;
            if (download_started) {
                if (temp_max_taskbar >= temp_value_taskbar && temp_value_taskbar >= 0) {
                    set_status_taskbar(temp_value_taskbar, temp_max_taskbar);
                }
            }
        }

        private void 關閉ToolStripMenuItem_Click ( object sender, EventArgs e ) {
            allow_this_close = true;
            form_must_close = true;
            this.Close();
        }

        private void 背景顏色ToolStripMenuItem_Click ( object sender, EventArgs e ) {
            colorDialog1.Color = Color.FromArgb(exe_bg_color);
            if (colorDialog1.ShowDialog()==DialogResult.OK) {
                bg_color_exe = colorDialog1.Color;
                exe_bg_color = colorDialog1.Color.ToArgb();
                write_config();
                read_config();
                set_from_config();
            }
        }

        private void 文字顏色ToolStripMenuItem_Click ( object sender, EventArgs e ) {
            colorDialog1.Color = Color.FromArgb(exe_f_color);
            if ( colorDialog1.ShowDialog() == DialogResult.OK ) {
                this.ForeColor = colorDialog1.Color;
                exe_f_color = colorDialog1.Color.ToArgb();
                write_config();
                read_config();
                set_from_config();
            }
        }

        private void 按鈕顏色ToolStripMenuItem_Click ( object sender, EventArgs e ) {
            colorDialog1.Color = Color.FromArgb(exe_button_color);
            if ( colorDialog1.ShowDialog() == DialogResult.OK ) {
                button1.ForeColor = colorDialog1.Color;
                button2.ForeColor = colorDialog1.Color;
                button3.ForeColor = colorDialog1.Color;
                button4.ForeColor = colorDialog1.Color;
                exe_button_color = colorDialog1.Color.ToArgb();
                write_config();
            }
        }

        private void 直接關閉未啟用ToolStripMenuItem_Click ( object sender, EventArgs e ) {
            if ( exe_allow_exit == 1 ) {
                直接關閉未啟用ToolStripMenuItem.Text = "直接關閉=未啟用";
                exe_allow_exit = 0;
            } else {
                直接關閉未啟用ToolStripMenuItem.Text = "直接關閉=啟用";
                exe_allow_exit = 1;
            }
        }

        public bool check_update_method(bool silent_check) {
            if (beta_version) {
                if (MessageBox.Show(this, "本版本為Beta！此為更新測試，請問是否更新？\n\nBeta Version Update Found!Do you want to update?", "更新 Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes) {
                    return true;
                } else {
                    return false;
                }
            }
            WebClient_auto wbcl_ = new WebClient_auto();
            string response = wbcl_.DownloadString("http://flickr_downloader.weil.app.wbftw.org/api/flickr/app_version.txt").Trim(new char[] { (char)10,(char)13,(char)32 });
            Debug.WriteLine(response);
            byte current_update_state = check_version(response);
            if (current_update_state == 0) {
                if (!silent_check) {
                    MessageBox.Show(this, "您已安裝最新版的產品，無須更新。\n\nThe product is the latest version. No need to update.", "更新 Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } else if (current_update_state == 1) {
                if (MessageBox.Show(this, "發現更新！是否更新版本為" + response + "？\n\nNew Update Found!Do you want to update to version " + response + "?", "更新 Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes) {
                    return true;
                }
            } else {
                if (!silent_check) {
                    MessageBox.Show(this, "檢查失敗，無法確定是否為最新版。\n\nChecking FAIL. Is server currently down or no network connection.", "更新 Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return false;
        }

        public void check_and_download() {
            try {
                if (check_update_method(false)) {
                    //download
                    if (download_started) {
                        msgbox_error("錯誤Error","請先停止下載，否則無法自動更新！\nPlease stop download first! Or you can't upgrade this app.");
                        return;
                    }
                    string temp_url_test = get_new_version_download_url();
                    if (temp_url_test == "") {
                        Process.Start("http://web.wbftw.org/product/flickrdownloader");
                        return;
                    }
                    if (InvokeRequired) {
                        this.Invoke(new Action(() => update_form_show(temp_url_test)));
                        return;
                    }
                }
            } catch {
                msgbox_error("錯誤Error","無法連線至伺服器！Can't connect to server!");
            }
        }

        public string get_new_version_download_url() {
            append_msg_THREAD("取得下載網址中...(Getting download link...)", color_f_m, bg_color_exe);
            string code__ = get_newest_url_code();
            if (code__=="") {
                return "";
            }
            download_started = true;
            string pcloud_api_str = get_response_from_url_DO_event("https://api.pcloud.com/getpublinkdownload?code=" + code__);
            download_started = false;
            JObject jObj = (JObject)JsonConvert.DeserializeObject(pcloud_api_str);//dynamic
            try {
                return "http://" + jObj["hosts"][0].ToString() + jObj["path"].ToString();
            } catch {
                return "";
            }
        }

        public void update_form_show(string temp_url_test) {
            update_msgbox msgbox = new update_msgbox();
            msgbox.TopMost = true;
            msgbox.target_url = temp_url_test;
            Debug.WriteLine("update download url:" + temp_url_test);
            msgbox.ShowDialog();
            if (msgbox.Tag.ToString() == "1") {
                try {
                    append_msg_THREAD("解壓縮中…(Extracting...)", color_f_m, bg_color_exe);
                    extract_7z_file(Application.StartupPath + "\\" + msgbox.filename);
                    System.IO.File.Delete(Application.StartupPath + "\\" + msgbox.filename);
                    string new_exe_path = Application.StartupPath + "\\Flickr_Downloader_new_" + (new Random().Next(0, 9999)) + ".exe";
                    System.IO.File.Move(Application.StartupPath + "\\update\\Flickr Downloader\\Flickr_Downloader.exe", new_exe_path);
                    Process.Start("\"" + new_exe_path + "\"", "update");
                    System.IO.Directory.Delete(Application.StartupPath + "\\update\\Flickr Downloader\\");
                    System.IO.Directory.Delete(Application.StartupPath + "\\update");
                    allow_this_close = true;
                    this.Close();
                } catch {

                }
            }
        }

        public void silent_check_and_download() {
            try {
                if (check_update_method(true)) {
                    //download
                    if (download_started) {
                        msgbox_error("錯誤Error", "請先停止下載，否則無法自動更新！\nPlease stop download first! Or you can't upgrade this app.");
                        return;
                    }
                    string temp_url_test = get_new_version_download_url();
                    if (temp_url_test == "") {
                        Process.Start("http://web.wbftw.org/product/flickrdownloader");
                        return;
                    }
                    if (InvokeRequired) {
                        this.Invoke(new Action(() => update_form_show(temp_url_test)));
                        return;
                    }
                }
            } catch {
            }
        }

        private void toolStripStatusLabel1_Click ( object sender, EventArgs e ) {
            System.Threading.Thread s = new System.Threading.Thread(() => check_and_download());
            s.Start();
        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e) {
            if (download_started) {
                msgbox_nonBlock("請先停止下載！\r\nPlease stop download first!","授權Auth");
                return;
            }
            if (signed_auth_bool) {
                if (MessageBox.Show(this,"確定登出並且切換帳號？Are you sure to log out and switch account?","更改帳號 Switch Account",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.Cancel) {
                    return;
                }
            }
            get_auth_url();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            options_4_size = photos_sizes_setting_obj.SelectedIndex;
            write_config();
        }

        private void timer2_Tick_1(object sender, EventArgs e) {
            login_test_timer.Stop();
            login_test_FUNC();
        }

        public void show_new_feature() {
            if (update_msg_show_) {
                try {
                    WebClient_auto wbcl_ = new WebClient_auto();
                    wbcl_.Encoding = Encoding.UTF8;
                    string response = wbcl_.DownloadString("http://flickr_downloader.weil.app.wbftw.org/api/flickr/news/" + Application.ProductVersion.ToString() + "/news.txt").Trim(new char[] { (char)10, (char)13, (char)32 });
                    if (response != "") {
                        msgbox_nonBlock(response, "更新內容 New Feature");
                    }
                } catch {

                }
                return;
            }
        }

        public void login_test_FUNC() {
            System.Threading.Thread s = new System.Threading.Thread(() => test_login());
            s.Start();
        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e) {
            if (download_started) {
                msgbox_nonBlock("請先停止當前下載！\n\nPlease STOP download first!","錯誤Error");
                return;
            }
            try {
                System.IO.File.Delete(Application.StartupPath + "\\userinfo.log");
                signed_auth_bool = false;
                toolStripDropDownButton2.Text = "授權此應用(Auth)";
                toolStripDropDownButton3.Visible = false;
                if (search_userid_type==1) {
                    search_userid_type = 0;
                    tab2_userid_selectbox_type.Tag = 1;
                    tab2_userid_selectbox_type.SelectedIndex = 0;
                    tab2_search_userid_textbox.Enabled = true;
                    tab2_search_userid_textbox.Text = "";
                    search_userid_privacy = 0;
                    tab2_userid_selectbox_privacyfilter.Tag = 1;
                    tab2_userid_selectbox_privacyfilter.SelectedIndex = 0;
                    tab2_userid_selectbox_privacyfilter.Visible = false;
                }
                if (search_safe_search!=0) {
                    search_safe_search = 0;
                    tab2_safesearch_selectbox.Tag = 1;
                    tab2_safesearch_selectbox.SelectedIndex = 0;
                }
                append_msg_THREAD("已刪除並登出！Successfully Log out and Delete userinfo!", color_f_g, bg_color_exe);
            } catch {
                append_msg_THREAD("無法刪除紀錄檔！Delete userinfo.log FAIL.", Color.Red, Color.Yellow);
            }
        }

        public void register_Protocol() {
            Microsoft.Win32.RegistryKey main_key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("fdwbft");
            main_key.SetValue("", "\"URL:fdwbft Protocol\"");
            main_key.SetValue("URL Protocol", "\"" + Application.ExecutablePath + "\"");
            Microsoft.Win32.RegistryKey sub_key_shell_open_command = main_key.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command");
            sub_key_shell_open_command.SetValue("", "\"" + Application.ExecutablePath + "\" \"%1\"" );
            sub_key_shell_open_command.Close();
            main_key.Close();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            options_5_thread_num = (toolStripComboBox1.SelectedIndex+1);
            write_config();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e) {
            try {
                Process.Start(e.LinkText.ToString());
            } catch {

            }
        }

        private void check_thread_callui_Tick(object sender, EventArgs e) {
            if (timer1_START) {
                timer1_START = false;
                if (!timer1.Enabled) {
                    timer1.Start();
                }
            }
            if (timer1_STOP) {
                timer1_STOP = false;
                if (timer1.Enabled) {
                    timer1.Stop();
                }
            }
        }

        private void 音效開關開ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (exe_sound_enabled) {
                音效開關開ToolStripMenuItem.Text = "音效開關=關Off";
                exe_sound_enabled = false;
            } else {
                音效開關開ToolStripMenuItem.Text = "音效開關=開On";
                exe_sound_enabled = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.A) {
                if (sender != null) { 
                    ((CustomTextBox)sender).SelectAll();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (exe_clipboard_mon_enabled) {
                剪貼簿偵測ToolStripMenu.Text = "剪貼簿偵測=關Off";
                exe_clipboard_mon_enabled = false;
            } else {
                剪貼簿偵測ToolStripMenu.Text = "剪貼簿偵測=開On";
                exe_clipboard_mon_enabled = true;
            }
        }

        private void 記住存檔位置toolStripMenuItem_Click(object sender, EventArgs e) {
            if (exe_remember_save_path_enabled) {
                記住存檔位置toolStripMenuItem.Text = "記住存檔位置=關Off";
                exe_remember_save_path_enabled = false;
                exe_remember_save_path = "";
            } else {
                記住存檔位置toolStripMenuItem.Text = "記住存檔位置=開On";
                exe_remember_save_path_enabled = true;
                cfbd.Title = "(將記住)選擇存檔資料夾 (Will Remember) Choose a dir to save files.";
                if (cfbd.ShowDialog(this) != DialogResult.OK) {
                    return;
                }
                exe_remember_save_path = cfbd.SelectedPath;
                write_config();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) {
            options_6_skip_download = skip_download_setting_obj.Checked;
            write_config();
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e) {
            try {
                if (MessageBox.Show(this, "回報問題前請先試著排除故障：\r\n\r\n一、請檢查此版本是否為當前最新版本。\r\n二、檢查該相簿/相片/藝術家/群組是否「公開」(登出網頁Flickr)，您是否有權限下載，如果是非公開相片，但您可以檢視，請先授權本程式。\r\n三、檢查網址是否與類別選項對應到(可將滑鼠移動至選項以檢查支援的網址型態。)\r\n四、現在Flickr官方網站伺服器是否無法運作或運行很慢。\r\n\r\n完成以上檢查，並排除非其他因素，確定要回報問題？(描述情形盡可能越詳細越好，如果可以請附上網址。)", "回報問題",MessageBoxButtons.YesNo,MessageBoxIcon.Information)==DialogResult.Yes) {
                    Process.Start("https://weils.net/contact.php");
                }
            } catch {
                msgbox_error("錯誤Error", "無法連線至伺服器！Can't connect to server!");
            }
        }

        private void Skip_append_text_toolStripMenuItem_Click(object sender, EventArgs e) {
            if (exe_show_detail_enabled) {
                Skip_append_text_toolStripMenuItem.Text = "詳細顯示所有資訊=關Off";
                exe_show_detail_enabled = false;
            } else {
                Skip_append_text_toolStripMenuItem.Text = "詳細顯示所有資訊=開On";
                exe_show_detail_enabled = true;
            }
        }

        public static void extract_7z_file(string _7z_file_path) {
            var dirinfo = new System.IO.DirectoryInfo(Application.StartupPath + "\\update\\");
            dirinfo.Create();
            using (var tmp = new SevenZipExtractor(_7z_file_path)) {
                for (int i = 0; i < tmp.ArchiveFileData.Count; i++) {
                    tmp.ExtractFiles(Application.StartupPath + "\\update\\", tmp.ArchiveFileData[i].Index);
                }
            }
        }

        public string get_newest_url_code() {
            string new_upgrade_url = "http://url-weil.wbftw.org/r";
            if (beta_version) {
                new_upgrade_url = "http://url-weil.wbftw.org/t";
            }
            try {
                System.Net.HttpWebRequest req =(HttpWebRequest) System.Net.WebRequest.Create(new_upgrade_url);
                req.AllowAutoRedirect = false;
                req.Proxy = null;
                System.Net.WebResponse resp = req.GetResponse();
                string target_URL = resp.Headers["Location"].ToString();
                Debug.WriteLine(target_URL);
                string[] temp_str = split_two_piece(target_URL, "?code=");
                return temp_str[1];
            } catch {
                return "";
            }
        }

        private void version_name_Click(object sender, EventArgs e) {
            extract_7z_file(Application.StartupPath + "\\Flickr Downloader.7z");
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e) {
            Process.Start("http://web.wbftw.org/product/flickrdownloader");
        }

        private void show_new_feature_timer_Tick(object sender, EventArgs e) {
            show_new_feature_timer.Stop();
            show_new_feature();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            tabs_ = tabControl1.SelectedIndex;
        }

        private void tab2_userid_selectbox_type_SelectedIndexChanged(object sender, EventArgs e) {
            if (tab2_userid_selectbox_type.Tag.ToString()=="1") {
                tab2_userid_selectbox_type.Tag = 0;
                return;
            }
            if (download_started) {
                return;
            }
            if (tab2_userid_selectbox_type.SelectedIndex==0) {
                tab2_userid_selectbox_privacyfilter.Tag = 1;
                tab2_userid_selectbox_privacyfilter.SelectedIndex = 0;
                search_userid_privacy = 0;
                tab2_search_userid_textbox.Text = "";
                tab2_search_userid_textbox.Enabled = true;
                tab2_userid_selectbox_privacyfilter.Visible = false;
                search_userid_type = 0;
            } else {
                if (!signed_auth_bool) {
                    msgbox_error("錯誤Error", "您尚未授權本程式，故無法抓取自己隱私圖片。\n\nYou have NOT authorized this app to fetch your own private photos.");
                    tab2_userid_selectbox_type.Tag= 1;
                    tab2_userid_selectbox_type.SelectedIndex = 0;
                    return;
                }
                search_userid_type = 1;
                tab2_search_userid_textbox.Text = "me";
                tab2_search_userid_textbox.Enabled = false;
                tab2_userid_selectbox_privacyfilter.Visible = true;
                Debug.WriteLine("search_userid_type="+search_userid_type);
            }
        }

        private void tab2_userid_selectbox_privacyfilter_SelectedIndexChanged(object sender, EventArgs e) {
            if (tab2_userid_selectbox_privacyfilter.Tag.ToString() == "1") {
                tab2_userid_selectbox_privacyfilter.Tag = 0;
                return;
            }
            if (download_started) {
                return;
            }
            search_userid_privacy = tab2_userid_selectbox_privacyfilter.SelectedIndex;
            Debug.WriteLine("search_userid_privacy=" + search_userid_privacy);
        }

        private void tab2_safesearch_selectbox_SelectedIndexChanged(object sender, EventArgs e) {
            if (tab2_safesearch_selectbox.Tag.ToString() == "1") {
                tab2_safesearch_selectbox.Tag = 0;
                return;
            }
            if (download_started) {
                return;
            }
            if (!signed_auth_bool) {
                msgbox_error("錯誤Error", "您尚未授權本程式，故無法設定此項目，此僅為登入用戶啟用。\n\nYou have NOT authorized this app so you can't use this function.");
                tab2_safesearch_selectbox.Tag = 1;
                tab2_safesearch_selectbox.SelectedIndex = 0;
                search_safe_search = 0;
                return;
            }
            search_safe_search = tab2_safesearch_selectbox.SelectedIndex;
            Debug.WriteLine("search_safe_search=" + search_safe_search);
        }

        private void tabControl1_SelectedIndexChanging(object sender, TabPageChangeEventArgs e) {
            if (download_started) {
                e.Cancel = true;
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e) {
            if (download_started) {
                e.Cancel = true;
            }
        }

        private void tab2_search_per_page_obj_ValueChanged(object sender, EventArgs e) {
            search_perpage = (int) tab2_search_per_page_obj.Value;
        }

        private void tab2_search_fetch_page_obj_ValueChanged(object sender, EventArgs e) {
            search_fetchpage = (int)tab2_search_fetch_page_obj.Value;
        }

        private void tab2_search_start_page_obj_ValueChanged(object sender, EventArgs e) {
            search_startpage = (int)tab2_search_start_page_obj.Value;
        }

        private void tab2_search_textbox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.A) {
                if (sender != null) {
                    ((CustomTextBox)sender).SelectAll();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void tab2_search_groupid_textbox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.A) {
                if (sender != null) {
                    ((CustomTextBox)sender).SelectAll();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void tab2_search_userid_textbox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.A) {
                if (sender != null) {
                    ((CustomTextBox)sender).SelectAll();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e) {
            options_7_time_format = toolStripTextBox1.Text;
        }

        private void video_download_setting_obj_CheckedChanged(object sender, EventArgs e) {
            if (!inited) {
                return;
            }
            if (video_download_setting_obj.Checked) {
                if (video_download_setting_obj.Tag.ToString().Equals("s") || !inited || MessageBox.Show(this, "下載影片功能目前僅限公開影片！Downloading video is only for public video!", "警告 Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)==DialogResult.OK) {
                    video_download_setting_obj.Tag = "";
                    options_8_video_download = video_download_setting_obj.Checked;
                    write_config();
                } else {
                    video_download_setting_obj.Checked = false;
                    options_8_video_download = false;
                    write_config();
                }
            } else {
                options_8_video_download = video_download_setting_obj.Checked;
                write_config();
            }
        }

        private void 重新設定APIKeyToolStripMenuItem_Click(object sender, EventArgs e) {
            if (download_started) {
                msgbox_nonBlock("請先停止下載！\r\nPlease stop download first!", "授權Auth");
                return;
            }
            if (signed_auth_bool) {
                if (MessageBox.Show(this, "確定登出並且切換Key？Are you sure to log out and change Key?", "更改Key", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) {
                    return;
                }
                try {
                    System.IO.File.Delete(Application.StartupPath + "\\userinfo.log");
                    signed_auth_bool = false;
                    toolStripDropDownButton2.Text = "授權此應用(Auth)";
                    toolStripDropDownButton3.Visible = false;
                    if (search_userid_type == 1) {
                        search_userid_type = 0;
                        tab2_userid_selectbox_type.Tag = 1;
                        tab2_userid_selectbox_type.SelectedIndex = 0;
                        tab2_search_userid_textbox.Enabled = true;
                        tab2_search_userid_textbox.Text = "";
                        search_userid_privacy = 0;
                        tab2_userid_selectbox_privacyfilter.Tag = 1;
                        tab2_userid_selectbox_privacyfilter.SelectedIndex = 0;
                        tab2_userid_selectbox_privacyfilter.Visible = false;
                    }
                    if (search_safe_search != 0) {
                        search_safe_search = 0;
                        tab2_safesearch_selectbox.Tag = 1;
                        tab2_safesearch_selectbox.SelectedIndex = 0;
                    }
                    append_msg_THREAD("已刪除並登出！Successfully Log out and Delete userinfo!", color_f_g, bg_color_exe);
                } catch {
                    append_msg_THREAD("無法刪除紀錄檔！Delete userinfo.log FAIL.", Color.Red, Color.Yellow);
                }
            }

            Update_keybox update_Keybox = new Update_keybox();
            update_Keybox.ShowDialog();

            if (update_Keybox.cancel_changing) {
                append_msg_THREAD("已取消！Canceled!", color_f_m, bg_color_exe);
                return;
            } else {
                try {
                    System.IO.File.Delete(Application.StartupPath + "\\key.log");
                } catch (Exception) {

                }
                if (update_Keybox.reset) {
                    api_key = default_api_key;
                    api_secret = default_api_secret;
                    append_msg_THREAD("已重設預設Key！Reset Key to default value!", color_f_g, bg_color_exe);
                } else {
                    api_key = update_Keybox.new_key;
                    api_secret = update_Keybox.new_secret;
                    try {
                        System.IO.File.WriteAllBytes(Application.StartupPath + "\\key.log", System.Text.UTF8Encoding.UTF8.GetBytes(api_key + CrLf + api_secret));
                        append_msg_THREAD("變更成功！Changed Key successfully!", color_f_g, bg_color_exe);
                    } catch (Exception) {
                        append_msg_THREAD("無法建立Key，變更Key失敗！Cannot create Key File. FAIL", Color.Red, Color.Yellow);
                    }
                }
            }
        }

        private void statusStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        /*protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }*/
    }
    
}