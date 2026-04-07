namespace Keyence_Device
{
    partial class ConfirmForm
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
            this.lab_notify = new System.Windows.Forms.Label();
            this.btn_yes = new System.Windows.Forms.Button();
            this.btn_no = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lab_notify
            // 
            this.lab_notify.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lab_notify.Location = new System.Drawing.Point(3, 11);
            this.lab_notify.Name = "lab_notify";
            this.lab_notify.Size = new System.Drawing.Size(212, 20);
            this.lab_notify.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_yes
            // 
            this.btn_yes.BackColor = System.Drawing.Color.Red;
            this.btn_yes.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btn_yes.Location = new System.Drawing.Point(29, 38);
            this.btn_yes.Name = "btn_yes";
            this.btn_yes.Size = new System.Drawing.Size(72, 35);
            this.btn_yes.TabIndex = 1;
            this.btn_yes.Text = "Có";
            this.btn_yes.Click += new System.EventHandler(this.btn_yes_Click);
            // 
            // btn_no
            // 
            this.btn_no.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btn_no.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btn_no.Location = new System.Drawing.Point(123, 38);
            this.btn_no.Name = "btn_no";
            this.btn_no.Size = new System.Drawing.Size(72, 35);
            this.btn_no.TabIndex = 2;
            this.btn_no.Text = "Không";
            this.btn_no.Click += new System.EventHandler(this.btn_no_Click);
            // 
            // ConfirmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(218, 73);
            this.Controls.Add(this.btn_no);
            this.Controls.Add(this.btn_yes);
            this.Controls.Add(this.lab_notify);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(10, 100);
            this.Name = "ConfirmForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lab_notify;
        private System.Windows.Forms.Button btn_yes;
        private System.Windows.Forms.Button btn_no;
    }
}