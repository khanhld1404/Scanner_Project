using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covert_Data
{
    public class Convert_Data
    {

        private readonly string _sqlServerConn;         // conn SQL Server nguồn
        private readonly string[] _tables;               // "IK", "RFC", "LoginUser"
        // Đường dẫn file .sdf đầu ra, nó sẽ lưu file sdf được tạo theo thư mục đường dẫn 
        private readonly string _sdfPath ;

        // Nếu muốn giữ file .sdf cũ, đặt false. Ở đây tạo mới mỗi lần cho sạch.
        private const bool _recreateEachRun = true;

        public Convert_Data(string sqlServerConn, string[] tables, string sdfPath)
        {
            _sqlServerConn = sqlServerConn;
            _tables = tables;
            _sdfPath = sdfPath;
        }

        public string Run()  // trả về đường dẫn file .sdf đã tạo
        {
            EnsureSqlCeDatabase();

            using (var ceConn = new SqlCeConnection(GetCeConnStr()))
            {
                ceConn.Open();

                foreach (var table in _tables)
                    RecreateTable(ceConn, table);

                foreach (var table in _tables)
                    CopyTable(table, ceConn);
            }

            return _sdfPath;
        }

        //Đường dẫn đến nơi database chứa sdf
        private string GetCeConnStr() => $"Data Source={_sdfPath};Persist Security Info=False;";

        //Tạo file sdf để chứa dữ liệu
        private void EnsureSqlCeDatabase()
        {
            if (File.Exists(_sdfPath) && _recreateEachRun)
            {
                File.Delete(_sdfPath);
            }

            if (!File.Exists(_sdfPath))
            {
                Console.WriteLine("Create database CE: " + _sdfPath);
                var engine = new SqlCeEngine(GetCeConnStr());
                engine.CreateDatabase();
            }
            else
            {
                Console.WriteLine("Using database CE: " + _sdfPath);
            }
        }

        private void RecreateTable(SqlCeConnection ceConn, string table)
        {
            // drop nếu tồn tại
            using (var cmd = ceConn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @t";
                cmd.Parameters.AddWithValue("@t", table);
                int exists = Convert.ToInt32(cmd.ExecuteScalar());

                if (exists > 0)
                {
                    using (var drop = ceConn.CreateCommand())
                    {
                        drop.CommandText = $"DROP TABLE [{table}];";
                        drop.ExecuteNonQuery();
                    }
                }
            }

            // create hợp lệ
            using (var create = ceConn.CreateCommand())
            {
                if (table != "LoginUser")
                {
                    create.CommandText = $@"
                CREATE TABLE [{table}] (
                    [an] BIGINT NULL,
                    [product] NVARCHAR(255) NULL
                );";
                }
                else
                {
                    create.CommandText = $@"
                CREATE TABLE [LoginUser] (
                    [user_code] NVARCHAR(50) NOT NULL PRIMARY KEY,
                    [password]  NVARCHAR(200) NULL,
                    [department] NVARCHAR(50) NULL
                );";
                }
                create.ExecuteNonQuery();
            }
        }

        private void CopyTable(string table, SqlCeConnection ceConn)
        {
            using (var src = new SqlConnection(_sqlServerConn))
            using (var readCmd = src.CreateCommand())
            using (var tx = ceConn.BeginTransaction())
            using (var ins = ceConn.CreateCommand())
            {
                src.Open();
                int copied = 0;

                if (table != "LoginUser")
                {
                    readCmd.CommandText = $"SELECT [an], [product] FROM dbo.[{table}];";

                    ins.Transaction = tx;
                    ins.CommandText = $"INSERT INTO [{table}] ([an], [product]) VALUES (@an, @product)";
                    ins.Parameters.Add(new SqlCeParameter("@an", DBNull.Value));
                    ins.Parameters.Add(new SqlCeParameter("@product", DBNull.Value));

                    using (var reader = readCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ins.Parameters[0].Value = reader.IsDBNull(0) ? (object)DBNull.Value : reader.GetInt64(0);
                            ins.Parameters[1].Value = reader.IsDBNull(1) ? (object)DBNull.Value : reader.GetString(1);
                            ins.ExecuteNonQuery();
                            copied++;
                        }
                    }
                }
                else
                {
                    readCmd.CommandText = $"SELECT [user_code], [password], [department] FROM dbo.[{table}];";

                    ins.Transaction = tx;
                    ins.CommandText = $"INSERT INTO [LoginUser] ([user_code], [password], [department]) VALUES (@u, @p, @d)";

                    ins.Parameters.Add(new SqlCeParameter("@u", SqlDbType.NVarChar, 50));
                    ins.Parameters.Add(new SqlCeParameter("@p", SqlDbType.NVarChar, 200));  
                    ins.Parameters.Add(new SqlCeParameter("@d", SqlDbType.NVarChar, 50));

                    using (var reader = readCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ins.Parameters[0].Value = reader.IsDBNull(0) ? (object)DBNull.Value : reader.GetString(0);
                            ins.Parameters[1].Value = reader.IsDBNull(1) ? (object)DBNull.Value : reader.GetString(1);
                            ins.Parameters[2].Value = reader.IsDBNull(2) ? (object)DBNull.Value : reader.GetString(2);
                            ins.ExecuteNonQuery();
                            copied++;
                        }
                    }
                }

                tx.Commit();
            }
        }

    }
}
