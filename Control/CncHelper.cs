using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Control
{
    class CncHelper
    {
        //寄存器
        //生成请求参数 多个index
        private static string setReadRegisterParameter(List<int> values)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["operation"] = "get_value";

            JArray jr = new JArray();

            JObject j1 = new JObject();
            j1["path"] = "/MACHINE/CONTROLLER/VARIABLE@REG_X";

            //生成index集合
            JArray index = new JArray();
            for (int i = 0; i < values.Count; i++)
            {
                index.Add(values[i]);
            }
            j1["index"] = index;

            jr.Add(j1);
            jo["items"] = jr;

            //生成json语句
            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }

        //发出多个index请求并处理
        public static List<int> readRegisterValue(List<int> values)
        {
            /*
              {
                "operation": "get_value",
                "items": [
                {
                "path": "/MACHINE/CONTROLLER/VARIABLE@REG_X",
                "index": [0,1,2,3]
                }
                ]
                }
             */
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            //获取请求语句
            string data = setReadRegisterParameter(values);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                //发送
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        //获取返回值
                        string responseData = streamReader.ReadToEnd();
                        Console.WriteLine("读取寄存器的值:" + responseData);

                        JObject obj = JObject.Parse(responseData);
                        //获取返回值
                        JToken jr = obj["value"][0];

                        List<int> res = new List<int>();
                        for (int j = 0; j < jr.Count(); j++)
                        {
                            //添加至集合
                            res.Add((int)jr[j]);
                        }
                        return res;
                    }
                }
            }
        }
        //寄存器
        //set 返回具有一个index和value的请求体
        public static string setWriteRegisterParameter(int position, int value)
        {
            /*{
            "operation": "set_value",
             "items": [
             {
             "path": "/MACHINE/CONTROLLER/VARIABLE@REG_X",
             "index":10,
             "value": 10
             }
             ]
            }*/
            JObject jo = new JObject();
            jo["operation"] = "set_value";

            JArray jr = new JArray();

            JObject j1 = new JObject();
            j1["path"] = "/MACHINE/CONTROLLER/VARIABLE@REG_X";
            j1["index"] = position;
            j1["value"] = value;

            jr.Add(j1);

            jo["items"] = jr;

            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }

        //set 写入寄存器
        public static void writeRegisterValue(int position, int value)
        {
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            string data = setWriteRegisterParameter(position, value);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();
                        Console.WriteLine("写入后的结果" + responseData);
                    }
                }
            }
        }


        //set 通道，寄存器，系统,宏变量都可以用这些index方法
        public static string setWriteIndexParameter(string address, int position, int value)
        {
            //这个方法只要参数里面有index
            JObject jo = new JObject();

            jo["operation"] = "set_value";
            JArray jr = new JArray();

            JObject j1 = new JObject();
            //j1["path"] = "/MACHINE/CONTROLLER/VARIABLE@REG_X";
            j1["path"] = address;
            j1["index"] = position;
            j1["value"] = value;

            jr.Add(j1);

            jo["items"] = jr;

            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }
        //set
        public static void writeIndex(string address, int position, int value)
        {
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            string data = setWriteIndexParameter(address, position, value);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();
                        Console.WriteLine("写入后的结果" + responseData);
                    }
                }
            }
        }


        //set：设置寄存器X下标为position的第offset位的值为value
        public static string setWriteIndexParameter(string address, int position, int offset, int value)
        {
            //这个方法只要参数里面有index
            JObject jo = new JObject();

            jo["operation"] = "set_value";
            JArray jr = new JArray();

            JObject j1 = new JObject();
            //j1["path"] = "/MACHINE/CONTROLLER/VARIABLE@REG_X";
            j1["path"] = address;
            j1["index"] = position;
            j1["offset"] = offset;
            j1["value"] = value;

            jr.Add(j1);

            jo["items"] = jr;

            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }
        //set：设置寄存器X下标为position的第offset位的值为value
        public static void writeIndexAndOffset(string address, int position, int offset, int value)
        {
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            string data = setWriteIndexParameter(address, position, offset, value);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();
                        Console.WriteLine("写入后的结果" + responseData);
                    }
                }
            }
        }

        public static string setReadIndexParameter(string address, int position)
        {
            //这个方法只要参数里面有index
            JObject jo = new JObject();

            jo["operation"] = "get_value";
            JArray jr = new JArray();

            JObject j1 = new JObject();
            //j1["path"] = "/MACHINE/CONTROLLER/VARIABLE@REG_X";
            j1["path"] = address;
            j1["index"] = position;

            jr.Add(j1);

            jo["items"] = jr;

            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }

        public static List<int> readIndex(string address, int position)
        {
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            string data = setReadIndexParameter(address, position);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();

                        Console.WriteLine("读取出值:" + responseData);

                        JObject obj = JObject.Parse(responseData);
                        JToken jr = obj["value"][0];

                        List<int> res = new List<int>();
                        for (int j = 0; j < jr.Count(); j++)
                        {
                            string value = jr[j].ToString();
                            if (value == "")
                            {
                                res.Add(0);
                            }
                            else
                            {
                                res.Add(Convert.ToInt32(value));
                            }
                        }
                        return res;
                    }
                }
            }
        }
        //请求 无参数
        public static string setPathParameter(string address)
        {
            //这个方法只要只有path，没有index
            //报警，进给速率，主轴倍率，加工件数
            JObject jo = new JObject();

            jo["operation"] = "get_value";
            JArray jr = new JArray();

            JObject j1 = new JObject();
            //j1["path"] = "/MACHINE/CONTROLLER/VARIABLE@REG_X";
            j1["path"] = address;
            jr.Add(j1);

            jo["items"] = jr;

            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }
        //获取报警信息
        public static Dictionary<string, string> readAlarm()
        {
            //读取报警信息
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            string data = setPathParameter("/MACHINE/CONTROLLER/WARNING");
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();

                        Console.WriteLine("读取报警值:" + responseData);

                        JObject obj = JObject.Parse(responseData);
                        JToken jr = obj["value"][0][0];

                        Dictionary<string, string> dicInfo = new Dictionary<string, string>();
                        for (int j = 0; j < jr.Count(); j++)
                        {
                            string number = (string)jr[j]["number"];
                            string text = (string)jr[j]["text"];
                            if (!dicInfo.ContainsKey(number))
                            {
                                dicInfo.Add(number, text);
                            }
                        }
                        return dicInfo;
                    }
                }
            }
        }


        public static string write(string data)
        {
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";

            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();
                        Console.WriteLine("写入后的结果" + responseData);
                        return responseData;
                    }

                }
            }
        }

        //获取程序名
        public static string readProName()
        {
            //程序名
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            string data = setPathParameter("/MACHINE/CONTROLLER/PROGRAM");
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();

                        Console.WriteLine("读取程序名:" + responseData);

                        JObject obj = JObject.Parse(responseData);
                        JToken jr = obj["value"][0][0];

                        return (string)jr;


                    }
                }
            }
        }

        public static List<double> readDoubleIndex(string address, int position)
        {
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            string data = setReadIndexParameter(address, position);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();

                        Console.WriteLine("读取出值:" + responseData);

                        JObject obj = JObject.Parse(responseData);
                        JToken jr = obj["value"][0];

                        List<double> res = new List<double>();
                        for (int j = 0; j < jr.Count(); j++)
                        {
                            res.Add((double)jr[j]);
                        }
                        return res;
                    }
                }
            }
        }

        public static List<double> readDoublePath(string address)
        {
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            string data = setPathParameter(address);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();

                        Console.WriteLine("读取出值:" + responseData);

                        JObject obj = JObject.Parse(responseData);
                        JToken jr = obj["value"][0];

                        List<double> res = new List<double>();
                        for (int j = 0; j < jr.Count(); j++)
                        {
                            res.Add((double)jr[j]);
                        }
                        return res;
                    }
                }
            }
        }

        public static List<int> readIntPath(string address)
        {
            string url = "http://10.50.42.41:19001/v1/1C4817A70E70957/data/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            string data = setPathParameter(address);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteData, 0, byteData.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseData = streamReader.ReadToEnd();

                        Console.WriteLine("读取出值:" + responseData);

                        JObject obj = JObject.Parse(responseData);
                        JToken jr = obj["value"][0];

                        List<int> res = new List<int>();
                        for (int j = 0; j < jr.Count(); j++)
                        {
                            res.Add((int)jr[j]);
                        }
                        return res;
                    }
                }
            }
        }
    }
}
