using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
namespace Update_ScanProgram
{
    public partial class UpdateForm : Form
    {
        string application_path = @"\Debug";
        public UpdateForm()
        {
            InitializeComponent();
        }

        // Hàm tạo thư mục
        public static void CreateURL(string url)
        {
            if (!Directory.Exists(url))
            {
                Directory.CreateDirectory(url);
            }
        }

        // copy từ folder tải sang folder chạy
        public void CopyFile(string sourceDir, string targetDir)
        {
            // Lấy và copy tất cả file từ folder tải sang chạy
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);
            }
            // nếu folder tải tồn tại các folder con thì cũng tải các file trong thư mục con đó
            foreach (var diretory in Directory.GetDirectories(sourceDir))
            {
                CopyFile(diretory, Path.Combine(targetDir, Path.GetFileName(diretory)));
            }
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            //Cập nhật phần mềm

            //thông tin đường dẫn
            string download_path = Path.Combine(application_path, "DownloadFiles");

            // Bắt đầu công việc nền trước khi ShowDialog
            using (var load_update = new LoadForm("Đang cập nhật phần mềm!"))
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        //Kiểm tra sự tồn tại của file dẫn đến nơi lưu trữ dữ liệu
                        CreateURL(download_path);
                        //Thay file từ thư mục tải sang đường dẫn thực sự
                        CopyFile(download_path, application_path);
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
            string exepath = Path.Combine(application_path,"Keyence_Device.exe");
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = exepath;
            Process.Start(psi);
            Application.Exit();
        }

    }
}