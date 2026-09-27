using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;


namespace Control
{

    class AgvTask
    {
        const string ip = "192.168.7.45";

        /// <summary>
        /// 获取位置
        /// </summary>
        /// <param name="id">设备db_ID</param>
        /// <returns></returns>
        public static string getLocation(int id)
        {
            switch (id) {
                case 51:return "004225RF003826";
                case 52:return "004225RF003826";
                case 53:return "004225RF003826";
                case 54:return "004225RF003826";
                case 60:return "004225RF003826";
                case 61:return "004225RF003826";
                case 62:return "004225RF003826";
                case 63:return "004225RF003826";
                case 64:return "004225RF003826";
                case 65:return "004225RF003826";
                case 66:return "004225RF003826";
                case 80:return "004225RF003826";
                default:
                    return "004225RF003826";
            }


        }
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="initial">初始位置ID</param>
        /// <param name="target">目标位置ID</param>
        /// <returns></returns>
        public static string genAgvSchedulingTask( int initial,int target)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["reqCode"] = Guid.NewGuid().ToString();
            jo["reqTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //任务类型
            jo["taskTyp"] = "F01";

            //工作位，一般为机台或工作台位置
            jo["wbCode"] = "";

            //生成集合[]
            JArray pathList = new JArray();

            JObject l = new JObject();
                //位置编号
                l["positionCode"] = getLocation(initial);
                //位置编号
                l["type"] = "00"; 
            JObject l1 = new JObject();
                //位置编号
                l["positionCode"] = getLocation(target);
                //位置编号
                l["type"] = "00";
                pathList.Add(l1);
            jo["positionCodePath"] = pathList;

            //货架编号，
            // jo["podCode"] = podCode;
            //jo["podDir"] = podDir;
            //货架类型
            //jo["podTyp"] = podTyp;
            //agv编号
            //jo["agvCode"] = agvCode;


            //生成json语句
            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }
       private static string genAgvSchedulingTask( string taskTyp, string ctnrTyp, string ctnrCode, List<Dictionary<string, string>> positionCodePath, string podTyp, string agvCode)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["reqCode"] = Guid.NewGuid().ToString();
            jo["reqTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //任务类型
            jo["taskTyp"] = taskTyp;

            //叉车类型
            if (!"F01".Equals(taskTyp))
            {
                //容器类型（叉车专用）
                jo["ctnrTyp"] = ctnrTyp;
                //容器编号（叉车专用）
                jo["ctnrCode"] = ctnrCode;
            }
            //工作位，一般为机台或工作台位置
            jo["wbCode"] = "";

            //生成集合[]
            JArray pathList = new JArray();
            foreach (Dictionary<string, string> Path in positionCodePath)
            {

                JObject l = new JObject();
                //位置编号
                l["positionCode"] = Path["positionCode"];
                //位置编号
                l["type"] = Path["type"];
                pathList.Add(l);

            }
            jo["positionCodePath"] = pathList;

            //货架编号，
            // jo["podCode"] = podCode;
            //jo["podDir"] = podDir;
            //货架类型
            jo["podTyp"] = podTyp;
            //agv编号
            jo["agvCode"] = agvCode;


            //生成json语句
            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }

        /// <summary>
        /// 继续任务
        /// </summary>
        /// <param name="wbCode">工作位</param>
        /// <param name="podCode">货架号</param>
        /// <param name="agvCode">AGV 编号</param>
        /// <param name="taskCode">任务单编号</param>
        /// <returns></returns>
        public static string continueTask(string wbCode, string podCode, string agvCode, string taskCode)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["reqCode"] = Guid.NewGuid().ToString();
            jo["reqTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //工作位，与RCS-LITE 端配置的位置名称一致。
            jo["wbCode"] = wbCode;
            //货架号，采用货架号触发的方式
            jo["podCode"] = podCode;
            //AGV 编号，采用 AGV 编号触发的方式。
            jo["agvCode"] = agvCode;
            //任务单编号
            jo["taskCode"] = taskCode;
            //生成json语句
            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }


        //取消任务
        public static string cancelTask(string agvCode, string taskCode)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["reqCode"] = Guid.NewGuid().ToString();
            jo["reqTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //取消该AGV 正在执行的任务单
            jo["agvCode"] = agvCode;
            //任务单编号, 取消该任务单
            jo["taskCode"] = taskCode;
            //生成json语句
            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }

        //查询任务状态
        public static string queryTaskStatus(string clientCodek, string tokenCode, List<string> taskCodeList, string agvCode)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["reqCode"] = Guid.NewGuid().ToString();
            jo["reqTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //客户端编号
            jo["clientCode"] = clientCodek;
            //令牌号
            jo["tokenCode"] = tokenCode;

            JArray pathList = new JArray();
            foreach (string taskCode in taskCodeList)
            {
                pathList.Add(taskCode);
            }
            // pathList.Add("61B5EFE3-9288-46DA-BE6C-A530738DD2D8_ECS");
            //任务单编号数组
            jo["taskCode"] = pathList;
            //AGV 编号
            jo["agvCode"] = agvCode;



            //生成json语句
            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }

        //货架与位置绑定、解绑
        public static string bindPodAndBerth(string podCode, string positionCode, string podDir, string indBind)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["reqCode"] = Guid.NewGuid().ToString();
            jo["reqTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //货架编号
            jo["podCode"] = podCode;
            //位置编号
            jo["positionCode"] = positionCode;
            //货架方向 “0”：横向，“1”：纵向
            jo["podDir"] = podDir;
            //"1"：绑定， "0"：解绑
            jo["indBind"] = indBind;
            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }
        //任务执行通知
        public static string agvCallback(string cooX, string cooY, string currentPositionCode, string mapCode, string mapDataCode, string method, string podCode, string podDir, string robotCode, string taskCode, string wbCode)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["reqCode"] = Guid.NewGuid().ToString();
            jo["reqTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //地码 X 坐标(mm)：任务完成时有值
            jo["cooX"] = cooX;
            //地码 Y 坐标(mm)：任务完成时有值
            jo["cooY"] = cooY;
            //当前位置编号
            //任务开始：该位置为任务起点
            //走出储位：该位置为任务起点
            //任务单取消：该位置为工作位编号
            //任务结束：该位置为任务终点
            jo["currentPositionCode"] = currentPositionCode;
            //地图编号
            jo["mapCode"] = mapCode;
            //地码编号：任务完成时有值
            jo["mapDataCode"] = mapDataCode;
            /*   方法名, 可使用任务类型做为方法名
                 由RCS - Lite 任务模板配置后并告知
                 上层系统
                 默认使用方式:
                 start: 任务开始
                 outbin : 走出储位
                 end : 任务结束
                 cancel : 任务单取消*/
            jo["method"] = method;
            //货架编号：背货架时有值
            jo["podCode"] = podCode;
            //“180”,”0”,”90”,”-90” 分别对应地图的” 左”,” 右”,”上”,”下”：
            jo["podDir"] = podDir;
            //AGV 编号
            jo["robotCode"] = robotCode;
            //当前任务单号
            jo["taskCode"] = taskCode;
            //工作位
            jo["wbCode"] = wbCode;

            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }
        //释放AGV
        public static string freeRobot(string robotCount, string mapShortName, string[] robots)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["reqCode"] = Guid.NewGuid().ToString();
            jo["reqTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //释放的机器人数，-1 表示所有机器人
            jo["robotCount"] = robotCount;
            //AGV 所在地图的简称
            jo["mapShortName"] = mapShortName;
            //具体机器人编号列表
            JArray pathList = new JArray();
            if (robots != null)
                foreach (string robot in robots)
                {
                    pathList.Add(robot);
                }
            jo["robots"] = pathList;

            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }

        //巷道与容器绑定、解绑
        public static string bindCtnrAndTunnel(string robotCount, string mapShortName, string tunnelCode, string indBind)
        {
            //生成请求体
            JObject jo = new JObject();
            jo["reqCode"] = Guid.NewGuid().ToString();
            jo["reqTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //客户端编号
            jo["clientCode"] = robotCount;
            //令牌号,
            jo["tokenCode"] = mapShortName;
            //巷道编号
            jo["tunnelCode"] = tunnelCode;
            //"1"：绑定， "0"：解绑
            jo["indBind"] = indBind;

            string sendData = JsonConvert.SerializeObject(jo);
            return sendData;
        }

        //发送请求
        public static void reqHttp(string uri, string json)
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
            string url = "http://" + ip + ":8182/" + uri;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //string data = "{ \"operation\": \"get_value\",\"items\": [{\"path\": \"/MACHINE/CONTROLLER/WARNING\"}}";
            //获取请求语句
            string data = json;
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;
            using (Stream requestBody = request.GetRequestStream())
            {
                //发送
                requestBody.Write(byteData, 0, byteData.Length);
                //Thread.Sleep(1000);

            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        //获取返回值
                        string responseData = streamReader.ReadToEnd();

                        Console.WriteLine("返回值:" + responseData);

                    }
                }
            }
        }





    }

}
