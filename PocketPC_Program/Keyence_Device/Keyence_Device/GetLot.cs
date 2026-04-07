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
    public partial class GetLot : Form
    {
        // Biến lưu thông tin lot
        private string lot;
        public GetLot(string tt)
        {
            InitializeComponent();
            txt_Wo.Focus();
            lot = tt;
        }

        //Xử lý việc quét mã vạch

        // Field của form
        private StringBuilder _scanBuffer = new StringBuilder(128); //Biến để nhận giá trị quét mã
        private Timer _scanTimeoutTimer;
        private const int SCAN_TIMEOUT_MS = 50;    // khoảng trống > 50–100ms coi như xong một lần quét
        
        // Kiểm tra thời gian quét mã đến lúc nhận được thông tin để xử lý
        private void EnsureScanTimer()
        {
            if (_scanTimeoutTimer == null)
            {
                _scanTimeoutTimer = new Timer();
                _scanTimeoutTimer.Interval = SCAN_TIMEOUT_MS;
                _scanTimeoutTimer.Tick += (s, e) =>
                {
                    _scanTimeoutTimer.Enabled = false;
                    if (_scanBuffer.Length > 0)
                    {
                        HandleScannedCode(_scanBuffer.ToString());
                        _scanBuffer.Length = 0; // clear
                    }
                };
            }
        }

        //Thông thường khi quét mã vạch ở cuối sẽ có thêm một nút enter, hàm dưới xử lý khi nhấn enter thì sẽ nhận 
        //thông tin và cho nó một khoảng thời gian để xem có nhận được bất kỳ giá trị nào nữa không
        //Không thì kết thúc
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            EnsureScanTimer();

            char ch = e.KeyChar;

            // 1) Nếu gặp Enter/Tab ⇒ kết thúc ngay lần quét
            if (ch == '\r' || ch == '\n' || ch == '\t')
            {
                _scanTimeoutTimer.Enabled = false;
                string code = _scanBuffer.ToString();
                _scanBuffer.Length = 0;

                if (code.Length > 0)
                {
                    // Ngăn Enter "lọt" xuống các control khác
                    e.Handled = true;
                    HandleScannedCode(code);
                }
                return;
            }

            // 2) Ký tự in được: thêm vào buffer
            if (!char.IsControl(ch))
            {
                _scanBuffer.Append(ch);
                // Gia hạn timer: nếu trong ~80ms không có ký tự mới, coi như xong một lần quét
                _scanTimeoutTimer.Enabled = false;
                _scanTimeoutTimer.Enabled = true;
                // Không cho ký tự đi tiếp vào control đang focus
                e.Handled = true;
            }
        }

        //Hàm xử lý mã vạch sau khi đã được quét
        private void HandleScannedCode(string code)
        {
            // (Tùy bạn) lọc/trim
            code = code.Trim();
            txt_Wo.Text = code;
            if (code == "1" || code.Trim() == lot.Trim())
            {
                this.DialogResult = DialogResult.OK;
            }
            else { 
                this.DialogResult = DialogResult.No;
            }
        }

        private void label1_ParentChanged(object sender, EventArgs e)
        {

        }
    }
}