namespace Keyence_Device
{
    partial class Detail_1_RFC
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
            this.Data_Pocket = new System.Windows.Forms.DataGrid();
            this.txt_infor = new System.Windows.Forms.TextBox();
            this.btn_Return = new System.Windows.Forms.Button();
            this.btn_new = new System.Windows.Forms.Button();
            this.btn_confirm = new System.Windows.Forms.Button();
            this.lab_ng = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lab_ok = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.SuspendLayout();
            // 
            // Data_Pocket
            // 
            this.Data_Pocket.BackColor = System.Drawing.Color.White;
            this.Data_Pocket.BackgroundColor = System.Drawing.Color.White;
            this.Data_Pocket.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.Data_Pocket.HeaderBackColor = System.Drawing.Color.Aqua;
            this.Data_Pocket.Location = new System.Drawing.Point(4, 4);
            this.Data_Pocket.Name = "Data_Pocket";
            this.Data_Pocket.RowHeadersVisible = false;
            this.Data_Pocket.Size = new System.Drawing.Size(224, 117);
            this.Data_Pocket.TabIndex = 0;
            this.Data_Pocket.TableStyles.Add(this.dataGridTableStyle1);
            // 
            // txt_infor
            // 
            this.txt_infor.BackColor = System.Drawing.Color.White;
            this.txt_infor.Enabled = false;
            this.txt_infor.ForeColor = System.Drawing.Color.Black;
            this.txt_infor.Location = new System.Drawing.Point(4, 160);
            this.txt_infor.Multiline = true;
            this.txt_infor.Name = "txt_infor";
            this.txt_infor.Size = new System.Drawing.Size(224, 89);
            this.txt_infor.TabIndex = 8;
            // 
            // btn_Return
            // 
            this.btn_Return.BackColor = System.Drawing.Color.Aqua;
            this.btn_Return.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Return.Location = new System.Drawing.Point(4, 260);
            this.btn_Return.Name = "btn_Return";
            this.btn_Return.Size = new System.Drawing.Size(72, 20);
            this.btn_Return.TabIndex = 9;
            this.btn_Return.Text = "Thoát";
            this.btn_Return.Click += new System.EventHandler(this.btn_Return_Click);
            // 
            // btn_new
            // 
            this.btn_new.BackColor = System.Drawing.Color.Aqua;
            this.btn_new.Enabled = false;
            this.btn_new.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btn_new.Location = new System.Drawing.Point(82, 260);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(72, 20);
            this.btn_new.TabIndex = 14;
            this.btn_new.Text = "Làm mới";
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_confirm
            // 
            this.btn_confirm.BackColor = System.Drawing.Color.Aqua;
            this.btn_confirm.Enabled = false;
            this.btn_confirm.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btn_confirm.Location = new System.Drawing.Point(163, 260);
            this.btn_confirm.Name = "btn_confirm";
            this.btn_confirm.Size = new System.Drawing.Size(72, 20);
            this.btn_confirm.TabIndex = 15;
            this.btn_confirm.Text = "Xác nhận";
            this.btn_confirm.Click += new System.EventHandler(this.btn_confirm_Click);
            // 
            // lab_ng
            // 
            this.lab_ng.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lab_ng.ForeColor = System.Drawing.Color.Red;
            this.lab_ng.Location = new System.Drawing.Point(153, 132);
            this.lab_ng.Name = "lab_ng";
            this.lab_ng.Size = new System.Drawing.Size(44, 20);
            this.lab_ng.Text = "0";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(112, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 20);
            this.label3.Text = "NG :";
            // 
            // lab_ok
            // 
            this.lab_ok.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lab_ok.ForeColor = System.Drawing.Color.Green;
            this.lab_ok.Location = new System.Drawing.Point(41, 132);
            this.lab_ok.Name = "lab_ok";
            this.lab_ok.Size = new System.Drawing.Size(44, 20);
            this.lab_ok.Text = "0";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(4, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.Text = "OK :";
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn2);
            this.dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn3);
            this.dataGridTableStyle1.MappingName = "Details";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Mã";
            this.dataGridTextBoxColumn1.MappingName = "Ma";
            this.dataGridTextBoxColumn1.Width = 40;
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Nội Dung";
            this.dataGridTextBoxColumn2.MappingName = "Content";
            this.dataGridTextBoxColumn2.Width = 118;
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            this.dataGridTextBoxColumn3.HeaderText = "Kết Quả";
            this.dataGridTextBoxColumn3.MappingName = "Result";
            this.dataGridTextBoxColumn3.Width = 60;
            // 
            // Detail_1_RFC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.lab_ng);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lab_ok);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_confirm);
            this.Controls.Add(this.btn_new);
            this.Controls.Add(this.btn_Return);
            this.Controls.Add(this.txt_infor);
            this.Controls.Add(this.Data_Pocket);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Detail_1_RFC";
            this.Load += new System.EventHandler(this.Detail_1_RFC_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid Data_Pocket;
        private System.Windows.Forms.TextBox txt_infor;
        private System.Windows.Forms.Button btn_Return;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Button btn_confirm;
        private System.Windows.Forms.Label lab_ng;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lab_ok;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
    }
}