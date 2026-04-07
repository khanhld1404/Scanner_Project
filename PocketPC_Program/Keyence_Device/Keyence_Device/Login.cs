using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Threading;
using Keyence_Device.Class;
namespace Keyence_Device
{
    public partial class Login : Form
    {
        // Biến kiêm tra mật khẩu có đúng không
        public bool check_mk;

        public Login()
        {
            InitializeComponent();
            txt_ma.Focus();
        }


        public void Enter() {
            string ma = txt_ma.Text.Trim().ToString();
            string mk = txt_mk.Text.Trim().ToString();
            if(ma == "" && mk == ""){
                MessageBox.Show("Mời bạn nhập mã và mật khẩu!");
                return;
            }else if(ma == ""){
                MessageBox.Show("Mời bạn nhập mã!");
                return;
            }
            else if (mk == "")
            {
                MessageBox.Show("Mời bạn nhập mật khẩu!");
                return;
            }

            string password = get_password(ma);

            //Kiểm tra mật khẩu
            // Bắt đầu công việc nền trước khi ShowDialog
            using(var load_login = new LoadForm("Đang kiểm tra tài khoản!")){
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        check_mk = Handle_Password.Verify(mk, password);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Sử dụng BeginIvoke để an toàn tránh phát sinh lỗi
                        load_login.BeginInvoke(new Action(() =>
                        {
                            if (!load_login.IsDisposed) load_login.Close();
                        }));
                    }
                });
                // Hiển thị thông báo phải ở sau ThreadPool vì nếu dùng trước để mà tắt nó thì cần Close(), 
                // nhưng close nằm trong ThreadPool vì vậy có hiện tượng nó chỉ hiện thị form load dữ liệu chứ
                // không thực hiện thread nền 
                load_login.ShowDialog();
            }
            
            // Kiểm tra xem mã và mật khẩu đã đúng chưa
            if(!check_ma(ma)){
                MessageBox.Show("Mã người dùng không chính xác!");
                return;
            }else if(!check_mk){
                MessageBox.Show("Mật khẩu không chính xác!");
                return;
            }
            string department = get_group(ma);
 
            if (string.IsNullOrEmpty(department)) {
                return;
            }


            // Chuyển sang một form mới
            Main_Function mf = new Main_Function(ma,department);
            mf.ShowDialog();
            this.Close();
        }
        private void btn_Enter_Click(object sender, EventArgs e)
        {
            Enter();
        }

        // Xử lý sự kiện nhấn enter của ô mã nhân viên
        private void txt_ma_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                if (txt_mk.Text.Trim() == "")
                {
                    txt_mk.Focus();
                }
                else {
                    Enter();
                }
            }
        }

        // Xử lý dự kiện nhấn enter của ô mật khẩu
        private void txt_mk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_ma.Text.Trim() == "")
                {
                    txt_ma.Focus();
                }
                else
                {
                    Enter();
                }
            }
        }

        //Kiểm tra xem mã (user_code) có tồn tại không
        public bool check_ma(string ma) {
            using (var conn = new SqlCeConnection(DbConfig._connect)) {
                conn.Open();
                string sql = @"SELECT CASE WHEN EXISTS 
                         (SELECT 1 FROM LoginUser WHERE user_code = @ma) 
                       THEN 1 ELSE 0 END";
                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = sql;
                    cmd.Parameters.Add("@ma", SqlDbType.NVarChar).Value = ma;
                    object scalar = cmd.ExecuteScalar();
                    int result = Convert.ToInt32(scalar);
                    if (result == 1)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
        }

        //Lấy thông tin password
        public string get_password(string ma)
        {
            try
            {
                using (var conn = new SqlCeConnection(DbConfig._connect))
                {
                    conn.Open();
                    string sql = @"
                    select cast(password as nvarchar(200))
                    from LoginUser
                    where user_code = @ma;
                ";
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Parameters.Add("@ma", SqlDbType.NVarChar, 50).Value = ma;
                        object scalar = cmd.ExecuteScalar();
                        string result = Convert.ToString(scalar);
                        return result;
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


        //Kiểm tra thông tin người dùng thuộc bộ phận nào
        public string get_group(string ma) {
            try {
                using (var conn = new SqlCeConnection(DbConfig._connect))
                {
                    conn.Open();
                    string sql = @"
                    select cast(department as nvarchar(50))
                    from LoginUser
                    where user_code = @ma;
                ";
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Parameters.Add("@ma", SqlDbType.NVarChar,50).Value = ma;
                        object scalar = cmd.ExecuteScalar();
                        string result = Convert.ToString(scalar);
                        return result;
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