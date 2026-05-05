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
namespace Update_Program
{
    public partial class Form1 : Form
    {


        // Hàm tạo thư mục
        public static void CreateURL(string url) {
            if (!Directory.Exists(url)) {
                Directory.CreateDirectory(url);
            }
        }

        // copy từ folder tải sang folder chạy
        public void CopyFile(string sourceDir, string targetDir) {
            // Lấy và copy tất cả file từ folder tải sang chạy
            foreach (string file in Directory.GetFiles(sourceDir)) {
                File.Copy(file,Path.Combine(targetDir,Path.GetFileName(file)),true);
            }
            // nếu folder tải tồn tại các folder con thì cũng tải các file trong thư mục con đó
            foreach (var diretory in Directory.GetDirectories(sourceDir)) {
                CopyFile(diretory, Path.Combine(targetDir, Path.GetFileName(diretory)));
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            //Cập nhật phần mềm
            // Bắt đầu công việc nền trước khi ShowDialog
            using (var load_update = new LoadForm2("Đang cập nhật phần mềm!"))
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        //Kiểm tra sự tồn tại của file dẫn đến nơi lưu trữ dữ liệu
                        string download_url = Application.StartupPath + "\\DownloadFiles";
                        CreateURL(download_url);
                        //Thay file từ thư mục tải sang đường dẫn thực sự
                        CopyFile(download_url, Application.StartupPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Sử dụng BeginIvoke để an toàn tránh phát sinh lỗi
                        load_update.BeginInvoke(new Action(() =>
                        {
                            if (!load_update.IsDisposed) load_update.Close();
                        }));
                    }
                });
                // Hiển thị thông báo phải ở sau ThreadPool vì nếu dùng trước để mà tắt nó thì cần Close(), 
                // nhưng close nằm trong ThreadPool vì vậy có hiện tượng nó chỉ hiện thị form load dữ liệu chứ
                // không thực hiện thread nền 
                load_update.ShowDialog();
            }

            // chạy chương trình thực sự
            Process.Start("Keyence_Device.exe");
            Application.Exit();
        }
       

    }
}
