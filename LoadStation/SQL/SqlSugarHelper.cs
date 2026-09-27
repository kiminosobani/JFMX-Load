using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadStation.SQL
{
    internal class SqlSugarHelper
    {
        //连接字符串
        //private const string CONNECTION_STRING = "server=128.2.10.21;Database=rh_fms1001;Uid=root;Pwd=Rifa1234.";
        private const string CONNECTION_STRING = "server=127.0.0.1;Database=rh_fms1001;Uid=root;Pwd=root";
        //生成的实体类存放路径
        //TODO 生成实体类路径修改
        private const string ENTITY_PATH = @"E:\project\mcm\RH-FMS1001-装卸站\LoadStation\pojo";
        //命名空间
        private const string ENTITY_NAMESPACE = "LoadStation.Sql.pojo";
        //单例数据库操作对象
        public readonly static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
        {
            ConnectionString = CONNECTION_STRING,//连接符字串
            DbType = SqlSugar.DbType.MySql,//数据库类型
            IsAutoCloseConnection = true //不设成true要手动close
        },
        db =>
        {
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                //获取原生SQL推荐 5.1.4.63  性能OK
                Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));
            };
        });
        public static SqlSugarClient GetInstance()
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = CONNECTION_STRING,//连接符字串
                DbType = SqlSugar.DbType.MySql,//数据库类型
                IsAutoCloseConnection = true,
            },
            db =>
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    //获取原生SQL推荐 5.1.4.63  性能OK
                    Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));
                };
            });
        }
        //创建实体类
        public static void CreateEntity(string tableName)
        {
            Db.DbFirst.IsCreateAttribute().Where(tableName).FormatClassName(x => ToPascal(x)).CreateClassFile(ENTITY_PATH, ENTITY_NAMESPACE);
            MessageBox.Show("生成完成");
        }
        //字符串转大驼峰命名
        private static string ToPascal(string str)
        {
            string[] split = str.Split(new char[] { '/', ' ', '_', '.' });
            string newStr = "";
            foreach (var item in split)
            {
                char[] chars = item.ToCharArray();
                chars[0] = char.ToUpper(chars[0]);
                for (int i = 1; i < chars.Length; i++)
                {
                    chars[i] = char.ToLower(chars[i]);
                }
                newStr += new string(chars);
            }
            return newStr;
        }
        //写锁操作，插入数据时禁止读
        //public static bool WriteLock(string tableName, SqlSugarScope db,Action<SqlSugarScope> func)
        //{
        //    try
        //    {
        //        // 加表级写锁
        //        db.Ado.ExecuteCommand("LOCK TABLES "+tableName+" WRITE");
        //        // 插入数据操作
        //        func(db);
        //        // 解锁表
        //        db.Ado.ExecuteCommand("UNLOCK TABLES");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("SqlSugarHelper.writeLock出现异常: " + ex.Message);
        //    }
        //    return true;
        //}
    }
}
