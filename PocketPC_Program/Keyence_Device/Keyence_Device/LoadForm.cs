using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Keyence_Device
{
    public partial class LoadForm : Form
    {
        
        private readonly ProgressBar bar;
        private readonly Timer tmr;
        private int dir = 1;
        // hàm xử lý giao diện khi load
        public LoadForm(string tt)
        {
            InitializeComponent();
            txt_load.Text = tt;
            // ProgressBar (không có Marquee trong CF)
            bar = new ProgressBar
            {
                Height = 22,
                Width = 215,
                Left = 3,
                Top = 35
            };

            // Khởi tạo giá trị để mô phỏng “indeterminate”
            bar.Minimum = 0;
            bar.Maximum = 100;
            bar.Value = 0;

            // Timer để “quét” qua lại
            tmr = new Timer();
            tmr.Interval = 50; // ms, tùy chỉnh cho mượt
            tmr.Tick += Tmr_Tick;

            this.Controls.Add(bar);

            this.Load += LoadingForm_Load;       // thay cho OnShown
            this.Closed += LoadingForm_Closed;   // thay cho OnFormClosed

        }



        private void LoadingForm_Load(object sender, EventArgs e)
        {
            if (tmr != null) tmr.Enabled = true;  // thay cho Start()
        }

        private void LoadingForm_Closed(object sender, EventArgs e)
        {
            try
            {
                if (tmr != null)
                {
                    tmr.Enabled = false;       // thay cho Stop()
                    tmr.Tick -= Tmr_Tick;
                    tmr.Dispose();
                    //tmr = null;
                }
            }
            catch { /* ignore an toàn trên CF */ }
        }


        // === Đây là hàm bạn hỏi “nằm ở đâu”: chính là handler của Timer.Tick ===
        private void Tmr_Tick(object sender, EventArgs e)
        {
            try
            {
                // Tăng/giảm giá trị để tạo cảm giác “đang chạy”
                int step = 5;
                int next = bar.Value + dir * step;

                if (next >= bar.Maximum)
                {
                    next = bar.Maximum;
                    dir = -1;
                }
                else if (next <= bar.Minimum)
                {
                    next = bar.Minimum;
                    dir = 1;
                }

                bar.Value = next;
            }
            catch
            {
                // Một số thiết bị CF có thể ném lỗi nếu Value vượt ngưỡng
                if (bar.Value > bar.Maximum) bar.Value = bar.Maximum;
                if (bar.Value < bar.Minimum) bar.Value = bar.Minimum;
            }

        }
    }
}