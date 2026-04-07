using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
// --------- HTTP helper ----------
// Xử lý việc gọi thông tin từ server và đẩy thông tin lên server 
namespace Keyence_Device.Class
{
    public static class ApiClient
    {
        // Gọi API để lấy dữ liệu về
        public static HttpWebResponse GetResponseWithHeaders(string url, string bearerToken, int timeoutMs)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.Timeout = timeoutMs;
            req.ReadWriteTimeout = timeoutMs;
            req.AllowAutoRedirect = true;
            req.UserAgent = "KeyenceDevice/1.0 (WinCE .NET CF 3.5)";
            req.Accept = "*/*";
            // Nếu server là HTTP (không phải HTTPS) thì thôi.
            // Nếu cần Bearer:
            if (!string.IsNullOrEmpty(bearerToken))
                req.Headers["Authorization"] = "Bearer " + bearerToken;

            // Nếu bạn dùng self-signed cert và thiết bị không tin cậy,
            // cần bỏ qua xác thực chứng chỉ (KHÔNG khuyến cáo trong production).
            // Trên .NET CF 3.5, callback này có thể KHÔNG khả dụng tùy ROM.
            // System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertsPolicy();

            var res = (HttpWebResponse)req.GetResponse();
            if (res.StatusCode != HttpStatusCode.OK &&
                res.StatusCode != HttpStatusCode.PartialContent)
            {
                res.Close();
                throw new WebException("Unexpected HTTP status: " + (int)res.StatusCode + " " + res.StatusDescription);
            }
            return res; // trả về để caller dùng Stream
        }

        public static void CopyStreamToFile(Stream input, string filePath)
        {
            byte[] buffer = new byte[16 * 1024]; // 16KB
            using (var fs = File.Create(filePath))
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, read);
                }
            }
        }

        /// <summary>
        /// Ghi đè an toàn: thay file đích bằng file tạm (đã ghi xong).
        /// Trên WinCE/CF, không có File.Replace. Ta xóa file đích nếu tồn tại, rồi Move tmp -> đích.
        /// </summary>
        public static void OverwriteNoBackup(string tmpPath, string destPath)
        {
            // Đảm bảo stream đóng hết trước khi thao tác file.
            if (File.Exists(destPath))
            {
                // Nếu file đích đang được mở (bởi app khác), sẽ throw => nên try-catch ở caller.
                File.Delete(destPath);
            }
            File.Move(tmpPath, destPath);
        }



        // Gọi API để đẩy dữ liệu lên


        public static void UploadCsv(
            string url,
            string bearerToken,
            int timeoutMs,
            string csvLocalPath,
            string serverFileName
        )
        {
            if (string.IsNullOrEmpty(serverFileName))
                serverFileName = "latest.csv";

            if (!File.Exists(csvLocalPath))
                throw new FileNotFoundException("CSV không tồn tại: " + csvLocalPath);

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            byte[] boundaryStart = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            // Part 1: name
            string nameHeader =
                "Content-Disposition: form-data; name=\"name\"\r\n" +
                "Content-Type: text/plain; charset=utf-8\r\n\r\n";
            byte[] nameHeaderBytes = Encoding.UTF8.GetBytes(nameHeader);
            byte[] nameValueBytes = Encoding.UTF8.GetBytes(serverFileName);
            byte[] crlf = Encoding.UTF8.GetBytes("\r\n");

            // Part 2: file
            string uploadFileName = Path.GetFileName(serverFileName);
            string fileHeader =
                "Content-Disposition: form-data; name=\"file\"; filename=\"" + uploadFileName + "\"\r\n" +
                "Content-Type: text/csv\r\n\r\n";
            byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeader);

            long fileLength = new FileInfo(csvLocalPath).Length;

            long contentLength =
                boundaryStart.Length + nameHeaderBytes.Length + nameValueBytes.Length + crlf.Length +
                boundaryStart.Length + fileHeaderBytes.Length + fileLength + trailer.Length;

            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = timeoutMs;
            req.ReadWriteTimeout = timeoutMs;
            req.UserAgent = "KeyenceDevice/1.0 (WinCE .NET CF 3.5)";
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            req.SendChunked = false;
            req.AllowWriteStreamBuffering = false;   // để true nếu muốn đơn giản hoá và có đủ RAM
            req.ContentLength = contentLength;

            if (!string.IsNullOrEmpty(bearerToken))
                req.Headers["Authorization"] = "Bearer " + bearerToken;

            using (var rs = req.GetRequestStream())
            {
                // --boundary + name part
                rs.Write(boundaryStart, 0, boundaryStart.Length);
                rs.Write(nameHeaderBytes, 0, nameHeaderBytes.Length);
                rs.Write(nameValueBytes, 0, nameValueBytes.Length);
                rs.Write(crlf, 0, crlf.Length);

                // --boundary + file header
                rs.Write(boundaryStart, 0, boundaryStart.Length);
                rs.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);

                // file body
                byte[] buffer = new byte[16 * 1024];
                using (var fs = File.OpenRead(csvLocalPath))
                {
                    int read;
                    while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        rs.Write(buffer, 0, read);
                    }
                }

                // trailer
                rs.Write(trailer, 0, trailer.Length);
            }

            using (var resp = (HttpWebResponse)req.GetResponse())
            {
                if (resp.StatusCode != HttpStatusCode.Created &&
                    resp.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("Upload thất bại: " + (int)resp.StatusCode + " " + resp.StatusDescription);
                }
            }
        }

    }
}
