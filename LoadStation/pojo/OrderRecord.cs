using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace LoadStation.Sql.pojo
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("order_record")]
    public partial class OrderRecord
    {
        public OrderRecord()
        {


        }
        public OrderRecord(Tuo tuo)
        {
            orderId = tuo.OrderId;
            chanType = tuo.chanType;
            chanNo = tuo.chanNo;
            task = tuo.task;
            chanState = tuo.chanState;
            tuoId = tuo.tuoId;
            updateTime = DateTime.Now;
        }
        /// <summary>
        /// Desc:主键
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true)]
        public int id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string orderId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string chanType { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string chanNo { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string task { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string chanState { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? tuoId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? updateTime { get; set; }

    }
}
