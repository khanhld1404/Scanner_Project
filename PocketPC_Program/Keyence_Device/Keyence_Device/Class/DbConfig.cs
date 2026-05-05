using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Keyence_Device.Class
{
    // Một nơi DÙNG CHUNG cho toàn app:
    public static class DbConfig
    {
        // đường dẫn trỏ đến nơi lưu trữ dữ liệu quét mã vạch của thiết bị
        public static readonly string BaseDir = @"\Application Data\KeyenceDevice";
        public static readonly string DbPath = Path.Combine(BaseDir, "SdfKeyence.sdf");
        public static readonly string CsvPath = Path.Combine(BaseDir,"CsvKeyence.csv");
        // thông tin kết nối của file sdf lưu dữ liệu quét
        public static readonly string ConnStr = "Data Source=" + DbPath + ";Persist Security Info=False;";


        // đường dẫn trỏ đến dữ liệu tài khoản, mã master,.. qua file KeyenceData.sdf
        public static readonly string _baseDir = @"\Debug";
        public static readonly string _dbPath = Path.Combine(_baseDir, "KeyenceData.sdf");
        // thông tin kết nối đến file thông tin master
        public static readonly string _connect = "Data Source=" + _dbPath + ";Persist Security Info=False;";


        // Các thông tin liên quan đến việc cập nhật phần mềm
        // đường dẫn đến file txt lưu thông tin phiên bản phần mềm hiện tại
        public static readonly string path_file_version = Path.Combine(_baseDir, "device_version.txt");
        // đường dẫn đến version mới nhất
        public static readonly string server_file_version = "0";
        // đường dẫn đến nơi để tải các file về
        public static readonly string server_file = "";
        // Đường dẫn đến nơi để lưu trữ file tải
        public static readonly string path_file_download = Path.Combine(_baseDir, "DownloadFiles");
    }

}
