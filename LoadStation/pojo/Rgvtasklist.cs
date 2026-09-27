using SqlSugar;

namespace LoadStation.Sql.pojo
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("rgvtasklist")]
    public partial class Rgvtasklist
    {
        public Rgvtasklist()
        {
            
        }

        public Rgvtasklist(int initial, int target, int trayId, int task, int flag, string status)
        {
            Initial = initial;
            Target = target;
            TrayId = trayId;
            Task = task;
            Flag = flag;
            Status = status;
        }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int PriorId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int Initial { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int Target { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:True
        /// </summary>           
        public int TrayId { get; set; }
        public int Task { get; set; }
        public int Flag { get; set; }

        public string Status { get; set; }

    }
}
