namespace WebDownloader {
  partial class frmWebDownloader {
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
      this.panel1 = new System.Windows.Forms.Panel();
      this.cbSkipRequest = new System.Windows.Forms.CheckBox();
      this.cbSilent = new System.Windows.Forms.CheckBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txtBody = new System.Windows.Forms.TextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnPost = new System.Windows.Forms.RadioButton();
      this.btnGet = new System.Windows.Forms.RadioButton();
      this.btnRun = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.txtSessions = new System.Windows.Forms.NumericUpDown();
      this.txtUrlTemplate = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.panel2 = new System.Windows.Forms.Panel();
      this.btnExample = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.txtFilenameTemplate = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.txtParams = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.numDelay = new System.Windows.Forms.NumericUpDown();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.txtSessions)).BeginInit();
      this.panel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.cbSkipRequest);
      this.panel1.Controls.Add(this.cbSilent);
      this.panel1.Controls.Add(this.label5);
      this.panel1.Controls.Add(this.txtBody);
      this.panel1.Controls.Add(this.groupBox1);
      this.panel1.Controls.Add(this.btnRun);
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.txtSessions);
      this.panel1.Controls.Add(this.txtUrlTemplate);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(897, 69);
      this.panel1.TabIndex = 0;
      // 
      // cbSkipRequest
      // 
      this.cbSkipRequest.AutoSize = true;
      this.cbSkipRequest.Checked = true;
      this.cbSkipRequest.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbSkipRequest.Location = new System.Drawing.Point(13, 47);
      this.cbSkipRequest.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.cbSkipRequest.Name = "cbSkipRequest";
      this.cbSkipRequest.Size = new System.Drawing.Size(150, 19);
      this.cbSkipRequest.TabIndex = 13;
      this.cbSkipRequest.Text = "Skip request if file exists";
      this.cbSkipRequest.UseVisualStyleBackColor = true;
      // 
      // cbSilent
      // 
      this.cbSilent.AutoSize = true;
      this.cbSilent.Location = new System.Drawing.Point(13, 24);
      this.cbSilent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.cbSilent.Name = "cbSilent";
      this.cbSilent.Size = new System.Drawing.Size(89, 19);
      this.cbSilent.TabIndex = 9;
      this.cbSilent.Text = "Silent Mode";
      this.cbSilent.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(259, 38);
      this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(34, 15);
      this.label5.TabIndex = 11;
      this.label5.Text = "Body";
      // 
      // txtBody
      // 
      this.txtBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtBody.Location = new System.Drawing.Point(295, 35);
      this.txtBody.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.txtBody.Name = "txtBody";
      this.txtBody.Size = new System.Drawing.Size(507, 23);
      this.txtBody.TabIndex = 10;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnPost);
      this.groupBox1.Controls.Add(this.btnGet);
      this.groupBox1.Location = new System.Drawing.Point(160, 3);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.groupBox1.Size = new System.Drawing.Size(53, 61);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      // 
      // btnPost
      // 
      this.btnPost.AutoSize = true;
      this.btnPost.Location = new System.Drawing.Point(7, 35);
      this.btnPost.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.btnPost.Name = "btnPost";
      this.btnPost.Size = new System.Drawing.Size(48, 19);
      this.btnPost.TabIndex = 1;
      this.btnPost.Text = "Post";
      this.btnPost.UseVisualStyleBackColor = true;
      // 
      // btnGet
      // 
      this.btnGet.AutoSize = true;
      this.btnGet.Checked = true;
      this.btnGet.Location = new System.Drawing.Point(7, 8);
      this.btnGet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.btnGet.Name = "btnGet";
      this.btnGet.Size = new System.Drawing.Size(43, 19);
      this.btnGet.TabIndex = 0;
      this.btnGet.TabStop = true;
      this.btnGet.Text = "Get";
      this.btnGet.UseVisualStyleBackColor = true;
      // 
      // btnRun
      // 
      this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRun.Location = new System.Drawing.Point(806, 2);
      this.btnRun.Margin = new System.Windows.Forms.Padding(0);
      this.btnRun.Name = "btnRun";
      this.btnRun.Size = new System.Drawing.Size(88, 61);
      this.btnRun.TabIndex = 4;
      this.btnRun.Text = "Run";
      this.btnRun.UseVisualStyleBackColor = true;
      this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(221, 6);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(72, 15);
      this.label2.TabIndex = 3;
      this.label2.Text = "Url template";
      // 
      // txtSessions
      // 
      this.txtSessions.Location = new System.Drawing.Point(66, 0);
      this.txtSessions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.txtSessions.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
      this.txtSessions.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.txtSessions.Name = "txtSessions";
      this.txtSessions.Size = new System.Drawing.Size(52, 23);
      this.txtSessions.TabIndex = 2;
      this.txtSessions.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
      // 
      // txtUrlTemplate
      // 
      this.txtUrlTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtUrlTemplate.Location = new System.Drawing.Point(295, 2);
      this.txtUrlTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.txtUrlTemplate.Name = "txtUrlTemplate";
      this.txtUrlTemplate.Size = new System.Drawing.Size(507, 23);
      this.txtUrlTemplate.TabIndex = 1;
      this.txtUrlTemplate.Text = "http:\\\\...{0}";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(4, 5);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(54, 15);
      this.label1.TabIndex = 0;
      this.label1.Text = "Sessions:";
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.label6);
      this.panel2.Controls.Add(this.numDelay);
      this.panel2.Controls.Add(this.btnExample);
      this.panel2.Controls.Add(this.label3);
      this.panel2.Controls.Add(this.txtFilenameTemplate);
      this.panel2.Controls.Add(this.label4);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel2.Location = new System.Drawing.Point(0, 69);
      this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(897, 51);
      this.panel2.TabIndex = 1;
      // 
      // btnExample
      // 
      this.btnExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExample.Location = new System.Drawing.Point(806, 3);
      this.btnExample.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.btnExample.Name = "btnExample";
      this.btnExample.Size = new System.Drawing.Size(88, 24);
      this.btnExample.TabIndex = 8;
      this.btnExample.Text = "Examples";
      this.btnExample.UseVisualStyleBackColor = true;
      this.btnExample.Click += new System.EventHandler(this.btnExamples_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(219, 5);
      this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(105, 15);
      this.label3.TabIndex = 7;
      this.label3.Text = "Filename template";
      // 
      // txtFilenameTemplate
      // 
      this.txtFilenameTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtFilenameTemplate.Location = new System.Drawing.Point(325, 3);
      this.txtFilenameTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.txtFilenameTemplate.Name = "txtFilenameTemplate";
      this.txtFilenameTemplate.Size = new System.Drawing.Size(477, 23);
      this.txtFilenameTemplate.TabIndex = 6;
      this.txtFilenameTemplate.Text = "T:\\Data\\...{0}.txt";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(4, 30);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(285, 15);
      this.label4.TabIndex = 5;
      this.label4.Text = "Parameters: delimeters are \";\" or \",\" or \"|\" or NewLine";
      // 
      // txtParams
      // 
      this.txtParams.Dock = System.Windows.Forms.DockStyle.Fill;
      this.txtParams.Location = new System.Drawing.Point(0, 120);
      this.txtParams.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.txtParams.MaxLength = 0;
      this.txtParams.Multiline = true;
      this.txtParams.Name = "txtParams";
      this.txtParams.Size = new System.Drawing.Size(897, 308);
      this.txtParams.TabIndex = 2;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(4, 6);
      this.label6.Margin = new System.Windows.Forms.Padding(0);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(153, 15);
      this.label6.TabIndex = 16;
      this.label6.Text = "Delay between requests, ms";
      // 
      // numDelay
      // 
      this.numDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this.numDelay.Location = new System.Drawing.Point(160, 3);
      this.numDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numDelay.Name = "numDelay";
      this.numDelay.Size = new System.Drawing.Size(52, 23);
      this.numDelay.TabIndex = 15;
      // 
      // frmWebDownloader
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(897, 428);
      this.Controls.Add(this.txtParams);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.Name = "frmWebDownloader";
      this.Text = "frmWebDownloader";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.txtSessions)).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnRun;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtUrlTemplate;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.TextBox txtParams;
    private System.Windows.Forms.NumericUpDown txtSessions;
    private System.Windows.Forms.Button btnExample;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtFilenameTemplate;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton btnPost;
    private System.Windows.Forms.RadioButton btnGet;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtBody;
    private System.Windows.Forms.CheckBox cbSilent;
        private System.Windows.Forms.CheckBox cbSkipRequest;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.NumericUpDown numDelay;
  }
}