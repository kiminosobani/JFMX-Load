using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace LoadStation
{
    class OverAll
    {
        // todo 待修改
        public static readonly EndPoint POINT = new IPEndPoint(IPAddress.Parse("192.168.100.200"), 7788);
        //public static readonly EndPoint POINT = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7788);
        // todo 待修改
        public static readonly string FILE_SERVER_IP = "192.168.100.200";
        // todo 修改
        //public static readonly string SOCKET_IP = "128.2.10.243";
        public static readonly string SOCKET_IP = "127.0.0.1";
        #region 工位编号
        /*
         * 1-12 托盘库
         * 80穿梭车
         * 21装卸站、22清洗房、23缓存位
         * 31AGV1、32AGV2、30AGV
         * 41-44 线外缓存位
         * 51机床1、52机床2、53机床1缓存位、54机床2缓存位
         */
        #endregion
        public static string PsInt2Name(int iPosition)
        {
            string PositionName;
            if (iPosition < 11)
                PositionName = iPosition.ToString() + @"#托盘";
            else if (iPosition == 22)
                PositionName = @"装卸站";
            else if (iPosition == 15)
                PositionName = @"加工设备1";
            else if (iPosition == 16)
                PositionName = @"加工设备2";
            else if (iPosition == 80)
                PositionName = @"穿梭车";
            else
                PositionName = @"位置错误";

            return PositionName;
        }

        public static int Name2Pos(string name)
        {
            try
            {
                return Convert.ToInt32(name);
            }
            catch (Exception e)
            {
                switch (name)
                {
                    case "装卸站":
                        return 22;
                    case "穿梭车":
                        return 80;
                    case "加工设备1":
                        return 15;
                    case "加工设备2":
                        return 16;
                }
                return -1;
            }
        }
        public static string GetLocalIP()
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

        public static Dictionary<int, List<string>> GetNextFrock(string ProduceType, int CurrentProcess)
        {

            string sourceFile = string.Format(@"C:\rfmx\加工程序\{0}\工装清单.txt", ProduceType);
            string sourceFolder = string.Format(@"C:\rfmx\加工程序\{0}", ProduceType);
            if (File.Exists(sourceFile))
            {
                //存在则删除
                File.Delete(sourceFile);

            }
            else
            {
                System.IO.Directory.CreateDirectory(sourceFolder);
            }

            string NetFile = string.Format(@"\\10.50.42.41\rfmx\加工程序\{0}\工装清单.txt", ProduceType);
            if (File.Exists(NetFile))
            {
                ////拷贝整个目录
                File.Copy(NetFile, sourceFile);


            }


            List<string> LineList = new List<string>();
            Dictionary<int, List<string>> InfoList = new Dictionary<int, List<string>>();
            using (StreamReader sr = new StreamReader(sourceFile, Encoding.GetEncoding("GBK")))
            {
                string Line;

                while ((Line = sr.ReadLine()) != null)
                {
                    LineList.Add(Line);
                }
            }
            foreach (string process in LineList)
            {
                int iProcess = Convert.ToInt32(process.Split('@')[0]);
                if (iProcess == CurrentProcess)
                {
                    InfoList.Add(0, new List<string>() { process.Split('@')[0], process.Split('@')[1] });
                    return InfoList;
                }
            }
            return InfoList;
        }

        public static int Pos(string pos)
        {
            switch (pos)
            {
                case "1":
                    return 1;
                case "2":
                    return 2;
                case "3":
                    return 3;
                case "4":
                    return 4;
                case "5":
                    return 5;
                case "6":
                    return 6;
                case "7":
                    return 7;
                case "8":
                    return 8;
                case "9":
                    return 9;
                case "10":
                    return 10;
                case "11":
                    return 11;
                case "12":
                    return 12;
                case "装卸站":
                    return 22;
                case "机床1":
                    return 15;
                case "机床2":
                    return 16;
            }
            return -1;
        }
    }
}
