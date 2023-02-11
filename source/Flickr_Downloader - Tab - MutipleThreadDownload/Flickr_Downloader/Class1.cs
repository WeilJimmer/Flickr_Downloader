using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net;
using System.ComponentModel;

namespace Flickr_Downloader {
    public class OpaqueRichTextBox : RichTextBox {
        /*
        private static readonly Color DefaultBackground = Color.Transparent;

        private Image TransparentImage;
        public OpaqueRichTextBox () {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = DefaultBackground;
        }

        public override System.Drawing.Color BackColor {
            get {
                return base.BackColor;
            }
            set {
                base.BackColor = value;
            }
        }

        public Image Image {
            get {
                return TransparentImage;
            }
            set {
                TransparentImage = value;
                Invalidate();
            }
        }

        // Infrastructure to cause the default background to be transparent
        public bool ShouldSerializeBackColor () {
            return BackColor == DefaultBackground;
        }

        // Infrastructure to cause the default background to be transparent
        public void ResetBackground () {
            BackColor = DefaultBackground;
        }

        protected override System.Windows.Forms.CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20;
                return cp;
            }
        }*/
    }

    public class ListViewNF : System.Windows.Forms.ListView {
        /*
        public ListViewNF() {
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m) {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14) {
                base.OnNotifyMessage(m);
            }
        }*/

    }

    public partial class CustomTextBox : TextBox {

        public CustomTextBox() {
            /*SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);*/
            BackColor = Color.Black;
        }

    }

    public static class ControlExtensions {
        public static void DoubleBuffered(this Control control, bool enable) {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo.SetValue(control, enable, null);
        }
    }

    public static class ControlHelper {
        #region Redraw Suspend/Resume
        [DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const int WM_SETREDRAW = 0xB;

        public static void SuspendDrawing(this Control target) {
            SendMessage(target.Handle, WM_SETREDRAW, 0, 0);
        }

        public static void ResumeDrawing(this Control target) { ResumeDrawing(target, true); }
        public static void ResumeDrawing(this Control target, bool redraw) {
            SendMessage(target.Handle, WM_SETREDRAW, 1, 0);

            if (redraw) {
                target.Refresh();
            }
        }
        #endregion
    }

    public class WebClient_auto : WebClient {
        private WebRequest _Request = null;

        protected override WebRequest GetWebRequest(Uri address) {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            this._Request = request;
            return request;
        }

        public HttpStatusCode StatusCode() {
            HttpStatusCode result;

            if (this._Request == null) {
                throw (new InvalidOperationException("Unable to retrieve the status code, maybe you haven't made a request yet."));
            }
            try {
                try {
                    HttpWebResponse response = base.GetWebResponse(this._Request) as HttpWebResponse;
                    if (response != null) {
                        result = response.StatusCode;
                    } else {
                        throw (new InvalidOperationException("Unable to retrieve the status code, maybe you haven't made a request yet."));
                    }
                } catch (WebException we) {
                    try {
                        HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;
                        result = response.StatusCode;
                    } catch {
                        result = HttpStatusCode.ServiceUnavailable;
                    }
                }
            } catch {
                result = HttpStatusCode.ServiceUnavailable;
            }

            return result;

        }

    }

    public class CustomLabel : Label {
        public CustomLabel() {
            this.SetStyle(ControlStyles.UserPaint, true); //Call in constructor, Use UserPaint
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (Enabled) {
                //use normal realization
                base.OnPaint(e);
                return;
            }
            //custom drawing
            using (Brush aBrush = new SolidBrush(Parent.ForeColor)) {
                e.Graphics.DrawString(Text, Font, aBrush, ClientRectangle);
            }
        }
    }

    public class TabControl : System.Windows.Forms.TabControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public TabControl() {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region Interop

        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR {
            public IntPtr HWND;
            public uint idFrom;
            public int code;
            public override String ToString() {
                return String.Format("Hwnd: {0}, ControlID: {1}, Code: {2}", HWND, idFrom, code);
            }
        }

        private const int TCN_FIRST = 0 - 550;
        private const int TCN_SELCHANGING = (TCN_FIRST - 2);

        private const int WM_USER = 0x400;
        private const int WM_NOTIFY = 0x4E;
        private const int WM_REFLECT = WM_USER + 0x1C00;

        #endregion

        #region BackColor Manipulation

        //As well as exposing the property to the Designer we want it to behave just like any other 
        //controls BackColor property so we need some clever manipulation.

        private Color m_Backcolor = Color.Empty;
        [Browsable(true), Description("The background color used to display text and graphics in a control.")]
        public override Color BackColor {
            get {
                if (m_Backcolor.Equals(Color.Empty)) {
                    if (Parent == null)
                        return Control.DefaultBackColor;
                    else
                        return Parent.BackColor;
                }
                return m_Backcolor;
            }
            set {
                if (m_Backcolor.Equals(value)) return;
                m_Backcolor = value;
                Invalidate();
                //Let the Tabpages know that the backcolor has changed.
                base.OnBackColorChanged(EventArgs.Empty);
            }
        }
        public bool ShouldSerializeBackColor() {
            return !m_Backcolor.Equals(Color.Empty);
        }
        public override void ResetBackColor() {
            m_Backcolor = Color.Empty;
            Invalidate();
        }

        #endregion

        protected override void OnParentBackColorChanged(EventArgs e) {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }


        protected override void OnSelectedIndexChanged(EventArgs e) {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            e.Graphics.Clear(BackColor);
            Rectangle r = ClientRectangle;
            if (TabCount <= 0) return;
            //Draw a custom background for Transparent TabPages
            r = SelectedTab.Bounds;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Font DrawFont = new Font(Font.FontFamily, 24, FontStyle.Regular, GraphicsUnit.Pixel);
            ControlPaint.DrawStringDisabled(e.Graphics, "Micks Ownerdraw TabControl", DrawFont, BackColor, (RectangleF)r, sf);
            DrawFont.Dispose();
            //Draw a border around TabPage
            r.Inflate(3, 3);
            TabPage tp = TabPages[SelectedIndex];
            SolidBrush PaintBrush = new SolidBrush(tp.BackColor);
            e.Graphics.FillRectangle(PaintBrush, r);
            ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, ButtonBorderStyle.Outset);
            //Draw the Tabs
            for (int index = 0; index <= TabCount - 1; index++) {
                tp = TabPages[index];
                r = GetTabRect(index);
                ButtonBorderStyle bs = ButtonBorderStyle.Outset;
                if (index == SelectedIndex) bs = ButtonBorderStyle.Inset;
                PaintBrush.Color = tp.BackColor;
                e.Graphics.FillRectangle(PaintBrush, r);
                ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, bs);
                PaintBrush.Color = tp.ForeColor;

                //Set up rotation for left and right aligned tabs
                if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right) {
                    float RotateAngle = 90;
                    if (Alignment == TabAlignment.Left) RotateAngle = 270;
                    PointF cp = new PointF(r.Left + (r.Width >> 1), r.Top + (r.Height >> 1));
                    e.Graphics.TranslateTransform(cp.X, cp.Y);
                    e.Graphics.RotateTransform(RotateAngle);
                    r = new Rectangle(-(r.Height >> 1), -(r.Width >> 1), r.Height, r.Width);
                }
                //Draw the Tab Text
                if (tp.Enabled)
                    e.Graphics.DrawString(tp.Text, Font, PaintBrush, (RectangleF)r, sf);
                else
                    ControlPaint.DrawStringDisabled(e.Graphics, tp.Text, Font, tp.BackColor, (RectangleF)r, sf);

                e.Graphics.ResetTransform();
            }

            PaintBrush.Dispose();

        }


        [Description("Occurs as a tab is being changed.")]
        public event SelectedTabPageChangeEventHandler SelectedIndexChanging;

        protected override void WndProc(ref Message m) {
            if (m.Msg == (WM_REFLECT + WM_NOTIFY)) {
                NMHDR hdr = (NMHDR)(Marshal.PtrToStructure(m.LParam, typeof(NMHDR)));
                if (hdr.code == TCN_SELCHANGING) {
                    TabPage tp = TestTab(PointToClient(Cursor.Position));
                    if (tp != null) {
                        TabPageChangeEventArgs e = new TabPageChangeEventArgs(SelectedTab, tp);
                        if (SelectedIndexChanging != null)
                            SelectedIndexChanging(this, e);
                        if (e.Cancel || tp.Enabled == false) {
                            m.Result = new IntPtr(1);
                            return;
                        }
                    }
                }
            }
            if ((m.Msg == TCM_ADJUSTRECT)) {
                RECT rc = (RECT)m.GetLParam(typeof(RECT));
                //Adjust these values to suit, dependant upon Appearance
                rc.Left -= 3;
                rc.Right += 3;
                rc.Top -= 8;
                rc.Bottom += 3;
                Marshal.StructureToPtr(rc, m.LParam, true);
            }
            base.WndProc(ref m);
        }

        private const Int32 TCM_FIRST = 0x1300;
        private const Int32 TCM_ADJUSTRECT = (TCM_FIRST + 40);
        private struct RECT {
            public Int32 Left;
            public Int32 Top;
            public Int32 Right;
            public Int32 Bottom;
        }

        private TabPage TestTab(Point pt) {
            for (int index = 0; index <= TabCount - 1; index++) {
                if (GetTabRect(index).Contains(pt.X, pt.Y))
                    return TabPages[index];
            }
            return null;
        }

    }


    public class TabPageChangeEventArgs : EventArgs {
        private TabPage _Selected = null;
        private TabPage _PreSelected = null;
        public bool Cancel = false;

        public TabPage CurrentTab {
            get {
                return _Selected;
            }
        }


        public TabPage NextTab {
            get {
                return _PreSelected;
            }
        }


        public TabPageChangeEventArgs(TabPage CurrentTab, TabPage NextTab) {
            _Selected = CurrentTab;
            _PreSelected = NextTab;
        }


    }


    public delegate void SelectedTabPageChangeEventHandler(Object sender, TabPageChangeEventArgs e);

    public class ToolStripSpringTextBox : ToolStripTextBox {
        public override Size GetPreferredSize(Size constrainingSize) {
            // Use the default size if the text box is on the overflow menu
            // or is on a vertical ToolStrip.
            if (IsOnOverflow || Owner.Orientation == Orientation.Vertical) {
                return DefaultSize;
            }

            // Declare a variable to store the total available width as 
            // it is calculated, starting with the display width of the 
            // owning ToolStrip.
            Int32 width = Owner.DisplayRectangle.Width;

            // Subtract the width of the overflow button if it is displayed. 
            if (Owner.OverflowButton.Visible) {
                width = width - Owner.OverflowButton.Width -
                    Owner.OverflowButton.Margin.Horizontal;
            }

            // Declare a variable to maintain a count of ToolStripSpringTextBox 
            // items currently displayed in the owning ToolStrip. 
            Int32 springBoxCount = 0;

            foreach (ToolStripItem item in Owner.Items) {
                // Ignore items on the overflow menu.
                if (item.IsOnOverflow) continue;

                if (item is ToolStripSpringTextBox) {
                    // For ToolStripSpringTextBox items, increment the count and 
                    // subtract the margin width from the total available width.
                    springBoxCount++;
                    width -= item.Margin.Horizontal;
                } else {
                    // For all other items, subtract the full width from the total
                    // available width.
                    width = width - item.Width - item.Margin.Horizontal;
                }
            }

            // If there are multiple ToolStripSpringTextBox items in the owning
            // ToolStrip, divide the total available width between them. 
            if (springBoxCount > 1) width /= springBoxCount;

            // If the available width is less than the default width, use the
            // default width, forcing one or more items onto the overflow menu.
            if (width < DefaultSize.Width) width = DefaultSize.Width;

            // Retrieve the preferred size from the base class, but change the
            // width to the calculated width. 
            Size size = base.GetPreferredSize(constrainingSize);
            size.Width = 200;
            return size;
        }
    }

}
