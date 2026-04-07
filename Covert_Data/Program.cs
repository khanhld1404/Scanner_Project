using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
namespace Covert_Data
{
    internal class Program
    {
        static int Main(string[] args)
        {
            try
            {
                // args[0] = đường dẫn sdf
                var sdfPath = args[0];

                var sqlConn = "Data Source=10.239.1.54;Initial Catalog=Keyence_Data;User ID=khanh_ld;Password=250711";
                var tables = new[] { "IK", "RFC", "LoginUser" };

                var svc = new Convert_Data(
                    sqlConn,
                    tables,
                    sdfPath
                );

                svc.Run();

                Console.WriteLine("SUCCESS");
                return 0;

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return 1;
            }
        }
    }
} 