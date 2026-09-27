using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;
namespace Control.DataBaseprogram
{
    class db_ProductDao:CDataBaseManage
    {
      
        public DataSet QueryProductInfo(string ProductName)
        {
            try
            {
                Connect();

                string strSQL = string.Format(@"Select * from product where chanName = '{0}'", ProductName);
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProductDao), ex);
                return null;
            }

        }
    }
}
