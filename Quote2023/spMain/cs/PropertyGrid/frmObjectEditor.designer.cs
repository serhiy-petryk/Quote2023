namespace spMain.cs {
    public partial class PGfrmObjectEditor {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PGfrmObjectEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listItems = new System.Windows.Forms.ListBox();
            this.tsTreeView = new System.Windows.Forms.ToolStrip();
            this.btnList_Add = new System.Windows.Forms.ToolStripButton();
            this.btnList_Edit = new System.Windows.Forms.ToolStripButton();
            this.btnList_Delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnList_Save = new System.Windows.Forms.ToolStripButton();
            this.pg = new System.Windows.Forms.PropertyGrid();
            this.tsPropertyGrid = new System.Windows.Forms.ToolStrip();
            this.btnOK = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnByCategory = new System.Windows.Forms.ToolStripButton();
            this.btnByAlpha = new System.Windows.Forms.ToolStripButton();
            this.separEdit = new System.Windows.Forms.ToolStripSeparator();
            this.btnPG_ShowStructure = new System.Windows.Forms.ToolStripButton();
            this.btnPG_ExpandAll = new System.Windows.Forms.ToolStripButton();
            this.btnPG_CollapseAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPG_AddItem = new System.Windows.Forms.ToolStripButton();
            this.btnPG_RemoveItem = new System.Windows.Forms.ToolStripButton();
            this.btnPG_ItemUp = new System.Windows.Forms.ToolStripButton();
            this.btnPG_ItemDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPG_Save = new System.Windows.Forms.ToolStripButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tsTreeView.SuspendLayout();
            this.tsPropertyGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listItems);
            this.splitContainer1.Panel1.Controls.Add(this.tsTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pg);
            this.splitContainer1.Panel2.Controls.Add(this.tsPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(867, 612);
            this.splitContainer1.SplitterDistance = 341;
            this.splitContainer1.TabIndex = 4;
            // 
            // listItems
            // 
            this.listItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listItems.FormattingEnabled = true;
            this.listItems.ItemHeight = 15;
            this.listItems.Location = new System.Drawing.Point(0, 25);
            this.listItems.Name = "listItems";
            this.listItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listItems.Size = new System.Drawing.Size(341, 587);
            this.listItems.TabIndex = 4;
            this.listItems.SelectedIndexChanged += new System.EventHandler(this.listItems_SelectedIndexChanged);
            this.listItems.DoubleClick += new System.EventHandler(this.listItems_DoubleClick);
            // 
            // tsTreeView
            // 
            this.tsTreeView.GripMargin = new System.Windows.Forms.Padding(0);
            this.tsTreeView.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnList_Add,
            this.btnList_Edit,
            this.btnList_Delete,
            this.toolStripSeparator5,
            this.btnList_Save});
            this.tsTreeView.Location = new System.Drawing.Point(0, 0);
            this.tsTreeView.Name = "tsTreeView";
            this.tsTreeView.Padding = new System.Windows.Forms.Padding(0);
            this.tsTreeView.Size = new System.Drawing.Size(341, 25);
            this.tsTreeView.TabIndex = 3;
            this.tsTreeView.Text = "toolStrip1";
            // 
            // btnList_Add
            // 
            this.btnList_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnList_Add.Image = global::spMain.Properties.Resources.AddNew;
            this.btnList_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnList_Add.Name = "btnList_Add";
            this.btnList_Add.Size = new System.Drawing.Size(23, 22);
            this.btnList_Add.Text = "Add ";
            this.btnList_Add.ToolTipText = "Add current object to collection";
            // 
            // btnList_Edit
            // 
            this.btnList_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnList_Edit.Image = global::spMain.Properties.Resources.Edit;
            this.btnList_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnList_Edit.Name = "btnList_Edit";
            this.btnList_Edit.Size = new System.Drawing.Size(23, 22);
            this.btnList_Edit.Text = "toolStripButton1";
            this.btnList_Edit.ToolTipText = "Edit current item";
            this.btnList_Edit.Click += new System.EventHandler(this.btnList_Edit_Click);
            // 
            // btnList_Delete
            // 
            this.btnList_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnList_Delete.Image = global::spMain.Properties.Resources.Delete;
            this.btnList_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnList_Delete.Name = "btnList_Delete";
            this.btnList_Delete.Size = new System.Drawing.Size(23, 22);
            this.btnList_Delete.Text = "Delete";
            this.btnList_Delete.ToolTipText = "Delete object from collection";
            this.btnList_Delete.Click += new System.EventHandler(this.btnList_Delete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnList_Save
            // 
            this.btnList_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnList_Save.Image = global::spMain.Properties.Resources.Save;
            this.btnList_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnList_Save.Name = "btnList_Save";
            this.btnList_Save.Size = new System.Drawing.Size(23, 22);
            this.btnList_Save.Text = "Save collection";
            this.btnList_Save.ToolTipText = "Save collection";
            this.btnList_Save.Click += new System.EventHandler(this.btnList_Save_Click);
            // 
            // pg
            // 
            this.pg.CommandsVisibleIfAvailable = false;
            this.pg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pg.Location = new System.Drawing.Point(0, 25);
            this.pg.Name = "pg";
            this.pg.Size = new System.Drawing.Size(522, 587);
            this.pg.TabIndex = 3;
            this.pg.ToolbarVisible = false;
            this.pg.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.pg_SelectedGridItemChanged);
            this.pg.Paint += new System.Windows.Forms.PaintEventHandler(this.pg_Paint);
            // 
            // tsPropertyGrid
            // 
            this.tsPropertyGrid.GripMargin = new System.Windows.Forms.Padding(0);
            this.tsPropertyGrid.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsPropertyGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOK,
            this.btnCancel,
            this.toolStripSeparator1,
            this.btnByCategory,
            this.btnByAlpha,
            this.separEdit,
            this.btnPG_ShowStructure,
            this.btnPG_ExpandAll,
            this.btnPG_CollapseAll,
            this.toolStripSeparator2,
            this.btnPG_AddItem,
            this.btnPG_RemoveItem,
            this.btnPG_ItemUp,
            this.btnPG_ItemDown,
            this.toolStripSeparator3,
            this.btnPG_Save});
            this.tsPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.tsPropertyGrid.Name = "tsPropertyGrid";
            this.tsPropertyGrid.Padding = new System.Windows.Forms.Padding(0);
            this.tsPropertyGrid.Size = new System.Drawing.Size(522, 25);
            this.tsPropertyGrid.TabIndex = 2;
            this.tsPropertyGrid.Text = "toolStrip1";
            // 
            // btnOK
            // 
            this.btnOK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(28, 22);
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnPG_OK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(47, 22);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnPG_Cancel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnByCategory
            // 
            this.btnByCategory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnByCategory.Image = global::spMain.Properties.Resources.PBCatego;
            this.btnByCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnByCategory.Margin = new System.Windows.Forms.Padding(1, 1, 0, 2);
            this.btnByCategory.Name = "btnByCategory";
            this.btnByCategory.Size = new System.Drawing.Size(23, 22);
            this.btnByCategory.Text = "Categorized";
            this.btnByCategory.ToolTipText = "Categorized";
            this.btnByCategory.Click += new System.EventHandler(this.btnByCategory_Click);
            // 
            // btnByAlpha
            // 
            this.btnByAlpha.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnByAlpha.Image = global::spMain.Properties.Resources.PBAlpha;
            this.btnByAlpha.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnByAlpha.Margin = new System.Windows.Forms.Padding(1, 1, 0, 2);
            this.btnByAlpha.Name = "btnByAlpha";
            this.btnByAlpha.Size = new System.Drawing.Size(23, 22);
            this.btnByAlpha.Text = "Alphabetical";
            this.btnByAlpha.ToolTipText = "Alphabetical";
            this.btnByAlpha.Click += new System.EventHandler(this.btnByAlpha_Click);
            // 
            // separEdit
            // 
            this.separEdit.Name = "separEdit";
            this.separEdit.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPG_ShowStructure
            // 
            this.btnPG_ShowStructure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPG_ShowStructure.Image = ((System.Drawing.Image)(resources.GetObject("btnPG_ShowStructure.Image")));
            this.btnPG_ShowStructure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPG_ShowStructure.Name = "btnPG_ShowStructure";
            this.btnPG_ShowStructure.Size = new System.Drawing.Size(88, 22);
            this.btnPG_ShowStructure.Text = "ShowStructure";
            this.btnPG_ShowStructure.Click += new System.EventHandler(this.btnPG_ShowStructure_Click);
            // 
            // btnPG_ExpandAll
            // 
            this.btnPG_ExpandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPG_ExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("btnPG_ExpandAll.Image")));
            this.btnPG_ExpandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPG_ExpandAll.Name = "btnPG_ExpandAll";
            this.btnPG_ExpandAll.Size = new System.Drawing.Size(63, 22);
            this.btnPG_ExpandAll.Text = "ExpandAll";
            this.btnPG_ExpandAll.Click += new System.EventHandler(this.btnPG_ExpandAll_Click);
            // 
            // btnPG_CollapseAll
            // 
            this.btnPG_CollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPG_CollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("btnPG_CollapseAll.Image")));
            this.btnPG_CollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPG_CollapseAll.Name = "btnPG_CollapseAll";
            this.btnPG_CollapseAll.Size = new System.Drawing.Size(70, 22);
            this.btnPG_CollapseAll.Text = "CollapseAll";
            this.btnPG_CollapseAll.Click += new System.EventHandler(this.btnPG_CollapseAll_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPG_AddItem
            // 
            this.btnPG_AddItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPG_AddItem.Image = global::spMain.Properties.Resources.AddNew;
            this.btnPG_AddItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPG_AddItem.Name = "btnPG_AddItem";
            this.btnPG_AddItem.Size = new System.Drawing.Size(23, 22);
            this.btnPG_AddItem.Text = "Add item to collection";
            this.btnPG_AddItem.ToolTipText = "Add item to collection";
            this.btnPG_AddItem.Click += new System.EventHandler(this.btnPG_Add_Click);
            // 
            // btnPG_RemoveItem
            // 
            this.btnPG_RemoveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPG_RemoveItem.Image = global::spMain.Properties.Resources.Delete;
            this.btnPG_RemoveItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPG_RemoveItem.Name = "btnPG_RemoveItem";
            this.btnPG_RemoveItem.Size = new System.Drawing.Size(23, 22);
            this.btnPG_RemoveItem.Text = "Delete item from collection";
            this.btnPG_RemoveItem.ToolTipText = "Delete item from collection";
            this.btnPG_RemoveItem.Click += new System.EventHandler(this.btnPG_Remove_Click);
            // 
            // btnPG_ItemUp
            // 
            this.btnPG_ItemUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPG_ItemUp.Image = global::spMain.Properties.Resources.UpArrow;
            this.btnPG_ItemUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPG_ItemUp.Name = "btnPG_ItemUp";
            this.btnPG_ItemUp.Size = new System.Drawing.Size(23, 22);
            this.btnPG_ItemUp.Text = "Move collection item Up";
            this.btnPG_ItemUp.Click += new System.EventHandler(this.btnPG_ItemUp_Click);
            // 
            // btnPG_ItemDown
            // 
            this.btnPG_ItemDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPG_ItemDown.Image = global::spMain.Properties.Resources.DownArrow;
            this.btnPG_ItemDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPG_ItemDown.Name = "btnPG_ItemDown";
            this.btnPG_ItemDown.Size = new System.Drawing.Size(23, 22);
            this.btnPG_ItemDown.Text = "Move collection item Down";
            this.btnPG_ItemDown.Click += new System.EventHandler(this.btnPG_ItemDown_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPG_Save
            // 
            this.btnPG_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPG_Save.Image = global::spMain.Properties.Resources.Save;
            this.btnPG_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPG_Save.Name = "btnPG_Save";
            this.btnPG_Save.Size = new System.Drawing.Size(23, 22);
            this.btnPG_Save.Text = "Save this object to object collection";
            this.btnPG_Save.ToolTipText = "Save";
            this.btnPG_Save.Click += new System.EventHandler(this.btnPG_Save_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "window.ico");
            this.imageList.Images.SetKeyName(1, "field.ico");
            this.imageList.Images.SetKeyName(2, "prop.ico");
            this.imageList.Images.SetKeyName(3, "field_lock.ico");
            this.imageList.Images.SetKeyName(4, "prop_lock.ico");
            // 
            // PGfrmObjectEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 612);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PGfrmObjectEditor";
            this.Text = "frmObjectManager";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tsTreeView.ResumeLayout(false);
            this.tsTreeView.PerformLayout();
            this.tsPropertyGrid.ResumeLayout(false);
            this.tsPropertyGrid.PerformLayout();
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.PropertyGrid pg;
      private System.Windows.Forms.ToolStrip tsPropertyGrid;
      private System.Windows.Forms.ToolStripButton btnByCategory;
      private System.Windows.Forms.ToolStripButton btnByAlpha;
      private System.Windows.Forms.ToolStripSeparator separEdit;
      private System.Windows.Forms.ToolStripButton btnPG_ExpandAll;
      private System.Windows.Forms.ToolStripButton btnPG_CollapseAll;
      private System.Windows.Forms.ToolStrip tsTreeView;
      private System.Windows.Forms.ImageList imageList;
      private System.Windows.Forms.ToolStripButton btnCancel;
      private System.Windows.Forms.ToolStripButton btnOK;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
			private System.Windows.Forms.ToolStripButton btnPG_ShowStructure;
			private System.Windows.Forms.ToolStripButton btnPG_AddItem;
			private System.Windows.Forms.ToolStripButton btnPG_RemoveItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
      private System.Windows.Forms.ToolStripButton btnPG_Save;
      private System.Windows.Forms.ListBox listItems;
      private System.Windows.Forms.ToolStripButton btnList_Add;
      private System.Windows.Forms.ToolStripButton btnList_Delete;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
      private System.Windows.Forms.ToolStripButton btnList_Save;
      private System.Windows.Forms.ToolStripButton btnList_Edit;
      private System.Windows.Forms.ToolStripButton btnPG_ItemUp;
      private System.Windows.Forms.ToolStripButton btnPG_ItemDown;
    }
  }
