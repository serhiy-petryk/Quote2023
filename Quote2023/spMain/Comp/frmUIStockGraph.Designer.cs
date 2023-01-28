namespace spMain.Comp {
  partial class frmUIStockGraph {
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
            this.uI_StockGraph1 = new spMain.Comp.UI_StockGraph();
            this.SuspendLayout();
            // 
            // uI_StockGraph1
            // 
            this.uI_StockGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uI_StockGraph1.Location = new System.Drawing.Point(0, 0);
            this.uI_StockGraph1.Name = "uI_StockGraph1";
            this.uI_StockGraph1.Size = new System.Drawing.Size(925, 510);
            this.uI_StockGraph1.TabIndex = 0;
            // 
            // frmUIStockGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 510);
            this.Controls.Add(this.uI_StockGraph1);
            this.Name = "frmUIStockGraph";
            this.Text = "frmUIStockGraph";
            this.Load += new System.EventHandler(this.frmUIStockGraph_Load);
            this.ResumeLayout(false);

    }

    #endregion

    private UI_StockGraph uI_StockGraph1;
  }
}