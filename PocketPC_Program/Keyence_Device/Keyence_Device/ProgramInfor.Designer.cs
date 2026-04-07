namespace Keyence_Device
{
    partial class ProgramInfor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Btn_Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(43, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 43);
            this.label1.Text = "Phần mềm quét mã vạch";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(3, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.Text = "Phiên bản : ";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(95, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.Text = "1";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(3, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 20);
            this.label4.Text = "Người phát triển :";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(127, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 20);
            this.label5.Text = "Khánh";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(3, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 20);
            this.label6.Text = "Số điện thoại liên hệ :";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(155, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 20);
            this.label7.Text = "274";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(3, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(189, 21);
            this.label8.Text = "Công ty Terumo Việt Nam";
            // 
            // Btn_Close
            // 
            this.Btn_Close.BackColor = System.Drawing.Color.Red;
            this.Btn_Close.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.Btn_Close.Location = new System.Drawing.Point(155, 178);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(72, 38);
            this.Btn_Close.TabIndex = 14;
            this.Btn_Close.Text = "Thoát";
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // ProgramInfor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(238, 217);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 40);
            this.Name = "ProgramInfor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button Btn_Close;
    }
}