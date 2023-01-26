namespace spMain.Comp {
  partial class UI_StockGraph {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_StockGraph));
      this._toolStrip = new System.Windows.Forms.ToolStrip();
      this._btnDataSelect = new System.Windows.Forms.ToolStripButton();
      this._btnSaveAsFile = new System.Windows.Forms.ToolStripButton();
      this._btnPrint = new System.Windows.Forms.ToolStripButton();
      this._btnCopyToClipboard = new System.Windows.Forms.ToolStripButton();
      this._btnShowAllPoints = new System.Windows.Forms.ToolStripButton();
      this._btnSaveDataToFile = new System.Windows.Forms.ToolStripButton();
      this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
      this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
      this._stockGraph = new spMain.Comp.StockGraph();
      this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
      this._toolStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // _toolStrip
      // 
      this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnDataSelect,
            this._btnSaveAsFile,
            this._btnPrint,
            this._btnCopyToClipboard,
            this._btnShowAllPoints,
            this._btnSaveDataToFile,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
      this._toolStrip.Location = new System.Drawing.Point(0, 0);
      this._toolStrip.Name = "_toolStrip";
      this._toolStrip.Size = new System.Drawing.Size(713, 25);
      this._toolStrip.TabIndex = 0;
      this._toolStrip.Text = "toolStrip1";
      // 
      // _btnDataSelect
      // 
      this._btnDataSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this._btnDataSelect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
      this._btnDataSelect.Image = ((System.Drawing.Image)(resources.GetObject("_btnDataSelect.Image")));
      this._btnDataSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._btnDataSelect.Name = "_btnDataSelect";
      this._btnDataSelect.Size = new System.Drawing.Size(75, 22);
      this._btnDataSelect.Text = "Select data";
      this._btnDataSelect.Click += new System.EventHandler(this._btnDataSelect_Click);
      // 
      // _btnSaveAsFile
      // 
      this._btnSaveAsFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._btnSaveAsFile.Image = global::spMain.Properties.Resources.Save;
      this._btnSaveAsFile.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._btnSaveAsFile.Name = "_btnSaveAsFile";
      this._btnSaveAsFile.Size = new System.Drawing.Size(23, 22);
      this._btnSaveAsFile.Text = "Save as file";
      this._btnSaveAsFile.Click += new System.EventHandler(this._btnSaveAsFile_Click);
      // 
      // _btnPrint
      // 
      this._btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._btnPrint.Image = global::spMain.Properties.Resources.PrintPreview;
      this._btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._btnPrint.Name = "_btnPrint";
      this._btnPrint.Size = new System.Drawing.Size(23, 22);
      this._btnPrint.Text = "Print";
      this._btnPrint.Click += new System.EventHandler(this._btnPrint_Click);
      // 
      // _btnCopyToClipboard
      // 
      this._btnCopyToClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._btnCopyToClipboard.Image = global::spMain.Properties.Resources.Copy;
      this._btnCopyToClipboard.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._btnCopyToClipboard.Name = "_btnCopyToClipboard";
      this._btnCopyToClipboard.Size = new System.Drawing.Size(23, 22);
      this._btnCopyToClipboard.Text = "Copy to clipboard";
      this._btnCopyToClipboard.Click += new System.EventHandler(this._btnCopyToClipboard_Click);
      // 
      // _btnShowAllPoints
      // 
      this._btnShowAllPoints.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._btnShowAllPoints.Image = global::spMain.Properties.Resources.ShowAllPoints;
      this._btnShowAllPoints.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._btnShowAllPoints.Name = "_btnShowAllPoints";
      this._btnShowAllPoints.Size = new System.Drawing.Size(23, 22);
      this._btnShowAllPoints.Text = "Show all points";
      this._btnShowAllPoints.Click += new System.EventHandler(this._btnShowAllPoints_Click);
      // 
      // _btnSaveDataToFile
      // 
      this._btnSaveDataToFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._btnSaveDataToFile.Image = global::spMain.Properties.Resources.SaveDataToFile;
      this._btnSaveDataToFile.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._btnSaveDataToFile.Name = "_btnSaveDataToFile";
      this._btnSaveDataToFile.Size = new System.Drawing.Size(23, 22);
      this._btnSaveDataToFile.Text = "Save data to file";
      this._btnSaveDataToFile.Click += new System.EventHandler(this._btnSaveDataToFile_Click);
      // 
      // toolStripButton1
      // 
      this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
      this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton1.Name = "toolStripButton1";
      this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
      this.toolStripButton1.Text = "toolStripButton1";
      // 
      // toolStripButton2
      // 
      this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
      this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton2.Name = "toolStripButton2";
      this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
      this.toolStripButton2.Text = "toolStripButton2";
      this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
      // 
      // _stockGraph
      // 
      this._stockGraph.Dock = System.Windows.Forms.DockStyle.Fill;
      this._stockGraph.IsEnableVZoom = false;
      this._stockGraph.IsShowHScrollBar = true;
      this._stockGraph.IsSynchronizeXAxes = true;
      this._stockGraph.Location = new System.Drawing.Point(0, 25);
      this._stockGraph.Name = "_stockGraph";
      this._stockGraph.ScrollGrace = 0;
      this._stockGraph.ScrollMaxX = 0;
      this._stockGraph.ScrollMaxY = 0;
      this._stockGraph.ScrollMaxY2 = 0;
      this._stockGraph.ScrollMinX = 0;
      this._stockGraph.ScrollMinY = 0;
      this._stockGraph.ScrollMinY2 = 0;
      this._stockGraph.Size = new System.Drawing.Size(713, 403);
      this._stockGraph.TabIndex = 1;
      // 
      // toolStripButton3
      // 
      this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
      this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton3.Name = "toolStripButton3";
      this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
      this.toolStripButton3.Text = "toolStripButton3";
      this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
      // 
      // UI_StockGraph
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._stockGraph);
      this.Controls.Add(this._toolStrip);
      this.Name = "UI_StockGraph";
      this.Size = new System.Drawing.Size(713, 428);
      this._toolStrip.ResumeLayout(false);
      this._toolStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip _toolStrip;
    private StockGraph _stockGraph;
    private System.Windows.Forms.ToolStripButton _btnSaveAsFile;
    private System.Windows.Forms.ToolStripButton _btnPrint;
    private System.Windows.Forms.ToolStripButton _btnCopyToClipboard;
    private System.Windows.Forms.ToolStripButton _btnShowAllPoints;
    private System.Windows.Forms.ToolStripButton _btnDataSelect;
    private System.Windows.Forms.ToolStripButton _btnSaveDataToFile;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
    private System.Windows.Forms.ToolStripButton toolStripButton2;
    private System.Windows.Forms.ToolStripButton toolStripButton3;

  }
}