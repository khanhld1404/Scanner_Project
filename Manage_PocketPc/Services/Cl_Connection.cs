namespace Manage_PocketPc.Services
{
    public class Cl_Connection
    {
        // Đường dẫn đến nơi lữu trữ thông tin tổng quan 
        public static string folder_data = Path.Combine(AppContext.BaseDirectory, "Pocket_Data");

        // đường dẫn đến nơi đọc các file csv để cho vào cơ sở dữ liệu
        //public static string folder_data = Path.Combine(folder_data,"Csv_file");
        public static string folder_csv = @"D:\Keyence_Project\Pocket_Data\Csv_file";

        // Đường dẫn đến nơi lưu các file cập nhật
        public static string folder_download = @"D:\Keyence_Project\Pocket_Data\DownloadedFiles";

        // Đường dẫn thư mục, có KeyenceData.sdf ở cuối là tên file được tạo chứ không phải nó có sẵn
        //var sdfPath = @"D:\Keyence_Project\Pocket_Data\KeyenceData.sdf";
        //var exePath = @"D:\Keyence_Project\Covert_Data\bin\Debug\Covert_Data.exe";

        // Đường dẫn đến nới lưu file dữ liệu sdf
        public static string sdfPath = Path.Combine(folder_data,"KeyenceData.sdf");

        // Đường dẫn đến nơi có chương trình chuyển đổi dữ liệu
        public static string exePath = Path.Combine(AppContext.BaseDirectory, "Tools", "Covert_Data.exe");

    }
}
