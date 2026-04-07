using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.IO;
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
            return ReadAllText(path, Encoding.UTF8);
        }


        // Lấy thông tin định danh thiết bị 

        public static string LoadDeviceCode()
        {
            string path = DbConfig._device_number;
            if (!File.Exists(path)) return "UNKNOWN";
            return ReadAllText(path).Trim();
        }
    }
}
