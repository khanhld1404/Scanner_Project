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
using Keyence_Device.Class;
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
                var current_version = Other_Function.Version_Program();
                // lấy phiên bản đang có trên server

                // so sáng phiên bản hiện tại với trên server
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