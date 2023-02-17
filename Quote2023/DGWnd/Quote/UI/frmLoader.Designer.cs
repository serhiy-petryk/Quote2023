
namespace DGWnd.Quote.UI
{
    partial class frmLoader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSaveIntradaySnapshotsToDb = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnStopSavingIntradaySnapshotsToDb = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 337);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(806, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(118, 17);
            this.statusLabel.Text = "toolStripStatusLabel1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(806, 337);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnStopSavingIntradaySnapshotsToDb);
            this.tabPage1.Controls.Add(this.btnSaveIntradaySnapshotsToDb);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(798, 311);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Loader";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSaveIntradaySnapshotsToDb
            // 
            this.btnSaveIntradaySnapshotsToDb.Location = new System.Drawing.Point(470, 17);
            this.btnSaveIntradaySnapshotsToDb.Name = "btnSaveIntradaySnapshotsToDb";
            this.btnSaveIntradaySnapshotsToDb.Size = new System.Drawing.Size(234, 23);
            this.btnSaveIntradaySnapshotsToDb.TabIndex = 0;
            this.btnSaveIntradaySnapshotsToDb.Text = "Save Intraday Quote Snapshots to DB";
            this.btnSaveIntradaySnapshotsToDb.UseVisualStyleBackColor = true;
            this.btnSaveIntradaySnapshotsToDb.Click += new System.EventHandler(this.btnSaveIntradaySnapshotsToDb_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(798, 311);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnStopSavingIntradaySnapshotsToDb
            // 
            this.btnStopSavingIntradaySnapshotsToDb.Location = new System.Drawing.Point(470, 60);
            this.btnStopSavingIntradaySnapshotsToDb.Name = "btnStopSavingIntradaySnapshotsToDb";
            this.btnStopSavingIntradaySnapshotsToDb.Size = new System.Drawing.Size(234, 23);
            this.btnStopSavingIntradaySnapshotsToDb.TabIndex = 1;
            this.btnStopSavingIntradaySnapshotsToDb.Text = "Stop saving Intraday Quote Snapshots to DB";
            this.btnStopSavingIntradaySnapshotsToDb.UseVisualStyleBackColor = true;
            this.btnStopSavingIntradaySnapshotsToDb.Click += new System.EventHandler(this.btnStopSavingIntradaySnapshotsToDb_Click);
            // 
            // frmLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 359);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmLoader";
            this.Text = "Loader";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button btnSaveIntradaySnapshotsToDb;
        private System.Windows.Forms.Button btnStopSavingIntradaySnapshotsToDb;
    }
}