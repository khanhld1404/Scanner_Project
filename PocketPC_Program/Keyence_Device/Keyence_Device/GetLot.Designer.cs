namespace Keyence_Device
{
    partial class GetLot
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
            this.txt_Wo = new System.Windows.Forms.TextBox();
            this.btn_End = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 20);
            this.label1.Text = "Mời bạn quét mã lô :";
            // 
            // txt_Wo
            // 
            this.txt_Wo.Enabled = false;
            this.txt_Wo.Location = new System.Drawing.Point(4, 42);
            this.txt_Wo.Name = "txt_Wo";
            this.txt_Wo.Size = new System.Drawing.Size(210, 23);
            this.txt_Wo.TabIndex = 1;
            // 
            // btn_End
            // 
            this.btn_End.Location = new System.Drawing.Point(127, 71);
            this.btn_End.Name = "btn_End";
            this.btn_End.Size = new System.Drawing.Size(87, 32);
            this.btn_End.TabIndex = 3;
            this.btn_End.Text = "Thoát";
            this.btn_End.Click += new System.EventHandler(this.btn_End_Click);
            // 
            // GetLot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(238, 95);
            this.Controls.Add(this.btn_End);
            this.Controls.Add(this.txt_Wo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 50);
            this.Name = "GetLot";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Wo;
        private System.Windows.Forms.Button btn_End;
    }
}