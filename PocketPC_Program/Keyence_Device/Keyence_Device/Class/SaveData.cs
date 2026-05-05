using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data.SqlTypes;
using System.Data;
namespace Keyence_Device.Class
{
    class SaveData
    {
        // Hàm để lưu thông tin vào file chứa dữ liệu quét của chương trình
        public static void Save(string user_code, string department, string master_code, int ok, int ng)
        {
            using (var conn = new SqlCeConnection(DbConfig.ConnStr))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO ScanData(User_code, Department, Master_code, OK_count, NG_count, DeviceNumber)
                    VALUES(@user_code, @department, @master_code, @ok, @ng, @device);";

                    // Khai báo kích thước cho NVARCHAR
                    var p1 = cmd.Parameters.Add("@user_code", SqlDbType.NVarChar, 50);
                    p1.Value = (object)user_code ?? DBNull.Value;

                    var p2 = cmd.Parameters.Add("@department", SqlDbType.NVarChar, 50);
                    p2.Value = department;

                    var p3 = cmd.Parameters.Add("@master_code", SqlDbType.NVarChar, 100);
                    p3.Value = master_code;

                    cmd.Parameters.Add("@ok", SqlDbType.Int).Value = ok;
                    cmd.Parameters.Add("@ng", SqlDbType.Int).Value = ng;

                    var p4 = cmd.Parameters.Add("@device", SqlDbType.NVarChar, 50);
                    p4.Value = Other_Function.GetDeviceName();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // hàm để xóa toàn bộ dữ liệu quét có trong file chứa dữ liệu quét
        public static void Delete() { 
            using(var conn = new SqlCeConnection(DbConfig.ConnStr)){
                conn.Open();
                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                        delete from ScanData
                    ";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}
