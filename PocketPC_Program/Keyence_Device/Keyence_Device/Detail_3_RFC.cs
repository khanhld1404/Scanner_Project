using System;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Data.SqlServerCe;
using Keyence_Device.Class;
using System.Drawing;
namespace Keyence_Device
{
    public partial class Detail_3_RFC : Form
    {
        // Biến lưu mã nhân viên sử dụng
        private string user_code;

        public Detail_3_RFC(string tt1)
        {
            InitializeComponent();
            this.KeyPreview = true;
            user_code = tt1;
        }

        private void Data_Load()
        {
            // Nạp dữ liệu mẫu
            var dt = new DataTable("Details");
            dt.Columns.Add("Ma", typeof(string));
            dt.Columns.Add("Content", typeof(string));
            dt.Columns.Add("Result", typeof(string));
            txt_infor.Text = @"Hãy đọc nhãn master";
            txt_infor.ForeColor = System.Drawing.Color.Black;
            // vô hiệu hóa nút làm mới
            btn_new.Enabled = false;

            Data_Pocket.DataSource = dt;
            // Đưa các giá trị đếm về bằng 0
            ok_count = 0;
            ng_count = 0;
            lab_ok.Text = ok_count.ToString();
            lab_ng.Text = ng_count.ToString();
        }
        private void Detail_2_Load(object sender, EventArgs e)
        {
            Data_Load();
        }
        // Xử lý chức năng Thoát 
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

                    if (!master_check)
                        SaveData.Save(user_code, "RFC", an_infor, ok_count, ng_count);
                }
            }
        }



        //Xử lý việc quét mã vạch

        // Field của form
        private bool _isClosing = false;
        private StringBuilder _scanBuffer = new StringBuilder(128); //Biến để nhận giá trị quét mã
        private Timer _scanTimeoutTimer;
        private const int SCAN_TIMEOUT_MS = 100;    // khoảng trống > 50–100ms coi như xong một lần quét

        // Biến kiểm tra có cần quét lại mã master không
        public bool master_check = true;

        // Các giá được tách ra từ mã vạch
        //Biến chứa thông tin master mã
        string exp, lot, pi, cd, an, qr_infor;
        //Biến chứa thông tin mã cần kiểm tra
        string exp_product, lot_product, pi_product, cd_product, an_product;
        //biến lưu kết quả kiểm tra
        string exp_result, lot_result, pi_result, cd_result, an_result;
        //biến để lưu các thông tin biến ở trên sau khi được xử lý
        string pi_infor, exp_infor, an_infor;
        //biến để đếm số lượng ok và ng
        int ok_count = 0;
        int ng_count = 0;

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
            // mở lại nút làm mới
            btn_new.Enabled = true;

            //Thiết lập bảng hiển thị thông tin
            var dt = new DataTable("Details");
            dt.Columns.Add("Ma", typeof(string));
            dt.Columns.Add("Content", typeof(string));
            dt.Columns.Add("Result", typeof(string));

            // Kiểm tra mã để substrong không bị lỗi
            if (code.Length <= 32 || code.Length >= 36)
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
                qr_infor = code;
                pi = code.Substring(2, 1);
                an = code.Substring(3, 12);
                cd = code.Substring(15, 1);
                exp = code.Substring(18, 6);
                lot = code.Substring(26);

                // Kiểm tra mã vạch có trong master không
                if (!check_QR(an))
                {
                    txt_infor.Text = @"Nhãn master chưa được đăng ký!. 
Hãy nhập nhãn master khác!";
                    Beeper.Error();
                    txt_infor.ForeColor = System.Drawing.Color.Orange;

                    return;
                }

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

                // Xử lý thông tin exp
                string year = exp.Substring(0, 2);
                string month = exp.Substring(2, 2);
                string day = exp.Substring(4, 2);
                exp_infor = "20" + year + "-" + month + "-" + day;

                // Xử lý thông tin an
                an_infor = GetInfor(an);

                //Đưa kết quả lên bảng
                dt.Rows.Add("PI", pi_infor, "OK");
                dt.Rows.Add("AN", an_infor, "OK");
                dt.Rows.Add("CD", cd, "OK");
                dt.Rows.Add("EXP", exp_infor, "OK");
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
                pi_product = code.Substring(2, 1);
                an_product = code.Substring(3, 12);
                cd_product = code.Substring(15, 1);
                exp_product = code.Substring(18, 6);
                lot_product = code.Substring(26);

                //Đưa bảng về rỗng
                Data_Pocket.DataSource = null;

                if (an_product == an && pi_product == pi && lot_product == lot)
                {
                    txt_infor.Text = @"Mã vạch khớp với nhãn master!
Hãy tiếp tục đọc nhãn để kiểm tra
                    ";
                    Beeper.Success2();
                    normal_status();
                    txt_infor.ForeColor = System.Drawing.Color.Lime;

                    // Cộng một vào giá trị Okkk
                    ok_count += 1;
                    lab_ok.Text = ok_count.ToString();
                }
                else
                {
                    txt_infor.Text = @"Mã vạch không khớp với nhãn master! 
Hãy tiếp tục đọc nhãn để kiểm tra
kjnvisjnv";
                    ng_count += 1;
                    Beeper.Error();
                    error_status();
                    lab_ng.Text = ng_count.ToString();
                }

                //Kiểm tra và so sánh mã sản phẩm cần quét và master
                if (pi_product == pi)
                {
                    pi_result = " = ";
                }
                else
                {
                    pi_result = " # ";
                }

                if (an_product == an)
                {
                    an_result = " = ";
                }
                else
                {
                    an_result = " # ";
                }

                if (cd_product == cd)
                {
                    cd_result = " = ";
                }
                else
                {
                    cd_result = " # ";
                }

                if (exp_product == exp)
                {
                    exp_result = " = ";
                }
                else
                {
                    exp_result = " # ";
                }

                if (lot_product == lot)
                {
                    lot_result = " = ";
                }
                else
                {
                    lot_result = " # ";
                }

                //Đưa kết quả lên bảng
                dt.Rows.Add("PI", pi_infor, pi_result);
                dt.Rows.Add("AN", an_infor, an_result);
                dt.Rows.Add("CD", cd, cd_result);
                dt.Rows.Add("EXP", exp_infor, exp_result);
                dt.Rows.Add("LOT", lot, lot_result);
                Data_Pocket.DataSource = dt;
            }
        }

        //Hàm kiểm tra id có trong master không
        private bool check_QR(string qr)
        {
            using (var conn = new SqlCeConnection(DbConfig._connect))
            {
                conn.Open();
                string sql = @"SELECT CASE WHEN EXISTS 
                         (SELECT 1 FROM RFC WHERE an = @qr) 
                        THEN 1 ELSE 0 END"; ;
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
                            FROM RFC
                            WHERE an = @tt;
                        "; 
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

        private void btn_new_Click(object sender, EventArgs e)
        {
            //Đọc lại nhãn master
            using (var kq = new ConfirmForm("Làm mới master", "Bạn có muốn đọc lại!"))
            {
                if (kq.ShowDialog() == DialogResult.OK && master_check == false)
                {
                    master_check = true;
                    // Lưu thông tin vào file 
                    SaveData.Save(user_code, "RFC", an_infor, ok_count, ng_count);
                }
                normal_status();
                Data_Load();
            }
        }

        // hàm để đổi màn nhằm đưa ra được thông tin là đang bình thường
        public void normal_status()
        { 
            // phần nhập
            txt_infor.BackColor = Color.White;
        }

        // hàm để đổi màn nhằm đưa ra được thông tin là đang sai
        public void error_status()
        {
            txt_infor.BackColor = Color.Red;
            txt_infor.ForeColor = Color.White;
        }
    }
}