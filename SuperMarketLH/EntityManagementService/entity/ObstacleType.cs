using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
   public class ObstacleType
    {
       public const string ELEVATOR = "电梯";
       public const string ESCALATOR = "扶梯";
       public const string STAIRS = "楼梯";
       public const string FIRE_ENGINE_ACCESS = "消防通道";
       public const string DECORATE = "楼层装饰";
       public const string TOLITE = "厕所";
       public const string ATM = "ATM";
       public const string BABY_ROOM = "母婴室";
       public const string SMOKING_ROOM = "吸烟室";
       public const string CHECKSTAND = "收银台";
       public const string SERVICE= "服务台";
       public const string LOST_FOUND = "失物招领";
       public const string GOODS_STROAGE = "物品寄存";
       public const string SHOP = "商铺";
       public static List<string> Type
       {
           get {
               return new string[]{
                    SHOP,ELEVATOR,ESCALATOR,STAIRS,FIRE_ENGINE_ACCESS,DECORATE,TOLITE,ATM,BABY_ROOM,SMOKING_ROOM,CHECKSTAND,SERVICE,LOST_FOUND,
                    GOODS_STROAGE
                }.ToList();
           }
       }
    }
}
