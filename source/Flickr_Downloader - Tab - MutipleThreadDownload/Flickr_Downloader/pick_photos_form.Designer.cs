namespace Flickr_Downloader {
    partial class pick_photos_form {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(pick_photos_form));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.反選ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全不選ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全選ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打勾選擇toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.重新整理toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listView1 = new Flickr_Downloader.ListViewNF();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList_large = new System.Windows.Forms.ImageList(this.components);
            this.imageList_small = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.prev_page_button = new System.Windows.Forms.Button();
            this.first_page_button = new System.Windows.Forms.Button();
            this.final_page_button = new System.Windows.Forms.Button();
            this.next_page_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("新細明體", 15F);
            this.button2.Location = new System.Drawing.Point(596, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(588, 45);
            this.button2.TabIndex = 1;
            this.button2.Text = "取消 (Cancel)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 577);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1187, 51);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("新細明體", 15F);
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(587, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "確定 (OK)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabel1.Text = "                        ";
            // 
            // statusStrip
            // 
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(52, 17);
            this.statusStrip.Text = "               ";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Black;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.statusStrip,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 642);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1193, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // 反選ToolStripMenuItem
            // 
            this.反選ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.反選ToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.反選ToolStripMenuItem.Name = "反選ToolStripMenuItem";
            this.反選ToolStripMenuItem.Size = new System.Drawing.Size(328, 22);
            this.反選ToolStripMenuItem.Text = "反選 (Reverse Select All)";
            this.反選ToolStripMenuItem.Click += new System.EventHandler(this.反選ToolStripMenuItem_Click);
            // 
            // 全不選ToolStripMenuItem
            // 
            this.全不選ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.全不選ToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.全不選ToolStripMenuItem.Name = "全不選ToolStripMenuItem";
            this.全不選ToolStripMenuItem.Size = new System.Drawing.Size(328, 22);
            this.全不選ToolStripMenuItem.Text = "全不選  (UnSelect All)";
            this.全不選ToolStripMenuItem.Click += new System.EventHandler(this.全不選ToolStripMenuItem_Click);
            // 
            // 全選ToolStripMenuItem
            // 
            this.全選ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.全選ToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.全選ToolStripMenuItem.Name = "全選ToolStripMenuItem";
            this.全選ToolStripMenuItem.Size = new System.Drawing.Size(328, 22);
            this.全選ToolStripMenuItem.Text = "全選 (Select All)";
            this.全選ToolStripMenuItem.Click += new System.EventHandler(this.全選ToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackgroundImage = global::Flickr_Downloader.Properties.Resources.Form8_ContextMenuStrip1_BACK;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打勾選擇toolStripMenuItem5,
            this.全選ToolStripMenuItem,
            this.全不選ToolStripMenuItem,
            this.反選ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.重新整理toolStripMenuItem5});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(329, 202);
            // 
            // 打勾選擇toolStripMenuItem5
            // 
            this.打勾選擇toolStripMenuItem5.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.打勾選擇toolStripMenuItem5.ForeColor = System.Drawing.Color.Red;
            this.打勾選擇toolStripMenuItem5.Name = "打勾選擇toolStripMenuItem5";
            this.打勾選擇toolStripMenuItem5.Size = new System.Drawing.Size(328, 22);
            this.打勾選擇toolStripMenuItem5.Text = "打勾/不勾選擇的 (Check/UnCheck Selectionl)";
            this.打勾選擇toolStripMenuItem5.Click += new System.EventHandler(this.打勾選擇toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.Red;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(328, 22);
            this.toolStripMenuItem1.Text = "切換大/小圖示 (Toggle Large/Small icon)";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.toolStripMenuItem2.ForeColor = System.Drawing.Color.Red;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(328, 22);
            this.toolStripMenuItem2.Text = "切換圖示/詳細資料 (Toggle Icon/Detail)";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.toolStripMenuItem3.ForeColor = System.Drawing.Color.Red;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(328, 22);
            this.toolStripMenuItem3.Text = "複製選中相簿 ID (Copy Checked Albums ID)";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.toolStripMenuItem4.ForeColor = System.Drawing.Color.Red;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(328, 22);
            this.toolStripMenuItem4.Text = "複製選中用戶 ID (Copy Checked User ID)";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // 重新整理toolStripMenuItem5
            // 
            this.重新整理toolStripMenuItem5.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.重新整理toolStripMenuItem5.ForeColor = System.Drawing.Color.Red;
            this.重新整理toolStripMenuItem5.Name = "重新整理toolStripMenuItem5";
            this.重新整理toolStripMenuItem5.Size = new System.Drawing.Size(328, 22);
            this.重新整理toolStripMenuItem5.Text = "刷新 (Refresh Page)";
            this.重新整理toolStripMenuItem5.Click += new System.EventHandler(this.重新整理toolStripMenuItem5_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.72987F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.135063F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.135063F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1193, 664);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.listView1.AllowColumnReorder = true;
            this.listView1.BackColor = System.Drawing.Color.White;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader5,
            this.columnHeader8});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.ForeColor = System.Drawing.Color.Lime;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.LabelWrap = false;
            this.listView1.LargeImageList = this.imageList_large;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(1187, 511);
            this.listView1.SmallImageList = this.imageList_small;
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.VirtualListSize = 1;
            this.listView1.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView1_DrawColumnHeader);
            this.listView1.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listView1_DrawItem);
            this.listView1.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView1_DrawSubItem);
            this.listView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Title";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ID";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "AlbumTitle";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "AlbumID";
            this.columnHeader4.Width = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "OwnerName";
            this.columnHeader6.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "OwnerID";
            this.columnHeader7.Width = 120;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "photo URL";
            this.columnHeader5.Width = 400;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "DateTaken";
            this.columnHeader8.Width = 120;
            // 
            // imageList_large
            // 
            this.imageList_large.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList_large.ImageSize = new System.Drawing.Size(240, 240);
            this.imageList_large.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList_small
            // 
            this.imageList_small.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList_small.ImageSize = new System.Drawing.Size(120, 120);
            this.imageList_small.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.prev_page_button, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.first_page_button, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.final_page_button, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.next_page_button, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 520);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1187, 51);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // prev_page_button
            // 
            this.prev_page_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prev_page_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prev_page_button.Font = new System.Drawing.Font("新細明體", 15F);
            this.prev_page_button.ForeColor = System.Drawing.Color.Yellow;
            this.prev_page_button.Location = new System.Drawing.Point(240, 3);
            this.prev_page_button.Name = "prev_page_button";
            this.prev_page_button.Size = new System.Drawing.Size(231, 45);
            this.prev_page_button.TabIndex = 2;
            this.prev_page_button.Text = "上一頁 (Prev Page)";
            this.prev_page_button.UseVisualStyleBackColor = true;
            this.prev_page_button.Click += new System.EventHandler(this.prev_page_button_Click);
            // 
            // first_page_button
            // 
            this.first_page_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.first_page_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.first_page_button.Font = new System.Drawing.Font("新細明體", 15F);
            this.first_page_button.ForeColor = System.Drawing.Color.Yellow;
            this.first_page_button.Location = new System.Drawing.Point(3, 3);
            this.first_page_button.Name = "first_page_button";
            this.first_page_button.Size = new System.Drawing.Size(231, 45);
            this.first_page_button.TabIndex = 1;
            this.first_page_button.Text = "第一頁 (First Page)";
            this.first_page_button.UseVisualStyleBackColor = true;
            this.first_page_button.Click += new System.EventHandler(this.first_page_button_Click);
            // 
            // final_page_button
            // 
            this.final_page_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.final_page_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.final_page_button.Font = new System.Drawing.Font("新細明體", 15F);
            this.final_page_button.ForeColor = System.Drawing.Color.Yellow;
            this.final_page_button.Location = new System.Drawing.Point(951, 3);
            this.final_page_button.Name = "final_page_button";
            this.final_page_button.Size = new System.Drawing.Size(233, 45);
            this.final_page_button.TabIndex = 5;
            this.final_page_button.Text = "最終頁 (Final Page)";
            this.final_page_button.UseVisualStyleBackColor = true;
            this.final_page_button.Click += new System.EventHandler(this.final_page_button_Click);
            // 
            // next_page_button
            // 
            this.next_page_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.next_page_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.next_page_button.Font = new System.Drawing.Font("新細明體", 15F);
            this.next_page_button.ForeColor = System.Drawing.Color.Yellow;
            this.next_page_button.Location = new System.Drawing.Point(714, 3);
            this.next_page_button.Name = "next_page_button";
            this.next_page_button.Size = new System.Drawing.Size(231, 45);
            this.next_page_button.TabIndex = 4;
            this.next_page_button.Text = "下一頁 (Next Page)";
            this.next_page_button.UseVisualStyleBackColor = true;
            this.next_page_button.Click += new System.EventHandler(this.next_page_button_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.numericUpDown1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(477, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(231, 45);
            this.tableLayoutPanel4.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("細明體", 15F);
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 45);
            this.label2.TabIndex = 2;
            this.label2.Text = "(";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.AutoSize = true;
            this.numericUpDown1.BackColor = System.Drawing.Color.Black;
            this.numericUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDown1.Font = new System.Drawing.Font("細明體", 15F);
            this.numericUpDown1.ForeColor = System.Drawing.Color.Yellow;
            this.numericUpDown1.Location = new System.Drawing.Point(72, 3);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(86, 31);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("細明體", 15F);
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(164, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "/1)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pick_photos_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1193, 664);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Lime;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "pick_photos_form";
            this.Text = "[LOADING] Select photos you want.";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.pick_photos_form_ResizeEnd);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        public System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem 反選ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全不選ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全選ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public Flickr_Downloader.ListViewNF listView1;
        private System.Windows.Forms.ImageList imageList_large;
        private System.Windows.Forms.ImageList imageList_small;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ToolStripMenuItem 打勾選擇toolStripMenuItem5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public System.Windows.Forms.Button prev_page_button;
        public System.Windows.Forms.Button first_page_button;
        public System.Windows.Forms.Button final_page_button;
        public System.Windows.Forms.Button next_page_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem 重新整理toolStripMenuItem5;
    }
}