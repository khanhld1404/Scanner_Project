namespace Manage_PocketPc.Services
{
    public class Cl_Connection
    {
        // đường dẫn đến nơi đọc các file csv để cho vào cơ sở dữ liệu
        //@"D:\Keyence_Project\Pocket_Data"

        // 4️⃣ Lưu file 
        public static string folder_data = @"D:\Keyence_Project\Pocket_Data";

        //public static string folder_data = Path.Combine(AppContext.BaseDirectory, "Pocket_Data");

        // Đường dẫn thư mục, có KeyenceData.sdf ở cuối là tên file được tạo chứ không phải nó có sẵn
        //var sdfPath = @"D:\Keyence_Project\Pocket_Data\KeyenceData.sdf";
        //var exePath = @"D:\Keyence_Project\Covert_Data\bin\Debug\Covert_Data.exe";

        // Đường dẫn đến nới lưu file dữ liệu sdf
        public static string sdfPath = @"C:\Khanh_Project\Keyence_Project\PocketPC_Web\Pocket_Data\KeyenceData.sdf";
        // Đường dẫn đến nơi có chương trình chuyển đổi dữ liệu
        public static string exePath = Path.Combine(AppContext.BaseDirectory, "Tools", "Covert_Data.exe");

    }
}
