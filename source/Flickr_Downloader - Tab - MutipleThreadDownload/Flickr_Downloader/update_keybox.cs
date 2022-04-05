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
    public partial class Update_keybox : Form {

        public bool lock_form = true;
        public bool cancel_changing = true;
        public string new_key = "";
        public string new_secret = "";
        public bool reset = false;

        public Update_keybox() {
            InitializeComponent();
        }

        private void Verify_waiting_FormClosing(object sender, FormClosingEventArgs e) {
            if (lock_form) {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            new_key = textBox1.Text;
            new_secret = textBox2.Text;
            cancel_changing = false;
            lock_form = false;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            cancel_changing = true;
            lock_form = false;
            this.Close();
        }

        private void Verify_waiting_Load(object sender, EventArgs e) {

        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://www.flickr.com/services/apps/create/apply/");
        }

        private void button3_Click(object sender, EventArgs e) {
            reset = true;
            cancel_changing = false;
            lock_form = false;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            textBox1.Text = textBox1.Text.Trim();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
            textBox2.Text = textBox2.Text.Trim();
        }
    }
}
