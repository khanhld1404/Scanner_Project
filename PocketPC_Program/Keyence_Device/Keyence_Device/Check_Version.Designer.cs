namespace Keyence_Device
{
    partial class Check_Version
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
            this.lab_tb = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lab_tb
            // 
            this.lab_tb.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lab_tb.ForeColor = System.Drawing.Color.Red;
            this.lab_tb.Location = new System.Drawing.Point(42, 14);
            this.lab_tb.Name = "lab_tb";
            this.lab_tb.Size = new System.Drawing.Size(178, 20);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(53, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 20);
            this.label2.Text = "Cập nhật ngay!";
            // 
            // Check_Version
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lab_tb);
            this.Name = "Check_Version";
            this.Text = "Cập nhật version";
            this.Load += new System.EventHandler(this.Check_Version_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lab_tb;
        private System.Windows.Forms.Label label2;
    }
}