
namespace ProxyChecker
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
            this.textBox_Results = new System.Windows.Forms.TextBox();
            this.btnStartApi = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_TotalProxy = new System.Windows.Forms.Label();
            this.label_GoodProxy = new System.Windows.Forms.Label();
            this.label_BadProxy = new System.Windows.Forms.Label();
            this.btnStartFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_Results
            // 
            this.textBox_Results.Location = new System.Drawing.Point(9, 10);
            this.textBox_Results.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Results.Multiline = true;
            this.textBox_Results.Name = "textBox_Results";
            this.textBox_Results.Size = new System.Drawing.Size(388, 269);
            this.textBox_Results.TabIndex = 0;
            // 
            // btnStartApi
            // 
            this.btnStartApi.Location = new System.Drawing.Point(137, 296);
            this.btnStartApi.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartApi.Name = "btnStartApi";
            this.btnStartApi.Size = new System.Drawing.Size(118, 49);
            this.btnStartApi.TabIndex = 2;
            this.btnStartApi.Text = "Start (from API)";
            this.btnStartApi.UseVisualStyleBackColor = true;
            this.btnStartApi.Click += new System.EventHandler(this.button_StartApi_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 296);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Total Proxy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 323);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Good Proxy";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 349);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Bad Proxy";
            // 
            // label_TotalProxy
            // 
            this.label_TotalProxy.AutoSize = true;
            this.label_TotalProxy.Location = new System.Drawing.Point(80, 297);
            this.label_TotalProxy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_TotalProxy.Name = "label_TotalProxy";
            this.label_TotalProxy.Size = new System.Drawing.Size(13, 13);
            this.label_TotalProxy.TabIndex = 6;
            this.label_TotalProxy.Text = "0";
            // 
            // label_GoodProxy
            // 
            this.label_GoodProxy.AutoSize = true;
            this.label_GoodProxy.Location = new System.Drawing.Point(80, 323);
            this.label_GoodProxy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_GoodProxy.Name = "label_GoodProxy";
            this.label_GoodProxy.Size = new System.Drawing.Size(13, 13);
            this.label_GoodProxy.TabIndex = 7;
            this.label_GoodProxy.Text = "0";
            // 
            // label_BadProxy
            // 
            this.label_BadProxy.AutoSize = true;
            this.label_BadProxy.Location = new System.Drawing.Point(77, 350);
            this.label_BadProxy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_BadProxy.Name = "label_BadProxy";
            this.label_BadProxy.Size = new System.Drawing.Size(13, 13);
            this.label_BadProxy.TabIndex = 8;
            this.label_BadProxy.Text = "0";
            // 
            // btnStartFile
            // 
            this.btnStartFile.Location = new System.Drawing.Point(279, 297);
            this.btnStartFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartFile.Name = "btnStartFile";
            this.btnStartFile.Size = new System.Drawing.Size(118, 49);
            this.btnStartFile.TabIndex = 9;
            this.btnStartFile.Text = "Start (from file)";
            this.btnStartFile.UseVisualStyleBackColor = true;
            this.btnStartFile.Click += new System.EventHandler(this.btnStartFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 378);
            this.Controls.Add(this.btnStartFile);
            this.Controls.Add(this.label_BadProxy);
            this.Controls.Add(this.label_GoodProxy);
            this.Controls.Add(this.label_TotalProxy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStartApi);
            this.Controls.Add(this.textBox_Results);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "MyProxy Scraper and Checker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Results;
        private System.Windows.Forms.Button btnStartApi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_TotalProxy;
        private System.Windows.Forms.Label label_GoodProxy;
        private System.Windows.Forms.Label label_BadProxy;
        private System.Windows.Forms.Button btnStartFile;
    }
}

