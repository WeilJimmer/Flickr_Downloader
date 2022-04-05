using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flickr_Downloader {
    public partial class msgbox_form : Form {

        public string text_ = "";

        public msgbox_form() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void msgbox_form_Load(object sender, EventArgs e) {
            label1.Text = text_;
            label1.Select(0, 0);
        }
    }
}
