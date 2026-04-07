using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.IO;
namespace Keyence_Device.Class
{

    public static class CsvExport
    {

        // Dấu phân tách CSV (dùng ','; nếu muốn TSV thì đổi thành '\t')
        private const char Sep = ',';

        /// <summary>
        /// Escape giá trị theo chuẩn CSV:
        /// - Nếu có dấu phẩy, nháy kép, xuống dòng -> bọc bằng "..."
        /// - Nháy kép bên trong -> thay bằng ""
        /// - null/DBNull -> chuỗi rỗng
        /// </summary>
        private static string CsvEscape(object val)
        {
            if (val == null || val == DBNull.Value) return "";
            string s = Convert.ToString(val);

            if (s.IndexOfAny(new char[] { '"', ',', '\r', '\n' }) >= 0)
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }

        /// <summary>
        /// Ghi một dòng CSV (đã escape), kết thúc bằng CRLF.
        /// </summary>
        private static void WriteCsvLine(StreamWriter sw, object[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (i > 0) sw.Write(Sep);
                sw.Write(CsvEscape(fields[i]));
            }
            sw.Write("\r\n");
        }

        // Xuất dữ liệu một bảng ra csv
        public static void ExportTable(string connStr,string tableName,string csvPath) 
        {
            // Đảm bảo thư mục tồn tại
            string dir = Path.GetDirectoryName(csvPath);
            if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            using (var conn = new SqlCeConnection(connStr))
            {
                conn.Open();

                // Build câu lệnh SELECT an toàn (không nối thẳng tableName từ người dùng nếu không tin cậy)
                string sql = "SELECT * FROM " + tableName;

                using (var cmd = new SqlCeCommand(sql, conn))
                using (var rdr = cmd.ExecuteReader())
                using (var fs = new FileStream(csvPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    // (Tùy chọn) Ghi BOM UTF-8 cho chắc (nếu phía server mong muốn BOM)
                    // sw.Write('\uFEFF');

                    // Header
                    int fieldCount = rdr.FieldCount;
                    object[] header = new object[fieldCount];
                    for (int i = 0; i < fieldCount; i++)
                        header[i] = rdr.GetName(i);
                    WriteCsvLine(sw, header);

                    // Rows
                    object[] row = new object[fieldCount];
                    while (rdr.Read())
                    {
                        for (int i = 0; i < fieldCount; i++)
                        {
                            object v = rdr.GetValue(i);
                            if (v is DateTime)
                            {
                                DateTime dt = (DateTime)v;
                                v = dt.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            row[i] = v == DBNull.Value ? "" : v;
                        }
                        WriteCsvLine(sw, row);
                    }
                }
            }
        }

        /// <summary>
        /// Xuất toàn bộ bảng trong DB ra nhiều file CSV (mỗi bảng 1 file).
        /// </summary>
        public static void ExportAllTables(string connStr, string outDir)   
        {
            if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);

            using (var conn = new SqlCeConnection(connStr))
            {
                conn.Open();
                // Lấy danh sách bảng user (bỏ __sys* …)
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT TABLE_NAME 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_TYPE='TABLE' 
                          AND TABLE_NAME NOT LIKE 'MSys%' 
                          AND TABLE_NAME NOT LIKE '__sys%'";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string table = rdr.GetString(0);
                            string csv = Path.Combine(outDir, table + ".csv");
                            ExportTable(connStr, table, csv);
                        }
                    }
                }
            }
        }
    }

}
