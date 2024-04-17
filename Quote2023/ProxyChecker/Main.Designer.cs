
namespace ProxyChecker
{
    partial class Main
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
            this.cbProxyScrape = new System.Windows.Forms.CheckBox();
            this.cbProxyListDownload = new System.Windows.Forms.CheckBox();
            this.cbFreeProxyList = new System.Windows.Forms.CheckBox();
            this.cbVPN = new System.Windows.Forms.CheckBox();
            this.cbProxyList = new System.Windows.Forms.CheckBox();
            this.cbList1 = new System.Windows.Forms.CheckBox();
            this.cbList2 = new System.Windows.Forms.CheckBox();
            this.cbList3 = new System.Windows.Forms.CheckBox();
            this.cbList4 = new System.Windows.Forms.CheckBox();
            this.cbList5 = new System.Windows.Forms.CheckBox();
            this.cbList6 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox_Results
            // 
            this.textBox_Results.Location = new System.Drawing.Point(10, 12);
            this.textBox_Results.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Results.Multiline = true;
            this.textBox_Results.Name = "textBox_Results";
            this.textBox_Results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Results.Size = new System.Drawing.Size(452, 310);
            this.textBox_Results.TabIndex = 0;
            // 
            // btnStartApi
            // 
            this.btnStartApi.Location = new System.Drawing.Point(503, 353);
            this.btnStartApi.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartApi.Name = "btnStartApi";
            this.btnStartApi.Size = new System.Drawing.Size(138, 57);
            this.btnStartApi.TabIndex = 2;
            this.btnStartApi.Text = "Start (from API)";
            this.btnStartApi.UseVisualStyleBackColor = true;
            this.btnStartApi.Click += new System.EventHandler(this.button_StartApi_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 342);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Total Proxy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 373);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Good Proxy";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 403);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Bad Proxy";
            // 
            // label_TotalProxy
            // 
            this.label_TotalProxy.AutoSize = true;
            this.label_TotalProxy.Location = new System.Drawing.Point(93, 343);
            this.label_TotalProxy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_TotalProxy.Name = "label_TotalProxy";
            this.label_TotalProxy.Size = new System.Drawing.Size(13, 15);
            this.label_TotalProxy.TabIndex = 6;
            this.label_TotalProxy.Text = "0";
            // 
            // label_GoodProxy
            // 
            this.label_GoodProxy.AutoSize = true;
            this.label_GoodProxy.Location = new System.Drawing.Point(93, 373);
            this.label_GoodProxy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_GoodProxy.Name = "label_GoodProxy";
            this.label_GoodProxy.Size = new System.Drawing.Size(13, 15);
            this.label_GoodProxy.TabIndex = 7;
            this.label_GoodProxy.Text = "0";
            // 
            // label_BadProxy
            // 
            this.label_BadProxy.AutoSize = true;
            this.label_BadProxy.Location = new System.Drawing.Point(90, 404);
            this.label_BadProxy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_BadProxy.Name = "label_BadProxy";
            this.label_BadProxy.Size = new System.Drawing.Size(13, 15);
            this.label_BadProxy.TabIndex = 8;
            this.label_BadProxy.Text = "0";
            // 
            // cbProxyScrape
            // 
            this.cbProxyScrape.AutoSize = true;
            this.cbProxyScrape.Checked = true;
            this.cbProxyScrape.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbProxyScrape.Location = new System.Drawing.Point(132, 343);
            this.cbProxyScrape.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbProxyScrape.Name = "cbProxyScrape";
            this.cbProxyScrape.Size = new System.Drawing.Size(135, 19);
            this.cbProxyScrape.TabIndex = 10;
            this.cbProxyScrape.Text = "api.proxyscrape.com";
            this.cbProxyScrape.UseVisualStyleBackColor = true;
            // 
            // cbProxyListDownload
            // 
            this.cbProxyListDownload.AutoSize = true;
            this.cbProxyListDownload.Checked = true;
            this.cbProxyListDownload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbProxyListDownload.Location = new System.Drawing.Point(132, 373);
            this.cbProxyListDownload.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbProxyListDownload.Name = "cbProxyListDownload";
            this.cbProxyListDownload.Size = new System.Drawing.Size(161, 19);
            this.cbProxyListDownload.TabIndex = 11;
            this.cbProxyListDownload.Text = "www.proxy-list.download";
            this.cbProxyListDownload.UseVisualStyleBackColor = true;
            // 
            // cbFreeProxyList
            // 
            this.cbFreeProxyList.AutoSize = true;
            this.cbFreeProxyList.Checked = true;
            this.cbFreeProxyList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFreeProxyList.Location = new System.Drawing.Point(132, 403);
            this.cbFreeProxyList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbFreeProxyList.Name = "cbFreeProxyList";
            this.cbFreeProxyList.Size = new System.Drawing.Size(120, 19);
            this.cbFreeProxyList.TabIndex = 12;
            this.cbFreeProxyList.Text = "free-proxy-list.net";
            this.cbFreeProxyList.UseVisualStyleBackColor = true;
            // 
            // cbVPN
            // 
            this.cbVPN.AutoSize = true;
            this.cbVPN.Location = new System.Drawing.Point(132, 438);
            this.cbVPN.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbVPN.Name = "cbVPN";
            this.cbVPN.Size = new System.Drawing.Size(95, 19);
            this.cbVPN.TabIndex = 13;
            this.cbVPN.Text = "151.115.56.95";
            this.cbVPN.UseVisualStyleBackColor = true;
            // 
            // cbProxyList
            // 
            this.cbProxyList.AutoSize = true;
            this.cbProxyList.Location = new System.Drawing.Point(132, 464);
            this.cbProxyList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbProxyList.Name = "cbProxyList";
            this.cbProxyList.Size = new System.Drawing.Size(73, 19);
            this.cbProxyList.TabIndex = 14;
            this.cbProxyList.Text = "Proxy list";
            this.cbProxyList.UseVisualStyleBackColor = true;
            // 
            // cbList1
            // 
            this.cbList1.AutoSize = true;
            this.cbList1.Checked = true;
            this.cbList1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbList1.Location = new System.Drawing.Point(316, 327);
            this.cbList1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbList1.Name = "cbList1";
            this.cbList1.Size = new System.Drawing.Size(82, 19);
            this.cbList1.TabIndex = 15;
            this.cbList1.Text = "Proxy list 1";
            this.cbList1.UseVisualStyleBackColor = true;
            // 
            // cbList2
            // 
            this.cbList2.AutoSize = true;
            this.cbList2.Checked = true;
            this.cbList2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbList2.Location = new System.Drawing.Point(316, 353);
            this.cbList2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbList2.Name = "cbList2";
            this.cbList2.Size = new System.Drawing.Size(82, 19);
            this.cbList2.TabIndex = 16;
            this.cbList2.Text = "Proxy list 2";
            this.cbList2.UseVisualStyleBackColor = true;
            // 
            // cbList3
            // 
            this.cbList3.AutoSize = true;
            this.cbList3.Checked = true;
            this.cbList3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbList3.Location = new System.Drawing.Point(316, 383);
            this.cbList3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbList3.Name = "cbList3";
            this.cbList3.Size = new System.Drawing.Size(82, 19);
            this.cbList3.TabIndex = 17;
            this.cbList3.Text = "Proxy list 3";
            this.cbList3.UseVisualStyleBackColor = true;
            // 
            // cbList4
            // 
            this.cbList4.AutoSize = true;
            this.cbList4.Checked = true;
            this.cbList4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbList4.Location = new System.Drawing.Point(316, 410);
            this.cbList4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbList4.Name = "cbList4";
            this.cbList4.Size = new System.Drawing.Size(82, 19);
            this.cbList4.TabIndex = 18;
            this.cbList4.Text = "Proxy list 4";
            this.cbList4.UseVisualStyleBackColor = true;
            // 
            // cbList5
            // 
            this.cbList5.AutoSize = true;
            this.cbList5.Checked = true;
            this.cbList5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbList5.Location = new System.Drawing.Point(316, 435);
            this.cbList5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbList5.Name = "cbList5";
            this.cbList5.Size = new System.Drawing.Size(82, 19);
            this.cbList5.TabIndex = 19;
            this.cbList5.Text = "Proxy list 5";
            this.cbList5.UseVisualStyleBackColor = true;
            // 
            // cbList6
            // 
            this.cbList6.AutoSize = true;
            this.cbList6.Checked = true;
            this.cbList6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbList6.Location = new System.Drawing.Point(316, 460);
            this.cbList6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbList6.Name = "cbList6";
            this.cbList6.Size = new System.Drawing.Size(82, 19);
            this.cbList6.TabIndex = 20;
            this.cbList6.Text = "Proxy list 6";
            this.cbList6.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 495);
            this.Controls.Add(this.cbList6);
            this.Controls.Add(this.cbList5);
            this.Controls.Add(this.cbList4);
            this.Controls.Add(this.cbList3);
            this.Controls.Add(this.cbList2);
            this.Controls.Add(this.cbList1);
            this.Controls.Add(this.cbProxyList);
            this.Controls.Add(this.cbVPN);
            this.Controls.Add(this.cbFreeProxyList);
            this.Controls.Add(this.cbProxyListDownload);
            this.Controls.Add(this.cbProxyScrape);
            this.Controls.Add(this.label_BadProxy);
            this.Controls.Add(this.label_GoodProxy);
            this.Controls.Add(this.label_TotalProxy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStartApi);
            this.Controls.Add(this.textBox_Results);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
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
        private System.Windows.Forms.CheckBox cbProxyScrape;
        private System.Windows.Forms.CheckBox cbProxyListDownload;
        private System.Windows.Forms.CheckBox cbFreeProxyList;
        private System.Windows.Forms.CheckBox cbVPN;
        private System.Windows.Forms.CheckBox cbProxyList;
        private System.Windows.Forms.CheckBox cbList1;
        private System.Windows.Forms.CheckBox cbList2;
        private System.Windows.Forms.CheckBox cbList3;
        private System.Windows.Forms.CheckBox cbList4;
        private System.Windows.Forms.CheckBox cbList5;
        private System.Windows.Forms.CheckBox cbList6;
    }
}

