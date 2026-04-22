using System;
using System.IO;

namespace Manage_PocketPc.Services
{
    public class CsvExporter
    {
        public static void WriteToFile(string content, string filePath, string fileName)
        {

            try
            {
                // 1. Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                // 2. Ghép đường dẫn thư mục + tên file
                string fullPath = Path.Combine(filePath, fileName);

                // 3. Ghi đè nội dung file
                File.WriteAllText(fullPath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi file: {ex.Message}");
            }

        }

    }
}
