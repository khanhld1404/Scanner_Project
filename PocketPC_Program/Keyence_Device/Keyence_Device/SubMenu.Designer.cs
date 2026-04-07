namespace Keyence_Device
{
    partial class SubMenu
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
            this.Btn_Close = new System.Windows.Forms.Button();
            this.Btn_Logout = new System.Windows.Forms.Button();
            this.Btn_Infor = new Keyence_Device.MultilineButton();
            this.Btn_Update = new Keyence_Device.MultilineButton();
            this.SuspendLayout();
            // 
            // Btn_Close
            // 
            this.Btn_Close.BackColor = System.Drawing.Color.Red;
            this.Btn_Close.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.Btn_Close.Location = new System.Drawing.Point(128, 85);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(91, 47);
            this.Btn_Close.TabIndex = 3;
            this.Btn_Close.Text = "Đóng";
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Btn_Logout
            // 
            this.Btn_Logout.BackColor = System.Drawing.Color.Yellow;
            this.Btn_Logout.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.Btn_Logout.Location = new System.Drawing.Point(17, 85);
            this.Btn_Logout.Name = "Btn_Logout";
            this.Btn_Logout.Size = new System.Drawing.Size(91, 47);
            this.Btn_Logout.TabIndex = 4;
            this.Btn_Logout.Text = "LogOut";
            this.Btn_Logout.Click += new System.EventHandler(this.Btn_Logout_Click);
            // 
            // Btn_Infor
            // 
            this.Btn_Infor.BackColor = System.Drawing.Color.Lime;
            this.Btn_Infor.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.Btn_Infor.Location = new System.Drawing.Point(128, 16);
            this.Btn_Infor.Name = "Btn_Infor";
            this.Btn_Infor.Size = new System.Drawing.Size(91, 52);
            this.Btn_Infor.TabIndex = 2;
            this.Btn_Infor.Text = "Thông tin phần mềm";
            this.Btn_Infor.Click += new System.EventHandler(this.Btn_Infor_Click);
            // 
            // Btn_Update
            // 
            this.Btn_Update.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Btn_Update.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.Btn_Update.Location = new System.Drawing.Point(17, 16);
            this.Btn_Update.Name = "Btn_Update";
            this.Btn_Update.Size = new System.Drawing.Size(91, 52);
            this.Btn_Update.TabIndex = 0;
            this.Btn_Update.Text = "Cập nhật cơ sở dữ liệu";
            this.Btn_Update.Click += new System.EventHandler(this.Btn_Update_Click);
            // 
            // SubMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 135);
            this.Controls.Add(this.Btn_Logout);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.Btn_Infor);
            this.Controls.Add(this.Btn_Update);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 50);
            this.Name = "SubMenu";
            this.ResumeLayout(false);

        }

        #endregion

        private MultilineButton Btn_Update;
        private MultilineButton Btn_Infor;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.Button Btn_Logout;
    }
}