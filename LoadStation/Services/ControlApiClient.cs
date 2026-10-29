using LoadStation.Sql.pojo;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace LoadStation.Services
{
    /// <summary>
    /// 总控装卸站 Web REST API 客户端 (基于 8080 端口与 station_task 状态机)
    /// </summary>
    internal static class ControlApiClient
    {
        private const int DEFAULT_TIMEOUT_MS = 5000;

        /// <summary>
        /// 是否启用总控 Web API (station_task) 流程；若为 false 则降级使用旧 TCP + 直写数据库模式
        /// </summary>
        public static bool UseWebStationTaskApi
        {
            get
            {
                try
                {
                    string val = ConfigurationManager.AppSettings["UseWebStationTaskApi"];
                    if (string.IsNullOrWhiteSpace(val)) return true;
                    return bool.TryParse(val, out bool res) ? res : true;
                }
                catch
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 总控 Web API 基地址（例如 http://localhost:8080）
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                try
                {
                    string url = ConfigurationManager.AppSettings["ControlApiUrl"];
                    if (!string.IsNullOrWhiteSpace(url)) return url.TrimEnd('/');
                }
                catch { }
                return "http://localhost:8080";
            }
        }

        private static RestClient CreateClient()
        {
            var client = new RestClient(BaseUrl)
            {
                Timeout = DEFAULT_TIMEOUT_MS
            };
            return client;
        }

        /// <summary>
        /// 创建或恢复装卸站任务 (POST /api/station/create-load-task)
        /// </summary>
        public static async Task<ApiResult<StationTaskDto>> CreateStationTaskAsync(string orderNum, string taskType = "装", int trayNo = 0)
        {
            try
            {
                var client = CreateClient();
                var request = new RestRequest("api/station/create-load-task", Method.POST);
                request.AddJsonBody(new
                {
                    orderNum = orderNum,
                    taskType = taskType,
                    tray_no = trayNo
                });

                var response = await client.ExecuteAsync(request);
                return ParseResponse<StationTaskDto>(response);
            }
            catch (Exception ex)
            {
                return ApiResult<StationTaskDto>.Fail($"请求创建装卸站任务异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 呼叫托盘入站 (POST /api/station/in-station)
        /// </summary>
        public static async Task<ApiResult<StationTaskDto>> InStationAsync(long taskId, int trayNo, string fixtureNo)
        {
            try
            {
                var client = CreateClient();
                var request = new RestRequest("api/station/in-station", Method.POST);
                request.AddJsonBody(new
                {
                    id = taskId,
                    tray_no = trayNo,
                    fixture_no = fixtureNo
                });

                var response = await client.ExecuteAsync(request);
                return ParseResponse<StationTaskDto>(response);
            }
            catch (Exception ex)
            {
                return ApiResult<StationTaskDto>.Fail($"呼叫托盘入站异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 工件装夹完成出站 (POST /api/station/out-station)
        /// </summary>
        public static async Task<ApiResult<StationTaskDto>> OutStationAsync(long taskId, int trayNo, string productNo)
        {
            try
            {
                var client = CreateClient();
                var request = new RestRequest("api/station/out-station", Method.POST);
                request.AddJsonBody(new
                {
                    id = taskId,
                    tray_no = trayNo,
                    product_no = productNo
                });

                var response = await client.ExecuteAsync(request);
                return ParseResponse<StationTaskDto>(response);
            }
            catch (Exception ex)
            {
                return ApiResult<StationTaskDto>.Fail($"工件出站异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 取消尚未入站的任务 (POST /api/station/cancel-station-task)
        /// </summary>
        public static async Task<ApiResult<StationTaskDto>> CancelStationTaskAsync(long taskId)
        {
            try
            {
                var client = CreateClient();
                var request = new RestRequest("api/station/cancel-station-task", Method.POST);
                request.AddJsonBody(new
                {
                    id = taskId
                });

                var response = await client.ExecuteAsync(request);
                return ParseResponse<StationTaskDto>(response);
            }
            catch (Exception ex)
            {
                return ApiResult<StationTaskDto>.Fail($"取消装卸任务异常: {ex.Message}");
            }
        }

        private static ApiResult<T> ParseResponse<T>(IRestResponse response)
        {
            if (response == null)
            {
                return ApiResult<T>.Fail("网络通信失败，未收到总控响应！");
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return ApiResult<T>.Fail("总控 HTTP 接口路径不存在，请检查总控是否启动！");
            }

            if (string.IsNullOrWhiteSpace(response.Content))
            {
                return ApiResult<T>.Fail($"总控服务未返回有效内容 (HTTP {(int)response.StatusCode})");
            }

            try
            {
                var result = JsonConvert.DeserializeObject<ApiResult<T>>(response.Content);
                if (result == null)
                {
                    return ApiResult<T>.Fail("解析总控响应 JSON 失败！");
                }
                return result;
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Fail($"解析总控响应异常: {ex.Message}，原始内容: {response.Content}");
            }
        }
    }

    /// <summary>
    /// 装卸站任务 DTO
    /// </summary>
    internal class StationTaskDto
    {
        public long id { get; set; }
        public string task_type { get; set; }
        public string task_status { get; set; }
        public string part_no { get; set; }
        public string order_no { get; set; }
        public string product_no { get; set; }
        public int tray_no { get; set; }
        public string fixture_no { get; set; }
        public string sub_task { get; set; }
        public string sub_task_status { get; set; }
        public string warning_msg { get; set; }
        public List<Tuo> available_trays { get; set; }
    }

    /// <summary>
    /// 统一 API 响应格式 (对应总控 VueModel)
    /// </summary>
    internal class ApiResult<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }

        public bool Success => code == 200;

        public static ApiResult<T> Fail(string errorMsg)
        {
            return new ApiResult<T>
            {
                code = 500,
                message = errorMsg,
                data = default
            };
        }
    }
}
