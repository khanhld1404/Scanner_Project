using CsvHelper;
using CsvHelper.Configuration;
using Manage_PocketPc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Manage_PocketPc.Services
{
    public class CsvImporter : ICsvImporter
    {
        private readonly string _connStr;

        public CsvImporter(IConfiguration config)
        {
            _connStr = config.GetConnectionString("Default")!;
        }


        public async Task<int> ImportAllAsync(string folderPath, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("folderPath is required.", nameof(folderPath));

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Folder not found: {folderPath}");

            var files = Directory.EnumerateFiles(folderPath, "*.csv", SearchOption.TopDirectoryOnly).ToList();
            if (files.Count == 0) return 0;

            int totalRows = 0;

            // Các format ngày giờ sẽ chấp nhận khi đọc từ CSV (12h với AM/PM như ví dụ của bạn)
            string[] dateFormats =
            {
        "MM/dd/yyyy HH:mm:ss tt",
        "MM/dd/yyyy H:mm:ss tt",
        "M/d/yyyy h:mm:ss tt",
        "M/d/yyyy hh:mm:ss tt"
    };

            foreach (var file in files)
            {
                ct.ThrowIfCancellationRequested();

                // Chuẩn bị DataTable đúng schema
                var table = new DataTable();
                table.Columns.Add("User_code", typeof(string));
                table.Columns.Add("Department", typeof(string));
                table.Columns.Add("Master_code", typeof(string));
                table.Columns.Add("OK_count", typeof(int));
                table.Columns.Add("NG_count", typeof(int));
                table.Columns.Add("DeviceNumber", typeof(string));
                table.Columns.Add("CreatedAt", typeof(DateTime));

                try
                {
                    var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        Delimiter = ",",
                        Encoding = System.Text.Encoding.UTF8,
                        TrimOptions = TrimOptions.Trim,
                        BadDataFound = null,
                        MissingFieldFound = null,
                        DetectColumnCountChanges = false,
                        IgnoreBlankLines = true
                    };

                    using (var reader = new StreamReader(file))
                    using (var csv = new CsvReader(reader, cfg))
                    {
                        if (!await csv.ReadAsync()) continue;
                        csv.ReadHeader();

                        while (await csv.ReadAsync())
                        {
                            ct.ThrowIfCancellationRequested();

                            string userCode = csv.TryGetField("User_code", out string? s1) ? s1?.Trim() ?? "" : "";
                            string department = csv.TryGetField("Department", out string? s2) ? s2?.Trim() ?? "" : "";
                            string masterCode = csv.TryGetField("Master_code", out string? s3) ? s3?.Trim() ?? "" : "";
                            string okRaw = csv.TryGetField("OK_count", out string? s4) ? s4?.Trim() ?? "" : "";
                            string ngRaw = csv.TryGetField("NG_count", out string? s5) ? s5?.Trim() ?? "" : "";
                            string device = csv.TryGetField("DeviceNumber", out string? s6) ? s6?.Trim() ?? "" : "";
                            string createdRaw = csv.TryGetField("CreatedAt", out string? s7) ? s7?.Trim() ?? "" : "";

                            // Bỏ qua nếu thiếu trường bắt buộc
                            if (string.IsNullOrWhiteSpace(userCode) ||
                                string.IsNullOrWhiteSpace(department) ||
                                string.IsNullOrWhiteSpace(masterCode) ||
                                string.IsNullOrWhiteSpace(okRaw) ||
                                string.IsNullOrWhiteSpace(ngRaw) ||
                                string.IsNullOrWhiteSpace(createdRaw))
                            {
                                continue;
                            }

                            // Parse int
                            if (!int.TryParse(okRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ok))
                                continue;
                            if (!int.TryParse(ngRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ng))
                                continue;

                            // Parse datetime: ưu tiên exact, fallback parse mềm
                            DateTime createdAt;
                            if (!DateTime.TryParseExact(createdRaw, dateFormats, CultureInfo.InvariantCulture,
                                    DateTimeStyles.AssumeLocal, out createdAt))
                            {
                                if (!DateTime.TryParse(createdRaw, CultureInfo.InvariantCulture,
                                        DateTimeStyles.AssumeLocal, out createdAt))
                                {
                                    continue;
                                }
                            }

                            table.Rows.Add(userCode, department, masterCode, ok, ng, device, createdAt);
                        }
                    }

                    if (table.Rows.Count == 0)
                        continue;

                    // Bulk copy vào bảng đích
                    using var conn = new SqlConnection(_connStr);
                    await conn.OpenAsync(ct);

                    using var bulk = new SqlBulkCopy(conn)
                    {
                        DestinationTableName = "dbo.ScanData",
                        BatchSize = 5000,
                        BulkCopyTimeout = 0
                    };

                    bulk.ColumnMappings.Add("User_code", "User_code");
                    bulk.ColumnMappings.Add("Department", "Department");
                    bulk.ColumnMappings.Add("Master_code", "Master_code");
                    bulk.ColumnMappings.Add("OK_count", "OK_count");
                    bulk.ColumnMappings.Add("NG_count", "NG_count");
                    bulk.ColumnMappings.Add("DeviceNumber", "DeviceNumber");
                    bulk.ColumnMappings.Add("CreatedAt", "CreatedAt");

                    await bulk.WriteToServerAsync(table, ct);

                    totalRows += table.Rows.Count;

                    // >>> XÓA FILE SAU KHI IMPORT THÀNH CÔNG <<<
                    try
                    {
                        System.IO.File.Delete(file);
                        Console.WriteLine("Xóa thành công!");
                    }
                    catch (Exception delEx)
                    {
                        // Nếu không xóa được (file lock / quyền), bạn có thể log cảnh báo

                        var logPath = Path.Combine(AppContext.BaseDirectory, "delete-errors.log");
                        await File.AppendAllTextAsync(logPath,
                            $"{DateTime.Now}: Không thể xóa {file} → {delEx}\n");

                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Lỗi xử lý file {Path.GetFileName(file)}: {ex.Message}");
                    // Không xóa file khi có lỗi (để kiểm tra). Bạn có thể move sang error/ nếu muốn.
                    continue;
                }
            }

            return totalRows;
        }

    }
}
