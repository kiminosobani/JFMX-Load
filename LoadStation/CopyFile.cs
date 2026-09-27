using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace LoadStation
{
    class CopyFile
    {
        public bool MoveDirectory(string path, string docID)
        {
            //todo 待修改
            string dosLine =string.Format(@"net use \\10.50.42.41\rfmx /user:administrator 123456");
            connect(dosLine);
            //net use h: \\ip\folders password /user:域名\user
            //if (path.IndexOf(GetLocalIP().Trim()) > 0)
            //{
            //    return true;
            //}

            CopyFolder(path, docID);

            //CopyFiles(path, docID);
            // return Flag;
            return true;
        }
        public static int CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest);//复制文件
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);//构建目标路径,递归复制文件
                }
                return 1;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return 0;
            }

        }

        public string GetLocalIP()
        {
            string AddressIP = string.Empty;
            foreach (System.Net.IPAddress _IPaddress in System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList)
            {
                if (_IPaddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPaddress.ToString();
                }
            }
            return AddressIP;
        }
        public string CopyFiles(string form_path, string toPath)//, string dosLine
        {

            Process proc = new Process();
            //string cmd = ($"xcopy {form_path} {toPath} /y /e /i /q"); //xcopy \\10.122.55.4\websites\test E:\demo\test\ /D /E /Y /K
            string cmd = ($"xcopy {form_path} {toPath}"); //xcopy \\10.122.55.4\websites\test E:\demo\test\ /D /E /Y /K
            try
            {
                return cmd + " == " + Docopy(proc, cmd);
            }
            catch (Exception ex)
            {

                cleanConnect();
                // connect(dosLine);
                Docopy(proc, cmd);

                return ex.Message;
            }
            //   return cmd;
        }
        //cmd命令拷贝 xcopy
        public string Docopy(Process proc, string cmd)
        {

            proc.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;

            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;//true表示不显示黑框，false表示显示dos界面

            // proc.StartInfo.Arguments = $" {cmd} ";// redirect ? @"/c " + "\"" + url  +"\"" : @"/k " + "\"" + url + "\"";


            proc.Start();
            proc.StandardInput.WriteLine(cmd);// (@"net use \\172.25.138.150User@123 /user:administrator");//xcopy \\eahis\netlogon\bmp c:\bmp /e/y


            proc.StandardInput.WriteLine("exit");

            while (!proc.HasExited)
            {
                proc.WaitForExit(1000);
            }
            string errormsg = proc.StandardError.ReadToEnd();
            proc.StandardError.Close();
            if (string.IsNullOrEmpty(errormsg))
            {
                // Flag = true;
            }
            else
            {
                throw new Exception(errormsg);
            }

            proc.Close();
            // proc.Dispose();


            return "";
        }

        //共享用户连接
        public bool connect(string dosLine)
        {
            try
            {
                return connectState(dosLine);
            }
            catch (Exception ex)
            {
                if (cleanConnect())
                {
                    return connectState(dosLine);
                }
            }
            return false;
        }

        public bool connectState(string dosLine)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                // string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }
        public bool cleanConnect()
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                // string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(" net use * /del /y");
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    // throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                // throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }

        //手动复制拷贝
        public void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }
}
}
