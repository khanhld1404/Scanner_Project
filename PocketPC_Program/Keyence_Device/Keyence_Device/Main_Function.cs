using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;                 // ThreadPool (không dùng Timer ở đây)
using System.Data.SqlServerCe;
using Keyence_Device.Class;

namespace Keyence_Device
{
    public partial class Main_Function : Form
    {
        // ====== CHỐT LỖI “PHÍM RƠI” ======
        private System.Windows.Forms.Timer _debounceTimer; // Rõ namespace để tránh trùng
        private DateTime _suppressUntilUtc;                // thời điểm kết thúc suppress
        private Control _neutralFocus;                     // điểm focus trung tính

        // ====== THÔNG TIN TÀI KHOẢN ======
        private string department;
        private string user_code;

        // cho phép bắt control
        private bool AllowSelfSigned = true;


        // DEV ONLY: bỏ kiểm tra SSL cert (không dùng cho production)
        private class TrustAllPolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(
                ServicePoint srvPoint,
                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                WebRequest request,
                int certificateProblem)
            { return true; }
        }

        // ====== CTOR ======
        public Main_Function(string tt1, string tt2)
        {
            InitializeComponent();

           
            this.KeyPreview = true;// Cho phép Form bắt phím trước control con
            if (AllowSelfSigned)
                ServicePointManager.CertificatePolicy = new TrustAllPolicy();

            user_code = tt1;
            department = tt2;

            // Thêm phần text cho màn hình
            this.Text = "Màn Hình Chức Năng " + department;

            // 1) Điểm focus trung tính (không có Click)
            _neutralFocus = new Label { Width = 0, Height = 0, TabStop = true };
            this.Controls.Add(_neutralFocus);

            // 2) Timer debounce: trong khoảng suppress, liên tục xả queue phím
            _debounceTimer = new System.Windows.Forms.Timer();
            _debounceTimer.Interval = 50;               // CF: dùng Enabled thay Start/Stop
            _debounceTimer.Tick += (s, e) =>
            {
                if (DateTime.UtcNow < _suppressUntilUtc)
                {
                    InputGuard.DrainInputQueue();   // xả WM_KEYDOWN/WM_CHAR… liên tục
                }
                else
                {
                    _debounceTimer.Enabled = false;
                }
            };

            // 3) Mỗi khi Main được kích hoạt → đưa focus khỏi mọi Button
            this.Activated += (s, e) =>
            {
                if (_neutralFocus != null && !_neutralFocus.IsDisposed)
                    _neutralFocus.Focus();
            };

            // 4) Button 7 (Button chuẩn) không được phép nhận focus
            try
            {
                btn_fun_7.TabStop = false;              // tránh Enter kích 7
                btn_fun_7.KeyPress += (s, e) =>
                {
                    if (e.KeyChar == '\r' || e.KeyChar == '\n') e.Handled = true;
                };
            }
            catch { /* nếu control khác tên thì bỏ qua */ }

            // Khởi tạo db
            EnsureDatabase();
        }

         // ====== Khởi tạo DB nội bộ ======

        private static void EnsureDatabase()
        {
            try
            {
                string dir = Path.GetDirectoryName(DbConfig.DbPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (!File.Exists(DbConfig.DbPath))
                {
                    using (var engine = new SqlCeEngine(DbConfig.ConnStr))
                        engine.CreateDatabase();

                    using (var conn = new SqlCeConnection(DbConfig.ConnStr))
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"
                            CREATE TABLE ScanData(
                                User_code    NVARCHAR(50) NOT NULL,
                                Department   NVARCHAR(50) NOT NULL,
                                Master_code  NVARCHAR(100) NOT NULL,
                                OK_count     INT NOT NULL,
                                NG_count     INT NOT NULL,
                                DeviceNumber  NVARCHAR(50),
                                CreatedAt    DATETIME NOT NULL DEFAULT GETDATE()
                            );";

                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "CREATE INDEX IX_ScanData_MasterCode ON ScanData(Master_code);";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "CREATE INDEX IX_ScanData_CreatedAt ON ScanData(CreatedAt);";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SqlCeException ex)
            {
                MessageBox.Show("Lỗi khởi tạo DB: " + ex.Message + "\nPath: " + DbConfig.DbPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo DB (khác): " + ex.Message);
            }
        }

        // ====== HỖ TRỢ: Debounce sau khi form con đóng ======
        private void StartDebounce(int ms)
        {
            _suppressUntilUtc = DateTime.UtcNow.AddMilliseconds(ms);
            InputGuard.DrainInputQueue();   // xả 1 lần ngay
            _debounceTimer.Enabled = true;  // 50ms/lần xả tiếp (trong Tick nhớ gọi DrainInputQueue)
        }

        private void AfterChildClosed()
        {
            // Xả toàn bộ KEY + MOUSE và khóa 600ms
            InputGuard.DrainInputQueue();
            StartDebounce(600);                 // hàm này vẫn dùng _debounceTimer như trước

            if (_neutralFocus != null && !_neutralFocus.IsDisposed)
                _neutralFocus.Focus();
        }

        // ====== Nuốt ENTER/hotkey trong thời gian debounce ======
        protected override void OnKeyUp(KeyEventArgs e)
        {
            // Đang suppress → nuốt toàn bộ phím
            if (_debounceTimer != null && _debounceTimer.Enabled)
            {
                e.Handled = true;
                return;
            }

            // Không cho Enter làm gì ở Main
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                return;
            }

            base.OnKeyUp(e);

            // (Nếu cần map phím F1/F2/F3… thì làm ở đây)
            // if (e.KeyCode == Keys.F1) { btn_fun_1_Click(null, null); e.Handled = true; }
            // if (e.KeyCode == Keys.F2) { btn_fun_2_Click(null, null); e.Handled = true; }
            // if (e.KeyCode == Keys.F3) { btn_fun_3_Click(null, null); e.Handled = true; }
        }

        // =====================================================================
        // ========================== CÁC CHỨC NĂNG =============================
        // =====================================================================


        private bool IsSuppressedNow()
        {
            return _debounceTimer != null && _debounceTimer.Enabled;
        }

        private void btn_fun_7_Click(object sender, EventArgs e)
        {
            using (var cn = new SubMenu()) {
                DialogResult kq = cn.ShowDialog();
                if (kq == DialogResult.OK) {
                    Login log = new Login();
                    log.ShowDialog();
                    this.Close();
                }
            }

        }

        private void btn_fun_1_Click(object sender, EventArgs e)
        {
            if (IsSuppressedNow()) return;

            if (department == "IK")
            {
                using (var d1 = new Detail_1_IK(user_code))
                    d1.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else if (department == "RFC")
            {
                using (var d2 = new Detail_1_RFC(user_code))
                    d2.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else
            {
                MessageBox.Show("Bộ phận không có trong dữ liệu");
                return;
            }

            AfterChildClosed();   // CHỐT phím rơi + focus
        }

        private void btn_fun_2_Click(object sender, EventArgs e)
        {
            if (IsSuppressedNow()) return;

            if (department == "IK")
            {
                using (var d4 = new Detail_2_IK(user_code))
                    d4.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else if (department == "RFC")
            {
                using (var d5 = new Detail_2_RFC(user_code))
                    d5.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else
            {
                MessageBox.Show("Bộ phận không có trong dữ liệu");
                return;
            }

            AfterChildClosed();   // CHỐT phím rơi + focus
        }

        private void btn_fun_3_Click(object sender, EventArgs e)
        {
            if (IsSuppressedNow()) return;

            if (department == "IK")
            {
                using (var d1 = new Detail_3_IK(user_code))
                    d1.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else if (department == "RFC")
            {
                using (var d2 = new Detail_3_RFC(user_code))
                    d2.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else
            {
                MessageBox.Show("Bộ phận không có trong dữ liệu");
                return;
            }               // <-- CE: chỉ có ShowDialog() không tham số

            AfterChildClosed();   // CHỐT phím rơi + focus
        }

        private void btn_fun_4_Click(object sender, EventArgs e)
        {
            if (IsSuppressedNow()) return;

            if (department == "IK")
            {
                using (var d1 = new Detail_4_IK(user_code))
                    d1.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else if (department == "RFC")
            {
                using (var d2 = new Detail_4_RFC(user_code))
                    d2.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else
            {
                MessageBox.Show("Bộ phận không có trong dữ liệu");
                return;
            }

            AfterChildClosed();   // CHỐT phím rơi + focus
        }

        private void btn_fun_5_Click(object sender, EventArgs e)
        {
            if (IsSuppressedNow()) return;

            if (department == "IK")
            {
                using (var d4 = new Detail_5_IK(user_code))
                    d4.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else if (department == "RFC")
            {
                using (var d5 = new Detail_5_RFC(user_code))
                    d5.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else
            {
                MessageBox.Show("Bộ phận không có trong dữ liệu");
                return;
            }

            AfterChildClosed();   // CHỐT phím rơi + focus
        }

        private void btn_fun_6_Click(object sender, EventArgs e)
        {
            if (IsSuppressedNow()) return;

            if (department == "IK")
            {
                using (var d1 = new Detail_6_IK(user_code))
                    d1.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else if (department == "RFC")
            {
                using (var d2 = new Detail_6_RFC(user_code))
                    d2.ShowDialog();            // <-- CE: chỉ có ShowDialog() không tham số
            }
            else
            {
                MessageBox.Show("Bộ phận không có trong dữ liệu");
                return;
            }              // <-- CE: chỉ có ShowDialog() không tham số

            AfterChildClosed();   // CHỐT phím rơi + focus
        }
    }
}