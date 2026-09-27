using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace LoadStation.Sql.pojo
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("rgvtasklist")]
    public partial class Rgvtasklist
    {
           public Rgvtasklist(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int PriorId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? Initial {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? Target {get;set;}

           /// <summary>
           /// Desc:
           /// Default:0
           /// Nullable:True
           /// </summary>           
           public int? TrayId {get;set;}

    }
}
