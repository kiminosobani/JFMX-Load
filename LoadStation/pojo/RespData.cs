using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadStation.pojo
{
    internal class RespData
    {
        //保存的位置
        public string absolutePath { get; set; }
        //校验码
        public string SHA256 { get; set; }
        //文件名
        public string fileName { get; set; }
        //数据
        public byte[] data { get; set; }

        public RespData(string absolutePath)
        {
            this.absolutePath = absolutePath;
        }
    }
}
