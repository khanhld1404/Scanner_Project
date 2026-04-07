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
        public static readonly string ConnStr = "Data Source=" + DbPath + ";Persist Security Info=False;";

        // đường dẫn trỏ đến dữ liệu tài khoản, mã master KeyenceData.sdf
        public static readonly string _baseDir = @"\Debug";
        public static readonly string _dbPath = Path.Combine(_baseDir, "KeyenceData.sdf");
        public static readonly string _device_number = Path.Combine(_baseDir, "device_infor.txt");
        public static readonly string _connect = "Data Source=" + _dbPath + ";Persist Security Info=False;";
    }

}
