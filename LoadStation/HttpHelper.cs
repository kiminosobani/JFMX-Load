using LoadStation.pojo;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoadStation
{
    internal class HttpHelper
    {
        //todo 待修改
        private const string ADDRESS = @"http://localhost:9900";
        //private const string ADDRESS = @"http://128.2.10.21:9900";
        private const string SAVE_TXT = "file/SaveTxt";
        private const string GET_FILES = "file/NoSHA256";
        private const string GET_FILE_PATH = "file/getFileList";
        private const int TIME_OUT = 1000;

        /// <summary>
        /// 把字符串追加到服务器的某一路径下 或 保存文件到服务器某一目录
        /// </summary>
        /// <param name="reqData">请求参数</param>
        /// <returns>成功 true 失败 抛异常</returns>
        /// <remarks>
        /// 追加字符串示例:
        /// <code>
        /// HttpHelper.SaveTxt(new ReqData(@"\产品加工记录\aaa\bbb\283\Record.txt", "txt", DateTime.Now + "    " + "加工完成" + "    " + "位置: '15'" + "\n"));
        /// </code>
        /// 保存文件示例:
        /// <code>
        /// HttpHelper.SaveTxt(new ReqData(@"\产品加工记录\aaa\bbb\283\" + fileInfo.Name, File.ReadAllBytes(fileInfo.FullName)));
        /// </code>
        /// </remarks>
        public static bool SaveTxt(ReqData reqData)
        {
            try
            {
                var client = new RestClient(ADDRESS);
                client.Timeout = TIME_OUT;
                var request = new RestRequest(SAVE_TXT, Method.POST);
                request.AddJsonBody(reqData);
                var response = client.Execute<RespData>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                throw new Exception("追加字符串到文件失败 HttpHelper.SaveTxt()");
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(HttpHeader), e);
                return false;
            }
        }
        /// <summary>
        /// 下载加工程序到本地
        /// </summary>
        /// <param name="path">加工程序信息</param>
        /// <param name="targetPath">目标位置</param>
        /// <returns>成功 true 失败 抛异常 目录不存在或为空 false</returns>
        /// <remarks>
        /// 示例:
        /// <code>
        /// HttpHelper.GetAllFile(@"\加工程序\本地测试 B1-α\1-J-5", @"C:\rfmx\test");
        /// </code>
        /// </remarks>
        public static bool GetAllFile(string path, string targetPath)
        {
            try
            {
                var client = new RestClient(ADDRESS);
                client.Timeout = TIME_OUT;
                var request = new RestRequest(GET_FILE_PATH, Method.POST);
                request.AddJsonBody(path);
                var response = client.Execute<RespData>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Content == "" || response.ContentType == "")
                    {
                        return false;
                    }
                    // 获取文件路径
                    string[] paths = JsonConvert.DeserializeObject<string[]>(response.Content).Select(p => p.Replace("D:", "")).ToArray();
                    // 清空目标文件夹
                    ClearDirectorysAndFiles(targetPath);
                    // 多线程获取文件
                    foreach (string s in paths)
                    {
                        Task.Run(() =>
                        {
                            GetSingleFile(s, path, targetPath);
                        });
                    }
                    return true;
                }
                throw new Exception("获取文件失败 HttpHelper.GetAllFile()");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(HttpHeader), ex);
                return false;
            }
        }
        /// <summary>
        /// 读取服务器中txt的文本到List中
        /// </summary>
        /// <param name="filePath">文件在服务器上的相对路径</param>
        /// <returns>成功 文本list 失败 抛异常</returns>
        /// <remarks>
        /// 示例:
        /// <code>
        /// List<string> ls = HttpHelper.ReadTxt(@"\rfmx\加工程序\本地测试 B1-α\1-J-5\ToolList.txt");
        /// </code>
        /// </remarks>
        public static List<string> ReadTxt(string filePath)
        {
            try
            {
                var client = new RestClient(ADDRESS);
                client.Timeout = TIME_OUT;
                var request = new RestRequest(filePath, Method.GET);
                var response = client.Execute<RespData>(request);
                if (response.StatusCode == HttpStatusCode.OK && response.RawBytes != null)
                {
                    string fileContent = Encoding.UTF8.GetString(response.RawBytes);
                    string[] lines = fileContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    return new List<string>(lines);
                }
                throw new Exception("读取txt失败 HttpHelper.GetSingleFile()");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(HttpHeader), ex);
                return null;
            }
        }
        /// <summary>
        /// 获取服务器单个文件
        /// </summary>
        /// <param name="filePath">文件在服务器上的相对路径</param>
        /// <param name="oldStr">需要替换的路径</param>
        /// <param name="newStr">替换后的路径</param>
        /// <returns>成功 true 失败 抛异常</returns>
        /// <remarks>
        /// 示例:
        /// <code>
        /// HttpHelper.GetSingleFile(@"\rfmx\加工程序\本地测试 B1-α\1-J-5\ToolList.txt", @"\rfmx\加工程序\本地测试 B1-α\1-J-5", @"C:\rfmx\test");
        /// </code>
        /// </remarks>
        private static bool GetSingleFile(string filePath, string oldStr, string newStr)
        {
            try
            {
                var client = new RestClient(ADDRESS);
                client.Timeout = TIME_OUT;
                var request = new RestRequest(filePath, Method.GET);
                var response = client.Execute<RespData>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    filePath = filePath.Replace(@"\rfmx" + oldStr, newStr);
                    string directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory); // 创建目录
                    }
                    File.WriteAllBytes(filePath, response.RawBytes);
                    return true;
                }
                throw new Exception("获取文件失败 HttpHelper.GetSingleFile()");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(HttpHeader), ex);
                return false;
            }
        }
        public static byte[] GetSingleRaw(string absolutePath)
        {
            try
            {
                var client = new RestClient(ADDRESS);
                client.Timeout = TIME_OUT;
                var request = new RestRequest("file/NoSHA256", Method.POST);
                // 服务器端用 fileVal.getAbsolutePath()，所以传 absolutePath 字段
                request.AddJsonBody(new { absolutePath = absolutePath });

                var response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.RawBytes;
                }
                throw new Exception("获取文件失败");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(HttpHelper), ex);
                return null;
            }
        }
        // 清空指定文件夹下的文件和目录
        private static void ClearDirectorysAndFiles(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    // 删除文件夹中的所有文件
                    var directoryInfo = new DirectoryInfo(directoryPath);

                    // 删除所有文件
                    foreach (var file in directoryInfo.GetFiles())
                    {
                        file.Delete();
                    }

                    // 删除所有子目录，包括递归删除
                    foreach (var directory in directoryInfo.GetDirectories())
                    {
                        directory.Delete(true); // true表示递归删除子目录及其内容
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常或重新抛出
                throw new Exception("An error occurred while deleting files and directories.", ex);
            }
        }


        #region 暂时不用
        //下载加工程序到指定路径
        private static bool GetPrograms(RespData reqData, string path)
        {
            try
            {
                var client = new RestClient(ADDRESS);
                client.Timeout = TIME_OUT;
                var request = new RestRequest(GET_FILES, Method.POST);
                request.AddJsonBody(reqData);

                // 同步发送请求
                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // 将返回的压缩文件数据写入到临时文件
                    string tempZipFilePath = path + @"\tempFile.zip";
                    // 先清空目标文件夹下的文件
                    ClearFiles(path);

                    // 使用同步方式写入文件
                    File.WriteAllBytes(tempZipFilePath, response.RawBytes);
                    GC.Collect();

                    // 解压压缩包
                    ExtractZipFile(tempZipFilePath, path);

                    // 删除临时压缩文件
                    File.Delete(tempZipFilePath);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //获取目录下所有文件绝对路径
        public static string[] GetAllFilePath(string path)
        {
            try
            {
                var client = new RestClient(ADDRESS);
                client.Timeout = TIME_OUT;
                var request = new RestRequest(GET_FILE_PATH, Method.POST);
                request.AddJsonBody(path);
                var response = client.Execute<RespData>(request);
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength != 0)
                {
                    return JsonConvert.DeserializeObject<string[]>(response.Content).Select(p => p.Replace(@"D:", "")).ToArray();
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(HttpHelper), ex);
                return null;
            }
        }
        // 清空指定文件夹下的文件
        private static void ClearFiles(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    // 删除文件夹中的所有文件
                    var directoryInfo = new DirectoryInfo(directoryPath);
                    foreach (var file in directoryInfo.GetFiles())
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // 解压文件（同步方式）
        private static void ExtractZipFile(string zipFilePath, string extractPath)
        {
            try
            {
                // 确保目标文件夹存在
                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }

                // 使用 ZipArchive 进行解压
                using (var zipArchive = ZipFile.OpenRead(zipFilePath))
                {
                    foreach (var entry in zipArchive.Entries)
                    {
                        // 忽略目录，只处理文件
                        if (string.IsNullOrEmpty(entry.Name))
                            continue;

                        string filePath = Path.Combine(extractPath, entry.FullName);
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        // 使用同步方式解压文件
                        using (var entryStream = entry.Open())
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            entryStream.CopyTo(fileStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
