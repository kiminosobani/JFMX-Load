using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Control
{
    internal class AgvHelper
    {
        const string prefix = "http://10.50.42.41:8182";
        const string taskTyp = "F01";
        const string type = "00";
        //生成任务单
        public static string genAgvSchedulingTask(string reqCode, string from, string to)
        {
            JArray positionCodePath = new JArray() {
                new JObject
                {
                    { "positionCode", from },
                    { "type", type }
                },
                new JObject
                {
                    { "positionCode", to },
                    { "type", type }
                }
            };
            JObject rq = new JObject
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "taskTyp", taskTyp },
                { "positionCodePath", positionCodePath },
                { "taskCode",reqCode },
                { "agvCode", "10001" }
            };

            return JsonConvert.SerializeObject(rq);
        }
        public static string genAgvSchedulingTask1(string reqCode, string from, string to, string wbCode,
            string podCode, string podDir, string podTyp, string agvCode)
        {
            JArray positionCodePath = new JArray() {
                new JObject
                {
                    { "positionCode", from },
                    { "type", type }
                },
                new JObject
                {
                    { "positionCode", to },
                    { "type", type }
                }
            };
            JObject rq = new JObject
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "taskTyp", taskTyp },
                { "wbCode", wbCode },
                { "positionCodePath", positionCodePath },
                { "podCode", podCode },
                { "podDir", podDir },
                { "podTyp", podTyp },
                { "taskCode",reqCode },
                { "agvCode", agvCode }
            };

            return JsonConvert.SerializeObject(rq);
        }
        //继续执行任务
        public static string continueTask(string taskCode)
        {
            JObject rq = new JObject
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "taskCode", taskCode }
            };
            return JsonConvert.SerializeObject(rq);
        }
        public static string continueTask1(string taskCode, string wbCode, string podCode, string agvCode)
        {
            JObject rq = new JObject
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "wbCode", wbCode },
                { "podCode", podCode },
                { "agvCode", agvCode },
                { "taskCode", taskCode }
            };
            return JsonConvert.SerializeObject(rq);
        }
        //取消任务
        public static string cancelTask(string taskCode)
        {
            JObject rq = new JObject
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "taskCode", taskCode }
            };
            return JsonConvert.SerializeObject(rq);
        }
        public static string cancelTask1(string taskCode, string agvCode)
        {
            JObject rq = new JObject
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "agvCode", agvCode },
                { "taskCode", taskCode }
            };
            return JsonConvert.SerializeObject(rq);
        }
        //货架与位置绑定、解绑
        public static string bindPodAndBerth(string podCode, string positionCode, string podDir, string indBind)
        {
            JObject rq = new JObject
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "podCode", podCode },
                { "positionCode", positionCode },
                { "podDir", podDir },
                { "indBind", indBind }
            };
            return JsonConvert.SerializeObject(rq);
        }
        //查询任务状态
        public static string queryTaskStatus(string clientCode, string tokenCode, string[] taskCodes, string agvCode)
        {
            JObject rq = new JObject
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "clientCode", clientCode },
                { "tokenCode", tokenCode },
                { "taskCodes", new JArray(taskCodes) },
                { "agvCode", agvCode }
            };
            return JsonConvert.SerializeObject(rq);
        }
        //释放AGV
        public static string freeRobot(string robotCount, string mapShortName, string[] robots)
        {
            JObject rq = new JObject()
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "robotCount", robotCount },
                { "mapShortName", mapShortName },
                { "robots", new JArray(robots) }
            };
            return JsonConvert.SerializeObject(rq);
        }
        //容器与仓位绑定、解绑
        public static string bindCtnrAndBin(string clientCode, string tokenCode, string ctnrCode, string ctnrTyp
            , string stgBinCode, string binName, string inBind)
        {
            JObject rq = new JObject()
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "clientCode", clientCode },
                { "tokenCode", tokenCode },
                { "ctnrCode", ctnrCode },
                { "ctnrTyp", ctnrTyp },
                { "stgBinCode", stgBinCode },
                { "binName", binName },
                { "inBind", inBind }
            };
            return JsonConvert.SerializeObject(rq);
        }
        //巷道与容器绑定、解绑
        public static string bindCtnrAndTunnel(string clientCode, string tokenCode, string tunnelCode, string inBind)
        {
            JObject rq = new JObject()
            {
                { "reqCode", Guid.NewGuid().ToString() },
                { "reqTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "clientCode", clientCode },
                { "tokenCode", tokenCode },
                { "tunnelCode", tunnelCode },
                { "inBind", inBind }
            };
            return JsonConvert.SerializeObject(rq);
        }
        //发送请求
        public static string HttpReq(string suffix, string reqData)
        {
            string url = prefix + suffix;
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(reqData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.SendAsync(request).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            return result;
        }
    }
}
