using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control
{
    class AddressDic
    {
        public const int EQUIP_CNC = 1;
        public const int EQUIP_RGV = 3;
        public const int EQUIP_PLC = 2;
        public const int EQUIP_Wash = 4;
        public const string EQUIP_RGV_Parameter = "/MACHINE/CONTROLLER/PARAMETER";
        public const string EQUIP_RGV_Reg = "/MACHINE/CONTROLLER/VARIABLE@REG_X";
        public const string EQUIP_RGV_Macro = "/MACHINE/CONTROLLER/VARIABLE@MACRO";
        public static object obtainAddress(int equip, String nodeName)
        {
            object address = "";
            if (equip == 1)
            {
                switch (nodeName)
                {


                    case "装载程序":
                        address = "/Plc/DB19.DBX13.6";
                        break;
                    case "执行程序":
                        address = "/Plc/DB21.DBX7.1";
                        break;
                    case "机床在线程序运行结束":
                        address = "/Plc/DB631.DBX0.5";
                        break;

                    case "托盘交换门关闭-控制":
                        address = "/Plc/DB631.DBX0.7";
                        break;
                    case "托盘交换门关闭-状态":
                        address = "/Plc/I46.0";
                        break;

                    case "托盘零点锁紧-控制":
                        address = "/Plc/DB631.DBX1.1";
                        break;
                    case "托盘零点锁紧-状态":
                        address = "/Plc/I46.0";
                        break;
                    case "零点吹气打开":
                        address = "/Plc/DB631.DBX1.2";
                        break;
                    case "零点吹气关闭":
                        address = "/Plc/DB631.DBX1.3";
                        break;

                    case "机床上下料准备":
                        address = "/Plc/DB631.DBX0.0";
                        break;
                    case "机床上下料开始":
                        address = "/Plc/DB631.DBX0.1";
                        break;
                    case "机床上下料完成":
                        address = "/Plc/DB631.DBX0.3";
                        break;

                    case "机床处于联机状态":
                        address = "/Plc/DB97.DBX4.5";
                        break;
                    case "机床可以进行托盘交换":
                        address = "/Plc/DB97.DBX8.0";
                        break;
                    case "主轴停止状态":
                        address = "/Plc/DB97.DBX6.0";
                        break;
                    case "机床有相关报警":
                        address = "/Plc/DB97.DBX8.1";
                        break;
                    case "托盘松开":
                        address = "/Plc/DB97.DBX8.6";
                        break;
                    case "托盘到达交换点位置":
                        address = "/Plc/DB97.DBX9.0";
                        break;
                    case "命令托盘交换门开启":
                        address = "/Plc/DB98.DBX2.0";
                        break;
                    case "命令托盘交换门关闭":
                        address = "/Plc/DB98.DBX2.1";
                        break;
                    case "零点定位吹气开":
                        address = "/Plc/DB98.DBX108.4";
                        break;
                    case "零点定位吹气关":
                        address = "/Plc/DB98.DBX108.5";
                        break;
                    case "穿梭车正常":
                        address = "/Plc/DB98.DBX108.3";
                        break;
                    case "托盘交换中":
                        address = "/Plc/DB98.DBX108.1";
                        break;
                    case "负载检测开关":
                        address = "/NC/_N_NC_GD4_ACX/LOADSWITCH[1]";
                        break;
                    case "系统复位":
                        address = "/Plc/DB21.DBX7.7";
                        break;
                    case "系统报警":
                        address = "/Plc/M1202.6 ";
                        break;
                    case "报警灯_黄":
                        address = "/Plc/Q49.0";
                        break;
                    case "报警灯_绿":
                        address = "/Plc/Q49.1";
                        break;
                    case "报警灯_红":
                        address = "/Plc/Q49.2";
                        break;
                    case "主轴旋转状态":
                        address = "/Plc/DB97.DBX6.0";
                        break;
                }
            }
            else if (equip == 2)//PLC
            {
                switch (nodeName)
                {
                    #region  托盘进出机床
                    case "1#托盘进出机床":
                        address = "\"1#交换台DB\".\"完整动作\"";
                        break;
                    case "2#托盘进出机床":
                        address = "\"2#交换台DB\".\"完整动作\"";
                        break;
                    case "1#叉臂动作完成":
                        address = "\"1#交换台DB\".\"完整动作结束\"";
                        break;
                    case "2#叉臂动作完成":
                        address = "\"2#交换台DB\".\"完整动作结束\"";
                        break;
                    case "1#零点吹气":
                        address = "\"上位机处理\".\"1#吹气\"";
                        break;
                    case "2#零点吹气":
                        address = "\"上位机处理\".\"2#吹气\"";
                        break;
                    #endregion 
                  

                }
            }
            else if (equip == 3)//RGV
            {
                switch (nodeName)
                {
                    case "初始位置":
                        address = 700;
                        break;
                    case "目标位置":
                        address = 701;
                        break;
                    case "执行":
                        address = 190001;
                        break;
                    case "状态字段":
                        address = 750;
                        break;
                    case "拿地址":
                        address = 800;
                        break;
                    case "放地址":
                        address = 801;
                        break;

                }
            }
            else if (equip == 4)//清洗房
            {
                switch (nodeName)
                {
                    case "自动":
                        address = "7";
                        break;
                    case "门开":
                        address = "10";
                        break;
                    case "门关":
                        address = "5";
                        break;
                    case "一号工件":
                        address = "15";
                        break;
                    case "二号工件":
                        address = "3";
                        break;
                    case "三号工件":
                        address = "3";
                        break;
                    case "允许产品进入":
                        address = "\"软件通讯点\".\"清洗房已准备好\"";
                        break;
                    case "放零件完毕":
                        address = "\"软件通讯点\".\"上位机放零件完毕\""; ;
                        break;
                    case "允许产品取出":
                        address = "\"软件通讯点\".\"呼叫穿梭车取零件\"";
                        break;
                    case "零件取完毕":
                        address = "\"软件通讯点\".\"穿梭车零件取完毕\"";
                        break;
                    
                }
            }
            
            return address;
        }
    }
}
