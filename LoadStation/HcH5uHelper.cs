using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace LoadStation
{
    public enum SoftElemType
    {
        //H5u
        REGI_H5U_Y = 0x30,       //Y元件的定义	
        REGI_H5U_X = 0x31,		//X元件的定义							
        REGI_H5U_S = 0x32,		//S元件的定义				
        REGI_H5U_M = 0x33,		//M元件的定义	
        REGI_H5U_B = 0x34,       //B元件的定义
        REGI_H5U_D = 0x35,       //D字元件的定义
        REGI_H5U_R = 0x36,       //R字元件的定义

    }
    public class HcH5uHelper
    {
        #region //标准库
        [DllImport("StandardModbusApi.dll", EntryPoint = "Init_ETH_String", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Init_ETH_String(string sIpAddr, int nNetId = 0, int IpPort = 502);

        [DllImport("StandardModbusApi.dll", EntryPoint = "Exit_ETH", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Exit_ETH(int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Write_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Write_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Read_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Read_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Read_Device_Block", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Read_Device_Block(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Write_Device_Block", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Write_Device_Block(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        #endregion
        public static bool Open()
        {
            return Init_ETH_String("10.50.42.24");
        }

        public static bool Close()
        {
            return Exit_ETH();
        }

        public static string Read(string startAddr)
        {
            byte[] pBuf = new byte[16000];
            int count = 2;
            SoftElemType ElemType = SoftElemType.REGI_H5U_D;
            string strData = "";
            int nRet = H5u_Read_Device_Block(ElemType, int.Parse(startAddr), count, pBuf);
            for (int i = 0; i < count / 2; i++)
            {
                byte[] databuf = new byte[4] { 0, 0, 0, 0 };
                databuf[0] = pBuf[i * 4];
                databuf[1] = pBuf[i * 4 + 1];
                databuf[2] = pBuf[i * 4 + 2];
                databuf[3] = pBuf[i * 4 + 3];
                float fTemp = BitConverter.ToSingle(databuf, 0);
                strData = strData + fTemp.ToString() + " ";
            }
            return strData;
        }

        public static bool Write(string startAddr,string value) {

            byte[] pBuf = new byte[16000];
            int nStartAddr = Convert.ToInt32(startAddr);
            int nCount = 1;
            bool bIsWord = false;//是否字元件

            SoftElemType ElemType = SoftElemType.REGI_H5U_D;
            

            string[] arr = value.Split(' ');

            GetDataFromUI(pBuf, arr, bIsWord, 0);
            int nRet = H5u_Write_Device_Block(ElemType, nStartAddr, nCount, pBuf);

            if (nRet != 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private static void GetDataFromUI(byte[] pBuf, string[] arr, bool bIsWord, int nDataType)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == string.Empty)
                {
                    break;
                }
                double nVal = Convert.ToDouble(arr[i]);
                if (bIsWord)
                {
                    if (nDataType == 0)//16位整形
                    {
                        int idata = Convert.ToInt16(arr[i]);
                        byte[] dataBuf = new byte[2] { 0, 0 };
                        dataBuf = BitConverter.GetBytes(idata);
                        pBuf[2 * i] = dataBuf[0];
                        pBuf[2 * i + 1] = dataBuf[1];
                    }
                    else if (nDataType == 1)//32位整形
                    {
                        int idata = Convert.ToInt32(arr[i]);
                        byte[] dataBuf = new byte[4] { 0, 0, 0, 0 };
                        dataBuf = BitConverter.GetBytes(idata);
                        pBuf[4 * i] = dataBuf[0];
                        pBuf[4 * i + 1] = dataBuf[1];
                        pBuf[4 * i + 2] = dataBuf[2];
                        pBuf[4 * i + 3] = dataBuf[3];
                    }
                    else if (nDataType == 2)//浮点数
                    {
                        float fdata = Convert.ToSingle(arr[i]);
                        byte[] dataBuf = new byte[4] { 0, 0, 0, 0 };
                        dataBuf = BitConverter.GetBytes(fdata);
                        pBuf[4 * i] = dataBuf[0];
                        pBuf[4 * i + 1] = dataBuf[1];
                        pBuf[4 * i + 2] = dataBuf[2];
                        pBuf[4 * i + 3] = dataBuf[3];
                    }
                }
                else
                {
                    pBuf[i] = (byte)nVal;
                }
            }
        }
    }
}
