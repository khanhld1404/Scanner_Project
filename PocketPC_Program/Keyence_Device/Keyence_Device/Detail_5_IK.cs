using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using Keyence_Device.Class;
namespace Keyence_Device
{
    public partial class Detail_5_IK : Form
    {
        // Biến lưu mã nhân viên sử dụng
        private string user_code;

        public Detail_5_IK(string tt)
        {
            InitializeComponent();
            this.KeyPreview = true;
            user_code = tt;
        }

        private void Detail_2_Load(object sender, EventArgs e)
        {
            // Nạp dữ liệu mẫu
            var dt = new DataTable("Details");
            dt.Columns.Add("Ma", typeof(string));
            dt.Columns.Add("Content", typeof(string));
            dt.Columns.Add("Result", typeof(string));
            Data_Pocket.DataSource = dt;
        }

        private void btn_Return_Click(object sender, EventArgs e)
        {
            using (var cf = new ConfirmForm("Xác nhận quay lại", "Bạn có chắc muốn thoát!"))
            {
                if (cf.ShowDialog() == DialogResult.OK)
                {
                    // TẮT NHẬN BARCODE NGAY

                    _isClosing = true;                 // <-- quan trọng
                    this.KeyPreview = false;


                    if (_scanTimeoutTimer != null)
                    {
                        _scanTimeoutTimer.Enabled = false;   // <- thay cho Stop()
                        _scanTimeoutTimer.Tick -= ScanTick;  // tháo handler
                        _scanTimeoutTimer.Dispose();         // giải phóng
                        _scanTimeoutTimer = null;
                    }

                    _scanBuffer.Length = 0;

                    InputGuard.DrainInputQueue();
                    InputGuard.SuppressForMs(600);

                    this.Close();
                    if (master_check == true && tmp_master != "")
                    {
                        // Lưu thông tin vào file 
                        SaveData.Save(user_code, "IK", an_infor, ok_count, ng_count);
                    }
                }
            }
        }

        // Xử lý việc quét mã 
        //Xử lý việc quét mã vạch

        // Field của form
        private bool _isClosing = false;
        private StringBuilder _scanBuffer = new StringBuilder(128); //Biến để nhận giá trị quét mã
        private Timer _scanTimeoutTimer;
        private const int SCAN_TIMEOUT_MS = 50;    // khoảng trống > 50–100ms coi như xong một lần quét
        //Biến để biết có đọc lại master không
        private bool master_check = true;

        // Các giá được tách ra từ mã vạch
        //Biến chứa thông tin master mã
        string lot, pi, cd, an, qr_infor;
        //Biến chứa thông tin mã cần kiểm tra
        string lot_product, pi_product, cd_product, an_product;
        //biến lưu kết quả kiểm tra
        string lot_result, pi_result, cd_result, an_result;
        //biến để lưu các thông tin biến ở trên sau khi được xử lý
        string pi_infor, an_infor;
        //Biến đêm số lượng ok và ng
        int ok_count = 0;
        int ng_count = 0;

        // Biến để lưu giá trị master code
        string tmp_master = "";

        //Xử lý quét mã
        // Đổi Tick handler sang method đặt tên để có thể tháo gỡ
        private void EnsureScanTimer()
        {
            if (_scanTimeoutTimer == null)
            {
                _scanTimeoutTimer = new Timer();
                _scanTimeoutTimer.Interval = SCAN_TIMEOUT_MS;
                _scanTimeoutTimer.Tick += ScanTick;   // <-- dùng handler riêng
            }
        }

        private void ScanTick(object s, EventArgs e)
        {
            if (_isClosing) return;                  // <-- đang đóng thì bỏ qua
            _scanTimeoutTimer.Enabled = false;
            if (_scanBuffer.Length > 0)
            {
                HandleScannedCode(_scanBuffer.ToString());
                _scanBuffer.Length = 0;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (_isClosing) { e.Handled = true; return; }  // <-- đang đóng thì nuốt phím

            base.OnKeyPress(e);
            EnsureScanTimer();

            char ch = e.KeyChar;
            if (ch == '\r' || ch == '\n' || ch == '\t')
            {
                _scanTimeoutTimer.Enabled = false;
                string code = _scanBuffer.ToString();
                _scanBuffer.Length = 0;

                if (code.Length > 0)
                {
                    e.Handled = true;
                    HandleScannedCode(code);
                }
                return;
            }

            if (!char.IsControl(ch))
            {
                _scanBuffer.Append(ch);
                _scanTimeoutTimer.Enabled = false;
                _scanTimeoutTimer.Enabled = true;
                e.Handled = true;
            }
        }

        //Hàm xử lý mã vạch sau khi đã được quét
        private void HandleScannedCode(string code)
        {
            // (Tùy bạn) lọc/trim
            code = code.Trim();

            //Thiết lập bảng hiển thị thông tin
            var dt = new DataTable("Details");
            dt.Columns.Add("Ma", typeof(string));
            dt.Columns.Add("Content", typeof(string));
            dt.Columns.Add("Result", typeof(string));

            // Kiểm tra thông tin master để substring không bị lỗi
            if (code.Length <= 22 || code.Length >= 32)
            {
                txt_infor.Text = "Mã vạch: " + code + @" không hợp lệ. 
Hãy nhập mã vạch khác!";
                Beeper.Error();
                txt_infor.ForeColor = System.Drawing.Color.Orange;

                return;
            }

            if (master_check)
            {

                //Đọc dữ liệu qr master
                if (code.Contains("("))
                {

                    qr_infor = code;
                    pi = code.Substring(4, 1);
                    an = code.Substring(5, 12);
                    cd = code.Substring(17, 1);
                    lot = code.Substring(22);
                }
                else
                {
                    qr_infor = code;
                    pi = code.Substring(2, 1);
                    an = code.Substring(3, 12);
                    cd = code.Substring(15, 1);
                    lot = code.Substring(18);
                }

                // Kiểm tra mã master có giống master cũ không
                if (an != tmp_master && tmp_master != "")
                {
                    // Lưu thông tin vào file 
                    SaveData.Save(user_code, "IK", an_infor, ok_count, ng_count);

                    ok_count = 0;
                    ng_count = 0;
                    lab_ok.Text = ok_count.ToString();
                    lab_ng.Text = ng_count.ToString();
                }
                // Kiểm tra mã vạch có trong master không
                if (!check_QR(an))
                {
                    txt_infor.Text = @"Nhãn master chưa được đăng ký!. 
Hãy nhập nhãn master khác!";
                    Beeper.Error();
                    txt_infor.ForeColor = System.Drawing.Color.Orange;

                    tmp_master = "";
                    return;
                }
                //
                tmp_master = an;
                //Xử lý thông tin pi
                if (pi == "0")
                {
                    pi_infor = "Túi Đóng Gói";
                }
                else if (pi == "3")
                {
                    pi_infor = "Hộp Đơn Vị";
                }
                else if (pi == "5")
                {
                    pi_infor = "Thùng Xuất Xưởng";
                }
                else
                {
                    pi_infor = pi;
                }

                // Xử lý thông tin an
                an_infor = GetInfor(an);

                //Đưa kết quả lên bảng
                dt.Rows.Add("PI", pi_infor, "OK");
                dt.Rows.Add("AN", an_infor, "OK");
                dt.Rows.Add("CD", cd, "OK");
                dt.Rows.Add("LOT", lot, "OK");
                Data_Pocket.DataSource = dt;

                txt_infor.Text = @"Nhãn Master đã được đăng ký!
Thực hiện đọc nhãn cần kiểm tra.
                ";
                Beeper.Success();
                txt_infor.ForeColor = System.Drawing.Color.Black;

                master_check = false;
            }
            else
            {
                // Đọc các giá  trị quét qr sản phẩm
                if (code.Contains("("))
                {

                    pi_product = code.Substring(4, 1);
                    an_product = code.Substring(5, 12);
                    cd_product = code.Substring(17, 1);
                    lot_product = code.Substring(22);
                }
                else
                {
                    pi_product = code.Substring(2, 1);
                    an_product = code.Substring(3, 12);
                    cd_product = code.Substring(15, 1);
                    lot_product = code.Substring(18);
                }

                //Đưa bảng về rỗng
                Data_Pocket.DataSource = null;

                //Đọc lại nhãn master
                master_check = true;

                if (an_product == an && lot_product == lot)
                {
                    txt_infor.Text = @"Mã vạch khớp với nhãn master!
Hãy đọc lại nhãn master
                    ";
                    Beeper.Success();
                    txt_infor.ForeColor = System.Drawing.Color.Lime;

                    // Cộng một vào giá trị Okkk
                    ok_count += 1;
                    lab_ok.Text = ok_count.ToString();
                }
                else
                {
                    txt_infor.Text = @"Mã vạch không khớp với nhãn master! 
Hãy đọc lại nhãn master";
                    Beeper.Error();
                    txt_infor.ForeColor = System.Drawing.Color.Red;
                    ng_count += 1;
                    lab_ng.Text = ng_count.ToString();
                }

                //Kiểm tra và so sánh mã sản phẩm cần quét và master
                if (pi_product == pi)
                {
                    pi_result = " OK ";
                }
                else
                {
                    pi_result = " NG ";
                }

                if (an_product == an)
                {
                    an_result = " OK ";
                }
                else
                {
                    an_result = " NG ";
                }

                if (cd_product == cd)
                {
                    cd_result = " OK ";
                }
                else
                {
                    cd_result = " NG ";
                }


                if (lot_product == lot)
                {
                    lot_result = " OK ";
                }
                else
                {
                    lot_result = " NG ";
                }

                //Đưa kết quả lên bảng
                dt.Rows.Add("PI", pi_infor, pi_result);
                dt.Rows.Add("AN", an_infor, an_result);
                dt.Rows.Add("CD", cd, cd_result);
                dt.Rows.Add("LOT", lot, lot_result);
                Data_Pocket.DataSource = dt;
            }
        }

        //Hàm kiểm tra mã có trong master không
        private bool check_QR(string qr)
        {
            try
            {
                using (var conn = new SqlCeConnection(DbConfig._connect))
                {
                    conn.Open();
                    string sql = @"SELECT CASE WHEN EXISTS 
                         (SELECT 1 FROM IK WHERE an = @qr) 
                        THEN 1 ELSE 0 END";
                    using (var cmd = conn.CreateCommand())
                    {
                        // delete theo QR
                        cmd.CommandText = sql;
                        var p = cmd.Parameters.Add("@qr", SqlDbType.NVarChar);
                        p.Value = qr;
                        object scalar = cmd.ExecuteScalar();
                        int result = Convert.ToInt32(scalar);
                        if (result == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                };
            }
            catch (SqlCeException ex)
            {
                // TODO: log ex (File/EventLog/Telemetry). Tránh MessageBox ở đây nếu là tầng DAL.
                MessageBox.Show(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                // TODO: log ex
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        // Hàm lấy thông tin product từ an
        private string GetInfor(string tt)
        {
            try
            {
                using (var conn = new SqlCeConnection(DbConfig._connect))
                {
                    conn.Open();
                    string sql = @"
                            SELECT product
                            FROM IK
                            WHERE an = @tt";
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        var p = cmd.Parameters.Add("@tt", SqlDbType.NVarChar);
                        p.Value = tt.Trim();

                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                            return null;

                        // Ép kiểu an toàn
                        return Convert.ToString(result);
                    }
                }
            }
            catch (SqlCeException ex)
            {
                // TODO: log ex (File/EventLog/Telemetry). Tránh MessageBox ở đây nếu là tầng DAL.
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                // TODO: log ex
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}