using LoadStation.Sql.pojo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoadStation.SQL.SqlSugarHelper;

namespace LoadStation.SQL
{
    internal class TuoDao
    {
        public static List<Tuo> QueryTrayInfoByVisible(int visible)
        {
            return Db.Queryable<Tuo>().Where(it => it.visible == visible).ToList();
        }
    }
}
