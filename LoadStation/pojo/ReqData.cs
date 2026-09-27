using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadStation.pojo
{
    internal class ReqData
    {
        //文件名
        public String name { get; set; }
        //上传或下载路径
        public String path { get; set; }
        //请求类型，是保存txt还是请求加工文件
        public String type { get; set; }
        //txt写入的内容
        public String txt { get; set; }
        //保存的文件
        public byte[] data { get; set; }

        public ReqData(string path, string type, string txt)
        {
            this.path = path;
            this.type = type;
            this.txt = txt;
        }

        public ReqData(string path, byte[] data)
        {
            this.path = path;
            this.data = data;
        }

        public ReqData(string path)
        {
            this.path = path;
        }
    }
}
