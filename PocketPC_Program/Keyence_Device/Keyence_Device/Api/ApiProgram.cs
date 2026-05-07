using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
namespace Keyence_Device.Api
{
    public class ApiProgram
    {
        // Khai báo tên class chứa file để dùng cho nhiều class

        public class UpdateFileInfo
        {
            public string FileName;
        }

        // Lấy phiên bản trên server về
        public static string Get_version(string url){
         
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 10000;
                request.KeepAlive = false;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                string version = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return(version.Trim());
            }
            catch (Exception ex)
            {
                return(ex.Message);
            }

        }

        // Lấy từng file về
        public static bool DownloadFile(string url, string savePath)
        {

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream responseStream = null;
            FileStream fileStream = null;

            try
            {
                string folder = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 60000;

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }

                responseStream = response.GetResponseStream();

                // Nếu file đã tồn tại thì ghi đè
                fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);

                byte[] buffer = new byte[4096];
                int bytesRead = 0;

                while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }

                fileStream.Flush();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
                if (responseStream != null) responseStream.Close();
                if (response != null) response.Close();
            }
        }

        // Lấy danh sách tên của file

        public static ArrayList GetFileList(string url)
        {
            ArrayList result = new ArrayList();

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream responseStream = null;
            StreamReader reader = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 60000;

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return result;
                }

                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                string content = reader.ReadToEnd();
                string[] lines = content.Split('\n');

                int i;
                for (i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();

                    if (line == "")
                        continue;

                    UpdateFileInfo item = new UpdateFileInfo();
                    item.FileName = line;
                    result.Add(item);
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (responseStream != null) responseStream.Close();
                if (response != null) response.Close();
            }
        }

        // tải tất cả các file theo tên

        public static bool DownloadAllFiles(string baseApiUrl, string localRootFolder)
        {

            try
            {
                string filesApi = baseApiUrl + "/files";

                ArrayList files = GetFileList(filesApi);

                if (files.Count == 0)
                {
                    return false;
                }

                int total = files.Count;
                int i;

                for (i = 0; i < total; i++)
                {
                    UpdateFileInfo item = (UpdateFileInfo)files[i];

                    string fileName = item.FileName;
                    string localPath = Path.Combine(localRootFolder, fileName);

                    // Nếu tên file có khoảng trắng thì encode đơn giản
                    string downloadUrl = baseApiUrl + "/download/" + fileName.Replace(" ", "%20");

                    bool ok = DownloadFile(downloadUrl, localPath);

                    if (!ok)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
