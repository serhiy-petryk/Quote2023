
namespace WebScanner
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dtpEndAt = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.dtpRunAt = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cbNasdaqScreener = new System.Windows.Forms.CheckBox();
            this.cbTradingViewScreener = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1.Location = new System.Drawing.Point(35, 132);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(92, 24);
            this.checkBox1.TabIndex = 45;
            this.checkBox1.Text = "Timer On";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // dtpEndAt
            // 
            this.dtpEndAt.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpEndAt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndAt.Location = new System.Drawing.Point(52, 36);
            this.dtpEndAt.Margin = new System.Windows.Forms.Padding(4, 7, 4, 4);
            this.dtpEndAt.Name = "dtpEndAt";
            this.dtpEndAt.Size = new System.Drawing.Size(159, 23);
            this.dtpEndAt.TabIndex = 43;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 36);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(49, 23);
            this.label3.TabIndex = 44;
            this.label3.Text = "End at";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label2.Size = new System.Drawing.Size(122, 23);
            this.label2.TabIndex = 42;
            this.label2.Text = "Interval in minutes";
            // 
            // nudInterval
            // 
            this.nudInterval.Location = new System.Drawing.Point(160, 75);
            this.nudInterval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(51, 23);
            this.nudInterval.TabIndex = 41;
            this.nudInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudInterval.ValueChanged += new System.EventHandler(this.nudInterval_ValueChanged);
            // 
            // dtpRunAt
            // 
            this.dtpRunAt.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpRunAt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpRunAt.Location = new System.Drawing.Point(52, 0);
            this.dtpRunAt.Margin = new System.Windows.Forms.Padding(4, 7, 4, 4);
            this.dtpRunAt.Name = "dtpRunAt";
            this.dtpRunAt.Size = new System.Drawing.Size(159, 23);
            this.dtpRunAt.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(50, 23);
            this.label1.TabIndex = 40;
            this.label1.Text = "Run at";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cbNasdaqScreener
            // 
            this.cbNasdaqScreener.AutoSize = true;
            this.cbNasdaqScreener.Checked = true;
            this.cbNasdaqScreener.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNasdaqScreener.Location = new System.Drawing.Point(404, 34);
            this.cbNasdaqScreener.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbNasdaqScreener.Name = "cbNasdaqScreener";
            this.cbNasdaqScreener.Size = new System.Drawing.Size(138, 21);
            this.cbNasdaqScreener.TabIndex = 46;
            this.cbNasdaqScreener.Text = "Nasdaq Screener";
            this.cbNasdaqScreener.UseVisualStyleBackColor = true;
            // 
            // cbTradingViewScreener
            // 
            this.cbTradingViewScreener.AutoSize = true;
            this.cbTradingViewScreener.Checked = true;
            this.cbTradingViewScreener.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTradingViewScreener.Location = new System.Drawing.Point(404, 74);
            this.cbTradingViewScreener.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbTradingViewScreener.Name = "cbTradingViewScreener";
            this.cbTradingViewScreener.Size = new System.Drawing.Size(167, 21);
            this.cbTradingViewScreener.TabIndex = 47;
            this.cbTradingViewScreener.Text = "TradingView Screener";
            this.cbTradingViewScreener.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 312);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(952, 22);
            this.statusStrip1.TabIndex = 48;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(118, 17);
            this.StatusLabel.Text = "toolStripStatusLabel1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(425, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 49;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 334);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cbTradingViewScreener);
            this.Controls.Add(this.cbNasdaqScreener);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.dtpEndAt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudInterval);
            this.Controls.Add(this.dtpRunAt);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.DateTimePicker dtpEndAt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudInterval;
        public System.Windows.Forms.DateTimePicker dtpRunAt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox cbNasdaqScreener;
        private System.Windows.Forms.CheckBox cbTradingViewScreener;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Button button1;
    }
}

