namespace Keyence_Device
{
    partial class Login
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
            this.txt_ma = new System.Windows.Forms.TextBox();
            this.txt_mk = new System.Windows.Forms.TextBox();
            this.btn_Enter = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.Text = "Mã nhân viên :";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.Text = "Mật khẩu :";
            // 
            // txt_ma
            // 
            this.txt_ma.Location = new System.Drawing.Point(24, 84);
            this.txt_ma.Name = "txt_ma";
            this.txt_ma.Size = new System.Drawing.Size(174, 23);
            this.txt_ma.TabIndex = 2;
            this.txt_ma.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_ma_KeyDown);
            // 
            // txt_mk
            // 
            this.txt_mk.Location = new System.Drawing.Point(24, 156);
            this.txt_mk.Name = "txt_mk";
            this.txt_mk.Size = new System.Drawing.Size(174, 23);
            this.txt_mk.TabIndex = 3;
            this.txt_mk.PasswordChar = '*';
            this.txt_mk.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_mk_KeyDown);
            // 
            // btn_Enter
            // 
            this.btn_Enter.Location = new System.Drawing.Point(118, 207);
            this.btn_Enter.Name = "btn_Enter";
            this.btn_Enter.Size = new System.Drawing.Size(80, 28);
            this.btn_Enter.TabIndex = 4;
            this.btn_Enter.Text = "Đăng Nhập";
            this.btn_Enter.Click += new System.EventHandler(this.btn_Enter_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(24, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 35);
            this.label3.Text = "Đăng Nhập Tài Khoản";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_Enter);
            this.Controls.Add(this.txt_mk);
            this.Controls.Add(this.txt_ma);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Login";
            this.Text = "Đăng nhập";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_ma;
        private System.Windows.Forms.TextBox txt_mk;
        private System.Windows.Forms.Button btn_Enter;
        private System.Windows.Forms.Label label3;
    }
}