using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Win32;
namespace SharpKeys
{
	public partial class Dialog_Main : System.Windows.Forms.Form
	{
    
    private System.Windows.Forms.ListView lvKeys;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.ColumnHeader lvcFrom;
    private System.Windows.Forms.ColumnHeader lvcTo;
    private System.Windows.Forms.Button btnDeleteAll;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.LinkLabel urlMain;
    private System.Windows.Forms.MenuItem menuItem5;
    private System.Windows.Forms.MenuItem mniAdd;
    private System.Windows.Forms.MenuItem mniEdit;
    private System.Windows.Forms.MenuItem mniDelete;
    private System.Windows.Forms.MenuItem mniDeleteAll;
    private System.Windows.Forms.ContextMenu mnuPop;
    private Panel mainPanel;
    private Panel headerPanel;
    private Label displayProduct;
    private LinkLabel urlCode;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dialog_Main));
      this.lvKeys = new System.Windows.Forms.ListView();
      this.lvcFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lvcTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.mnuPop = new System.Windows.Forms.ContextMenu();
      this.mniAdd = new System.Windows.Forms.MenuItem();
      this.mniEdit = new System.Windows.Forms.MenuItem();
      this.mniDelete = new System.Windows.Forms.MenuItem();
      this.menuItem5 = new System.Windows.Forms.MenuItem();
      this.mniDeleteAll = new System.Windows.Forms.MenuItem();
      this.btnSave = new System.Windows.Forms.Button();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnEdit = new System.Windows.Forms.Button();
      this.btnDeleteAll = new System.Windows.Forms.Button();
      this.label11 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.urlMain = new System.Windows.Forms.LinkLabel();
      this.mainPanel = new System.Windows.Forms.Panel();
      this.headerPanel = new System.Windows.Forms.Panel();
      this.displayProduct = new System.Windows.Forms.Label();
      this.urlCode = new System.Windows.Forms.LinkLabel();
      this.mainPanel.SuspendLayout();
      this.headerPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // lvKeys
      // 
      this.lvKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lvKeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcFrom,
            this.lvcTo});
      this.lvKeys.ContextMenu = this.mnuPop;
      this.lvKeys.ForeColor = System.Drawing.SystemColors.WindowText;
      this.lvKeys.FullRowSelect = true;
      this.lvKeys.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.lvKeys.HideSelection = false;
      this.lvKeys.Location = new System.Drawing.Point(14, 45);
      this.lvKeys.MultiSelect = false;
      this.lvKeys.Name = "lvKeys";
      this.lvKeys.Size = new System.Drawing.Size(579, 282);
      this.lvKeys.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.lvKeys.TabIndex = 0;
      this.lvKeys.UseCompatibleStateImageBehavior = false;
      this.lvKeys.View = System.Windows.Forms.View.Details;
      this.lvKeys.SelectedIndexChanged += new System.EventHandler(this.lvKeys_SelectedIndexChanged);
      this.lvKeys.DoubleClick += new System.EventHandler(this.lvKeys_DoubleClick);
      // 
      // lvcFrom
      // 
      this.lvcFrom.Text = "From:";
      // 
      // lvcTo
      // 
      this.lvcTo.Text = "To:";
      // 
      // mnuPop
      // 
      this.mnuPop.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniAdd,
            this.mniEdit,
            this.mniDelete,
            this.menuItem5,
            this.mniDeleteAll});
      this.mnuPop.Popup += new System.EventHandler(this.mnuPop_Popup);
      // 
      // mniAdd
      // 
      this.mniAdd.Index = 0;
      this.mniAdd.Text = "Add";
      this.mniAdd.Click += new System.EventHandler(this.mniAdd_Click);
      // 
      // mniEdit
      // 
      this.mniEdit.Index = 1;
      this.mniEdit.Text = "Edit";
      this.mniEdit.Click += new System.EventHandler(this.mniEdit_Click);
      // 
      // mniDelete
      // 
      this.mniDelete.Index = 2;
      this.mniDelete.Text = "Delete";
      this.mniDelete.Click += new System.EventHandler(this.mniDelete_Click);
      // 
      // menuItem5
      // 
      this.menuItem5.Index = 3;
      this.menuItem5.Text = "-";
      // 
      // mniDeleteAll
      // 
      this.mniDeleteAll.Index = 4;
      this.mniDeleteAll.Text = "Delete All";
      this.mniDeleteAll.Click += new System.EventHandler(this.mniDeleteAll_Click);
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSave.Location = new System.Drawing.Point(409, 339);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(106, 23);
      this.btnSave.TabIndex = 5;
      this.btnSave.Text = "&Write to Registry";
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.Location = new System.Drawing.Point(521, 339);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(72, 23);
      this.btnClose.TabIndex = 6;
      this.btnClose.Text = "&Close";
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnAdd
      // 
      this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnAdd.Location = new System.Drawing.Point(14, 339);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(72, 23);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "&Add";
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // btnDelete
      // 
      this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnDelete.Location = new System.Drawing.Point(170, 339);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(72, 23);
      this.btnDelete.TabIndex = 3;
      this.btnDelete.Text = "&Delete";
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // btnEdit
      // 
      this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnEdit.Location = new System.Drawing.Point(92, 339);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new System.Drawing.Size(72, 23);
      this.btnEdit.TabIndex = 2;
      this.btnEdit.Text = "&Edit";
      this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
      // 
      // btnDeleteAll
      // 
      this.btnDeleteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnDeleteAll.Location = new System.Drawing.Point(248, 339);
      this.btnDeleteAll.Name = "btnDeleteAll";
      this.btnDeleteAll.Size = new System.Drawing.Size(72, 23);
      this.btnDeleteAll.TabIndex = 4;
      this.btnDeleteAll.Text = "De&lete All";
      this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
      // 
      // label11
      // 
      this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.label11.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.label11.Location = new System.Drawing.Point(9, 373);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(588, 3);
      this.label11.TabIndex = 7;
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label1.AutoSize = true;
      this.label1.Enabled = false;
      this.label1.Location = new System.Drawing.Point(15, 385);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(274, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "SharpKeys 3.5 - Copyright 2004 - 2012 RandyRants.com";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label2.AutoSize = true;
      this.label2.Enabled = false;
      this.label2.Location = new System.Drawing.Point(15, 403);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(226, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "Registry hack for remapping keys for Windows";
      // 
      // urlMain
      // 
      this.urlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.urlMain.AutoSize = true;
      this.urlMain.Location = new System.Drawing.Point(451, 403);
      this.urlMain.Name = "urlMain";
      this.urlMain.Size = new System.Drawing.Size(142, 13);
      this.urlMain.TabIndex = 11;
      this.urlMain.TabStop = true;
      this.urlMain.Text = "http://www.randyrants.com/";
      this.urlMain.TextAlign = System.Drawing.ContentAlignment.TopRight;
      this.urlMain.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.urlMain_LinkClicked);
      // 
      // mainPanel
      // 
      this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.mainPanel.BackColor = System.Drawing.Color.Transparent;
      this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.mainPanel.Controls.Add(this.headerPanel);
      this.mainPanel.Controls.Add(this.urlCode);
      this.mainPanel.Controls.Add(this.urlMain);
      this.mainPanel.Controls.Add(this.label2);
      this.mainPanel.Controls.Add(this.lvKeys);
      this.mainPanel.Controls.Add(this.btnAdd);
      this.mainPanel.Controls.Add(this.label1);
      this.mainPanel.Controls.Add(this.btnEdit);
      this.mainPanel.Controls.Add(this.btnDelete);
      this.mainPanel.Controls.Add(this.label11);
      this.mainPanel.Controls.Add(this.btnDeleteAll);
      this.mainPanel.Controls.Add(this.btnSave);
      this.mainPanel.Controls.Add(this.btnClose);
      this.mainPanel.Location = new System.Drawing.Point(12, 12);
      this.mainPanel.Name = "mainPanel";
      this.mainPanel.Size = new System.Drawing.Size(608, 430);
      this.mainPanel.TabIndex = 12;
      this.mainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPanel_Paint);
      // 
      // headerPanel
      // 
      this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.headerPanel.BackColor = System.Drawing.Color.Transparent;
      this.headerPanel.Controls.Add(this.displayProduct);
      this.headerPanel.Location = new System.Drawing.Point(0, 0);
      this.headerPanel.Name = "headerPanel";
      this.headerPanel.Size = new System.Drawing.Size(606, 29);
      this.headerPanel.TabIndex = 7;
      this.headerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.headerPanel_Paint);
      // 
      // displayProduct
      // 
      this.displayProduct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.displayProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.displayProduct.ForeColor = System.Drawing.Color.White;
      this.displayProduct.Location = new System.Drawing.Point(10, 2);
      this.displayProduct.Name = "displayProduct";
      this.displayProduct.Size = new System.Drawing.Size(586, 23);
      this.displayProduct.TabIndex = 1;
      this.displayProduct.Text = "SharpKeys";
      this.displayProduct.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      // 
      // urlCode
      // 
      this.urlCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.urlCode.AutoSize = true;
      this.urlCode.Location = new System.Drawing.Point(409, 385);
      this.urlCode.Name = "urlCode";
      this.urlCode.Size = new System.Drawing.Size(184, 13);
      this.urlCode.TabIndex = 11;
      this.urlCode.TabStop = true;
      this.urlCode.Text = "http://www.codeplex.com/sharpkeys";
      this.urlCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
      this.urlCode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.urlMain_LinkClicked);
      // 
      // Dialog_Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(632, 454);
      this.Controls.Add(this.mainPanel);
      this.DoubleBuffered = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(640, 480);
      this.Name = "Dialog_Main";
      this.Text = "SharpKeys";
      this.Closing += new System.ComponentModel.CancelEventHandler(this.Dialog_Main_Closing);
      this.Load += new System.EventHandler(this.Dialog_Main_Load);
      this.Paint += new System.Windows.Forms.PaintEventHandler(this.Dialog_Main_Paint);
      this.Resize += new System.EventHandler(this.Dialog_Main_Resize);
      this.mainPanel.ResumeLayout(false);
      this.mainPanel.PerformLayout();
      this.headerPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		#endregion
	}
}

