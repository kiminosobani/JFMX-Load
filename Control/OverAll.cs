using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.IO;
using Opc.Ua;
namespace Control
{
    class OverAll
    {
        //统一状态名，方便修改，避免状态输入多字少字前后不统一问题

        //设备通用状态
        public const string EQUIP_STATE_FREE = "空闲";
        public const string EQUIP_STATE_TO_BE_REMOVED = "待取出";
        public const string EQUIP_STATE_TO_BE_PLACED = "待置入";
        public const string EQUIP_STATE_PLACED = "产品置入";
        public const string EQUIP_STATE_LOCK = "锁定";
        public const string EQUIP_STATE_PROCESSED = "加工中";
        //RGV状态
        public const string RGV_STATE_HANDLING = "搬运";
        //Wash状态
        public const string TUO_STATE_TO_BE_CLEANED = "待清洗";
        public const string TUO_STATE_TO_BE_CLEANED_LOOK = "待清洗锁定";
        public const string TUO_STATE_TO_BE_PROCESSED = "待加工";

        //缓存位状态






        public static Boolean JudgeDs(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                return false;
            }
            return true;


        }
        /************************Rfid数据格式转化*************************************/
        #region Rfid数据格式转化
        public static int Hex2Int(Byte[] HexValue)
        {
            string result = null;
            for (int i = 0; i < HexValue.Length; i++)
            {
                result += Convert.ToString(HexValue[i], 16);
            }

            return Convert.ToInt32(result);
        }
        public static int Byte2Int(Byte[] ByteValue)
        {
            string stringHex = null;
            for (int i = 0; i < ByteValue.Length; i++)
            {
                stringHex += ByteValue[i].ToString("X");
            }
            int result = Convert.ToInt32(stringHex, 16);


            return result;
        }
        public static string Hex2String(Byte[] HexValue)
        {

            return Encoding.Default.GetString(HexValue);


        }
        public static double Hex2Double(Byte[] HexValue)
        {
            double dValue;
            //1：符号位 0:﹢，1：-
            //2、3：整数位
            //4、5：小数位
            byte[] SingValue = new byte[1];
            Array.Copy(HexValue, 0, SingValue, 0, 1);
            int iSing = Hex2Int(SingValue);

            byte[] IntValue = new byte[2];
            Array.Copy(HexValue, 1, IntValue, 0, 2);
            int iInt = Hex2Int(IntValue);

            byte[] DecValue = new byte[2];
            Array.Copy(HexValue, 3, DecValue, 0, 2);
            int iDec = Hex2Int(DecValue);

            dValue = iInt + iDec / 1000;
            return iSing == 0 ? dValue : -dValue;
        }


        public static double Byte2Double(Byte[] ByteValue)
        {
            double dValue;
            //1：符号位 0:﹢，1：-
            //2、3：整数位
            //4、5：小数位
            byte[] SingValue = new byte[1];
            Array.Copy(ByteValue, 0, SingValue, 0, 1);
            int iSing = Byte2Int(SingValue);

            byte[] IntValue = new byte[2];
            Array.Copy(ByteValue, 1, IntValue, 0, 2);
            int iInt = Byte2Int(IntValue);

            byte[] DecValue = new byte[2];
            Array.Copy(ByteValue, 3, DecValue, 0, 2);
            int iDec = Byte2Int(DecValue);

            dValue = iInt + (double)iDec / 1000;
            return iSing == 0 ? dValue : -dValue;
        }

        public static Byte[] Int2Hex(int iValue, int iLength)
        {

            String strA = iValue.ToString("X" + (2 * iLength).ToString());
            byte[] buffer = new byte[iLength];
            for (int i = 0; i < iLength * 2; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(strA.Substring(i, 2), 16);
            }

            return buffer;
        }
        public static byte[] String2Hex(string sValue, int iLength)
        {
            int[] iValue = new int[iLength];
            byte[] array = System.Text.Encoding.ASCII.GetBytes(sValue);
            byte[] valueArray = new byte[iLength];

            for (int i = 0; i < iLength - array.Length; i++)
            {
                valueArray[i] = 0x00;
            }
            Array.Copy(array, 0, valueArray, iLength - array.Length, array.Length);

            return valueArray;
        }


        public static Byte[] Double2Hex(double dValue)
        {
            byte[] valueArray = new byte[5];
            //1：符号位 0:﹢，1：-
            //2、3：整数位
            //4、5：小数位
            if (dValue < 0)
                valueArray[0] = 0x01;
            else
                valueArray[0] = 0x00;

            Array.Copy(Int2Hex((int)Math.Abs(dValue), 2), 0, valueArray, 1, 2);

            int iDouble = 0;
            string s = dValue.ToString();
            if (s.Contains("."))
            {
                s = s.Split('.')[1];
                int sl = s.Length;
                for (int i = 3; i > sl; i--)
                {
                    s += "0";
                }
                iDouble = Convert.ToInt32(s);
            }

            Array.Copy(Int2Hex(iDouble, 2), 0, valueArray, 3, 2);

            return valueArray;
        }
        #endregion
        /// <summary>
        /// 上位机和下位机位置点编号转换，返回值：是否有门
        /// </summary>

        public static void DebugLogTest(string action)
        {
            return;
#if DEBUG
#else
            return;
#endif

            string nianyuetime = DateTime.Now.ToString("yyyy.MM");
            string ritime = DateTime.Now.ToString("dd");
            string fenmiaotime = DateTime.Now.ToString("mm.ss");

            //判断路径是否存在
            string address = string.Format(@"C:\{0}", nianyuetime);
            if (!System.IO.Directory.Exists(address))
            {
                System.IO.Directory.CreateDirectory(address);
            }
            string address1 = string.Format(@"C:\{0}\{1}", nianyuetime, ritime + "日");
            if (!System.IO.Directory.Exists(address1))
            {
                System.IO.Directory.CreateDirectory(address1);
            }

            //保存txt
            string _time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            string result1 = address1 + "\\" + ritime + ".txt";
            FileStream fs = new FileStream(result1, FileMode.Append);
            StreamWriter wr = null;
            try
            {
                wr = new StreamWriter(fs);
                wr.BaseStream.Seek(0, SeekOrigin.End);
                wr.Write(_time + "    "
                         + action
                         + "\n");
                wr.Flush();
                wr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OverAll), ex);
                wr.Flush();
                wr.Close();
                fs.Close();
            }
        }
        //获取变量名
        public static string GetVarName(System.Linq.Expressions.Expression<Func<string, string>> exp)
        {
            return ((System.Linq.Expressions.MemberExpression)exp.Body).Member.Name;
        }

        public static void UpdateTrayInfo()
        {
        }

        public static int Equip2Int(string _EquipName)
        {
            if (_EquipName == @"五轴1")
                return 51;
            else if (_EquipName == @"五轴2")
                return 52;
            else if (_EquipName == @"三轴1")
                return 53;
            else if (_EquipName == @"三轴2")
                return 54;
            else if (_EquipName == @"三轴3")
                return 55;
            else if (_EquipName == @"清洗站")
                return 56;
            else if (_EquipName == @"测量站")
                return 57;
            else if (_EquipName == @"装卸站1")
                return 61;
            else if (_EquipName == @"装卸站2")
                return 62;
            else
                return 80;

        }
        public static int getEquipId(string _EquipName)
        {
            switch (_EquipName)
            {
                case "加工设备1": return 51;
                case "加工设备2": return 52;
                case "清洗房": return 53;
                case "装卸站": return 54;
                case "装卸站缓存位": return 60;
                case "加工设备1待加工缓存位": return 61;
                case "加工设备1成品缓存位": return 62;
                case "加工设备2待加工缓存位": return 63;
                case "加工设备2成品缓存位": return 64;
                case "五轴1缓存位": return 64;
                case "五轴2缓存位": return 64;
                default: return 80;//穿梭车
            }
        }
        /// <summary>
        /// 检测远程主机 ICMP Ping 连通性（基于 .NET 原生高并发网络类库，无 cmd.exe 开销，支持自定义超时）
        /// </summary>
        /// <param name="remoteHost">目标 IP 或主机名</param>
        /// <param name="timeoutMs">超时毫秒数，默认 800ms（工业局域网通常 1~5ms）</param>
        /// <returns>是否连通</returns>
        public static bool Ping(string remoteHost, int timeoutMs = 800)
        {
            if (string.IsNullOrWhiteSpace(remoteHost))
                return false;

            try
            {
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = ping.Send(remoteHost.Trim(), timeoutMs);
                    return reply != null && reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OverAll), ex);
                return false;
            }
        }

        /// <summary>
        /// 异步检测远程主机 ICMP Ping 连通性
        /// </summary>
        /// <param name="remoteHost">目标 IP 或主机名</param>
        /// <param name="timeoutMs">超时毫秒数，默认 800ms</param>
        /// <returns>是否连通</returns>
        public static async Task<bool> PingAsync(string remoteHost, int timeoutMs = 800)
        {
            if (string.IsNullOrWhiteSpace(remoteHost))
                return false;

            try
            {
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = await ping.SendPingAsync(remoteHost.Trim(), timeoutMs);
                    return reply != null && reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OverAll), ex);
                return false;
            }
        }
        public static void ProductRecord(string _Product, string _Order, string _No, string _action, string _event)
        {


            string nianyuetime = DateTime.Now.ToString("yyyy.MM");
            string ritime = DateTime.Now.ToString("dd");
            string fenmiaotime = DateTime.Now.ToString("mm.ss");

            //判断路径是否存在
            string address = string.Format(@"C:\RfmxLog\ProductRecord\{0}\{1}\{2}", _Order, _Product, _No);
            if (!System.IO.Directory.Exists(address))
            {
                System.IO.Directory.CreateDirectory(address);
            }
            //保存txt
            string _time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            string result1 = address + "\\" + "Record.txt";
            FileStream fs = new FileStream(result1, FileMode.Append);
            StreamWriter wr = null;
            try
            {
                wr = new StreamWriter(fs);
                wr.BaseStream.Seek(0, SeekOrigin.End);
                wr.Write(_time + "    "
                         + _action + "    "
                         + _event
                         + "\n");
                wr.Flush();
                wr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                wr.Flush();
                wr.Close();
                fs.Close();
                LogHelper.WriteLog(typeof(OverAll), ex);
            }
        }
        /// <summary>
        /// 系统报警记录
        /// </summary>
        public static void AlarmRecordToDoc(string EqiupType, VariantCollection AlarmInfo)
        {
            if (AlarmInfo[6].ToString() != "1000")
            {
                return;
            }

            string nianyuetime = DateTime.Now.ToString("yyyy.MM");
            string ritime = DateTime.Now.ToString("dd");
            //string fenmiaotime = DateTime.Now.ToString("mm.ss");

            //判断路径是否存在
            string address1 = string.Format(@"C:\RfmxLog\AlarmLog\{0}\{1}\{2}", EqiupType, nianyuetime, ritime + "日");
            if (!System.IO.Directory.Exists(address1))
            {
                System.IO.Directory.CreateDirectory(address1);
            }

            //保存txt
            string _time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            string result1 = address1 + "\\" + ritime + ".txt";
            FileStream fs = new FileStream(result1, FileMode.Append);
            StreamWriter wr = null;
            string action = "";
            foreach (Variant Info in AlarmInfo)
            {
                action = action + Info.ToString() + "  ";
            }
            try
            {
                wr = new StreamWriter(fs);
                wr.BaseStream.Seek(0, SeekOrigin.End);
                wr.Write(_time + "    "
                         + action
                         + "\n");
                wr.Flush();
                wr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                wr.Flush();
                wr.Close();
                fs.Close();
            }
        }


        public static void DebugLogState(string _action, string _event)
        {


            string nianyuetime = DateTime.Now.ToString("yyyy.MM");
            string ritime = DateTime.Now.ToString("dd");

            //判断路径是否存在
            string address = string.Format(@"C:\RfmxLog\LogState\{0}\{1}", nianyuetime, ritime);
            if (!System.IO.Directory.Exists(address))
            {
                System.IO.Directory.CreateDirectory(address);
            }
            //保存txt
            string _time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            string result1 = address + "\\" + "Record.txt";
            FileStream fs = new FileStream(result1, FileMode.Append);
            StreamWriter wr = null;
            try
            {
                wr = new StreamWriter(fs);
                wr.BaseStream.Seek(0, SeekOrigin.End);
                wr.Write(_time + "    "
                         + _action + "    "
                         + _event
                         + "\n");
                wr.Flush();
                wr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                wr.Flush();
                wr.Close();
                fs.Close();
                LogHelper.WriteLog(typeof(OverAll), ex);
            }
        }

        public static int GetWeekOfYear(DateTime TimeValue)
        {
            int NewYear = Convert.ToInt32(TimeValue.Year);
            int WeekNow = Convert.ToInt32(TimeValue.DayOfWeek);
            int dayDiff = (-1 * WeekNow);
            int days = TimeValue.AddDays(dayDiff).DayOfYear;
            int oldYear = Convert.ToInt32(TimeValue.AddDays(dayDiff).Year);
            if (NewYear == oldYear)
            {
                int week = days / 7;
                if (days % 7 != 0)
                {
                    week++;
                }
                return week + 1;
            }
            else
            {
                return 1;
            }
        }

        public static void ActionRecord(string action)
        {



            string nianyuetime = DateTime.Now.ToString("yyyy.MM");
            string ritime = DateTime.Now.ToString("dd");

            //判断路径是否存在
            string address = string.Format(@"C:\RfmxLog\ActionLog\{0}", nianyuetime);
            if (!System.IO.Directory.Exists(address))
            {
                System.IO.Directory.CreateDirectory(address);
            }

            //保存txt
            string _time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            string result1 = address + "\\" + ritime + ".txt";
            FileStream fs = new FileStream(result1, FileMode.Append);
            StreamWriter wr = null;
            string msgs = null;

            try
            {
                wr = new StreamWriter(fs);
                wr.BaseStream.Seek(0, SeekOrigin.End);
                wr.Write(_time + "    "
                         + action + "    " + "\n");
                wr.Flush();
                wr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OverAll), ex);
                wr.Flush();
                wr.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// x-y格式工序号翻译成中文
        /// </summary>
        public static string ProcessTransition(string sNextProcess) {

            string ProcessState = "";
            string[] ProcessList = sNextProcess.Split('-');
            if (sNextProcess == "")
                return "手动模式锁定";
            switch (ProcessList[1]) {
                case "J":
                    ProcessState = "待加工";
                    break;
                case "Q":
                    ProcessState = "待清洗";
                    break;
                case "C":
                    ProcessState = "待测量";
                    break;
                case "F":
                    ProcessState = "待翻面";
                    break;
                case "L":
                    ProcessState = "待清理";
                    break;
                case "A":
                    ProcessState = "待重装";
                    break;
                case "Z":
                    ProcessState = "待中断";
                    break;
                default:
                    ProcessState = "解析异常";
                    break;

            }
            
            return ProcessState;
        }


        public static string AgvName2Pos(string _PosName) {

            string PosId = "";
            switch (_PosName) {
                case "1#待加工缓存位":
                    PosId = "089244AA080024";
                    break;
                case "1#成品缓存位":
                    PosId = "093438AA080040";
                    break;
                case "2#待加工缓存位":
                    PosId = "097234AA080045";
                    break;
                case "2#成品缓存位":
                    PosId = "101444AA080054";
                    break;
                case "托盘库缓存位":
                    PosId = "084080AA076594";
                    break;
                case "1#加工设备缓存位":
                    PosId = "091200AA076595";
                    break;
                case "2#加工设备缓存位":
                    PosId = "099188AA076606";
                    break;

            }

            return PosId;
        }
        public static int AgvPos2Name(string _Position)
        {

            int EquipId = 0;
            switch (_Position)
            {
                case "089244AA080024":
                    EquipId = 41;
                    break;
                case "093438AA080040":
                    EquipId = 42;
                    break;
                case "097234AA080045":
                    EquipId = 43;
                    break;
                case "101444AA080054":
                    EquipId = 44;
                    break;
                case "084080AA076594":
                    EquipId = 23;
                    break;
                case "091200AA076595":
                    EquipId = 53;
                    break;
                case "099188AA076606":
                    EquipId = 54;
                    break;

            }

            return EquipId;
        }
    }


}
