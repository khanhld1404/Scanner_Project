using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Keyence_Device.Class;
using System.Threading;
using System.Net;
using System.IO;
namespace Keyence_Device
{
    public partial class SubMenu : Form
    {
        public SubMenu()
        {
            InitializeComponent();
        }

        // ====== CẤU HÌNH DOWNLOADER ======
        private string ServerUrl = "http://172.31.9.31/test_api/api/v1/sdf";
        private string BearerToken = null;
        private int TimeoutMs = 120000;

        private void Btn_Update_Click(object sender, EventArgs e)
        {

            ConfirmForm cf = new ConfirmForm("Cập nhật dữ liệu", "Bạn có muốn cập nhật?");
            if (cf.ShowDialog() == DialogResult.OK)
            {
                // Xuất CSV
                CsvExport.ExportTable(DbConfig.ConnStr, "ScanData",
                    @"\Application Data\KeyenceDevice\CsvKeyence.csv");

                // Xóa dữ liệu đã lưu (nếu cần)
                SaveData.Delete();

                // Nhả capture để tránh “giữ trạng thái nhấn”
                var mb = sender as Control;
                if (mb != null) mb.Capture = false;

                // Chặn bấm liên tiếp
                var btn = sender as Button;
                if (btn != null) btn.Enabled = false;

                string message = null;

                // tên file csv sẽ xuất hiện trên server
                string filename = Other_Function.LoadDeviceCode() + "_" + DateTime.Now.ToString("ddmmyyyy-HH-mm") + ".csv";

                using (var loading = new LoadForm("Đang cập nhật dữ liệu!"))
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            // 1) Upload CSV
                            try
                            {
                                ApiClient.UploadCsv(ServerUrl /* https://host/api/v1/sdf */,
                                          BearerToken,
                                          TimeoutMs,
                                          DbConfig.CsvPath,
                                          filename);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            // 2) Tải SDF về
                            string dir = Path.GetDirectoryName(DbConfig._dbPath);
                            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                            using (HttpWebResponse res = ApiClient.GetResponseWithHeaders(ServerUrl, BearerToken, TimeoutMs))
                            {
                                string tmp = Path.Combine(dir, Guid.NewGuid().ToString("N") + ".tmp");
                                using (Stream rs = res.GetResponseStream())
                                    ApiClient.CopyStreamToFile(rs, tmp);

                                ApiClient.OverwriteNoBackup(tmp, DbConfig._dbPath);
                                message = "Cập nhật dữ liệu thành công!";
                            }
                        }
                        catch
                        {
                            message = "Mạng có vấn đề. Vui lòng thử ở nơi có Wi‑Fi hoặc liên hệ IT (máy 274).";
                        }


                        finally
                        {
                            loading.BeginInvoke(new Action(() =>
                            {
                                if (!loading.IsDisposed) loading.Close();
                            }));
                        }
                    });
                    loading.ShowDialog();
                }

                if (btn != null) btn.Enabled = true;
                MessageBox.Show(message);
            }
        }

        private void Btn_Infor_Click(object sender, EventArgs e)
        {
            using (var tt = new ProgramInfor()) {
                tt.ShowDialog();
            }
        }

        private void Btn_Logout_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}