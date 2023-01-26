namespace spMain.QData.Common {
//  public partial class TimeInterval {
    partial class TimeIntervalFormEditor {
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
        this.numCustom = new System.Windows.Forms.NumericUpDown();
        this.btnCustom = new System.Windows.Forms.RadioButton();
        this.btn30 = new System.Windows.Forms.RadioButton();
        this.btn15 = new System.Windows.Forms.RadioButton();
        this.btn10 = new System.Windows.Forms.RadioButton();
        this.btn5 = new System.Windows.Forms.RadioButton();
        this.btn3 = new System.Windows.Forms.RadioButton();
        this.btn2 = new System.Windows.Forms.RadioButton();
        this.btn1 = new System.Windows.Forms.RadioButton();
        this.btnYear = new System.Windows.Forms.RadioButton();
        this.btnMonth = new System.Windows.Forms.RadioButton();
        this.btnWeek = new System.Windows.Forms.RadioButton();
        this.btnDay = new System.Windows.Forms.RadioButton();
        this.btn60 = new System.Windows.Forms.RadioButton();
        this.btn30s = new System.Windows.Forms.RadioButton();
        this.btn20s = new System.Windows.Forms.RadioButton();
        this.btn15s = new System.Windows.Forms.RadioButton();
        this.btn10s = new System.Windows.Forms.RadioButton();
        this.btn5s = new System.Windows.Forms.RadioButton();
        this.btn3s = new System.Windows.Forms.RadioButton();
        this.btn2s = new System.Windows.Forms.RadioButton();
        this.btn1s = new System.Windows.Forms.RadioButton();
        ((System.ComponentModel.ISupportInitialize)(this.numCustom)).BeginInit();
        this.SuspendLayout();
        // 
        // numCustom
        // 
        this.numCustom.Location = new System.Drawing.Point(105, 202);
        this.numCustom.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
        this.numCustom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
        this.numCustom.Name = "numCustom";
        this.numCustom.Size = new System.Drawing.Size(43, 20);
        this.numCustom.TabIndex = 28;
        this.numCustom.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
        // 
        // btnCustom
        // 
        this.btnCustom.AutoSize = true;
        this.btnCustom.Location = new System.Drawing.Point(3, 202);
        this.btnCustom.Name = "btnCustom";
        this.btnCustom.Size = new System.Drawing.Size(105, 17);
        this.btnCustom.TabIndex = 27;
        this.btnCustom.Text = "Custom Seconds";
        this.btnCustom.UseVisualStyleBackColor = true;
        this.btnCustom.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn30
        // 
        this.btn30.AutoSize = true;
        this.btn30.Location = new System.Drawing.Point(105, 142);
        this.btn30.Name = "btn30";
        this.btn30.Size = new System.Drawing.Size(77, 17);
        this.btn30.TabIndex = 21;
        this.btn30.Text = "30 Minutes";
        this.btn30.UseVisualStyleBackColor = true;
        this.btn30.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn15
        // 
        this.btn15.AutoSize = true;
        this.btn15.Location = new System.Drawing.Point(105, 119);
        this.btn15.Name = "btn15";
        this.btn15.Size = new System.Drawing.Size(77, 17);
        this.btn15.TabIndex = 20;
        this.btn15.Text = "15 Minutes";
        this.btn15.UseVisualStyleBackColor = true;
        this.btn15.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn10
        // 
        this.btn10.AutoSize = true;
        this.btn10.Location = new System.Drawing.Point(105, 96);
        this.btn10.Name = "btn10";
        this.btn10.Size = new System.Drawing.Size(77, 17);
        this.btn10.TabIndex = 19;
        this.btn10.Text = "10 Minutes";
        this.btn10.UseVisualStyleBackColor = true;
        this.btn10.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn5
        // 
        this.btn5.AutoSize = true;
        this.btn5.Location = new System.Drawing.Point(105, 73);
        this.btn5.Name = "btn5";
        this.btn5.Size = new System.Drawing.Size(71, 17);
        this.btn5.TabIndex = 18;
        this.btn5.Text = "5 Minutes";
        this.btn5.UseVisualStyleBackColor = true;
        this.btn5.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn3
        // 
        this.btn3.AutoSize = true;
        this.btn3.Location = new System.Drawing.Point(105, 50);
        this.btn3.Name = "btn3";
        this.btn3.Size = new System.Drawing.Size(71, 17);
        this.btn3.TabIndex = 17;
        this.btn3.Text = "3 Minutes";
        this.btn3.UseVisualStyleBackColor = true;
        this.btn3.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn2
        // 
        this.btn2.AutoSize = true;
        this.btn2.Location = new System.Drawing.Point(105, 27);
        this.btn2.Name = "btn2";
        this.btn2.Size = new System.Drawing.Size(71, 17);
        this.btn2.TabIndex = 16;
        this.btn2.Text = "2 Minutes";
        this.btn2.UseVisualStyleBackColor = true;
        this.btn2.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn1
        // 
        this.btn1.AutoSize = true;
        this.btn1.Checked = true;
        this.btn1.Location = new System.Drawing.Point(105, 3);
        this.btn1.Name = "btn1";
        this.btn1.Size = new System.Drawing.Size(66, 17);
        this.btn1.TabIndex = 15;
        this.btn1.TabStop = true;
        this.btn1.Text = "1 Minute";
        this.btn1.UseVisualStyleBackColor = true;
        this.btn1.Click += new System.EventHandler(this.btn_Click);
        // 
        // btnYear
        // 
        this.btnYear.AutoSize = true;
        this.btnYear.Location = new System.Drawing.Point(200, 73);
        this.btnYear.Name = "btnYear";
        this.btnYear.Size = new System.Drawing.Size(47, 17);
        this.btnYear.TabIndex = 26;
        this.btnYear.Text = "Year";
        this.btnYear.UseVisualStyleBackColor = true;
        this.btnYear.Click += new System.EventHandler(this.btn_Click);
        // 
        // btnMonth
        // 
        this.btnMonth.AutoSize = true;
        this.btnMonth.Location = new System.Drawing.Point(200, 50);
        this.btnMonth.Name = "btnMonth";
        this.btnMonth.Size = new System.Drawing.Size(55, 17);
        this.btnMonth.TabIndex = 25;
        this.btnMonth.Text = "Month";
        this.btnMonth.UseVisualStyleBackColor = true;
        this.btnMonth.Click += new System.EventHandler(this.btn_Click);
        // 
        // btnWeek
        // 
        this.btnWeek.AutoSize = true;
        this.btnWeek.Location = new System.Drawing.Point(200, 26);
        this.btnWeek.Name = "btnWeek";
        this.btnWeek.Size = new System.Drawing.Size(54, 17);
        this.btnWeek.TabIndex = 24;
        this.btnWeek.Text = "Week";
        this.btnWeek.UseVisualStyleBackColor = true;
        this.btnWeek.Click += new System.EventHandler(this.btn_Click);
        // 
        // btnDay
        // 
        this.btnDay.AutoSize = true;
        this.btnDay.Location = new System.Drawing.Point(200, 3);
        this.btnDay.Name = "btnDay";
        this.btnDay.Size = new System.Drawing.Size(44, 17);
        this.btnDay.TabIndex = 23;
        this.btnDay.Text = "Day";
        this.btnDay.UseVisualStyleBackColor = true;
        this.btnDay.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn60
        // 
        this.btn60.AutoSize = true;
        this.btn60.Location = new System.Drawing.Point(105, 165);
        this.btn60.Name = "btn60";
        this.btn60.Size = new System.Drawing.Size(77, 17);
        this.btn60.TabIndex = 22;
        this.btn60.Text = "60 Minutes";
        this.btn60.UseVisualStyleBackColor = true;
        this.btn60.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn30s
        // 
        this.btn30s.AutoSize = true;
        this.btn30s.Location = new System.Drawing.Point(3, 165);
        this.btn30s.Name = "btn30s";
        this.btn30s.Size = new System.Drawing.Size(80, 17);
        this.btn30s.TabIndex = 36;
        this.btn30s.Text = "30 seconds";
        this.btn30s.UseVisualStyleBackColor = true;
        this.btn30s.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn20s
        // 
        this.btn20s.AutoSize = true;
        this.btn20s.Location = new System.Drawing.Point(3, 142);
        this.btn20s.Name = "btn20s";
        this.btn20s.Size = new System.Drawing.Size(80, 17);
        this.btn20s.TabIndex = 35;
        this.btn20s.Text = "20 seconds";
        this.btn20s.UseVisualStyleBackColor = true;
        this.btn20s.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn15s
        // 
        this.btn15s.AutoSize = true;
        this.btn15s.Location = new System.Drawing.Point(3, 119);
        this.btn15s.Name = "btn15s";
        this.btn15s.Size = new System.Drawing.Size(80, 17);
        this.btn15s.TabIndex = 34;
        this.btn15s.Text = "15 seconds";
        this.btn15s.UseVisualStyleBackColor = true;
        this.btn15s.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn10s
        // 
        this.btn10s.AutoSize = true;
        this.btn10s.Location = new System.Drawing.Point(3, 96);
        this.btn10s.Name = "btn10s";
        this.btn10s.Size = new System.Drawing.Size(80, 17);
        this.btn10s.TabIndex = 33;
        this.btn10s.Text = "10 seconds";
        this.btn10s.UseVisualStyleBackColor = true;
        this.btn10s.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn5s
        // 
        this.btn5s.AutoSize = true;
        this.btn5s.Location = new System.Drawing.Point(3, 73);
        this.btn5s.Name = "btn5s";
        this.btn5s.Size = new System.Drawing.Size(74, 17);
        this.btn5s.TabIndex = 32;
        this.btn5s.Text = "5 seconds";
        this.btn5s.UseVisualStyleBackColor = true;
        this.btn5s.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn3s
        // 
        this.btn3s.AutoSize = true;
        this.btn3s.Location = new System.Drawing.Point(3, 50);
        this.btn3s.Name = "btn3s";
        this.btn3s.Size = new System.Drawing.Size(74, 17);
        this.btn3s.TabIndex = 31;
        this.btn3s.Text = "3 seconds";
        this.btn3s.UseVisualStyleBackColor = true;
        this.btn3s.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn2s
        // 
        this.btn2s.AutoSize = true;
        this.btn2s.Location = new System.Drawing.Point(3, 27);
        this.btn2s.Name = "btn2s";
        this.btn2s.Size = new System.Drawing.Size(74, 17);
        this.btn2s.TabIndex = 30;
        this.btn2s.Text = "2 seconds";
        this.btn2s.UseVisualStyleBackColor = true;
        this.btn2s.Click += new System.EventHandler(this.btn_Click);
        // 
        // btn1s
        // 
        this.btn1s.AutoSize = true;
        this.btn1s.Checked = true;
        this.btn1s.Location = new System.Drawing.Point(3, 3);
        this.btn1s.Name = "btn1s";
        this.btn1s.Size = new System.Drawing.Size(69, 17);
        this.btn1s.TabIndex = 29;
        this.btn1s.TabStop = true;
        this.btn1s.Text = "1 second";
        this.btn1s.UseVisualStyleBackColor = true;
        this.btn1s.Click += new System.EventHandler(this.btn_Click);
        // 
        // TimeIntervalFormEditor
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.SystemColors.Control;
        this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.Controls.Add(this.btn30s);
        this.Controls.Add(this.btn20s);
        this.Controls.Add(this.btn15s);
        this.Controls.Add(this.btn10s);
        this.Controls.Add(this.btn5s);
        this.Controls.Add(this.btn3s);
        this.Controls.Add(this.btn2s);
        this.Controls.Add(this.btn1s);
        this.Controls.Add(this.btn60);
        this.Controls.Add(this.numCustom);
        this.Controls.Add(this.btnCustom);
        this.Controls.Add(this.btn30);
        this.Controls.Add(this.btn15);
        this.Controls.Add(this.btn10);
        this.Controls.Add(this.btn5);
        this.Controls.Add(this.btn3);
        this.Controls.Add(this.btn2);
        this.Controls.Add(this.btn1);
        this.Controls.Add(this.btnYear);
        this.Controls.Add(this.btnMonth);
        this.Controls.Add(this.btnWeek);
        this.Controls.Add(this.btnDay);
        this.Name = "TimeIntervalFormEditor";
        this.Size = new System.Drawing.Size(276, 232);
        ((System.ComponentModel.ISupportInitialize)(this.numCustom)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.NumericUpDown numCustom;
      private System.Windows.Forms.RadioButton btnCustom;
      private System.Windows.Forms.RadioButton btn30;
      private System.Windows.Forms.RadioButton btn15;
      private System.Windows.Forms.RadioButton btn10;
      private System.Windows.Forms.RadioButton btn5;
      private System.Windows.Forms.RadioButton btn3;
      private System.Windows.Forms.RadioButton btn2;
      private System.Windows.Forms.RadioButton btn1;
      private System.Windows.Forms.RadioButton btnYear;
      private System.Windows.Forms.RadioButton btnMonth;
      private System.Windows.Forms.RadioButton btnWeek;
      private System.Windows.Forms.RadioButton btnDay;
      private System.Windows.Forms.RadioButton btn60;
      private System.Windows.Forms.RadioButton btn30s;
      private System.Windows.Forms.RadioButton btn20s;
      private System.Windows.Forms.RadioButton btn15s;
      private System.Windows.Forms.RadioButton btn10s;
      private System.Windows.Forms.RadioButton btn5s;
      private System.Windows.Forms.RadioButton btn3s;
      private System.Windows.Forms.RadioButton btn2s;
      private System.Windows.Forms.RadioButton btn1s;
    }
//  }
}
