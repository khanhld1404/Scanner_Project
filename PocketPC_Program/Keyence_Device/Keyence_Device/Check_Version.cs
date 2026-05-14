using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Net;
using Keyence_Device.Function;
using Keyence_Device.Api;
namespace Keyence_Device
{
    public partial class Check_Version : Form
    {

        public Check_Version()
        {
            InitializeComponent();
        }

        private void Check_Version_Load(object sender, EventArgs e)
        {
            try {
                
                // Kiểm tra xem đã có nơi chưa file lưu chưa, nếu chưa thì tạo
                CreateURL(DbConfig.path_file_download);
                // kiểm tra phiên bản hiện tại
                var current_version = Convert.ToInt32(Other_Function.Version_Program());

                // lấy phiên bản đang có trên server
                int server_version = Convert.ToInt32(ApiProgram.Get_version(DbConfig.server_file_version));

                // so sáng phiên bản hiện tại với trên server
                if (current_version >= server_version) {
                    Login lg = new Login();
                    lg.ShowDialog();
                    this.Close();
                }else{
                    // Kiểm tra người dùng có muốn cập nhật phần mềm mới không
                    lab_tb.Text = @"Phần mềm đang có phiên bản " + server_version;   
                    ConfirmForm cf = new ConfirmForm("Bạn có muốn cập nhật");
                    if (cf.DialogResult == DialogResult.OK)
                    {
                        // Cập nhật file hiện tại sang phiên bản mới nhất
                        Other_Function.WriteAllText(DbConfig.path_file_version, server_version.ToString());
                        // tải phiên bản mới nhất về
                        ApiProgram.DownloadAllFiles(DbConfig.server_file, DbConfig.path_file_download);

                        // chạy chương trình cập nhật phần mềm
                        string exepath = Path.Combine(DbConfig._baseDir, "Update_ScanProgram.exe");
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = exepath;
                        Process.Start(psi);
                        Application.Exit();
                    }
                    else {
                        Login lg = new Login();
                        lg.ShowDialog();
                        this.Close();
                    }
                }
            }catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateURL(string _url)
        {
            //Kiểm tra đường dẫn
            if (!Directory.Exists(_url))
                Directory.CreateDirectory(_url);
        }

        private void getFiles()
        {
            //trỏ đến file package trong thư mục của máy local
            string download_zip_file = DbConfig.path_file_download + "Package.zip";
            
        }
    }
}