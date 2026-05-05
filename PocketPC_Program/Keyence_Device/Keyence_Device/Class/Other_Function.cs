using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.IO;
using Microsoft.Win32;   
namespace Keyence_Device.Class
{
    public class Other_Function
    {
        //Hàm đọc toàn bộ thông tin có trong file

        public static string ReadAllText(string path, Encoding encoding)
        {
            if (path == null) throw new ArgumentNullException("path");
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sr = new StreamReader(fs, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        // Overload mặc định UTF-8 (không BOM) – đổi nếu bạn lưu Unicode/ANSI khác
        public static string ReadAllText(string path)
        {
            // hàm ở dưới là một hàm khác, không phải đệ quy
            return ReadAllText(path,Encoding.UTF8);
        }


        // Lấy thông tin phiên bản phần mềm
        public static string Version_Program()
        {
            string path = DbConfig.path_file_version;
            if (!File.Exists(path)) return "UNKNOWN";
            return ReadAllText(path).Trim();
        }

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

        //Lấy tên thiết bị
        public static string GetDeviceName()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Ident");
                if (key != null)
                {
                    object name = key.GetValue("Name");
                    if (name != null)
                        return name.ToString();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "UnknownDevice";
        }
    }
}
